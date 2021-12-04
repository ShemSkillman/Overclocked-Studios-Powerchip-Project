using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private Movement enemyMovement;

    private Transform playerTransform;

    private NavMeshAgent agent;
    private Vector3 velocity;
    private CharacterController controller;
    private Combat enemyCombat;

    [SerializeField] private Behaviour currentBehaviour = Behaviour.Idle;

    enum Behaviour { Idle, Pursue, Attack };

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<Movement>();
        enemyCombat = GetComponent<Combat>();

        agent = gameObject.GetComponent<NavMeshAgent>();
        controller = gameObject.GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentBehaviour)
        {
            case Behaviour.Idle:
                if (playerTransform != null)
                {
                    currentBehaviour = Behaviour.Pursue;
                } else
                {
                    IdleBehaviour();
                }
                break;

            case Behaviour.Pursue:
                if (playerTransform == null)
                {
                    currentBehaviour = Behaviour.Idle;
                }
                else if (IsPlayerInRange())
                {
                    currentBehaviour = Behaviour.Attack;
                }
                else
                {
                    PursueBehaviour();
                }
                break;

            case Behaviour.Attack:
                if (!IsPlayerInRange())
                {
                    currentBehaviour = Behaviour.Pursue;
                }
                else
                {
                    AttackBehaviour();
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
    }

    private void IdleBehaviour()
    {
        enemyMovement.Move(Vector3.zero);
    }

    private void PursueBehaviour()
    {
        Vector3 lookPos;

        agent.destination = playerTransform.position;
        velocity = agent.desiredVelocity;

        //agent.updatePosition = false;
        //agent.updateRotation = false;

        lookPos = playerTransform.position - transform.position;
        lookPos.y = 0f;

        enemyMovement.Move(velocity.normalized);
        agent.velocity = controller.velocity;
    }

    private void AttackBehaviour()
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
