using System.Collections.Generic;
using UnityEngine;

public class GlobalGameState : MonoBehaviour
{
    private float Money = 1000;
    private static GlobalGameState GGS;
    private List<GameObject> Workers;
    public bool isBuildMode = false;
    public Placeable currentPlaceable;
    public int AnimationMachines;
    public int UpgradeMachines;
    
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

    public float getMoney()
    {
        return Money;
    }

    public void addMoney(float Dollars)
    {
        Money += Dollars;
    }
}