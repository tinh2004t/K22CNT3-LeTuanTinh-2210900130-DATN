using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;

    public float detectionRange = 10f;

    public string GetItemName()
    {
        return ItemName;
    }


    void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);
        if (distance < detectionRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {

            if (InventorySystem.Instance.CheckSlotsAvailable(1))
            {



                InventorySystem.Instance.AddToInventory(ItemName);

                InventorySystem.Instance.itemsPickedup.Add(gameObject.name);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory Full");
            }

        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        playerInRange = false;
    //    }
    //}
}