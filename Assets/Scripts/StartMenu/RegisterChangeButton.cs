using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RegisterChangeButton : MonoBehaviour
{
    public GameObject LoginScreen; //This variable holds the LoginCanvas GameObject.
    public GameObject RegisterScreen; //This variable holds the RegisterCanvas GameObject.
    public Button button; //This variable holds the button component of the button GameObject that the user uses to confirm their decision to chnage to register.
    private GameObject buttonGo; //This variable holds the button GameObject that the user uses to confirm their decision to change to register.
    void Start()
    {
        LoginScreen = GameObject.Find("LoginCanvas"); //This assigns LoginScreen to the LoginCanvas allowing the switch between this and the RegisterCanvas.
        RegisterScreen = GameObject.Find("RegisterCanvas"); //This assigns RegisterScreen to the RegisterCanvas allowing the switch between this and the LoginCanvas.
        LoginScreen.SetActive(true); //This will set the LoginCanvas GameObject to active so it will be visible to the user and scripts attached to it will run.
        RegisterScreen.SetActive(false); //This will set the RegisterCanvas GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        buttonGo = GameObject.Find("RegisterChangeButton");  //This assigns the button variable to the RegisterChangeButton GameObject.
        button = buttonGo.GetComponent<Button>();  //This assigns the button variable to the Button component attached to the RegisterChangeButton GameObject.
        button.onClick.AddListener(buttonclick); //This adds a listener to the button variable that will run the buttonclick method when the user clicks on the RegisterChangeButton GameObject with their mouse.
    }
    //The buttonclick script is used to switch between the LoginCanvas and the RegisterCanvas when the user clicks the button.
    public void buttonclick()
    {
        LoginScreen.SetActive(false); //This will set the LoginCanvas GameObject to inactive so it will be invisible to the user and no scripts attached to it will run.
        RegisterScreen.SetActive(true); //This will set the RegisterCanvas GameObject to active so it will be visible to the user and scripts attached to it will run.
    }
}