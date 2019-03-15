using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class DialogueReciever : MonoBehaviour 
{
    public static DialogueReciever s_instance; // Singleton

    public Text dialogueText;
    public Image textBox;
    public bool displayText = false;
    public string fullText;
    public float letterPause;
    public bool startTyping = false;

    [FMODUnity.EventRef]
    public string inputsound;

    void Awake()
    {
        s_instance = this;
    }

    // Use this for initialization
    void Start () 
    {
        dialogueText.enabled = false;
        textBox.enabled = false;
        dialogueText.text = "";
	}

    //use this function to open text box
    public void AddText(string outputText) 
    {
        StopCoroutine(TypeText());
        dialogueText.text = "";
        fullText = outputText;
        displayText = true;
        startTyping = true;
    }

	// Update is called once per frame
	void Update () 
    {
        //check if text should be displayed
        if (displayText)
        {
            textBox.enabled = true;
            dialogueText.enabled = true;

            //lets players skip text
            if (Input.GetButtonDown("Fire1")) {
                //skips text
                if (GetIsTyping())
                {
                    dialogueText.text = fullText;
                    StopCoroutine("TypeText");
                    
                }
                else 
                {//closes text box
                    displayText = false;
                }
            }
            //called to type text
            if (startTyping)
            {
                StartCoroutine("TypeText");
                startTyping = false;
            }

        }
        else {//this runs if no text is being typed or displayed
            dialogueText.enabled = false;
            textBox.enabled = false;
        }
    }

    //used to type text
    IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            dialogueText.text += letter;
            {
                FMODUnity.RuntimeManager.PlayOneShot(inputsound);
                yield return 0;
            }
            yield return new WaitForSeconds(letterPause);
        }
    }

    public bool GetIsTyping()
    {
        return !dialogueText.text.Equals(fullText);
    }
}