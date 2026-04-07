using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    [Header("Settings")]
    public VideoPlayer introVideo;
    public string gameSceneName = "GameScene";
    public float timeToSkip = 3f;

    [Header("UI Skip Elements")]
    public GameObject skipPromptRoot;
    public Image holdFillImage;

    [Header("Fade Transition")]
    public CanvasGroup faderCanvasGroup;
    public float fadeDuration = 1f;    

    private float currentHoldTime = 0f;
    private bool isTransitioning = false;

    void Start()
    {
        if (introVideo != null)
        {
            introVideo.loopPointReached += OnVideoEnd;
        }

        if (skipPromptRoot != null) skipPromptRoot.SetActive(false);

        if (faderCanvasGroup != null) faderCanvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (isTransitioning) return;

        if (Input.GetKey(KeyCode.Space))
        {
            if (skipPromptRoot != null && !skipPromptRoot.activeSelf)
                skipPromptRoot.SetActive(true);

            currentHoldTime += Time.deltaTime;

            if (holdFillImage != null)
            {
                holdFillImage.fillAmount = Mathf.Clamp01(currentHoldTime / timeToSkip);
            }

            if (currentHoldTime >= timeToSkip)
            {
                StartTransition();
            }
        }
        else
        {
            if (currentHoldTime > 0)
            {
                currentHoldTime = 0f;
                if (holdFillImage != null) holdFillImage.fillAmount = 0f;
                if (skipPromptRoot != null) skipPromptRoot.SetActive(false);
            }
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StartTransition();
    }

    void StartTransition()
    {
        if (isTransitioning) return;

        isTransitioning = true;

        if (skipPromptRoot != null) skipPromptRoot.SetActive(false);

        if (introVideo != null && introVideo.isPlaying)
        {
            introVideo.Pause();
        }

        StartCoroutine(FadeToBlackAndLoad());
    }

    IEnumerator FadeToBlackAndLoad()
    {
        float timer = 0f;

        if (faderCanvasGroup == null)
        {
            Debug.LogError("Chưa gán CanvasGroup cho Fader!");
            SceneManager.LoadScene(gameSceneName);
            yield break;
        }

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            faderCanvasGroup.alpha = timer / fadeDuration;

            yield return null;
        }

        faderCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(gameSceneName);
    }
}