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
    private ArrayList enemiesInRange; // game object list

    new private void Awake()
    {
        base.Awake();
    }

    new private void Start()
    {
        base.Start();
        enemiesInRange = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        // if no current target and there are enemies in range!
        if (target != null && enemiesInRange.Count > 0)
        {
            // Find the closest enemy and set them as target
            target = (GameObject) enemiesInRange[0];
            Debug.Log(target);
        }

        if (!isMovingToDestination && target != null)
        {
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete); // ADD instead of OnPathComplete an attack function that starts attacking??? Idk
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitObject = collision.gameObject;
        Debug.Log("The collidiosn");
        Debug.Log(LayerMask.NameToLayer("Unit"));
        Debug.Log(hitObject.layer);
        // if it is a unit not on my team
        if (hitObject.layer == LayerMask.NameToLayer("Unit") && hitObject.tag != gameObject.tag)
        {
            enemiesInRange.Add(hitObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the other one is a trigger, ignore it
        // we only want to check for a trigger range hitting the actual object, not the two ranges hitting each other
        GameObject hitObject = collision.gameObject;
        Debug.Log("The Trigger");
        Debug.Log(LayerMask.NameToLayer("Unit"));
        Debug.Log(hitObject.layer);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject hitObject = collision.gameObject;
        enemiesInRange.Remove(hitObject); // if it is not there will it crash?
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
