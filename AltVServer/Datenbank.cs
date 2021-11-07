﻿using AltV.Net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVServer
{
    class Datenbank : Server
    {
        public static bool DatenbankVerbindung = false;
        public static MySqlConnection Connection;
        public string Host { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Database { get; set; }

        public Datenbank ()
        {
            this.Host = "localhost";
            this.Username = "altv";
            this.Password = "12345";
            this.Database = "altv";
        }

        public static String GetConnectionString()
        {
            Datenbank sql = new Datenbank();
            String SQLConnection = $"Server={sql.Host}; Database={sql.Database}; UID={sql.Username}; Password={sql.Password}";
            return SQLConnection;
        }

        public static void InitConnection()
        {
            String SQLConnection = GetConnectionString();
            Connection = new MySqlConnection(SQLConnection);
            try
            {
                Connection.Open();
                DatenbankVerbindung = true;
                Alt.Log("MYSQL Verbindung erfolgreich aufgebaut!");
            } catch(Exception e)
            {
                DatenbankVerbindung = false;
                Alt.Log("MYSQL Verbindung konnte nicht aufgebaut werden!");
                Alt.Log(e.ToString());
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public static bool IstAccountBereitsVorhanden(string name)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE name=@name LIMIT 1";
            command.Parameters.AddWithValue("@name", name);
            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    return true;
                }
            }
            return false;
        }

        public static int NeuenAccountErstellen(String name, string password)
        {
            string saltedPw = BCrypt.HashPassword(password, BCrypt.GenerateSalt());

            try
            {
                MySqlCommand command = Connection.CreateCommand();
                command.CommandText = "INSERT INTO accounts (password, name) VALUES (@password, @name)";

                command.Parameters.AddWithValue("@password", saltedPw);
                command.Parameters.AddWithValue("@name");
                command.ExecuteNonQuery();

                return (int)command.LastInsertedId;
            }
            catch(Exception e)
            {
                Alt.Log("Fehler bei NeuenAccountErstellen: " + e.ToString());
                return -1;
            }
        }

        public static void AccountLaden(TPlayer tplayer)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM accounts WHERE name=@name LIMIT 1";

            command.Parameters.AddWithValue("@name", tplayer.SpielerName);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    tplayer.SpielerID = reader.GetInt32("id");
                    tplayer.Adminlevel = reader.GetInt32("adminlevel");
                    tplayer.Geld = reader.GetInt32("geld");

                }
            }
        }

        public static void AccountSpeichern(TPlayer tplayer)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "UPDATE accounts SET adminlevel=@adminlevel, geld=@geld WHERE id=@id";

            command.Parameters.AddWithValue("@adminlevel", tplayer.Adminlevel);
            command.Parameters.AddWithValue("@geld", tplayer.Geld);
            command.Parameters.AddWithValue("@id", tplayer.SpielerID);
        }

        public static bool PasswortCheck(string name, string passwordinput)
        {
            string password = "";
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT password FROM sccounts WHERE name@name Limit 1";
            command.Parameters.AddWithValue("@name", name);

            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    password = reader.GetString("password");
                }
            }

            if (BCrypt.CheckPassword(passwordinput, password)) return true;
            return false;

        }
     
    }
}
