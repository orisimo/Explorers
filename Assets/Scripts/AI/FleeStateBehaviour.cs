using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private float _flightDuration;
    [SerializeField] private float _flightDistance;

    private float _remainingFlightDuration;
    private AiAgent _aiAgent;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _remainingFlightDuration = _flightDuration;
        _aiAgent = animator.GetComponent<AiAgent>();
        SetFleeDestination();
    }

    private void SetFleeDestination()
    {
        var agentPosition = _aiAgent.transform.position;
        var fleeDestination = agentPosition + (agentPosition - _aiAgent.FleeFromPosition).normalized * _flightDistance;
        _aiAgent.NavMeshAgent.SetDestination(fleeDestination);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, animatorStateInfo, layerIndex);
        _remainingFlightDuration -= Time.deltaTime;
        if (_aiAgent.NavMeshAgent.remainingDistance <= 1f)
        {
            _remainingFlightDuration = 0f;
        }
        if (_remainingFlightDuration <= 0f)
        {
            animator.SetBool(AnimatorParameterNames.IS_FLEEING_BOOL_NAME, false);
        }
    }
}
