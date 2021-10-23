using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

[TaskCategory("BadChefAI")]
[TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]
public class ChaseTarget : Action
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI) sharedMovementAI.Value; }
	private NavMeshAgent agent;

	public override void OnStart()
	{
		agent = ai.agent;
	}

	public override TaskStatus OnUpdate()
	{
		agent.SetDestination(ai.targetPos);

        if (ai.Reached())
        {
			return TaskStatus.Success;
        }

		return TaskStatus.Running;
	}
}