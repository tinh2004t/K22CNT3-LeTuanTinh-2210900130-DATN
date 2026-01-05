using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StorageBox : MonoBehaviour
{
    public bool playerInrange;

    [SerializeField] public List<string> items;

    public enum BoxType
    {
        smallBox,
        bigBox
    }

    public BoxType thisBoxType;

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInrange = true;
        }
        else
        {
            playerInrange = false;
        }
    }
}
