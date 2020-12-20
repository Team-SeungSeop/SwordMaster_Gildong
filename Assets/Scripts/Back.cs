using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{   
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;    
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateBack(int StageNum){
        if (StageNum == 1){
            spriteRenderer.sprite = sprites[0];
        }
        else if (StageNum == 2){
            spriteRenderer.sprite = sprites[1];
        }
        else if (StageNum == 3){
            spriteRenderer.sprite = sprites[2];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
