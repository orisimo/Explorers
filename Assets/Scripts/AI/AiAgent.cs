using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiAgent : MonoBehaviour
{
    [SerializeField] private float _sightRadius;
    [SerializeField] private Vector2 _roamDistanceMinMax;
    public float SightRadius => _sightRadius;
    public Vector3 RoamDistanceMinMax => _roamDistanceMinMax;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    private NavMeshAgent _navMeshAgent;

    public bool IsAtDestination =>
        NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
        NavMeshAgent.remainingDistance < float.Epsilon;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.Warp(transform.position);
    }
}
