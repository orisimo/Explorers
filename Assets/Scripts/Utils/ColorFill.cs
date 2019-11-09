using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class ColorFill : MonoBehaviour
{
    [SerializeField] private Color _emptyColor = Color.red;
    [SerializeField] private Color _fullColor = Color.blue;
    [SerializeField] private Transform _scaleTransform;
    [SerializeField] private Vector3 _scaleMin = Vector3.zero;
    [SerializeField] private Vector3 _scaleMax = Vector3.one;

    private Image _image;

    private void OnEnable()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (!_image.IsActive()) return;
        _image.color = Color.Lerp(_emptyColor, _fullColor, _image.fillAmount);
        if(_scaleTransform == null) return;
        _scaleTransform.localScale = Vector3.Lerp(_scaleMin, _scaleMax, _image.fillAmount);
    }
}
