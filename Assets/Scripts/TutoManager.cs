using UnityEngine;

public class TutoManager : MonoBehaviour
{
    private int state;
    public GameObject[] anims;

    public void NextAnim()
    {
        anims[state].SetActive(false);
        state++;
        anims[state].SetActive(true);
    }
}
