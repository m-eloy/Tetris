using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//database
using Mono.Data.Sqlite;
using System.Data;
using System;

public class InscriptionController : MonoBehaviour {

    public Text pseudo;
    public InputField pseudoField;
    public Text email;
    public InputField emailField;
    private string conn;
    private int BDScore;
    List<string> joueurs = new List<string>();
    bool inscription = false;

    // Use this for initialization
    void Start () {
        conn = "URI=file:" + Application.dataPath + "/StreamingAssets/database.s3db"; //Path to database.
        ShowPlayerBD();
        
    }

    private void ShowPlayerBD()
    {
        using (IDbConnection dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();

            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM utilisateur";

                dbcmd.CommandText = sqlQuery;

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string pseudo = reader.GetString(1);
                        joueurs.Add(pseudo);
                    }
                    dbconn.Close();
                    reader.Close();
                }
            }
        }
    }



    private void AjoutJoueur()
    {
        foreach (string element in joueurs)
        {
            if (inscription != true)
            {
                Debug.Log(element);
                if (pseudoField.text != element)
                {
                    using (IDbConnection dbconn = new SqliteConnection(conn))
                    {
                        dbconn.Open();

                        using (IDbCommand dbcmd = dbconn.CreateCommand())
                        {
                            string sqlQuery = "INSERT INTO utilisateur(pseudo,email,score) VALUES ('" + pseudoField.text + "', '"+ emailField.text + "', 0)";
                            inscription = true;
                            dbcmd.CommandText = sqlQuery;

                            using (IDataReader reader = dbcmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {


                                }
                                dbconn.Close();
                                reader.Close();
                            }
                        }
                    }

                }
            }
        }
    }

    public void Inscription()
    {
        AjoutJoueur();
        inscription = false;
        Application.LoadLevel("Identification");
    }
}
