using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class DynamicSplitScreen : MonoBehaviour {
    
    [SerializeField] private Camera singleCam;
    [SerializeField] private Camera playerCam1;
    [SerializeField] private Camera playerCam2;

    [SerializeField] private GameObject splitVerticalImage;
    
    [SerializeField] private GameObject playerListener1;
    [SerializeField] private GameObject playerListener2;
    
    [SerializeField] private float softSplitDistance = 10f;
    [SerializeField] private float hardSplitDistance = 30f;
    [SerializeField] private float listenerDistance = 2f;

    [SerializeField] private RawImage Cam1;
    [SerializeField] private RawImage Cam2;

    [SerializeField] private CinemachineMixingCamera singleCineCam;
    [SerializeField] private CinemachineMixingCamera player1CineCam;
    [SerializeField] private CinemachineMixingCamera player2CineCam;

    public Camera SingleCam => singleCam;
    public Camera PlayerCam1 => playerCam1;
    public Camera PlayerCam2 => playerCam2;
    
    public bool IsSplit { get; private set; }
    
    private Transform _player1; 
    private Transform _player2;

    private float _playersDistance;
    private float _playersSoftRatio;

    public void OnCreate(Transform player1, Transform player2) {
        _player1 = player1;
        _player2 = player2;
    }

    public void OnUpdate() {
        _playersDistance = Vector3.Distance(_player1.position, _player2.position);
        if (_playersDistance < softSplitDistance)
        {
            IsSplit = false;
            splitVerticalImage.SetActive(false);
        }
        else
        {
            IsSplit = true;
            splitVerticalImage.SetActive(true);
        }
        _playersSoftRatio = Mathf.Clamp01((_playersDistance - softSplitDistance) / (hardSplitDistance - softSplitDistance));
        player1CineCam.SetWeight(0, 0.5f + _playersSoftRatio / 2);
        player1CineCam.SetWeight(1, 0.5f - _playersSoftRatio / 2);
        player2CineCam.SetWeight(0, 0.5f - _playersSoftRatio / 2);
        player2CineCam.SetWeight(1, 0.5f + _playersSoftRatio / 2);
        playerCam1.rect = new(0, 0, 1 - _playersSoftRatio / 2, 1);
        playerCam2.rect = new(0 + _playersSoftRatio / 2, 0, 1 - _playersSoftRatio / 2, 1);
    }

}
