using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{/*
    public List<Quest> quest;
    private QuestManager questManager;

    public GameObject questWindow;
    public TMP_Text qTitle;
    public TMP_Text qDetail;
    public TMP_Text qTask;
    public TMP_Text qReward;

    private int QuestIDUi;

    //Handles Quest Accept, Reject and Display on Screen
    private void Start()
    {
        quest = questManager.activeQuests;
    }
    
    public void OpenQuestWindow(int Q_Id){
        QuestIDUi = Q_Id;
        Debug.Log(Q_Id);
        questWindow.SetActive(true);
        qTitle.text = quest[Q_Id].questName;
        *//*qTask.text = quest[Q_Id].objectives;*//*
        qReward.text = quest[Q_Id].GetRewardsTxt();
    }

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
*//*        QuestManager.StartQuest();*//*
    }

    public void Reset()
    {
        questWindow.SetActive(false);
    }*/
}
