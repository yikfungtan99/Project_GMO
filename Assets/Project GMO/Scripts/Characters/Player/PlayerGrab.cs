using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] private Transform playerAim;
    [SerializeField] private float interactDistance;

    [SerializeField] private LayerMask playerGrabLayer;
    [SerializeField] private Transform playerGrabPos;

    RaycastHit hit;
    GameObject hitObject = null;

    // Update is called once per frame
    void Update()
    {

        if(Physics.RaycastAll(playerAim.position, playerAim.forward, interactDistance,  playerGrabLayer, QueryTriggerInteraction.Collide).Length > 0)
        {
            print(Physics.RaycastAll(playerAim.position, playerAim.forward, interactDistance, playerGrabLayer, QueryTriggerInteraction.Collide)[0].collider.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.RaycastAll(playerAim.position, playerAim.forward, interactDistance, playerGrabLayer, QueryTriggerInteraction.Collide).Length > 0)
            {
                hitObject = Physics.RaycastAll(playerAim.position, playerAim.forward, interactDistance, playerGrabLayer, QueryTriggerInteraction.Collide)[0].collider.gameObject;
                print(hitObject.name);

                if (hitObject.GetComponent<ICanbeGrabbed>() != null)
                {
                    print(hitObject.gameObject.name + " grabbed.");
                    hitObject.GetComponent<ICanbeGrabbed>().ReceiveBeginGrab(playerGrabPos);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (hitObject == null) return;
            print(hitObject.gameObject.name + "released.");
            hitObject.GetComponent<ICanbeGrabbed>().ReceiveEndGrab();
            hitObject = null;
        }
    }
}
