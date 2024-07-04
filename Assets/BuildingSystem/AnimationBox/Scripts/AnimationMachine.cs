using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationMachine : MonoBehaviour
{
    public struct MachineStats
    {
        public float MoneyMade;
        public int UpgradeLvl;
    }

    private MachineStats Stats;
    public TextMeshProUGUI Text;
    public GameObject Worker;

    public void SpawnWorker()
    {
        if (Worker != null) return;

        WorkerSpawner WS = FindFirstObjectByType<WorkerSpawner>();

        if (WS != null)
        {
            Worker = WS.SpawnWorkerReturn();
            Worker.GetComponent<WorkerBehavior>().MyMachine = this;
        }
    }

    public void doUpgrade()
    {
        Stats.UpgradeLvl += 1;
        setStats();
    }

    private void Start()
    {
        Stats.UpgradeLvl = 1;
        setStats();
    }

    public void Work()
    {
        Stats.MoneyMade += Stats.UpgradeLvl * Time.deltaTime;
        setStats();
    }

    public void setStats()
    {
        Text.text = "Money Made: $" + Stats.MoneyMade.ToString("0.00") + "\n" +
               "Upgrade Lvl: " + Stats.UpgradeLvl;
    }

    private void OnDestroy()
    {
        if (Worker != null)
        {
            Destroy(Worker);
        }
    }
}
