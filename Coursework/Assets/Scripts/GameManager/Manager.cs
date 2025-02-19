using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 1f;
    public GameObject completeLevelUI;

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            // restart game
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        // finds the current name of the scene and loads it.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
