using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class Wander : Action
{

	[SerializeField] private NavMeshAgent agent;

	[SerializeField] private float wanderIntervalMin;
	[SerializeField] private float wanderIntervalMax;

	[SerializeField] private float wanderDistance;
	[SerializeField] private LayerMask wanderLayer;

	private bool haveDest = false;
	private float currentWanderTime = 0;

	public override void OnStart()
	{
		currentWanderTime = Random.Range(wanderIntervalMin, wanderIntervalMax);
	}

	public override TaskStatus OnUpdate()
	{
        if (!haveDest)
        {
			if (currentWanderTime > 0)
			{
				currentWanderTime -= Time.deltaTime;
			}
			else
			{
				haveDest = true;
				agent.SetDestination(RandomNavSphere(transform.position, wanderDistance, wanderLayer));
			}
        }
        else
        {
			if (Reached())
            {
				haveDest = false;
				currentWanderTime = Random.Range(wanderIntervalMin, wanderIntervalMax);
			}
        }

		return TaskStatus.Running;
	}

	public bool Reached()
    {
        bool reach = false;

        if (agent.isOnNavMesh)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        reach = true;
                    }
                }
            }
        }

        return reach;
    }

	public Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
	{
		Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

		randomDirection += origin;

		NavMeshHit navHit;

		NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

		return navHit.position;
	}
}