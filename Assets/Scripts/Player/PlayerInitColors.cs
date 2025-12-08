using UnityEngine;

public class PlayerInitColors : MonoBehaviour
{
    [SerializeField] Material _carMaterial;
    Material _instCarMat;
    [SerializeField] Material _neonMaterial;
    Material _instNeonTrailMat;
    private Renderer rend;

    [SerializeField] Color _playerMainColor = Color.red;
    [SerializeField] Color _playerNeonColor = Color.red;
    [SerializeField] float _playerNeonInt = 1;

    [SerializeField] Color _trailNeonColor = Color.red;
    [SerializeField] float _trailNeonInt = 1;

    [SerializeField] TrailRenderer _trailRendererL;
    [SerializeField] TrailRenderer _trailRendererR;


    void Start()
    {
        rend = GetComponent<Renderer>();
        _instCarMat = Instantiate(_carMaterial);
        
        ApplyColor(_playerMainColor, _instCarMat, "_CentralLines_Color");
        ApplyHDRColor(_playerNeonColor, _playerNeonInt, _instCarMat, "_PharesArrBig");
        ApplyHDRColor(_playerNeonColor, _playerNeonInt, _instCarMat, "_Wheel_EmissiveColor");

        _instNeonTrailMat = Instantiate(_neonMaterial);
        ApplyHDRColor(_trailNeonColor, _trailNeonInt, _instNeonTrailMat, "_End_Color");

        rend.material = _instCarMat;
        _trailRendererL.material = _instNeonTrailMat;
        _trailRendererR.material = _instNeonTrailMat;

    }
    private void ApplyHDRColor(Color _baseColor, float _intensity, Material _matName, string _PropertyName)
    {
        Color hdr = _baseColor * _intensity;
        hdr.a = 1f; 

        _matName.SetColor(_PropertyName, hdr);
    }

    private void ApplyColor(Color _baseColor, Material _matName, string _PropertyName)
    {
        _matName.SetColor(_PropertyName, _baseColor);
    }
}
