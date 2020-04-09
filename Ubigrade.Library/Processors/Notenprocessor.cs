using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Ubigrade.Library.Models;

namespace Ubigrade.Library.Processors
{
    public class NotenProcessor
    {
        public static List<NotenDLModel> ListeNoten = new List<NotenDLModel>();

        public static void CreateNote(int nid, int bez, int min)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
                {
                    NpgsqlCommand command;

                    connection.Open();
                    command =
                        new NpgsqlCommand($"insert into notenschema (nid, bez, mindestanforderung) VALUES ({nid},{bez},{min});", connection);

                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<NotenDLModel> LoadNoten()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
                {
                    ListeNoten.Clear();
                    NpgsqlCommand command;

                    connection.Open();
                    command =
                        new NpgsqlCommand($"select * from notenschema order by nid", connection);

                    var dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        ListeNoten.Add(
                            new NotenDLModel(
                                int.Parse(dr[0].ToString()),
                                int.Parse(dr[1].ToString()),
                                int.Parse(dr[2].ToString())
                                )
                            );
                    }

                    connection.Close();
                }
                return ListeNoten;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void SaveNote(int id, NotenDLModel changednote)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
                {
                    NpgsqlCommand command;

                    //Edit Note props [notenschema]
                    connection.Open();
                    command =
                    new NpgsqlCommand
                    ($"update notenschema set bez = {changednote.Bezeichnung}, mindestanforderung = {changednote.Mindestanforderung} where nid = {id};", connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static NotenDLModel GetByIdNote(int id)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
                {
                    NpgsqlCommand command;

                    //Edit Schueler props [notenschema]
                    connection.Open();
                    command =
                    new NpgsqlCommand
                    ($"select * from notenschema where nid = {id};", connection);
                    var dr = command.ExecuteReader();
                    NotenDLModel editnote = null;

                    while (dr.Read())
                    {
                        editnote =
                            new NotenDLModel(
                                int.Parse(dr[0].ToString()),
                                int.Parse(dr[1].ToString()),
                                int.Parse(dr[2].ToString())
                                );
                    }
                    connection.Close();

                    return editnote;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteNote(int id)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString()))
                {
                    NpgsqlCommand command;

                    //Delete Schueler props [notenschema]
                    connection.Open();
                    command =
                        new NpgsqlCommand($"delete from notenschema where nid = {id};", connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
