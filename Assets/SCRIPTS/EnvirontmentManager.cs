using UnityEngine;

public class EnvirontmentManager : MonoBehaviour
{
   public static EnvirontmentManager Instance { get; set; }

    public GameObject allItems;

    private void Awake()
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
}
