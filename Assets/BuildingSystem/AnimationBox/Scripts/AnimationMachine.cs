using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMachine : MonoBehaviour
{
    public struct MachineStats
    {
        public float MoneyMade;
        public float MoneyPerSec;
        
        public int UpgradeLvl;

        public float UpgradeCost;
    }

    public const float UPGRADE_COST_DEFAULT = 300;
    public const float WORKER_COST_DEFAULT = 300;

    private GlobalGameState GGS;
    
    private MachineStats Stats;
    public TextMeshProUGUI Text;

    public Button WorkerButton;
    public Button UpgradeButton;
    
    public TextMeshProUGUI UpgradeText;
    public TextMeshProUGUI WorkerText;
    
    public GameObject Worker;
    public bool Working = false;

    public MachineStats getStats()
    {
        return Stats;
    }

    public void SpawnWorker()
    {
        if (GGS.getMoney() < WORKER_COST_DEFAULT) return;
        GGS.addMoney(-WORKER_COST_DEFAULT);
        
        if (Worker != null) return;

        WorkerSpawner WS = FindFirstObjectByType<WorkerSpawner>();

        if (WS != null)
        {
            Worker = WS.SpawnWorkerReturn();
            Worker.GetComponent<WorkerBehavior>().MyMachine = this;
            WorkerButton.enabled = false;
            WorkerText.text = "WORKER EMPLOYED!";
        }
    }

    void UpdateUpgradeCost()
    {
        //M = Machine Amount - GGS>AnimationMachines
        //C = Default Cost
        //U = Upgrade Amount
        //y = sqrt(M^2 * U^2 * C^2)
        Stats.UpgradeCost = Math.Max(
            UPGRADE_COST_DEFAULT,
            (float) Math.Sqrt(
                Math.Pow(GGS.AnimationMachines,2) *
                Math.Pow(UPGRADE_COST_DEFAULT,2) *
                Math.Pow(Stats.UpgradeLvl + 1,2)
                ));

        if(Stats.UpgradeLvl < 10) UpgradeText.text = "Upgrade: " + Stats.UpgradeCost.ToString("0.00");
    }

    public void doUpgrade()
    {
        if (Stats.UpgradeLvl == 10) return;
        
        if (GGS.getMoney() >= Stats.UpgradeCost)
        {
            Stats.UpgradeLvl += 1;
            setStats();
            GGS.addMoney(-Stats.UpgradeCost);
            UpdateUpgradeCost();

            if (Stats.UpgradeLvl == 10)
            {
                UpgradeText.text = "MAX UPGRADE";
                UpgradeButton.enabled = false;
            }
        }
    }

    private void Start()
    {
        Stats.UpgradeLvl = 1;
        GGS = GlobalGameState.getGameState();
        GGS.AnimationMachines++;
        setStats();
        UpdateUpgradeCost();
        UpdateAllOtherMachines();
        WorkerText.text = "Hire Employee: $" + WORKER_COST_DEFAULT.ToString("0.00");
    }

    public void Work()
    {
        float AddMoneyAmount = (float)Math.Pow(Stats.UpgradeLvl, 2);
        Stats.MoneyPerSec = AddMoneyAmount;
        Stats.MoneyMade += AddMoneyAmount * Time.deltaTime;
        GGS.addMoney(AddMoneyAmount * Time.deltaTime);
        setStats();
        Working = true;
    }

    public void setStats()
    {
        Text.text = "Money Per Sec: " + MoneyString.MoneyToString(Stats.MoneyPerSec) + "\n" + 
                "Money Made: " + MoneyString.MoneyToString(Stats.MoneyMade) + "\n\n" +
                "Upgrade Lvl: " + (Stats.UpgradeLvl == 10 ? "MAX" : Stats.UpgradeLvl);
    }

    void UpdateAllOtherMachines()
    {
        foreach (AnimationMachine Machines in FindObjectsOfType<AnimationMachine>())
        {
            Machines.UpdateUpgradeCost();
        }
    }

    private void OnDestroy()
    {
        GGS.AnimationMachines--;
        
        if (Worker != null)
        {
            Destroy(Worker);
        }
        
        UpdateAllOtherMachines();
    }
}
