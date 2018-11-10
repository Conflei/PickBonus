using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour {


    public bool isPooper;

    public Sprite winSprite;
    public Sprite loseSprite;

    Image currentImage;

    public void Init(bool isPooper)
    {
        this.isPooper = isPooper;
        currentImage = this.gameObject.GetComponent<Image>();
    }

    public void UserClick()
    {
        this.gameObject.GetComponent<Button>().interactable = false;
        if (isPooper) currentImage.sprite = loseSprite;
        else
        {
            currentImage.sprite = winSprite;
        }
    }
}
