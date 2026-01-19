using UnityEngine;

public class MovementManager : MonoBehaviour
{

    #region ||-- Singleton --||
    public static MovementManager Instance { get; set; }

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
    #endregion

    public bool canMove = true;
    public bool canLookAround = true;

    public void EnableMovement(bool trigger)
    {
        canMove = trigger;
    }

    public void EnableLook(bool trigger)
    {
        canLookAround = trigger;
    }

}
