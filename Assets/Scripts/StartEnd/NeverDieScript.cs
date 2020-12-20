using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NeverDieScript : MonoBehaviour
{   
    public int sn = 0;
    public int tmp = -1;
    public int tmp2 = -1;
    public GameObject stageNumObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void call(){
        if (sn == 0){
            Debug.Log("Error");
            return;
        }
        SceneManager.LoadScene("BasicScene");
        DontDestroyOnLoad(stageNumObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
