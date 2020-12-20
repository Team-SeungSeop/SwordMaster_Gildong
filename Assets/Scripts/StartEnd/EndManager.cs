using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public enum PortNumber{
        COM1, COM2, COM3, COM4,
        COM5, COM6, COM7, COM8,
        COM9, COM10, COM11, COM12,
        COM13, COM14, COM15, COM16
    }
    private SerialPort serial;

    [SerializeField]
    private PortNumber portNumber = PortNumber.COM5;
    [SerializeField]
    private string baudRate = "9600";

    private int StageNum = 0;
    public NeverDieScript nd;

    public Text Head;
    public Text InfoT;
    public Text InfoN;

    public GameObject SN;

    // Start is called before the first frame update
    void Start()
    {   
        
        serial = new SerialPort(portNumber.ToString(), int.Parse(baudRate), Parity.None, 8, StopBits.One);
        //serial = new SerialPort("COM5", 9600);

        serial.Open();
        serial.DtrEnable = true;
        serial.ReadTimeout = 1;
        

        SN = GameObject.Find("stagenum");
        StageNum = SN.GetComponent<NeverDieScript>().sn;

        if (StageNum == 1){
            Head.text = "Game Over!";
            InfoN.text = string.Format("SCORE: {0}", SN.GetComponent<NeverDieScript>().tmp);

            if (SN.GetComponent<NeverDieScript>().tmp2 == 0){
                InfoT.text = "Too Many Monsters!";
            }
            else if (SN.GetComponent<NeverDieScript>().tmp2 == 1){
                InfoT.text = "Time Finished!";
            }
        }
        else if (StageNum == 2){
            if (SN.GetComponent<NeverDieScript>().tmp == 0){
                Head.text = "Game Clear!";
                InfoT.text = "Congratulations!";
                InfoN.text = "LEFT MONSTERS: 0";
            }
            else {
                Head.text = "Game Over!";
                InfoT.text = "You Used All Types of Slashes!";
                InfoN.text = string.Format("LEFT MONSTERS: {0}", SN.GetComponent<NeverDieScript>().tmp);
            }
        }
        else if (StageNum == 3){
            Head.text = "Game Over!";
            InfoT.text = "Too Many Monsters!";
            InfoN.text = string.Format("SCORE: {0}", SN.GetComponent<NeverDieScript>().tmp);
        }
    }

    void Restart(){
        Destroy(GameObject.Find("stagenum"));
        SceneManager.LoadScene("StartScreen");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow)){
            serial.Close();
            Restart();
        }
        
        
        if (serial.IsOpen){
            try{
                int tmp = serial.ReadByte();
                Debug.Log(tmp);
                serial.Close();
                Restart(); 
            }
            catch(System.TimeoutException e){
                Debug.Log(e);
                throw;
            }
        }
        else if (!serial.IsOpen){
            serial.Open();
        }
              
        
    }
}
