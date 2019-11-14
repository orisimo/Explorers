using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class HiredStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private float _followDistance = 5f;
    [SerializeField] private float _hireDuration = 120f;
    private AiAgent _aiAgent;
    private float _remainingHireDuration;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _aiAgent = animator.GetComponent<AiAgent>();
        if (_aiAgent.HiringPlayer == null)
        {
            SetIsNotHired(animator);
            return;
        }

        _remainingHireDuration = _hireDuration;
        _followTarget = _aiAgent.HiringPlayer.transform;
    }

    private void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateExit(animator, animatorStateInfo, layerIndex);
        _aiAgent.HiringPlayer.InteractionController.HasBodyGuard = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, animatorStateInfo, layerIndex);
        if (_followTarget == null ||
            _remainingHireDuration <= 0f)
        {
            SetIsNotHired(animator);
            return;
        }
        
        _remainingHireDuration -= Time.deltaTime;
        
        var distance = Vector3.Distance(_aiAgent.transform.position, _followTarget.position);
        if (distance < _followDistance)
        {
            return;
        }
        _aiAgent.NavMeshAgent.SetDestination(_followTarget.position);
    }

    private static void SetIsNotHired(Animator animator)
    {
        animator.SetBool(AnimatorParameterNames.IS_HIRED_BOOL_NAME, false);
    }
}
