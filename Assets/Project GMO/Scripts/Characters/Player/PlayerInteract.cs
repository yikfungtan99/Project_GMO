using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform playerAim;
    [SerializeField] private float interactDistance;

    RaycastHit hit;

    GameObject hitObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(playerAim.position, playerAim.forward, out hit, interactDistance))
            {
                hitObject = hit.collider.gameObject;

                if (hitObject.GetComponent<ICanBeInteracted>() != null)
                {
                    print(hitObject.gameObject.name + " interacted.");
                    hitObject.GetComponent<ICanBeInteracted>().ReceiveInteract();
                }
            }
        }
    }
}
