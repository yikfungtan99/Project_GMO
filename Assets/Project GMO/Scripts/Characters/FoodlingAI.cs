using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoodlingAI : MonoBehaviour, ICanbeGrabbed
{
    private bool grabbed;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float nauseaMeter = 0;
    [SerializeField] private float nauseaThreshold;

    [SerializeField] private float nauseaRecoverSpeed;

    [SerializeField] private float dropIntervalMin;
    [SerializeField] private float dropIntervalMax;
    [SerializeField] private float dropTime;

    [SerializeField] private ItemObject dropItem;
    [SerializeField] private Dispenser dispenser;

    [SerializeField] private float grabSpeed;

    public bool IsNausea { get; private set; }

    float currentPosYChanges;
    private float prevPosY = 0;

    private Transform grabbedPos;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ReceiveBeginGrab(Transform grabPos)
    {
        grabbed = true;
        prevPosY = transform.position.y;
        agent.enabled = false;
        rb.isKinematic = true;
        grabbedPos = grabPos;
    }

    public void ReceiveEndGrab()
    {
        grabbed = false;
        prevPosY = 0;
        agent.enabled = true;
        rb.isKinematic = false;
        grabbedPos = null;
    }

    // Update is called once per frame
    public void Update()
    {
        if (grabbed)
        {
            transform.position = Vector3.Lerp(transform.position, grabbedPos.position, grabSpeed * Time.deltaTime);

            if (!IsNausea)
            {
                if (nauseaMeter < nauseaThreshold)
                {
                    nauseaMeter += currentPosYChanges;
                }
                else
                {
                    nauseaMeter = nauseaThreshold;
                    IsNausea = true;
                }

                currentPosYChanges = Mathf.Abs(transform.position.y - prevPosY);
                prevPosY = transform.position.y;
            }
        }

        if (nauseaMeter > 0)
        {
            nauseaMeter -= nauseaRecoverSpeed * Time.deltaTime;
        }
        else
        {
            dropTime = 0;
            nauseaMeter = 0;
            IsNausea = false;
        }

        if (IsNausea)
        {
            if (dropTime > 0)
            {
                dropTime -= Time.deltaTime;
            }
            else
            {
                dispenser.Dispense(dropItem);
                dropTime = Random.Range(dropIntervalMin, dropIntervalMax);
            }
        }

       
    }
}
