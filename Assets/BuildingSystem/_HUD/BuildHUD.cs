using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildHUD : MonoBehaviour
{
    [SerializeField]
    private Placeable[] Placeables;

    [SerializeField] private Button PlaceableButton;
    
    [SerializeField] private GameObject ButtonParent;
    
    [SerializeField] private GameObject HUD;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(Placeable placeable in Placeables)
        {
            //Create New Button For Object
            Button b = Instantiate(PlaceableButton, Vector3.zero, Quaternion.identity) as Button;
            b.transform.parent = ButtonParent.transform;
            b.onClick.AddListener(delegate{onPlaceableButtonClicked(placeable);});
            b.GetComponentInChildren<TextMeshProUGUI>().text = placeable.name;
        }
    }

    void onPlaceableButtonClicked(Placeable placeable)
    {
        Debug.Log(placeable.name + " has been clicked...");
        GlobalGameState.getGameState().currentPlaceable = placeable;
    }

    // Update is called once per frame
    void Update()
    {
        bool isVis = GlobalGameState.getGameState().isBuildMode;
        HUD.SetActive(isVis);
    }
}
