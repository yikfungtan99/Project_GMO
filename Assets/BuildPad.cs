using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildPad : MonoBehaviour
{
    [SerializeField] private List<GameObject> availableStations = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI stationNameText;
    [SerializeField] private TextMeshProUGUI stationPriceText;

    [SerializeField] private GameObject buildInterface;

    private Station currentStation;

    private int currentSelectionIndex = 0;

    private void Start()
    {
        UpdateInterface();
    }

    public void NextStation()
    {
        currentSelectionIndex++;

        if (currentSelectionIndex >= availableStations.Count) currentSelectionIndex = 0;

        UpdateInterface();
    }

    public void PrevStation()
    {
        currentSelectionIndex--;

        if (currentSelectionIndex < 0) currentSelectionIndex = availableStations.Count - 1;

        UpdateInterface();
    }

    public void UpdateInterface()
    {
        stationNameText.text = availableStations[currentSelectionIndex].GetComponent<Station>().InfoName;
        stationPriceText.text = availableStations[currentSelectionIndex].GetComponent<Station>().StationPrice.ToString();
    }

    public void BuildStation()
    {
        GameObject builtInstance = Instantiate(availableStations[currentSelectionIndex], transform);
        
        currentStation = builtInstance.GetComponent<Station>();
        currentStation.OnDestroyed += BuildInterface;

        builtInstance.transform.localPosition = Vector3.zero;
        buildInterface.SetActive(false);
    }

    public void BuildInterface()
    {
        currentStation.OnDestroyed -= BuildInterface;
        buildInterface.SetActive(true);
    }
}
