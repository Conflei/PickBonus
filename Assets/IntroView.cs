using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroView : MonoBehaviour {


    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Text walletText;

    [SerializeField]
    private Text betText;

	// Use this for initialization
    IEnumerator Start () {
        yield return new WaitForSeconds(0.5f);
        SetUpScreen();
	}

    public void SetUpScreen()
    {
        walletText.text = "" + gameController.user.CurrentMoney;
        betText.text = "$" + gameController.user.CurrentBet;
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
	

}
