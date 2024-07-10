using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationUpgrader : MonoBehaviour
{
    public TextMeshProUGUI UpgradeCoster;
    public TextMeshProUGUI Stats;
    public GameObject Spinner;
    public float Radius;

    private float NextUpgradeCost = 0;
    private int UpgradeLvl = 0;
    private float MoneyMade = 0;
    private float MoneyPerSec;

    private const int MAX_LVL = 10;
    private const float UPGRADE_COST_DEFAULT = 500;
    private const float MONEY_PER_SEC = 50;
    
    // Start is called before the first frame update
    void Start()
    {
        GlobalGameState.getGameState().UpgradeMachines++;
        Upgrade();
    }

    public void Upgrade()
    {
        if (GlobalGameState.getGameState().getMoney() < NextUpgradeCost || UpgradeLvl == MAX_LVL) return;
        UpgradeLvl++;
        Radius = UpgradeLvl;
        Spinner.transform.localScale = new Vector3(Radius,Radius,Radius);
        NextUpgradeCost = Math.Max(
            UPGRADE_COST_DEFAULT,
            (float) Math.Sqrt(
                Math.Pow(GlobalGameState.getGameState().UpgradeMachines,2) *
                Math.Pow(UPGRADE_COST_DEFAULT,2) *
                Math.Pow(UpgradeLvl,2)
            ));
        UpgradeCoster.text = "$" + NextUpgradeCost.ToString("0.00");
        if (UpgradeLvl == MAX_LVL) UpgradeCoster.text = "MAX";
        setText();
    }

    public void setText()
    {
        Stats.text = "Upgrade Lvl: " + (UpgradeLvl == MAX_LVL ? "MAX" : UpgradeLvl) + "\n\n" +
                     "Money Per Sec: " + "\n" + MoneyString.MoneyToString(MoneyPerSec) +"\n"+
                     "Money Made: " + "\n" + MoneyString.MoneyToString(MoneyMade) +
                     (MoneyMade == 0 ? "\nMachine Needs Animation Machines To Work!" : "");
    }

    // Update is called once per frame
    void Update()
    {
        int upgrades = 1;
        int count = 1;
        foreach (Collider c in Physics.OverlapSphere(Spinner.transform.position, Radius))
        {
            if ( c.GetComponent<AnimationMachine>() && c.GetComponent<AnimationMachine>().Worker != null ||
                 c.GetComponentInParent<AnimationMachine>() && c.GetComponentInParent<AnimationMachine>().Worker != null || 
                 c.GetComponentInChildren<AnimationMachine>() && c.GetComponentInChildren<AnimationMachine>().Worker != null)
            {
                int Upgrade = 1;

                //Get Upgrade 
                if (c.GetComponent<AnimationMachine>())
                    Upgrade = c.GetComponent<AnimationMachine>().getStats().UpgradeLvl;
                else if (c.GetComponentInParent<AnimationMachine>())
                    Upgrade = c.GetComponentInParent<AnimationMachine>().getStats().UpgradeLvl;
                else if (c.GetComponentInChildren<AnimationMachine>())
                    Upgrade = c.GetComponentInChildren<AnimationMachine>().getStats().UpgradeLvl;
                
                upgrades += Upgrade;
                count++;
            }
        }

        //Add The Cash
        MoneyPerSec = (float) (MONEY_PER_SEC * Math.Pow(upgrades + UpgradeLvl/count, 2));
        MoneyPerSec = Math.Clamp(MoneyPerSec, 0, 100000);
        MoneyMade += MoneyPerSec * Time.deltaTime;
        GlobalGameState.getGameState().addMoney(MoneyPerSec * Time.deltaTime);
        setText();
    }
}
