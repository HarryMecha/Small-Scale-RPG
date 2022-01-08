using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEditor;
public class DatabaseManager : MonoBehaviour
{
    private static string connectionString; //This is a string containing the sile location.
    private static string UserEmail; //This is the User's Email which is stored for further comparisons when loading and saving data.
    private static int ID; //This is the User's ID as defined in the SQL Table which is stored for further comparisons when loading and saving data.
    private static string[] p_list = new string[5]; //This is an array contaning the names of collumns used to help simplify when searching for specific columns.
    // Use this for initialization
    void Start()
    {
        connectionString = "URI=file:" + Application.dataPath + "/Database/Users.db"; // This is setting the connection string to the location of the database. Application.dataPath is /Assets, the default for unity
        p_list[0] = "hp"; //These set the strings in the array which are added in to help simplify the searching for different collumns in the stats table.
        p_list[1] = "pp";
        p_list[2] = "def";
        p_list[3] = "atk";
        p_list[4] = "spd";
    }
    //The InsertUser method is used to Insert a new user into the database.
    public static void InsertUser(string Email, string HashedPass) //The method requires the inputs from both InputFields in the Start Menu.
    {
        Debug.Log("InsertUser");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //This opens up the connection to the database using the file location previously set.
        {
            dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = String.Format("INSERT INTO Users(Email,HashedPass) VALUES(\"{0}\",\"{1}\")", Email, HashedPass); //This assigns a new variable sqlQuery to the SQL query we want to perform. I have set to Insert the input email and hashedpassword into the database in the Users table.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                dbCmd.ExecuteScalar(); //This executes the query.
                dbConnection.Close(); //This closes the connection to the database once the query has been executed.
            }
        }
    }
    //The UserChecker method is used to compare the Email and Password the User input into the InputFields with the Email and Password in the database.
    public static void UserChecker(string Email, string HashedPass) //The method requires the inputs from both InputFields in the Start Menu.
    {
        UserEmail = Email; //This assigns the variable UserEmail to the Email in the InputField further comparisons when loading and saving data.
        Debug.Log("UserChecker");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //This opens up the connection to the database using the file location previously set.
        {
            dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = "SELECT Users.HashedPass FROM Users WHERE (Users.Email = \"" + (Email) + "\")";  //This assigns a new variable sqlQuery to the SQL query we want to perform. I have set the query to compare emails with all in Email collumn in the Users Table, if there is a match then it will return the hashed password from the same row.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                {
                    while (reader.Read()) //Everything that takes place in this while loop is done when the reader is open.
                    {
                        LoginButton.PasswordCheck = (reader.GetString(0)); //Takes the output from the query and converts it to string, that is used to assign PasswordCheck in the Login Button Class that is used to compare the hashedpassword that the user has input.
                    }
                    dbConnection.Close(); //This closes the connection to the database once the query has been executed.
                    reader.Close(); //This closes the connection to the reader once the output has been read.
                }
            }
        }
    }
    //The GetUserID method is used to obtain the User's ID, that is preset in the database when a record is created, for comparisons when saving and loading data.
    public static void GetUserID(string Email) //The method requires the input from both the email InputField in the Start Menu.
    {
        UserEmail = Email; //This assigns the variable UserEmail to the Email in the InputField further comparisons when loading and saving data.
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //This opens up the connection to the database using the file location previously set.
        {
            dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = "SELECT Users.UserID FROM Users WHERE (Users.Email = \"" + (Email) + "\")"; //This assigns a new variable sqlQuery to the SQL query we want to perform. I have set it to find the ID where the email input is the same as the one in the table by comparing every email in the table.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                {
                    while (reader.Read()) //Everything that takes place in this while loop is done when the reader is open.
                    {
                        ID = (reader.GetInt32(0)); //Takes the output from the query and converts it to an input, that is used to assign ID.
                    }
                    dbConnection.Close(); //This closes the connection to the database once the query has been executed.
                    reader.Close(); //This closes the connection to the reader once the output has been read.
                }
            }
        }
    }
    //The StatUpdater method takes the characters stats from the CharacterStats class attached to the playable character and adds them into the database so that they may be loaded when the user next logs in.
    public static void StatUpdater(string p_name, int p_hp, int p_pp, int p_def, int p_atk, int p_spd) //The method requires all of the stats that are present in the CharacterStats class.
    {
        Scene currentScene = SceneManager.GetActiveScene(); //This retrieves the name of the scene so that the user can be loaded into that scene when they login.
        string SceneName = currentScene.name; //This assings the SceneName string the name of the current scene.
        bool RecordExists = false; //This is a bool used to see if a record exists in order to determine if a new row needs to be inserted into the table or data can be written over.
        Debug.Log("StatUpdater");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //This opens up the connection to the database using the file location previously set.
        {
            dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = "SELECT UserID FROM Stats WHERE UserID =" + (ID);  //This assigns a new variable sqlQuery to the SQL query we want to perform. This checks whether the UserID is present in the table
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                {
                    if (reader.IsDBNull(0) == true) //This if statement checks if the reader doesn't contain anything, IsDBNull, and if it is it will perform this statement.
                    {
                        RecordExists = false; //This assigns RecordExists to false which will determine which query is performed next. 
                    }
                    else
                    {
                        RecordExists = true; //This assigns RecordExists to true which will determine which query is performed next.
                    }
                    dbConnection.Close(); //This closes the connection to the database once the query has been executed.
                    reader.Close(); //This closes the connection to the reader once the output has been read.
                }
            }
            if (RecordExists == true) //This if statement only runs when the UserID is found in the table.
            {
                dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
                using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
                {
                    string sqlQuery = "UPDATE Stats SET p_hp = " + (p_hp) + ", p_pp = " + (p_pp) + ", p_def = " + (p_def) + ", p_atk = " + (p_atk) + ", p_spd = " + (p_spd) + ", p_location = '" + (SceneName) + "' WHERE UserID = " + (ID) + ""; //This assigns a new variable sqlQuery to the SQL query we want to perform. This updates the stats in the table to the stats in the CharacterStats with corresponding names.
                    dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                    dbCmd.ExecuteScalar(); //This executes the query.
                    dbConnection.Close(); //This closes the connection to the reader once the output has been read.
                }
            }
            if (RecordExists == false) //This if statement only runs when the UserID is not found in the table.
            {
                dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
                using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
                {
                    string sqlQuery = "INSERT INTO Stats (UserID, p_name, p_hp, p_pp, p_def, p_atk, p_spd, p_location) SELECT Users.UserID,'" + (p_name) + "'," + (p_hp) + "," + (p_pp) + "," + (p_def) + "," + (p_atk) + "," + (p_spd) + ",'" + (SceneName) + " ' FROM Users WHERE UserID = " + (ID); //This assigns a new variable sqlQuery to the SQL query we want to perform. This inserts a new record into the stats table with the CharacterStats with corresponding names.
                    dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                    using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                    {
                        while (reader.Read()) //Everything that takes place in this while loop is done when the reader is open.
                        {
                        }
                        dbConnection.Close(); //This closes the connection to the reader once the output has been read.
                        reader.Close(); //This closes the connection to the reader once the output has been read.
                    }
                }
            }
        }
        Debug.Log("Saved");
    }
    //The InventoryUpdater method takes the characters slots[] from the Character class attached to the playable character and adds them into the database so that they may be loaded when the user next logs in.
    public static void InventoryUpdater(Item[] items) //The method requires the characters inventory which is stored in an array of items.
    {
        bool RecordExists = false; //This assigns the RecordExists boolean to false.
        Debug.Log("InventoryUpdater");
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //This opens up the connection to the database using the file location previously set.
        {
            dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = "SELECT UserID FROM Inventory WHERE UserID =" + (ID); //This assigns a new variable sqlQuery to the SQL query we want to perform. This searches through the inventory table to find a record with a matching ID.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                {
                    if (reader.IsDBNull(0) == true) //This if statement checks if the reader doesn't contain anything, IsDBNull, and if it is it will perform this statement.
                    {
                        RecordExists = false; //This assigns RecordExists to false which will determine which query is performed next.
                    }
                    else
                    {
                        RecordExists = true; //This assigns RecordExists to true which will determine which query is performed next.
                    }
                    dbConnection.Close(); //This closes the connection to the reader once the output has been read.
                    reader.Close(); //This closes the connection to the reader once the output has been read.
                }
            }
            if (RecordExists == true) //This if statement only runs when the UserID is found in the table.
            {
                Debug.Log("RecordExists");
                dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
                using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
                {
                    for (int i = 0; i < 18; i++) //For loop that goes through 0 to 18, the amount of slots in the user's inventory.
                    {
                        string sqlQuery = " UPDATE Inventory SET Slot" + (i + 1) + "_ID =" + (items[i].ID) + " WHERE UserID = " + (ID); //This assigns a new variable sqlQuery to the SQL query we want to perform. This will update the inventory slot in the Inventory table with the item that is held in the corresponding slot in the Item array.
                        dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                        dbCmd.ExecuteScalar(); //This executes the query.
                    }
                    dbConnection.Close(); //This closes the connection to the reader once the output has been read.
                }
            }
            if (RecordExists == false)  //This if statement only runs when the UserID is not found in the table.
            {
                Debug.Log("RecordNoExists");
                dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
                using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
                {
                    string sqlQuery = "INSERT INTO Inventory (UserID) VALUES (" + (ID) + ")"; //This assigns a new variable sqlQuery to the SQL query we want to perform. This query inserts a record into the Inventory table with each of the inventory slots with the item that is held in the corresponding slot in the Item array.
                    dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                    using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                    {
                        dbCmd.ExecuteScalar(); //This executes the query.
                        dbConnection.Close(); //This closes the connection to the reader once the output has been read.
                    }
                }
                dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
                using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
                {
                    for (int i = 0; i < 18; i++) //For loop that goes through 1 to 18, the amount of slots in the user's inventory.
                    {
                        string sqlQuery = " UPDATE Inventory SET Slot" + (i + 1) + "_ID =" + (items[i].ID) + " WHERE UserID = " + (ID); //This assigns a new variable sqlQuery to the SQL query we want to perform. This will update the inventory slot in the Inventory table with the item that is held in the corresponding slot in the Item array.
                        dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                        dbCmd.ExecuteScalar(); //This executes the query.
                    }
                }
            }
        }
        Debug.Log("Saved");
    }
    //The StatLoader method takes the characters stats from the Stats database table and then replaces the stats in the CharacterStats class when you load in.
    public static void StatLoader(int p_hp, int p_pp, Stat p_def, Stat p_atk, Stat p_spd)  //The method requires all of the stats that are present in the CharacterStats class.
    {
        GameObject CharacterGo = GameObject.Find("CharacterOverworld"); //This finds and assigns the players Character so that the CharacterStats can be accessed.
        bool RecordExists = false; //This assigns the RecordExists boolean to false.
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //This opens up the connection to the database using the file location previously set.
        {
            dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = "SELECT UserID FROM Inventory WHERE UserID =" + (ID); //This assigns a new variable sqlQuery to the SQL query we want to perform. This searches through the inventory table to find a record with a matching ID.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                {
                    if (reader.IsDBNull(0) == true) //This if statement checks if the reader doesn't contain anything, IsDBNull, and if it is it will perform this statement.
                    {
                        RecordExists = false; //This assigns RecordExists to false which will determine which query is performed next.
                    }
                    else
                    {
                        RecordExists = true; //This assigns RecordExists to true which will determine which query is performed next.
                    }
                    dbConnection.Close(); //This closes the connection to the reader once the output has been read.
                    reader.Close(); //This closes the connection to the reader once the output has been read.
                }
            }
        }
        if (RecordExists == true) //This if statement only runs when the UserID is found in the table.
        {
            using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //This opens up the connection to the database using the file location previously set.            
            {
                for (int i = 0; i < 5; i++) //For loop that goes through 0 to 5, the amount of columns that need to be updated .
                {
                    dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
                    using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
                    {
                        string sqlQuery = "SELECT p_" + (p_list[i]) + " FROM Stats WHERE UserID =" + (ID); //This assigns a new variable sqlQuery to the SQL query we want to perform. This retrieves the record from the column that corresponds to the item in the array at slot i where the ID is the same as the User's.
                        dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                        using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                        {
                            while (reader.Read()) //Everything that takes place in this while loop is done when the reader is open.
                            {
                                if (i == 0)
                                {
                                    CharacterGo.GetComponent<CharacterStats>().p_hp = reader.GetInt32(0); //This sets the p_hp stat in CharactersStats to the value retrieved by the query.
                                }
                                if (i == 1)
                                {
                                    CharacterGo.GetComponent<CharacterStats>().p_pp = reader.GetInt32(0); //This sets the p_pp stat in CharactersStats to the value retrieved by the query.
                                }
                                if (i == 2)
                                {
                                    CharacterGo.GetComponent<CharacterStats>().p_def.BaseValue = reader.GetInt32(0); //This sets the p_def stat in CharactersStats to the value retrieved by the query.
                                }
                                if (i == 3)
                                {
                                    CharacterGo.GetComponent<CharacterStats>().p_atk.BaseValue = reader.GetInt32(0); //This sets the p_atk stat in CharactersStats to the value retrieved by the query.
                                }
                                if (i == 4)
                                {
                                    CharacterGo.GetComponent<CharacterStats>().p_spd.BaseValue = reader.GetInt32(0); //This sets the p_spd stat in CharactersStats to the value retrieved by the query.
                                }
                            }
                            dbConnection.Close(); //This closes the connection to the reader once the output has been read.
                            reader.Close(); //This closes the connection to the reader once the output has been read.
                        }
                    }
                }
            }
        }
    }
    //The InventoryLoader method retrieves each of the ItemID's in the Inventory table for the users's ID, checks the ID against every Item GameObject and then inserts that GameObject into the user's Inventory
    public static void InventoryLoader(Item[] items) //The method requires the characters inventory, items, which is stored in an array of items.
    {
        bool RecordExists = false;  //This assigns the RecordExists boolean to false.
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))  //This opens up the connection to the database using the file location previously set.
        {
            dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
            {
                string sqlQuery = "SELECT UserID FROM Inventory WHERE UserID =" + (ID); //This assigns a new variable sqlQuery to the SQL query we want to perform. This searches through the inventory table to find a record with a matching ID.
                dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                {
                    if (reader.IsDBNull(0) == true) //This if statement checks if the reader doesn't contain anything, IsDBNull, and if it is it will perform this statement.
                    {
                        RecordExists = false; //This assigns RecordExists to false which will determine which query is performed next. 
                    }
                    else
                    {
                        RecordExists = true; //This assigns RecordExists to true which will determine which query is performed next.
                    }
                    dbConnection.Close(); //This closes the connection to the database once the query has been executed.
                    reader.Close(); //This closes the connection to the reader once the output has been read.
                }
            }
            if (RecordExists == true) //This if statement only runs when the UserID is found in the table.
            {
                for (int i = 0; i < 18; i++) //For loop that goes through 0 to 18, the amount of slots in the user's inventory.
                {
                    dbConnection.Open(); //This opens up the connection using System.Data's .open function that is held in it's IDbConnection class.
                    using (IDbCommand dbCmd = dbConnection.CreateCommand()) //This allows the text to be entered into the command line.
                    {
                        string sqlQuery = "SELECT Slot" + (i + 1) + "_ID FROM Inventory WHERE UserID =" + (ID); //This assigns a new variable sqlQuery to the SQL query we want to perform. This query selects an item slot from Inventory where the the User's ID is the same as the ID in the table.
                        dbCmd.CommandText = sqlQuery; //This enters the sqlQuery query into the command line.
                        using (IDataReader reader = dbCmd.ExecuteReader()) //This builds an SqlDataReader where the query's output will be stored.
                        {
                            int ItemID = Int32.Parse(reader.GetValue(0).ToString()); //This assigns the value of ItemID to the databases ItemID held in slot i.
                            InventorySearcher(ItemID, Character.slot, i); //This calls the Inventory search method with the ItemID collected by the reader, this is then compared against every item to obtain the GameObject with the same ID and that would be inserted into the player's inventory at slot i.
                            dbConnection.Close(); //This closes the connection to the reader once the output has been read.
                            reader.Close(); //This closes the connection to the reader once the output has been read.
                        }
                    }
                }
            }
            else
            {
            }
        }
    }
    //The InventorySearcher method takes the ItemID from an item in the inventory and then compares that with every item in the project's IDs, then when it finds a match it will place that Item into the corresponding Inventory lot using int InventorySlot.
    public static void InventorySearcher(int ItemID, Item[] ItemsReference, int InventorySlot) //The method requires the ItemID obtained in InventoryUpdater from the database, a list of each item in the projects IDs, ItemsReference and the inventory slot the GameObject needs to be placed into, InventorySlot.
    {
        GameObject[] AllItems = Resources.LoadAll<GameObject>("Items"); //This creates an array of GameObjects that is populated by every prefab GameObject that is in Resources/Items. 
        for (int j = 0; j < AllItems.Length; j++) //This is a for loop fro 0 to the amount of GameObjects in AllItems[].
        {
            if (AllItems[j].GetComponent<Item>().ID == ItemID) //This if statement only performs when a match between the ID from the Database and item GameObject is found.
            {
                ItemsReference[InventorySlot] = AllItems[j].GetComponent<Item>(); //This places the matching GameObject into the user's inventory.
            }
        }
    }
}
