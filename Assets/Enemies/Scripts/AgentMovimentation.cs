using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AgentMovimentation : MonoBehaviour
{
    private GameObject playerObj;
    private Animator animator;
    private NavMeshAgent agent;
    private Rigidbody agentRigidBody;
    private Vector3 agentInitialPosition;
    private Vector3 playerCurrentPosition;
    private Vector3 agentCurrentPosition;
    private float playerAgentCurrentDistance;
    private float agentCurrentDistanceFromInitialPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agentRigidBody = GetComponent<Rigidbody>();
        agentInitialPosition = agentRigidBody.position;
        playerObj = GameObject.Find("Stylized Astronaut");
    }

    void AnimationHandler()
    {
        if (agentCurrentDistanceFromInitialPosition > 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void DestinationHandler()
    {
        if (playerAgentCurrentDistance > 20)
        {
            agent.SetDestination(agentInitialPosition);
        }
        else
        {
            agent.SetDestination(playerCurrentPosition);
        }
    }

    private Vector3 getObjectLocation(GameObject auxObject)
    {
        return new Vector3(auxObject.transform.position.x, 
                           auxObject.transform.position.y,
                           auxObject.transform.position.z);
    }

    void FixedUpdate()
    {
        playerCurrentPosition = getObjectLocation(playerObj);
        agentCurrentPosition = agentRigidBody.position;

        playerAgentCurrentDistance = Vector2.Distance(new Vector2(agentRigidBody.position.x, 
                                                                        agentRigidBody.position.z), 
                                                            new Vector2(playerObj.transform.position.x,
                                                                        playerObj.transform.position.z));
        agentCurrentDistanceFromInitialPosition = Vector2.Distance(new Vector2(agentRigidBody.position.x, 
                                                                                     agentRigidBody.position.z), 
                                                                         new Vector2(agentInitialPosition.x,
                                                                                     agentInitialPosition.z));
        AnimationHandler();
        DestinationHandler();
    }
}
