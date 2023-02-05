using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField]
    private float moveSpeed = 5f;
    private Vector2 moveInput = Vector2.zero;
    private float cameraAngle = 45;
    private Vector3 smoothedMoveInput;
    private Vector3 currentVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var velocity = new Vector3(moveInput.x, 0, moveInput.y) * (moveSpeed * Time.deltaTime);
        velocity = Quaternion.AngleAxis(cameraAngle, Vector3.up) * velocity;

        smoothedMoveInput = Vector3.SmoothDamp(
            smoothedMoveInput,
            velocity,
            ref currentVelocity,
            0.1f
        );

        RotateTowardsInput();

        controller.Move(smoothedMoveInput);
    }

    private void RotateTowardsInput()
    {
        if (moveInput != Vector2.zero)
        {
            var targetAngle = Mathf.Atan2(smoothedMoveInput.x, smoothedMoveInput.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
