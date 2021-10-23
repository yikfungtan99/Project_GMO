using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin vcamNoise;

    private float shakeTime;
    private float shakeDuration;

    // Start is called before the first frame update
    void Start()
    {
        CinemachineVirtualCamera vcam = (CinemachineVirtualCamera)GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        vcamNoise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTime > 0)
        {
            vcamNoise.m_AmplitudeGain = Mathf.Lerp(vcamNoise.m_AmplitudeGain, 0f, 1 - (shakeTime / shakeDuration));
            shakeTime -= Time.deltaTime;
        }
        else
        {
            vcamNoise.m_AmplitudeGain = 0;
            shakeTime = 0;
        }
    }

    public void ShakeCamera(float shakeAmp, float shakeDuration)
    {
        this.shakeTime = shakeDuration;
        this.shakeDuration = shakeDuration;
        vcamNoise.m_AmplitudeGain = shakeAmp;
    }
}
