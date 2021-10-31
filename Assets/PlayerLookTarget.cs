using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookTarget : MonoBehaviour
{
    [SerializeField] private Transform playerAim;
    [SerializeField] private float detectDistance;

    public IHaveHealth currentTargetHealth { get; private set; }
    public IHaveInfoName currentTargetInfoName { get; private set; }
    RaycastHit hit;
    private GameObject hitObject;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(playerAim.position, playerAim.forward, out hit, detectDistance))
        {
            if (hit.collider.transform.parent == transform) return;

            if(hitObject != hit.collider.gameObject)
            {
                hitObject = hit.collider.gameObject;

                if (hitObject.GetComponentInParent<IHaveHealth>() != null)
                {
                    currentTargetHealth = hitObject.GetComponentInParent<IHaveHealth>();
                }

                if (hitObject.GetComponentInParent<IHaveInfoName>() != null)
                {
                    currentTargetInfoName = hitObject.GetComponentInParent<IHaveInfoName>();
                }
            }
        }
        else
        {
            if (hitObject != null)
            {
                hitObject = null;

                currentTargetHealth = null;
                currentTargetInfoName = null;
            }
        }
    }
}
