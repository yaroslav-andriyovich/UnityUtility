using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DrawTransformAxisRay : MonoBehaviour
{
    private const int _AXIS_TRIGGERS_SIZE = 3;

    [SerializeField] private float _rayDistance = 50f;
    [SerializeField] private AxisTriggerField[] _axisTriggers = new AxisTriggerField[_AXIS_TRIGGERS_SIZE]
    {
        new AxisTriggerField(Axis.Right, Color.red),
        new AxisTriggerField(Axis.Up, Color.green),
        new AxisTriggerField(Axis.Forward, Color.blue)
    };

    public enum Axis : int
    {
        Left = 0,
        Right,
        Up,
        Down,
        Forward,
        Back
    }

    private void Update()
    {
        CheckAxisTriggers();
    }

    private void OnValidate()
    {
        _rayDistance = Mathf.Clamp(_rayDistance, 1f, Mathf.Infinity);

        if (_axisTriggers.Length != _AXIS_TRIGGERS_SIZE)
        {
            Debug.LogWarning("Don't change this array size!");
            System.Array.Resize(ref _axisTriggers, _AXIS_TRIGGERS_SIZE);
        }
    }

    private void CheckAxisTriggers()
    {
        foreach(AxisTriggerField axisTriggerField in _axisTriggers)
        {
            if (axisTriggerField.enabled)
            {
                DrawRay(GetDirectionVector(axisTriggerField.axis), axisTriggerField.color);
            }
        }
    }

    private Vector3 GetDirectionVector(Axis axis)
    {
        switch(axis)
        {
            case Axis.Right:
                return transform.right;

            case Axis.Up:
                return transform.up;

            case Axis.Forward:
                return transform.forward;

            default:
                Debug.LogException(new System.Exception("Axis not found."));
                return Vector3.zero;
        }
    }

    private void DrawRay(Vector3 direction, Color color)
    {
        Debug.DrawRay(transform.position, direction * _rayDistance, color);
    }
}

[System.Serializable]
internal struct AxisTriggerField
{
    [HideInInspector] public string name;
    [HideInInspector] public DrawTransformAxisRay.Axis axis;
    [ReadOnlyAttribute] public Color color;
    public bool enabled;

    public AxisTriggerField(DrawTransformAxisRay.Axis axis, Color color)
    {
        this.axis = axis;
        this.name = axis.ToString();
        this.color = color;
        this.enabled = true;
    }
}

internal class ReadOnlyAttribute : PropertyAttribute {}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
internal class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
