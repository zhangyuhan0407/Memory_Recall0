using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseChange : MonoBehaviour
{
    public Texture2D texture;
    public Texture2D textureNor;
    public Transform transform;
    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        //transform = GetComponent<Transform>();
        //hit = new RaycastHit();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 orign = transform.position;
        //RaycastHit2D ray = Physics2D.Raycast(orign, Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (ray.transform.gameObject.layer == 9)
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "wall")
                Cursor.SetCursor(textureNor, new Vector2(16, 16), CursorMode.Auto);
            else
            {
                Cursor.SetCursor(texture, new Vector2(16, 16), CursorMode.Auto);
            }
        }

        //Debug.DrawRay(orign, Input.mousePosition);
    }
}
