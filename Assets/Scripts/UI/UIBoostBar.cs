using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class UIBoostBar : MonoBehaviour {
        private static readonly int FillValue = Shader.PropertyToID("_Fill_Value");
        
        [SerializeField, Range(0, 1)] private float startBoost = 1;
        
        public float CurrentBoost { get; private set; }
        
        private Material _material;
        
        private void Awake() {
            CurrentBoost = startBoost;
            
            _material = GetComponent<Image>().material;
            _material.SetFloat(FillValue, startBoost);
        }

        public void DecreaseBoost(float value) {
            CurrentBoost -= value;
            math.clamp(CurrentBoost, 0, 1);
            
            _material.SetFloat(FillValue, CurrentBoost);
        }
        
        public void IncreaseBoost(float value) {
            CurrentBoost += value;
            math.clamp(CurrentBoost, 0, 1);
            
            _material.SetFloat(FillValue, CurrentBoost);
        }
    }
}