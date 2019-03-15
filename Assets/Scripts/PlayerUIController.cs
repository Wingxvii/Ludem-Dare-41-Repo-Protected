using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour 
{
	public static PlayerUIController s_instance;

	public Text actionText;


	void Awake ()
	{
		s_instance = this;

		actionText.text = "";

		Cursor.visible = false;		
	}

	public void SetActionText(string newText = "")
	{
		actionText.text = newText;
	}

	public void SetActionText(Interactable interact)
	{
		actionText.text = interact.interactText;
	}
}
