using System;
using UnityEngine;

public interface ICanBeAttack
{
    void ReceiveAttack();
}

public interface ICanBeDamage
{

    delegate void DamageCallback(int damageAmount = 0);

    event DamageCallback OnReceivedDamage;

    void ReceiveDamage(Damage damage);
}

public interface ICanPickUpItems
{
    void PickUpItem(Pickups pickup);
}

public interface ICanBeInteracted
{
    Transform transform { get; }
    void HighlightInteractable();
    void DeHighlightInteractable();
    void SetInteractable();
    void ReceiveInteract(PlayerInteract interactor);
}

public interface ICanbeGrabbed
{
    void ReceiveBeginGrab(Transform grabPos);
    void ReceiveEndGrab();
}