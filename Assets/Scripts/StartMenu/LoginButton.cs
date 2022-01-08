using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoginButton : MonoBehaviour
{
    public Button button; //This variable holds the button component of the button GameObject that the user uses to confirm their decision to login.
    private GameObject buttonGo; //This variable holds the button GameObject that the user uses to confirm their decision to login.
    public int counter; //This variable holds an integer that increases everytime the button is pressed in order to set a limit and timeout the user if too many invalid tries are made.
    public bool TimerFinished; //This is a boolean the determines whether the timer is active or not that will let the user try to log in again.
    public string HashedPassword; //This is the HashedPassword that will be obtained from the CalcMD5 method in the HashingAlgorithm script.
    public static InputField EmailField; //This contains the InputFieild the user will input the email into.
    public static InputField PassField; //This contains the InputFieild the user will input the password into.
    public static string PasswordCheck; //This contains PasswordCheck a string that is retrieved from the HashedPassword Column in the Users table of the database by the UserChecker method in the DatabaseManager script.
    private void Start()
    {
        counter = 0; //This set the counter to 0 at start as no attempts to login have been made.
        TimerFinished = false; //This sets the TimerFinished to false as the timer has not been started on start.
        buttonGo = GameObject.Find("PlayButton"); //This assigns the buttonGo to the GameObject PlayButton.
        EmailField = GameObject.Find("EmailField").GetComponent<InputField>(); //This assigns the EmailField variable to the InputField component attached to the EmailField GameObject.
        PassField = GameObject.Find("PassField").GetComponent<InputField>(); //This assigns the PassField variable to the InputField component attached to the PassField GameObject.
        button = buttonGo.GetComponent<Button>(); //This assigns the button variable to the Button component attached to the PlayButton GameObject.
        button.onClick.AddListener(buttonclick); //This adds a listener to the button variable that will run the buttonclick method when the user clicks on the PlayButton GameObject with their mouse.
    }
    //The buttonclick script is used to perform the check between the users inputs in the InputFields and the values in the Users table of the database.
    public void buttonclick()
    {
        buttoncounter(); //This runs the button counter script that either adds 1 to the counter if the counter is less than 5 or stops the user from trying to log in if it's over 5 by starting a timer.
        if (counter < 5) //This if statement only runs while the counter is less than 5 showing that the user is not on their time out.
        {
            string Email = EmailField.text; //This creates a string and assigns it's value to the Text component attached to the EmailField.
            string Pass = PassField.text; //This creates a string and assigns it's value to the Text component attached to the PassField.
            HashedPassword = HashingAlgorithm.CalcMD5(Pass); //This will assign the value to the user inputted password run through the MD5 Hashing algorithm.
            DatabaseManager.UserChecker(Email, HashedPassword); //This runs the Userchecker method in the DatabaseManager sript to see if the HashedPassword and the email match the record in the Usser Table of the database.
            DatabaseManager.GetUserID(Email); //This runs the GetUserID method found in the DatabaseManager using the input Email to retrieve and assign the ID to be used in other scripts.
            if (PasswordCheck == HashedPassword) //This if statement only runs whilst the HashedPassword from the database is the same as the HashedPassword created by the users input.
            {
                SceneManager.LoadScene("City"); //This will load the City scene showing the user their input was correct.
            }
            else
            {
                Debug.Log("User Not Found"); //This will show the user their input was incorrect.
            }
        }
        else { }
    }
    //The buttoncounter method is used to count how many times the user has incorrectly clicked the ButtonGameObject and penalise them after too many incorrect clicks by making them wait 30 seconds.
    public void buttoncounter()
    {
        if (counter < 5)//This if statement only runs while the counter is less than 5 showing that the user is not on their time out.
        {
            counter++; //This adds one to the counter.
        }
        else
        {
            Debug.Log("Too many incorrect attempts, wait 30 seconds"); //This shows the user they have inputted an incorrect login too many times and will need to wait 30 seconds.
            StartCoroutine(TimerWait()); //This will start a coroutine that will wait 30 and will then change the value of the boolean TimerFinished to true that will allow this script to continue.
            if (TimerFinished == true) //This if statement will only run when the timer has finished giving the user the ability to try to login again.
            {
                counter = 0; //This sets the counter to 0 giving the user 5 more tries.
                TimerFinished = false; //This will set the TimerFinished back to false so that the if statement won't loop.
            }
        }
    }
    //This timer wait method will wait 30 seconds not allowing the user to try to login in that time period.
    IEnumerator TimerWait()
    {
        yield return new WaitForSeconds(30); //This will wait for 30 before continuing any other lines of code in the method.
        TimerFinished = true; //The TimerFinished is set to true so the timer has finished giving the user the ability to try to login again.
        StopCoroutine(TimerWait()); //This will stop this coroutine from running.
    }
}
