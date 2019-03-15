using UnityEngine;

public class MoveForward : MonoBehaviour {
    public Rigidbody self;
    public float speed;
    public UIManager ui;

    // Update is called once per frame
    void Update () {
        if (ui.raceStarted)
        {
            self.velocity = new Vector3(0, 0, speed);
        }
        else{
            self.velocity = Vector3.zero;
        }
    }
}
