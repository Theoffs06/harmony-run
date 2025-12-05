using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UISpeedBar : MonoBehaviour {
        private static readonly int FillValue = Shader.PropertyToID("_Fill_Value");
        
        private Material _material;

        private void Awake() {
            _material = GetComponent<Image>().material;
            _material.SetFloat(FillValue, 1);
        }

        public void UpdateSpeed(float speed, float maxSpeed) {
            _material.SetFloat(FillValue, Mathf.Clamp01(speed / maxSpeed));
        }
    }
}