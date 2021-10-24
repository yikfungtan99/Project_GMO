using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class UseLadder : Action
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

	bool firstClimb = true;
	public override TaskStatus OnUpdate()
	{

        if (firstClimb)
        {
			ai.UseLadder();
			firstClimb = false;
        }

		if (!ai.climbing)
        {
			firstClimb = true;
			return TaskStatus.Success;
        }
        else
        {
			ai.UseLadder();
			return TaskStatus.Running;
		}
	}
}