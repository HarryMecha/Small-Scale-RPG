using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class HashingAlgorithm : MonoBehaviour
{
    private static MD5 md5; //This variables holds the MD5 algorithm used to convert the input string.
    private static byte[] inputBytes; //This variable holds the users input converted into a byte array.
    private static byte[] hashBytes; //This variable holds the user input converted into a byte array and then hashed each value.
    //The CalcMD5 method takes the users password Input and converts it into an MD5 Hash so that it can be put into the Users table in my database.
    public static string CalcMD5(string input) //This requires the input from thr InputField to convert.
    {
        StringBuilder StringBuilder = new StringBuilder(); //Creates a new StringBuilder
        md5 = System.Security.Cryptography.MD5.Create(); //Creates a new MD5 object
        inputBytes = System.Text.Encoding.ASCII.GetBytes(input); //Converts string to bytes
        hashBytes = md5.ComputeHash(inputBytes); //runs the hashing algorithm on the hashed bytes
        for (int i = 0; i < hashBytes.Length; i++)
        {
            StringBuilder.Append(hashBytes[i].ToString("X2")); //Converts the byte array into hex string
        }
        return StringBuilder.ToString(); //Converts the hex to string
    }
}
