﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBall : BaseBehavior
{
    private PlayerHoldingState _holdingState;

    private PlayerFrozenState _frozenState;

    protected override void Awake()
    {
        base.Awake();

        _holdingState = this.GetComponent<PlayerHoldingState>();

        _frozenState = this.GetComponent<PlayerFrozenState>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!_frozenState.IsFrozen)
        {
            var collidedWith = collision.gameObject;

            //check if the collision was with the ball and make sure it wasn't thrown by another player if it was
            if (collidedWith.tag == "Ball" && !collidedWith.GetComponent<BallThrownState>().WasThrown)
            {
                _holdingState.StartHoldingBall(collidedWith); //Pick up the ball
            }
        }
    }
}
