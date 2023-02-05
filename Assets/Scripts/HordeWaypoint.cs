using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HordeWaypoint : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.5f;
    private Vector2 moveInput = Vector2.zero;
    public GameObject player;
    public float maxDistance = 0.3f;
    private Vector3 currentMoveVelocity;
    private Vector3 moveDampVelocity;

    private void Start() {
        transform.position = player.transform.position;
    }

    private void Update()
    {
        Vector3 playerInput = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical"),
        };

        if (playerInput.magnitude > 1f)
        {
            playerInput.Normalize();
        }

        Vector3 moveVector = transform.TransformDirection(playerInput);

        if (moveVector.Equals(Vector3.zero))
        {
            var step =  moveSpeed * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, step);
            return;
        }

        var velocity = moveVector * moveSpeed * Time.deltaTime;
        Vector3 moveDist = (transform.localPosition + velocity);
        if (moveDist.magnitude > maxDistance)  
        {
            moveDist += velocity * 3;
            moveDist = moveDist.normalized * maxDistance;
        }
        
        transform.localPosition = moveDist;
    }
}
