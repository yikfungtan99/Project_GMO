using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;

public class Drop : Action
{
	public SharedBehaviour sharedMovementAI;
	private EnemyMovementAI ai { get => (EnemyMovementAI)sharedMovementAI.Value; }

	private float fallRecoverDuration = 0.5f;
	private float fallRecoverTime;

	private bool jumped = false;
	private bool waitForLanding = false;

    public override void OnStart()
    {
		fallRecoverTime = fallRecoverDuration;
    }

    public override TaskStatus OnUpdate()
	{
		if (!jumped)
		{
			ai.agent.enabled = false;
			ai.rb.isKinematic = false;

			ai.rb.AddForce(transform.up * ai.DropVerticalHeight);
			ai.rb.AddForce(transform.forward * ai.DropHorizontalHeight);

			jumped = true;
		}

        if (jumped)
        {
			if(fallRecoverTime > 0)
            {
				fallRecoverTime -= Time.deltaTime;
            }
            else
            {
				waitForLanding = true;
			}
        }

        if (waitForLanding)
        {
            if (ai.onGround)
            {
				ai.agent.enabled = true;
				ai.rb.isKinematic = true;

				return TaskStatus.Success;
			}
        }

		return TaskStatus.Running;
	}

    public override void OnEnd()
    {
        jumped = false;
        waitForLanding = false;
        fallRecoverTime = fallRecoverDuration;

		ai.agent.enabled = true;
		ai.rb.isKinematic = true;

		base.OnEnd();
    }
}