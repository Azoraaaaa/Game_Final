using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineMachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

    private float _shakeTime;
    private float _shakeTimeTotal;
    private float _shakeIntensity;
    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _multiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _shakeIntensity = 5;
        _shakeTimeTotal = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ShakeCamera();
        }
        if(_shakeTime > 0)
        {
            _shakeTime -= Time.deltaTime;
            _multiChannelPerlin.AmplitudeGain = Mathf.Lerp(0, _shakeIntensity, _shakeTime /  _shakeTimeTotal);
        }
    }
    private void ShakeCamera()
    {
        _shakeTime = _shakeTimeTotal;
        _multiChannelPerlin.AmplitudeGain = _shakeIntensity;
    }
}
