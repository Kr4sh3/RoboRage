using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerController PlayerController;
    private InputMaster _controls;
    private PlayerInput _playerInput;
    public string CurrentControlScheme;
    private Camera _mainCamera;

    private void Start()
    {
        _controls = new InputMaster();
        _controls.Enable();
        _playerInput = GetComponent<PlayerInput>();
        CurrentControlScheme = _playerInput.currentControlScheme;
        _mainCamera = Camera.main;

        _controls.Player.Attack.performed += ctx => { Attack(); };
        _controls.Player.Attack.canceled += ctx => { CancelAttack(); };
        _controls.Player.Movement.performed += ctx => { Movement(ctx.ReadValue<Vector2>()); };
        _controls.Player.Jump.performed += ctx => { Jump(); };
        _controls.Player.Jump.canceled += ctx => { CancelJump(); };
        _controls.Player.Pause.performed += ctx => { Pause(); };
        _controls.Player.DevMode.performed += ctx => { DevMode(); };
        _controls.Player.Aim.performed += ctx => { Aim(ctx.ReadValue<Vector2>()); };
    }
    private void Update()
    {
        if (CurrentControlScheme != _playerInput.currentControlScheme)
            CurrentControlScheme = _playerInput.currentControlScheme;

    }
    private void Attack()
    {
        if (PlayerController == null)
            return;
        PlayerController.AttackPerformed();

    }
    private void CancelAttack()
    {
        if (PlayerController == null)
            return;
        PlayerController.AttackCanceled();

    }
    private void Movement(Vector2 move)
    {
        if (PlayerController == null)
            return;
        PlayerController.Movement(move.normalized);
    }
    private void Jump()
    {
        if (PlayerController == null)
            return;
        PlayerController.Jump();
    }
    private void CancelJump()
    {
        if (PlayerController == null)
            return;
        PlayerController.CancelJump();
    }
    private void Aim(Vector2 direction)
    {
        if (PlayerController == null)
            return;
        if (CurrentControlScheme == "KeyboardAndMouse")
        {
            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(direction);
            Vector3 aimDirection = worldPoint - PlayerController.transform.position;
            Vector2 aimDirection2D = new Vector2(aimDirection.x, aimDirection.y);
            PlayerController.Aim(aimDirection2D.normalized);
        }
        else
            PlayerController.Aim(direction.normalized);
    }
    private void Pause()
    {
        if (PlayerController == null)
            return;
    }
    private void DevMode()
    {
        if (Application.isEditor)
        {

        }
    }
}
