using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RoamStateBehaviour : StateMachineBehaviour
{
    private static float ROAM_EPSILON = 0.1f;
    
    private AiAgent _aiAgent;
    private Vector3 _roamPosition;
    private Vector3? _guardPosition;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _aiAgent = animator.GetComponent<AiAgent>();
        UpdateRoamPosition();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        var isPlayerInRadius = AiUtil.Instance.IsPlayerInRadius(animator.transform.position, _aiAgent.SightRadius);
        animator.SetBool(AnimatorParameterNames.PLAYER_IN_RADIUS_BOOL, isPlayerInRadius);
        HandleRoam();
    }

    private void UpdateRoamPosition()
    {
        var randomDirection = Random.insideUnitSphere;
        var randomDistance = Random.Range(_aiAgent.RoamDistanceMinMax.x, _aiAgent.RoamDistanceMinMax.y);
        randomDirection.y = 0f;
        var roamPosition = (_guardPosition.HasValue ? _guardPosition.Value : _aiAgent.transform.position) + randomDirection * randomDistance;
        _aiAgent.NavMeshAgent.SetDestination(roamPosition);
    }
    
    private void HandleRoam()
    {
        if (_aiAgent.IsAtDestination)
        {
            UpdateRoamPosition();
        }
    }
}
