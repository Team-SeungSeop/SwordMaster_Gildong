﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimChange2 : MonoBehaviour
{   
    public Animator anim;

    //private int count;
    // Start is called before the first frame update

    void Start()
    {   
        anim = GetComponent<Animator>();
        anim.SetBool("State", true);
    }

    public void Triangle(){
        anim.SetTrigger("Triangle");
    }        

    // Update is called once per frame
    void Update()
    {   
        /*
        if (count % 300 == 0){
            UpDown();
        }
        count++;
        */
    }
}
