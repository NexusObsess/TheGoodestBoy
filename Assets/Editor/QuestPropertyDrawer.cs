using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Quest))]
public class QuestPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float y = position.y;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        // creating and setting quest type
        SerializedProperty questType = property.FindPropertyRelative("QuestType");
        Quest.questType selectedType = (Quest.questType)questType.enumValueIndex;

        // variables that appear for each quest
        //DrawHeader(ref y, position, "Quest Basics", spacing);
        DrawProperty(ref y, position, property.FindPropertyRelative("questName"), spacing);
        DrawProperty(ref y, position, property.FindPropertyRelative("questDescription"), spacing);
        DrawProperty(ref y, position, property.FindPropertyRelative("questDifficulty"), spacing);

        DrawProperty(ref y, position, property.FindPropertyRelative("questCompleted"), spacing);
        DrawProperty(ref y, position, property.FindPropertyRelative("questFailed"), spacing);
        DrawProperty(ref y, position, property.FindPropertyRelative("questFailed"), spacing);
        DrawProperty(ref y, position, property.FindPropertyRelative("questWon"), spacing);

        DrawProperty(ref y, position, property.FindPropertyRelative("itemReward"), spacing);
        DrawProperty(ref y, position, property.FindPropertyRelative("townMoraleIncrease"), spacing);

        DrawProperty(ref y, position, questType, spacing);

        // quest type variables
        bool hasQuestFields = false;

        SerializedProperty iterator = property.Copy();
        SerializedProperty end = iterator.GetEndProperty();

        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (SerializedProperty.EqualContents(iterator, end)) break;

            // Find field on Quest
            FieldInfo field = typeof(Quest).GetField(iterator.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null) continue;

            // Find QuestTypeAttribute
            QuestTypeAttribute attribute = field.GetCustomAttribute<QuestTypeAttribute>();
            if (attribute == null) continue;

            // check if this field for the selected QuestType?
            if (attribute.QuestType != selectedType) continue;

            // Draw header once
            if (!hasQuestFields)
            {
                y += spacing;
                DrawHeader(ref y, position, "For " + selectedType, spacing);
                hasQuestFields = true;
            }

            // Draw field
            DrawProperty(ref y, position, iterator, spacing);
        }

        //quest line
        SerializedProperty questLine = property.FindPropertyRelative("QuestLine");
        Quest.questLine selectedQuestLine = (Quest.questLine)questLine.enumValueIndex;

        // Always show QuestLine dropdown
        y += spacing;
        DrawProperty(ref y, position, questLine, spacing);

        // questlines variables
        if (selectedQuestLine != Quest.questLine.None) // None = don't show any QuestLine-specific variables
        {
            bool hasQuestLineFields = false;

            iterator = property.Copy();
            end = iterator.GetEndProperty();

            enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (SerializedProperty.EqualContents(iterator, end)) break;

                // Find field on Quest
                FieldInfo field = typeof(Quest).GetField(iterator.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null)continue;

                // Find QuestLineTypeAttribute
                QuestLineTypeAttribute attribute = field.GetCustomAttribute<QuestLineTypeAttribute>();
                if (attribute == null) continue;
                // check field for the selected QuestLine
                if (!attribute.Matches(selectedQuestLine)) continue;

                // Draw header once
                if (!hasQuestLineFields)
                {
                    y += spacing;
                    DrawHeader(ref y, position, "For " + selectedQuestLine, spacing);
                    hasQuestLineFields = true;
                }

                // Draw field
                DrawProperty(ref y, position, iterator, spacing);
            }
        }

        EditorGUI.EndProperty();
    }

    // draw property
    private void DrawProperty(ref float y, Rect position, SerializedProperty property, float spacing)
    {
        if (property == null)
        {
            Debug.LogError("QuestPropertyDrawer: property is null");
            return;
        }

        // Get the actual height Unity needs for this property.
        float propertyHeight = EditorGUI.GetPropertyHeight(property, true);
        Rect rect = new Rect(position.x, y, position.width, propertyHeight);
        EditorGUI.PropertyField(rect, property, true);
        y += propertyHeight + spacing;
    }

    // draw header
    private void DrawHeader(ref float y, Rect position, string text, float spacing)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        Rect rect = new Rect(position.x, y, position.width, lineHeight);
        EditorGUI.LabelField(rect, text, EditorStyles.boldLabel);
        y += lineHeight + spacing;
    }

    // get property height
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        float height = 0;

        // constant variables
        height += EditorGUIUtility.singleLineHeight + spacing;
        height += GetPropertyHeight(property.FindPropertyRelative("questName"), spacing);
        height += GetPropertyHeight(property.FindPropertyRelative("questDescription"), spacing);
        height += GetPropertyHeight(property.FindPropertyRelative("questDifficulty"), spacing);

        height += EditorGUIUtility.singleLineHeight + spacing;
        height += GetPropertyHeight(property.FindPropertyRelative("questCompleted"), spacing);
        height += GetPropertyHeight(property.FindPropertyRelative("questFailed"), spacing);
        height += GetPropertyHeight(property.FindPropertyRelative("questWon"), spacing);

        height += EditorGUIUtility.singleLineHeight + spacing;
        height += GetPropertyHeight(property.FindPropertyRelative("itemReward"), spacing);
        height += GetPropertyHeight(property.FindPropertyRelative("townMoraleIncrease"), spacing);

        SerializedProperty questType = property.FindPropertyRelative("QuestType");
        height += GetPropertyHeight(questType, spacing);

        // quest type variables
        Quest.questType selectedType = (Quest.questType)questType.enumValueIndex;

        bool hasQuestFields = false;

        SerializedProperty iterator = property.Copy();
        SerializedProperty end = iterator.GetEndProperty();

        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (SerializedProperty.EqualContents(iterator, end)) break;

            FieldInfo field = typeof(Quest).GetField(iterator.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) continue;

            QuestTypeAttribute attribute = field.GetCustomAttribute<QuestTypeAttribute>();
            if (attribute == null) continue;
            if (attribute.QuestType != selectedType) continue;

            if (!hasQuestFields)
            {
                // QuestType header
                height += EditorGUIUtility.singleLineHeight + spacing;
                hasQuestFields = true;
            }

            height += GetPropertyHeight(iterator, spacing);
        }

        // questline dropdown
        SerializedProperty questLine = property.FindPropertyRelative("QuestLine");

        // Always show QuestLine dropdown
        height += spacing;
        height += GetPropertyHeight(questLine, spacing);

        // Quest line variables
        Quest.questLine selectedQuestLine = (Quest.questLine)questLine.enumValueIndex;

        if (selectedQuestLine != Quest.questLine.None) // Don't add QuestLine fields if None
        {
            bool hasQuestLineFields = false;

            iterator = property.Copy();
            end = iterator.GetEndProperty();

            enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (SerializedProperty.EqualContents(iterator, end)) break;

                FieldInfo field = typeof(Quest).GetField(iterator.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null) continue;

                QuestLineTypeAttribute attribute = field.GetCustomAttribute<QuestLineTypeAttribute>();
                if (attribute == null)continue;
                if (!attribute.Matches(selectedQuestLine)) continue;

                if (!hasQuestLineFields)
                {
                    // QuestLine header
                    height += EditorGUIUtility.singleLineHeight + spacing;
                    hasQuestLineFields = true;
                }

                height += GetPropertyHeight(iterator, spacing);
            }
        }

        return height;
    }

    // Get the actual height Unity needs for a property.
    private float GetPropertyHeight(SerializedProperty property, float spacing)
    {
        if (property == null) return 0;

        return EditorGUI.GetPropertyHeight(property, true) + spacing;
    }
}