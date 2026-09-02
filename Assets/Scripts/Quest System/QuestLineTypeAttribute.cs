using UnityEngine;

public class QuestLineTypeAttribute : PropertyAttribute
{
    public Quest.questLine[] QuestLines { get; }

    public QuestLineTypeAttribute(params Quest.questLine[] questLines)
    {
        QuestLines = questLines;
    }

    public bool Matches(Quest.questLine questLine)
    {
        foreach (Quest.questLine line in QuestLines)
        {
            if (line == questLine)
                return true;
        }

        return false;
    }
}