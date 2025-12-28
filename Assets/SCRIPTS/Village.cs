using UnityEngine;

public class Village : MonoBehaviour
{
    public Checkpoint reachVillage_Dana;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            reachVillage_Dana.isCompleted = true;
        }
    }
}

