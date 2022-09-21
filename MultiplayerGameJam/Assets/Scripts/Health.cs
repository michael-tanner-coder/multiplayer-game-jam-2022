using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp;
    public float maxHP;
    public Image healthbar;

    void TakeDamage(float damage) 
    {
        hp -= damage;
        if (hp <= 0) 
        {
            hp = 0;
        }
    }

    void RecoverHealth(float recoverAmount) 
    {
        hp += recoverAmount;
        if (hp > maxHP) 
        {
            hp = maxHP;
        }
    }

    void Update()
    {
        Transform healthbarTransform = healthbar.GetComponent<Transform>();
        healthbarTransform.position = gameObject.transform.position;
        healthbar.fillAmount = hp / maxHP;

        if (hp <= 0) 
        {
            Destroy(gameObject);
            EventManager.current.DestructionTrigger();
        }
    }
}
