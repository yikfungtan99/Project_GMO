using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform playerAim;
    [SerializeField] private float interactDistance;

    RaycastHit hit;

    GameObject hitObject = null;

    private SelectionManager selectionManager;
    Ray ray = new Ray();

    private void Start()
    {
        selectionManager = SelectionManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = playerAim.position;
        ray.direction = playerAim.forward;

        selectionManager.CheckInteractable(transform.position, ray);

        if (Input.GetKeyDown(KeyCode.E))
        {
            selectionManager.Select(this);
        }
    }
}
