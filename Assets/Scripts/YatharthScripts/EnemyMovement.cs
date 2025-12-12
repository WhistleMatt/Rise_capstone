using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float strollRadius; 
    public float waitTime = 2f;     
    private NavMeshAgent navMeshAgent;
    private float waitTimer = 0f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                MoveToRandomPoint();
            }
        }
    }

    void MoveToRandomPoint()
    {
        Vector3 randomPoint = GetRandomPointWithinRadius();

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, strollRadius, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning("Could not find a valid NavMesh position near " + randomPoint);
        }
    }

    Vector3 GetRandomPointWithinRadius()
    {
        Vector3 randomDirection = Random.insideUnitSphere * strollRadius;
        randomDirection += transform.position; 
        randomDirection.y = transform.position.y; 

        return randomDirection;
    }

}
