using UnityEngine;
using UnityEngine.UI;

public class HeathBar : MonoBehaviour
{
    private Slider slider;
    public TMPro.TMP_Text healthCounter;

    public GameObject playerState;

    private float currentHealth, maxHealth;



    void Awake()
    {
        slider = GetComponent<Slider>();


    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;

        healthCounter.text = currentHealth + " / " + maxHealth;



    }
}
