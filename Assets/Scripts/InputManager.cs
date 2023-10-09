using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerController PlayerController;
    private InputMaster _controls;

    private void Start()
    {
        _controls = new InputMaster();
        _controls.Enable();

        _controls.Player.Attack.performed += ctx => { Attack(); };
        _controls.Player.Attack.canceled += ctx => { CancelAttack(); };
        _controls.Player.Movement.performed += ctx => { Movement(ctx.ReadValue<Vector2>()); };
        _controls.Player.Jump.performed += ctx => { Jump(); };
        _controls.Player.Jump.canceled += ctx => { CancelJump(); };
        _controls.Player.Pause.performed += ctx => { Pause(); };
        _controls.Player.DevMode.performed += ctx => { DevMode(); };
    }
    private void Attack()
    {
        if(PlayerController != null){
            PlayerController.AttackPerformed();
        }
    }
    private void CancelAttack()
    {
        if(PlayerController != null){
            PlayerController.AttackCanceled();
        }
    }
    private void Movement(Vector2 move)
    {
        if(PlayerController != null){
            PlayerController.Movement(move);
        }
    }
    private void Jump()
    {
        if(PlayerController != null){
            PlayerController.Jump();
        }
    }
    private void CancelJump()
    {
        if(PlayerController != null){
            PlayerController.CancelJump();
        }
    }
    private void Pause()
    {

    }
    private void DevMode()
    {
        if (Application.isEditor)
        {

        }
    }
}
