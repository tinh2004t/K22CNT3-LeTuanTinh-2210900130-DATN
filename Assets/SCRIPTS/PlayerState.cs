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

    public bool isPlayerDead;


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

    public AudioSource playerAudioSource;
    public AudioClip playerPainSound;
    public AudioClip playerDeathSound;

    public RespawnLocation registeredRespawnLocation;
    public event Action OnRespawnRegistered;

    private float hurtSoundDelay = 2f;
    private float nextHurtTime = 0f;

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

        if (currentHealth <= 0 && !isPlayerDead)
        {
            PlayerDead();
        }
        else
        {
            if (currentHealth > 0 && Time.time >= nextHurtTime)
            {
                playerAudioSource.PlayOneShot(playerPainSound);
                Debug.Log("Player is hurt");
                
                nextHurtTime = Time.time + hurtSoundDelay;
            }
        }
    }

    public void PlayerDead()
    {
        isPlayerDead = true;
        playerAudioSource.PlayOneShot(playerDeathSound);

        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }
    public IEnumerator RespawnCoroutine()
    {
        playerBody.GetComponent<PlayerMovement>().enabled = false;
        playerBody.GetComponent<MouseMovement>().enabled = false;

        if (registeredRespawnLocation != null)
        {
            Vector3 position = registeredRespawnLocation.transform.position;

            position.y += 3f; // above the location
            position.z += 7f; // next to the location

            playerBody.transform.position = position; // actually respawn the player

            currentHealth = maxHealth;
        }

        yield return new WaitForSeconds(0.2f);

        isPlayerDead = false;

        playerBody.GetComponent<PlayerMovement>().enabled = true;
        playerBody.GetComponent<MouseMovement>().enabled = true;
    }

    internal void SetRegisteredLocation(RespawnLocation respawnLocation)
    {
        registeredRespawnLocation = respawnLocation;
        OnRespawnRegistered?.Invoke();
    }
}
