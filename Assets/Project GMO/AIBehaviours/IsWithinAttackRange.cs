using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class IsWithinAttackRange : Conditional
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI movementAI { get => (EnemyMovementAI)sharedMovementAI.Value; }

    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask obstacleLayer;

	public override TaskStatus OnUpdate()
	{
        if (movementAI.agent.enabled && movementAI.onGround && !movementAI.climbing && CanSeeTarget())
        {
			return TaskStatus.Success;
        }

		return TaskStatus.Failure;
	}

    public bool CanSeeTarget()
    {
        bool inRange = false;

        if (movementAI.target)
        {
            if (Vector3.Distance(transform.position, movementAI.target.position) < attackRange)
            {
                float dstToTarget = Vector3.Distance(transform.position, movementAI.target.position);
                Vector3 dirToTarget = (movementAI.target.position - transform.position).normalized;

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleLayer))
                {
                    inRange = true;
                }
            }
        }

        return inRange;
    }
}