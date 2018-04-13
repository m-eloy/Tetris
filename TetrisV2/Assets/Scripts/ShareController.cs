using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//database
using Mono.Data.Sqlite;
using System.Data;
using System;

//share

public class ShareController : MonoBehaviour {

    public Dropdown dropdown;
    private string conn;
    private int BDScore;
    private string pseudo;
    private int score;

    void Start () {
        conn = "URI=file:" + Application.dataPath + "/StreamingAssets/database.s3db"; //Path to database.
        SelectScorePlayer();
    }  

    private void SelectScorePlayer()
    {
        using (IDbConnection dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();

            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM utilisateur WHERE idJoueur=" + DropdownPlayer.idPlayer + "";

                dbcmd.CommandText = sqlQuery;

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pseudo = reader.GetString(1);
                        score = reader.GetInt32(3);
                        Debug.Log(pseudo + " " + score);
                    }
                    dbconn.Close();
                    reader.Close();
                }
            }
        }
    }
    
    public void Return()
    {
        Application.LoadLevel("GameOver");
    }
    
    
}
