using Unity.Mathematics;
using UnityEngine;

public class DynamicSplitScreen : MonoBehaviour {
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [SerializeField] private Camera singleCam;
    [SerializeField] private Camera playerCam1;
    [SerializeField] private Camera playerCam2;

    [SerializeField] private GameObject splitVerticalImage;
    [SerializeField] private GameObject splitHorizontalImage;
    
    [SerializeField] private GameObject playerListener1;
    [SerializeField] private GameObject playerListener2;
    
    [SerializeField] private float splitDistance = 10f;
    [SerializeField] private float listenerDistance = 2f;
    
    public Camera SingleCam => singleCam;
    public Camera PlayerCam1 => playerCam1;
    public Camera PlayerCam2 => playerCam2;
    
    public bool IsSplit { get; private set; }

    private static readonly Rect Left = new(0, 0, 0.5f, 1);
    private static readonly Rect Right = new(0.5f, 0, 0.5f, 1);
    private static readonly Rect Top = new(0, 0.5f, 1, 0.5f);
    private static readonly Rect Bottom = new(0, 0, 1, 0.5f);

    private void Start() {
        SetSingleCamera();
    }

    private void Update() {
        switch (math.distance(player1.position, player2.position) > splitDistance) {
            case true when !IsSplit:
                SetSplitCameras();
                IsSplit = true;
                break;
            case false when IsSplit:
                SetSingleCamera();
                IsSplit = false;
                break;
        }
    }

    private void SetSingleCamera() {
        singleCam.gameObject.SetActive(true);
        playerCam1.gameObject.SetActive(false);
        playerCam2.gameObject.SetActive(false);
        splitVerticalImage.SetActive(false);
        splitHorizontalImage.SetActive(false);
    }

    private void SetSplitCameras() {
        singleCam.gameObject.SetActive(false);
        playerCam1.gameObject.SetActive(true);
        playerCam2.gameObject.SetActive(true);
        UpdateViewport();
    }

    private void UpdateViewport() {
        var dx = Mathf.Abs(player1.position.x - player2.position.x);
        var dy = Mathf.Abs(player1.position.y - player2.position.y);

        var horizontalSplit = dx >= dy;
        splitVerticalImage.SetActive(horizontalSplit);
        splitHorizontalImage.SetActive(!horizontalSplit);

        if (horizontalSplit) {
            if (player1.position.x < player2.position.x) {
                playerListener1.transform.localPosition = Vector3.right * listenerDistance;
                playerListener2.transform.localPosition = Vector3.left * listenerDistance;
                SetRects(Left, Right);
            }
            else {
                playerListener1.transform.localPosition = Vector3.left * listenerDistance;
                playerListener2.transform.localPosition = Vector3.right * listenerDistance;
                SetRects(Right, Left);
            }
        }
        else {
            if (player1.position.y > player2.position.y)
                SetRects(Top, Bottom);
            else
                SetRects(Bottom, Top);
        }
    }

    private void SetRects(Rect playerRect1, Rect playerRect2) {
        playerCam1.rect = playerRect1;
        playerCam2.rect = playerRect2;
    }
}
