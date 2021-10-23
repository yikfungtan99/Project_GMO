using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class NotOnNavMesh : Conditional
{
    public SharedBehaviour sharedMovementAI;
    private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

    public override TaskStatus OnUpdate()
	{
        if (!ai.agent.isOnNavMesh)
        {
            return TaskStatus.Success;
        }

		return TaskStatus.Failure;
	}
}