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
        if (Input.GetMouseButtonDown(0) && //Left mouse button
            !InventorySystem.Instance.isOpen && 
            !CraftingSystem.Instance.isOpen && 
            !SelectionManager.Instance.handIsVisible && 
            !ConstructionManager.Instance.inConstructionMode &&
            !DialogSystem.Instance.dialogUIActive &&
            !QuestManager.Instance.isQuestMenuOpen &&
            !MenuManager.Instance.isMenuOpen)

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
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);

    }
}
