using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    [SerializeField]
    protected Image image;

    [SerializeField]
    private string animationName = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private Animation animationUI;

    [SerializeField]
    private void Start()
    {
        Hide();
    }

    public void PlayAnimation()
    {
        Display();

        animationUI[animationName].time = 0;

        animationUI.Play();
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
