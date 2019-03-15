using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finder : MonoBehaviour 
{	
	// Update is called once per frame
	void Update () 
	{
        var foundObjects = FindObjectsOfType<FMOD_Listener>();
        Debug.Log(foundObjects + " : " + foundObjects.Length);
	}
}
