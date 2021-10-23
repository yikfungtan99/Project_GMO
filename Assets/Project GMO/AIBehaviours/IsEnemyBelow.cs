using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class IsEnemyBelow : Conditional
{
    public SharedBehaviour sharedMovementAI;
    private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }
    public override TaskStatus OnUpdate()
    {
        if (ai.transform.position.y - ai.targetPos.y > ai.DepthReachThreshold)
        {
            ai.dropPoint = ai.agent.destination;
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}