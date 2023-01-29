using System;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextTweening : MonoBehaviour
{
    public TextMeshProUGUI myText;

    private Tween a;

    public void AnimateText(AudioClip clip)
    {
        string text = myText.text;
        myText.text = String.Empty;
        a = myText.DOText(text, clip.length);
    }

    public void CancelAnimations()
    {
        if(a != null)
            a.Kill();
    }
}