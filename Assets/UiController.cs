using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private Transform[] _playerTransforms;
    [SerializeField] private GameObject[] _viewBubbleGameObjects;
    [SerializeField, Tooltip("How much buffer, in percent of screen, to add before toggling HUD visibility")] private float _frustumMargin = 0.1f;
    [SerializeField] private Camera _mainCamera;

    void Update()
    {
        for (var playerIndex = 0; playerIndex < _playerTransforms.Length; playerIndex++)
        {
            var playerTransform = _playerTransforms[playerIndex];
            var viewBubble = _viewBubbleGameObjects[playerIndex];

            HandlePlayerBubble(playerTransform, viewBubble);
        }
    }

    private void HandlePlayerBubble(Transform playerTransform, GameObject viewBubble)
    {
        var playerScreenPoint = _mainCamera.WorldToScreenPoint(playerTransform.position);
        playerScreenPoint.x = playerScreenPoint.x / Screen.width;
        playerScreenPoint.y = playerScreenPoint.y / Screen.height;
        var leftPosition = -(playerScreenPoint.x + _frustumMargin) / _frustumMargin;
        var rightPosition = -(1f - playerScreenPoint.x  + _frustumMargin) / _frustumMargin; ;
        var bottomPosition = -(playerScreenPoint.y + _frustumMargin) / _frustumMargin;
        var topPosition = -(1f - playerScreenPoint.y + _frustumMargin) / _frustumMargin;
        var distanceFromBorder = Mathf.Clamp01(Mathf.Max(leftPosition, rightPosition, bottomPosition, topPosition));
        viewBubble.transform.localScale = Vector3.one * distanceFromBorder;
    }
}
