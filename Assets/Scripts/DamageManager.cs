using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public int health;
    public int attackDamage;

    public void TakeDamage(int takenDamage)
    {
        health-=takenDamage;
        if(health <= 0 && this.tag=="Player")//ako je player umro
        {
            Debug.Log("player je umro");
        }
        else//ako je enemy
        {
            Debug.Log("enemy je umro");
        }
    }

    public void DealDamage(GameObject target)
    {
        DamageManager manager = target.GetComponent<DamageManager>();
        if (manager != null)
        {
            manager.TakeDamage(attackDamage);
        }
    }
}
