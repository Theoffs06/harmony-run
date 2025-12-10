using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class UIDuoAnimation : UIAnimation
{
    [SerializeField]
    private PlayerSyncActions playerSyncActions;

    [SerializeField]
    private Image timingImage;

    [SerializeField]
    private List<Sprite> timingTextures;

    [SerializeField]
    private List<float> timingTextureScales;

    private void Start()
    {
        Hide();
    }

    public new void PlayAnimation()
    {
        SetTimingLevelUI();
        base.PlayAnimation();
    }

    public new void Hide()
    {
        timingImage.enabled = false;
        base.Hide();
    }

    public void DisplayTiming()
    {
        timingImage.enabled = true;
    }

    private void SetTimingLevelUI()
    {
        int timinglevel = playerSyncActions.GetTimingLevel();

        int index = Mathf.Clamp(timinglevel - 1, 0, timingTextures.Count - 1);

        timingImage.sprite = timingTextures[index];

        timingImage.gameObject.GetComponent<RectTransform>().localScale = new Vector3(
            timingTextureScales[index],
            timingTextureScales[index],
            timingTextureScales[index]
        );

        Debug.Log("affichage => " + index);

        if (timinglevel == 0)
        {
            timingImage.enabled = false;
        }
    }
}
