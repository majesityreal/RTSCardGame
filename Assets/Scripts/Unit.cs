using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Unit : MonoBehaviour
{

    private GameObject isSelectedSprite;

    public float moveSpeed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    private void Awake()
    {
        isSelectedSprite = transform.Find("Selected").gameObject; // hard coded, do not do
        SetSelectedVisible(false);
    }



    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            rb.velocity = new Vector2();
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 move = direction * moveSpeed * Time.deltaTime;

        rb.velocity = move;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

    }

    #region C* pathfinding stuff

    public void MoveTo(Vector3 targetPosition)
    {
        seeker.StartPath(rb.position, targetPosition, OnPathComplete);
    }

    public void SetSelectedVisible(bool visible)
    {
        isSelectedSprite.SetActive(visible);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    #endregion





}