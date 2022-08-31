using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantWelee : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player") && collision.GetType().ToString() == "UnityEngine.CircleCollider2D")
        {
            if (playerHealth != null)
                playerHealth.DamagePlayer(damage);
        }
    }
}
