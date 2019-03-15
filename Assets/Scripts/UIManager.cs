using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
    
public class UIManager : MonoBehaviour 
{

    public static UIManager s_instance; // Singleton

    //ending display
    public float endTime;
    public Image loseImage;
    public GameObject restartButton;
    public GameObject creditsButton;


    //countdown
    public Text countdown;
    public bool raceStarted;
    public bool userControl;
    public float gameStartTime;
    public float timePassed;

    //positioning
    public Transform player;
    public Transform Car2;
    public Transform Car3;
    public Transform Car4;
    public Text position;
    public bool playerLast = false;

    //Speedometer
    public Text speedometer;
    private CharacterController playerCharacterController;

    //timer
    public Text timer;

    //"You Lose!" SFX
    [FMODUnity.EventRef]
    public string inputsound;

    private UnityEngine.Object music;

    //wrong way
    public Transform playerCamera;
    public Image wrongway;

    public float gameoverLapTimeFlashPause;

    void Awake()
    {
        s_instance = this;
    }

    //start
    void Start() 
    {
        gameStartTime = Time.time;
        endTime = 180.0f + UnityEngine.Random.Range(1.0f, 45.0f);
        loseImage.enabled = false;
        restartButton.SetActive(false);
        creditsButton.SetActive(false);

        wrongway.enabled = false;

        playerCharacterController = player.GetComponentInChildren<CharacterController>();
    }

    //counts down
    void CountDown() 
    {

        //cycle countdown
        if (timePassed <= 1.0)
        {
            countdown.text = "3";
            //play sound #####################################################################
        }
        else if (timePassed <= 2.0)
        {
            countdown.text = "2";
            //play sound #####################################################################
        }
        else if (timePassed <= 3.0)
        {
            countdown.text = "1";
            //play sound #####################################################################
        }
        else if (timePassed <= 4.0)
        {
            countdown.text = "GO!";
            //play sound #####################################################################
            raceStarted = true;
            userControl = true;

            // Enable Player Movement - This functionality should be moved to the GameManager
            player.GetComponentInChildren<PlayerController>().enabled = true;

        }
        else
        {
            countdown.enabled = false;
        }

    }

    //formats time display
    string GetFormattedTime(float time)
    {

        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeSpan.Minutes,
        timeSpan.Seconds,
        timeSpan.Milliseconds);

        if (formattedTime.Length == 9)
        {
            formattedTime = formattedTime.Remove(formattedTime.Length - 1);
        }

        return formattedTime;
    }

    bool playLostAudio = false;
   
    //displays loss image
    void ShowLoss() 
    {
        loseImage.enabled = true;
        //restartButton.SetActive(true);
        creditsButton.SetActive(true);

        // Disable Player Movement
        player.GetComponentInChildren<PlayerController>().enabled = false;
        Cursor.visible = true; // show cursor

        playLostAudio = true;
        {
            if (playLostAudio == true)
            {
                FMODUnity.RuntimeManager.PlayOneShot(inputsound);
                playLostAudio = false;
                music = GameObject.Find("/Music");
                Destroy(music);
            }
        }

    }

    //determines the player's position in relation to remaining cars
    void DeterminePosition() 
    {

        if ((player.position.z < Car4.position.z)||playerLast)
        {
            position.text = "4th";
            playerLast = true;
        }
        else if (player.position.z < Car3.position.z)
        {
            position.text = "3rd";
        }
        else if (player.position.z < Car2.position.z)
        {
            position.text = "2nd";
        }
        else {
            position.text = "1st";
        }
    }

    // Update is called once per frame
    void Update () 
    {
        timePassed = Time.time - gameStartTime;

        //updates countdown
        CountDown();

        //updates timer
        if (raceStarted) 
        {
            timer.text = GetFormattedTime(timePassed - 3.0f);
        }
        if (timePassed > endTime) 
        {
            ShowLoss();

            // ensures only called once
            if (userControl)
            {
                StopCoroutine(FlashTime());
                StartCoroutine(FlashTime());
            }

            userControl = false;
            raceStarted = false;
        } else
        {
            //updates position relative to cars
            DeterminePosition();
        }

        //wrong way
        if ((playerCamera.rotation.y > 0.75f || playerCamera.rotation.y < -0.75f) && raceStarted)
        {
            wrongway.enabled = true;
        }
        else 
        {
            wrongway.enabled = false;
        }



        print(SceneManager.GetActiveScene().buildIndex);

        //endgame input
        if (!raceStarted) {
            if (Input.GetKey(KeyCode.X))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Input.GetKey(KeyCode.Y))
            {
                SceneManager.LoadScene(1);
            }
        }

        

        //updates speedometer
        Vector3 horizontalVelocity = new Vector3(playerCharacterController.velocity.x,
                                                 0, playerCharacterController.velocity.z);
        string speed = String.Format("{0} KMH", (int)horizontalVelocity.magnitude);

        speedometer.text = speed;

        //check if player wants to quit game
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

	//Flashing coroutine
    IEnumerator FlashTime()
    {
        string lapTime = timer.text;
        while (true)
        {
            if (timer.text != "")
            {
                timer.text = "";
                position.text = "";
            } else
            {
                timer.text = lapTime;
                position.text = "4th";
            }
            yield return new WaitForSeconds(gameoverLapTimeFlashPause);
        }
    }
}
