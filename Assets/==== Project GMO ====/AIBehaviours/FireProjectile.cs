using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Pathfinding;

[TaskCategory("BadChefAI")]
public class FireProjectile : Action
{
	public SharedGameObject target;
	[SerializeField] private AIPath agent;

	[SerializeField] private float angleThreshold;

	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform projectileFireLocation;

	[SerializeField] private float projectileHorizontalForce;
	[SerializeField] private float projectileVerticalForce;

	[SerializeField] private float attackInterval;
	private float attackTime;

	private Transform targetTransform;

    public override void OnAwake()
    {
		base.OnAwake(); 
		attackTime = 0;
	}

    public override void OnStart()
    {
        base.OnStart();

        if (target.Value != null)
        {
			targetTransform = target.Value.transform;
		}

		agent.canMove = false;
    }

    public override TaskStatus OnUpdate()
	{
		Vector3 selfXZpos = new Vector3(transform.position.x, 0, transform.position.z);
		Vector3 targetPos = new Vector3(targetTransform.position.x, 0, targetTransform.position.z);

		Vector3 targetDir = targetPos - selfXZpos;

		Quaternion newDirection = Quaternion.LookRotation(targetDir);

		transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, 10 * Time.deltaTime);

		float angle = Vector3.Angle(targetDir, transform.forward);

		if (attackTime > 0)
		{
			attackTime -= Time.deltaTime;
		}
		else
		{
			if (angle < angleThreshold)
			{
				GameObject projectile = GameObject.Instantiate(projectilePrefab, projectileFireLocation.position, Quaternion.identity);
				projectile.GetComponent<Projectile>().SetDamage(new Damage(1, null));
				Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
				projectileRb.AddForce(projectileFireLocation.forward * projectileHorizontalForce);
				projectileRb.AddForce(projectileFireLocation.up * projectileVerticalForce);
			}

			attackTime = attackInterval;
		}

		return TaskStatus.Running;
	}

    public override void OnEnd()
    {
        base.OnEnd();
		agent.canMove = true;
    }
}