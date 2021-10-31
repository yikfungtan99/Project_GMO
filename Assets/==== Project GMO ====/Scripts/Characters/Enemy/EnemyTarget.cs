using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{

    [Tooltip("The location of the vision cone origin")]
    [SerializeField] private Transform eyeTransform;

    [Tooltip("The layer of the object that we are searching for")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [Tooltip("The field of view angle of the agent (in degrees)")]
    [SerializeField] private float fieldOfViewAngle = 90;

    [Tooltip("The distance that the agent can see")]
    [SerializeField] private float viewDistance = 10;

    [SerializeField] private GameObject objectiveTarget;
    private GameObject visionTarget;

    public GameObject target { get; private set; }

    [SerializeField] private float forgetTargetDuration;
    private float forgetTargetTime;

    private void Start()
    {
        target = objectiveTarget;
        forgetTargetTime = forgetTargetDuration;
    }

    // Update is called once per frame
    void Update()
    {
        visionTarget = WithinSight(fieldOfViewAngle, viewDistance);

        if (visionTarget)
        {
            target = visionTarget;
            forgetTargetTime = forgetTargetDuration;
        }
        else
        {
            if (target != objectiveTarget)
            {
                if(forgetTargetTime > 0)
                {
                    forgetTargetTime -= Time.deltaTime;
                }
                else
                {
                    target = objectiveTarget;
                    forgetTargetTime = forgetTargetDuration;
                }
            }
        }
    }

    private GameObject WithinSight(float fieldOfViewAngle, float viewDistance)
    {
        List<GameObject> targets = new List<GameObject>();

        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewDistance, targetLayer);

        for (int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < fieldOfViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                RaycastHit hit;
                if (!Physics.Raycast(transform.position, dirToTarget, out hit, dstToTarget, obstacleLayer))
                {
                    targets.Add(target.gameObject);
                }
            }
        }

        if(targets.Count <= 0)
        {
            return null;
        }

        return targets[0];
    }

    private bool LineOfSight(GameObject targetObject)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, targetObject.transform.position, out hit))
        {
            if (hit.transform.IsChildOf(targetObject.transform) || targetObject.transform.IsChildOf(hit.transform))
            {
                return true;
            }
        }
        return false;
    }

    public void SetObjective(GameObject target)
    {
        objectiveTarget = target;
    }

    /// <summary>
    /// Draws the line of sight representation
    /// </summary>
    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var oldColor = UnityEditor.Handles.color;
        var color = Color.yellow;
        color.a = 0.1f;
        UnityEditor.Handles.color = color;

        if (eyeTransform)
        {
            var halfFOV = fieldOfViewAngle * 0.5f;
            var beginDirection = Quaternion.AngleAxis(-halfFOV, Vector3.up) * eyeTransform.forward;
            UnityEditor.Handles.DrawSolidArc(eyeTransform.position, eyeTransform.up, beginDirection, fieldOfViewAngle, viewDistance);

            UnityEditor.Handles.color = oldColor;
        }
#endif
    }
}
