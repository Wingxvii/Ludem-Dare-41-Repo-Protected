using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Restart : MonoBehaviour {
    
    //call to restart
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

    //call to go to credits scene
    public void Credits() {
        SceneManager.LoadScene(1); // loads credits scene
    }
}
