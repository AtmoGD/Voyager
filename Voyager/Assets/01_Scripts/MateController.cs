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
    Vector3 idlePosition = Vector3.zero;
    float sine = 0f;

    [SerializeField]
    private Transform mate = null;

    [SerializeField]
    private Vector3 offset = Vector3.zero;

    [SerializeField]
    private Vector3 randmonOffsetMin = Vector3.zero;

    [SerializeField]
    private Vector3 randomOffsetMax = Vector3.zero;

    [SerializeField]
    private float newOffsetChance = 0.01f;

    [SerializeField]
    private float followSpeed = 3f;

    [SerializeField]
    private float followThreshold = 0.2f;

    [SerializeField]
    private float rotationSpeed = 1f;

    [SerializeField]
    private float waitTillFollowDistance = 3f;

    [SerializeField]
    private float sineHeight = 0.2f;

    [SerializeField]
    private float sineSpeed = 1.3f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        StartCoroutine(RandomOffset());
    }

    void Update()
    {
        SineOffset();
        CheckState();
        anim.SetFloat("Speed", velocity.magnitude);
    }

    void CheckState()
    {
        switch (state)
        {
            case FlightState.Following:
                if (FinishedFollowing())
                {
                    idlePosition = mate.position;
                    state = FlightState.Idle;
                }
                else
                {
                    Follow();
                }
                break;

            case FlightState.Idle:
                if (!MateIsInDistance())
                {
                    state = FlightState.Following;
                }
                else
                {
                    Idle();
                }
                break;

        }
    }

    bool FinishedFollowing()
    {
        return ((mate.position + CurrentOffset()) - transform.position).magnitude < followThreshold;
    }

    bool MateIsInDistance()
    {
        return ((mate.position + CurrentOffset()) - transform.position).magnitude < waitTillFollowDistance;
    }

    void SineOffset() {
        sine = (Mathf.Sin(Time.realtimeSinceStartup * sineSpeed) * sineHeight);
    }

    IEnumerator RandomOffset() {
        float rnd = UnityEngine.Random.Range(0f, 1f);
        if(rnd < newOffsetChance) {
            float rndX = UnityEngine.Random.Range(randmonOffsetMin.x, randomOffsetMax.x);
            float rndY = UnityEngine.Random.Range(randmonOffsetMin.y, randomOffsetMax.y);
            float rndZ = UnityEngine.Random.Range(randmonOffsetMin.z, randomOffsetMax.z);

            offset = new Vector3(rndX, rndY, rndZ);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(RandomOffset());
    }

    Vector3 CurrentOffset() {
        Vector3 currentOffset = offset;
        currentOffset.y += sine;
        return currentOffset;
    }

    void Follow()
    {
        Quaternion oldRot = transform.rotation;
        transform.LookAt(mate.position + CurrentOffset() + mate.forward);
        Quaternion newRot = transform.rotation;
        transform.rotation = Quaternion.Lerp(oldRot, newRot, rotationSpeed * Time.deltaTime);

        Vector3 destination = mate.position + CurrentOffset();
        Vector3 direction = (destination - transform.position).normalized;

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, followSpeed);
    }

    void Idle()
    {
        Vector3 destination = Vector3.Lerp(transform.position, idlePosition + CurrentOffset(), Time.deltaTime);
        transform.position = destination;
    }


}
