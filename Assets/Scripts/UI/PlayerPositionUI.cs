using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerPositionUI : MonoBehaviour
{
    [SerializeField, Range(1, 2)]
    private int playerCamNumber = 1;

    [SerializeField]
    private Transform playerToFollow;

    [SerializeField]
    private DynamicSplitScreen cameraManager;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private Vector2 offset;

    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (!cameraManager.IsSplit)
        {
            UpdatePositionFromCameraSingle(cameraManager.SingleCam);
            rectTransform.gameObject.SetActive(true);
        }
        else
        {
            Camera cameraToCheck =
                playerCamNumber == 1 ? cameraManager.PlayerCam1 : cameraManager.PlayerCam2;
            UpdatePositionFromCameraSplited(cameraToCheck);

            // the point is not visible if the player is behind the camera
            bool isVisible =
                Vector3.Dot(
                    cameraToCheck.transform.forward,
                    playerToFollow.transform.position - cameraToCheck.transform.position
                ) > 0f;

            rectTransform.gameObject.SetActive(isVisible);
        }
    }

    private void UpdatePositionFromCameraSingle(Camera camera)
    {
        Vector2 playerScreenPosition = camera.WorldToScreenPoint(playerToFollow.position);

        float xPosition = Mathf.Clamp(
            playerScreenPosition.x - camera.pixelWidth / 2 + offset.x,
            -camera.pixelWidth / 2,
            camera.pixelWidth / 2
        );
        float yPosition = Mathf.Clamp(
            playerScreenPosition.y - camera.pixelHeight / 2 + offset.y,
            -camera.pixelHeight / 2,
            camera.pixelHeight / 2
        );

        rectTransform.localPosition = new Vector2(xPosition, yPosition);
    }

    private void UpdatePositionFromCameraSplited(Camera camera)
    {
        Vector2 playerScreenPosition = camera.WorldToScreenPoint(playerToFollow.position);

        var camRect = camera.rect;
        bool isLeft = camRect.x <= 0.01f;

        float sign = isLeft ? -1 : 0.5f;

        float xPosition = Mathf.Clamp(
            playerScreenPosition.x - camera.pixelWidth / 2 + offset.x,
            -camera.pixelWidth / 2,
            camera.pixelWidth / 2
        );
        float yPosition = Mathf.Clamp(
            playerScreenPosition.y - camera.pixelHeight / 2 + offset.y,
            -camera.pixelHeight / 2,
            camera.pixelHeight / 2
        );
        xPosition = playerScreenPosition.x - camera.pixelWidth / 2 + offset.x;
        yPosition = playerScreenPosition.y - camera.pixelHeight / 2 + offset.y;

        if (!isLeft)
        {
            xPosition -= 3 * camera.pixelWidth / 4;
        }

        xPosition = isLeft
            ? Mathf.Clamp(xPosition, -camera.pixelWidth / 2, camera.pixelWidth / 2)
            : Mathf.Clamp(xPosition, -camera.pixelWidth / 4, 3 * camera.pixelWidth / 4);

        yPosition = Mathf.Clamp(yPosition, -camera.pixelHeight / 2, camera.pixelHeight / 2);

        rectTransform.localPosition =
            new Vector2(xPosition, yPosition) + sign * Vector2.right * camera.pixelWidth / 2;
    }
}
