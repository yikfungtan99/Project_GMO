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

		//GoToRandomPoint();
		//haveDest = true;
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
                bool isAlreadyOnPath = !agent.pathPending && (agent.reachedEndOfPath || !agent.hasPath);

                if (isAlreadyOnPath)
                {
                    GoToRandomPoint();
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

    private void GoToRandomPoint()
    {
        Vector3 randomPoint = PickRandomPoint();

        GraphNode currentNode = AstarPath.active.GetNearest(transform.position).node;
        GraphNode destNode = AstarPath.active.GetNearest(randomPoint).node;

        if (PathUtilities.IsPathPossible(currentNode, destNode))
        {
            agent.destination = (Vector3)destNode.position;
            haveDest = true;
        }
    }

    Vector3 PickRandomPoint()
	{
		var point = Random.insideUnitSphere * wanderDistance;

		point.y = transform.position.y;
		point += transform.position;
		return point;
	}
}