using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance;

    public GameObject NormalVirus;
    public GameObject AdvancedVirus;
    private int count;
    private int N_count;
    private int A_count;
    private int ComboCount;
    private int ComboOnOff;
    private GameObject [] N_Virus = new GameObject[30];
    private GameObject [] A_Virus = new GameObject[30];

    private int fortest;
    Coroutine runningCoroutine = null;
    
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
    
    public Text myScore;
    private int total_score = 0;

    public Text myTime;
    private float left_time = 90;

    private int GameMode;
    public GameObject SN;

    private int [] AbleSlash = new int[3];

    // Start is called before the first frame update
    void Start()
    {   
        instance = this;

        count = 0;
        N_count = 0;
        A_count = 0;
        ComboCount = 1;
        ComboOnOff = 0;
        
        
        serial = new SerialPort(portNumber.ToString(), int.Parse(baudRate), Parity.None, 8, StopBits.One);
        //serial = new SerialPort("COM5", 9600);

        serial.Open();
        serial.DtrEnable = true; 
        serial.ReadTimeout = 1;
        

        SN = GameObject.Find("stagenum");
        GameMode = SN.GetComponent<NeverDieScript>().sn;
        Debug.Log(GameMode);


        if (GameMode == 1){
            GameObject.Find("SpiritSavior").GetComponent<AudioSource>().Play();
        }
        else if (GameMode == 2){
            GameObject.Find("DragonBall").GetComponent<AudioSource>().Play();
        }
        else if (GameMode == 3){
            GameObject.Find("Basket").GetComponent<AudioSource>().Play();
            myTime.text = "You Should Slash Faster!!!";
        }

        if (GameMode == 1 || GameMode == 3){
            myScore.text = "SCORE: " + total_score;
        }
        else if (GameMode == 2){
            for (int i=0; i<3; i++){
                AbleSlash[i] = 1;
            }
            myScore.text = "AVAILABLE: [" + AbleSlash[0] + "] [" + AbleSlash[1] + "] [" + AbleSlash[2] + "]";
        }

        GameObject.Find("Arkana").GetComponent<Back>().UpdateBack(GameMode);
    }

    void Add_Score(int num){
        total_score += num;
        if (GameMode == 1 || GameMode == 3){
            myScore.text = "SCORE: " + total_score;
        }
    }


    void Slash_LeftRight(){
        GameObject.Find("ParallelSlash").GetComponent<AnimChange>().LeftRight();
        GameObject.Find("ParallelSlash").GetComponent<AudioSource>().Play();
        

        for (int i=0; i<N_count; i++){
            if (N_Virus[i].transform.position.y <= 1.5 && N_Virus[i].transform.position.y >= -1.5){
                N_Virus[i].GetComponent<NormalVirus>().N_Dead();
                Add_Score(100);
            }
        }
        for (int i=0; i<A_count; i++){
            if (A_Virus[i].transform.position.y <= 1.5 && A_Virus[i].transform.position.y >= -1.5){
                A_Virus[i].GetComponent<AdvancedVirus>().A_Dead();
                Add_Score(150);
            }
        }
    }

    void Slash_Roll(){
        GameObject.Find("RollSlash").GetComponent<AnimChange4>().Roll();
        GameObject.Find("RollSlash").GetComponent<AudioSource>().Play();

        for (int i=0; i<N_count; i++){
            if (N_Virus[i].transform.position.x <= 4 && N_Virus[i].transform.position.x >= -4 && N_Virus[i].transform.position.y <= 4 && N_Virus[i].transform.position.y >= -4){
                N_Virus[i].GetComponent<NormalVirus>().N_Dead();
                Add_Score(100);
            }
        }
        for (int i=0; i<A_count; i++){
            if (A_Virus[i].transform.position.x <= 4 && A_Virus[i].transform.position.x >= -4 && A_Virus[i].transform.position.y <= 4 && A_Virus[i].transform.position.y >= -4){
                A_Virus[i].GetComponent<AdvancedVirus>().A_Dead();
                Add_Score(150);
            }
        }        
    }

    void Slash_Triangle(){
        GameObject.Find("TriangleSlash").GetComponent<AnimChange2>().Triangle();
        GameObject.Find("TriangleSlash").GetComponent<AudioSource>().Play();

        for (int i=0; i<N_count; i++){
            if (N_Virus[i].transform.position.x <= 3 && N_Virus[i].transform.position.x >= -3){
                N_Virus[i].GetComponent<NormalVirus>().N_Dead();
                Add_Score(100);
            }
        }
        for (int i=0; i<A_count; i++){
            if (A_Virus[i].transform.position.x <= 3 && A_Virus[i].transform.position.x >= -3){
                A_Virus[i].GetComponent<AdvancedVirus>().A_Dead();
                Add_Score(150);
            }
        }            
    }

    IEnumerator WaitEffect(){
        yield return new WaitForSecondsRealtime(1.8f);

        this.GetComponent<AudioSource>().Play();

        yield return new WaitForSecondsRealtime(1.1f);

        for (int i=0; i<N_count; i++){
            N_Virus[i].GetComponent<NormalVirus>().N_Dead();
            Add_Score(100);
        }
        for (int i=0; i<A_count; i++){
            A_Virus[i].GetComponent<AdvancedVirus>().A_Dead();
            Add_Score(150);
        }   
    }

    void Slash_Special(){
        GameObject.Find("Special").GetComponent<AnimChange3>().Special();
        GameObject.Find("ultimate_1").GetComponent<AudioSource>().Play();
        GameObject.Find("ultimate_2").GetComponent<AudioSource>().Play();

        if (runningCoroutine != null){
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(WaitEffect());
    }

    void Check_Dead(){
        for (int i=0; i<N_count; i++){
            if (N_Virus[i].transform.position.y <= -6){
                Destroy(N_Virus[i]);
                for (int j=i; j<N_count-1; j++){
                    N_Virus[j] = N_Virus[j+1];
                }
                N_count--;
                i--;
            }
        }
        for (int i=0; i<A_count; i++){
            if (A_Virus[i].transform.position.y <= -6){
                Destroy(A_Virus[i]);
                for (int j=i; j<A_count-1; j++){
                    A_Virus[j] = A_Virus[j+1];
                }
                A_count--;
                i--;
            }
        }        
    }

    void BasicSlash(int Number){
        
        if (Number == 1){
            if (GameMode == 2 && AbleSlash[0] == 1){
                Slash_LeftRight();
                AbleSlash[0] = 0;
                myScore.text = "AVAILABLE: [" + AbleSlash[0] + "] [" + AbleSlash[1] + "] [" + AbleSlash[2] + "]";
            }
            else if (GameMode == 2 && AbleSlash[0] == 0){
                GameObject.Find("error").GetComponent<AudioSource>().Play();
            }
            else{
                Slash_LeftRight();
            }

            if (ComboOnOff == 0){
                ComboCount++;
            }
        }
        else if (Number == 2){
            if (GameMode == 2 && AbleSlash[1] == 1){
                Slash_Triangle();
                AbleSlash[1] = 0;
                myScore.text = "AVAILABLE: [" + AbleSlash[0] + "] [" + AbleSlash[1] + "] [" + AbleSlash[2] + "]";
            }
            else if (GameMode == 2 && AbleSlash[1] == 0){
                GameObject.Find("error").GetComponent<AudioSource>().Play();
            }
            else{
                Slash_Triangle();
            }
            if (ComboOnOff == 0){
                ComboCount++;
            }
        }
        else if (Number == 3){
            if (GameMode == 2 && AbleSlash[2] == 1){
                Slash_Roll();
                AbleSlash[2] = 0;
                myScore.text = "AVAILABLE: [" + AbleSlash[0] + "] [" + AbleSlash[1] + "] [" + AbleSlash[2] + "]";
            }
            else if (GameMode == 2 && AbleSlash[2] == 0){
                GameObject.Find("error").GetComponent<AudioSource>().Play();
            }
            else{
                Slash_Roll();
            }
            if (ComboOnOff == 0){
                ComboCount++;
            }
        }
        else{
            GameObject.Find("error").GetComponent<AudioSource>().Play();
            Debug.Log("Received Number Error: "+Number);
        }
    }

    void ComboSlash(int Number){
        if (GameObject.Find("CommandBox1").GetComponent<CommandBox>().CommandOnOff == 0){

            GameObject.Find("CommandBox1").GetComponent<CommandBox>().StartCommand();
            GameObject.Find("CommandBox2").GetComponent<CommandBox>().StartCommand();
            GameObject.Find("CommandBox3").GetComponent<CommandBox>().StartCommand();

        }
        else if (GameObject.Find("CommandBox1").GetComponent<CommandBox>().CommandOnOff == 1){
            if (GameObject.Find("CommandBox1").GetComponent<CommandBox>().CommandOX == 0){
                if (GameObject.Find("CommandBox1").GetComponent<CommandBox>().CommandNum == Number){
                    GameObject.Find("CommandBox1").GetComponent<CommandBox>().DoCommand();
                }
            }
            else if (GameObject.Find("CommandBox2").GetComponent<CommandBox>().CommandOX == 0){
                if (GameObject.Find("CommandBox2").GetComponent<CommandBox>().CommandNum == Number){
                    GameObject.Find("CommandBox2").GetComponent<CommandBox>().DoCommand();
                }
            }
            else if (GameObject.Find("CommandBox3").GetComponent<CommandBox>().CommandOX == 0){
                if (GameObject.Find("CommandBox3").GetComponent<CommandBox>().CommandNum == Number){
                    GameObject.Find("CommandBox3").GetComponent<CommandBox>().DoCommand();
                    Slash_Special();
                    ComboOnOff = 0;
                    GameObject.Find("CommandBox1").GetComponent<CommandBox>().FinishCommand();
                    GameObject.Find("CommandBox2").GetComponent<CommandBox>().FinishCommand();
                    GameObject.Find("CommandBox3").GetComponent<CommandBox>().FinishCommand();

                }
            }
        }
    }


    void GameEnd(){
        serial.Close();
        if (GameMode == 1 || GameMode == 3){
            GameObject.Find("stagenum").GetComponent<NeverDieScript>().tmp = total_score;
        }
        else if (GameMode == 2){
            GameObject.Find("stagenum").GetComponent<NeverDieScript>().tmp = N_count+A_count;
        }
        SceneManager.LoadScene("GameOverScene");
    }

    IEnumerator WaitThreeSec(){
        yield return new WaitForSecondsRealtime(3.0f);

        GameEnd();
    }    

    // Update is called once per frame
    void Update(){
        if (GameMode == 1){
            left_time -= Time.deltaTime;
            if (left_time < 0){
                left_time = 0;
            }
            myTime.text = string.Format("LEFT TIME: {0:N2}s", left_time);
            if (left_time <= 0){
                GameObject.Find("stagenum").GetComponent<NeverDieScript>().tmp2 = 1;
                GameEnd();
            }            
        }
        
        if (GameMode == 1){
            if (count % 75 == 0){
                if (N_count == 30){
                    GameObject.Find("stagenum").GetComponent<NeverDieScript>().tmp2 = 0;
                    GameEnd();
                }
                GameObject N_curVirus = Instantiate(NormalVirus, new Vector3(0, 0, 0), Quaternion.identity);
                N_Virus[N_count] = N_curVirus;
                N_count++;
            }
            if (count % 100 == 0){
                if (A_count == 20){
                    GameObject.Find("stagenum").GetComponent<NeverDieScript>().tmp2 = 0;
                    GameEnd();
                }
                GameObject A_curVirus = Instantiate(AdvancedVirus, new Vector3(0, 0, 0), Quaternion.identity);
                A_Virus[A_count] = A_curVirus;
                A_count++;
            }
        }
        else if (GameMode == 2){
            if (count == 0){
                for (int i=0; i<2; i++){
                    GameObject N_curVirus = Instantiate(NormalVirus, new Vector3(0, 0, 0), Quaternion.identity);
                    N_Virus[N_count] = N_curVirus;
                    N_count++;
                }
                
                for (int i=0; i<1; i++){
                    GameObject A_curVirus = Instantiate(AdvancedVirus, new Vector3(0, 0, 0), Quaternion.identity);
                    A_Virus[A_count] = A_curVirus;
                    A_count++;
                }
            }
            myTime.text = string.Format("LEFT MONSTER: {0}", N_count+A_count);
            if (AbleSlash[0] == 0 && AbleSlash[1] == 0 && AbleSlash[2] == 0){
                StartCoroutine(WaitThreeSec());
            }
            else if (N_count+A_count == 0){
                StartCoroutine(WaitThreeSec());
            }
        }
        else if (GameMode == 3){
            if (count % 37 == 0){
                if (N_count == 30){
                    GameEnd();
                }
                GameObject N_curVirus = Instantiate(NormalVirus, new Vector3(0, 0, 0), Quaternion.identity);
                N_Virus[N_count] = N_curVirus;
                N_count++;
            }
            if (count % 50 == 0){
                if (A_count == 20){
                    GameEnd();
                }
                GameObject A_curVirus = Instantiate(AdvancedVirus, new Vector3(0, 0, 0), Quaternion.identity);
                A_Virus[A_count] = A_curVirus;
                A_count++;
            }
        }

        Check_Dead();
        count++;
    }

    void FixedUpdate()
    {   
        /*
        if (count % 100 == 0){
            BasicSlash(1);
        }
        */
        
        if (Input.GetKey(KeyCode.DownArrow)){
            fortest = 2;
            if (ComboCount%20 == 0){
                ComboOnOff = 1;
                ComboSlash(fortest);
            }
            BasicSlash(fortest);            
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            fortest = 3;
            if (ComboCount%20 == 0){
                ComboOnOff = 1;
                ComboSlash(fortest);
            }
            BasicSlash(fortest);     
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            fortest = 1;
            if (ComboCount%20 == 0){
                ComboOnOff = 1;
                ComboSlash(fortest);
            }
            BasicSlash(fortest);     
        }
        
        //Debug.Log(ComboCount);
        
        if (serial.IsOpen){
            try{
                int tmp = serial.ReadByte();
                Debug.Log(tmp);
                if (ComboCount%20 == 0){
                    ComboOnOff = 1;
                    ComboSlash(tmp);
                }
                BasicSlash(tmp);
                
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