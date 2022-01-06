using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorScript : MonoBehaviour
{

    public ArrayList selected;

    private Camera cam;

    public Vector2 boxPos1;
    public Vector2 boxPos2;

    public Texture2D cursor;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        selected = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        // handle first frame mouse click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            boxPos1 = new Vector2((Input.mousePosition.x / Screen.width) * 32f, (Input.mousePosition.y / Screen.height) * 18f);
            boxPos1 -= new Vector2(16f, 9f);
            boxPos1 += new Vector2(cam.transform.position.x, cam.transform.position.y);
            // reseting the selected units (TODO - unless the shift key is pressed)
            foreach (GameObject obj in selected)
            {
                Unit unit = obj.gameObject.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.SetSelectedVisible(false);
                }
            }
            selected.Clear();
        }
        // updates the rectangular select box
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // takes proportion of screen from pixels and multiplies it by unit length of screen in Unity units (32 x 18)
            boxPos2 = new Vector2((Input.mousePosition.x / Screen.width) * 32f, (Input.mousePosition.y / Screen.height) * 18f);
            boxPos2 -= new Vector2(16f, 9f);
            boxPos2 += new Vector2(cam.transform.position.x, cam.transform.position.y);
            // this is to fix things
            Vector2 averagePos = ((boxPos1 + boxPos2) / 2f);
            Vector2 size = (boxPos2 - boxPos1);
            size.x = Mathf.Abs(size.x);
            size.y = Mathf.Abs(size.y);
            gameObject.transform.position = averagePos;
            gameObject.transform.localScale = size;

        }
        // resets selector
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            boxPos2 = new Vector2();
            boxPos2 = new Vector2();
            gameObject.transform.localScale = new Vector2();

            if (selected.Count > 0)
            {
                Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.ToString());
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit") && Input.GetKey(KeyCode.Mouse0)) {
            if (!selected.Contains(other.gameObject))
            {
                selected.Add(other.gameObject);
                Unit unit = other.gameObject.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.SetSelectedVisible(true);
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit") && Input.GetKey(KeyCode.Mouse0))
        {
            selected.Remove(other.gameObject);
            Unit unit = other.gameObject.GetComponent<Unit>();
            if (unit != null)
            {
                unit.SetSelectedVisible(false);
            }
        }
    }
}
