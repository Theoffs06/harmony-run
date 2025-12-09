using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private string animationName = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private Animation animation;

    [SerializeField]
    private void Start()
    {
        Hide();
    }

    public void PlayAnimation()
    {
        Debug.Log("play animation");
        // animation[animationName].time = 0;

        Display();

        animation.Play();
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
