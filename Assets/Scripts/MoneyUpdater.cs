using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUpdater : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float cMoney = GlobalGameState.getGameState().getMoney();
        float MoneyPerSec = GlobalGameState.getGameState().MoneyPerSec;

        Text.text = MoneyString.MoneyToString(cMoney) + "\n" +
                    MoneyString.MoneyToString(MoneyPerSec) + "/s";
    }
}
