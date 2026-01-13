using System;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] GameObject seedModel;
    [SerializeField] GameObject youngPlantModel;
    [SerializeField] GameObject maturePlantModel;

    [SerializeField] List<GameObject> plantProduceSpawns;

    [SerializeField] GameObject producePrefab;

    [SerializeField] public int dayOfPlanting;
    [SerializeField] int plantAge = 0; //depends on the watering frequency

    [SerializeField] int ageForYoungModel;
    [SerializeField] int ageForMatureModel;
    [SerializeField] int ageForFirstProduceBatch;

    [SerializeField] int daysForNewProduce; // Days it takes for new fruit to grow after the initial batch;
    [SerializeField] int daysRemainingForNewProduceCounter;

    [SerializeField] bool isOneTimeHarvest;
    [SerializeField] public bool isWatered; // Only if the plant is watered at the end of the day, it will "age";

    private void OnEnable()
    {
        TimeManager.Instance.OnDayPass.AddListener(Daypass);
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnDayPass.RemoveListener(Daypass);
    }

    private void OnDestroy()
    {
        GetComponentInParent<Soil>().isEmpty = true;
        GetComponentInParent<Soil>().plantName = "";
        GetComponentInParent<Soil>().currentPlant = null;


    }

    private void Daypass()
    {
        if (isWatered)
        {
            plantAge++;

            isWatered = false;
            GetComponentInParent<Soil>().MakeSoilNotWatered();
        }

        CheckGrowth();

        if (!isOneTimeHarvest)
        {
            CheckProduce();

        }


    }

    private void CheckProduce()
    {
        if (plantAge == ageForFirstProduceBatch)
        {
            GenerateProduceForEmptySpawns();
        }

        if (plantAge > ageForFirstProduceBatch)
        {
            if (daysRemainingForNewProduceCounter == 0)
            {
                GenerateProduceForEmptySpawns();

                daysRemainingForNewProduceCounter = daysForNewProduce;
            }
            else
            {
                daysRemainingForNewProduceCounter--;
            }
        }
    }

    private void GenerateProduceForEmptySpawns()
    {
        foreach (GameObject spawn in plantProduceSpawns)
        {
            if (spawn.transform.childCount == 0)
            {
                // Instantiate the produce from the prefab.
                GameObject produce = Instantiate(producePrefab);

                // Set the produce to be a child of the current spawn in the list.
                produce.transform.parent = spawn.transform;

                // Position the produce in the middle of the spawn
                Vector3 producePosition = Vector3.zero;
                producePosition.y = 0f;
                produce.transform.localPosition = producePosition;
            }
        }
    }

    private void CheckGrowth()
    {
        seedModel.SetActive(plantAge < ageForYoungModel);
        youngPlantModel.SetActive(plantAge >= ageForYoungModel && plantAge < ageForMatureModel);
        maturePlantModel.SetActive(plantAge >= ageForMatureModel);

        if (plantAge >= ageForMatureModel && isOneTimeHarvest)
        {
            MakePlantPickable();
        }

    }

    private void MakePlantPickable()
    {
        GetComponent<InteractableObject>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }
}
