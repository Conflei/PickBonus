using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGrid : MonoBehaviour {

    [SerializeField]
    private Chest[] chests = null;

    [SerializeField]
    private GameController gameController = null;

    private int lastChest;

	public void Init()
    {
        for (int i = 0; i < chests.Length; i++)
            chests[i].Init(false, i);


        int randChest = Random.Range(0, 9);
        chests[randChest].Init(true, randChest);


    }

    public void OpenChest(int index)
    {
        lastChest = index;

        float newPrize = gameController.NextPrize();
        if (chests[index].isPooper || newPrize < 0f)
        {
            PooperSelected(); //The grid takes action on the Pooper
            chests[index].ShowPooper(); // The chest is informed that now he is a dynamic pooper
            return;
        }

        chests[index].Open(newPrize);


    }

    public void EveryonePooper()
    {
        for (int i = 0; i < chests.Length; i++)
            if (i != lastChest) chests[i].Init(true, i);
    }

    public void UninteractableChests()
    {
        foreach (var chest in chests) chest.gameObject.GetComponent<Button>().interactable = false;
    }

    public void PooperSelected()
    {
        UninteractableChests();
        gameController.EndGame();
    }

}

public class ChestSolution
{
    public List<float> winAmounts;
    public float multipliedBet;
    private int amountOfChests;

    Node solutionTree;

    public int iterationsOnSolution;

    public ChestSolution(float multipliedBet, int amountOfChests = 8)
    {
        winAmounts = new List<float>();
        this.multipliedBet = multipliedBet;
        this.amountOfChests = amountOfChests;

        solutionTree = new Node(multipliedBet);
        iterationsOnSolution = 0;

    }

    public bool GetChests()
    {
        iterationsOnSolution++;

        //Regular solution, tree solution using the 8 chests
        if(multipliedBet>=2f){
            solutionTree.left = null;
            solutionTree.right = null;
            winAmounts = new List<float>();

            FillTree(solutionTree);

            float acum = 0f;
            float sum = 0f;

            for (int i = 0; i < winAmounts.Count; i++)
            {
                float rawAmount = winAmounts[i];
                winAmounts[i] = NearestTrick(winAmounts[i]);

                if (winAmounts[i] <= 0f) winAmounts[i] = 0.05f;

                acum += winAmounts[i] - rawAmount;
                sum += winAmounts[i];

            }


            acum = acum * -1;
            sum = 0f;
            winAmounts.Sort();

            winAmounts[winAmounts.Count - 1] = winAmounts[winAmounts.Count - 1] + acum;
            for (int i = 1; i < winAmounts.Count; i++)
            {

                sum += winAmounts[i];
                if (winAmounts[i].ToString() == winAmounts[i - 1].ToString()) return false;
            }

            return true;
        }
        else
        {
            float[] possibleEntries = { 0.5f, 0.45f, 0.4f, 0.35f, 0.3f, 0.25f, 0.2f, 0.15f, 0.1f, 0.05f };
            float aux = multipliedBet;
            int i = 0;
            while(aux>0f)
            {
                Debug.Log(aux + " " + possibleEntries[i]);
                if(aux-possibleEntries[i]<0f){

                }else{
                    aux -= possibleEntries[i];
                    aux = User.Trunc(aux);
                    winAmounts.Add(possibleEntries[i]);
                    //possibleEntries.Remove(possibleEntries[i]);

                }
                i++;
            }
            winAmounts.Sort();
            winAmounts.Add(0f);
           
            return true;
        }
        //return true;

    }

    public float NearestTrick(float rawF)
    {
        string sRaw = rawF.ToString();
        string[] splitted = sRaw.Split('.');

        if (!sRaw.Contains(".")) return rawF;
        if (splitted[1].Length < 2) return rawF;

        string decimalPar = splitted[1];

        int lastD = int.Parse(decimalPar[1].ToString());

        if (lastD < 4) 
            sRaw = splitted[0] + "." + decimalPar[0] + "0";
        else
            sRaw = splitted[0] + "." + decimalPar[0] + "5";


        //Debug.Log(rawF + " => " + sRaw);
        return float.Parse(sRaw);
    }

    #region TreeSolution

    public bool IsCorrect()
    {
        float sum = winAmounts[0];
        for (int i = 1; i < winAmounts.Count; i++)
        {
            sum += winAmounts[i];
        }

        if (sum == multipliedBet) return true;

        return false;
    }

    public bool FillTree(Node node)
    {

        if (node == null) return false;

        if(node.degree == 3)//is leaf
        {
            winAmounts.Add(node.amount);
        }
        else
        {
            node.CreateLeafs();
        }

        FillTree(node.left);
        FillTree(node.right);
        return false;
    }



    #endregion

    public void ZeroSolution()
    {
        winAmounts = new List<float>();
        winAmounts.Add(0f);
        winAmounts.Add(0f);
    }
}

/// <summary>
/// Tree implementation
/// </summary>
public class Node
{
    public float amount;
    public int degree;

    public Node left;
    public Node right;

    public Node(float amount, int degree = 0)
    {
        this.amount = amount;
        this.degree = degree;
    }

    public void AddLeftLeaf(float amount)
    {
        left = new Node(amount, degree + 1);
    }

    public void AddRightLeaf(float amount)
    {
        right = new Node(amount, degree + 1);
    }

    public void CreateLeafs()
    {
        float randHalf = User.Trunc(Random.Range(0, amount));
        float otherHalf = User.Trunc(amount - randHalf);
        AddLeftLeaf(randHalf); AddRightLeaf(otherHalf);
    }

    public void DeleateLeafs()
    {

    }
}
