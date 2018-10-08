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
    public class RetireTcasesClass
    {
        public int case_id { get; set; }
        public string case_id_name { get; set; }
        public MySqlConnection mysql;

        public void MysqlConnect()
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            mysql = new MySqlConnection(mainconn);
        }


        public RetireTcasesClass Detail(int id)
        {
            MysqlConnect();
            string query = "select * from tcase where retire=1 and case_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            RetireTcasesClass x = new RetireTcasesClass
            {
                case_id = (int) dr["case_id"],
                case_id_name = dr["case_id_name"].ToString()
            };
            mysql.Close();
            return x;
        }

        public RetireTcasesClass Detail(string case_id_name)
        {
            MysqlConnect();
            string query = "select * from tcase where retire=1 and case_id_name='" + case_id_name + "'";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            RetireTcasesClass x = new RetireTcasesClass
            {
                case_id = (int)dr["case_id"],
                case_id_name = dr["case_id_name"].ToString()
            };
            mysql.Close();
            return x;
        }


        public List<RetireTcasesClass> Read()
        {
            MysqlConnect();
            List<RetireTcasesClass> list1 = new List<RetireTcasesClass>();
            string query = "select * from tcase where retire=1";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new RetireTcasesClass
                {
                    case_id = (int)dr["case_id"],
                    case_id_name = dr["case_id_name"].ToString()
                });
            }
            mysql.Close();
            return list1;
        }

        public void Delete(RetireTcasesClass p)
        {
            MysqlConnect();
            string query = "Delete from `tcase`  where case_id=" + p.case_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Delete(int case_id)
        {
            MysqlConnect();
            string query = "Delete from `tcase`  where case_id=" + case_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Active(int case_id)
        {
            MysqlConnect();
            string query = "UPDATE `tcase` SET `retire` = 0 WHERE `tcase`.`case_id` = " + case_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }
    }
}