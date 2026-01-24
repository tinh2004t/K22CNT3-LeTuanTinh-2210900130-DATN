using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{

    public bool playerInRange;

    public bool isTalkingWithPlayer;

    TextMeshProUGUI npcDialogText;

    Button optionButton1;
    TextMeshProUGUI optionButton1Text;

    Button optionButton2;
    TextMeshProUGUI optionButton2Text;

    public List<Quest> quests;
    public Quest currentActiveQuest = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialog;

    private void Start()
    {
        npcDialogText = DialogSystem.Instance.dialogText;

        optionButton1 = DialogSystem.Instance.option1BTN;
        optionButton1Text = DialogSystem.Instance.option1BTN.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        optionButton2 = DialogSystem.Instance.option2BTN;
        optionButton2Text = DialogSystem.Instance.option2BTN.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();



    }

    internal void StartConversation()
    {
        isTalkingWithPlayer = true;

        LookAtPlayer();


        // First time interaction with this NPC
        if (firstTimeInteraction)
        {
            firstTimeInteraction = false;
            currentActiveQuest = quests[activeQuestIndex];
            StartQuestInitialDialog();
            currentDialog = 0;
        }
        else // Continuing interaction with this NPC
        {
            if (currentActiveQuest.declined)
            {

                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.comebackAfterDecline;

                SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.comebackAfterDeclineClip);

                SetAcceptAndDeclineOptions();
            }

            // If we return while the quest is still in progress
            if (currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
            {
                if (AreQuestRequirmentsCompleted())
                {

                    SubmitRequiredItems();

                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackCompleted;
                    SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.comebackCompletedClip);

                    optionButton1Text.text = "[Take Reward]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() => {
                        ReceiveRewardAndCompleteQuest();
                    });
                }
                else
                {
                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackInProgress;
                    SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.comebackInProgressClip);

                    optionButton1Text.text = "[Close]";
                    optionButton1.onClick.RemoveAllListeners();
                    optionButton1.onClick.AddListener(() => {
                        DialogSystem.Instance.CloseDialogUI();
                        isTalkingWithPlayer = false;
                    });
                }
            }

            if (currentActiveQuest.isCompleted == true)
            {
                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.finalWords;
                SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.finalWordsClip);

                optionButton1Text.text = "[Close]";
                optionButton1.onClick.RemoveAllListeners();
                optionButton1.onClick.AddListener(() => {
                    DialogSystem.Instance.CloseDialogUI();
                    isTalkingWithPlayer = false;
                });
            }

            // If there is another quest available
            if (currentActiveQuest.initialDialogCompleted == false)
            {
                StartQuestInitialDialog();
            }

        }


    }

    private void SubmitRequiredItems()
    {

        string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredAmount);
        }


        string secondtRequiredItem = currentActiveQuest.info.secondRequirementItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(secondtRequiredItem, secondRequiredAmount);
        }
    }

    private bool AreQuestRequirmentsCompleted()
    {
        

        // First Item Requirment

        string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        var firstItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == firstRequiredItem)
            {
                firstItemCounter++;
            }
        }

        // Second Item Requirment -- If we dont have a second item, just set it to 0

        string secondRequiredItem = currentActiveQuest.info.secondRequirementItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        var secondItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == secondRequiredItem)
            {
                secondItemCounter++;
            }
        }

        SetQuestHasCheckpoints(currentActiveQuest);

        bool allCheckpointsCompleted = false;

        if (currentActiveQuest.info.hasCheckPoints)
        {
            foreach (Checkpoint cp in currentActiveQuest.info.checkpoints)
            {
                if(!cp.isCompleted)
                {
                    allCheckpointsCompleted = false;
                    break;
                }
                
                allCheckpointsCompleted = true;
            }
        }





        if (firstItemCounter >= firstRequiredAmount && secondItemCounter >= secondRequiredAmount)
        {
            if (currentActiveQuest.info.hasCheckPoints)
            {
                if (allCheckpointsCompleted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    private void SetQuestHasCheckpoints(Quest activeQuest)
    {
        if (activeQuest.info.checkpoints.Count > 0)
        {
            activeQuest.info.hasCheckPoints = true;
        }
        else
        {
            activeQuest.info.hasCheckPoints = false;
        }
    }

    private void StartQuestInitialDialog()
    {
        DialogSystem.Instance.OpenDialogUI();
        npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
        SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.initialDialogClips[currentDialog]);

        optionButton1Text.text = "Next";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() =>
        {
            currentDialog++;
            CheckIfDialogDone();
        });

        optionButton2.gameObject.SetActive(false);

    }

    private void CheckIfDialogDone()
    {
        if (currentDialog == currentActiveQuest.info.initialDialog.Count - 1) // If its the last dialog 
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.initialDialogClips[currentDialog]);

            currentActiveQuest.initialDialogCompleted = true;

            SetAcceptAndDeclineOptions();
        }
        else  // If there are more dialogs
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
            SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.initialDialogClips[currentDialog]);

            optionButton1Text.text = "Next";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() => {
                currentDialog++;
                CheckIfDialogDone();
            });
        }
    }

    private void SetAcceptAndDeclineOptions()
    {
        optionButton1Text.text = currentActiveQuest.info.acceptOption;
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            AcceptedQuest();
        });

        optionButton2.gameObject.SetActive(true);
        optionButton2Text.text = currentActiveQuest.info.declineOption;
        optionButton2.onClick.RemoveAllListeners();
        optionButton2.onClick.AddListener(() => {
            DeclinedQuest();
        });
    }

    public void ReceiveRewardAndCompleteQuest()
    {
        QuestManager.Instance.MarkQuestCompleted(currentActiveQuest);

        currentActiveQuest.isCompleted = true;

        var coinsRecieved = currentActiveQuest.info.coinReward;
        print("You recieved " + coinsRecieved + " gold coins");
        InventorySystem.Instance.currentCoins += coinsRecieved;

        if (currentActiveQuest.info.rewardItem1 != "")
        {
            print("You recieved " + currentActiveQuest.info.rewardItem1);

            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem1);
        }

        if (currentActiveQuest.info.rewardItem2 != "")
        {
            print("You recieved " + currentActiveQuest.info.rewardItem2);

            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem2);

        }

        activeQuestIndex++;

        // Start Next Quest 
        if (activeQuestIndex < quests.Count)
        {
            currentActiveQuest = quests[activeQuestIndex];
            currentDialog = 0;
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        }
        else
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
            print("No more quests");
        }

    }

    private void AcceptedQuest()
    {

        QuestManager.Instance.AddActiveQuest(currentActiveQuest);

        currentActiveQuest.accepted = true;
        currentActiveQuest.declined = false;

        if (currentActiveQuest.hasNoRequirements)
        {
            npcDialogText.text = currentActiveQuest.info.comebackCompleted;
            SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.comebackCompletedClip);

            optionButton1Text.text = "[Take Reward]";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() => {
                ReceiveRewardAndCompleteQuest();
            });
            optionButton2.gameObject.SetActive(false);
        }
        else
        {
            npcDialogText.text = currentActiveQuest.info.acceptAnswer;
            SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.acceptAnswerClip);

            CloseDialogUI();
        }

    }

    private void DeclinedQuest()
    {
        currentActiveQuest.declined = true;

        npcDialogText.text = currentActiveQuest.info.declineAnswer;
        SoundManager.Instance.PlayVoiceOver(currentActiveQuest.info.declineAnswerClip);

        CloseDialogUI();
    }

    private void CloseDialogUI()
    {
        optionButton1Text.text = "[Close]";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        });
        optionButton2.gameObject.SetActive(false);
    }


    public void LookAtPlayer()
    {
        var player = PlayerState.Instance.playerBody.transform;

        
        Vector3 direction = player.position - transform.position;

        
        direction.y = 0;

        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        print("Direction: " + direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
