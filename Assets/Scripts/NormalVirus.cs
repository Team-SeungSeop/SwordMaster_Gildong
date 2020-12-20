using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalVirus : MonoBehaviour
{ 
    int x_v;
    int y_v;
    int component;
    int count = 0;
    int life;

    public Animator anim;

    //private SpriteRenderer spriteRenderer;
    //public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {  
        anim = GetComponent<Animator>();
        anim.SetBool("Dead", false);
        life = 1;

        x_v = Random.Range(4, 8);
        y_v = Random.Range(4, 8);

        component = Random.Range(0, 2);
        if (component == 1){
            x_v = -1*x_v;
        }
        component = Random.Range(0, 2);
        if (component == 1){
            y_v = -1*y_v;
        }        
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(x_v, y_v);
    }

    public void N_Dead(){
        this.GetComponent<AudioSource>().Play();
        //GameObject.Find("death").GetComponent<AudioSource> ().Play ();
        anim.SetBool("Dead", true);
        anim.SetTrigger("Hitted");

        life = 0;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -7);  
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        count++;

        if (this.transform.position.x <= -7.8){
            x_v = Random.Range(4, 8);
            y_v = Random.Range(4, 8);

            component = Random.Range(0, 2);
            if (component == 1){
                y_v = -1*y_v;
            }

            this.GetComponent<Rigidbody2D>().velocity = new Vector2(x_v, y_v);            
        }
        else if (this.transform.position.x >= 7.8){
            x_v = Random.Range(4, 8);
            y_v = Random.Range(4, 8);

            x_v = -1*x_v;

            component = Random.Range(0, 2);
            if (component == 1){
                y_v = -1*y_v;
            }
                    
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(x_v, y_v);
        }
        else if (this.transform.position.y >= 4){
            x_v = Random.Range(4, 8);
            y_v = Random.Range(4, 8);

            y_v = -1*y_v;

            component = Random.Range(0, 2);
            if (component == 1){
                x_v = -1*x_v;
            }
                    
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(x_v, y_v);           
        }
        else if (this.transform.position.y <= -4){

            if (life == 0){
                return;
            }

            x_v = Random.Range(4, 8);
            y_v = Random.Range(4, 8);

            component = Random.Range(0, 2);
            if (component == 1){
                x_v = -1*x_v;
            }
                    
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(x_v, y_v);             
        }
        else{
            return;
        }        
        
    }

    /*
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "left"){
            x_v = Random.Range(4, 8);
            y_v = Random.Range(4, 8);

            component = Random.Range(0, 2);
            if (component == 1){
                y_v = -1*y_v;
            }

            this.GetComponent<Rigidbody2D>().velocity = new Vector2(x_v, y_v);
        }
        else if (other.tag == "right"){
            x_v = Random.Range(4, 8);
            y_v = Random.Range(4, 8);

            x_v = -1*x_v;

            component = Random.Range(0, 2);
            if (component == 1){
                y_v = -1*y_v;
            }
                    
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(x_v, y_v);
        }
        else if (other.tag == "up"){
            x_v = Random.Range(4, 8);
            y_v = Random.Range(4, 8);

            y_v = -1*y_v;

            component = Random.Range(0, 2);
            if (component == 1){
                x_v = -1*x_v;
            }
                    
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(x_v, y_v);           
        }
        else if (other.tag == "down"){
            x_v = Random.Range(4, 8);
            y_v = Random.Range(4, 8);

            component = Random.Range(0, 2);
            if (component == 1){
                x_v = -1*x_v;
            }
                    
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(x_v, y_v);             
        }
        else{
            return;
        }
    }
    */
}
