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
        Text.text = MoneyString.MoneyToString(GlobalGameState.getGameState().getMoney());
    }
}
