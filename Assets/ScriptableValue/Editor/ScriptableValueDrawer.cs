using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReaaliStudio.Systems.ScriptableValue
{
    [CustomPropertyDrawer(typeof(ScriptableValue<>), true)]
    public class ScriptableValueDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, DrawerProperties> _initializedDrawers = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var properties = Initialize(property);
            
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = Mathf.Min(200f, labelWidth);
            var value = (BaseScriptableValue)property.objectReferenceValue;
            float availableWidth = position.width;
            
            float width = Mathf.Min(500f, availableWidth - 120f);
            position.width = width;
            properties.secondaryRect.x = position.x + position.width + 20;
            properties.secondaryRect.y = position.y;
            properties.secondaryRect.height = position.height;
            
            EditorGUI.PropertyField(position, property, label);
            EditorGUIUtility.labelWidth = labelWidth;

            if (!value)
            {
                properties.secondaryRect.width = Mathf.Min(100f, availableWidth - position.width - 20f);

                if (GUI.Button(properties.secondaryRect, "Create New"))
                {
                    Type type = fieldInfo.FieldType;
                    ScriptableObject asset = ScriptableValueDatabase.CreateValueAsset(type, string.Empty);
                    property.objectReferenceValue = asset;
                }
            }
            else if(!value.TargetTypeIsList && property.objectReferenceValue)
            {
                if (Application.isPlaying)
                {
                    EditorGUI.LabelField(properties.secondaryRect, "No editing while playing", EditorStyles.boldLabel);
                    return;
                }
                
                properties.secondaryRect.width = Mathf.Max(0f, availableWidth - position.width - 20f);
                properties.valueSerializedObject ??= new SerializedObject(property.objectReferenceValue);
                SerializedProperty valueProperty = properties.valueSerializedObject.FindProperty(ScriptableValue<dynamic>.ValueProperty);
                EditorGUI.PropertyField(properties.secondaryRect, valueProperty, new GUIContent(""));
                properties.valueSerializedObject.ApplyModifiedProperties();
            }
        }

        private DrawerProperties Initialize(SerializedProperty property)
        {
            string drawerKey = property.propertyPath;
            if (_initializedDrawers.TryGetValue(drawerKey, out var properties))
            {
                return properties;
            }

            var drawerProperties = new DrawerProperties
            {
                secondaryRect = new Rect()
            };

            _initializedDrawers.Add(drawerKey, drawerProperties);
            return drawerProperties;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        
        private struct DrawerProperties
        {
            public SerializedObject valueSerializedObject;
            public Rect secondaryRect;
        }
    }
}