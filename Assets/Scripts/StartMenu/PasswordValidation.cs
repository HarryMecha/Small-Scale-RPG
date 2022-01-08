using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class PasswordValidation : MonoBehaviour {

    public static InputField PasswordField; //This contains the InputFieild the user will input the password into.
    public static InputField ConfirmPasswordField; //This contains the InputFieild the user will input the password into again to confirm it.
    public static bool PasswordGood; //This is a boolean that determines whether is Password is valid when it's run through the regular expression.
    public static string password; //This variable holds the users input into the PasswordInputField.
    //The passwordchecker script checks the password input against the confirm password input and the regular expression to see if it's valid when creating an account.
    public static void passwordchecker()
    {
        PasswordGood = false; //The PasswordGood is set to false as no attempt to check the password has been made
        PasswordField = GameObject.Find("NewPassField").GetComponent<InputField>(); //Finds the InputField where the password input will be types.
        ConfirmPasswordField = GameObject.Find("ConfirmPassField").GetComponent<InputField>(); //Finds the InputField where the password input will be retyped.
        password = PasswordField.text; //Sets the text that was typed into the InputField.
        string confirmpassword = ConfirmPasswordField.text; //Sets the text that was typed into the InputField.
        Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,16}$"); //A regular expression the input password is tested against that requires 1+ number, 1+ lowercase and 1+ uppercase, 8 to 255 characters long.
        Match match = regex.Match(password); //Sets up a new match object.
        if (match.Success) //Shows if the match is successful.
        {
            if (confirmpassword == password) //Confirms if both passwords are the same.
            {
                PasswordGood = true; //This changes the boolean to true to indicate the passoword inputted matches the confirmpassword and fits into the regular expression.
            }
        }
        else //Shows if the match is unsuccessful
        {
            PasswordGood = false; //This changes the boolean to false to indicate the passoword inputted does not matches the confirmpassword and does not fit into the regular expression.
        }
    }
}
