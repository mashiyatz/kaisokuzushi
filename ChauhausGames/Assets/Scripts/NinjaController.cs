using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaController : MonoBehaviour
{
    public enum LANE { LEFT, CENTER, RIGHT };
    public float[] dutchAngles;
    private Rigidbody rb;
    [SerializeField] private LANE currentLane;
    public Vector3 leftPos;
    public Vector3 centerPos;
    public Vector3 rightPos;
    public float translationSpeed;

    private Coroutine translationCoroutine;
    public CinemachineVirtualCamera backCam;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;

    private bool isGameOver = false;

    private void OnEnable()
    {
        ObstacleBehavior.OnObstacleCollision += BlastingOffAgain;
    }

    private void OnDisable()
    {
        ObstacleBehavior.OnObstacleCollision -= BlastingOffAgain;
    }

    private void BlastingOffAgain()
    {
        StartCoroutine(FallingBack());
    }

    void Start()
    {
        currentLane = LANE.CENTER;
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator FallingBack()
    {
        float timer = 0;
        while (timer < 1.0f)
        {
            transform.Translate(Vector3.back * GlobalVars.runningSpeed * 2); 
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator MoveToPos(int laneIndex)
    {
        LANE lane = (LANE)laneIndex;
        Vector3 endPos = Vector3.zero;
        currentLane = lane;
        switch (lane) {
            case LANE.LEFT:
                endPos = leftPos;
                break;
            case LANE.CENTER:
                endPos = centerPos;
                break;
            case LANE.RIGHT:
                endPos = rightPos;
                break;
            default: 
                break;
        }
        float currentDutch = backCam.m_Lens.Dutch;
        Vector3 currentPos = transform.position;
        while (transform.position != endPos)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, endPos, translationSpeed * Time.deltaTime);
            backCam.m_Lens.Dutch = Mathf.Lerp(currentDutch, dutchAngles[(int)currentLane], 
                1 - Vector3.Distance(transform.position, endPos) / Vector3.Distance(currentPos, endPos));
            transform.position = newPos;
            yield return null;
        }
    }

    void InterpretTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchEndPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEndPos = touch.position;
            }

            if (Vector2.Distance(touchStartPos, touchEndPos) > 100)
            {
                if (touchEndPos.x > touchStartPos.x) Shift(true);
                else if (touchEndPos.x < touchStartPos.x) Shift(false);
            }
        }
    }

    void Shift(bool right)
    {
        if (!right) {
            if ((int)currentLane - 1 >= 0)
            {
                if (translationCoroutine != null) StopCoroutine(translationCoroutine);
                translationCoroutine = StartCoroutine(MoveToPos((int)currentLane - 1));
            }
        }
        else {
            if ((int)currentLane + 1 < 3)
            {
                if (translationCoroutine != null) StopCoroutine(translationCoroutine);
                translationCoroutine = StartCoroutine(MoveToPos((int)currentLane + 1));
            }
        }
    }

    
    void Update()
    {
        if (isGameOver) return;

        // touchscreen
        InterpretTouch();

        // keyboard
        if (Input.GetKeyDown(KeyCode.LeftArrow)) Shift(false);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Shift(true);
    }
}
