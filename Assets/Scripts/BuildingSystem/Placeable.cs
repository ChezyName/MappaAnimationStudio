using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlaceableObject", order = 1)]
public class Placeable : ScriptableObject
{
    public GameObject Spawnable;
    public bool isWall = false;
}
