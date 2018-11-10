using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour {

    [SerializeField]
    private Chest[] chests;

	public void Init()
    {
        foreach (var chest in chests) chest.Init(false);

        chests[Random.Range(0, 9)].Init(true);


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
