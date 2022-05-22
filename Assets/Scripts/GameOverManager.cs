using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] int mainMenuSceneNumber = 0;

    public void QuitGame()
    {
        if (Time.timeScale <= 0 || SongManager.Instance.IsPaused) 
        { SongManager.Instance.ToggleScaleAndPauseVar(false); }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(mainMenuSceneNumber, LoadSceneMode.Single);
    }

    public void RetryLevel()
    {
        if (Time.timeScale <= 0 || SongManager.Instance.IsPaused)
        { SongManager.Instance.ToggleScaleAndPauseVar(false); }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
