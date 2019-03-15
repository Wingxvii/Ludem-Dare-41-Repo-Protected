using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Label Text")]
    [Tooltip("Text Shown in Interaction UI Text")]
    public string interactText;

    [Header("Interaction Character Title Text")]
    [Tooltip("Title of Speaker shown in Dialogue Text")]
    public string titleText;

    [Header("Interaction Character Title Dialogue(s)")]
    [Tooltip("Array of Text Shown in during-interaction Dialogue Text Box")]
    public List<string> dialogueText;

    private int _dialogueIndex = 0;

    public void Act()
	{
		
		if (dialogueText.Count >= 1 
			&& !DialogueReciever.s_instance.GetIsTyping()
			&& DialogueReciever.s_instance.displayText == false)
		{
			// Display dialogue message of this Interactable
			string message = GetNextDialogueText();
			if (message != "")
			{
				DialogueReciever.s_instance.AddText(message);
			}
		}

		PowerUp power = GetComponentInChildren<PowerUp>();
		if (power)
		{
			power.isTalking = true;
		}
	}

	/// <summary>
	/// Returns the next dialogue string that has not appeared
	/// </summary>
	/// <returns>Next dialogue string that has not appeared</returns>
	public string GetNextDialogueText()
	{
		if (_dialogueIndex >= dialogueText.Count || dialogueText.Count < 1)
		{
			_dialogueIndex = 0;
			return "";
		}

		string result = dialogueText[_dialogueIndex];
		_dialogueIndex++;

		return result;
	}

}
