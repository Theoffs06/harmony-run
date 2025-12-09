using Unity.Mathematics;
using UnityEngine;

namespace UI
{
    public class UIPlayerPosition : MonoBehaviour
    {
        [SerializeField, Range(1, 2)]
        private int playerCamNumber = 1;

        [SerializeField]
        private Vector2 offset;

        private Transform _playerToFollow;
        private DynamicSplitScreen _cameraManager;
        private RectTransform _rectTransform;

        public void OnCreate(Transform playerToFollow, DynamicSplitScreen cameraManager)
        {
            _rectTransform = transform.GetChild(0).GetComponent<RectTransform>();

            _playerToFollow = playerToFollow;
            _cameraManager = cameraManager;
        }

        public void OnUpdate()
        {
            if (!_cameraManager.IsSplit && false)
            {
                UpdatePositionFromCameraSingle(_cameraManager.SingleCam);
                _rectTransform.gameObject.SetActive(true);
            }
            else
            {
                var cameraToCheck =
                    playerCamNumber == 1 ? _cameraManager.PlayerCam1 : _cameraManager.PlayerCam2;
                UpdatePositionFromCameraSplit(cameraToCheck);

                // the point is not visible if the player is behind the camera
                _rectTransform.gameObject.SetActive(
                    math.dot(
                        cameraToCheck.transform.forward,
                        _playerToFollow.transform.position - cameraToCheck.transform.position
                    ) > 0f
                );
            }
        }

        private void UpdatePositionFromCameraSingle(Camera cam)
        {
            if (!cam)
                return;
            Vector2 playerScreenPosition = cam.WorldToScreenPoint(_playerToFollow.position);

            var xPosition = math.clamp(
                playerScreenPosition.x - cam.pixelWidth / 2f + offset.x,
                -cam.pixelWidth / 2f,
                cam.pixelWidth / 2f
            );
            var yPosition = math.clamp(
                playerScreenPosition.y - cam.pixelHeight / 2f + offset.y,
                -cam.pixelHeight / 2f,
                cam.pixelHeight / 2f
            );
            _rectTransform.localPosition = new Vector2(xPosition, yPosition);
        }

        private void UpdatePositionFromCameraSplit(Camera cam)
        {
            if (!cam)
                return;
            Vector2 playerScreenPosition = cam.WorldToScreenPoint(_playerToFollow.position);

            var camRect = cam.rect;

            var isLeft = camRect.x <= 0.01f;
            var sign = isLeft ? -1 : 0.5f;

            var xPosition = playerScreenPosition.x - cam.pixelWidth / 2f + offset.x;
            var yPosition = playerScreenPosition.y - cam.pixelHeight / 2f + offset.y;

            if (!isLeft)
                xPosition -= 3 * cam.pixelWidth / 4f;

            xPosition = isLeft
                ? math.clamp(xPosition, -cam.pixelWidth / 2f, cam.pixelWidth / 2f)
                : math.clamp(xPosition, -cam.pixelWidth / 4f, 3 * cam.pixelWidth / 4f);

            yPosition = math.clamp(yPosition, -cam.pixelHeight / 2f, cam.pixelHeight / 2f);
            _rectTransform.localPosition =
                new Vector2(xPosition, yPosition) + Vector2.right * (sign * cam.pixelWidth) / 2f;
        }
    }
}
