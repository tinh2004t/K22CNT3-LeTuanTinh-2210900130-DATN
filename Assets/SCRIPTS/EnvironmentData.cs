using System.Collections.Generic;

[System.Serializable]

public class EnvironmentData
{
    public List<string> pickedupItems;
    public EnvironmentData(List<string> _pickedupItems)
    {
        pickedupItems = _pickedupItems;
    }
}