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

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var velocity = new Vector3(moveInput.x, 0, moveInput.y) * (moveSpeed * Time.deltaTime);
        velocity = Quaternion.AngleAxis(cameraAngle, Vector3.up) * velocity;
        controller.Move(velocity);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
