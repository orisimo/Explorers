using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceTraveledUiController : MonoBehaviour
{
    [SerializeField] private Transform _campfireTransform;
    [SerializeField] private TextMeshProUGUI _distanceText;
    [SerializeField] private string _distanceTextFormat = "Distance Traveled: {0}m";

    private void Update()
    {
        if(!_campfireTransform) return;
        var distanceTraveledText = string.Format(_distanceTextFormat, (int) _campfireTransform.transform.position.x);
        _distanceText.text = distanceTraveledText;
    }
}
