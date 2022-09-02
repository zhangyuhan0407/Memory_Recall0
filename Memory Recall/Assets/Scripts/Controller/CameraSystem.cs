using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{

    private CinemachineVirtualCamera vcam;
    public GrapplingGun gun;
    public GrapplingGun1 gun1;
    public PlayerMovement playerMove;
    public GameObject player;

    float totalTime = 3;
    float currentUsedTime;

    [SerializeField]
    AnimationCurve curve;
    private float height;
    private float originPos;
    private float updatePos;
    private float originSize;
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindGameObjectWithTag("player");
        originPos = updatePos = player.transform.position.y;
        originSize = vcam.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        currentUsedTime += Time.deltaTime;
        float t = currentUsedTime / totalTime;

        if(gun.canTouchFall || gun1.canTouchFall)
        {
            if (player.transform.position.y > updatePos + 0.2f)
            {
                vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, vcam.m_Lens.OrthographicSize + 10f * Mathf.Abs(player.transform.position.y - updatePos), Time.deltaTime);
                updatePos = player.transform.position.y;
            }
            else if (player.transform.position.y < updatePos - 3f)
            {
                vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, vcam.m_Lens.OrthographicSize - 6f * Mathf.Abs(player.transform.position.y - updatePos), Time.deltaTime);
                updatePos = player.transform.position.y;
            }

            /*            if (player.transform.position.y > updatePos + 40f)
                        {
                            vcam.m_Lens.OrthographicSize = Mathf.Lerp(updatePos, 30f, curve.Evaluate(t));
                            updatePos = player.transform.position.y;
                        }
                        else if (player.transform.position.y > updatePos + 30f)
                        {
                            vcam.m_Lens.OrthographicSize = Mathf.Lerp(updatePos, 25f, curve.Evaluate(t));
                            updatePos = player.transform.position.y;
                        }
                        else if (player.transform.position.y > updatePos + 20f)
                        {
                            vcam.m_Lens.OrthographicSize = Mathf.Lerp(updatePos, 20f, curve.Evaluate(t));
                            updatePos = player.transform.position.y;
                        }
                        else if (player.transform.position.y > updatePos + 10f)
                        {
                            vcam.m_Lens.OrthographicSize = Mathf.Lerp(updatePos, 15f, curve.Evaluate(t));
                            updatePos = player.transform.position.y;
                        }*/
        }
        if (playerMove.isOnGround && originPos+10f >= player.transform.position.y)
            if(vcam.m_Lens.OrthographicSize != originSize)
                vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize,originSize, Time.deltaTime);
    }
}
