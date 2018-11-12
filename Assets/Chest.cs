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
        grid.OpenChest(index);
    }

    public void OpenChest(float prize = 0f)
    {
        if(isPooper || prize <= 0f)
        {
            this.currentImage.sprite = loseSprite;
            return;
        }

        currentImage.sprite = winSprite;
        StartCoroutine(ShowPrizeAnimation(prize));

    }

    public IEnumerator ShowPrizeAnimation(float prize)
    {
        PrizeText.gameObject.SetActive(true);
        PrizeText.text = "$" + GameView.BeautifyF(prize);
        yield return new WaitForSeconds(2f);
        PrizeText.gameObject.SetActive(false);
    }
}
