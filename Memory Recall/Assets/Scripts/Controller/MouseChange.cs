using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseChange : MonoBehaviour
{
    public Texture2D textureNormal;
    public Texture2D textureWall;


    private RaycastHit2D hitinfo;
    public Ray2D ray;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 orign = transform.position;
        //RaycastHit2D ray = Physics2D.Raycast(orign, Input.mousePosition);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //hitinfo = Physics2D.Raycast(Camera.main.WorldToScreenPoint(Input.mousePosition), Vector2.zero);
        hitinfo = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        //��ʾ�д�����Collider
        if (hitinfo)
        {
            switch (hitinfo.collider.tag)
            {
                //������wallʱ����ʾwall���
                case "wall":
                    Cursor.SetCursor(textureWall, new Vector2(16, 16), CursorMode.Auto);
                    break;
                //��������Colliderû��Tagʱ����ʾĬ�Ϲ��
                default:
                    Cursor.SetCursor(textureNormal, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
        //��ʾû�д�����Collider
        else
        {
            Cursor.SetCursor(textureNormal, new Vector2(16, 16), CursorMode.Auto);
        }
    }

}
