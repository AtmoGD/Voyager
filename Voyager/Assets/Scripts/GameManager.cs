using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    PlayerController player = null;

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector3 movement = context.ReadValue<Vector2>();
        // player.movement = new Vector3(movement.x, 0, movement.y);
    }
}
