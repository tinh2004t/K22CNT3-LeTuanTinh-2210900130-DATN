using System.Collections;
using System.Security.Cryptography;
using UnityEngine;


[RequireComponent(typeof(Animation))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;





    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
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
            return;
        }

        Transform targetedTransform = SelectionManager.Instance.currentTarget;

        if (targetedTransform != null)
        {
            Animal animal = targetedTransform.GetComponent<Animal>();

            if (animal != null && animal.playerInRange && !animal.isDead)
            {
                int damage = EquipSystem.Instance.GetWeaponDamage();

                SelectionManager.Instance.StartCoroutine(SelectionManager.Instance.DealDamageTo(animal, 0f, damage));
            }
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);

    }
}
