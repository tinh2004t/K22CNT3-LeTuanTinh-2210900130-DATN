using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    private Slider slider;
    public TMPro.TMP_Text oxygenCounter;

    public GameObject playerState;

    private float currentOxygen, maxOxygen;



    void Awake()
    {
        slider = GetComponent<Slider>();


    }

    // Update is called once per frame
    void Update()
    {
        currentOxygen = PlayerState.Instance.currentOxygenPercent;
        maxOxygen = PlayerState.Instance.maxOxygenPercent;

        float fillValue = currentOxygen / maxOxygen;
        slider.value = fillValue;

        oxygenCounter.text = currentOxygen + " %";



    }
}
