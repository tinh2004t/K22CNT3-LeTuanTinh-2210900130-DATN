using UnityEngine;
using UnityEngine.UI;

public class CaloriesBara : MonoBehaviour
{
    private Slider slider;
    public TMPro.TMP_Text caloriesCounter;

    public GameObject playerState;

    private float currentCalories, maxCalories;



    void Awake()
    {
        slider = GetComponent<Slider>();


    }

    // Update is called once per frame
    void Update()
    {
        currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerState>().maxCalories;

        float fillValue = currentCalories / maxCalories;
        slider.value = fillValue;

        caloriesCounter.text = currentCalories + " / " + maxCalories;



    }
}
