using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerPosition : MonoBehaviour
    {
        [SerializeField, Range(0,1)] private float hideWaypointTreshold;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerMovement mateMovement;
        [SerializeField] private RawImage waypointImage;

        private float _splineAdvancementDifference;
        private Vector3 _normalScale = new(1,1,1);
        private Vector3 _reverseScale = new(1,-1,1);

        private Vector2 _direction;

        public void OnUpdate()
        {
            _splineAdvancementDifference = playerMovement.splineAdvancement - mateMovement.splineAdvancement;

            Vector2 u, v;

            if (_splineAdvancementDifference < 0)
            {
                transform.localScale = _reverseScale;
                _direction = (new Vector2(mateMovement.gameObject.transform.position.x, mateMovement.gameObject.transform.position.z) - new Vector2(playerMovement.gameObject.transform.position.x, playerMovement.gameObject.transform.position.z));
                u = new Vector2(mateMovement.gameObject.transform.forward.x, mateMovement.gameObject.transform.forward.z); // Direction for the x-axis
                v = new Vector2(-mateMovement.gameObject.transform.right.x, -mateMovement.gameObject.transform.right.z); // Direction for the y-axis
            }
            else
            {
                transform.localScale = _normalScale;
                _direction = (new Vector2(playerMovement.gameObject.transform.position.x, playerMovement.gameObject.transform.position.z) - new Vector2(mateMovement.gameObject.transform.position.x, mateMovement.gameObject.transform.position.z));
                u = new Vector2(playerMovement.gameObject.transform.forward.x, playerMovement.gameObject.transform.forward.z); // Direction for the x-axis
                v = new Vector2(-playerMovement.gameObject.transform.right.x, -playerMovement.gameObject.transform.right.z); // Direction for the y-axis
            }

            // Create the transformation matrix
            Matrix4x4 transformationMatrix = new Matrix4x4(
                new Vector4(u.x, v.x, 0, 0), // First column
                new Vector4(u.y, v.y, 0, 0), // Second column
                new Vector4(0, 0, 1, 0),      // Third column (for homogeneity)
                new Vector4(0, 0, 0, 1)       // Fourth column (for homogeneity)
            );

            // Convert Vector2 to Vector3
            Vector3 vector3 = new Vector3(_direction.x, _direction.y, 1);

            // Apply the transformation
            Vector3 transformedVector = transformationMatrix.MultiplyPoint3x4(vector3);

            // Convert back to Vector2 if needed
            Vector2 result = new Vector2(transformedVector.x, transformedVector.y);

            float angleInRadians = Mathf.Atan2(result.y, result.x);

            // Convert the angle to degrees
            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

            angleInDegrees = Mathf.Clamp(angleInDegrees, -45, 45);
            transform.rotation = Quaternion.Euler(new(0,0,angleInDegrees));
            if (Mathf.Abs(_splineAdvancementDifference) < hideWaypointTreshold) waypointImage.enabled = false;
            else waypointImage.enabled = true;

        }
    }
}
