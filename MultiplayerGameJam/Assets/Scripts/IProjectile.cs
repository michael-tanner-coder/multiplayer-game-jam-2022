using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void Init() {}
    void SetDirection(Vector3 direction) {}
    void SetDamage(float damage) {}
}
