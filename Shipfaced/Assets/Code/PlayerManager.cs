using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int playerAmount;
    public GameObject boatPrefab;
    public SimpleCarController[] players;
    public static PlayerManager instance;
    public GameObject uICanvas;
    public Text textPrefab;
    public Animator startTextAnimator;
    public Animator shuffleTextAnimator;

    //List for the players' controls text.
    public List<Text> textList;
    bool isTimerRunning;
    bool isStartTimerRunning;
    [SerializeField]
    Text startTimerText;
    [SerializeField]
    Text shuffleTimerText;
    public List<KeyCode> remainingKeys = new List<KeyCode>();

    //Power Up Related
    public int playerSkip;
    public bool skip = false;

    public List<int> playerPositions = new List<int>();
    public List<int> nextCheckpoint = new List<int>();
    public List<GameObject> checkpoints = new List<GameObject>();


    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        playerAmount = UIManager.playerCount;
        remainingKeys.Clear();
        foreach (KeyCode key in GameObject.Find("GlobalGameObject").GetComponent<GlobalGameObject>().keys)
        {
            remainingKeys.Add(key);
        }
        StartCoroutine("StartTimer", 3);

    }

    //Method to start the timer at the beginning of the game, also instantiates the boats.
    IEnumerator StartTimer(int seconds)
    {
        isStartTimerRunning = true;
        isTimerRunning = false;
        players = new SimpleCarController[playerAmount];
        startTextAnimator = startTimerText.GetComponent<Animator>();

        for (int i = 0; i < playerAmount; i++)
        {
            GameObject tempBoat = (GameObject)Instantiate(boatPrefab, new Vector3(boatPrefab.transform.position.x + ((i % 5) * 4f), boatPrefab.transform.position.y, (boatPrefab.transform.position.z - 5 * (i / 5) + 10 * (i / 5))), Quaternion.identity);
            tempBoat.GetComponent<SimpleCarController>().nameOfCar = "Player " + (i + 1);
            players[i] = tempBoat.GetComponent<SimpleCarController>();
            players[i].playerColor = RandomizeColor();
            players[i].ShuffleKeys();
            players[i].enabled = false;
            Text tempText = Instantiate(textPrefab, uICanvas.transform);
            tempText.text = PlayerControlsString(i, players[i].leftKey, players[i].rightKey);
            tempText.transform.localScale = new Vector3(tempText.transform.localScale.x * 0.75f, tempText.transform.localScale.y * 0.75f, tempText.transform.localScale.z * 0.75f);
            tempText.GetComponent<Text>().color = players[i].playerColor;
            textList.Add(tempText);
            nextCheckpoint.Add(0);

        }

        startTimerText.gameObject.SetActive(true);

        for (int i = seconds; i > 0; i--)
        {
            startTimerText.text = i.ToString();
            startTextAnimator.Play("StartTextAnim", -1);
            yield return new WaitForSeconds(1);

        }

        startTimerText.gameObject.SetActive(false);

        foreach (SimpleCarController boat in players)
        {
            boat.enabled = true;
        }
        isStartTimerRunning = false;
    }

    //Method to shuffle the controls at a regular interval.
    IEnumerator TimerForShuffle(float seconds)
    {
        shuffleTextAnimator.SetBool("timerIsShown", false);
        shuffleTimerText.gameObject.SetActive(true);
        isTimerRunning = true;
        float timeBetweenShuffle = seconds;

        while (timeBetweenShuffle > 0)
        {
            timeBetweenShuffle -= Time.deltaTime;
            shuffleTimerText.text = "Control shuffle in: " + (((int)(timeBetweenShuffle)) + 1).ToString();
            if (!shuffleTextAnimator.GetBool("timerIsShown") && timeBetweenShuffle <= 5)
            {
                shuffleTextAnimator.SetBool("timerIsShown", true);
            }
            yield return null;
        }

        for (int i = 0; i < players.Length; i++)
        {
            if (skip && i == playerSkip)
            {
                skip = false;
                continue;
            }
            players[i].ShuffleKeys();
            textList[i].text = PlayerControlsString(i, players[i].leftKey, players[i].rightKey);
        }

        isTimerRunning = false;
    }


    Color RandomizeColor()
    {
        List<int> colors = new List<int>();
        colors.Add(0);
        colors.Add(1);
        colors.Add(2);

        Color randomCol = new Color();
        int randomNumber = Random.Range(0, colors.Count);

        randomCol[colors[randomNumber]] = 1;
        colors.RemoveAt(randomNumber);

        randomCol[colors[Random.Range(0, colors.Count)]] = Random.Range(0, 1f);
        randomCol.a = 1;
        return randomCol;
    }

    // Update is called once per frame

    void Update()
    {
        //Start the timer for the shuffling of controls, if it is not already running and the starttimer has ceased running.
        if (!isTimerRunning && !isStartTimerRunning)
        {
            StartCoroutine("TimerForShuffle", 20);
        }
    }


    //Method for Player Positions
    public void CheckPositions()
    {
        playerPositions.Clear();

        bool check = false;

        for (int i = 0; i < players.Length; i++)
        {
            if (i == 0)
            {
                playerPositions.Add(i);
            }
            else
            {
                int j;
                for (j = 0; j < playerPositions.Count; j++)
                {
                    if (nextCheckpoint[playerPositions[j]] < nextCheckpoint[i])
                    {
                        check = true;
                        break;
                    }
                    else if (nextCheckpoint[playerPositions[j]] == nextCheckpoint[i])
                    {
                        if (Vector3.Distance(checkpoints[nextCheckpoint[playerPositions[j]]].transform.position, players[j].transform.position) < Vector3.Distance(checkpoints[nextCheckpoint[i]].transform.position, players[i].transform.position))
                        {
                            check = true;
                            break;
                        }
                    }
                }
                if (check)
                {
                    playerPositions.Insert(j, i);
                }
                else
                {
                    playerPositions.Add(i);
                }
            }
        }
    }

    public string PlayerControlsString(int player, KeyCode left, KeyCode right)
    {
        string completeString;

        completeString = "P" + (player + 1) + ": ";
        switch (left)
        {
            case KeyCode.Alpha0:
                completeString += "0 || ";
                break;
            case KeyCode.Alpha1:
                completeString += "1 || ";
                break;
            case KeyCode.Alpha2:
                completeString += "2 || ";
                break;
            case KeyCode.Alpha3:
                completeString += "3 || ";
                break;
            case KeyCode.Alpha4:
                completeString += "4 || ";
                break;
            case KeyCode.Alpha5:
                completeString += "5 || ";
                break;
            case KeyCode.Alpha6:
                completeString += "6 || ";
                break;
            case KeyCode.Alpha7:
                completeString += "7 || ";
                break;
            case KeyCode.Alpha8:
                completeString += "8 || ";
                break;
            case KeyCode.Alpha9:
                completeString += "9 || ";
                break;
            case KeyCode.Keypad0:
                completeString += "Keypad 0 || ";
                break;
            case KeyCode.Keypad1:
                completeString += "Keypad 1 || ";
                break;
            case KeyCode.Keypad2:
                completeString += "Keypad 2 || ";
                break;
            case KeyCode.Keypad3:
                completeString += "Keypad 3 || ";
                break;
            case KeyCode.Keypad4:
                completeString += "Keypad 4 || ";
                break;
            case KeyCode.Keypad5:
                completeString += "Keypad 5 || ";
                break;
            case KeyCode.Keypad6:
                completeString += "Keypad 6 || ";
                break;
            case KeyCode.Keypad7:
                completeString += "Keypad 7 || ";
                break;
            case KeyCode.Keypad8:
                completeString += "Keypad 8 || ";
                break;
            case KeyCode.Keypad9:
                completeString += "Keypad 9 || ";
                break;
            case KeyCode.Minus:
                completeString += "- || ";
                break;
            case KeyCode.Slash:
                completeString += "' || ";
                break;
            case KeyCode.Semicolon:
                completeString += "¨ || ";
                break;
            case KeyCode.LeftBracket:
                completeString += "´ || ";
                break;
            case KeyCode.Backslash:
                completeString += "§ || ";
                break;
            case KeyCode.RightBracket:
                completeString += "Å || ";
                break;
            case KeyCode.BackQuote:
                completeString += "Ö || ";
                break;
            case KeyCode.Quote:
                completeString += "Ä || ";
                break;
            case KeyCode.LeftCommand:
                completeString += "Windows || ";
                break;
            case KeyCode.Mouse0:
                completeString += "Left Click || ";
                break;
            case KeyCode.Mouse1:
                completeString += "Right Click || ";
                break;
            case KeyCode.Mouse2:
                completeString += "Middle Mouse Button || ";
                break;
            default:
                completeString += left.ToString() + " || ";
                break;
        }
        switch (right)
        {
            case KeyCode.Alpha0:
                completeString += "0";
                break;
            case KeyCode.Alpha1:
                completeString += "1";
                break;
            case KeyCode.Alpha2:
                completeString += "2";
                break;
            case KeyCode.Alpha3:
                completeString += "3";
                break;
            case KeyCode.Alpha4:
                completeString += "4";
                break;
            case KeyCode.Alpha5:
                completeString += "5";
                break;
            case KeyCode.Alpha6:
                completeString += "6";
                break;
            case KeyCode.Alpha7:
                completeString += "7";
                break;
            case KeyCode.Alpha8:
                completeString += "8";
                break;
            case KeyCode.Alpha9:
                completeString += "9";
                break;
            case KeyCode.Keypad0:
                completeString += "Keypad 0";
                break;
            case KeyCode.Keypad1:
                completeString += "Keypad 1";
                break;
            case KeyCode.Keypad2:
                completeString += "Keypad 2";
                break;
            case KeyCode.Keypad3:
                completeString += "Keypad 3";
                break;
            case KeyCode.Keypad4:
                completeString += "Keypad 4";
                break;
            case KeyCode.Keypad5:
                completeString += "Keypad 5";
                break;
            case KeyCode.Keypad6:
                completeString += "Keypad 6";
                break;
            case KeyCode.Keypad7:
                completeString += "Keypad 7";
                break;
            case KeyCode.Keypad8:
                completeString += "Keypad 8";
                break;
            case KeyCode.Keypad9:
                completeString += "Keypad 9";
                break;
            case KeyCode.Minus:
                completeString += "Hyphen";
                break;
            case KeyCode.Slash:
                completeString += "'";
                break;
            case KeyCode.Semicolon:
                completeString += "¨";
                break;
            case KeyCode.LeftBracket:
                completeString += "´";
                break;
            case KeyCode.Backslash:
                completeString += "§";
                break;
            case KeyCode.RightBracket:
                completeString += "Å";
                break;
            case KeyCode.BackQuote:
                completeString += "Ö";
                break;
            case KeyCode.Quote:
                completeString += "Ä";
                break;
            case KeyCode.LeftCommand:
                completeString += "Windows";
                break;
            case KeyCode.Mouse0:
                completeString += "Left Click";
                break;
            case KeyCode.Mouse1:
                completeString += "Right Click";
                break;
            case KeyCode.Mouse2:
                completeString += "Middle Mouse Button";
                break;
            default:
                completeString += right.ToString();
                break;
        }
        return completeString;
    }
}
