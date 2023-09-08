using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject cam;

    [Header("Mouse Controls")]
    public bool mouseScrolling = true;
    public bool showMouseScrolling = true;
    public float mouseScrollSize = 1.0f;
    public float scrollSensitivity = 1.0f;

    // these control the scrolling of the camera
    public float scrollX;
    public float scrollY;

    private BoxCollider scrollLeft;
    private BoxCollider scrollRight;
    private BoxCollider scrollUp;
    private BoxCollider scrollDown;

    [Header("Selector")]
    public GameRTSController selector;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider[] colliderList = GetComponentsInChildren<BoxCollider>();
        scrollLeft = colliderList[0];
        scrollRight = colliderList[1];
        scrollUp = colliderList[2];
        scrollDown = colliderList[3];
        selector = FindObjectOfType<GameRTSController>();
    }

    // update the scroll colliders to the value of the scroll size. Only call this when the mouseScrollSize changes!
    void UpdateScrollColliders()
    {
        Vector3 tempPos = cam.transform.position;
        tempPos.x -= (16f - (0.5f * mouseScrollSize));
        scrollLeft.transform.position = tempPos;

        tempPos = cam.transform.position;
        tempPos.x += (16f - (0.5f * mouseScrollSize));
        scrollRight.transform.position = tempPos;

        tempPos = cam.transform.position;
        tempPos.y += (9f - (0.5f * mouseScrollSize));
        scrollUp.transform.position = tempPos;

        tempPos = cam.transform.position;
        tempPos.y -= (9f - (0.5f * mouseScrollSize));
        scrollDown.transform.position = tempPos;

        scrollLeft.size = new Vector3(mouseScrollSize, 18, 0);
        scrollRight.size = new Vector3(mouseScrollSize, 18, 0);
        scrollUp.size = new Vector3(32, mouseScrollSize, 0);
        scrollDown.size = new Vector3(32, mouseScrollSize, 0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScrollColliders();
        ScrollCheck();
        // TODO - add in WASD or arrow scrolling
        float x = cam.transform.position.x;
        float y = cam.transform.position.y;

        Vector3 scroll = new Vector3(scrollX, scrollY);
        scroll *= scrollSensitivity;

        // checks for screen size
        if (x + scroll.x < -6f)
        {
            cam.transform.position = new Vector3(-6f, cam.transform.position.y);
            scroll.x = 0;
        }
        else if (x + scroll.x > 20f)
        {
            cam.transform.position = new Vector3(20f, cam.transform.position.y);
            scroll.x = 0;
        }
        if (y + scroll.y < -12)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, -12f);
            scroll.y = 0;
        }
        else if (y + scroll.y > 6f)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, 6f);
            scroll.y = 0;
        }

        cam.transform.position += scroll;

        // this commands the units to go to spot
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //
            Vector3 worldPosition = cam.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Linecast(worldPosition, worldPosition);
            if (hit)
            {
                // case for right clicking on an obstacle
                if (hit.collider.gameObject.name == "Obstacles")
                {
                    // if worker, make it farm

                    // else, find closest point to move towards
                }
                Debug.Log(hit.collider.gameObject.name);
            }

            List<Vector3> targetPositionList = GetPositionListAround(worldPosition, new float[] { 1f, 2f, 3f }, new int[] { 5, 10, 20 });

            int targetPositionListIndex = 0;

            foreach (Unit unit in selector.selectedUnitList)
            {
                if (unit != null)
                {
                    // world position of the mouse
                    unit.MoveTo(targetPositionList[targetPositionListIndex]);
                    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                }
            }
        }

    }

    // this gets a list around a single point
    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }
    
    //
    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

    public void ScrollCheck()
    {
        scrollX = 0f;
        scrollY = 0f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name == "ScrollLeft")
            {
                scrollX = -1f;
            }
            if (hit.transform.name == "ScrollRight")
            {
                scrollX = 1f;
            }
            if (hit.transform.name == "ScrollUp")
            {
                scrollY = 1f;
            }
            if (hit.transform.name == "ScrollDown")
            {
                scrollY = -1f;
            }
        }
        // this section if for the mouse being off the screen
        if (Input.mousePosition.x >= Screen.width)
        {
            scrollX = 1f;
        }
        if (Input.mousePosition.x <= 0)
        {
            scrollX = -1f;
        }
        if (Input.mousePosition.y >= Screen.height)
        {
            scrollY = 1f;
        }
        if (Input.mousePosition.y <= 0)
        {
            scrollY = -1f;
        }
    }

    private void OnDrawGizmos()
    {
        if (showMouseScrolling)
        {
            Gizmos.color = Color.red;
            // top
            Vector3 topPos = cam.transform.position;
            topPos.y += (9f - (0.5f * mouseScrollSize));
            Gizmos.DrawCube(topPos, new Vector3(32, mouseScrollSize, 0));
            topPos.y = cam.transform.position.y;
            // bottom
            topPos.y -= (9f - (0.5f * mouseScrollSize));
            Gizmos.DrawCube(topPos, new Vector3(32, mouseScrollSize, 0));
            topPos.y = cam.transform.position.y;
            // left
            topPos.x -= (16f - (0.5f * mouseScrollSize));
            Gizmos.DrawCube(topPos, new Vector3(mouseScrollSize, 18, 0));
            topPos.x = cam.transform.position.x;
            // right
            topPos.x += (16f - (0.5f * mouseScrollSize));
            Gizmos.DrawCube(topPos, new Vector3(mouseScrollSize, 18, 0));
            topPos.x = cam.transform.position.x;
        }
    }

}
