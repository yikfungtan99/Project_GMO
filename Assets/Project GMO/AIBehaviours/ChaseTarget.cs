
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;

[TaskCategory("BadChefAI")]
[TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]
public class ChaseTarget : Action
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI) sharedMovementAI.Value; }
	private AIPath agent;

	public override void OnStart()
	{
		agent = ai.agent;
	}

	public override TaskStatus OnUpdate()
	{
		agent.destination = ai.targetPos;

        if (agent.reachedDestination)
        {
			return TaskStatus.Success;
        }

		return TaskStatus.Running;
	}
}