using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAlphaAnimationController : UIAnimationBaseController
{
    [SerializeField] private float startValue;
    [SerializeField] private float targetValue;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public override void Initialize()
    {
        base.Initialize();

        ChangeAlpha(startValue, 0);
        //ChangeAlpha(targetValue, speed);
    }

    public void ChangeAlpha(float targetAlpha, float duration)
    {
        if (tween != null)
        {
            LeanTween.cancel(tween.uniqueId);
        }

        Color currentColor = image.color;


        tween = LeanTween.value(gameObject, currentColor.a, targetAlpha, duration)
            .setOnUpdate((float value) => {
                currentColor.a = value;
                image.color = currentColor;
            }).setDelay(delay).setOnComplete(()=>ChangeAlpha(targetValue, speed));
    }

    public override void Conclude()
    {
        if (tween != null)
        {
            LeanTween.cancel(tween.uniqueId);
        }

        base.Conclude();
    }
}
