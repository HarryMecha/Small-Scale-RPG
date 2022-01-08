using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RegisterButton : MonoBehaviour
{
    public GameObject LoginScreen; //This variable holds the LoginCanvas GameObject.
    public GameObject RegisterScreen; //This variable holds the RegisterCanvas GameObject.
    public Button button; //This variable holds the button component of the button GameObject that the user uses to confirm their decision to register.
    private GameObject buttonGo; //This variable holds the button GameObject that the user uses to confirm their decision to register.
    public string HashedPassword; //This is the HashedPassword that will be obtained from the CalcMD5 method in the HashingAlgorithm script.
    public static bool RegisterSuccess;
    private void Start()
    {
        LoginScreen = GameObject.Find("LoginCanvas"); //This assigns LoginScreen to the LoginCanvas allowing the switch between this and the RegisterCanvas.
        RegisterScreen = GameObject.Find("RegisterCanvas"); //This assigns RegisterScreen to the RegisterCanvas allowing the switch between this and the LoginCanvas.
        buttonGo = GameObject.Find("RegisterButton");  //This assigns the buttonGo to the GameObject RegisterButton.
        button = buttonGo.GetComponent<Button>();  //This assigns the button variable to the Button component attached to the RegisterButton GameObject.
        button.onClick.AddListener(buttonclick); //This adds a listener to the button variable that will run the buttonclick method when the user clicks on the RegisterButton GameObject with their mouse.
    }
    //The buttonclick script is used to perform the check between the users inputs in the InputFields and the values in the Users table of the database.
    public void buttonclick()
    {
        EmailValidation.emailchecker(); //This runs the emailchecker method in the EmailValidation script to check is the email the user inputted into the InputField is valid in comparion to the regular expression.
        PasswordValidation.passwordchecker(); //This runs the passwordchecker method in the EmailValidation script to check is the password the user inputted into the InputField is valid in comparion to the regular expression.
        if ((PasswordValidation.PasswordGood == true) && (EmailValidation.EmailGood == true)) //This if statement only runs when both the PasswordGood and EmailGood booleans are set to true showing that both the input username and password are valid.
        {
            HashedPassword = HashingAlgorithm.CalcMD5(PasswordValidation.password); //This assign the HashedPassword to the input password run through the CalcMD5 script in the HashingAlgorithm script.
            DatabaseManager.InsertUser(EmailValidation.email, HashedPassword); //This will run the InsertUser script in the DatabaseManager which will create a new record in the User table with the input email and password run through the hashing algorithm.
            LoginScreen.SetActive(true); //This will then set the LoginScreen to true giving the user a visual representation that registration was successful and they can now login whilst making the LoginCanvas visible.
            RegisterScreen.SetActive(false); //This will then set the RegisterScreen to false giving the user a visual representation that registration was successful and they can now login whilst making the RegisterCanvas invisible.
        }
    }
}
