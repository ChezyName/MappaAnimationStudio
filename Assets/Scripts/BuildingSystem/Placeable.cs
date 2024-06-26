using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeable", menuName = "ScriptableObjects/PlaceableObject", order = 1)]
public class Placeable : ScriptableObject
{
    public GameObject Spawnable;
    public bool isWall = false;
    public bool isDestructible = true;
    public int Cost = 0;

    public string Name;
    public string Description;
}
