using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public Animator sushiAnimator;
    public GameObject ground;

    private void OnEnable()
    {
        FuchkaBehavior.OnFuchkaCollision += IncreaseSpeed;
    }

    private void OnDisable()
    {
        FuchkaBehavior.OnFuchkaCollision -= IncreaseSpeed;
    }

    void Start()
    {
        // initialize speeds
        sushiAnimator.SetFloat("speedMultiplier", 1);
        ground.GetComponent<MeshRenderer>().material.SetVector("_ScrollSpeed", new Vector2(-1, 2));
    }

    void IncreaseSpeed()
    {
        GlobalVars.runningSpeed += 0.2f;
        sushiAnimator.SetFloat("speedMultiplier", GlobalVars.runningSpeed / 2);
        ground.GetComponent<MeshRenderer>().material.SetVector("_ScrollSpeed", new Vector2(GlobalVars.runningSpeed / -2, 2));
        if (GlobalVars.spawnInterval > 0.25f) GlobalVars.spawnInterval -= 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
