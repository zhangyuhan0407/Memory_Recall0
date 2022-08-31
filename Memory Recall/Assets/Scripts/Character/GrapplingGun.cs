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
    public Transform gunPivot;
    private Transform gunPivot1;
    public Transform firePoint;
    public Transform firePoint1;

    [Header("Physics Ref:")]
    public DistanceJoint2D m_distanceJoint2D;
    public Rigidbody2D m_rigidbody;
    public GameObject attackWeapon;
    public float m_gravity;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;
    [SerializeField] public LayerMask groundLayer;

    private bool check = true;
    private bool checkIsAir = true;
    private bool checkIsGrappling = true;
    private float grapplingTime;
    /*    private bool castToCollider = false;*/
    public bool canTouchFall = false;

    [SerializeField] private Vector3 cameraPositionNow;
    [SerializeField] private Vector3 cameraPositionNow1;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 5.7f;
    [SerializeField] private float launchSpeed1 = 10f;
    [SerializeField] private float originV = 5.7f;
    [SerializeField] private float refreshV = 5.7f;
    [SerializeField] private Vector2 m_speed = new Vector2(1, 1).normalized;
    [SerializeField] private float horDistance;
    [SerializeField] private float verDistance;
    [SerializeField] private bool checkMovementDir = true;
    [SerializeField] private float diruaction = 5f;
    [SerializeField] private float airTime;


[Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    /*    [SerializeField] private float targetFrequncy = 1;
    */
    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grapplePoint0;
    [HideInInspector] public Vector2 grappleDistanceVector;
    [HideInInspector] public Vector2 grappleDistanceVector0;

    [HideInInspector] public Vector2 distanceVector = new Vector2(0, 0);
    [HideInInspector] public Vector2 distanceVector1 = new Vector2(0, 0);
    [HideInInspector] public Vector2 distanceVector0;
    [HideInInspector] public Vector2 finalV;

    private void Start()
    {
        rope.enabled = false;
        //attackWeapon = GameObject.FindGameObjectWithTag("attack");
        //attackWeapon.SetActive(false);
        //grappleRope0.enabled = false;
        //m_distanceJoint2D.enabled = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();

            if (!canTouchFall)
            {
                //Debug.Log(canTouchFall);
                rope.enabled = false;
                rope1.enabled = false;
                //m_distanceJoint2D.enabled = false;
                m_rigidbody.gravityScale = m_gravity;
                controller.velocity = Vector2.zero;
                checkIsGrappling = true;
                return;
            }
            attackWeapon.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!canTouchFall)
            {
                //Debug.Log(canTouchFall);
                rope.enabled = false;
                rope1.enabled = false;
                //m_distanceJoint2D.enabled = false;
                m_rigidbody.gravityScale = m_gravity;
                controller.velocity = Vector2.zero;
                checkIsGrappling = true;
                return;
            }
            if (rope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }
            //目前正在修改的地方
            //目前问题: 速度无法控制减速,空气阻力之影响物体降落速度, 第一个钩索作用后,释放第二个钩索概率发生反方向位移(目前推测由于第一个Vector2未作用完(第一个distance joint未作用完,按下Mouse1导致2个distance joint的碰撞问题?)
            //不知都特性的API控制加速度/速度减速
            //通过x,y分速度分别 除1.2,确保最后速度为0, 不知道为什么未影响物体速度
            if (launchToPoint && rope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    grapplingMovement();
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            check = true;
            checkIsAir = true;
            rope.enabled = false;
            m_distanceJoint2D.enabled = false;
            m_rigidbody.gravityScale = m_gravity;
            canTouchFall = false;
            checkIsGrappling = true;
            checkMovementDir = true;
            attackWeapon.SetActive(false);
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);          //设置gunpivot随着鼠标旋转
        }
    }


    //根据矢量模长计算速率函数
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
        //考虑三角函数确定旋转角度
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {//使用deltatime确保实时更新
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
        //位移矢量
        distanceVector = cameraPositionNow - gunPivot.position;
        //视线检测(normalized -> 标准化使矢量为单位1)
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            RaycastHit2D[] _hits = Physics2D.RaycastAll(firePoint.position, distanceVector.normalized, maxDistnace, groundLayer);
            if (_hit.transform.gameObject.layer == 16)
            {
                rope.enabled = false;
                canTouchFall = false;
                return;
            }
            for (int i = 0; i < _hits.Length; i++)
            {
                if (_hits[i].transform.gameObject.layer == grappableLayerNumber)//||grappleAll
                {
                    //确定是否超出限定最大距离
                    if (Vector2.Distance(_hits[i].point, firePoint.position) <= maxDistnace || !hasMaxDistance)
                    {
                        grapplePoint = _hits[i].point;
                        grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                        rope.enabled = true;
                        canTouchFall = true;
                        return;
                    }
                }
            }


            //确定是否对象所属layer为预先设定的碰撞体对象layer

        }

    }

    /*    RaycastHit2D[] RaycastAll(Vector2 offset, Vector2 raydiraction, float length, LayerMask layer)
        {
            Vector2 pos = firePoint.position;
            Color color=Color.green;
            RaycastHit2D[] hit = Physics2D.RaycastAll(pos + offset, raydiraction, length, layer);
            Debug.DrawRay(pos, raydiraction, color);

            return hit;
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
            //m_distanceJoint2D.enabled = true;
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
                    //m_distanceJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    m_rigidbody.gravityScale = 0;
                    /*                    m_rigidbody.velocity = Vector2.zero;*/
                    break;
            }
        }
    }

    bool checkBugGrappling()
    {
        if (!canTouchFall)
            return false;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!canTouchFall)
            {
                check = true;
                checkIsAir = true;
                rope.enabled = false;
                rope1.enabled = false;
                m_distanceJoint2D.enabled = false;
                m_rigidbody.gravityScale = m_gravity;
                checkIsGrappling = true;
                return false;
            }
        }
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }
    }

    void grapplingMovement()
    {
        Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
        Vector2 targetPos = grapplePoint - firePointDistnace;
        if (checkIsAir)
            player.checkAir(true);
        checkIsAir = false;
        if (check && Input.GetKey(KeyCode.Mouse1))
        {
            attackWeapon.SetActive(true);
            Vector2 fnlvector0 = grapplePoint - new Vector2(grapplingGun.gunHolder.position.x, grapplingGun.gunHolder.position.y);
            Vector2 fnlvector1 = grapplingGun1.grapplePoint - new Vector2(grapplingGun1.gunHolder.position.x, grapplingGun1.gunHolder.position.y);
            /*Debug.Log("第一个:"+fnlvector0.magnitude);
                Debug.Log("第二个"+fnlvector1.magnitude);*/

            finalV = (fnlvector0 + fnlvector1);
            gunPivot1 = grapplingGun1.gunPivot;
            firePoint1 = grapplingGun1.firePoint;
            float m_distance = finalV.magnitude;
            if(m_distance>50)
            {
                m_distance = 50f;
            }
            //Debug.Log(checkIsGrappling);
            if (checkIsGrappling)//、、、、、、、、记得修改之后每次rope.enable = false的时候都要调用
            {
                if (finalV.magnitude > 1.5 * 5.7)
                    controller.velocity = new Vector2(originV*finalV.x, originV * finalV.y);
                else 
                {
                    Debug.Log(finalV.magnitude);
                    Debug.Log(1.5 * launchSpeed);
                    Vector2 newSpeed = finalV.normalized;
                    controller.velocity = new Vector2(newSpeed.x * refreshV, newSpeed.y * refreshV);
                }
                grapplingTime = 1.3f * m_distance / controller.velocity.magnitude;
                grapplingTime = Time.time + grapplingTime;
                checkIsGrappling = false;
                //滞空状态下速度缓慢降至0的过程
                airTime = Time.time + diruaction;
            }
            else
            {

            }
            //controller.AddForce(force*finalV,ForceMode2D.Force);
            check = false;
        }
        else if (!Input.GetKey(KeyCode.Mouse1))
        {
            //gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
            m_speed = distanceVector.normalized;
            if(checkMovementDir)
            {
                horDistance = targetPos.x - gunHolder.position.x;
                verDistance = targetPos.y - gunHolder.position.y;
                checkMovementDir = false;
            }
            float xVelocity = Input.GetAxis("Horizontal");
            float yVelocity = Input.GetAxis("Vertical");
/*            if(horDistance>verDistance)
*/            {
                controller.velocity = new Vector2(xVelocity * 10 + m_speed.x  * launchSpeed1, m_speed.y * launchSpeed1);
                if(Mathf.Abs(gunHolder.position.x - targetPos.x)>= 2)
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
            }
/*            else if (horDistance < verDistance)
            {
                controller.velocity = new Vector2(m_speed.x  * launchSpeed, yVelocity * 10 + m_speed.y * launchSpeed);
                if (Mathf.Abs(gunHolder.position.y - targetPos.y) >= 2)
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
            }
            else
            {
                gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
            }*/
        }
        Debug.DrawRay(gunHolder.position, controller.velocity, Color.yellow);
/*        if (Input.GetKeyDown(KeyCode.Space))
        {
            rope.enabled = false;
            rope1.enabled = false;
            m_distanceJoint2D.enabled = false;
            m_rigidbody.gravityScale = m_gravity;
            checkIsGrappling = true;
        }*/
        if(!checkIsGrappling)
        {
            if (grapplingTime < Time.time)
            {
                controller.velocity = grapplingInAir();
                rope.enabled = false;
                rope1.enabled = false;
                m_distanceJoint2D.enabled = false;
                m_rigidbody.gravityScale = m_gravity;
/*                for (int i = 0; i < 10; i++)
                    Time.timeScale = 0;
                Time.timeScale = 1;*/
                //然后给予一段时间的系统暂停，保证玩家可以思考下一步操作
            }
        }
        //目前问题: 速度无法控制减速,空气阻力之影响物体降落速度
        //不知都特性的API控制加速度/速度减速
        //通过x,y分速度分别 除1.2,确保最后速度为0, 不知道为什么未影响物体速度
        //controller.velocity = finalV/*new Vector2(finalV.x/(float)1.2,finalV.y/(float)1.2)*/;
    }
    Vector2 grapplingInAir()
    {
        if (airTime < Time.time)
            return Vector2.zero;
        else
            return new Vector2(controller.velocity.x / 5, controller.velocity.y / 5);
    }
}
