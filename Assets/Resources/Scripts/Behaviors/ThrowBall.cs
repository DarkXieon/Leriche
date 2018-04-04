﻿using UnityEngine;
using System.Collections;

public class ThrowBall : BaseBehavior
{
    [SerializeField]
    private float _maxThrowPower = 10f;

    [SerializeField]
    private float _minThrowPower = 3f;

    [SerializeField]
    private float _maxPowerHoldTime = 1f;

    private PlayerHoldingState _holdingState;

    private float _currentHoldTime = 0f;

    protected override void Awake()
    {
        base.Awake();

        _holdingState = this.GetComponent<PlayerHoldingState>();
    }

    private void Update()
    {
        if(_inputState.IsPressed(Buttons.THROW) && _holdingState.HoldingBall) //if the throw button is pressed and the player is holding the ball...
        {
            _currentHoldTime = _inputState.GetButtonHoldTime(Buttons.THROW);
        }
        else if(!_inputState.IsPressed(Buttons.THROW) && _holdingState.HoldingBall && _currentHoldTime != 0f) //if the throw button is not pressed, the player is holding the ball, and the player WAS holding the throw button
        {
            var ball = _holdingState.StopHoldingBall();

            var ballBody = ball.GetComponent<Rigidbody>();

            var forceAxis = _holdingState.HoldingIn.forward; //the forward axis relative to the object's current rotation

            var forceMultiplier = Mathf.Min(_currentHoldTime, _maxPowerHoldTime) / _maxPowerHoldTime; //this changes power based on hold time

            var force = Mathf.Max(_maxThrowPower * forceMultiplier, _minThrowPower); 

            var forceOnAxis = forceAxis * force; 

            ballBody.AddForce(forceOnAxis, ForceMode.VelocityChange); //we don't want to have to worry about the weight of the ball... at least not yet

            _currentHoldTime = 0f;
        }
    }
}
