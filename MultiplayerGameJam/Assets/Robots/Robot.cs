using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Robot", order = 1)]
public class Robot : ScriptableObject
{
    public string name;
    public PartScriptableObject weapon;
    public PartScriptableObject mobility;
    public PartScriptableObject targeting;
}
