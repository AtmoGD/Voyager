using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent = null;
    public Vector3 movement { get; set; }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (movement.magnitude > 0.0f)
            Move();
    }

    public void TakeMovementInput(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();
        movement = new Vector3(input.x, 0, input.y).normalized;
    }

    void Move()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 movingDirection = forward * movement.z + right * movement.x;
        Vector3 newPosition = transform.position + movingDirection.normalized;

        agent.SetDestination(newPosition);
    }
}
