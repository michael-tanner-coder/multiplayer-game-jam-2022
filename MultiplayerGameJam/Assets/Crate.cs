using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IDestructible
{
    [SerializeField] private PartScriptableObject _partType;
    [SerializeField] private GameObject _partDrop;
    [SerializeField] private List<PartScriptableObject> _potentialDrops;

    public void Destruct() 
    {
        GameObject partDrop = Instantiate(_partDrop, transform.position, Quaternion.identity);
        PartBehavior partBehavior = partDrop.GetComponent<PartBehavior>();

        var randomIndex = Random.Range(0, _potentialDrops.Count);
        partBehavior.part = _potentialDrops[randomIndex];
        
        Destroy(gameObject);
        
        SoundManager.instance.Play("Explosion");
    }
}
