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

    public void Init(bool isPooper, int index)
    {
        this.isPooper = isPooper;
        this.index = index;
        currentImage = this.gameObject.GetComponent<Image>();
        grid = this.transform.parent.GetComponent<GameGrid>();
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
        PrizeText.gameObject.SetActive(true);
        PrizeText.text = "$" + GameView.BeautifyF(prize);
        yield return new WaitForSeconds(2f);
        PrizeText.gameObject.SetActive(false);
    }
}
