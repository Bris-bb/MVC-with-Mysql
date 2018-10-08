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
    public enum roleEnum { Guset, AdminJunior, AdminSenior, SuperAdmin };
    //{
    //    [Display(Name = "Guest")]
    //    Guest,
    //    [Display(Name = "Admin Junior")]
    //    AJ,
    //    [Display(Name = "Admin Senior")]
    //    AS
    //}
    public class UserClass
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string pwd { get; set; }
        public string role { get; set; }

        public MySqlConnection mysql;

        public void MysqlConnect()
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            mysql = new MySqlConnection(mainconn);
        } 


        public UserClass Detail(int id)
        {
            MysqlConnect();
            string query = "select * from user where user_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            UserClass x =
            new UserClass
            {
                user_id = (int)dr["user_id"],
                    role = dr["role"].ToString(),
                pwd = dr["pwd"].ToString(),
                user_name = dr["user_name"].ToString()
            };
            mysql.Close();

            return x;
        }
        public UserClass Detail(string user_name)
        {
            MysqlConnect();
            string query = "select * from user where user_name='" + user_name + "'";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            UserClass x =
            new UserClass
            {
                user_id = (int)dr["user_id"],
                role = dr["role"].ToString(),
                pwd = dr["pwd"].ToString(),
                user_name = dr["user_name"].ToString()
            };
            mysql.Close();

            return x;
        }
        public List<UserClass> Read()
        {
            MysqlConnect();
            List<UserClass> list1 = new List<UserClass>();
            string query = "select * from user";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new UserClass
                {
                    user_id = (int)dr["user_id"],
                    role = dr["role"].ToString(),
                    pwd = dr["pwd"].ToString(),
                    user_name = dr["user_name"].ToString()
                });
            }
            mysql.Close();
            return list1;
        }

        public void Add(UserClass p)
        {
            MysqlConnect();
            string query = "INSERT INTO `user` (`user_id`, `user_name`, `pwd`, `role`) VALUES(NULL, '" + p.user_name + "', '" + p.pwd + "' , 'Guest');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Edit(UserClass p)
        {
            MysqlConnect();

            string query = "UPDATE `user` SET `user_name` = '" + p.user_name + "', `pwd` = '" + p.pwd + "', `role` = '" + p.role + "' WHERE `user`.`user_id` = " + p.user_id;
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
            string query = "Delete from `user`  where user_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }


        public bool isExist(UserClass p)
        {
            MysqlConnect();
            string query = "select * from user where user_id!="+p.user_id+" and user_name='" + p.user_name + "'";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            bool flg = dr.HasRows;
            mysql.Close();
            if (flg) return true;
            return false;
        }

        public bool isUser(UserClass p)
        {
            MysqlConnect();
            string query = "select * from user where user_name='" + p.user_name + "' and pwd='" + p.pwd + "'";
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