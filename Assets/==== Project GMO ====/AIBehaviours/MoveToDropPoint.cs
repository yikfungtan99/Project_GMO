using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class MoveToDropPoint : Action
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

    NavMeshHit hit;

	public override TaskStatus OnUpdate()
	{
        if (ai.agent.isActiveAndEnabled)
        {
            ai.agent.destination = new Vector3(ai.targetPos.x, transform.position.y, ai.targetPos.z);

            if (!ai.agent.reachedDestination)
            {
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Success;
            }
        }
        else
        {
			return TaskStatus.Success;
        }
	}
}