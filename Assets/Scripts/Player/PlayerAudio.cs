using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Player {
    public class PlayerAudio : MonoBehaviour {
        [SerializeField] private EventReference motorEvent;
        [SerializeField] private EventReference windEvent;
        [SerializeField] private EventReference boostStartEvent;
        [SerializeField] private EventReference boostEndEvent;
        [SerializeField] private EventReference jumpEvent;
        [SerializeField] private bool muteSound;

        private EventInstance _motorInstance;
        private EventInstance _windInstance;

        public void OnCreate(Rigidbody rb) {
            if (muteSound) return;
            _motorInstance = RuntimeManager.CreateInstance(motorEvent);
            RuntimeManager.AttachInstanceToGameObject(_motorInstance, gameObject, rb);

            _windInstance = RuntimeManager.CreateInstance(windEvent);
            RuntimeManager.AttachInstanceToGameObject(_windInstance, gameObject, rb);
        }

        public void OnStart() {
            if (muteSound) return;
            _motorInstance.start();
            _windInstance.start();
        }
        
        public void OnJump() {
            if (muteSound) return;
            RuntimeManager.PlayOneShotAttached(jumpEvent, gameObject);
        }
        
        public void OnBoostStart() {
            if (muteSound) return;
            RuntimeManager.PlayOneShotAttached(boostStartEvent, gameObject);
        }

        public void OnBoostEnd() {
            if (muteSound) return;
            RuntimeManager.PlayOneShotAttached(boostEndEvent, gameObject);
        }

        public void SetSpeed(float speed) {
            if (!_motorInstance.isValid() || !_windInstance.isValid()) return;
            _motorInstance.setParameterByName("Speed", speed);
            _windInstance.setParameterByName("Speed", speed);
        }

        public void SetBoost(float boosted) {
            if (!_motorInstance.isValid() || !_windInstance.isValid()) return;
            _windInstance.setParameterByName("Boost", boosted);
            _motorInstance.setParameterByName("Boost", boosted);
        }
    }
}