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


        remainingKeys.Add(KeyCode.Escape);
        remainingKeys.Add(KeyCode.F1);
        remainingKeys.Add(KeyCode.F2);
        remainingKeys.Add(KeyCode.F3);
        remainingKeys.Add(KeyCode.F4);
        remainingKeys.Add(KeyCode.F5);
        remainingKeys.Add(KeyCode.F6);
        remainingKeys.Add(KeyCode.F7);
        remainingKeys.Add(KeyCode.F8);
        remainingKeys.Add(KeyCode.F9);
        remainingKeys.Add(KeyCode.F10);
        remainingKeys.Add(KeyCode.F11);
        remainingKeys.Add(KeyCode.F12);
        remainingKeys.Add(KeyCode.Alpha0);
        remainingKeys.Add(KeyCode.Alpha1);
        remainingKeys.Add(KeyCode.Alpha2);
        remainingKeys.Add(KeyCode.Alpha3);
        remainingKeys.Add(KeyCode.Alpha4);
        remainingKeys.Add(KeyCode.Alpha5);
        remainingKeys.Add(KeyCode.Alpha6);
        remainingKeys.Add(KeyCode.Alpha7);
        remainingKeys.Add(KeyCode.Alpha8);
        remainingKeys.Add(KeyCode.Alpha9);
        remainingKeys.Add(KeyCode.Q);
        remainingKeys.Add(KeyCode.W);
        remainingKeys.Add(KeyCode.E);
        remainingKeys.Add(KeyCode.R);
        remainingKeys.Add(KeyCode.T);
        remainingKeys.Add(KeyCode.Y);
        remainingKeys.Add(KeyCode.U);
        remainingKeys.Add(KeyCode.I);
        remainingKeys.Add(KeyCode.O);
        remainingKeys.Add(KeyCode.P);
        remainingKeys.Add(KeyCode.A);
        remainingKeys.Add(KeyCode.S);
        remainingKeys.Add(KeyCode.D);
        remainingKeys.Add(KeyCode.F);
        remainingKeys.Add(KeyCode.G);
        remainingKeys.Add(KeyCode.H);
        remainingKeys.Add(KeyCode.J);
        remainingKeys.Add(KeyCode.K);
        remainingKeys.Add(KeyCode.L);
        remainingKeys.Add(KeyCode.Z);
        remainingKeys.Add(KeyCode.X);
        remainingKeys.Add(KeyCode.C);
        remainingKeys.Add(KeyCode.V);
        remainingKeys.Add(KeyCode.B);
        remainingKeys.Add(KeyCode.N);
        remainingKeys.Add(KeyCode.M);
        remainingKeys.Add(KeyCode.Comma);
        remainingKeys.Add(KeyCode.Period);
        remainingKeys.Add(KeyCode.Minus);
        remainingKeys.Add(KeyCode.Space);
        remainingKeys.Add(KeyCode.LeftArrow);
        remainingKeys.Add(KeyCode.UpArrow);
        remainingKeys.Add(KeyCode.DownArrow);
        remainingKeys.Add(KeyCode.RightArrow);
        remainingKeys.Add(KeyCode.Keypad0);
        remainingKeys.Add(KeyCode.Keypad1);
        remainingKeys.Add(KeyCode.Keypad2);
        remainingKeys.Add(KeyCode.Keypad3);
        remainingKeys.Add(KeyCode.Keypad4);
        remainingKeys.Add(KeyCode.Keypad5);
        remainingKeys.Add(KeyCode.Keypad6);
        remainingKeys.Add(KeyCode.Keypad7);
        remainingKeys.Add(KeyCode.Keypad8);
        remainingKeys.Add(KeyCode.Keypad9);
        remainingKeys.Add(KeyCode.KeypadDivide);
        remainingKeys.Add(KeyCode.KeypadEnter);
        remainingKeys.Add(KeyCode.KeypadMinus);
        remainingKeys.Add(KeyCode.KeypadPlus);
        remainingKeys.Add(KeyCode.KeypadMultiply);
        remainingKeys.Add(KeyCode.Backspace);
        remainingKeys.Add(KeyCode.Return);
        remainingKeys.Add(KeyCode.PageDown);
        remainingKeys.Add(KeyCode.PageUp);
        remainingKeys.Add(KeyCode.Home);
        remainingKeys.Add(KeyCode.Delete);
        remainingKeys.Add(KeyCode.Mouse0);
        remainingKeys.Add(KeyCode.Mouse1);
        remainingKeys.Add(KeyCode.Mouse2);
        remainingKeys.Add(KeyCode.Tab);

    }

    // Use this for initialization
    void Start()
    {
        playerAmount = UIManager.playerCount;
        StartCoroutine("StartTimer", 3);
        
    }

    IEnumerator StartTimer(int seconds)
    {
        isStartTimerRunning = true;
        isTimerRunning = false;
        players = new SimpleCarController[playerAmount];
        startTextAnimator = startTimerText.GetComponent<Animator>();

        for (int i = 0; i < playerAmount; i++)
        {
            GameObject tempBoat = (GameObject)Instantiate(boatPrefab, new Vector3(boatPrefab.transform.position.x + (i * 5), boatPrefab.transform.position.y, boatPrefab.transform.position.z), Quaternion.identity);
            tempBoat.name = "Player " + (i + 1);
            players[i] = tempBoat.GetComponent<SimpleCarController>();
            players[i].ShuffleKeys();
            players[i].enabled = false;
            Text tempText = Instantiate(textPrefab, uICanvas.transform);
            tempText.text = "P" + (i + 1) + ": " + players[i].leftKey.ToString() + " || " + players[i].rightKey.ToString();
            tempText.transform.localScale = new Vector3(tempText.transform.localScale.x * 0.75f, tempText.transform.localScale.y * 0.75f, tempText.transform.localScale.z * 0.75f);
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
            textList[i].text = "P" + (i + 1) + ": " + players[i].leftKey.ToString() + " || " + players[i].rightKey.ToString();
        }

        isTimerRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimerRunning && !isStartTimerRunning)
        {
            StartCoroutine("TimerForShuffle", 30);
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
}
