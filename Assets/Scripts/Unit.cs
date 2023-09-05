using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Unit : MonoBehaviour
{

    private GameObject isSelectedSprite;

    public float moveSpeed = 200f;
    public float nextWaypointDistance = 3f;

    protected Path path;
    protected int currentWaypoint = 0;
    protected bool isMovingToDestination = false;

    protected Seeker seeker;
    protected Rigidbody2D rb;

    protected void Awake()
    {
        isSelectedSprite = transform.Find("Selected").gameObject; // hard coded, do not do
        SetSelectedVisible(false);
        Debug.Log("I have stuff");
    }



    // Start is called before the first frame update
    protected void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            isMovingToDestination = false;
            rb.velocity = new Vector2();
            return;
        }
        else
        {
            isMovingToDestination = true;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 move = direction * moveSpeed * Time.deltaTime;

        rb.velocity = move;

        float distanceToWaypoint = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distanceToWaypoint < nextWaypointDistance)
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

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    #endregion





}