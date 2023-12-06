using UnityEditor;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DaimahouGames.Editor.Common
{
    public static class SerializationUtilities
    {
        private const string SPACE = " ";

        public static bool CreateChildProperties(this SerializedObject serializedObject, VisualElement root,
            bool hideLabelsInChildren, params string[] excludeFields)
        {
            var iteratorProperty = serializedObject.GetIterator();
            iteratorProperty.Next(true);
            var endProperty = iteratorProperty.GetEndProperty(true);

            var numProperties = 0;
            if (!iteratorProperty.NextVisible(true)) return false;

            do
            {
                if (SerializedProperty.EqualContents(iteratorProperty, endProperty)) break;
                if (excludeFields.Contains(iteratorProperty.name)) continue;

                var field = hideLabelsInChildren
                    ? new PropertyField(iteratorProperty, SPACE)
                    : new PropertyField(iteratorProperty);

                root.Add(field);
                numProperties += 1;
            } while (iteratorProperty.NextVisible(false));

            root.Bind(serializedObject);
            return numProperties != 0;  
        }
    }
}