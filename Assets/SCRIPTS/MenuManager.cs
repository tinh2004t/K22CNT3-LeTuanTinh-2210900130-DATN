using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance { get; private set; }

    public GameObject menuCanvas;
    public GameObject uiCanvas;

    public GameObject saveMenu;
    public GameObject settingMenu;
    public GameObject menu;

    public bool isMenuOpen;

    public int currentFront = 0;


    public int SetAsFront ()
    {
        return currentFront++;  
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isMenuOpen)
        {
            uiCanvas.SetActive(false);
            menuCanvas.SetActive(true);

            isMenuOpen = true;

            MovementManager.Instance.EnableLook(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        }
        else if (Input.GetKeyDown(KeyCode.M) && isMenuOpen)
        {
            settingMenu.SetActive(false);
            saveMenu.SetActive(false);
            menu.SetActive(true);


            menuCanvas.SetActive(false);
            uiCanvas.SetActive(true);
            isMenuOpen = false;

            MovementManager.Instance.EnableLook(true);

            if (!CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }

    


}
