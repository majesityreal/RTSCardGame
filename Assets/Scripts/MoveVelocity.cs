﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity {

    [SerializeField] private float moveSpeed;

    private Vector3 velocityVector;
    private Rigidbody2D rigidbody;
    private Character_Base characterBase;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rigidbody.velocity = velocityVector * moveSpeed;

        // TODO - make the sprite change based on the vector
/*        characterBase.PlayMoveAnim(velocityVector);
*/    }

    public void Disable() {
        this.enabled = false;
        rigidbody.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }

}
