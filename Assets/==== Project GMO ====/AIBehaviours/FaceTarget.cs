using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FaceTarget : Action
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

	public override TaskStatus OnUpdate()
	{
		Vector3 selfXZpos = new Vector3(transform.position.x, 0, transform.position.z);
		Vector3 targetPos = new Vector3(ai.targetPos.x, 0, ai.targetPos.z);

		Vector3 targetDir = targetPos - selfXZpos;

		Quaternion newDirection = Quaternion.LookRotation(targetDir);

		transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, 10 * Time.deltaTime);

		float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle < ai.AngleThreshold)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
	}
}