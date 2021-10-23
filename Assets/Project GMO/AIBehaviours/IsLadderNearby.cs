using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsLadderNearby : Conditional
{
    public SharedBehaviour sharedMovementAI;
    private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

    public override TaskStatus OnUpdate()
	{
        if(GetNearestLadder(out ai.currentLadder))
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
	}

    private bool GetNearestLadder(out Ladder ladder)
    {
        ladder = null;
        bool haveLadder = false;

        Collider[] nearbyLadder = Physics.OverlapSphere(transform.position, ai.LadderDetectionRadius, ai.LadderLayer);

        float nearestDistance = Mathf.Infinity;

        Vector3 XZpos = new Vector3(transform.position.x, 0, transform.position.z);

        for (int i = 0; i < nearbyLadder.Length; i++)
        {
            Vector3 ladderXZpos = new Vector3(nearbyLadder[i].transform.position.x, 0, nearbyLadder[i].transform.position.z);
            float currentLadderDistance = Vector3.Distance(XZpos, ladderXZpos);

            if (currentLadderDistance < nearestDistance)
            {
                ladder = nearbyLadder[i].GetComponent<Ladder>();
                nearestDistance = currentLadderDistance;
            }
        }

        haveLadder = ladder != null;

        return haveLadder;
    }
}