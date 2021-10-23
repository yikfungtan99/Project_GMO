using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovementAI : MonoBehaviour, ILadderUser<Vector3>
{
    public Camera cam { get; protected set; }
    public NavMeshAgent agent { get; protected set; }
    public Rigidbody rb { get; protected set; }

    public bool useMouseDebug = false;

    //==============================================================Wander Variables==============================================================
    [Header("Wander Variables")]
    [SerializeField] protected float wanderIntervalMin;
    public float WanderIntervalMin { get { return wanderIntervalMin; } protected set { value = wanderIntervalMin; } }

    [SerializeField] protected float wanderIntervalMax;
    public float WanderIntervalMax { get { return wanderIntervalMax; } protected set { value = wanderIntervalMax; } }

    [SerializeField] protected float wanderDistance;
    public float WanderDistance { get { return wanderDistance; } protected set { value = wanderDistance; } }

    [SerializeField] protected LayerMask wanderLayer;
    public LayerMask WanderLayer { get { return wanderLayer; } protected set { value = wanderLayer; } }

    //==============================================================Find Target Variables==============================================================
    [Header("Find Target Variables")]
    [SerializeField] protected float fieldOfViewRange;
    [SerializeField] protected float fieldOfViewAngle;
    [SerializeField] protected LayerMask targetMask;
    [SerializeField] protected LayerMask obstacleMask;

    [SerializeField] protected float forgetTargetDelay;
    protected float currentForgetTargetTime;

    public bool allowForgetTarget { get; protected set; }
    public bool forgettingTarget { get; protected set; }

    List<GameObject> targets = new List<GameObject>();

    public Transform target;
    public Vector3 targetPos;

    //==============================================================Ladder Variables==============================================================
    [Header("Ladder Variables")]
    public float heightReachThreshold;
    [SerializeField] protected float climbSpeed;

    [SerializeField] protected float offLadderJumpHeight;
    [SerializeField] protected float offLadderJumpDuration;

    [HideInInspector] public Ladder currentLadder;
    public bool climbing;

    [SerializeField] protected float ladderDetectionRadius;
    public float LadderDetectionRadius { get { return ladderDetectionRadius; } protected set { value = ladderDetectionRadius; } }

    [SerializeField] protected float ladderDistance;
    public float LadderDistance { get { return ladderDistance; } protected set { value = ladderDistance; } }

    [SerializeField] protected LayerMask ladderLayer;
    public LayerMask LadderLayer { get { return ladderLayer; } protected set { value = ladderLayer; } }

    protected bool offingLadder = false;

    //==============================================================Drop Variables==============================================================
    
    [HideInInspector] public Vector3 dropPoint;

    [Header("Drop Variables")]
    [SerializeField] protected float angleThreshold;
    [SerializeField] protected float turnSpeed;
    
    public float TurnSpeed { get { return turnSpeed; } protected set { value = turnSpeed; } }
    public float AngleThreshold { get { return angleThreshold; } protected set { value = angleThreshold; } }
    [SerializeField] protected float depthReachThreshold;
    public float DepthReachThreshold { get { return depthReachThreshold; } protected set { value = depthReachThreshold; } }
    [SerializeField] protected float dropLedgeDistance;
    public float DropLedgeDistance { get { return dropLedgeDistance; } protected set { value = dropLedgeDistance; } }
    [SerializeField] protected float dropVerticalHeight;
    public float DropVerticalHeight { get { return dropVerticalHeight; } protected set { value = dropVerticalHeight; } }

    [SerializeField] protected float dropHorizontalHeight;
    public float DropHorizontalHeight { get { return dropHorizontalHeight; } protected set { value = dropHorizontalHeight; } }

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundDistance;

    RaycastHit groundHit;
    public bool onGround { get; protected set; }

    // Start is called before the first frame update
    protected void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        targetPos = transform.position;

        currentForgetTargetTime = forgetTargetDelay;
    }

    protected virtual void Update()
    {
        if (useMouseDebug)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);

            targetPos = hit.point;
        }
        else
        {
            FindTargets();
            if (target)
            {
                targetPos = target.position;
            }
        }

        onGround = Physics.Raycast(transform.position, -transform.up, out groundHit, groundDistance);
    }

    public void UseLadder()
    {
        if (!currentLadder || offingLadder) return;

        if (!climbing)
        {
            climbing = true;
            agent.enabled = false;
        }

        currentLadder.ClimbLadder(transform, climbSpeed, 1);
    }

    public void GetOffLadder(Vector3 offPos)
    {
        if (offingLadder) return;
        offingLadder = true;
        transform.DOJump(offPos, offLadderJumpHeight, 1, offLadderJumpDuration);
        DOVirtual.DelayedCall(offLadderJumpDuration, OffLadder);
    }

    public void OffLadder()
    {
        offingLadder = false;
        agent.enabled = true;

        climbing = false;
        currentLadder = null;
    }

    public void OffLadderMidAir()
    {
        offingLadder = false;
        climbing = false;
        currentLadder = null;
    }

    public void FindTargets()
    {
        targets.Clear();

        Collider[] targetsInView = Physics.OverlapSphere(transform.position, fieldOfViewRange, targetMask);

        for (int i = 0; i < targetsInView.Length; i++)
        {
            
            Transform target = targetsInView[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < fieldOfViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                RaycastHit hit;
                if (!Physics.Raycast (transform.position,  dirToTarget, out hit, dstToTarget, obstacleMask))
                {
                    targets.Add(target.gameObject);
                }
            }
        }

        if (targets.Count > 0)
        {
            if (forgettingTarget)
            {
                currentForgetTargetTime = forgetTargetDelay;
                forgettingTarget = false;
            }

            target = targets[0].transform;
        }
        else
        {
            if (target)
            {
                forgettingTarget = true;

                if (currentForgetTargetTime > 0)
                {
                    currentForgetTargetTime -= Time.deltaTime;
                }
                else
                {
                    ForgetTarget();
                    currentForgetTargetTime = forgetTargetDelay;
                }
            }
        }
    }

    protected void ForgetTarget()
    {
        target = null;
        forgettingTarget = false;
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool DestinationReachable()
    {
        bool reachable = false;

        Vector3 finalPath = new Vector3(agent.path.corners[agent.path.corners.Length - 1].x, 0, agent.path.corners[agent.path.corners.Length - 1].z);

        Vector3 target = new Vector3(targetPos.x, 0, targetPos.z);

        if(finalPath == target)
        {
            reachable = true;
        }

        return reachable;
    }

    public bool Reached()
    {
        bool reach = false;

        if (agent.isOnNavMesh)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        reach = true;
                    }
                }
            }
        }

        return reach;
    }

    //protected void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawWireSphere(transform.position, ladderDetectionRadius);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(targetPos, 2);

    //    Handles.color = Color.blue;
    //    Handles.DrawWireArc(transform.position, transform.up, transform.forward, 360, fieldOfViewRange);

    //    Vector3 viewAngleA = DirFromAngle(-fieldOfViewAngle / 2, false);
    //    Vector3 viewAngleB = DirFromAngle(fieldOfViewAngle / 2, false);

    //    Handles.DrawLine(transform.position, transform.position + viewAngleA * fieldOfViewRange);
    //    Handles.DrawLine(transform.position, transform.position + viewAngleB * fieldOfViewRange);
    //}
}
