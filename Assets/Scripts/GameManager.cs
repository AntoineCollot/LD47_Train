using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static float difficulty { get; private set; }
    public static float scoreMultiplier { get; private set; }
    public static float baseScore { get; private set; }

    [Header("Difficulty")]
    [SerializeField] float timeToReachMaxDifficulty = 180;

    [Header("Score")]
    [SerializeField] float scorePerSecond = 1;
    [SerializeField] TextMeshProUGUI scoreText = null;

    [Header("GameOver")]
    public UnityEvent onGameOver = new UnityEvent();
    public static GameManager Instance;
    public static bool gameIsOver { get; private set; }

    public float Score { get => baseScore * scoreMultiplier; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gameIsOver = false;
        scoreMultiplier = 1;
        difficulty = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
            return;
        baseScore += scorePerSecond * Time.deltaTime;
        scoreText.text = $"Score : {Score.ToString("N0")}";

        difficulty += Time.deltaTime / timeToReachMaxDifficulty;
        difficulty = Mathf.Clamp01(difficulty);

        if (Input.GetKeyDown(KeyCode.Escape))
            GameOver("Gave up");
    }

    public void AddScoreMultiplier(float amount)
    {
        scoreMultiplier += amount;
    }

    public void GameOver(string msg)
    {
        Debug.Log("Game Over "+msg);
        gameIsOver = true;
        onGameOver.Invoke();
    }
}
