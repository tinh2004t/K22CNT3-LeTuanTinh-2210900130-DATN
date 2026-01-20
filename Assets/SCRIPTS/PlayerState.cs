using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // ------ Player Health ------

    public float currentHealth;
    public float maxHealth;




    // ------ Player Calories ------
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;


    // ------ Player Hydration ------
    public float currentHydrationPercent;
    public float maxHydrationPercent;

    public bool isHydrationActive;

    // ------ Player Oxygen ------
    public float currentOxygenPercent;
    public float maxOxygenPercent = 100;
    public float oxygenDecreasedPerSecond = 1f;
    private float oxygenTimer = 0f;
    private float decreaseInterval = 1f; // Decrease oxygen every second

    public float outOfAirDamegePerSecond = 5f;



    void Awake()
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


    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        currentOxygenPercent = maxOxygenPercent;


        StartCoroutine(decreaseHydration());


    }

    IEnumerator decreaseHydration()
    {
        while (true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(15);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerBody.GetComponent<PlayerMovement>().isUnderwater)
        {
            oxygenTimer += Time.deltaTime;

            if (oxygenTimer >= decreaseInterval)
            {
                DecreaseOxygen();
                oxygenTimer = 0f;
            }
        }


        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;

        }





        //Test code to decrease health
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }


        
    }

    private void DecreaseOxygen()
    {
        currentOxygenPercent -= oxygenDecreasedPerSecond * decreaseInterval;

        if (currentOxygenPercent < 0)
        {
            currentOxygenPercent = 0;
            setHealth(currentHealth - outOfAirDamegePerSecond);
        }
    }

    public void setHealth(float newHealth)
    {

        currentHealth = newHealth;
    }

    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }

    public void setHydration(float newHydration)
    {
        currentHydrationPercent = newHydration;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead");
        }
        else
        {
            Debug.Log("Player is hurt");
        }
    }
}
