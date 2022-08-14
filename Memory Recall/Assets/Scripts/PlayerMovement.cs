using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public LayerMask ground;

    [Header("移动参数")]
    public float speed =8.0f;

    public float xVelocity;

    [Header("跳跃参数")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;

    float jumpTime;

    [Header("状态")]
    public bool isOnGround;
    public bool isJump;
    public bool isAir;

    [Header("环境监测")]
    public float footOffset = 0.4f;
    public float heightOffest = -0.36f;
    public float groundDistance = 0.2f;

    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        PhysicsCheck();
        Movement();
        MidAirMovement();
    }

    void Movement()
    {
        xVelocity = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Mouse0))
            rb.velocity = rb.velocity;
        else if(rb.IsTouchingLayers(ground)&& !Input.GetKey(KeyCode.Mouse0))
            rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

        //flipdirection
/*        if(xVelocity<0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);*/
    }

    void PhysicsCheck()
    {
        RaycastHit2D leftcheck = Raycast(new Vector2(-footOffset, heightOffest), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightcheck = Raycast(new Vector2(footOffset, heightOffest), Vector2.down, groundDistance, groundLayer);


        if (leftcheck || rightcheck)
            isOnGround = true;
        else
            isOnGround = false;
    }
    
    void MidAirMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space ) && ((isOnGround && !isJump ) || isAir))
        {
            isOnGround = false;
            isJump = true;
            if(isAir)
                jumpForce = 80;
            isAir = false;
            jumpTime = Time.time + jumpHoldDuration;

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        else if (isJump)
        {
            if (jumpTime < Time.time)
            {
                isJump = false;
                jumpForce = 20;
            }
        }
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 raydiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, raydiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, raydiraction, color);

        return hit;
    }

    public void checkAir(bool check)
    {
        isAir = check;
    }
}
