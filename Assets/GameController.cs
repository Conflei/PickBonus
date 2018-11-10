using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


/// <summary>
/// Game controller.
/// This controller affects both Intro and Game View
/// </summary>
public class GameController : MonoBehaviour
{

    [SerializeField]
    private IntroView introView;


    [SerializeField]
    private GameObject introViewGO;

    public User user;
    public GameGrid grid;

    int[] lowMultiplyers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    int[] medMultiplyers = { 12, 16, 24, 32, 48, 64 };
    int[] highMultiplyers = { 100, 200, 300, 400, 500 };
    private int multiplier;
    private float bet;

    // Use this for initialization
    void Start()
    {
        multiplier = SelectMultiplier();

        this.user = new User();
        grid.Init();
    }

    #region INTROVIEW

    public void StartGame()
    {
        //introViewGO.SetActive(false);
        Debug.Log("Start with Multiplier " + multiplier + "\nBet "+user.CurrentBet);

        StartCoroutine(GetSolution());
    }

    public IEnumerator GetSolution()
    {
        ChestSolution solution = new ChestSolution(multiplier * user.CurrentBet);
        while (!solution.GetChests())
            yield return null;

        Debug.Log("-------FINAL CHESTS!-----");
        float finalSum = 0f;
        for (int i = 0; i < solution.winAmounts.Count; i++)
        {
            Debug.Log("FINAL CHEST #" + i + " PRIZE: " + solution.winAmounts[i]);
            finalSum += solution.winAmounts[i];
        }
        Debug.Log("Total " + finalSum +"\nAmount of iterations: "+solution.iterationsOnSolution);
    }

    #endregion

    #region GAME

    /// <summary>
    /// Selects the multiplier based on the description of the Game
    /// 0 - 50% of the time (first chest is empty)
    ///1,2,3,4,5,6,7,8,9,10 - one of these 30% of the time
    ///12,16,24,32,48,64 - one of these 15% of the time
    ///100,200,300,400,500 - one of these 5% of the time
    /// </summary>
    /// <returns>The multiplier.</returns>
    public int SelectMultiplier()
    {
        int rangeProb = Random.Range(0, 100);
        int arrayToUse, multiplier = 0;

        if (rangeProb < 50)
        {

            arrayToUse = 0;

        }
        else
        {

            if (rangeProb < 80)
            {
                arrayToUse = 1;
            }
            else
            {

                if (rangeProb < 95)
                {
                    arrayToUse = 2;
                }
                else
                {
                    arrayToUse = 3;
                }

            }
        }
        Debug.Log(arrayToUse);
        switch (arrayToUse)
        {
            case 0:
                multiplier = 0;
                break;
            case 1:
                multiplier = lowMultiplyers[Random.Range(0, lowMultiplyers.Length)];
                break;
            case 2:
                multiplier = medMultiplyers[Random.Range(0, medMultiplyers.Length)];
                break;
            case 3:
                multiplier = highMultiplyers[Random.Range(0, highMultiplyers.Length)];
                break;
        }
        return multiplier;
    }

    /// <summary>
    /// Gets the prize. In this method I already made all the calculations using compound probabilities
    /// There's 1/9 chances to get a pooper = 0.111
    ///  50% Chance to get a ZEROMULTY if is NOT A POOPER => NOT A POOPER (0.889) * ISZEROMULTY (0.5) = 0.444
    ///  30% Chance to get a LOWMULTY if is NOT A POOPER => NOT A POOPER (0.889) * ISLOWMULTY (0.333) = 0.296
    ///  15% Chance to get a MEDMULTY if is NOT A POOPER => NOT A POOPER (0.889) * ISMEDMULTY (0.15) = 0.133
    ///  5% Chance to get a HIGHMULTY if is NOT A POOPER => NOT A POOPER (0.889) * ISMEDMULTY (0.05) = 0.044
    /// </summary>
    /// <returns>The prize.</returns>
    public int GetPrize()
    {

        return 0;
    }
    #endregion

}

#region User
public class User
{

    private float currentMoney;
    public float CurrentMoney { get { return this.currentMoney; } set { this.currentMoney = value; PlayerPrefs.SetFloat("Wallet", value); } }

    private float currentBet;
    public float CurrentBet { get { return this.currentBet; } set { this.currentBet = value; } }

    private float startingBet;
    public float StartingBet { get { return Trunc(0.25f); } set { this.startingBet = value; } }

    public User()
    {
        CurrentMoney = PlayerPrefs.GetFloat("Wallet") > 0f ? PlayerPrefs.GetFloat("Wallet") : 10f;
        CurrentBet = StartingBet;


        Debug.Log("User Created");
    }


    public string DecreaseBet()
    {

        switch (Trunc(CurrentBet).ToString())
        {
            case "0.25":
                CurrentBet = 0.25f;
                break;
            case "0.5":
                CurrentBet = 0.25f;
                break;
            case "1":
                CurrentBet = 0.5f;
                break;
            case "5":
                CurrentBet = 1f;
                break;
        }

        return FF(CurrentBet);
    }

    public string IncreaseBet()
    {
        Debug.Log("Increase: " + Trunc(CurrentBet).ToString());
        switch (Trunc(CurrentBet).ToString())
        {
            case "0.25":
                CurrentBet = 0.50f;
                break;
            case "0.5":
                CurrentBet = 1.0f;
                break;
            case "1":
                CurrentBet = 5.0f;
                break;
            case "5":
                CurrentBet = 5.0f;
                break;
        }
        Debug.Log("C" + CurrentBet);

        return FF(CurrentBet);
    }

    public static float Trunc(float f)
    {
        return Mathf.Round(f * 100f) / 100f;
    }

    static string FF(float f)//Final Float To Show
    {
        string finalFloat = "";

        switch (f.ToString())
        {
            case "0.25":
                finalFloat = "0.25";
                break;
            case "0.5":
                finalFloat = "0.50";
                break;
            case "1":
                finalFloat = "1.00";
                break;
            case "5":
                finalFloat = "5.00";
                break;
        }

        return finalFloat;
    }


}
#endregion
