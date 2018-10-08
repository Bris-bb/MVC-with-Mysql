using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Net;

namespace WebApplication5.Models
{
    public class RetiretagsClass
    {
        public int tag_id { get; set; }
        public string tag_name { get; set; }
        public MySqlConnection mysql;

        public void MysqlConnect()
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            mysql = new MySqlConnection(mainconn);
        }


        public RetiretagsClass Detail(int id)
        {
            MysqlConnect();
            string query = "select * from tag where retire=1 and tag_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            RetiretagsClass x = new RetiretagsClass
            {
                tag_id = (int)dr["tag_id"],
                tag_name = dr["tag_name"].ToString()
            };
            mysql.Close();
            return x;
        }
        public RetiretagsClass Detail(string tag_name)
        {
            MysqlConnect();
            string query = "select * from tag where retire=1 and tag_name='" + tag_name + "'";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            RetiretagsClass x = new RetiretagsClass
            {
                tag_id = (int)dr["tag_id"],
                tag_name = dr["tag_name"].ToString()
            };
            mysql.Close();
            return x;
        }

        public List<RetiretagsClass> Read()
        {
            MysqlConnect();
            List<RetiretagsClass> list1 = new List<RetiretagsClass>();
            string query = "select * from tag where retire=1";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new RetiretagsClass
                {
                    tag_id = (int)dr["tag_id"],
                    tag_name = dr["tag_name"].ToString()
                });
            }
            mysql.Close();
            return list1;
        }

        public void Delete(RetiretagsClass p)
        {
            MysqlConnect();
            string query = "Delete from `tag`  where tag_id=" + p.tag_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Delete(int tag_id)
        {
            MysqlConnect();
            string query = "Delete from `tag`  where tag_id=" + tag_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Active(int tag_id)
        {
            MysqlConnect();
            string query = "UPDATE `tag` SET `retire` = 0 WHERE `tag`.`tag_id` = " + tag_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }
    }
}