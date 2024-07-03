using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSpawner : MonoBehaviour
{
    private GlobalGameState GGS;

    [SerializeField]
    private GameObject Worker;

    void Start()
    {
        GGS = FindFirstObjectByType<GlobalGameState>();
    }

    public void SpawnWorker(GameObject spawnableWorker)
    {
        //Spawns a worker at location with self as Parent
        GameObject newWorker = Instantiate(spawnableWorker, transform.position, transform.rotation, transform);
        
        if (GGS == null) GGS = GlobalGameState.getGameState();
        else GGS.addWorker(newWorker);
    }
    
    public GameObject SpawnWorkerReturn()
    {
        //Spawns a worker at location with self as Parent
        GameObject newWorker = Instantiate(Worker, transform.position, transform.rotation, transform);
        return newWorker;
    }

    void Update()
    {
        //DEV / DEBUG
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnWorker(Worker);
        }
    }
}
