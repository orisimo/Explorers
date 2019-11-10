using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ChaseStateBehaviour : StateMachineBehaviour
{
    private const int SAMPLE_RATE = 5;
    
    private AiAgent _aiAgent;
    private int _sampleCount;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _aiAgent = animator.GetComponent<AiAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        var isPlayerInRadius = AiUtil.Instance.IsPlayerInRadius(animator.transform.position, _aiAgent.SightRadius);
        animator.SetBool(AnimatorParameterNames.PLAYER_IN_RADIUS_BOOL, isPlayerInRadius);
        HandleChase();
    }

    private void SetChaseTarget()
    {
        var chaseTargetPosition = AiUtil.Instance.GetNearestPlayerPosition(_aiAgent.transform.position, out var nearestPlayer, out var nearestPlayerDistance);
        if (nearestPlayer != null &&
            nearestPlayerDistance < PlayersManager.Instance.ChaseFleeRadius)
        {
            nearestPlayer.MovementController.TrySetFleeing(_aiAgent.transform.position);
        }
        _aiAgent.NavMeshAgent.SetDestination(chaseTargetPosition);
    }
    
    private void HandleChase()
    {
        if (_sampleCount > 1)
        {
            _sampleCount--;
            return;
        }

        _sampleCount = SAMPLE_RATE;
        SetChaseTarget();
    }
}
