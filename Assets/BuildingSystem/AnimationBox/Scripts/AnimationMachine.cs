using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMachine : MonoBehaviour
{
    public struct MachineStats
    {
        public int MoneyMade;
        public int UpgradeLvl;
        public float Stress;
    }

    public MachineStats Stats;
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

    public void Work()
    {
        Debug.Log("Working!!!!");
    }
}
