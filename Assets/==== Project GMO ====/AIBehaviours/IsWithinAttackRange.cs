using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class IsWithinAttackRange : Conditional
{
    public SharedGameObject Target;

    private Transform target;

    [SerializeField] private float attackRange;


    public override void OnStart()
    {
        base.OnStart();

        if(Target.Value != null) target = Target.Value.transform;
    }

    public override TaskStatus OnUpdate()
	{
        if(target != null)
        {
            if(Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                return TaskStatus.Success;
            }
        }

		return TaskStatus.Failure;
	}
}