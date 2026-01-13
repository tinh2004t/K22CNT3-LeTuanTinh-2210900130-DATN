using System;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class Soil : MonoBehaviour
{
    public bool isEmpty = true;
    public bool playerInrange;

    public string plantName;

    public Plant currentPlant;

    public Material defaultMaterial;
    public Material wateredMaterial;

    internal void MakeSoilWatered()
    {
        GetComponent<Renderer>().material = wateredMaterial;
    }

    internal void MakeSoilNotWatered()
    {
        GetComponent<Renderer>().material = defaultMaterial;
    }

    internal void PlantSeed()
    {
        InventoryItem selectedSeed = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>();
        isEmpty = false;

        string onlyPlantName = selectedSeed.thisName.Split(new string[] { " Seed" }, StringSplitOptions.None)[0];
        plantName = onlyPlantName;

        // Instantiate Plant Prefab
        GameObject instantiatedPlant = Instantiate(Resources.Load($"{onlyPlantName} Plant") as GameObject);

        // Set the instantiated plant to be a child of the soil
        instantiatedPlant.transform.parent = gameObject.transform;

        // Make the plant's position in the middle of the soil
        Vector3 plantPosition = Vector3.zero;
        plantPosition.y = 0f;
        instantiatedPlant.transform.localPosition = plantPosition;

        currentPlant = instantiatedPlant.GetComponent<Plant>();

        currentPlant.dayOfPlanting = TimeManager.Instance.dayInGame;
    }

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
    }
}
