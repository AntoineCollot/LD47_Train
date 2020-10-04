using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    bool isLoading = false;

    public void LoadScene(int id)
    {
        if (isLoading)
            return;

        isLoading = true;
        SceneManager.LoadScene(id);
    }
}
