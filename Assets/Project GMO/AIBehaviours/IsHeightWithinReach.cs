using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsHeightWithinReach : Conditional
{
    public SharedBehaviour sharedMovementAI;
    private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

    public override TaskStatus OnUpdate()
	{
		if(ai.targetPos.y - ai.transform.position.y < ai.heightReachThreshold)
        {
			return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
	}
}