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
    public class TagsClass
    {
        public int tag_id { get; set; }
        public string tag_name { get; set; }
        public int retire { get; set; }
        public int used_number { get; set; }

        public MySqlConnection mysql;

        public void MysqlConnect()
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            mysql = new MySqlConnection(mainconn);
        }

        public TagsClass Detail(int id)
        {
            MysqlConnect();
            string query = "select * from tag where retire=0 and tag_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            TagsClass x= new TagsClass
            {
                tag_id = (int)dr["tag_id"],
                tag_name = dr["tag_name"].ToString()
            };
            mysql.Close();
            return x;
        }
        public TagsClass Detail(string tag_name)
        {
            MysqlConnect();
            string query = "select * from tag where retire=0 and tag_name='" + tag_name+"'";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            TagsClass x = new TagsClass
            {
                tag_id = (int)dr["tag_id"],
                tag_name = dr["tag_name"].ToString()
            };
            mysql.Close();
            return x;
        }
        
        public List<TagsClass> ReadWithUsedNumber()
        {
            MysqlConnect();
            List<TagsClass> list1 = new List<TagsClass>();
            string query = "select tag.*, count(tag_id) as usedNumber from tag join case_tag as ct on ct.f_tag_id=tag.tag_id where retire=0 GROUP by tag_id"; // order by tag.tag_id
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new TagsClass
                {
                    tag_id = (int)dr["tag_id"],
                    tag_name = dr["tag_name"].ToString(),
                    used_number = dr.GetInt32(3)
                });
            }
            mysql.Close();
            return list1;
        }
        public List<TagsClass> Read()
        {
            MysqlConnect();
            List<TagsClass> list1 = new List<TagsClass>();
            string query = "select * from tag where retire=0"; // order by tag.tag_id
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new TagsClass
                {
                    tag_id = (int)dr["tag_id"],
                    tag_name = dr["tag_name"].ToString()
                });
            }
            mysql.Close();
            return list1;
        }



        public void Add(TagsClass p)
        {
            MysqlConnect();
            string query = "INSERT INTO `tag` (`tag_id`, `tag_name`, `retire`) VALUES(NULL, '" + p.tag_name + "', 0);";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Edit(TagsClass p)
        {
            MysqlConnect();

            string query = "UPDATE `tag` SET `tag_name` = '" + p.tag_name + "' WHERE `tag`.`tag_id` = " + p.tag_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Retire(TagsClass p)
        {
            MysqlConnect();
            string query = "UPDATE `tag` SET `retire` = 1 WHERE `tag`.`tag_id` = " + p.tag_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        


        public bool isExist(TagsClass p)
        {
            MysqlConnect();
            string query = "select * from tag where tag_name='" + p.tag_name + "'";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            bool flg = dr.HasRows;
            mysql.Close();
            if (flg) return true;
            return false;
        }



        public bool isRealExist(TagsClass p)
        {
            MysqlConnect();
            string query = "select * from tag where retire=0 and tag_name='" + p.tag_name + "'";
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