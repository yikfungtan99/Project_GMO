using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;

public class Wander : Action
{

	[SerializeField] private AIPath agent;

	[SerializeField] private float wanderIntervalMin;
	[SerializeField] private float wanderIntervalMax;

	[SerializeField] private float wanderDistance;
	[SerializeField] private LayerMask wanderLayer;

	private float currentWanderTime = 0;

	private bool haveDest = false;

	public override void OnStart()
	{
		currentWanderTime = Random.Range(wanderIntervalMin, wanderIntervalMax);
	}

	public override TaskStatus OnUpdate()
	{
		if (currentWanderTime > 0)
		{
			currentWanderTime -= Time.deltaTime;
		}
		else
		{

			if (!haveDest)
            {
				if (!agent.pathPending && (agent.reachedEndOfPath || !agent.hasPath))
                {
					agent.destination = PickRandomPoint();
					agent.SearchPath();
					haveDest = true;
				}
			}
            else
            {
                if (agent.reachedDestination)
                {
					currentWanderTime = Random.Range(wanderIntervalMin, wanderIntervalMax);
					haveDest = false;
				}
            }
		}

		return TaskStatus.Running;
	}

	Vector3 PickRandomPoint()
	{
		var point = Random.insideUnitSphere * wanderDistance;

		point.y = transform.position.y;
		point += transform.position;
		return point;
	}
}