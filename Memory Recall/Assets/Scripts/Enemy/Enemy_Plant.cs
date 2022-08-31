using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    public Transform player_transform;
    //make sure bullte velocity in the real time
    private float distance;

    [Header("Range Attack")]
    public  GameObject bullet;
    public float attack_range;
    public float cd_range;

    [Header("Melee Attack")]
    public GameObject poison;
    public float attack_melee;
    public float cd_melee;
    private float duration;
    [Tooltip("melee attack duration time")]
    public float offsetTime;

    new
        // Start is called before the first frame update
        void Start()
    {
        base.Start();
        player_transform = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        //poison = GameObject.FindGameObjectWithTag("poison");
        poison.SetActive(false);
        duration = Time.time + offsetTime;
        //attack_melee = 1f;
        //attack_range = 20f;
        //cd_range = 3f;
    }

    new

        // Update is called once per frame
        void Update()
    {
        player_transform = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        distance = (player_transform.position - transform.position).magnitude;
        cd_range -= 1f;
        if (duration < Time.time&&poison.activeSelf==true)
        {
            poison.SetActive(false);
            duration = Time.time + offsetTime;
        }
        if (distance<= attack_melee)
        {
            Invoke("MeleeAttack", cd_melee);
        }
        else if((distance < attack_range))
        {
            if(cd_range<0)
            {
                RangeAttack();
                cd_range = 250f;
            }
        }
    }

    void MeleeAttack()
    {
        if (poison.activeSelf == false)
        {
            poison.SetActive(true);
        }
    }

    void RangeAttack()
    {
        Instantiate(bullet);
        bullet.transform.position = transform.position;
    }

}
