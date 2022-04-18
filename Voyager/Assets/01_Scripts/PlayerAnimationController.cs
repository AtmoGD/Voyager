using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerAnimationController : MonoBehaviour
{
    Animator anim = null;
    PlayerController playerController = null;
    NavMeshAgent playerNavMesh = null;
    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerNavMesh = GetComponent<NavMeshAgent>();

        playerController.OnMove += SetMovementSpeed;
    }

    void SetMovementSpeed(Vector3 _movement)
    {

        anim.SetFloat("Speed", _movement.magnitude);
    }
}
