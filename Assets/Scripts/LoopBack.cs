
using UnityEngine;

public class LoopBack : MonoBehaviour {

    public Transform self;
    public UIManager ui;
    public float timing;

	// Update is called once per frame
	void Update () {
        //resets itself
        if (self.position.z > timing*(ui.endTime/2)) {
            self.position = new Vector3(self.position.x, 0, -timing * (ui.endTime/2));
        }
	}
}
