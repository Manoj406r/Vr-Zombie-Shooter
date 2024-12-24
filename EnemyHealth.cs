using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxhealth = 100f;
    public float currentheakth;
    

    private void Start()
    {
        currentheakth = maxhealth;
    }
    public void TakeDamage(float damage)
    {
        
        currentheakth -= damage;
        if(currentheakth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

