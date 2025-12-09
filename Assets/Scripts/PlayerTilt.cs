using Player;
using Unity.Mathematics;
using UnityEngine;

public class PlayerTilt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private PlayerGround playerGround;

    [SerializeField]
    private Transform carTransform;

    [SerializeField]
    private float maxTiltAngle = 15f;

    [SerializeField]
    private float delayFactor = 0.5f;

    private Vector3 initialEuler;

    private float tilt;

    void Start()
    {
        initialEuler = carTransform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerVelocity = playerMovement.GetComponent<Rigidbody>().linearVelocity;

        Vector3 carRight = math.normalize(math.cross(new float3(0, 1, 0), carTransform.right));

        float playerHorizontalSpeed = Vector3.Dot(playerVelocity, carRight);
        float playerHorizontalSpeedAbs = Mathf.Abs(playerHorizontalSpeed);

        float tiltFactorAbs = math.clamp(
            math.remap(0f, 20f, 0, 1f, playerHorizontalSpeedAbs),
            0,
            1f
        );

        if (!playerGround.IsGrounded())
        {
            carTransform.localRotation = Quaternion.Euler(initialEuler);
        }
        else
        {
            float tiltTarget =
                -Mathf.Sign(playerHorizontalSpeed) * Mathf.Lerp(0f, maxTiltAngle, tiltFactorAbs);

            float actualTilt = carTransform.localRotation.x;

            tilt = Mathf.Lerp(tilt, tiltTarget, delayFactor);

            carTransform.localRotation = Quaternion.Euler(
                tilt,
                carTransform.localRotation.eulerAngles.y,
                carTransform.localRotation.eulerAngles.z
            );
        }
    }
}
