using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ReachTargetPos : Conditional
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

	public override TaskStatus OnUpdate()
	{
		if (!ai.Reached())
		{
			return TaskStatus.Failure;
		}
		else
		{
			return TaskStatus.Success;
		}
	}
}