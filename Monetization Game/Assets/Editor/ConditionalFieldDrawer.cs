using Core;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.ConditionFieldName);

        MonoBehaviour targetMonoBehaviour = property.serializedObject.targetObject as MonoBehaviour;
        if (targetMonoBehaviour != null)
        {
            if (conditionProperty != null && conditionProperty.boolValue)
            {
                // Відображаємо поле та додаємо або знаходимо відповідний компонент
                EditorGUI.PropertyField(position, property, label, true);

                if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
                {
                    MonoBehaviour component = FindOrCreateComponent(targetMonoBehaviour, fieldInfo.FieldType);
                    if (component != null)
                    {
                        property.objectReferenceValue = component;
                    }
                }
            }
            else
            {
                // Видаляємо відповідний компонент, якщо він існує
                RemoveComponentIfExists(targetMonoBehaviour, fieldInfo.FieldType);
                property.objectReferenceValue = null; // Очищаємо посилання
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditional.ConditionFieldName);

        if (conditionProperty != null && conditionProperty.boolValue)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing;
        }
    }

    private MonoBehaviour FindOrCreateComponent(MonoBehaviour parent, System.Type type)
    {
        // Перевіряємо всі дочірні об'єкти
        MonoBehaviour[] childrenComponents = parent.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (MonoBehaviour component in childrenComponents)
        {
            if (component.GetType() == type)
            {
                return component;
            }
        }

        // Якщо компонент не знайдено, створюємо новий дочірній об'єкт
        GameObject childObject = new GameObject(type.Name);
        childObject.transform.parent = parent.transform;
        return childObject.AddComponent(type) as MonoBehaviour;
    }

    private void RemoveComponentIfExists(MonoBehaviour parent, System.Type type)
    {
        // Перевіряємо всі дочірні об'єкти
        MonoBehaviour[] childrenComponents = parent.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (MonoBehaviour component in childrenComponents)
        {
            if (component.GetType() == type)
            {
                // Видаляємо дочірній об'єкт, який містить цей компонент
                GameObject.DestroyImmediate(component.gameObject);
                break;
            }
        }
    }
}
