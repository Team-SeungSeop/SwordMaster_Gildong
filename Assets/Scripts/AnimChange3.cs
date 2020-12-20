using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimChange3 : MonoBehaviour
{
    public Animator anim;    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("State", true);        
    }

    public void Special(){
        anim.SetTrigger("Special");
    }        

    // Update is called once per frame
    void Update()
    {
        
    }
}
