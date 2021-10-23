using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TargetPosBelow : Conditional
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

	public override TaskStatus OnUpdate()
	{
		if(transform.position.y > ai.targetPos.y)
        {
			if (transform.position.y - ai.targetPos.y > ai.DepthReachThreshold)
			{
				if (ai.climbing)
				{
					ai.OffLadderMidAir();
				}

				return TaskStatus.Success;
			}
		}

		return TaskStatus.Failure;
	}
}