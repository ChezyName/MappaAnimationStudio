using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMachine : MonoBehaviour
{
    public struct MachineStats
    {
        public float MoneyMade;
        public int UpgradeLvl;

        public float UpgradeCost;
    }

    public const float UPGRADE_COST_DEFAULT = 300;
    public const float WORKER_COST_DEFAULT = 300;

    private GlobalGameState GGS;
    
    private MachineStats Stats;
    public TextMeshProUGUI Text;

    public Button WorkerButton;
    public TextMeshProUGUI UpgradeText;
    public TextMeshProUGUI WorkerText;
    
    public GameObject Worker;

    public void SpawnWorker()
    {
        if (GGS.getMoney() < WORKER_COST_DEFAULT) return;
        
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

        UpgradeText.text = "Upgrade: $" + Stats.UpgradeCost.ToString("0.00");
    }

    public void doUpgrade()
    {
        if (GGS.getMoney() >= Stats.UpgradeCost)
        {
            Stats.UpgradeLvl += 1;
            setStats();
            GGS.addMoney(-Stats.UpgradeCost);
            UpdateUpgradeCost();
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
        Stats.MoneyMade += AddMoneyAmount * Time.deltaTime;
        GGS.addMoney(AddMoneyAmount * Time.deltaTime);
        setStats();
    }

    public void setStats()
    {
        Text.text = "Money Made: $" + Stats.MoneyMade.ToString("0.00") + "\n" +
               "Upgrade Lvl: " + Stats.UpgradeLvl;
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
