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
    private List<Sprite> timingTextures;

    [SerializeField]
    private List<float> timingTextureScales;

    public new void PlayAnimation()
    {
        SetTimingLevelUI();
        base.PlayAnimation();
    }

    private void SetTimingLevelUI()
    {
        int timinglevel = playerSyncActions.GetTimingLevel();

        int index = Mathf.Clamp(timinglevel - 1, 0, timingTextures.Count - 1);

        image.sprite = timingTextures[index];

        image.transform.parent.GetComponent<RectTransform>().localScale = new Vector3(
            timingTextureScales[index],
            timingTextureScales[index],
            timingTextureScales[index]
        );

        Debug.Log("affichage => " + index);
    }
}
