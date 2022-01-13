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

    [SerializeField] private float minAttackIdleTime = 0.5f, maxAttackIdleTime = 1.5f;
    float timeUntilNextAttack = 0f;
    float timeSinceAttack = Mathf.Infinity;

    [SerializeField] GameObject targetMarker;

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
        timeSinceAttack += Time.deltaTime;

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

                    if (IsPlayerInRange(false))
                    {
                        currentBehaviour = Behaviour.Attack;
                    }
                }                
                break;

            case Behaviour.Attack:
                if (!IsPlayerInRange(true))
                {
                    currentBehaviour = Behaviour.Pursue;
                    enemyMovement.lookAt = null;
                }
                else
                {
                    UpdateAttackBehaviour();
                }
                break;
        }
    }

    public void SetTargetMarkerVisibility(bool isEnabled)
    {
        targetMarker.SetActive(isEnabled);
    }


    private void OnTriggerEnter(Collider other)
    {
        // If not the player
        if(other.gameObject.tag != "Player")
        {
            return;
        }

        playerTransform = other.transform;
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
        enemyMovement.lookAt = playerTransform;

        if (timeSinceAttack < timeUntilNextAttack)
        {
            return;
        }

        timeSinceAttack = 0f;
        timeUntilNextAttack = Random.Range(minAttackIdleTime, maxAttackIdleTime) + enemyCombat.GetAttackRate();

        enemyCombat.StartMeleeAttack();
        enemyMovement.Move(Vector3.zero);
    }

    private bool IsPlayerInRange(bool ignoreBehind)
    {
        Collider[] targetColliders = enemyCombat.GetTargetColliders();

        foreach (var collider in targetColliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                if (ignoreBehind)
                {
                    return true;
                }
                else
                {
                    return !enemyCombat.IsTargetBehind(collider.transform);
                }
            }
        }
        return false;
    }
}
