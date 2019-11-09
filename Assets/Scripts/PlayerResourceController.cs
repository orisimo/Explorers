using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerMovementController))]
public class PlayerResourceController : MonoBehaviour
{
    [SerializeField] private float _maxCalories;
    [SerializeField, Tooltip("in Calories per Second")] private float _caloryBurnRate;
    [SerializeField, Range(0,1)] private float _initialFullness;
    [SerializeField] private HungerFloatValuePair[] _hungerFullnessValuePairs;

    [SerializeField] private Image _fullnessFillImage;
    
    public float Calories => _calories;
    public float Fullness => _calories / _maxCalories;

    private PlayerMovementController _playerMovementController;
    private float _calories;

    public void AddCalories(float caloriesToAdd)
    {
        _calories += caloriesToAdd;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _calories = _maxCalories * _initialFullness;
        _playerMovementController = GetComponent<PlayerMovementController>();
        if (_fullnessFillImage == null)
        {
            Debug.LogError("Player Resource Controller missing Fullness display Image");
        }
    }

    private void Update()
    {
        BurnCalories();
        UpdateFullnessDisplay();
    }

    private void BurnCalories()
    {
        _calories = _calories - _caloryBurnRate * Time.deltaTime;
        var fullnessPercent = Fullness;
        
        for (var valuePairIndex = 0; valuePairIndex < _hungerFullnessValuePairs.Length; valuePairIndex++)
        {
            var hungerFullnessValuePair = _hungerFullnessValuePairs[valuePairIndex];
            if (fullnessPercent > hungerFullnessValuePair.Float)
            {
                _playerMovementController.HungerState = hungerFullnessValuePair.HungerState;
                if (_calories < 0f)
                {
                    _calories = 0f;
                }
                return;
            }
        }

        if (_calories < 0f)
        {
            _calories = 0f;
        }
    }

    private void UpdateFullnessDisplay()
    {
        _fullnessFillImage.fillAmount = Fullness;
    }
}
