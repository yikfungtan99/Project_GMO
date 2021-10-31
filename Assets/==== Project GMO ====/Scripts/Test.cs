using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animatext;

public class Test : MonoBehaviour
{
    [SerializeField] AnimatextTMPro animatex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animatex.StartEffect(0);
            animatex.PlayEffect(0);
        }
    }
}
