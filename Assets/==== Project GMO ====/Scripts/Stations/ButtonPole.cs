using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPole : MonoBehaviour, ICanBeInteracted
{
    [SerializeField] private UnityEvent activateEvent;
    [SerializeField] private Animator buttonAnimation;

    private float lookPercentage;
    public float LookPercentage { get => lookPercentage; set => lookPercentage = value; }

    // Start is called before the first frame update
    void Start()
    {
        SetInteractable();
    }

    public void SetInteractable()
    {
        SelectionManager.Instance.AddInteractables(this);
    }

    public void ReceiveInteract(PlayerInteract interactor = null)
    {
        if(buttonAnimation != null) buttonAnimation.Play("ButtonPole");
        activateEvent.Invoke();
    }

    public void HighlightInteractable()
    {
        GetComponent<Outline>().enabled = true;
    }

    public void DeHighlightInteractable()
    {
        GetComponent<Outline>().enabled = false;
    }
}
