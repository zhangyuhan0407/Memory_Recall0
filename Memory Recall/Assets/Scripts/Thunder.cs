using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public int PosSize;
    public bool useArcing;
    public bool useSine;
    public bool useWiggle;
    [Range(-1, 1)]
    public float _ArcingPowParam1;
    [Range(0,5)]
    public float _SineScaleX;
    public float _SineScaleY;
    public float _CenterOffset;
    public float _Adjust;
    public float _RandomSize;
    public float _Speed;

    LineRenderer lineRenderer;
    List<Vector3> posList = new List<Vector3>();
    List<Vector3> wiggleRandom = new List<Vector3>();
    Vector3 startPos;
    Vector3 endPos;
    float fps;
    float sineRanom = 0;

    private void OnEnable()
    {
        StartCoroutine(WiggleR(0.1f));
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        startPos = lineRenderer.GetPosition(0);
        endPos = lineRenderer.GetPosition(1);
        LerpPos();
    }

    // Update is called once per frame
    void Update()
    {
        fps += Time.deltaTime * _Speed;

        if(fps>1)
        {
            sineRanom = (float)Random.Range(0, posList.Count * 10) / 10;
            fps = 0;
        }
        List<Vector3> list = new List<Vector3>();
        for(int i =0;i<posList.Count;i++)
        {
            Vector3 point = posList[i];
            if (useArcing)
                point = Vector3.Lerp(posList[i], posList[i] + new Vector3(0, Arcing(i), 0), fps);
            if(useSine && i!=0 && i!=posList.Count-1)
            {
                if (useWiggle)
                    point += new Vector3(0, Sine(i + sineRanom) + Wiggle(i).y, 0);
                else
                    point += new Vector3(0, Sine(i + sineRanom), 0);
            }
            list.Add(point);
        }
        SetLinePosition(list);
    }

    void SetLinePosition(List<Vector3> listPoint)
    {
        lineRenderer.positionCount = listPoint.Count;
        for (int i = 0; i < listPoint.Count; i++)
        {
            lineRenderer.SetPosition(i, listPoint[i]);
        }
    }

    void LerpPos()
    {
        int index = PosSize + 2;
        for(int i =0;i<index;i++)
        {
            posList.Add(Vector3.Lerp(startPos, endPos, (float)i / index));
        }
    }

    float Arcing(float param)
    {
        return _ArcingPowParam1 * Mathf.Pow((param - (float)posList.Count / 2 + _CenterOffset) * _ArcingPowParam1,2)+_Adjust;
    }

    float Sine(float param)
    {
        return Mathf.Sin((float)param / posList.Count * 2 * 3.14f * _SineScaleX) * _SineScaleY;
    }

    Vector3 Wiggle(int listIndex)
    {
        if(wiggleRandom.Count<=0)
        {
            for(int i =0; i< posList.Count; i++)
            {
                wiggleRandom.Add(posList[i]);
            }
            StartCoroutine(WiggleR(0.1f));
        }
        return wiggleRandom[listIndex];
    }

    IEnumerator WiggleR(float time)
    {
        while(true)
        {
            for(int i =0; i<wiggleRandom.Count;i++)
            {
                wiggleRandom[i] = new Vector3(0, Random.Range(0, 10) * _RandomSize, 0);
            }
            yield return new WaitForSeconds(time);
        }
    }
}
