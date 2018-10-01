﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Jump : BaseBehavior
{
    public Vector3 jump; // controls height of jump
    public float jumpForce = 2.0f;
    public float velocityTest = 0.5f;
    private bool wasOnGround = false;

    private PlayerFrozenState _playerFrozenState;
    private AnimationSwitcher _animationSwitcher;

    public LayerMask CollisionLayer;
    public Vector3 gizmoPosition;
    public float radius;
  
    protected override void Start()
    {
        base.Start();

        jump = new Vector3(0.0f, 2.0f, 0.0f); // set jump height value here

        _playerFrozenState = GetComponent<PlayerFrozenState>();
        _animationSwitcher = GetComponent<AnimationSwitcher>();
    }

    private void Update ()
    {
        if(isLocalPlayer)
        {
            bool isTouchingGround = Physics.CheckSphere(this.transform.position + gizmoPosition, radius, CollisionLayer); // sees if the gizmo is colliding with terrain

            if (isTouchingGround && !wasOnGround)
            {
                CmdEndJumpAnimation();
            }

            wasOnGround = isTouchingGround;

            if (_inputState.IsPressed(Buttons.JUMP) && isTouchingGround && !_playerFrozenState.IsFrozen) // checks if jump button is pressed and gizmo is colliding with terrain
            {
                //var quickfix = this.transform.up;
                _body.AddForce(jump/*jumpForce*/, ForceMode.VelocityChange);
                CmdStartJumpAnimation();
            }
            //Debug.Log(isTouchingGround);
        }
    }
    
    private void OnDrawGizmos() // creates a gizmo
    {
        Gizmos.DrawSphere(this.transform.position + gizmoPosition, radius); // sets gizmo origin and causes gizmo to follow player
    }
    
    [Command]
    private void CmdStartJumpAnimation()
    {
        _animationSwitcher.Jump();
    }

    [Command]
    private void CmdEndJumpAnimation()
    {
        _animationSwitcher.Land();
    }
}