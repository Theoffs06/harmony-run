using Player;
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
    private void Start()
    {
        playerSyncActions.OnPlayerSucceededTricks.AddListener(RollAnimationOnSucces);
        Hide();
        rollUIAnimation["RollUI"].layer = 0;
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

        rollUIAnimation.Play(PlayMode.StopAll);
    }

    public void Display()
    {
        image.enabled = true;
    }

    public void Hide()
    {
        image.enabled = false;
    }

    // Update is called once per frame
    void Update() { }
}
