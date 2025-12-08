using Player;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class ScreenShakeManager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private CinemachineBasicMultiChannelPerlin cameraShakePerlin;

    [SerializeField]
    private float AmplitudeMin;

    [SerializeField]
    private float AmplitudeMax;

    [SerializeField]
    private float SpeedMin;

    [SerializeField]
    private float SpeedMax;

    [SerializeField, Range(0f, 1f)]
    private float delayFactor = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        float playerSpeed = playerMovement.GetComponent<Rigidbody>().linearVelocity.magnitude;
        float shakeFactor = math.clamp(math.remap(SpeedMin, SpeedMax, 0f, 1f, playerSpeed), 0f, 1f);

        float targetShakeAmplitude = Mathf.Lerp(AmplitudeMin, AmplitudeMax, shakeFactor);
        cameraShakePerlin.AmplitudeGain = Mathf.Lerp(
            cameraShakePerlin.AmplitudeGain,
            targetShakeAmplitude,
            delayFactor
        );
    }
}
