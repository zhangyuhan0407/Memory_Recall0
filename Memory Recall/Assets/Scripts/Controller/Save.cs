using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetFloat("x", collision.transform.position.x);
        PlayerPrefs.SetFloat("y", collision.transform.position.y);
        PlayerPrefs.SetFloat("z", collision.transform.position.z);
    }
}
