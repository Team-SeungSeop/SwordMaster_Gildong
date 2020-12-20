using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimChange : MonoBehaviour
{   
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {   
        anim = GetComponent<Animator>();
        anim.SetBool("State", true);
    }
    
    public void LeftRight(){
        anim.SetTrigger("LeftRight");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
