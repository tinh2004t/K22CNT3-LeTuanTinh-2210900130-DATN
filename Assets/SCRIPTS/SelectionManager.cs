using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;

    public GameObject selectedObject;


    public TMPro.TextMeshProUGUI interaction_text;
    public GameObject interaction_Info_UI;

    public Image centerDotImage;
    public Image handIcon;


    public bool handIsVisible;

    public GameObject selectedTree;
    public GameObject chopHolder;

    public GameObject selectedStorageBox;

    public GameObject selectedCampfire;


    public void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            var selectionTransform = hit.transform;



            NPC npc = selectionTransform.GetComponent<NPC>();

            if (npc && npc.playerInRange)
            {
                interaction_text.text = "Talk";
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButtonDown(0) && npc.isTalkingWithPlayer == false)
                {
                    npc.StartConversation();
                }

                if (DialogSystem.Instance.dialogUIActive)
                {
                    interaction_Info_UI.SetActive(false);
                    centerDotImage.gameObject.SetActive(true);
                }
            }

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            // Nếu không thấy, thử tìm trong các đối tượng con (vì script có thể nằm ở object con trigger như 'tree_base')
            if (choppableTree == null)
            {
                choppableTree = selectionTransform.GetComponentInChildren<ChoppableTree>();
            }

            // (Tùy chọn) Đôi khi raycast trúng lá cây (con), script lại nằm ở cha, nên có thể thêm dòng này để chắc chắn:
            if (choppableTree == null)
            {
                choppableTree = selectionTransform.GetComponentInParent<ChoppableTree>();
            }

            if (choppableTree && choppableTree.playerInRange)
            {
                interaction_text.text = "Chop";
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);

                GlobalState.Instance.resourceHealth = choppableTree.treeHealth;
                GlobalState.Instance.resourceMaxHealth = choppableTree.treeMaxHealth;
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            var interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable != null && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;
            }

            StorageBox storageBox = selectionTransform.GetComponent<StorageBox>();

            if (storageBox && storageBox.playerInrange && !PlacementSystem.Instance.inPlacementMode)
            {
                interaction_text.text = "Open";
                interaction_Info_UI.SetActive(true);

                selectedStorageBox = storageBox.gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    StorageManager.Instance.OpenBox(storageBox);
                }
            }
            else
            {
                if (selectedStorageBox != null)
                {
                    selectedStorageBox = null;
                }
            }

            Campfire campfire = selectionTransform.GetComponent<Campfire>();

            if (campfire && campfire.playerInrange && !PlacementSystem.Instance.inPlacementMode)
            {
                interaction_text.text = "Interact";
                interaction_Info_UI.SetActive(true);

                selectedCampfire = campfire.gameObject;

                if (Input.GetMouseButtonDown(0) && campfire.isCooking == false)
                {
                    campfire.OpenUI();
                }
            }
            else
            {
                if (selectedStorageBox != null)
                {
                    selectedStorageBox = null;
                }
            }

            Animal animal = selectionTransform.GetComponent<Animal>();

            if (animal && animal.playerInRange)
            {
                if (animal.isDead)
                {
                    interaction_text.text = "Loot";
                    interaction_Info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        Lootable lootable = animal.GetComponent<Lootable>();
                        Loot(lootable);
                    }
                }
                else
                {
                    interaction_text.text = animal.animalName;
                    interaction_Info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject .SetActive(false);

                    handIsVisible = false;

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                    {
                        StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                    }
                }
            }

            if (!interactable && !animal)
            {
                onTarget = false; 
                handIsVisible = false;

                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

            if (!npc && !interactable && !animal && !choppableTree && !storageBox && !campfire)
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false); 
            }
        }
    }

    private void Loot(Lootable lootable)
    {
        if (lootable.wasLootCalculated == false)
        {
            List<LootReceived> receivedLoot = new List<LootReceived>();

            foreach (LootPossibility loot in lootable.possibleLoot)
            {
                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax + 1);
                if (lootAmount != 0)
                {
                    LootReceived lt = new LootReceived();
                    lt.item = loot.item;
                    lt.amount = lootAmount;

                    receivedLoot.Add(lt);
                }
            }

            lootable.finalLoot = receivedLoot;
            lootable.wasLootCalculated = true;
        }

        // Spawning the loot on the ground
        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;

        foreach (LootReceived lootReceived in lootable.finalLoot)
        {
            for (int i = 0; i < lootReceived.amount; i++)
            {
                
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootReceived.item.name + "_Model"),
                    new Vector3(lootSpawnPosition.x, lootSpawnPosition.y + 0.2f, lootSpawnPosition.z),
                    Quaternion.Euler(0, 0, 0));
                print(lootSpawn);
                
            }
        }

        // If we want the blood puddle to stay on the ground
        if (lootable.GetComponent<Animal>())
        {
            lootable.GetComponent<Animal>().bloodPuddle.transform.SetParent(lootable.transform.parent);
        }

        // Destroy Looted body
        Destroy(lootable.gameObject);

    }

    IEnumerator  DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        animal.TakeDamage(damage);

    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}