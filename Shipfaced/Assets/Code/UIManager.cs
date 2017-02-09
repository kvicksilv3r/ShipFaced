using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Animator playButtonAnimator;
    public Animator optionsButtonAnimator;
    public Animator quitButtonAnimator;
    public Animator inputFieldAnimator;
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;
    int selectedButton;
    public Text inputFieldText;
    Animator[] buttonAnimators;
    Button[] buttons;
    int tempIndex;
    public static int playerCount;


    //Method to set the player count for the game.
    //If <2 is given, the player count is set to 2.
    //If > 20 is given, the player count is set to 20.
    public void SetPlayerCount()
    {
        if (int.Parse(inputFieldText.text) <= 20 && int.Parse(inputFieldText.text) >= 2 )
        {
            playerCount = int.Parse(inputFieldText.text);
        }

        else if(int.Parse(inputFieldText.text) < 2)
        {
            playerCount = 2;
        }

        else
        {
            playerCount = 20;
        }
        SceneManager.LoadScene("ShipFaced");
    }


    //Method to decide what to happen when a button is pressed
    public void PressButton(int indexOfButton)
    {
        foreach (Animator b in buttonAnimators)
        {
            b.SetBool("isPressed", true);
        }

        switch (indexOfButton)
        {
            case 0:
                inputFieldAnimator.SetBool("isPlayPressed", true);
                foreach (Button b in buttons)
                {
                    b.enabled = false;
                }
                return;
            case 1:
                return;
            case 2:
                Application.Quit();
                return;
        }



    }

    public void SelectButton(int indexOfButton)
    {
        if (selectedButton != indexOfButton)
        {
            buttons[selectedButton].transform.GetChild(1).gameObject.SetActive(false);
            buttonAnimators[selectedButton].SetBool("isSelected", false);
            selectedButton = indexOfButton;
            buttons[selectedButton].Select();
            buttonAnimators[selectedButton].SetBool("isSelected", true);
            buttons[selectedButton].transform.GetChild(1).gameObject.SetActive(true);
        }

        else
        {
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
        buttonAnimators = new Animator[] { playButtonAnimator, optionsButtonAnimator, quitButtonAnimator };
        buttons = new Button[] { playButton, optionsButton, quitButton };
        selectedButton = 0;
        buttons[selectedButton].Select();
        buttonAnimators[selectedButton].SetBool("isSelected", true);
        buttons[selectedButton].transform.GetChild(1).gameObject.SetActive(true);
        tempIndex = selectedButton;
    }

    void Update()
    {
        //Navigate down on the main menu with the down-arrow
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectedButton < 2)
            {
                tempIndex++;
                SelectButton(tempIndex);
            }

            else
            {
                tempIndex = 0;
                SelectButton(tempIndex);
            }
        }

        //Navigate up on the main menu with the up-arrow
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedButton > 0)
            {
                tempIndex--;
                SelectButton(tempIndex);
            }
            else
            {
                tempIndex = 2;
                SelectButton(tempIndex);
            }
        }
    }
}
