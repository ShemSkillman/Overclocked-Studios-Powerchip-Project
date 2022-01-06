using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private Movement enemyMovement;
    private NavMeshAgent agent;
    private Vector3 velocity;
    private CharacterController controller;
    private Combat enemyCombat;

    [SerializeField] private Behaviour currentBehaviour = Behaviour.Idle;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float wanderRange = 5f;

    [SerializeField] private float minIdleTime = 1f, maxIdleTime = 5f;
    private float currentIdleTime, targetIdleTime;

    enum Behaviour { Idle, Wander, Pursue, Attack };

    void Start()
    {
        enemyMovement = GetComponent<Movement>();
        enemyCombat = GetComponent<Combat>();

        agent = gameObject.GetComponent<NavMeshAgent>();
        controller = gameObject.GetComponent<CharacterController>();

        StartIdleBehaviour();
    }

    void Update()
    {
        switch (currentBehaviour)
        {
            case Behaviour.Idle:
                UpdateIdleBehaviour();

                if (playerTransform != null)
                {
                    currentBehaviour = Behaviour.Pursue;
                }
                else if (currentIdleTime >= targetIdleTime)
                {
                    StartWanderBehaviour();
                    currentIdleTime = 0f;
                }
                break;

            case Behaviour.Wander:
                UpdateWanderBehaviour();

                if (playerTransform != null)
                {
                    currentBehaviour = Behaviour.Pursue;
                }
                else if (IsAtDestination())
                {
                    StartIdleBehaviour();
                }               
                break;

            case Behaviour.Pursue:                
                if (playerTransform == null)
                {
                    StartIdleBehaviour();
                }
                else
                {
                    UpdatePursueBehaviour();

                    if (IsPlayerInRange())
                    {
                        currentBehaviour = Behaviour.Attack;
                    }
                }                
                break;

            case Behaviour.Attack:
                if (!IsPlayerInRange())
                {
                    currentBehaviour = Behaviour.Pursue;
                }
                else
                {
                    UpdateAttackBehaviour();
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If not the player
        if(other.gameObject.tag != "Player")
        {
            return;
        }

        playerTransform = other.transform;
        Debug.Log("Player has entered the collider");
        agent.destination = playerTransform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        // If not the player
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        playerTransform = null;
        agent.destination = transform.position;
        Debug.Log("Player has exited the collider");
    }

    private void StartIdleBehaviour()
    {
        currentBehaviour = Behaviour.Idle;

        targetIdleTime = Random.Range(minIdleTime, maxIdleTime);
    }

    private void UpdateIdleBehaviour()
    {
        enemyMovement.Move(Vector3.zero);

        currentIdleTime += Time.deltaTime;
    }

    private void MoveToLocation()
    {
        velocity = agent.desiredVelocity;

        enemyMovement.Move(velocity.normalized);
        agent.velocity = controller.velocity;
    }

    private bool IsAtDestination()
    {
        NavMesh.SamplePosition(transform.position, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas);

        float dist = Vector3.Distance(hit.position, agent.destination);

        return dist <= 1f;
    }

    private void StartWanderBehaviour()
    {
        currentBehaviour = Behaviour.Wander;

        Vector3 wanderLocation = Random.insideUnitSphere;

        wanderLocation.y = 0f;

        Vector3 randomPos = wanderLocation.normalized * wanderRange;

        NavMesh.SamplePosition(transform.position + randomPos, out NavMeshHit hit, wanderRange, NavMesh.AllAreas);

        agent.destination = hit.position;
    }

    private void UpdateWanderBehaviour()
    {        
        MoveToLocation();
    }

    private void UpdatePursueBehaviour()
    {
        agent.destination = playerTransform.position;
        MoveToLocation();
    }

    private void UpdateAttackBehaviour()
    {
        enemyCombat.StartMeleeAttack();
        enemyMovement.Move(Vector3.zero);
    }

    private bool IsPlayerInRange()
    {
        Collider[] targetColliders = enemyCombat.GetTargetColliders();

        foreach (var collider in targetColliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
}
