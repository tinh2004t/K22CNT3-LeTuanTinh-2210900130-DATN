using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public bool onTarget;

    public GameObject selectedObject;


    public TextMeshProUGUI interaction_text;
    public GameObject interaction_Info_UI;

    public Image centerDotImage;
    public Image handIcon;


    public bool handIsVisible;

    public GameObject selectedTree;
    public GameObject chopHolder;


    public void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponentInChildren<TextMeshProUGUI>();
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
            var interactable = selectionTransform.GetComponent<InteractableObject>();

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            if (choppableTree && choppableTree.playerInRange)
            {
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

            // Kiểm tra: Có script interactable KHÔNG? VÀ Player có đang trong vùng không?
            if (interactable != null && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);


                if (interactable.CompareTag("pickable"))
                {
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                    handIsVisible = true;

                }
                else
                {
                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                    handIsVisible = false;
                }

                }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
                handIsVisible = false;
            }
        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            centerDotImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
            handIsVisible = false;
        }
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