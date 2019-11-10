using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerInteractionController))]
[RequireComponent(typeof(PlayerResourceController))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavMeshObstacle))]
public class PlayerContext : MonoBehaviour
{
    public PlayerInputController InputController { get; private set; }
    public PlayerMovementController MovementController { get; private set; }
    public PlayerInteractionController InteractionController { get; private set; }
    public PlayerResourceController ResourceController { get; private set; }
    
    public NavMeshAgent NavMeshAgent { get; private set; }
    public NavMeshObstacle NavMeshObstacle { get; private set; }

    void Awake()
    {
        InputController = GetComponent<PlayerInputController>();
        MovementController = GetComponent<PlayerMovementController>();
        InteractionController = GetComponent<PlayerInteractionController>();
        ResourceController = GetComponent<PlayerResourceController>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshObstacle = GetComponent<NavMeshObstacle>();
    }
}
