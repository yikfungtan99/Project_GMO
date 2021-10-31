using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveText : MonoBehaviour
{
    [SerializeField] private float showTime;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] Incubator incubator;

    private float waitTime;

    private void OnEnable()
    {
        if(incubator) incubator.OnAttackEvent += ShowText;
    }

    public async void ShowText()
    {
        waitTime = Time.time + showTime;

        text.enabled = true;

        while (Time.time < waitTime)
        { 
            await System.Threading.Tasks.Task.Yield();
        }

        text.enabled = false;
    }

    private void OnDisable()
    {
        if (incubator) incubator.OnAttackEvent -= ShowText;
    }
}
