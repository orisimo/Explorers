using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ChaseStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private PricesDictionary _hirePrice = new PricesDictionary();

    private PricesDictionary _remainingPrice;
    
    private const int SAMPLE_RATE = 5;
    
    private AiAgent _aiAgent;
    private int _sampleCount;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _remainingPrice = _hirePrice;
        _aiAgent = animator.GetComponent<AiAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, animatorStateInfo, layerIndex);
        var isPlayerInRadius = AiUtil.Instance.IsPlayerInRadius(animator.transform.position, _aiAgent.SightRadius);
        animator.SetBool(AnimatorParameterNames.PLAYER_IN_RADIUS_BOOL, isPlayerInRadius);
        HandleChase(animator);
    }

    private void SetChaseTarget(Animator animator)
    {
        var chaseTargetPosition = AiUtil.Instance.GetNearestPlayerPosition(_aiAgent.transform.position, out var nearestPlayer, out var nearestPlayerDistance);
        if (nearestPlayer == null ||
            nearestPlayer.InteractionController.HasBodyGuard)
        {
            animator.SetBool(AnimatorParameterNames.IS_FLEEING_BOOL_NAME, true);
            _aiAgent.FleeFromPosition = nearestPlayer.transform.position;
            return;
        }
        
        _aiAgent.NavMeshAgent.SetDestination(chaseTargetPosition);
            
        if(nearestPlayerDistance < PlayersManager.Instance.ChaseFleeRadius)
        {
            OnCaughtTarget(nearestPlayer, animator);
        }
    }

    private void OnCaughtTarget(PlayerContext nearestPlayer, Animator animator)
    {
        var newRemainingPrice = new PricesDictionary();
        _remainingPrice.CopyTo(newRemainingPrice);
        foreach (var priceItem in _remainingPrice)
        {
            if (!nearestPlayer.InteractionController.IsGrabbingObjects)
            {
                break;
            }
            if (!nearestPlayer.InteractionController.TryGiveItem(priceItem.Key))
            {
                continue;
            }
            newRemainingPrice[priceItem.Key] -= 1;
            if (newRemainingPrice[priceItem.Key] <= 0)
            {
                newRemainingPrice.Remove(priceItem.Key);
            }
        }

        _remainingPrice = newRemainingPrice;
        
        if (_remainingPrice.Count == 0)
        {
            SetHired(animator, nearestPlayer);
            return;
        }
        
        nearestPlayer.MovementController.TrySetFleeing(_aiAgent.transform.position);
    }

    private void SetHired(Animator animator, PlayerContext hiringPlayer)
    {
        _aiAgent.SetHiringPlayer(hiringPlayer);
        animator.SetBool(AnimatorParameterNames.IS_HIRED_BOOL_NAME, true);
    }

    private void HandleChase(Animator animator)
    {
        if (_sampleCount > 1)
        {
            _sampleCount--;
            return;
        }

        _sampleCount = SAMPLE_RATE;
        SetChaseTarget(animator);
    }
}
