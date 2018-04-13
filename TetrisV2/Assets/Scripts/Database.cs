using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;


public class Database : MonoBehaviour {

    public static void Main()
    {
        const string connectionString = "URI=file:database.db";
        IDbConnection dbcon = new SqliteConnection(connectionString);
        dbcon.Open();
        IDbCommand dbcmd = dbcon.CreateCommand();
        const string sql = "SELECT idJoueur,pseudo, score " + "FROM utilisateur";
        dbcmd.CommandText = sql;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int idJoueur = reader.GetInt32(0);
            string pseudo = reader.GetString(1);
            int score = reader.GetInt32(2);

            Debug.Log("idJoueur= " + idJoueur + "  pseudo =" + pseudo + "  score =" + score);
        }
        // clean up
        reader.Dispose();
        dbcmd.Dispose();
        dbcon.Close();
    }
}
