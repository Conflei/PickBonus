using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour {


    public bool isPooper;

    public Sprite winSprite;
    public Sprite loseSprite;
    public Text PrizeText;

    Image currentImage;

    private int index;

    private GameGrid grid;
    private Transform prizeLastPos;

    public void Init(bool isPooper, int index)
    {
        this.isPooper = isPooper;
        this.index = index;
        currentImage = this.gameObject.GetComponent<Image>();
        grid = this.transform.parent.GetComponent<GameGrid>();

        prizeLastPos = this.transform.GetChild(1);
    }

    public void UserClick()
    {
        this.gameObject.GetComponent<Button>().interactable = false;

        if(isPooper) // This is for static poopers
        {
            grid.PooperSelected();
            ShowPooper();

        }
        else
        {
            grid.OpenChest(index);
        }

    }

    public void Open(float prize = 0f)
    {
        currentImage.sprite = winSprite;
        StartCoroutine(ShowPrizeAnimation(prize));

    }

    public void ShowPooper()
    {
        this.currentImage.sprite = loseSprite;
    }

    public IEnumerator ShowPrizeAnimation(float prize)
    {

        float multiplier = 1f;
        if (prize > 1f) multiplier = 1.25f;
        if (prize > 50f) multiplier = 1.5f;
        PrizeText.gameObject.SetActive(true);
        PrizeText.text = "+$" + GameView.BeautifyF(prize);
        iTween.ScaleTo(PrizeText.gameObject, iTween.Hash("scale", Vector3.one*multiplier, "time", 0.5f, "easeType", iTween.EaseType.easeOutExpo));
        yield return new WaitForSeconds(0.5f);
        iTween.MoveTo(PrizeText.gameObject, iTween.Hash("position", prizeLastPos.position, "time", 1.25f, "easeType", iTween.EaseType.easeInExpo));
        yield return new WaitForSeconds(5f);
        PrizeText.gameObject.SetActive(false);
    }
}
