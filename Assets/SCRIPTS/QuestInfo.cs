using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/QuestInfo", order = 1)]
public class QuestInfo : ScriptableObject
{
    [TextArea(5,10)]
    public List<string> initialDialog;
    public List<AudioClip> initialDialogClips;


    [Header("Option")]
    [TextArea(5, 10)]

    public string acceptOption;
    [TextArea(5, 10)]

    public string acceptAnswer;
    public AudioClip acceptAnswerClip;

    [TextArea(5, 10)]

    public string declineOption;
    [TextArea(5, 10)]

    public string declineAnswer;
    public AudioClip declineAnswerClip;

    [TextArea(5, 10)]

    public string comebackAfterDecline;
    public AudioClip comebackAfterDeclineClip;

    [TextArea(5, 10)]

    public string comebackInProgress;
    public AudioClip comebackInProgressClip;

    [TextArea(5, 10)]

    public string comebackCompleted;
    public AudioClip comebackCompletedClip;

    [TextArea(5, 10)]

    public string finalWords;
    public AudioClip finalWordsClip;

    [Header("Rewards")]
    public int coinReward;

    public string rewardItem1;
    public string rewardItem2;

    [Header("Requirements")]
    public string firstRequirementItem;
    public int firstRequirementAmount;

    public string secondRequirementItem;
    public int secondRequirementAmount;

    public bool hasCheckPoints;
    public List<Checkpoint> checkpoints;

    [Header("Unlock NPC Feature")]
    public bool unlocksNPC;

    public NPC_ID npcToUnlock;

}