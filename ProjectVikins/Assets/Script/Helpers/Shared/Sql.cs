using System.Data;
using System.IO;
using Mono.Data.SqliteClient;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.ComponentModel;

namespace Assets.Script.Helpers.Shared
{
    public class Sql<TEntity>
        where TEntity : class
    {
        List<TEntity> _Data = new List<TEntity>();
        public List<TEntity> Data { get { return _Data; } }
        string conn = "";
        private string sqlQuery;
        IDbConnection dbconn;
        IDbCommand dbcmd;

        public void Add(TEntity data)
        {
            using (dbconn = new SqliteConnection(conn))
            {
                dbconn.Open();
                dbcmd = dbconn.CreateCommand();
                sqlQuery = string.Format("insert into {0} values ( ", typeof(TEntity).Name);
                var values = data.GetType().GetProperties().Select(x => x.GetValue(data, null)).ToArray();
                var count = values.Count();
                for (int i = 0; i < count; i++)
                {
                    sqlQuery += values[i];
                    sqlQuery += i != count ? ", " : " )";
                }

                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteScalar();
                dbconn.Close();
            }
        }
        //public void Delete(int id)
        //{
        //    using (dbconn = new SqliteConnection(conn))
        //    {
        //        dbconn.Open();
        //        dbcmd = dbconn.CreateCommand();
        //        sqlQuery = string.Format("Delete from Usersinfo WHERE ID=\"{0}\"", id);
        //        dbcmd.CommandText = sqlQuery;
        //        dbcmd.ExecuteScalar();
        //        dbconn.Close();
        //    }
        //}


        public void Update(TEntity data)
        {
            using (dbconn = new SqliteConnection(conn))
            {

                dbconn.Open();
                dbcmd = dbconn.CreateCommand();
                sqlQuery = string.Format("UPDATE {0} set", typeof(TEntity).Name);

                var names = data.GetType().GetProperties().ToArray();
                var values = data.GetType().GetProperties().Select(x => x.GetValue(data, null)).ToArray();
                var count = values.Count();

                for (int i = 0; i < count; i++)
                {
                    sqlQuery += string.Format("{0}={1}", names[i], values[i]);
                    sqlQuery += i != count ? ", " : " )";
                }
                var keyProperty = data.GetType().GetProperties().SingleOrDefault(x => x.GetDisplayName() == "Key");
                sqlQuery += string.Format("WHERE {0}={1}", keyProperty.Name, keyProperty.GetValue(data, null));
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteScalar();
                dbconn.Close();
            }
        }


        public TEntity GetData(int Id)
        {
            using (dbconn = new SqliteConnection(conn))
            {
                dbconn.Open();
                dbcmd = dbconn.CreateCommand();
                sqlQuery = string.Format("SELECT * " + "FROM {0}", typeof(TEntity).Name);
                var prop = typeof(TEntity).GetProperties().SingleOrDefault(x => x.GetDisplayName() == "Key");
                sqlQuery += string.Format("WHERE {0}={1}", prop.Name, Id);
                dbcmd.CommandText = sqlQuery;
                IDataReader reader = dbcmd.ExecuteReader();
                object data = Activator.CreateInstance(typeof(TEntity));
                DataTable schemaTable = reader.GetSchemaTable();

                foreach (DataRow row in schemaTable.Rows)
                {
                    foreach (DataColumn column in schemaTable.Columns)
                    {
                        data.GetType().GetProperty(column.ColumnName).SetValue(data, row[column], null);
                    }
                }
                reader.Close();
                reader = null;
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;
                return (TEntity)data;
            }
        }


        public List<TEntity> GetData()
        {
            string conn = "URI=file:" + Application.dataPath + "/Database/SQLite.db";

            using (dbconn = new SqliteConnection(conn))
            {
                object data = Activator.CreateInstance(typeof(TEntity));
                var keyProperty = data.GetType().GetProperties().SingleOrDefault(x => x.GetDisplayName() == "Key");

                var T = new List<TEntity>();
                dbconn.Open();
                dbcmd = dbconn.CreateCommand();
                sqlQuery = string.Format("SELECT * " + "FROM {0}", typeof(TEntity).Name);
                dbcmd.CommandText = sqlQuery;

                using (var reader = dbcmd.ExecuteReader())
                {
                    do
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var prop = data.GetType().GetProperty(reader.GetName(i));
                                if (prop.PropertyType == typeof(bool))
                                    prop.SetValue(data, reader.GetBoolean(i), null);
                                else
                                    prop.SetValue(data, reader[i], null);

                            }
                            var keyValue = data.GetType().GetProperty(keyProperty.Name).GetValue(data, null);
                            var keys = _Data.Select(y => y.GetType().GetProperties().SingleOrDefault(x => x.GetDisplayName() == "Key").GetValue(y, null));
                            if (keys.Contains(keyValue))
                            {
                                data = Activator.CreateInstance(typeof(TEntity));
                                continue;
                            }

                            T.Add((TEntity)data);
                            data = Activator.CreateInstance(typeof(TEntity));
                        }
                    } while (reader.NextResult());
                }
                dbcmd.Dispose();
                dbcmd = null;
                dbconn.Close();
                dbconn = null;
                _Data.AddRange(T);
                return T;
            }
        }
    }
}
