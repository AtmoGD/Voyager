using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlightState
{
    Controlled,
    Idle,
    Orbiting,
    Following
}


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class MateController : MonoBehaviour
{
    Rigidbody rb = null;
    Animator anim = null;
    public FlightState state = FlightState.Following;
    public Vector3 velocity = Vector3.zero;
    // Vector3 idlePosition = Vector3.zero;
    float sine = 0f;

    [SerializeField] private Transform mate = null;
    [SerializeField] private float idleSpeed = 0.2f;
    [SerializeField] private float velocitySlerp = 0.1f;
    [SerializeField] private float velocityMax = 20f;
    [SerializeField] private float heightMin = 2f;
    [SerializeField] private float heightMax = 10f;

    [SerializeField] private float heightRegulationSpeed = 5f;

    [SerializeField] private Vector3 offset = Vector3.zero;

    [SerializeField] private Vector3 randmonOffsetMin = Vector3.zero;

    [SerializeField] private Vector3 randomOffsetMax = Vector3.zero;

    [SerializeField] private float newOffsetChance = 0.01f;
    [SerializeField] private float randomRotation = 0f;

    [SerializeField] private float followSpeed = 3f;

    [SerializeField] private float followThreshold = 0.2f;

    [SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private float waitTillFollowDistance = 3f;

    [SerializeField] private float sineHeight = 0.2f;

    [SerializeField] private float sineSpeed = 1.3f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        // StartCoroutine(RandomOffsetCoroutine());
    }

    void FixedUpdate()
    {
        SineOffset();
        CheckState();
        // anim.SetFloat("Speed", velocity.magnitude);
    }

    void CheckState()
    {
        if (Mathf.Abs(((mate.position + CurrentOffset()) - transform.position).magnitude) < followThreshold)
        {
            RandomOffset();
        }

        Follow();
    }
        // switch (state)
        // {
        //     case FlightState.Following:
        //         if (FinishedFollowing())
        //         {
        //             // idlePosition = mate.position;
        //             state = FlightState.Idle;
        //         }
        //         else
        //         {
        //         }
        //         break;

                // case FlightState.Idle:
                //     if (!MateIsInDistance())
                //     {
                //         state = FlightState.Following;
                //     }
                //     else
                //     {
                //         Idle();
                //     }
                //     break;

        // }
    // }

    // bool FinishedFollowing()
    // {
    //     if (Mathf.Abs(((mate.position + CurrentOffset()) - transform.position).magnitude) < followThreshold)
    //     {
    //         RandomOffset();
    //     }

    //     return false;
    // }

    // bool MateIsInDistance()
    // {
    //     return Mathf.Abs((mate.position - transform.position).magnitude) < waitTillFollowDistance;
    // }

    void SineOffset()
    {
        sine = (Mathf.Sin(Time.realtimeSinceStartup * sineSpeed) * sineHeight);
    }

    IEnumerator RandomOffsetCoroutine()
    {
        float rnd = UnityEngine.Random.Range(0f, 1f);
        if (rnd < newOffsetChance)
        {
            RandomOffset();
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(RandomOffsetCoroutine());
    }

    void RandomOffset()
    {
        float rndX = UnityEngine.Random.Range(randmonOffsetMin.x, randomOffsetMax.x);
        float rndY = UnityEngine.Random.Range(randmonOffsetMin.y, randomOffsetMax.y);
        float rndZ = UnityEngine.Random.Range(randmonOffsetMin.z, randomOffsetMax.z);

        randomRotation = UnityEngine.Random.Range(0f, 360f);

        offset = new Vector3(rndX, rndY, rndZ);
    }

    Vector3 CurrentOffset()
    {
        Vector3 currentOffset = offset;
        currentOffset.y += sine;
        return currentOffset;
    }

    void Follow()
    {
        Quaternion oldRot = transform.rotation;
        transform.LookAt(mate.position + CurrentOffset() + mate.forward + velocity);
        Quaternion newRot = transform.rotation;
        transform.rotation = Quaternion.Slerp(oldRot, newRot, rotationSpeed * Time.deltaTime);

        // Vector3 destination = mate.position + CurrentOffset();
        Move(followSpeed);
        // Vector3 direction = (destination - transform.position).normalized;

        // transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, followSpeed);
        // transform.position = Vector3.Slerp(transform.position, destination, followSpeed * Time.deltaTime);
    }

    void Move(float _speed)
    {
        Vector3 destination = mate.position + CurrentOffset();
        Vector3 direction = destination - transform.position;
        velocity = Vector3.Slerp(velocity, direction, velocitySlerp * Time.deltaTime);
        velocity = Vector3.ClampMagnitude(velocity, velocityMax);
        transform.position += velocity * _speed * Time.deltaTime;


        if (transform.position.y - mate.position.y < heightMin)
        {
            velocity += Vector3.up * Time.deltaTime * heightRegulationSpeed;
        }

        if (transform.position.y - mate.position.y > heightMax)
        {
            velocity += Vector3.down * Time.deltaTime * heightRegulationSpeed;
        }
        // rb.MovePosition(transform.position + velocity * _speed * Time.deltaTime);
        // transform.position = Vector3.Slerp(transform.position, _destination, _speed * Time.deltaTime);
        anim.SetFloat("Speed", velocity.magnitude);
    }

    void Idle()
    {
        Vector3 targetRot = transform.rotation.eulerAngles;
        targetRot.y = randomRotation;
        Quaternion target = Quaternion.Euler(targetRot);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);

        // Vector3 destination = mate.position + CurrentOffset();
        Move(idleSpeed);
        // Vector3 direction = (destination - transform.position).normalized;

        // transform.position = Vector3.Slerp(transform.position, destination, idleSpeed * Time.deltaTime);

        // transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, idleSpeed);


        // Vector3 destination = Vector3.Lerp(transform.position, idlePosition + CurrentOffset() * idleSpeed, Time.deltaTime);
        // transform.position = destination;
    }


}
