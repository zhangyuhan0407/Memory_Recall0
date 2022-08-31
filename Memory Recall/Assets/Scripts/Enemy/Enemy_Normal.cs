using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Normal : Enemy
{
    public float attackScale;
    public float speed;
    public float distance;
    public float startWaitTime;
    private float waitTime;

    public Transform[] moveSpots;
    private Transform playerTransform;
    public float radius;

    [Header("»·¾³¼à²â")]
    public float armOffset = 0.4f;
    public float heightOffest = -0.36f;
    public float _distance = 4f;

    public LayerMask playerLayer;
    public GameObject attack;

    private int i = 0;
    private bool movingRight = true;

    private bool canAttack = false;
    private RaycastHit2D leftcheck;
    private RaycastHit2D rightcheck;

    new
        // Start is called before the first frame update
        void Start()
    {
        base.Start();
        playerTransform = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        waitTime = startWaitTime;
    }

    new

        // Update is called once per frame
        void Update()
    {
        base.Update();
        //playerTransform = GameObject.Find("player").GetComponent<Transform>();

        //distance = (playerTransform.position - transform.position).magnitude;
        PhysicsCheck();
        Flip();

        if (playerTransform != null)
        {
            distance = Mathf.Abs(transform.position.x - playerTransform.position.x);

            if(distance<=attackScale)
            {
                canAttack = true;
                attack.SetActive(true);
            }
            else if (leftcheck)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x + attackScale, transform.position.y), speed * Time.deltaTime);
                canAttack = true;
                attack.SetActive(false);

            }
            else if (rightcheck)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x - attackScale, transform.position.y), speed * Time.deltaTime);
                canAttack = true;
                attack.SetActive(false);

            }
            else
            {
                attack.SetActive(false);
                canAttack = false;

            }
        }
    }

    void Flip()
    {
        if (!canAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[i].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, moveSpots[i].position) < 0.1f)
            {
                if (waitTime <= 0)
                {
                    if (movingRight == true)
                    {
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        movingRight = false;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        movingRight = true;
                    }

                    if (i == 0)
                    {

                        i = 1;
                    }
                    else
                    {

                        i = 0;
                    }

                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }

    void PhysicsCheck()
    {
        leftcheck = Raycast(new Vector2(-armOffset, heightOffest), Vector2.left, radius, playerLayer);
        rightcheck = Raycast(new Vector2(armOffset, heightOffest), Vector2.right, radius, playerLayer);


/*        if (leftcheck || rightcheck)
            canAttack = true;
        else
            canAttack = false;*/
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 raydiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, raydiraction, length, layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, raydiraction, color);

        return hit;
    }
}


