using UnityEngine;

public class GrapplingGun1 : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope1 rope1;
    public GrapplingRope rope;
    public GrapplingGun grapplingGun;
    public GrapplingGun1 grapplingGun1;

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
    public Transform gunPivot;
    public Transform firePoint;

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
    [HideInInspector] public Vector2 grappleDistanceVector;

    [HideInInspector] public Vector2 finalV;

    private void Start()
    {
        rope1.enabled = false;
        m_distanceJoint2D.enabled = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            if (rope1.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if (launchToPoint && rope1.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistnace;
                    if (check && Input.GetKey(KeyCode.Mouse0))
                    {
                        Vector2 fnlvector0 = rope.targetPosition - new Vector2(grapplingGun.firePoint.position.x, grapplingGun.firePoint.position.y);
                        Vector2 fnlvector1 = rope1.targetPosition - new Vector2(grapplingGun1.firePoint.position.x, grapplingGun1.firePoint.position.y);
                        Debug.Log("第一个:" + fnlvector0.magnitude);
                        Debug.Log("第二个" + fnlvector1.magnitude);

                        finalV =  (fnlvector0 + fnlvector1);
                        controller.velocity = new Vector2(4 * finalV.x, 4 * finalV.y);
                        //controller.AddForce(force*finalV, ForceMode2D.Force);
                        check = false;
                    }
                    else if (!Input.GetKey(KeyCode.Mouse0))
                    {
                        gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                    }
                    //controller.velocity = finalV;
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            rope1.enabled = false;
            m_distanceJoint2D.enabled = false;
            m_rigidbody.gravityScale = 5;
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetGrapplePoint()
    {
        //位移矢量
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        //视线检测(normalized -> 标准化使矢量为单位1)
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            //确定是否对象所属layer为预先设定的碰撞体对象layer
            if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                //确定是否超出限定最大距离
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                {
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    rope1.enabled = true;
                }
            }
        }
    }

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
                    //m_rigidbody.velocity = Vector2.zero;
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
