using Player;
using Unity.Mathematics;
using UnityEngine;

public class SpeedLinesManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private Material speedLinesMaterial;

    [SerializeField]
    private float MinSpeed;

    [SerializeField]
    private float MaxSpeed;

    [SerializeField, Range(0.0f, 1.0f)]
    private float minEffectDensity;

    [SerializeField, Range(0.0f, 1.0f)]
    private float maxEffectDensity;

    private float currentFactor = 0f;

    [SerializeField, Range(0f, 1f)]
    private float delayFactor = 0.2f;

    void Start()
    {
        speedLinesMaterial.SetFloat("_Effect_Density", minEffectDensity);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVelocity = playerMovement.GetComponent<Rigidbody>().linearVelocity;

        Vector3 carForward = playerMovement.transform.forward;

        float playerForwardSpeed = Vector3.Dot(playerVelocity, carForward);

        float speedLinesFactor = math.clamp(
            math.remap(MinSpeed, MaxSpeed, 0f, 1f, playerForwardSpeed),
            0f,
            1f
        );

        currentFactor = Mathf.Lerp(currentFactor, speedLinesFactor, delayFactor);

        speedLinesMaterial.SetFloat(
            "_Effect_Density",
            Mathf.Lerp(minEffectDensity, maxEffectDensity, currentFactor)
        );
    }
}
