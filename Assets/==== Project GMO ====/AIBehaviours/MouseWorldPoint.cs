using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("BadChefAI")]
public class MouseWorldPoint : Conditional
{
	
	public SharedVector3 mouseWorldPos;

	private EnemyMovementAI ai;
	private Camera cam;

	public override void OnStart()
	{
		ai = GetComponent<EnemyMovementAI>();
		cam = ai.cam;
	}

	public override TaskStatus OnUpdate()
	{

		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		Physics.Raycast(ray, out hit);

		mouseWorldPos.Value = hit.point;
		return TaskStatus.Running;
	}
}