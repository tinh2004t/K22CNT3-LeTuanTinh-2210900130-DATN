using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureWalkState : StateMachineBehaviour
{
    float timer;
    public float walkingTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionAreaRadius = 18f;
    public float walkSpeed = 2f;

    List<Transform> waypointsList = new List<Transform>();


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // -- Initialization -- //
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        agent = animator.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = walkSpeed;
        }
        timer = 0;

        // -- Get all waypoints and Move to the first waypoint -- //
        waypointsList.Clear();

        NPCWaypoints waypointsComponent = animator.GetComponent<NPCWaypoints>();
        if (waypointsComponent != null && waypointsComponent.npmWaypointsCluster != null)
        {
            GameObject waypointsCluster = waypointsComponent.npmWaypointsCluster;
            foreach (Transform t in waypointsCluster.transform)
            {
                waypointsList.Add(t);
            }
        }

        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh && waypointsList.Count > 0)
        {
            Vector3 firstPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;
            agent.SetDestination(firstPosition);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            // -- If agent arrived at waypoint, move to next waypoint -- //
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (waypointsList.Count > 0)
                {
                    agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
                }
            }
        }

        // -- Transition to Idle State -- //
        timer += Time.deltaTime;
        if (timer > walkingTime)
        {
            animator.SetBool("isWalking", false);
        }

        // -- Transition to Chase State -- //
        if (player != null)
        {
            float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
            if (distanceFromPlayer < detectionAreaRadius)
            {
                animator.SetBool("isChasing", true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }
}