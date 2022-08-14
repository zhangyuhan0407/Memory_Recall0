using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope rope;
    public GrapplingRope1 rope1;
    public GrapplingGun grapplingGun;
    public GrapplingGun1 grapplingGun1;
    public PlayerMovement player;

    [Header("Controller:")]
    public Rigidbody2D controller;
    public float force;
    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
   // public Transform gunHolder0;
    public Transform gunPivot;
   // public Transform gunPivot0;
    public Transform firePoint;
   // public Transform firePoint0;

    [Header("Physics Ref:")]
    public DistanceJoint2D m_distanceJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;

    private bool check = true;
    private bool checkIsAir = true;
    private bool castToCollider = false;

    [SerializeField] private Vector3 cameraPositionNow;

private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grapplePoint0;
    [HideInInspector] public Vector2 grappleDistanceVector;
    [HideInInspector] public Vector2 grappleDistanceVector0;

    [HideInInspector] public Vector2 distanceVector = new Vector2(0, 0);
    [HideInInspector] public Vector2 distanceVector0;
    [HideInInspector] public Vector2 finalV;

    private void Start()
    {
        rope.enabled = false;
        //grappleRope0.enabled = false;
        m_distanceJoint2D.enabled = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (rope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                    Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                    RotateGun(mousePos, true);
            }
            //Ŀǰ�����޸ĵĵط�
            //Ŀǰ����: �ٶ��޷����Ƽ���,��������֮Ӱ�����彵���ٶ�, ��һ���������ú�,�ͷŵڶ����������ʷ���������λ��(Ŀǰ�Ʋ����ڵ�һ��Vector2δ������(��һ��distance jointδ������,����Mouse1����2��distance joint����ײ����?)
            //��֪�����Ե�API���Ƽ��ٶ�/�ٶȼ���
            //ͨ��x,y���ٶȷֱ� ��1.2,ȷ������ٶ�Ϊ0, ��֪��ΪʲôδӰ�������ٶ�
            if (launchToPoint && rope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    if(checkIsAir)
                         player.checkAir(true);
                    checkIsAir = false;
                    if (check && Input.GetKey(KeyCode.Mouse1))
                    {
                        Vector2 fnlvector0 = rope.targetPosition - new Vector2(grapplingGun.firePoint.position.x, grapplingGun.firePoint.position.y);
                        Vector2 fnlvector1 = rope1.targetPosition - new Vector2(grapplingGun1.firePoint.position.x, grapplingGun1.firePoint.position.y);
                        Debug.Log("��һ��:"+fnlvector0.magnitude);
                        Debug.Log("�ڶ���"+fnlvector1.magnitude);

                        finalV = (fnlvector0 + fnlvector1);
                        controller.velocity = new Vector2( 4*finalV.x,4*finalV.y);
                        //controller.AddForce(force*finalV,ForceMode2D.Force);
                        check = false;
                    }
                    else if(!Input.GetKey(KeyCode.Mouse1))
                    {
                        gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);

                    }
                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        rope.enabled = false;
                        rope1.enabled = false;
                        m_distanceJoint2D.enabled = false;
                        m_rigidbody.gravityScale = 5;
                    }
                    //Ŀǰ����: �ٶ��޷����Ƽ���,��������֮Ӱ�����彵���ٶ�
                    //��֪�����Ե�API���Ƽ��ٶ�/�ٶȼ���
                    //ͨ��x,y���ٶȷֱ� ��1.2,ȷ������ٶ�Ϊ0, ��֪��ΪʲôδӰ�������ٶ�
                    //controller.velocity = finalV/*new Vector2(finalV.x/(float)1.2,finalV.y/(float)1.2)*/;
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            check = true;
            checkIsAir = true;
            if (!Input.GetKey(KeyCode.Z))
            {
                rope.enabled = false;
                m_distanceJoint2D.enabled = false;
                m_rigidbody.gravityScale = 5;
            }
            /*else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    SetGrapplePoint0();
                }
                else if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (grappleRope0.enabled)
                    {
                        RotateGun(grapplePoint0, false);
                    }
                    else
                    {
                        Vector2 mousePos0 = m_camera.ScreenToWorldPoint(Input.mousePosition);
                        RotateGun(mousePos0, true);
                    }

                    if (launchToPoint && grappleRope0.isGrappling)
                    {
                        if (launchType == LaunchType.Transform_Launch)
                        {
                            Vector2 firePointDistnace0 = firePoint0.position - gunHolder0.localPosition;
                            Vector2 targetPos0 = grapplePoint0 - firePointDistnace0;
                            gunHolder0.position = Vector2.Lerp(gunHolder0.position, targetPos0, Time.deltaTime * launchSpeed);
                        }
                    }
                }
            }*/
        }
        else
        {
            if (!Input.GetKey(KeyCode.Z))
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);          //����gunpivot���������ת
            }
        }
    }


    //����ʸ��ģ���������ʺ���
    float getVec(Vector2 finalVector)
    {
        float length = finalVector.magnitude;
        if (length < 5.7)
        {
            return 6;
        }
        else
            return length;
    }
    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;
        //�������Ǻ���ȷ����ת�Ƕ�
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {//ʹ��deltatimeȷ��ʵʱ����
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetGrapplePoint()
    {
        cameraPositionNow = m_camera.ScreenToWorldPoint(Input.mousePosition);
        //λ��ʸ��
        distanceVector = cameraPositionNow - gunPivot.position;
        //���߼��(normalized -> ��׼��ʹʸ��Ϊ��λ1)
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            //ȷ���Ƿ��������layerΪԤ���趨����ײ�����layer
            if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                //ȷ���Ƿ񳬳��޶�������
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    rope.enabled = true;
                }
            }
        }
        
    }

/*    void SetGrapplePoint0()
    {
        distanceVector0 = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot0.position;
        Vector2 jointVector = distanceVector + distanceVector0;
        //���߼��(normalized -> ��׼��ʹʸ��Ϊ��λ1)
        if (Physics2D.Raycast(firePoint0.position, distanceVector0.normalized))
        {
            RaycastHit2D _hit0 = Physics2D.Raycast(firePoint0.position, distanceVector0.normalized);
            //ȷ���Ƿ��������layerΪԤ���趨����ײ�����layer
            if (_hit0.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                //ȷ���Ƿ񳬳��޶�������
                if (Vector2.Distance(_hit0.point, firePoint0.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint0 = _hit0.point;
                    grappleDistanceVector0 = grapplePoint0 - (Vector2)gunPivot0.position;
                    grappleRope0.enabled = true;
                }
            }
        }
    }*/


    public void Grapple()
    {
        m_distanceJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_distanceJoint2D.distance = targetDistance;
            //m_distanceJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_distanceJoint2D.autoConfigureDistance = true;
                //m_distanceJoint2D.frequency = 0;
            }

            m_distanceJoint2D.connectedAnchor = grapplePoint;
            m_distanceJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    m_distanceJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    m_distanceJoint2D.distance = distanceVector.magnitude;
                    //m_distanceJoint2D.frequency = launchSpeed;
                    m_distanceJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
/*                    m_rigidbody.velocity = Vector2.zero;*/
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

}
