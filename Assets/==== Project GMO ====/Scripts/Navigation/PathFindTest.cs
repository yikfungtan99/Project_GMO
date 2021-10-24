using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFindTest : MonoBehaviour
{
    NavMeshAgent nav;

    [SerializeField] private Transform aiModel;

    [SerializeField] private float groundCheckDistance = 1.5f;

    [SerializeField] private float jumpPointMinDistance;
    [SerializeField] private float jumpHorizontalForce;
    [SerializeField] private float jumpVerticalForce;

    private Rigidbody rb;
    private bool onGround = false;

    //Climb Variables
    [SerializeField] private float heightClimbThreshold;
    [SerializeField] private float climbPointDetectionRadius;
    [SerializeField] private float climbWallDistance;
    [SerializeField] private LayerMask climbPointLayer;

    private Vector3 finalDestination;
    private Vector3 currentDestination;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        nav = GetComponent<NavMeshAgent>();
        nav.updateRotation = false;

        cam = Camera.main;
    }

    private void Update()
    {
        GroundCheck();
        SetFinalDestination();
        SetDestination();
        MoveNavAgent();
    }

    private void MoveNavAgent()
    {
        nav.SetDestination(currentDestination);
    }

    private void SetDestination()
    {
        if(finalDestination.y - transform.position.y > heightClimbThreshold)
        {
            Transform climbPoint;

            if (DetectClimbPoint(out climbPoint))
            {
                currentDestination = climbPoint.position;
            }
        }
        else
        {
            currentDestination = finalDestination;
        }
    }

    private void Climb()
    {
        print(CheckWallInFront());
    }

    private bool CheckWallInFront()
    {
        bool wallInFront = false;

        Ray wallRay = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(wallRay, climbWallDistance))
        {
            wallInFront = true;
        }

        return wallInFront;
    }

    private bool DetectClimbPoint(out Transform point)
    {
        Transform climbPoint = null;
        Collider[] climbPoints = Physics.OverlapSphere(aiModel.position, climbPointDetectionRadius, climbPointLayer);

        foreach (Collider target in climbPoints)
        {
            float lastDistance = Mathf.Infinity;

            float currentDistance = Vector2.Distance(target.transform.position, transform.position);

            if (lastDistance > currentDistance)
            {
                lastDistance = currentDistance;
                climbPoint = target.transform;
            }
        }

        point = climbPoint;

        return climbPoint != null;
    }

    private void LateUpdate()
    {
        SetRotationToFaceDestination();
    }

    private void SetRotationToFaceDestination()
    {
        if (nav.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            aiModel.rotation = Quaternion.LookRotation(nav.velocity.normalized);
        }
    }

    private void SetFinalDestination()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            if (!nav.isOnNavMesh) return;
            finalDestination = hit.point;
        }
    }

    private void GroundCheck()
    {
        Ray groundRay = new Ray(transform.position, -transform.up);

        onGround = (Physics.Raycast(groundRay, groundCheckDistance));
    }

    void Jump()
    {
        if (onGround)
        {
            StartCoroutine(RecoverJumpOnLanding());
            rb.AddForce(transform.forward * jumpHorizontalForce);
            rb.AddForce(transform.up * jumpVerticalForce);
        }
    }

    IEnumerator RecoverJumpOnLanding()
    {
        ToggleNavAgent();
        yield return new WaitForSeconds(0.3f);
        
        yield return new WaitUntil(() => onGround);
        ToggleNavAgent();
    }

    void ToggleNavAgent()
    {
        nav.enabled = !nav.enabled;
        rb.isKinematic = !rb.isKinematic;
        rb.ResetInertiaTensor();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(aiModel.position, climbPointDetectionRadius);
    }
}
