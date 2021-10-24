
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;
using UnityEngine;

[TaskCategory("BadChefAI")]
[TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]
public class ChaseTarget : Action
{
	[SerializeField] private AIPath agent;
	public SharedGameObject Target;

    private Transform target;

    public override TaskStatus OnUpdate()
	{
        if (Target.Value != null) target = Target.Value.transform;

        if (target) agent.destination = target.position;

        if (agent.reachedDestination)
        {
			return TaskStatus.Success;
        }

		return TaskStatus.Running;
	}
}