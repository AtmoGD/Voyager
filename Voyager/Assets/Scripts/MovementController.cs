using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private Rigidbody rb = null;
    private Vector3 movement = Vector3.zero;
    private Vector3 currentMovement = Vector3.zero;

    [SerializeField]
    private float maxSpeed = 1;

    [SerializeField]
    private float acceleration = 0.1f;

    [SerializeField]
    private float decceleration = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (movement.magnitude > currentMovement.magnitude)
            currentMovement = Vector3.Lerp(currentMovement, movement, acceleration);
        else
            currentMovement = Vector3.Lerp(currentMovement, movement, decceleration);

        Vector3 newPosition = transform.position + (currentMovement * maxSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movement.x = movementVector.x;
        movement.z = movementVector.y;
    }
}
