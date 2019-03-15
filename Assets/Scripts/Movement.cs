using UnityEngine;

public class Movement : MonoBehaviour {

    public Rigidbody playerBody;
    public float forwardSpeed;
    public float strafeSpeed;
    public float brakeSpeed;

	// Update is called once per frame
	void Update () {
        if (Input.GetKey("w")) {
            playerBody.AddForce(0, 0, forwardSpeed * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            playerBody.AddForce(-strafeSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("d"))
        {
            playerBody.AddForce(strafeSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey("s"))
        {
            playerBody.AddForce(0, 0, -brakeSpeed * Time.deltaTime);
        }

    }
}
