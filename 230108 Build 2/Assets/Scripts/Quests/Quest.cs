using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[System.Serializable]
public class Quest
{
    public QuestBase Base { get; private set; }
    public QuestStatus Status { get; private set; }

    public Quest(QuestBase _base)
    {
        Base = _base;
    }

    public Quest(QuestSaveData saveData)
    {
        Base = QuestDB.GetObjectByName(saveData.name);
        Status = saveData.status;
    }

    public QuestSaveData GetSaveData()
    {
        var saveData = new QuestSaveData()
        {
            name = Base.Name,
            status = Status
        };
        return saveData;
    }

    public IEnumerator StartQuest()
    {
        Status = QuestStatus.Started;

        yield return DialogManagerRef.instance.ShowDialog(Base.StartDialogue);

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
    }

    public IEnumerator CompleteQuest(Transform player)
    {
        Status = QuestStatus.Completed;

        yield return DialogManagerRef.instance.ShowDialog(Base.CompletedDialogue);

        var inventario = Inventario.GetInventory();
        if (Base.RequiredItem != null)
        {
            inventario.RemoveItem(Base.RequiredItem);
        }

        if (Base.RewardItem != null)
        {
            inventario.AddItem(Base.RewardItem);

            string playerName = player.GetComponent<PlayerCharacter>().Name;

            AudioManager.i.PlaySfx(AudioId.ItemObtained, pauseMusic: true);

            yield return player.GetComponent<PlayerCharacter>().RaiseItem(Base.RewardItem);

            yield return DialogManagerRef.instance.ShowDialogText($"¡{playerName} encontro {Base.RewardItem.Name}!");

            yield return player.GetComponent<PlayerCharacter>().LowerItem();
        }

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
    }

    public bool CanBeCompleted()
    {
        var inventario = Inventario.GetInventory();
        if (Base.RequiredItem != null)
        {
            if (!inventario.HasItem(Base.RequiredItem))
                return false;
        }

        return true;
    }
}

[System.Serializable]
public class QuestSaveData
{
    public string name;
    public QuestStatus status;
}

public enum QuestStatus { None, Started, Completed }
