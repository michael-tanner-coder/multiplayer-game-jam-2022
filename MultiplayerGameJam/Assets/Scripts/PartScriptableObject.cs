using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType {MOBILITY, WEAPON, TARGETING};
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PartScriptableObject", order = 1)]
public class PartScriptableObject : ScriptableObject
{
    public string name;
    public PartType type;
}
