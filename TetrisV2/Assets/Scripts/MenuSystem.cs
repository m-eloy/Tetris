using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//database
using Mono.Data.Sqlite;
using System.Data;
using System;

public class MenuSystem : MonoBehaviour
{

    public Text lastScore;
    int newScore = Game.currentScore;
    private string conn;
    private int BDScore;
    int idPlayer = DropdownPlayer.idPlayer;

    //highscore
    public Text joueur1;
    public Text joueur2;
    public Text joueur3;
    public Text score1;
    public Text score2;
    public Text score3;
    List<string> names = new List<string>() { };
    List<int> scores = new List<int>() { };


    void Start()
    {

        lastScore.text = newScore.ToString();

        System.Diagnostics.Debug.WriteLine(newScore.ToString());

        conn = "URI=file:" + Application.dataPath + "/StreamingAssets/database.s3db"; //Path to database.
        SelectScoreBD();
        UpdateScoreIfHigher();
        GetScoreAndPlayers();
        ShowHighscores();
        
    }

    private void GetScoreAndPlayers()
    {
        using (IDbConnection dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();

            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM UTILISATEUR ORDER BY score DESC LIMIT 3";

                dbcmd.CommandText = sqlQuery;

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        names.Add(reader.GetString(1));
                        scores.Add(reader.GetInt32(3));
                        Debug.Log(reader.GetString(1) + '-' + reader.GetInt32(3));
                    }
                    dbconn.Close();
                    reader.Close();
                }
            }
        }
    }

    private void ShowHighscores()
    {
        joueur1.text = names[0].ToString();
        score1.text = scores[0].ToString();

        joueur2.text = names[1].ToString();
        score2.text = scores[1].ToString();

        joueur3.text = names[2].ToString();
        score3.text = scores[2].ToString();
    }

    private void SelectScoreBD()
    {
        using (IDbConnection dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();

            using(IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM utilisateur WHERE idJoueur="+ idPlayer + "";

                dbcmd.CommandText = sqlQuery;

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       Debug.Log("score =" + reader.GetInt32(3));
                       BDScore = reader.GetInt32(3);
                    }
                    dbconn.Close();
                    reader.Close();
                }
            }
        }
    }

    private void UpdateScoreIfHigher()
    {
        if(BDScore < newScore)
        {
            using (IDbConnection dbconn = new SqliteConnection(conn))
        {
            dbconn.Open();

            using (IDbCommand dbcmd = dbconn.CreateCommand())
            {
                string sqlQuery = "UPDATE utilisateur SET score = '" + newScore + "' WHERE idJoueur=" + idPlayer + "";

                dbcmd.CommandText = sqlQuery;

                using(IDataReader reader = dbcmd.ExecuteReader())
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


    public void PlayAgain()
    {
        Game.currentScore = 0;
        Application.LoadLevel("Level");
    }

    public void ChangePlayer()
    {
        Game.currentScore = 0;
        DropdownPlayer.idPlayer = 0;
        Application.LoadLevel("Identification");

    }

    public void ShareScore()
    {
        Application.LoadLevel("Share");

    }
}
