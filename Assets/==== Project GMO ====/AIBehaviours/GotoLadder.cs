using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;

public class GotoLadder : Action
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }
	private AIPath agent;

	public override void OnStart()
	{
		agent = ai.agent;
	}

	public override TaskStatus OnUpdate()
	{ 
		if (ai.currentLadder != null)
        {
			Vector3 ladderXZpos = new Vector3(ai.currentLadder.transform.position.x, transform.position.y, ai.currentLadder.transform.position.z);

            if (!agent.enabled)
            {
				agent.enabled = true;
            }

			agent.destination = ladderXZpos;

			if (agent.reachedDestination)
            {
				return TaskStatus.Success;
			}
        }

		return TaskStatus.Running;
	}
}