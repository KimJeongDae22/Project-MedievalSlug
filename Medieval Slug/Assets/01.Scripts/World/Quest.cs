[System.Serializable]
public class Quest
{
    public QuestData data;
    public QuestStatus status;
    public int currentProgress;

    public Quest(QuestData questData)
    {
        data = questData;
        status = QuestStatus.NotStarted;
        currentProgress = 0;
    }

    
    public bool IsComplete()
    {
        return currentProgress >= data.targetCount;
    }
    
    
    public void AddProgress(int amount = 1)
    {
        currentProgress += amount;
        if (IsComplete() && status == QuestStatus.InProgress)
        {
            status = QuestStatus.Completed;
        }
    }
}
