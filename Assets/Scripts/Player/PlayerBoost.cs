using UI;
using UnityEngine;

namespace Player {
    public class PlayerBoost : MonoBehaviour {
        [SerializeField] private float boostConsumptionRate = 0.01f;
        
        public bool IsBoosted { get; private set; }
        
        private PlayerAudio _audio; 
        private UIBoostBar _boostUI;

        public void OnCreate(PlayerAudio audioManager, UIBoostBar boostUI) {
            _audio = audioManager;
            _boostUI = boostUI;
        }
        
        public bool TryConsumeBoost() {
            if (!IsBoosted || _boostUI.CurrentBoost <= 0f) return false;
            _boostUI.DecreaseBoost(Time.deltaTime * boostConsumptionRate);
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