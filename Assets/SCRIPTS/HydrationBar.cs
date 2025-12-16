using UnityEngine;
using UnityEngine.UI;

public class HydrationBar : MonoBehaviour
{
    private Slider slider;
    public TMPro.TMP_Text hydrationCounter;

    public GameObject playerState;

    private float currentHydration, maxHydration;



    void Awake()
    {
        slider = GetComponent<Slider>();


    }

    // Update is called once per frame
    void Update()
    {
        currentHydration = playerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxHydration = playerState.GetComponent<PlayerState>().maxHydrationPercent;

        float fillValue = currentHydration / maxHydration;
        slider.value = fillValue;

        hydrationCounter.text = currentHydration + " %";



    }
}
