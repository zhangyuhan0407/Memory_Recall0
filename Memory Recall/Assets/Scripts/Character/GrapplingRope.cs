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

    private void OnEnable()         //Ĭ��Ϊtrue, ִ��˳��Ϊ: Awake  ->  OnEnable  ->  Start
    {
        moveTime = 0;   //����һ�̿�ʼ����ʱ��
        m_lineRenderer.positionCount = percision;   //����,player����㵽Ŀ���֮��..���ٵ�(ÿ����position)
        waveSize = StartWaveSize;   //�ֶ��޸Ĳ���size
        strightLine = false;    //��ʼ���ӱ���Ϊ����,֮���ֱ

        LinePointsToFirePoint();    //������ȷ���ĵ㸳ֵ�� firepoint

        m_lineRenderer.enabled = true;
    }

    private void OnDisable()        //���ű�����Ϊfalseʱ ִ��
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

    void DrawRope()     //����
    {
        if (!strightLine)       //����Ƿ���Ҫ��������ֱ
        {
            //���OnEnable�����õľ���positionsȫ����playerͼ�ε���
            if (m_lineRenderer.GetPosition(percision - 1).x == grapplingGun.grapplePoint.x)     
            {
                strightLine = true;
            }
            else
            {   //���򽫻��Ʋ���
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
            if (waveSize > 0)   //��ʼ�𽥼�С���Ƴߴ�,֪����Ϊֱ��
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (m_lineRenderer.positionCount != 2) { m_lineRenderer.positionCount = 2; }

                DrawRopeNoWaves();//���ý�����. ��һ����Ϊfirepoint,�ڶ�����Ϊgrapplepoint
            }
        }
    }

    void DrawRopeWaves()    //����wavesize,delta,fireposition��   ������������ƫ��
    {
        for (int i = 0; i < percision; i++)
        {
            delta = (float)i / ((float)percision - 1f);
            offset = Vector2.Perpendicular(grapplingGun.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grapplePoint, delta) + offset;
            //ͨ��movetime�ƶ�ʱ������λ����
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
