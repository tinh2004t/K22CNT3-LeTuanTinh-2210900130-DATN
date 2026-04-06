using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    [Header("Settings")]
    public VideoPlayer introVideo;
    public string gameSceneName = "GameScene";
    public float timeToSkip = 3f;

    private float currentHoldTime = 0f;
    private bool isSkipping = false;

    void Start()
    {
        if (introVideo != null)
        {
            introVideo.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("Chưa gán VideoPlayer vào IntroManager!");
        }
    }

    void Update()
    {
        if (isSkipping) return;

        if (Input.GetKey(KeyCode.Space))
        {
            currentHoldTime += Time.deltaTime;


            if (currentHoldTime >= timeToSkip)
            {
                SkipIntro();
            }
        }
        else
        {
            currentHoldTime = 0f;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SkipIntro();
    }

    void SkipIntro()
    {
        isSkipping = true;
        SceneManager.LoadScene(gameSceneName);
    }
}