using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildDirection
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public class Building : MonoBehaviour
{
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeY;

    public Vector2Int size { get => new Vector2Int(sizeX, sizeY); }

    public BuildDirection dir;
}
