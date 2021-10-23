using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("BadChefAI")]
public class FireProjectile : Action
{
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform projectileFireLocation;

	[SerializeField] private float projectileHorizontalForce;
	[SerializeField] private float projectileVerticalForce;

	[SerializeField] private float attackInterval;
	private float attackTime;

	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

    public override void OnAwake()
    {
		base.OnAwake(); 
		attackTime = 0;
	}

	public override TaskStatus OnUpdate()
	{
		ai.agent.enabled = false;

		Vector3 selfXZpos = new Vector3(transform.position.x, 0, transform.position.z);
		Vector3 targetPos = new Vector3(ai.targetPos.x, 0, ai.targetPos.z);

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
			if (angle < ai.AngleThreshold)
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
		ai.agent.enabled = true;
    }
}