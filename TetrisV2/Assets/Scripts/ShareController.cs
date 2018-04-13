using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//database
using Mono.Data.Sqlite;
using System.Data;
using System;

//share
using VoxelBusters.NativePlugins;

public class ShareController : MonoBehaviour {

    public Dropdown dropdown;
    private string conn;
    private int BDScore;
    private string pseudo;
    private int score;

    void Start () {
        conn = "URI=file:" + Application.dataPath + "/database.s3db"; //Path to database.
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

    public void ShareViaEmail()
    {
            if (NPBinding.Sharing.IsMailServiceAvailable())
            {
                // Create new instance and populate fields
                MailShareComposer _composer = new MailShareComposer();
                _composer.Subject = "De " + pseudo + " : nouveau score au Tetris !";
                _composer.Body = "Regarde mon super score au Tetris! \n Score : " + score + "";


            // Show composer
            NPBinding.Sharing.ShowView(_composer, OnFinishedSharing);
            }
            else
            {
                // Device doesn't support sending emails
            }
    }

    public void ShareViaShareSheet()
    {
        // Create new instance and populate fields
        SocialShareSheet _shareSheet = new SocialShareSheet();
        _shareSheet.Text = "Regarde mon super score au Tetris! \n Score : " + score + "";
        // On iPad, popover view is used to show share sheet. So we need to set its position
        NPBinding.UI.SetPopoverPointAtLastTouchPosition();
        // Show composer
        NPBinding.Sharing.ShowView(_shareSheet, OnFinishedSharing);
    }


    private void OnFinishedSharing(eShareResult _result)
    {
        Debug.Log("Finished sharing");
        Debug.Log("Share Result = " + _result);
    }
    
}
