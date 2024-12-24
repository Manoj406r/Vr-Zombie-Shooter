using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axescript : MonoBehaviour
{
    public float damage = 15f; // Adjust damage for the axe
    public GameObject hitEffectPrefab; // Optional hit effect prefab

    private void OnTriggerEnter(Collider other)
    {
        // Check if the axe hits an enemy
        if (other.CompareTag("Enemy"))
        {
            // Spawn hit effect at the point of impact
            SpawnEffect(other.ClosestPoint(transform.position), transform.forward);

            
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    private void SpawnEffect(Vector3 position, Vector3 normal)
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, position, Quaternion.LookRotation(normal));
            Destroy(effect, 2f);
        }
    }
}
