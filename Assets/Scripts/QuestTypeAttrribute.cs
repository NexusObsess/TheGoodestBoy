using UnityEngine;

public class QuestTypeAttribute : PropertyAttribute
{
    public Quest.questType QuestType { get; }

    public QuestTypeAttribute(Quest.questType questType)
    {
        QuestType = questType;
    }
}