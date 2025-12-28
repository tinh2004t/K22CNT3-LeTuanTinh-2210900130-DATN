using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CraftingSystem : MonoBehaviour
{ 
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, refineScreenUI, constructionScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //CaegoryButtons
    Button toolsBTN, survivalBTN, refineBTN, constructionBTN;                                                                 

    //Craft Buttons
    Button craftAxeBTN, craftPlankBTN, craftFoundationBTN, craftWallBTN;

    //Requirement Text
    public TMP_Text AxeReq1, AxeReq2, PlankReq1, FoundationReq1, WallReq1;

    public bool isOpen;

    //All Blueprint

    public Blueprint AxeBLP = new Blueprint("Axe",1, 2, "Stone", 3, "Stick", 3);
    public Blueprint PlankBLP = new Blueprint("Plank",2, 1, "Log", 1, "", 0);
    public Blueprint FoundationBLP = new Blueprint("Foundation",3, 1, "Plank", 1, "", 0);
    public Blueprint WallBLP = new Blueprint("Wall",3, 1, "Plank", 2, "", 0);



    public static CraftingSystem Instance { get; set; }

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isOpen = false;


        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

        constructionBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        constructionBTN.onClick.AddListener(delegate { OpenConstructionCategory(); });


        //Axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TMP_Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TMP_Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        //Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<TMP_Text>();

        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });

        //Foundation
        FoundationReq1 = constructionScreenUI.transform.Find("Foundation").transform.Find("req1").GetComponent<TMP_Text>();

        craftFoundationBTN = constructionScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
        craftFoundationBTN.onClick.AddListener(delegate { CraftAnyItem(FoundationBLP); });

        //Wall
        WallReq1 = constructionScreenUI.transform.Find("Wall").transform.Find("req1").GetComponent<TMP_Text>();

        craftWallBTN = constructionScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate { CraftAnyItem(WallBLP); });
    }



    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);

    }

    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);

    }

    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);

    }

    void OpenConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        constructionScreenUI.SetActive(true);
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);


        StartCoroutine(craftedDelayForSound(blueprintToCraft));
        
        if (blueprintToCraft.numOfRequriments == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);



        }else if (blueprintToCraft.numOfRequriments == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }



        StartCoroutine(calculale());




    }


    public IEnumerator calculale()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculeList();
        RefreshNeededItems();

    }

    public IEnumerator craftedDelayForSound(Blueprint blueprintToCraft)
    {
        
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < blueprintToCraft.numberOfItemsProduced; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }

    }



    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {


            craftingScreenUI.SetActive(true);

            craftingScreenUI.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen )
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);



            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            }

            isOpen = false;
        }

    }

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;
        int plank_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count++;
                    break;
                case "Stick":
                    stick_count++;
                    break;
                case "Log":
                    log_count++;
                    break;
                case "Plank":
                    plank_count++;
                    break;
            }


        }

        //--- Axe ---//

        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);

        }


        //--- Plank x2 ---//

        PlankReq1.text = "1 Log [" + log_count + "]";

        if (log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);

        }

        //--- Foundation x3 ---//

        FoundationReq1.text = "1 Plank [" + plank_count + "]";

        if (plank_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(3))
        {
            craftFoundationBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFoundationBTN.gameObject.SetActive(false);

        }

        //--- Wall x3 ---//

        WallReq1.text = "2 Plank [" + plank_count + "]";

        if (plank_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);

        }

    }



}
