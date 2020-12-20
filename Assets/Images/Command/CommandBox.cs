using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBox : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public int CommandOnOff = 0; //On이 1, Off가 0
    public int CommandNum = -1; //1~3
    public int CommandOX = -1; //X가 0, O가 1로
    // Start is called before the first frame update
    void Start()
    {   
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    public void StartCommand(){
        int tmp = Random.Range(1, 4);
        CommandNum = tmp;
        spriteRenderer.sprite = sprites[tmp];
        CommandOnOff = 1;
        CommandOX = 0;
    }

    public void DoCommand(){
        if (CommandNum == 1){
            spriteRenderer.sprite = sprites[4];
            CommandOX = 1;
        }
        else if (CommandNum == 2){
            spriteRenderer.sprite = sprites[5];
            CommandOX = 1;
        }
        else if (CommandNum == 3){
            spriteRenderer.sprite = sprites[6];
            CommandOX = 1;
        }
        else {
            Debug.Log("DoCommand Error");
        }
    }

    public void FinishCommand(){
        CommandNum = -1;
        CommandOX = -1;
        CommandOnOff = 0;
        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
