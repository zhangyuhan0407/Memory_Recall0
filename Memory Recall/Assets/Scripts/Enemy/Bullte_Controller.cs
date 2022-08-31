using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullte_Controller : MonoBehaviour
{
    public Rigidbody2D rb_player;
    public float speed;
    public int damage;


    private float duration;
    [Tooltip("Bullet life time")]
    public float lifeTime;
    [Tooltip("Bullet speed modification time difference (delay)")]
    public float offsetTime;

    public Rigidbody2D rb;
    private PlayerHealth playerHealth;

    private Vector2 bulltePos;
    private Vector2 playerPos;

    public float cd_Trace;
    // Start is called before the first frame update
    void Start()
    {
        rb_player = GameObject.FindGameObjectWithTag("player").GetComponent<Rigidbody2D>();
        playerHealth = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        duration = Time.time + lifeTime + offsetTime;
        cd_Trace = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        cd_Trace -= 0.02f;
        if(cd_Trace<0)
        {
            TracePlayer();
            cd_Trace = 1f;
        }
/*        if (rb != null)
        {
            Invoke("TracePlayer", offsetTime);
        }*/
        if(duration < Time.time)
            Destroy(gameObject);
    }
    void TracePlayer()
    {
        bulltePos = (Vector2)transform.position;
        playerPos = rb_player.position;
        Vector2 _temp = (playerPos - bulltePos).normalized;
        rb.velocity = new Vector2(_temp.x * speed, _temp.y * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="player" || collision.tag == "wall")
        {
            if (collision.gameObject.CompareTag("player") && collision.GetType().ToString() == "UnityEngine.CircleCollider2D")
            {
                if (playerHealth != null)
                    playerHealth.DamagePlayer(damage);
            }
            Debug.Log("11");
            Destroy(gameObject);
        }
        
    }
}
