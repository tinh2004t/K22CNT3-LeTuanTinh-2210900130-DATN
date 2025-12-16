using System.Collections;
using System.Security.Cryptography;
using UnityEngine;


[RequireComponent(typeof(Animation))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            InventorySystem.Instance.isOpen == false && 
            CraftingSystem.Instance.isOpen == false && 
            SelectionManager.Instance.handIsVisible == false && 
            !ConstructionManager.Instance.inConstructionMode) //Left mouse button
        {

            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");

        }



    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);

            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);

    }
}
