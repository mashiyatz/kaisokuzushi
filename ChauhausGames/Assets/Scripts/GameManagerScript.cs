using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public enum PHASE { START, RUN, END, WAIT }
    public PHASE currentPhase;
    public GameObject[] vcams;
    public Animator sushiAnimator;

    public ObstacleGenerator obstacleGenerator;
    public GameObject startPrompt;
    public GameObject instructions;
    public GameObject resetScreen;
    public TextMeshProUGUI scoreTextbox;
    public TextMeshProUGUI resultsTextbox;
    public TextMeshProUGUI highScoreTextbox;
    public GameObject newRecordText;

    private bool isGameOver = false;
    private float score = 0;
    private float timer = 0;

    private void OnEnable()
    {
        ObstacleBehavior.OnObstacleCollision += PromptGameReset;
    }

    private void OnDisable()
    {
        ObstacleBehavior.OnObstacleCollision -= PromptGameReset;
    }

    private void PromptGameReset()
    {
        StartCoroutine(DelayToEndScreen());
    }

    IEnumerator DelayToEndScreen()
    {
        isGameOver = true;
        scoreTextbox.enabled = false;
        yield return new WaitForSeconds(1.5f);
        resetScreen.SetActive(true);
        resultsTextbox.text = $"Score: {(score * 10):00}";

        float highScore = PlayerPrefs.GetFloat("High Score", 0);
        highScoreTextbox.text = $"Last high score: {highScore:00}";
        if (score * 10 > highScore) {
            PlayerPrefs.SetFloat("High Score", score * 10);
            newRecordText.SetActive(true);
        }

        currentPhase = PHASE.END;
    }

    void Start()
    {
        resetScreen.SetActive(false);
        newRecordText.SetActive(false);
        SetPhase(PHASE.START);
    }

    public void SetPhase(PHASE phase)
    {
        if (phase == PHASE.START)
        {
            vcams[0].SetActive(true);
            vcams[1].SetActive(false);
            sushiAnimator.ResetTrigger("startRun");
            sushiAnimator.SetTrigger("stopRun");
            
            obstacleGenerator.enabled = false;
            startPrompt.SetActive(true);
            instructions.SetActive(false);
            scoreTextbox.enabled = false;
        }
        else if (phase == PHASE.RUN) {
            vcams[0].SetActive(false);
            vcams[1].SetActive(true);
            sushiAnimator.ResetTrigger("stopRun");
            sushiAnimator.SetTrigger("startRun");

            obstacleGenerator.enabled = true;
            startPrompt.SetActive(false);
            StartCoroutine(ReplaceInstructionsAfterWait());
        }
        currentPhase = phase;
    }

    IEnumerator ReplaceInstructionsAfterWait()
    {
        instructions.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        instructions.SetActive(false);
        scoreTextbox.enabled = true;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 30)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (currentPhase == PHASE.RUN && !isGameOver)
        {
            score += Time.deltaTime * GlobalVars.runningSpeed;
            scoreTextbox.text = $"{(score * 10):00}";
        }

        if (Input.touchCount > 0)
        {
            timer = 0;
            if (currentPhase == PHASE.START && Time.timeSinceLevelLoad > 1.0f) SetPhase(PHASE.RUN);
            if (currentPhase == PHASE.END)
            {
                currentPhase = PHASE.WAIT;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
