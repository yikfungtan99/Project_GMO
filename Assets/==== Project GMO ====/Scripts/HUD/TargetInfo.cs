using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetInfo : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI textInfoName;

    [SerializeField] private PlayerLookTarget target;

    IHaveInfoName currentTargetInfoName;
    IHealth currentTargetHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target.currentTargetInfoName == null)
        {
            if (textInfoName.gameObject.activeSelf) textInfoName.gameObject.SetActive(false);
        }
        else
        {
            if (!textInfoName.gameObject.activeSelf) textInfoName.gameObject.SetActive(true);
        }

        if(target.currentTargetHealth == null)
        {
            if (healthBar.gameObject.activeSelf) healthBar.gameObject.SetActive(false);
        }
        else
        {
            if (!healthBar.gameObject.activeSelf) healthBar.gameObject.SetActive(true);
        }

        if(currentTargetInfoName != target.currentTargetInfoName)
        {
            currentTargetInfoName = target.currentTargetInfoName;

            if (currentTargetInfoName != null)
            {
                textInfoName.text = currentTargetInfoName.InfoName;
            }
            else
            {
                textInfoName.text = string.Empty;
            }
        }

        if (currentTargetHealth != target.currentTargetHealth)
        {
            currentTargetHealth = target.currentTargetHealth;
        }

        if (currentTargetHealth != null)
        {
            healthBar.value = ((float)currentTargetHealth.Health / (float)currentTargetHealth.MaxHealth);
        }
    }
}
