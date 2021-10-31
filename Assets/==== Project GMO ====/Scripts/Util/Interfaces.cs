using System;
using UnityEngine;

public interface ICanBeAttack
{
    void ReceiveAttack();
}

public interface ICanBeDamage
{
    void ReceiveDamage(Damage damage);
}

public interface ICanPickUpItems
{
    void PickUpItem(Pickups pickup);
}

public interface ICanBeInteracted
{
    void ReceiveInteract(PlayerInteract interactor);
}

public interface ICanbeGrabbed
{
    void ReceiveBeginGrab(Transform grabPos);
    void ReceiveEndGrab();
}