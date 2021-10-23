using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class HaveTarget : Conditional
{

    public SharedBehaviour sharedMovementAI;
    private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

    public override TaskStatus OnUpdate()
	{
        if(ai.target == null && !ai.forgettingTarget && !ai.useMouseDebug)
        {
            return TaskStatus.Failure;
        }

        return TaskStatus.Success;
	}
}