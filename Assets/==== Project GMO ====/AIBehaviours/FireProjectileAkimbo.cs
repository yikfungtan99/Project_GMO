using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectileAkimbo : FireProjectile
{
    [SerializeField] private List<Transform> fireLocations = new List<Transform>();

    private int currentFireLocationIndex = 0;

    private void SwitchFireLocation()
    {
        currentFireLocationIndex += 1;

        if(currentFireLocationIndex >= fireLocations.Count)
        {
            currentFireLocationIndex = 0;
        }

        projectileFireLocation = fireLocations[currentFireLocationIndex];
    }

    protected override void SpawnProjectile()
    {
        base.SpawnProjectile();
        SwitchFireLocation();
    }
}
