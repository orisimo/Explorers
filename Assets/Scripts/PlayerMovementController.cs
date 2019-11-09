using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Animations;
using XInputDotNetPure;
[RequireComponent(typeof(PlayerInteractionController))]
[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _forceMultiplier = 100f;
    [SerializeField] private float _torqueMultiplier = 5f;
    [SerializeField] private HungerFloatValuePair[] _hungerSpeedModifierValuePairs;

    public HungerState HungerState = HungerState.Full;
    
    private Rigidbody _playerRigidbody;
    private PlayerInteractionController _playerInteractionController;
    private PlayerInputController _playerInputController;

    private readonly Dictionary<HungerState, float> _hungerSpeedModifierDictionary = new Dictionary<HungerState, float>();
    
    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerInteractionController = GetComponent<PlayerInteractionController>();
        _playerInputController = GetComponent<PlayerInputController>();
        
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
        _playerRigidbody.AddForce(_playerInputController.InputData.MovementVector * _forceMultiplier * GetHungerModifier(), ForceMode.Force);
        _playerRigidbody.AddTorque(new Vector3(0f, Vector3.SignedAngle(transform.forward, _playerInputController.InputData.MovementVector.normalized, Vector3.up) * _torqueMultiplier * Time.deltaTime, 0f), ForceMode.Force);
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
