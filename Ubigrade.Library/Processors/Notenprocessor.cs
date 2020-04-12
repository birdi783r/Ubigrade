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
    public class NotenProcessor
    {
        public async static Task<bool> CreateNoteAsync(int nid, int bez, int min,string sql)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(sql))
                {
                    {
                        NpgsqlCommand command;

                        connection.Open();
                        command =
                            new NpgsqlCommand($"insert into notenschema (nid, bez, mindestanforderung) VALUES ({nid},{bez},{min});", connection);

                        int i = await command.ExecuteNonQueryAsync();
                        connection.Close();
                        if (i == 1)
                            return true;
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<List<NotenDLModel>> LoadNotenAsync(string con)
        {
            try
            {
                //using (NpgsqlConnection connection = new NpgsqlConnection(SqlDataAccess.GetConnectionString())) 
                List<NotenDLModel> list = new List<NotenDLModel>();

                using (NpgsqlConnection connection = new NpgsqlConnection(con))
                {
                    NpgsqlCommand command;

                    connection.Open();
                    command =
                        new NpgsqlCommand($"select * from notenschema order by nid", connection);

                    var dr = await command.ExecuteReaderAsync();

                    while (dr.Read())
                    {
                        list.Add(
                            new NotenDLModel(
                                int.Parse(dr[0].ToString()),
                                int.Parse(dr[1].ToString()),
                                int.Parse(dr[2].ToString())
                                )
                            );
                    }

                    connection.Close();
                }
                if (list.Count > 0)
                    return list;
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<bool> SaveNoteAsync(int id, NotenDLModel changednote,string sql)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(sql))
                {
                    NpgsqlCommand command;

                    //Edit Note props [notenschema]
                    connection.Open();
                    command =
                    new NpgsqlCommand
                    ($"update notenschema set bez = {changednote.Bezeichnung}, mindestanforderung = {changednote.Mindestanforderung} where nid = {id};", connection);
                    int i = await command.ExecuteNonQueryAsync();

                    connection.Close();
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

        public static async Task<NotenDLModel> GetByIdNoteAsync(int id,string sql)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(sql))
                {
                    NpgsqlCommand command;
                    //Edit Schueler props [notenschema]
                    connection.Open();
                    command =
                    new NpgsqlCommand
                    ($"select * from notenschema where nid = {id};", connection);
                    var dr = await command.ExecuteReaderAsync();
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

        public static async Task<bool> DeleteNoteAsync(int id, string sql)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(sql))
                {
                    NpgsqlCommand command;

                    //Delete Schueler props [notenschema]
                    connection.Open();
                    command =
                        new NpgsqlCommand($"delete from notenschema where nid = {id};", connection);
                    int i = await command.ExecuteNonQueryAsync();

                    connection.Close();
                    if (i == 1)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
