using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class EmailValidation : MonoBehaviour
{
    public static InputField EmailField; //This is is a variable that will hold the InputField the user will input their email into.
    public static bool EmailGood; //This is a boolean that wil be used to determine whether the email fits the regular expression or not.
    public static string email; //This is a variable that will contain the email that is entered into the InputField so it can be compared to the regular expression.
    //The emailchecker method will take the email inputted by the user into the InputField and run it against the regular expression to see if it's valid or not.
    public static void emailchecker()
    {
        EmailGood = false; //This will set the EmailGood variable to false as the input has yet to be checked.
        EmailField = GameObject.Find("NewEmailField").GetComponent<InputField>(); //Finds the InputField where email input will be typed.
        email = EmailField.text; //Sets the text that was typed into the InputField
        Regex regex = new Regex(@"^((\w)+)(@)((\w)+)(.)(((\w){2,4})+)$"); //This is a regular expression A word followed by an @ followed by a word followed by a . followed by a word with 2-4 characters
        Match match = regex.Match(email); //Sets up a new match object
        if (match.Success) //Shows if the match is successful
        {
            EmailGood = true; //This will set the EmailGood boolean to true showing the input is valid when compared to the regular expression.
        }
        else
        {
            EmailGood = false; //This will set the EmailGood boolean to false showing the input is valid when compared to the regular expression.
        }
    }
}