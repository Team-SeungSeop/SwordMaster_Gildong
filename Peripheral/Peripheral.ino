#include <Arduino_LSM9DS1.h>
#include <ArduinoBLE.h>
#include <TensorFlowLite.h>
#include <tensorflow/lite/micro/all_ops_resolver.h>
#include <tensorflow/lite/micro/micro_error_reporter.h>
#include <tensorflow/lite/micro/micro_interpreter.h>
#include <tensorflow/lite/schema/schema_generated.h>
#include <tensorflow/lite/version.h>

#include "model.h"

const int LR = 1;
const int HURR = 2;
const int CIRCLE = 3;

const int RLED = 10;
const int GLED = 11;
const int BLED = 12;

const float accelerationThreshold = 3.0; // threshold of significant in G's
const int numSamples = 79;
int samplesRead = numSamples;

// global variables used for TensorFlow Lite (Micro)
tflite::MicroErrorReporter tflErrorReporter;

// pull in all the TFLM ops, you can remove this line and
// only pull in the TFLM ops you need, if would like to reduce
// the compiled size of the sketch.
tflite::AllOpsResolver tflOpsResolver;

const tflite::Model* tflModel = nullptr;
tflite::MicroInterpreter* tflInterpreter = nullptr;
TfLiteTensor* tflInputTensor = nullptr;
TfLiteTensor* tflOutputTensor = nullptr;

// Create a static memory buffer for TFLM, the size may need to
// be adjusted based on the model you are using
constexpr int tensorArenaSize = 8 * 1024;
byte tensorArena[tensorArenaSize];

// array to map gesture index to a name
const char* GESTURES[] = {
  "LR",
  "RL",
  "UD"
};

#define NUM_GESTURES (sizeof(GESTURES) / sizeof(GESTURES[0]))

BLEService* sword = nullptr;

BLEByteCharacteristic* gestCharacteristic = nullptr;


void setup() {
  pinMode(RLED, OUTPUT);
  pinMode(GLED, OUTPUT);
  pinMode(BLED, OUTPUT);
  analogWrite(RLED, 0);
  analogWrite(GLED, 0);
  analogWrite(BLED, 0);
  sword = new BLEService("19B10000-E8F2-537E-4F6C-D104768A1214");
  gestCharacteristic = new BLEByteCharacteristic("19B10000-E8F2-537E-4F6C-D104768A1214", BLERead | BLENotify);
  Serial.begin(9600);
  //while (!Serial);
  IMU.begin();
  BLE.begin();
  BLE.setLocalName("Sword");
  BLE.setAdvertisedService(*sword);
  sword->addCharacteristic(*gestCharacteristic);
  BLE.addService(*sword);
  gestCharacteristic->writeValue(0);
  BLE.advertise();
  Serial.println("waiting for connection");
  
  tflModel = tflite::GetModel(model);
  if (tflModel->version() != TFLITE_SCHEMA_VERSION) {
    Serial.println("Model schema mismatch!");
    while (1);
  }

  // Create an interpreter to run the model
  tflInterpreter = new tflite::MicroInterpreter(tflModel, tflOpsResolver, tensorArena, tensorArenaSize, &tflErrorReporter);

  // Allocate memory for the model's input and output tensors
  tflInterpreter->AllocateTensors();

  // Get pointers for the model's input and output tensors
  tflInputTensor = tflInterpreter->input(0);
  tflOutputTensor = tflInterpreter->output(0);
  
}

void loop() {
  float aX, aY, aZ, gX, gY, gZ;
  BLEDevice central = BLE.central();
  if (central){
    Serial.println("connected to central");
    Serial.println(central.address());

    while (central.connected()){
      if (gestCharacteristic->subscribed()){
          while (samplesRead == numSamples){
            if (IMU.accelerationAvailable()){
              IMU.readAcceleration(aX, aY, aZ);
              //float aSum = fabs(aX) + fabs(aY) + fabs(aZ);
              if (aY >= accelerationThreshold){
                samplesRead = 0;
                break;
              }
            }
          }

          while (samplesRead < numSamples){
            if (IMU.accelerationAvailable() && IMU.gyroscopeAvailable()){
              IMU.readAcceleration(aX, aY, aZ);
              IMU.readGyroscope(gX, gY, gZ);

              //normalize the IMU data 0 ~ 1
              tflInputTensor->data.f[samplesRead * 6 + 0] = (aX + 4.0) / 8.0;
              tflInputTensor->data.f[samplesRead * 6 + 1] = (aY + 4.0) / 8.0;
              tflInputTensor->data.f[samplesRead * 6 + 2] = (aZ + 4.0) / 8.0;
              tflInputTensor->data.f[samplesRead * 6 + 3] = (gX + 2000.0) / 4000.0;
              tflInputTensor->data.f[samplesRead * 6 + 4] = (gY + 2000.0) / 4000.0;
              tflInputTensor->data.f[samplesRead * 6 + 5] = (gZ + 2000.0) / 4000.0;

              samplesRead++;

              if (samplesRead == numSamples){
                TfLiteStatus invokeStatus = tflInterpreter->Invoke();
                if (invokeStatus != kTfLiteOk){
                  Serial.println("Failed to invoke");
                  while(1);
                  return;
                }

                byte gestNum = 0;
                for (int i = 0; i < NUM_GESTURES; i++){
                  if (tflOutputTensor->data.f[i] > 0.7){
                    gestNum = i + 1;
                    break;
                  }
                }
                if (gestNum == LR){
                  analogWrite(RLED, 255);
                  analogWrite(GLED, 255);
                  analogWrite(BLED, 0);
                }
                else if (gestNum == HURR){
                  analogWrite(RLED, 100);
                  analogWrite(GLED, 255);
                  analogWrite(BLED, 0);
                }
                else if (gestNum == CIRCLE){
                  analogWrite(RLED, 0);
                  analogWrite(GLED, 100);
                  analogWrite(BLED, 255);
                }
                else if (gestNum == 0){
                  analogWrite(RLED, 0);
                  analogWrite(GLED, 255);
                  analogWrite(BLED, 255);
                }
                gestCharacteristic->writeValue((byte)gestNum);
                Serial.print(gestNum);
                Serial.println("     Write Complete");
              }
            }
          }
      }
    }
  }
}
