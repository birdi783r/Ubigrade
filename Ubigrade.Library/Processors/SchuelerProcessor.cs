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
        public async static Task<bool> CreateSchuelerAsync(string checknp, string nname, string vname, string geschlecht, string email, int sjahr, string sql)
        {

            try
            {
                bool b1 = await ExistsSchuelerByIdAsync(checknp, sql);
                if (b1 == true)
                    return false;
                string statement = $"call insertschueler('{vname}', '{nname}', '{geschlecht.ToUpper()}', '{email}', '{checknp}', {sjahr.ToString()});";
                using (NpgsqlConnection connection = new NpgsqlConnection(sql))
                {
                    connection.Open();
                    NpgsqlCommand command;
                    command = new NpgsqlCommand(statement, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();

                    await command.ExecuteNonQueryAsync();

                    connection.Close();

                }
                bool b2 = await ExistsSchuelerByIdAsync(checknp, sql);
                if (b1 == false && b2 == true)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async static Task<bool> CreateSchueler2FaecherAsync(string checknp, string nname, string vname, string geschlecht, string email, int sjahr, string sql)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(sql))
                {
                    NpgsqlCommand command;
                    var x = geschlecht.ToUpper();

                    connection.Open();
                    //Fehler
                    command =
                        new NpgsqlCommand
                        ($"insert into schueler2faecher (bez, skennzahl) VALUES ('M',{checknp},('E',{checknp}),('NWT1',{checknp}),('WIRE',{checknp}),('ITP2',{checknp});", connection);
                    int i = await command.ExecuteNonQueryAsync();
                    connection.Close();
                    // sicher? 
                    if (i == 1)
                        return true;
                    return false;
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async static Task<List<SchuelerDLModel>> LoadSchuelerAsync(string sql)
        {
            List<SchuelerDLModel> ListeSchueler = new List<SchuelerDLModel>();

            using (NpgsqlConnection connection = new NpgsqlConnection(sql))
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
                            //int.Parse(dr[0].ToString().Trim()),
                            //int.Parse(dr[1].ToString().Trim()),
                            dr[0].ToString().Trim(),
                            dr[1].ToString().Trim(),
                            dr[2].ToString().Trim(),
                            dr[3].ToString().Trim(),
                            dr[4].ToString().Trim(),
                            dr[5].ToString().Trim(),
                            int.Parse(dr[6].ToString().Trim())
                            )
                        );
                }
                connection.Close();
            }
            return ListeSchueler;
        }
        public async static Task<bool> ExistsSchuelerByIdAsync(string userid, string sql)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(sql))
            {
                NpgsqlCommand command;

                connection.Open();
                command =
                    new NpgsqlCommand
                    ($"select skennzahl, checkpersonnumber, nname, vname, gender, email, sstufe from schueler join person on skennzahl = checkpersonnumber where checkpersonnumber = '{userid}' order by skennzahl;", connection);

                var dr = await command.ExecuteReaderAsync();
                var d = dr.Read();
                if (d)
                {
                    var id = dr[1].ToString();
                    connection.Close();
                    if (id.Contains(userid))
                    {
                        connection.Close();
                        return true;
                    }
                    return false;
                }
                connection.Close();
                return false;
            }
        }
        public async static Task<bool> SaveSchuelerAsync(string sql, string id, SchuelerDLModel changedschueler)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(sql))
            {
                NpgsqlCommand command;

                //Edit Schueler props [person]
                connection.Open();
                command =
                new NpgsqlCommand
                ($"update person set vname = '{changedschueler.VName}', nname = '{changedschueler.NName}', " +
                $"gender = '{changedschueler.Geschlecht.ToUpper()}', email = '{changedschueler.EmailAdresse}' where checkpersonnumber = '{id}'; ", connection);
                int i = await command.ExecuteNonQueryAsync();

                //Edit Schueler sstufe [schueler]
                command =
                new NpgsqlCommand
                ($"update schueler set sstufe = {changedschueler.Schuljahr} where skennzahl = '{id}'; ", connection);
                i += await command.ExecuteNonQueryAsync();
                connection.Close();
                if (i == 2)
                    return true;
                return false;
            }
        }
        public async static Task<SchuelerDLModel> GetByIdSchuelerAsync(string sql, string id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(sql))
            {
                NpgsqlCommand command;

                //Edit Schueler props [person]
                connection.Open();
                command =
                new NpgsqlCommand
                ($"select skennzahl, checkpersonnumber, nname, vname, gender, email, sstufe from schueler " +
                $"join person on skennzahl = checkpersonnumber " +
                $"where checkpersonnumber = '{id}' " +
                $"order by skennzahl;", connection);
                var dr = await command.ExecuteReaderAsync();
                SchuelerDLModel editschueler = null;

                while (dr.Read())
                {
                    editschueler = new SchuelerDLModel(/*int.Parse(dr[0].ToString()), int.Parse(dr[1].ToString()),*/dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), int.Parse(dr[6].ToString()));
                }
                editschueler.Faecher = await LoadSchuelerFaecherAsync(sql, id);
                connection.Close();

                return editschueler;
            }
        }
        public static async Task<bool> DeleteSchuelerAsync(string sql, string id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(sql))
            {
                NpgsqlCommand command;
                //noch ned ganz fertig denk ich - rohit
                //Delete Schueler props [person]
                connection.Open();
                command =
                    new NpgsqlCommand($"delete from person where checkpersonnumber = '{id}';", connection);
                int i = await command.ExecuteNonQueryAsync();
                int b = 0;
                if (i == 1)
                {
                    command =
                        new NpgsqlCommand($"delete from schueler where skennzahl = '{id}';", connection);
                    b = await command.ExecuteNonQueryAsync();
                    connection.Close();
                }
                if (b == 1)
                    return true;
                return false;
            }
        }

        public async static Task<List<FaecherDLModel>> LoadSchuelerFaecherAsync(string sql, string id)
        {
            List<FaecherDLModel> SchuelerFaecher = new List<FaecherDLModel>();
            using (NpgsqlConnection connection = new NpgsqlConnection(sql))
            {
                SchuelerFaecher.Clear();
                NpgsqlCommand command;
                connection.Open();
                command =
                new NpgsqlCommand($"select skennzahl, bez from schueler2faecher where skennzahl = '{id}';", connection);
                var dr = await command.ExecuteReaderAsync();
                while (dr.Read())
                {
                    SchuelerFaecher.Add(new FaecherDLModel { Skennzahl = dr[0].ToString(), Fachbezeichnung = dr[1].ToString().Trim() });
                }
                connection.Close();
            }
            return SchuelerFaecher;
        }
    }
}
