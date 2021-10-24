using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PropContainer
{
    public List<Transform> spawnPoints;
    public List<GameObject> props;
}

public class MultiStoryBuilding : Building
{
    [Header("Multi Story")]
    [SerializeField] private Transform buildingModel;
    [Header("Story")]
    [SerializeField] private GameObject storyPrefab;
    [SerializeField] private int storyCount;

    [SerializeField] private bool randomStoryCount;
    [SerializeField] private int storyCountRangeMin;
    [SerializeField] private int storyCountRangeMax;

    [SerializeField] private float storyOffset;
    [Space(10)]
    [SerializeField] private Transform baseFloorRef;
    [SerializeField] private float baseStoryOffset;
    [Space(10)]
    [SerializeField] private Transform roofRef;
    [SerializeField] private float roofOffset;

    [Header("Variation")]
    [SerializeField] private bool randomMaterial;
    [SerializeField] private List<Material> matVariation;

    [Header("Props")]
    [SerializeField] private List<PropContainer> propContainers;

    private Vector3 baseDiff;

    // Start is called before the first frame update
    void Start()
    {
        BuildStory();
        BuildMaterialVariation();
    }

    private void BuildStory()
    {
        if (randomStoryCount) storyCount = Random.Range(storyCountRangeMin, storyCountRangeMax);

        for (int i = 0; i < storyCount; i++)
        {
            Transform storyInstance = Instantiate(storyPrefab, buildingModel).transform;

            if (i == 0)
            {
                baseDiff = baseFloorRef.localPosition + new Vector3(0, baseStoryOffset);
                storyInstance.localPosition = baseDiff;
            }
            else
            {
                storyInstance.localPosition = new Vector3(baseDiff.x, baseDiff.y + (i * storyOffset));

                if (i == storyCount - 1)
                {
                    roofRef.localPosition = new Vector3(storyInstance.localPosition.x, storyInstance.localPosition.y + roofOffset);
                }
            }
        }
    }

    private void BuildMaterialVariation()
    {
        if (!randomMaterial) return;

        Material mat = matVariation[Random.Range(0, matVariation.Count)];

        foreach (Transform child in buildingModel)
        {
            foreach (Transform grandChild in child)
            {
                if(grandChild.GetComponent<MeshRenderer>() != null)
                {
                    grandChild.GetComponent<MeshRenderer>().material = mat;
                }
            }
        }
    }
}
