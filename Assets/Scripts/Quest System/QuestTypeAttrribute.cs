using UnityEngine;

public class QuestTypeAttribute : PropertyAttribute // for weird editor stuff
{
    public Quest.questType QuestType { get; }

    public QuestTypeAttribute(Quest.questType questType)
    {
        QuestType = questType;
    }
}