using UnityEngine;

namespace Core
{
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public string ConditionFieldName;

        public ConditionalFieldAttribute(string conditionFieldName)
        {
            ConditionFieldName = conditionFieldName;
        }
    }
}

