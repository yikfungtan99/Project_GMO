using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Fall : Action
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

	public override TaskStatus OnUpdate()
	{
        if (ai.onGround)
		{
			ai.rb.isKinematic = true;
			ai.agent.enabled = true;
			return TaskStatus.Success;
		}

		return TaskStatus.Running;
	}
}