using UnityEngine;

public class ScoreConverterTrigger : MonoBehaviour
{
    private ScoreConverterManager _scoreConverterManager;

    public void OnCreate()
    {
        _scoreConverterManager = GetComponentInParent<ScoreConverterManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collision");
        if (collider.gameObject.layer == 8)
        {
            Debug.Log("CollisionAnno");
            _scoreConverterManager.TriggeredScoreConverter(gameObject.name == "Player 1" ? 0 : 1);
        }
    }

}
