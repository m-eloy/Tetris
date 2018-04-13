using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//database
using Mono.Data.Sqlite;
using System.Data;
using System;

public class DropdownPlayer : MonoBehaviour {

    List<string> players = new List<string>() { };

    public Dropdown dropdown;
    public Text selectedName;
    private string conn;
    private int BDScore;
    public static int idPlayer=0;
    public Text meilleurScore;


    public void Dropdown_IndexChanged(int index)
    {
        selectedName.text = players[index];
        idPlayer = index;
        SelectScorePlayer();
    }

    void Start()
    {
        conn = "URI=file:" + Application.dataPath + "/database.s3db"; //Path to database.
        ShowPlayerBD();
        PopulateList();
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
                        players.Add(pseudo);
                    }
                    dbconn.Close();
                    reader.Close();
                }
            }
        }
    }

    private void SelectScorePlayer()
    {
        using (IDbConnection dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();

            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM utilisateur WHERE idJoueur="+ idPlayer +"";

                dbcmd.CommandText = sqlQuery;

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        meilleurScore.text = reader.GetInt32(3).ToString();
                    }
                    dbconn.Close();
                    reader.Close();
                }
            }
        }
    }

    void PopulateList()
    {
        dropdown.AddOptions(players);
    }

    public void Jouez()
    {
        Application.LoadLevel("Level");
    }

    public void Inscription()
    {
        Application.LoadLevel("Inscription");
    }


}
