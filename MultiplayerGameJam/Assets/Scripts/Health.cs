using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp;
    public float maxHP;
    public GameObject healthbar;
    private GameObject canvas;
    private GameObject hpBar;

    public void Start() 
    {
        GameObject canvas = GameObject.Find("WorldCanvas");
        hpBar = Instantiate(healthbar);
        hpBar.transform.parent = canvas.GetComponent<Transform>();
    }

    public void TakeDamage(float damage) 
    {
        hp -= damage;
        if (hp <= 0) 
        {
            hp = 0;
        }
    }

    public void RecoverHealth(float recoverAmount) 
    {
        hp += recoverAmount;
        if (hp > maxHP) 
        {
            hp = maxHP;
        }
    }

    void Update()
    {
        Image healthbarImage = hpBar.GetComponent<Image>();
        Transform healthbarTransform = healthbarImage.GetComponent<Transform>();
        healthbarTransform.position = gameObject.transform.position;
        healthbarTransform.position = new Vector3(healthbarTransform.position.x, healthbarTransform.position.y + 1.2f, healthbarTransform.position.z);
        healthbarImage.fillAmount = hp / maxHP;

        if (hp <= 0) 
        {
            Destroy(gameObject);
            EventManager.current.DestructionTrigger();
        }
    }
}
