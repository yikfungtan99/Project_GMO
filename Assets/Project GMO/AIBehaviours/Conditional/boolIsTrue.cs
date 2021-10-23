using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class boolIsTrue : Conditional
{
	[SerializeField] private SharedBool boolToCheck;

	public override TaskStatus OnUpdate()
	{
        if (boolToCheck.Value)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
	}
}