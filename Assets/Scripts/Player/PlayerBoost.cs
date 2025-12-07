using UI;
using UnityEngine;

namespace Player {
    public class PlayerBoost : MonoBehaviour {
        [SerializeField] private float boostConsumptionRate = 0.01f;
        [SerializeField] private UIBoostBar boostUI;
        
        public bool IsBoosted { get; private set; }
        
        private PlayerAudio _audio;

        public void OnCreate(PlayerAudio audioManager) {
            _audio = audioManager;
        }
        
        public bool TryConsumeBoost() {
            if (!IsBoosted || boostUI.CurrentBoost <= 0f) return false;
            boostUI.DecreaseBoost(Time.deltaTime * boostConsumptionRate);
            return true;
        }

        public void StartBoost() {
            IsBoosted = true;
            
            _audio.OnBoostStart();
            _audio.SetBoost(1f);
        }
        
        public void StopBoost() {
            IsBoosted = false;
            
            _audio.OnBoostEnd();
            _audio.SetBoost(0f);
        }
    }
}