using UnityEngine;


[CreateAssetMenu(fileName = "Checkpoint", menuName = "ScriptableObject/Checkpoint", order = 1)]

public class Checkpoint : ScriptableObject
{

    public string name;
    public bool isCompleted = false;

}
