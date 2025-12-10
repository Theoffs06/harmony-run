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
        [SerializeField] private EventReference figureTryEvent;
        [SerializeField] private EventReference figureSuccessEvent;
        [SerializeField] private EventReference figureSuccessTeamEvent;

        [SerializeField] private EventReference figureFailEvent;
        [SerializeField] private EventReference ringCrossedEvent;
        [SerializeField] private EventReference ringCrossedTeamEvent;
        [SerializeField] private bool muteSound;

        private EventInstance _motorInstance;
        private EventInstance _windInstance;

        private EventInstance _figureSuccessInstance;


        public void OnCreate(Rigidbody rb) {
            if (muteSound) return;
            _motorInstance = RuntimeManager.CreateInstance(motorEvent);
            RuntimeManager.AttachInstanceToGameObject(_motorInstance, gameObject, rb);

            _windInstance = RuntimeManager.CreateInstance(windEvent);
            RuntimeManager.AttachInstanceToGameObject(_windInstance, gameObject, rb);

            _figureSuccessInstance =  RuntimeManager.CreateInstance(figureSuccessEvent);
            RuntimeManager.AttachInstanceToGameObject(_figureSuccessInstance, gameObject, rb);

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

        public void OnFigureTry(int Combo = 0) {
            if (muteSound) return;
            RuntimeManager.PlayOneShotAttached(figureTryEvent, gameObject);
        }


        public void OnFigureFail() {
            if (muteSound) return;
            RuntimeManager.PlayOneShotAttached(figureFailEvent, gameObject);
        }

        public void OnFigureSuccess(int Combo) {
            if (muteSound) return;
            _figureSuccessInstance.setParameterByName("Combo", Combo);
            _figureSuccessInstance.start();
        }

        public void OnRingCrossed(bool isTeam) {
            if (muteSound) return;
            RuntimeManager.PlayOneShotAttached(isTeam ? ringCrossedTeamEvent : ringCrossedEvent, gameObject);
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

        public void SetScore(float score) {
            RuntimeManager.StudioSystem.setParameterByName("Score", score);
        }
    }
}