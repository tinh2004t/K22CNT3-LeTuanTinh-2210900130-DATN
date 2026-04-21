using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    [Header("Sound")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip animalHitAndScream;
    [SerializeField] AudioClip animalHitAndDie;

    [SerializeField] AudioClip animalAttack;

    private Animator animator;
    public bool isDead;

    [SerializeField] ParticleSystem bloodSplashParticles;

    public Slider healthBarSlider;

    public event Action OnDestroyed;

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

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        OnDestroyed?.Invoke();
    }


    public void TakeDamage(int damage)
    {

        if (isDead == false)
        {
            currentHealth -= damage;
            healthBarSlider.value = currentHealth / maxHealth;

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
                animator.SetTrigger("HURT");

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

    public void PlayAttackSound()
    {
        soundChannel.PlayOneShot(animalAttack);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            healthBarSlider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            healthBarSlider.gameObject.SetActive(false);
        }
    }

}
