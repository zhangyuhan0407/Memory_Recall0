using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int Blinks;
    public float time;
    public float dieTime;

    private Renderer myRender;
    private Animator anim;
    private Rigidbody2D rb2d;
    private ScreenFlash flash;
    // Start is called before the first frame update
    void Start()
    {
        myRender = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        flash = GetComponent<ScreenFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(int damage)
    {
        flash.FlashScreen();
        health -= damage;
        //Debug.Log(health);
        if(health<=0)
        {
            health = 0;
        }
        if (health <= 0)
        {
            rb2d.velocity = new Vector2(0, 0);
            //rb2d.gravityScale = 0.0f;
            GameController.isGameAlive = false;
            anim.SetTrigger("Die");
            Invoke("KillPlayer", dieTime);
            health = 5;
        }
        BlinkPlayer(Blinks, time);
        GameController.isGameAlive = true;
        
    }

    void KillPlayer()
    {
        rb2d.position = Vector2.zero;
    }
    void BlinkPlayer(int numBlinks,float seconds)
    {
        StartCoroutine(DoBlinks(numBlinks, seconds));
    }

    IEnumerator DoBlinks(int  numBlinks,float seconds)
    {
        for(int i =0;i<numBlinks*2;i++)
        {
            myRender.enabled = !myRender.enabled;
            yield return new WaitForSeconds(seconds);
        }
        myRender.enabled = true;
    }
}
