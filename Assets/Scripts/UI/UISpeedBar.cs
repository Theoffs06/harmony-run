using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UISpeedBar : MonoBehaviour {
        private static readonly int FillValue = Shader.PropertyToID("_Fill_Value");
        
        [SerializeField] private float smoothTime = 0.15f;
        [SerializeField] private Image fillImage;
        
        private float _currentValue = 1f;
        private Coroutine _smoothRoutine;
        
        private Material _material;

        public void OnCreate() {
            _material = Instantiate(fillImage.material);
            _material.SetFloat(FillValue, _currentValue);
            
            fillImage.material = _material;
        }

        public void UpdateSpeed(float speed, float maxSpeed) {
            var targetValue = Mathf.Clamp01(speed / maxSpeed);

            if (_smoothRoutine != null) StopCoroutine(_smoothRoutine);

            _smoothRoutine = StartCoroutine(SmoothUpdate(targetValue));
        }
        
        private IEnumerator SmoothUpdate(float targetValue) {
            var startValue = _currentValue;
            var elapsed = 0f;

            while (elapsed < smoothTime) {
                elapsed += Time.deltaTime;
                _currentValue = Mathf.Lerp(startValue, targetValue, elapsed / smoothTime);
                _material.SetFloat(FillValue, _currentValue);
                yield return null;
            }

            _currentValue = targetValue;
            _material.SetFloat(FillValue, _currentValue);
        }
    }
}