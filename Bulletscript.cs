using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletscript : MonoBehaviour
{
    public float damage = 10f;
    public GameObject hiteffectPrefab;
    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            ContactPoint contact = collision.contacts[0];
            SpawnEffect(contact.point, contact.normal);
            EnemyHealth enemyHealth = collision.gameObject. GetComponent<EnemyHealth>();
            if(enemyHealth != null )
            {

                enemyHealth.TakeDamage(damage);
            }
            
            
        }
        Destroy(gameObject);
    }
    private void SpawnEffect(Vector3 position, Vector3 normal)
    {
        if (hiteffectPrefab != null)
        {
            GameObject effect = Instantiate(hiteffectPrefab, position, Quaternion.LookRotation(normal));
            Destroy(effect, 2f);
        }
    }

}
