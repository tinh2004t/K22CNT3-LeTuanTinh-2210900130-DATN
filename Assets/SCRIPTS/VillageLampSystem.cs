using System;
using UnityEngine;

public class VillageLampSystem : MonoBehaviour
{
    public int timeLampsTurnOn;
    public int timeLampsTurnOff;

    public bool lampsAreOn;

    public DayNightSystem DayNightSystem;

    private void Update()
    {
        if(DayNightSystem.currentHour == timeLampsTurnOn && !lampsAreOn)
        {
            TurnOnLamps();
        }
        if(DayNightSystem.currentHour == timeLampsTurnOff && lampsAreOn)
        {
            TurnOffLamps();
        }
    }

    private void TurnOffLamps()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Light>().enabled = false;
        }
        lampsAreOn = false;
    }

    private void TurnOnLamps()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Light>().enabled = true;
        }
        lampsAreOn = true;
    }
}
