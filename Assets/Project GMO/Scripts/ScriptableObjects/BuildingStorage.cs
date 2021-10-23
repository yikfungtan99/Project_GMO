using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuildingContainer
{
    public Building building;
    public float buildChance;
}

[CreateAssetMenu(fileName = "BuildingStorage", menuName = "BadChef/BuildingStorage", order = 1)]
public class BuildingStorage : ScriptableObject
{
    public List<BuildingContainer> tier1Buildings;
    public List<BuildingContainer> tier2Buildings;
    public List<BuildingContainer> tier3Buildings;
}
