using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RollAnimation : MonoBehaviour
{
    [SerializeField]
    private Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private Animation rollUIAnimation;

    [SerializeField, Range(0, 2)]
    private int playerNumber = 0;

    [SerializeField]
    private PlayerSyncActions playerSyncActions;

    [SerializeField]
    private PlayerTricks playerTricks;

    [SerializeField]
    private TextMeshProUGUI multiplierText;

    [SerializeField]
    private int fontSizeIncreaseOverTricksCount = 3;

    [SerializeField]
    private int maxFontSize = 110;

    private float multiplierTextDefaultSize;

    [SerializeField]
    private void Start()
    {
        playerSyncActions.OnPlayerSucceededTricks.AddListener(RollAnimationOnSucces);
        Hide();
        rollUIAnimation["RollUI"].layer = 0;
        multiplierTextDefaultSize = multiplierText.fontSize;
    }

    void RollAnimationOnSucces(int playerNumberTricking)
    {
        Debug.Log("player id => " + playerNumberTricking);
        if (playerNumber != playerNumberTricking)
        {
            return;
        }
        rollUIAnimation["RollUI"].time = 0;

        Display();

        UpdateMultiplierText(playerTricks.GetTrickCounter());

        rollUIAnimation.Play(PlayMode.StopAll);
    }

    void UpdateMultiplierText(int tricksCount)
    {
        if (tricksCount <= 1)
        {
            multiplierText.enabled = false;
            multiplierText.text = "";
            return;
        }

        multiplierText.text = ">< " + tricksCount.ToString();
        multiplierText.fontSize =
            multiplierTextDefaultSize + fontSizeIncreaseOverTricksCount * tricksCount;
    }

    public void Display()
    {
        image.enabled = true;
        multiplierText.enabled = true;
    }

    public void Hide()
    {
        image.enabled = false;
        multiplierText.enabled = false;
    }

    // Update is called once per frame
    void Update() { }
}
