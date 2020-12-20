using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class StartManager : MonoBehaviour
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

    private int StageNum = 1;
    public SpriteRenderer menu1;
    public SpriteRenderer menu2;
    public SpriteRenderer menu3;
    public SpriteRenderer menu4;

    public NeverDieScript nd;

    // Start is called before the first frame update
    void Start()
    {   
        
        serial = new SerialPort(portNumber.ToString(), int.Parse(baudRate), Parity.None, 8, StopBits.One);
        //serial = new SerialPort("COM5", 9600);

        serial.Open();
        serial.DtrEnable = true;
        serial.ReadTimeout = 1;
        
        
    }

    void Next_Select(){
        if (StageNum == 1){
            menu2.color = new Color(0, 0.5f, 1, 0.7f);
            menu1.color = new Color(1, 1, 1, 0.7f);
            StageNum = 2;
        }
        else if (StageNum == 2){
            menu3.color = new Color(0, 0.5f, 1, 0.7f);
            menu2.color = new Color(1, 1, 1, 0.7f);
            StageNum = 3;

        }
        else if (StageNum == 3){
            menu4.color = new Color(0, 0.5f, 1, 0.7f);
            menu3.color = new Color(1, 1, 1, 0.7f);
            StageNum = 4;
        }
        else if (StageNum == 4){
            menu1.color = new Color(0, 0.5f, 1, 0.7f);
            menu4.color = new Color(1, 1, 1, 0.7f);
            StageNum = 1;
        }
    }

    void Start_Game(){
        nd.sn = StageNum;
        nd.call();
    }


    void FixedUpdate()
    {   

        if (Input.GetKey(KeyCode.LeftArrow)){
            serial.Close();
            Start_Game();
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            Next_Select();
        }
        
        if (serial.IsOpen){
            try{
                int tmp = serial.ReadByte();
                Debug.Log(tmp);
                if (tmp == 1){
                    Next_Select();
                }
                else if (tmp == 3){
                    serial.Close();
                    Start_Game();
                }
                
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
