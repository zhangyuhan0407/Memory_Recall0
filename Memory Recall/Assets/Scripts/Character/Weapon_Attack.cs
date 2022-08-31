using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Attack : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
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
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "enemy")
        {
            //Time.timeScale = NumberDecrease(1,0, Time.timeScale);
            //vcam.m_Lens.OrthographicSize = NumberDecrease(vcam.m_Lens.OrthographicSize, vcam.m_Lens.OrthographicSize - 5, vcam.m_Lens.OrthographicSize);
            Destroy(collision.gameObject);
            //Time.timeScale = NumberIncrease(0,1, Time.timeScale);
            //vcam.m_Lens.OrthographicSize = NumberIncrease(vcam.m_Lens.OrthographicSize, vcam.m_Lens.OrthographicSize + 5, vcam.m_Lens.OrthographicSize);
        }
    }

    float NumberIncrease(float init, float target, float now)
    {
        now += (target - init) * Time.deltaTime / 5;
        if (now >= target)
            now = target;
        return now;
    }

    float NumberDecrease(float init, float target, float now)
    {
        now -= (init - target) * Time.deltaTime / 5;
        if (now <= target)
            now = target;
        return now;
    }
}
