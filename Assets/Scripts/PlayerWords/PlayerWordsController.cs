using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWordsController : MonoBehaviour
{
    [SerializeField] private Canvas wordCanvas;
    [SerializeField] private Sprite[] wordSprites;

    private UIAnimationBaseController[] canvasAnimations;
    private Image canvasImage;
    private bool isEnable;

    private void Awake()
    {
        canvasAnimations = wordCanvas.GetComponentsInChildren<UIAnimationBaseController>();
        canvasImage = wordCanvas.GetComponentInChildren<Image>();

        wordCanvas.gameObject.SetActive(false);
    }

    public void EnableWord()
    {
        if(!isEnable)
        {
            isEnable = true;

            wordCanvas.transform.position = transform.position;

            wordCanvas.gameObject.SetActive(true);

            canvasImage.sprite = wordSprites[Random.Range(0, wordSprites.Length)];

            foreach (UIAnimationBaseController animationController in canvasAnimations)
            {
                animationController.Initialize();
            }

            LeanTween.delayedCall(3f, DisableWord);
        }
    }

    private void DisableWord()
    {
        if(isEnable)
        {
            foreach (UIAnimationBaseController animationController in canvasAnimations)
            {
                animationController.Conclude();
            }

            isEnable = false;

            wordCanvas.gameObject.SetActive(false);
        }
    }
}
