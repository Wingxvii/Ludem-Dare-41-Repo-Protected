using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsEvent : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Y))
        {
            Application.Quit();
        }
    }
}
