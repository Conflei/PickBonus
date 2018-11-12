using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameView : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup gameCG;

    [Header("Start UI")]
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Text walletText;

    [SerializeField]
    private Text betText;

    [SerializeField]
    private Text currentPrize;

    [SerializeField]
    private CanvasGroup startUICG;

    [Header("Game View UI")]

    [SerializeField]
    private GameObject currentPrizeGO;
    [SerializeField]
    private GameObject currentPrizeLastPos;

    [SerializeField]
    private CanvasGroup grid;
    [SerializeField]
    private GameObject[] chests;

    [SerializeField]
    private GameObject playAgainGO;
    [SerializeField]
    private GameObject playAgainLastPos;

    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        FadeOut();
        SetUpScreen();
    }

    public void FadeIn()
    {
        gameCG.DOFade(1f, 1f);
    }

    public void FadeOut()
    {
        gameCG.DOFade(0f, 1f);
    }
    #region Intro

    public void SetUpScreen()
    {
        currentPrize.text = "Current Prize: 0";
        walletText.text = "$ " + BeautifyF(gameController.user.CurrentMoney);
        betText.text = "$ " + gameController.user.CurrentBet;
    }

    public void UpdateCurrentMoney()
    {
        walletText.text = "$ " + BeautifyF(gameController.user.CurrentMoney);
    }

    public void IncreaseBet()
    {
        betText.text = "$" + gameController.user.IncreaseBet();
    }

    public void DecreaseBet()
    {
        betText.text = "$" + gameController.user.DecreaseBet();
    }
    public void StartGame()
    {
        gameController.StartGame();
        AnimateStartGameUI();
    }

    public void AnimateStartGameUI()
    {
        //Debug.Log("tween");
        iTween.MoveTo(currentPrizeGO, iTween.Hash("position", currentPrizeLastPos.transform.position, "time", 1f, "easeType", iTween.EaseType.easeOutBounce));
        startUICG.DOFade(0.6f, 1f);
        startUICG.interactable = false;
        //Roar
        StartCoroutine(AnimateChests());
        //currentPrizeGO.transform.DOMove(currentPrizeLastPos.transform.position, 2f);
    }

    public IEnumerator AnimateChests()
    {
        yield return new WaitForSeconds(1f);
        iTween.ScaleTo(chests[0], iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutBounce));
        iTween.ScaleTo(chests[2], iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutBounce));
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(chests[4], iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutBounce));
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(chests[6], iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutBounce));
        iTween.ScaleTo(chests[8], iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutBounce));
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(chests[1], iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutBounce));
        iTween.ScaleTo(chests[3], iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutBounce));
        iTween.ScaleTo(chests[5], iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutBounce));
        iTween.ScaleTo(chests[7], iTween.Hash("scale", Vector3.one, "time", 0.75f, "easeType", iTween.EaseType.easeOutBounce));
        yield return null;

        grid.interactable = true;
    }
    #endregion

    #region GameView

    public void ChangePrizeText(float f)
    {
        currentPrize.text = "Current Prize: $"+BeautifyF(f);
        StartCoroutine(BouncePrize());
    }

    public IEnumerator BouncePrize()
    {
        iTween.ScaleTo(currentPrize.gameObject, iTween.Hash("scale", Vector3.one * 1.15f, "time", 0.75f, "easeType", iTween.EaseType.easeInExpo));
        yield return new WaitForSeconds(1f);
        iTween.ScaleTo(currentPrize.gameObject, iTween.Hash("scale", Vector3.one , "time", 0.3f, "easeType", iTween.EaseType.easeOutExpo));
    }

    public static string BeautifyF(float f)
    {
        string finalText = f.ToString();

        string sRaw = f.ToString();
        string[] splitted = sRaw.Split('.');

        if (!sRaw.Contains("."))
            finalText = f + ".00";
        else
            if (splitted[1].Length < 2) 
                finalText = f + "0";
            else
                if (splitted[1].Length > 2)
                    finalText = User.Trunc(f) + "";


        return finalText;
    }

    public void EndGame()
    {
        iTween.MoveTo(playAgainGO, iTween.Hash("position", playAgainLastPos.transform.position, "time", 1.5f, "easeType", iTween.EaseType.easeOutExpo));
    }

    #endregion

}
