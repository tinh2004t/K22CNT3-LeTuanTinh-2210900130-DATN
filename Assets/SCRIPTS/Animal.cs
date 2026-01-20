using System.Collections;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Sound")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip animalHitAndScream;
    [SerializeField] AudioClip animalHitAndDie;

    private Animator animator;
    public bool isDead;

    [SerializeField] ParticleSystem bloodSplashParticles;

    enum AnimalType
    {
        Rabbit,
        Bear,
    }

    [SerializeField] AnimalType thisAnimalType;
    public GameObject bloodPuddle;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;
            bloodSplashParticles.Play();
            if (currentHealth <= 0)
            {
                PlayDyingSound();

                animator.SetTrigger("DIE");

                //if (thisAnimalType == AnimalType.Rabbit)
                //{
                //GetComponent<AI_Movement>().enabled = false;

                //}
                StartCoroutine(PuddleDelay());
                isDead = true;
            }
            else
            {
                PlayHitSound();
            }
        }
        
    }

    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(1f);
        bloodPuddle.SetActive(true);
    }

    private void PlayDyingSound()
    {
       soundChannel.PlayOneShot(animalHitAndDie);

    }

    private void PlayHitSound()
    {
                soundChannel.PlayOneShot(animalHitAndScream);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
