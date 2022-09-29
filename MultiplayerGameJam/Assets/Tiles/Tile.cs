using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Tile", order = 2)]
public class Tile : ScriptableObject
{
    public TileBase[] tiles;
    public float friction;
    public bool isGap;
    public string name;
}
