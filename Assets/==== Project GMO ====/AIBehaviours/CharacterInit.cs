using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CharacterInit : Conditional
{

    public SharedBehaviour ai;

    public override void OnAwake()
    {
        base.OnAwake();
        ai = (SharedBehaviour)GetComponent<EnemyMovementAI>();
    }

    public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}