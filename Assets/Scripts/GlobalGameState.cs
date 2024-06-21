using System.Collections.Generic;
using UnityEngine;

public class GlobalGameState : MonoBehaviour
{
    private int Money;
    private static GlobalGameState GGS;
    private List<GameObject> Workers;
    
    private void Awake()
    {
        if (GGS != null && GGS != this)
        {
            Destroy(this.gameObject);
        } else {
            GGS = this;
            DontDestroyOnLoad(this);
        }
    }

    public void addWorker(GameObject newWorker)
    {
        Workers.Add(newWorker);
    }

    public static GlobalGameState getGameState()
    {
        return GGS;
    }

    public int getMoney()
    {
        return Money;
    }

    public void addMoney(int Dollars)
    {
        Money += Dollars;
    }
}