using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerBehavior : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private GameObject Visual;
    
    public AnimationMachine MyMachine;
    private CurrentTask Task;
    
    public enum CurrentTask
    {
        WorkingMachine,
    }

    void Update()
    {
        switch (Task)
        {
            case CurrentTask.WorkingMachine:
                WorkingInMachine();
                break;
        }
    }


    private bool insideMachine = false;
    void WorkingInMachine()
    {
        //Walk To / Work In Machine
        if (insideMachine)
        {
            MyMachine.Work();
        }
        else
        {
            Agent.destination = MyMachine.transform.position;
            if (Vector3.Distance(transform.position, MyMachine.transform.position) <= 0.22)
            {
                insideMachine = true;
            }
        }
    }
}
