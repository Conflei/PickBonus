using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System;
using System.Linq;
#endif


/// <summary>
/// Attribute that will restrict displaying the variable its attached to
/// </summary>
public class DisplayIfAttribute : PropertyAttribute
{
    /// <summary>
    /// The field to check
    /// </summary>
    private string field;

    /// <summary>
    /// The value we are comparing against
    /// </summary>
    private object[] values;

    /// <summary>
    /// Display only if the value is in the range given
    /// </summary>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <param name="compareMode"></param>
    public DisplayIfAttribute(string field, params object[] values)
    {
        this.field = field;
        this.values = values;
    }

#if UNITY_EDITOR

    /// <summary>
    /// Check if the property should be displayed
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool ShouldDisplay(SerializedObject obj)
    {
        SerializedProperty property = obj.FindProperty(field);

        if (property == null)
        {
            Debug.LogError("Could not find the field with name " + field + " on " + obj.targetObject.name + " of type " + obj.targetObject.GetType());
        }

        //Compare with comparison
        if (values != null)
        {
            if (property.propertyType == SerializedPropertyType.Boolean)
            {
                return values.Contains(property.boolValue);
            }
            else if (property.propertyType == SerializedPropertyType.Enum)
            {
                return values.Any(x => x.ToString() == property.enumNames[property.enumValueIndex]);
            }
        }
        return true;
    }
#endif
}

#if UNITY_EDITOR

/// <summary>
/// Drawer for any variables using the DisplayIf attribute
/// </summary>
[CustomPropertyDrawer(typeof(DisplayIfAttribute), true)]
public class DisplayIfAttributeDrawer : PropertyDrawer
{
    /// <summary>
    /// Draw the inspector
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);

        //Get display attribute
        DisplayIfAttribute displayAttribute = (DisplayIfAttribute)attribute;
        bool enabled = displayAttribute.ShouldDisplay(property.serializedObject);

        //Enable/disable the property
        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;

        EditorGUI.BeginChangeCheck();

        if (enabled)
            EditorGUI.PropertyField(position, property, label, true);

        EditorGUI.EndChangeCheck();

        //Ensure that the next property that is being drawn uses the correct settings
        GUI.enabled = wasEnabled;

        EditorGUI.EndProperty();
    }

    /// <summary>
    /// Override the get height method
    /// </summary>
    /// <param name="property"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //Get display attribute
        DisplayIfAttribute displayAttribute = (DisplayIfAttribute)attribute;
        bool enabled = displayAttribute.ShouldDisplay(property.serializedObject);

        if (enabled)
            return base.GetPropertyHeight(property, label);
        else
            return 0f;
    }
}

#endif