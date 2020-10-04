using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float difficulty { get; private set; }
    public static bool gameIsOver { get; private set; }

    public static GameManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver(string msg)
    {
        Debug.Log("Game Over "+msg);
        gameIsOver = true;
    }
}
