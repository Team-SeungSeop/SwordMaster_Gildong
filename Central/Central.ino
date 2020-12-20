#include <ArduinoBLE.h>

void setup() {
  Serial.begin(9600);
  //while (!Serial);
  BLE.begin();
}

void loop() {
  BLEDevice peripheral;
  BLE.scanForUuid("19B10000-E8F2-537E-4F6C-D104768A1214");
  peripheral = BLE.available();
  
  if (peripheral){
    BLE.stopScan();
    getData(peripheral);
  }
  
}

void getData(BLEDevice peripheral){
  peripheral.connect();
  peripheral.discoverAttributes();
  BLECharacteristic gestCharacteristic = peripheral.characteristic("19B10000-E8F2-537E-4F6C-D104768A1214"); 
  gestCharacteristic.subscribe();
  while (peripheral.connected()){
    if (gestCharacteristic.valueUpdated()){
      byte gest;
      gestCharacteristic.readValue(gest);

      //write to serial or keydown
      Serial.write(gest);
      delay(20);
    }
  }
}
