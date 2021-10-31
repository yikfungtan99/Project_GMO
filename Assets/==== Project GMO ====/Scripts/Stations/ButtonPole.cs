using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPole : MonoBehaviour, ICanBeInteracted
{
    [SerializeField] private UnityEvent activateEvent;
    [SerializeField] private Animator buttonAnimation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveInteract(PlayerInteract interactor = null)
    {
        buttonAnimation.Play("ButtonPole");
        activateEvent.Invoke();
    }
}
