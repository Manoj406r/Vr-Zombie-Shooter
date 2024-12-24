using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private CapsuleCollider caps;
    public Transform player;
    public float attackRange = 2f;
    public float detectionRadius = 15f;
    public float speed = 3.5f;

    public Animator animator;
    public NavMeshAgent enemy;

    private bool isFollowingPlayer = false;
    private bool isAttacking = false;
    private bool isReacting = false;

    private Vector3 patrolTarget;
    public float patrolRadius = 10f;
    public float patrolWaitTime = 3f;
    private float patrolTimer;

    private float maxhealth = 100f;
    private float currenthealth;
    private float bulletdamage = 20f;
    private float Axedamage = 30f;

    public AudioSource zombieattack;
    public AudioSource zombiedamage;
    public AudioSource zombie_follow;

    void Start()
    {
        caps = GetComponent<CapsuleCollider>();
        enemy = GetComponent<NavMeshAgent>();
        enemy.speed = speed;
        SetNewPatrolTarget();

        // Initialize health
        currenthealth = maxhealth;
        zombie_follow.loop = true; // Follow audio should loop
        zombieattack.loop = true;  // Attack audio should loop
        zombiedamage.loop = false;
    }

    void Update()
    {
        if (isReacting) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isFollowingPlayer = true;
        }
        else
        {
            isFollowingPlayer = false;
        }

        if (isFollowingPlayer)
        {
            FollowPlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    void FollowPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            enemy.isStopped = true;
            animator.SetBool("IsWalking", false);
            animator.SetBool("Attack", true);

            if (!zombieattack.isPlaying) // Play attack audio only if not already playing
            {
                zombieattack.Play();
            }

            if (zombie_follow.isPlaying) // Stop follow audio when attacking
            {
                zombie_follow.Stop();
            }
        }
        else
        {
            isAttacking = false;
            enemy.isStopped = false;
            enemy.SetDestination(player.position);
            animator.SetBool("Attack", false);
            animator.SetBool("IsWalking", true);

            if (!zombie_follow.isPlaying) // Play follow audio only if not already playing
            {
                zombie_follow.Play();
            }

            if (zombieattack.isPlaying) // Stop attack audio when following
            {
                zombieattack.Stop();
            }
        }
    }

    void Patrol()
    {
        isAttacking = false;
        animator.SetBool("Attack", false);

        if (enemy.remainingDistance <= enemy.stoppingDistance && !enemy.pathPending)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolTarget();
                patrolTimer = 0f;
            }
        }
        else
        {
            patrolTimer = 0f;
        }

        enemy.isStopped = false;
        enemy.SetDestination(patrolTarget);
        animator.SetBool("IsWalking", true);

        if (zombie_follow.isPlaying) // Stop follow audio while patrolling
        {
            zombie_follow.Stop();
        }
    }

    void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = navHit.position;
        }
        else
        {
            patrolTarget = transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Reactiontohit();
            Damage(bulletdamage);
            Debug.Log("Bullet hit");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Axe"))
        {
            Reactiontohit2();
            Damage(Axedamage);
            Debug.Log("Axe hit");
        }
    }

    void Damage(float damage)
    {
        currenthealth -= damage;
        if (currenthealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("Death");

        enemy.isStopped = true;
        enemy.ResetPath();

        animator.SetBool("IsWalking", false);
        animator.SetBool("Attack", false);

        caps.enabled = false;
        this.enabled = false;
    }

    void Reactiontohit()
    {
        if (isReacting) return;

        if (!zombiedamage.isPlaying) // Play damage audio only if not already playing
        {
            zombiedamage.Play();
        }

        isReacting = true;
        enemy.isStopped = true;

        animator.SetBool("React", true);
        StartCoroutine(endreaction());
    }

    void Reactiontohit2()
    {
        if (isReacting) return;

        if (!zombiedamage.isPlaying) // Play damage audio only if not already playing
        {
            zombiedamage.Play();
        }

        isReacting = true;
        enemy.isStopped = true;

        animator.SetBool("React2", true);
        StartCoroutine(endreaction());
    }

    IEnumerator endreaction()
    {
        yield return new WaitForSeconds(2f);
        isReacting = false;

        if (isAttacking)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("IsWalking", true);
        }

        animator.SetBool("React", false);
        animator.SetBool("React2", false);
        enemy.isStopped = false;
    }
}
