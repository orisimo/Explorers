using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using XInputDotNetPure;
[RequireComponent(typeof(PlayerContext))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _forceMultiplier = 100f;
    [SerializeField] private float _torqueMultiplier = 5f;
    [SerializeField] private HungerFloatValuePair[] _hungerSpeedModifierValuePairs;
    [SerializeField] private float _fleeDistance = 15f;

    public HungerState HungerState { get; set; } = HungerState.Full;

    public bool IsFleeing
    {
        get => _isFleeing;
    }

    private bool _isFleeing;
    private Rigidbody _playerRigidbody;
    private PlayerContext _playerContext;

    private readonly Dictionary<HungerState, float> _hungerSpeedModifierDictionary = new Dictionary<HungerState, float>();
    
    private void Awake()
    {
        Init();
    }

    public void TrySetFleeing(Vector3 chaserPosition)
    {
        if (_isFleeing)
        {
            return;
        }

        _isFleeing = true;
        _playerContext.NavMeshObstacle.enabled = false;
        _playerContext.NavMeshAgent.enabled = true;
        var vectorFromChaser = transform.position - chaserPosition;
        var fleePosition = transform.position + vectorFromChaser.normalized * _fleeDistance;
        _playerContext.NavMeshAgent.SetDestination(fleePosition);
    }

    private void SetNotFleeing()
    {
        _isFleeing = false;
        _playerContext.NavMeshAgent.enabled = false;
        _playerContext.NavMeshObstacle.enabled = true;
    }
    
    private void Init()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerContext = GetComponent<PlayerContext>();
        
        for (var keyValuePairIndex = 0; keyValuePairIndex < _hungerSpeedModifierValuePairs.Length; keyValuePairIndex++)
        {
            var keyValuePair = _hungerSpeedModifierValuePairs[keyValuePairIndex];
            _hungerSpeedModifierDictionary.Add(keyValuePair.HungerState, keyValuePair.Float);
        }
    }
    
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        if (IsFleeing)
        {
            if (_playerContext.NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete &&
                _playerContext.NavMeshAgent.remainingDistance < float.Epsilon)
            {
                SetNotFleeing();
            }
            return;
        }
        _playerRigidbody.AddForce(_playerContext.InputController.InputData.MovementVector * _forceMultiplier * GetHungerModifier(), ForceMode.Force);
        _playerRigidbody.AddTorque(new Vector3(0f, Vector3.SignedAngle(transform.forward, _playerContext.InputController.InputData.MovementVector.normalized, Vector3.up) * _torqueMultiplier * Time.deltaTime, 0f), ForceMode.Force);
    }

    private float GetHungerModifier()
    {
        if (!_hungerSpeedModifierDictionary.TryGetValue(HungerState, out var result))
        {
            return 1f;
        }
        return result;
    }
}
