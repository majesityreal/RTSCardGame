using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackUnit : Unit
{

    public float attackRange;
    public float targetRange;

    // TODO - add a target single type rather than a GameObject
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private LayerMask layersToAttack;

    new private void Awake()
    {
        base.Awake();
    }

    new private void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // if no current target and there are enemies in range!
        if (target == null)
        {
            // Find the closest enemy and set them as target
            Collider2D[] hitInRange = Physics2D.OverlapCircleAll(gameObject.transform.position, targetRange);
            // now we filter to find closest potential enemies
            float closestCollider = targetRange;
            foreach (Collider2D collider in hitInRange)
            {
                if (collider.tag == gameObject.tag || !((collider.gameObject.layer | layersToAttack) > 0)) // bitmask, we & the layer mask with the current layer and if it is greater than 0 it hits
                {
                    continue;
                }
                Debug.Log(collider.name);
                Vector2 thisUnitPos = gameObject.transform.position;
                Debug.Log(thisUnitPos);
                float distance = Vector2.Distance(gameObject.transform.position, collider.transform.position);
                if (distance < closestCollider)
                {
                    closestCollider = distance;
                    target = collider.gameObject;
                }
            }
            Debug.Log(target);
        }

        if (!isMovingToDestination && target != null)
        {
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete); // ADD instead of OnPathComplete an attack function that starts attacking??? Idk
        }

    }

    // looking colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the other one is a trigger, ignore it
        // we only want to check for a trigger range hitting the actual object, not the two ranges hitting each other
        GameObject hitObject = collision.gameObject;
        // if it is not my team and it is a unit, you can set enemy
        if (hitObject.tag != gameObject.tag && hitObject.layer == LayerMask.NameToLayer("Unit"))
        {
            Debug.Log("I HIT A RED ONE HAHA");
        }
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(gameObject.transform.position, Vector3.back, attackRange);
        UnityEditor.Handles.DrawWireDisc(gameObject.transform.position, Vector3.back, targetRange);
    }

    // if moving, do not attack
    // if idle and there is enemy unit in range, move to it and attack
    // if selected, and right click on enemy unit, set target to that. Move to that unit and attack.

}
