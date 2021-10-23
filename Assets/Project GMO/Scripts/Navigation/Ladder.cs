using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILadderUser<Vector3>
{
    void UseLadder();
    void GetOffLadder(Vector3 offPos);
}

public class Ladder : MonoBehaviour
{
    [SerializeField] private Transform ladderTop;
    public Transform LadderTop { get { return ladderTop; } private set { value = ladderTop; } }

    [SerializeField] private Transform ladderBottom;
    public Transform LadderBottom { get { return ladderBottom; } private set { value = ladderBottom; } }

    [SerializeField] private Transform ladderSide;
    public Transform LadderSide { get { return ladderSide; } private set { value = ladderSide; } }

    [SerializeField] private Transform ladderTopDetection;
    [SerializeField] private Transform ladderBottomDetection;

    /// <summary>
    /// Call every frame to go up or down the ladder.
    /// </summary>
    /// 
    public void ClimbLadder(Transform ladderUser, float climbSpeed, int direction)
    {

        if (ladderUser.transform.position.y > ladderTopDetection.position.y)
        {
            Vector3 topPos = ladderTop.position;
            ladderUser.GetComponent<ILadderUser<Vector3>>().GetOffLadder(topPos);
        }

        if (ladderUser.transform.position.y < ladderBottomDetection.position.y)
        {
            Vector3 btmPos = ladderBottom.position;
            ladderUser.GetComponent<ILadderUser<Vector3>>().GetOffLadder(btmPos);
        }

        float spd = 0;

        if (direction > 0)
        {
            spd = climbSpeed;
        }
        else if (direction < 0)
        {
            spd = -climbSpeed;
        }

        ladderUser.position = new Vector3(ladderSide.position.x, ladderUser.position.y + spd * Time.deltaTime, ladderSide.position.z);
        ladderUser.rotation = ladderSide.rotation;

    }
}
