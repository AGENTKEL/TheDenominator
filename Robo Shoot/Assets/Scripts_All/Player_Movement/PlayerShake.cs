using UnityEngine;
using Cinemachine;

public class PlayerShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    public float shakeIntensity = 2.0f;
    public float shakeTime = 0.5f;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    PlayerHealth playerTakeDamage;

    private void Awake()
    {
        playerTakeDamage = GetComponent<PlayerHealth>();
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        StopShake();
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = shakeIntensity;

        timer = shakeTime;
    }

    public void StopShake()
    {
        CinemachineBasicMultiChannelPerlin _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;
        timer = 0f;
    }

    private void Update()
    {
        

        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StopShake();
            }
        }
    }

}
