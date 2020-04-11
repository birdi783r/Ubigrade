using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Ubigrade.Library.Models;

namespace Ubigrade.Library.Processors
{
    public static class SchuelerProcessor
    {
        public static void CreateSchueler(int checknp, string nname, string vname, string geschlecht, string email, int sjahr)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
                {
                    NpgsqlCommand command;
                    var x = geschlecht.ToUpper();

                    connection.Open();
                    command =
                            new NpgsqlCommand
                            ($"call insertschueler('{vname}', '{nname}', '{geschlecht.ToUpper()}', '{email}', {checknp}, {sjahr});", connection);
                    command.ExecuteNonQuery();


                    //Fehler
                    command =
                        new NpgsqlCommand
                        ($"insert into schueler2faecher (bez, skennzahl) VALUES ('M',{checknp},('E',{checknp}),('NWT1',{checknp}),('WIRE',{checknp}),('ITP2',{checknp});", connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async static Task<List<SchuelerDLModel>> LoadSchuelerAsync(/*string sql*/ )
        {
            List<SchuelerDLModel> ListeSchueler = new List<SchuelerDLModel>();

            using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
            {
                ListeSchueler.Clear();
                NpgsqlCommand command;

                connection.Open();
                command =
                    new NpgsqlCommand
                    ($"select skennzahl, checkpersonnumber, nname, vname, gender, email, sstufe from schueler join person on skennzahl = checkpersonnumber order by skennzahl;", connection);

                var dr = await command.ExecuteReaderAsync();

                while (dr.Read())
                {
                    ListeSchueler.Add(
                        new SchuelerDLModel(
                            int.Parse(dr[0].ToString()),
                            int.Parse(dr[1].ToString()),
                            dr[2].ToString(),
                            dr[3].ToString(),
                            dr[4].ToString(),
                            dr[5].ToString(),
                            int.Parse(dr[6].ToString())
                            )
                        );
                }

                connection.Close();
            }
            return ListeSchueler;
        }

        public static void SaveSchueler(int id, SchuelerDLModel changedschueler)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
            {
                NpgsqlCommand command;

                //Edit Schueler props [person]
                connection.Open();
                command =
                new NpgsqlCommand
                ($"update person set checkpersonnumber = {changedschueler.Checkpersonnumber}, vname = '{changedschueler.VName}', nname = '{changedschueler.NName}', " +
                $"gender = '{changedschueler.Geschlecht.ToUpper()}', email = '{changedschueler.EmailAdresse}' where checkpersonnumber = {id}; ", connection);
                command.ExecuteNonQuery();

                connection.Close();

                //Edit Schueler sstufe [schueler]
                connection.Open();
                command =
                new NpgsqlCommand
                ($"update schueler set sstufe = {changedschueler.Schuljahr} where skennzahl = {id}; ", connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static SchuelerDLModel GetByIdSchueler(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
            {
                NpgsqlCommand command;

                //Edit Schueler props [person]
                connection.Open();
                command =
                new NpgsqlCommand
                ($"select skennzahl, checkpersonnumber, nname, vname, gender, email, sstufe from schueler " +
                $"join person on skennzahl = checkpersonnumber " +
                $"where checkpersonnumber = {id} " +
                $"order by skennzahl;", connection);
                var dr = command.ExecuteReader();
                SchuelerDLModel editschueler = null;

                while (dr.Read())
                {
                    editschueler = new SchuelerDLModel(int.Parse(dr[0].ToString()), int.Parse(dr[1].ToString()), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), int.Parse(dr[6].ToString()));
                }
                connection.Close();

                return editschueler;
            }
        }

        public static void DeleteSchueler(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
            {
                NpgsqlCommand command;

                //Delete Schueler props [person]
                connection.Open();
                command =
                    new NpgsqlCommand($"delete from person where checkpersonnumber = {id};", connection);
                command.ExecuteNonQuery();

                connection.Close();

                //Delete sstufe [schueler]
                connection.Open();
                command =
                    new NpgsqlCommand($"delete from schueler where skennzahl = {id};", connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static List<FaecherDLModel> LoadSchuelerFaecher(int id)
        {
            List<FaecherDLModel> SchuelerFaecher = new List<FaecherDLModel>();
            using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
            {
                SchuelerFaecher.Clear();
                NpgsqlCommand command;

                connection.Open();
                command =
                new NpgsqlCommand($"select skennzahl, bez from schueler2faecher where skennzahl = {id} order by 2;", connection);
                var dr = command.ExecuteReader();

                while (dr.Read())
                {
                    SchuelerFaecher.Add(new FaecherDLModel { Skennzahl = int.Parse(dr[0].ToString()), Fachbezeichnung = dr[1].ToString() });
                }

                connection.Close();
            }
            return SchuelerFaecher;
        }
    }
}
