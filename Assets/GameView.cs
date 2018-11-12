using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{


    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Text walletText;

    [SerializeField]
    private Text betText;

    [SerializeField]
    private Text currentPrize;

    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        SetUpScreen();
    }
    #region Intro

    public void SetUpScreen()
    {
        currentPrize.text = "Current Prize: 0";
        walletText.text = "$ " + BeautifyF(gameController.user.CurrentMoney);
        betText.text = "$ " + gameController.user.CurrentBet;
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
    }
    #endregion

    #region GameView

    public void ChangePrizeText(float f)
    {
    

        currentPrize.text = "Current Prize: $"+BeautifyF(f);
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

    #endregion

}
