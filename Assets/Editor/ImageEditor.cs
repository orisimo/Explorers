using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Image), true)]
public class ImageEditor : Editor
{
    private Image _targetImage;
    private Color _emptyColor = Color.red;
    private Color _fullColor = Color.green;

    void OnEnable()
    {
        _targetImage = target as Image;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TryDrawFillGUI();
    }

    private void TryDrawFillGUI()
    {
        if (_targetImage.type != Image.Type.Filled)
        {
            return;
        }
    
        _emptyColor = EditorGUILayout.ColorField("Empty Color", _emptyColor);
        _fullColor = EditorGUILayout.ColorField("Empty Color", _fullColor);
        _targetImage.color = Color.Lerp(_emptyColor, _fullColor, _targetImage.fillAmount);
    }
}