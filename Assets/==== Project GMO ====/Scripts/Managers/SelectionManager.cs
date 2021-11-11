using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private float autoSelectLookThreshold;
    [SerializeField] private float selectRange;

    private static SelectionManager _instance;

    private ICanBeInteracted selection;

    public static SelectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SelectionManager>();
            }

            return _instance;
        }
    }

    public List<ICanBeInteracted> interactables = new List<ICanBeInteracted>();

    public void CheckInteractable(Vector3 position, Ray ray)
    {
        if (selection != null)
        {
            selection.DeHighlightInteractable();
            selection = null;
        }

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, selectRange))
        {
            if(hit.collider.GetComponent<ICanBeInteracted>() != null)
            {
                SetSelection(hit.collider.GetComponent<ICanBeInteracted>());
            }
        }
        
        if(selection == null)
        {
            AutoSelectNearest(position, ray);
        }
    }

    private void AutoSelectNearest(Vector3 position, Ray ray)
    {
        float closest = 0f;

        for (int i = 0; i < interactables.Count; i++)
        {
            var vector1 = ray.direction;
            var vector2 = interactables[i].transform.position - ray.origin;

            var lookPercentage = Vector3.Dot(vector1.normalized, vector2.normalized);

            if ((1 - lookPercentage) <= autoSelectLookThreshold && lookPercentage > closest)
            {
                closest = lookPercentage;
                float currentDistance = Vector3.Distance(position, interactables[i].transform.position);

                if (currentDistance <= selectRange)
                {
                    if (selection != null)
                    {
                        selection.DeHighlightInteractable();
                        selection = null;
                    }

                    SetSelection(interactables[i]);
                }
            }
        }
    }

    public void AddInteractables(ICanBeInteracted interactable)
    {
        interactables.Add(interactable);
    }

    public void Select(PlayerInteract interactor)
    {
        if(selection != null)
        {
            selection.ReceiveInteract(interactor);
        }
    }

    private void SetSelection(ICanBeInteracted interactable)
    {
        selection = interactable;
        interactable.HighlightInteractable();
    }
}
