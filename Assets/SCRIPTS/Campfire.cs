using System;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public bool playerInrange;

    public bool isCooking;
    public float cookingTimer;

    public CookableFood foodBeingCooked;
    public string readyFood;

    public GameObject fire;
    

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInrange = true;
        }
        else
        {
            playerInrange = false;
        }

        if (isCooking)
        {
            cookingTimer -= Time.deltaTime;
            fire.SetActive(true);
        }

        if (cookingTimer <= 0 && isCooking)
        {
            isCooking = false;
            readyFood = GetCookedFood(foodBeingCooked);
            fire.SetActive(false);
        }

    }

    private string GetCookedFood(CookableFood food)
    {
        return food.cookedFoodName;
    }

    public void OpenUI()
    {
        CampfireUIManager.Instance.OpenUI();
        CampfireUIManager.Instance.selectedCampfire = this;

        if (readyFood != "")
        {
            GameObject rf = Instantiate(Resources.Load<GameObject>(readyFood), 
                CampfireUIManager.Instance.foodSlot.transform.position, 
                CampfireUIManager.Instance.foodSlot.transform.rotation);

            rf.transform.SetParent(CampfireUIManager.Instance.foodSlot.transform);

            readyFood = "";
        }
    }

    public void StartCooking(InventoryItem food)
    {
        foodBeingCooked = ConverIntoCookable(food);

        isCooking = true;

        cookingTimer = TimeToCookFood(foodBeingCooked);
    }

    private CookableFood ConverIntoCookable(InventoryItem food)
    {
        foreach (CookableFood cookable in CampfireUIManager.Instance.cookingData.validFoods)
        {
            if (cookable.name == food.thisName)
            {
                return cookable;
            }
        }

        return new CookableFood();
    }

    private float TimeToCookFood(CookableFood food)
    {
        return food.timeToCook;
    }
}
