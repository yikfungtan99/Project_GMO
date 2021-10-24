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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(playerAim.position, playerAim.forward, out hit, interactDistance, playerGrabLayer))
            {
                hitObject = hit.collider.gameObject;
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
