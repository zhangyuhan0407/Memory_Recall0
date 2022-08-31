using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    [Header("General Refernces:")]
    public GrapplingGun grapplingGun;
    public LineRenderer m_lineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int percision = 40;
    [Range(0, 20)] [SerializeField] private float straightenLineSpeed = 5;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;
    float waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 1;

    float moveTime = 0;

    [SerializeField] public bool isGrappling = false;

    public bool strightLine = true;

    [SerializeField] public float delta;
    [SerializeField] public Vector2 offset;
    [SerializeField] public Vector2 targetPosition = new Vector2(0, 0);
    [SerializeField] public Vector2 currentPosition = new Vector2(0, 0);

    private void OnEnable()         //默认为true, 执行顺序为: Awake  ->  OnEnable  ->  Start
    {
        moveTime = 0;   //从这一刻开始计算时间
        m_lineRenderer.positionCount = percision;   //精度,player从起点到目标点之间..多少点(每个点position)
        waveSize = StartWaveSize;   //手动修改波纹size
        strightLine = false;    //初始绳子表现为弯曲,之后变直

        LinePointsToFirePoint();    //将精度确定的点赋值给 firepoint

        m_lineRenderer.enabled = true;
    }

    private void OnDisable()        //当脚本设置为false时 执行
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }

    private void LinePointsToFirePoint()
    {
        for (int i = 0; i < percision; i++)
        {
            m_lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
        }
    }

    private void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
    }

    void DrawRope()     //画线
    {
        if (!strightLine)       //检测是否需要将线条变直
        {
            //如果OnEnable中设置的精度positions全部被player图形到达
            if (m_lineRenderer.GetPosition(percision - 1).x == grapplingGun.grapplePoint.x)     
            {
                strightLine = true;
            }
            else
            {   //否则将绘制波纹
                DrawRopeWaves();
            }
        }
        else
        {
            if (!isGrappling)
            {
                grapplingGun.Grapple();
                isGrappling = true;
            }
            if (waveSize > 0)   //开始逐渐减小波纹尺寸,知道变为直线
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (m_lineRenderer.positionCount != 2) { m_lineRenderer.positionCount = 2; }

                DrawRopeNoWaves();//设置结束后. 第一个点为firepoint,第二个点为grapplepoint
            }
        }
    }

    void DrawRopeWaves()    //根据wavesize,delta,fireposition等   慢慢调整波纹偏差
    {
        for (int i = 0; i < percision; i++)
        {
            delta = (float)i / ((float)percision - 1f);
            offset = Vector2.Perpendicular(grapplingGun.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grapplePoint, delta) + offset;
            //通过movetime移动时间评估位移量
            currentPosition = Vector2.Lerp(grapplingGun.firePoint.position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            m_lineRenderer.SetPosition(i, currentPosition);
        }
    }

    void DrawRopeNoWaves()
    {
        m_lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
        m_lineRenderer.SetPosition(1, grapplingGun.grapplePoint);
    }
}
