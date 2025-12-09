using Player;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class FOVManager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private CinemachineCamera camera;

    [SerializeField]
    private float FOVMinSpeed;

    [SerializeField]
    private float FOVMaxSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        float playerSpeed = playerMovement.GetComponent<Rigidbody>().linearVelocity.magnitude;
        float FOVFactor = math.clamp(math.remap(60f, 120f, 0f, 1f, playerSpeed), 0f, 1f);
        float targetFOV = Mathf.Lerp(FOVMinSpeed, FOVMaxSpeed, FOVFactor);
        camera.Lens.FieldOfView = Mathf.Lerp(camera.Lens.FieldOfView, targetFOV, 0.1f);
    }
}
