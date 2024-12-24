using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;



public class dummyenemy : MonoBehaviour
{
    public Transform player;
    public float attackrange;
    public float detectionradius;
    public float speed;

    public Animator animator;
    private NavMeshAgent navemesh;

    private bool isfollowingplayer = false;
    private bool isattacking = false;

    private Vector3 patroltarget;

    public float patrolradius;
    public float patrolwaittime;
    private float patroltimer;


    void Start()
    {
        navemesh = GetComponent<NavMeshAgent>();
        navemesh.speed = speed;
        setnewpatroltarget();
        
    }


    
    void Update()
    {
        float distancetoplayer = Vector3.Distance(player.position,transform.position);

        if(distancetoplayer <= detectionradius)
        {
            isfollowingplayer = true;
        }
        else
        {
            isfollowingplayer= false;
        }
        if (isfollowingplayer)
        {
            followplayer(distancetoplayer);
        }  
        else
        {
            patrol();
        }
    }
    void followplayer(float distancetoplayer)
    {
        if(distancetoplayer <= attackrange)
        {
            isattacking = true;
            navemesh.isStopped = true;
            animator.SetBool("IsWalking", false);
            
            animator.SetBool("Attack", true);
        }
        else
        {
            isattacking= false;
            navemesh.isStopped= false;
            navemesh.SetDestination(player.position);
            animator.SetBool("IsWalking", true);

            animator.SetBool("Attack", false);
        }

    }
    void patrol()
    {
        isattacking = false;
        animator.SetBool("Attack",false);

        if(navemesh.remainingDistance <= navemesh.stoppingDistance && !navemesh.pathPending)
        {
            patroltimer += Time.deltaTime;
            if(patroltimer >= patrolwaittime)
            {
                setnewpatroltarget();
                patroltimer = 0;

            }
            else
            {
                patroltimer = 0;
            }
        }
        navemesh.isStopped = false;
        navemesh.SetDestination(patroltarget);
        animator.SetBool("IsWalking", true);


    }
    void setnewpatroltarget()
    {
        Vector3 randomdirection = UnityEngine.Random.insideUnitSphere * patrolradius;
        randomdirection += transform.position;

        NavMeshHit navhit;

        if(NavMesh.SamplePosition(randomdirection,out navhit,patrolradius,NavMesh.AllAreas))
         {
            patroltarget = navhit.position;

        }
        else
        {
            patroltarget =transform.position;
        }

        
    }

}
