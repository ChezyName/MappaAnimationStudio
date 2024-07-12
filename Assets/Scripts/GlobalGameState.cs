using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameState : MonoBehaviour
{
    private float Money = 2500;
    public float MoneyPerSec = 0;
    
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

    private void Update()
    {
        //get all machines
        float cMoneyPerSec = 0;

        foreach (AnimationMachine Machine in FindObjectsOfType<AnimationMachine>())
        {
            cMoneyPerSec += Machine.getStats().MoneyPerSec;
        }
        foreach (AnimationUpgrader Machine in FindObjectsOfType<AnimationUpgrader>())
        {
            cMoneyPerSec += Machine.getMPS();
        }

        MoneyPerSec = cMoneyPerSec;
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