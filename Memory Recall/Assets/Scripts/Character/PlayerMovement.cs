using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    GrapplingGun gun;
    GrapplingGun1 gun1;
    public LayerMask ground;

    [Header("移动参数")]
    public float runSpeed = 8.0f;
    private Vector2 move;

    public float xVelocity;

    [Header("跳跃参数")]
    public float jumpForce = 6.3f;
    private float temp_jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;

    float jumpTime;

    [Header("状态")]
    public bool isOnGround;
    public bool isJump;
    public bool isAir;
    public bool isNotGrappling;

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
        if(GameController.isGameAlive == true)
        {
            PhysicsCheck();
            Flip();
            Movement();
            MidAirMovement();
        }
    }

   void Movement()
    {
        xVelocity = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            rb.velocity = new Vector2(xVelocity * runSpeed + rb.velocity.x , rb.velocity.y);
        else if(rb.IsTouchingLayers(ground)&& !Input.GetKey(KeyCode.Mouse0))
            rb.velocity = new Vector2(xVelocity * runSpeed  , rb.velocity.y);
/*        if(Input.GetKey(KeyCode.LeftControl))
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            rb.gravityScale = 5f;
        }*/

        /*         //flipdirection
                if(xVelocity<0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);*/
    }

    void Flip()
    {
        bool plyerHasXAxisSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (plyerHasXAxisSpeed)
        {
            if (rb.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (rb.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Run()
    {
        if (gun.canTouchFall == false)
        {
            float moveDir = Input.GetAxis("Horizontal");

            Vector2 playerVelocity = new Vector2(moveDir * runSpeed, rb.velocity.y);
            rb.velocity = playerVelocity;
        }
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
    //跳跃功能、冲刺功能
    void MidAirMovement()
    {
        if(isNotGrappling && (Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)))
        {
            if(Input.GetKeyDown(KeyCode.Space)&& !isJump)
            {
                isOnGround = false;
                isNotGrappling = false;
                isJump = true;
                jumpTime = Time.time + jumpHoldDuration;
                temp_jumpForce = 1.5f * jumpForce;
                rb.AddForce(new Vector2(0f, temp_jumpForce), ForceMode2D.Impulse);
                Debug.Log("99999999999999");
            }

        }
        else if (Input.GetKeyDown(KeyCode.Space ) && isOnGround && !isJump )
        {
            isOnGround = false;
            isJump = true;
            jumpTime = Time.time + jumpHoldDuration;

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        else if (isJump)
        {
            if (jumpTime < Time.time)
            {
                isJump = false;
                isNotGrappling = true;
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
