using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TargetPosAbove : Conditional
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

	public override TaskStatus OnUpdate()
	{
		if(ai.targetPos.y > transform.position.y)
        {
			if (ai.targetPos.y - transform.position.y > ai.heightReachThreshold)
			{
				return TaskStatus.Success;
			}
		}
		
		return TaskStatus.Failure;
	}
}