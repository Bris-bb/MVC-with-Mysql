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
    public class Case_tagClass
    {
        public int case_tag_id { get; set; }
        public int f_case_id { get; set; }
        public int f_tag_id { get; set; }

        public MySqlConnection mysql;

        public void MysqlConnect()
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            mysql = new MySqlConnection(mainconn);
        }

        public void DeleteAll()
        {
            MysqlConnect();
            string query = "TRUNCATE `case_tag`";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public List<Case_tagClass> Read()
        {
            MysqlConnect();
            List<Case_tagClass> list1 = new List<Case_tagClass>();
            string query = "select * from case_tag";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new Case_tagClass
                {
                    f_case_id = (int)dr["f_case_id"],
                    f_tag_id = (int)dr["f_tag_id"]
                });
            }
            mysql.Close();
            return list1;
        }
        public void Add(Case_tagClass p)
        {
            MysqlConnect();
            string query = "INSERT INTO `case_tag` (`case_tag_id`, `f_case_id`, `f_tag_id`) VALUES(NULL, '" + p.f_case_id + "', '" + p.f_tag_id + "');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Edit(Case_tagClass p)
        {
            MysqlConnect();
            string query = "UPDATE `case_tag` SET `f_case_id` = '" + p.f_case_id + ", `f_tag_id` = '" + p.f_tag_id + "' WHERE `case_tag`.`case_tag_id` = " + p.case_tag_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Delete(int id)
        {
            MysqlConnect();
            string query = "Delete from `case_tag`  where case_tag_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }
        public void Delete(Case_tagClass p)
        {
            MysqlConnect();
            string query = "Delete from `case_tag`  where case_tag_id=" + p.case_tag_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }


        public bool isExist(Case_tagClass p)
        {
            MysqlConnect();
            string query = "select * from case_tag where f_case_id='" + p.f_case_id + "' and f_tag_id='" + p.f_tag_id + "'";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            bool flg = dr.HasRows;
            mysql.Close();
            if (flg) return true;
            return false;
        }
        public bool isCaseIdExist(int case_id)
        {
            MysqlConnect();
            string query = "select * from case_tag where f_case_id='" + case_id+ "'";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            bool flg = dr.HasRows;
            mysql.Close();
            if (flg) return true;
            return false;
        }
    }
}