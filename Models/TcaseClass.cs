using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Net;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class TcaseClass
    {
        [Required(ErrorMessage = "This field can not be empty.")]
        public int case_id { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string case_id_name { get; set; }
        public string code { get; set; }
		public int retire { get; set; }
        public MySqlConnection mysql;

        public void Retire(TcaseClass p)
        {
            MysqlConnect();
            string query = "UPDATE `tcase` SET `retire` = 1 WHERE `tcase`.`case_id` = " + p.case_id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void MysqlConnect()
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            mysql = new MySqlConnection(mainconn);
        }

        public List<List<string>> GetData()
        {
            List<List<string>> rtn_list = new List<List<string>>();
            Case_tagClass ct = new Case_tagClass();
            List<Case_tagClass> list = ct.Read();
            TcaseClass tc = new TcaseClass();
            TagsClass tags = new TagsClass();

            
            foreach (var item in tc.Read())
            {
                List<string> myArrayList = new List<string>();
                myArrayList.Add(item.case_id_name);
                myArrayList.Add(item.case_id.ToString());
                int i = 0;
                foreach (var item1 in tags.Read())
                {
                    if (i==0) myArrayList.Add(item1.tag_id.ToString());
                    i++;
                    if (list.Any(cus => cus.f_case_id == item.case_id && cus.f_tag_id == item1.tag_id))
                    {
                        myArrayList.Add("checked");
                    }
                    else {
                        myArrayList.Add("");
                    }
                }
                rtn_list.Add(myArrayList);
            }
            return rtn_list;
        }

        public void RetireTcase(string tcase_name)
        {
            MysqlConnect();
            string query = "UPDATE `tcase` SET `retire` = 1 WHERE `tcase`.`case_id_name` =" + '"' + tcase_name + '"';
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }


        public void AddTCase(string id)
        {
            MysqlConnect();
            string query = "INSERT INTO `tcase` (`case_id`, `case_id_name`) VALUES(NULL, '" + id + "');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public List<TcaseClass> GetUntaggedTcases()
        {
            MysqlConnect();
            List<TcaseClass> list1 = new List<TcaseClass>();
            string query = "select tcase.* from tcase left join case_tag on case_tag.f_case_id=tcase.case_id where tcase.retire=0 and f_case_id is null order by case_id_name";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new TcaseClass
                {
                    case_id = (int)dr["case_id"],
                    case_id_name = dr["case_id_name"].ToString()
                });
            }
            mysql.Close();
            return list1;
        }

        public void DeleteAll()
        {
            MysqlConnect();
            string query = "DELETE FROM `tcase`";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            comm.ExecuteNonQuery();
            mysql.Close();
            return ;
        }

        public TcaseClass Detail(int id)
        {
            MysqlConnect();
            string query = "select * from tcase where retire=0 and case_id="+ id;
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            TcaseClass x =
            new TcaseClass
            {
                case_id = (int)dr["case_id"],
                case_id_name = dr["case_id_name"].ToString()
            };
            mysql.Close();

            return x;
        }
        public List<TcaseClass> Read()
        {
            MysqlConnect();
            List<TcaseClass> list1 = new List<TcaseClass>();
             string query = "select * from tcase where retire=0 order by case_id_name";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new TcaseClass
                {
                    case_id = (int)dr["case_id"],
                    case_id_name = dr["case_id_name"].ToString()
                });
            }
            mysql.Close();
            return list1;
        }

        public void Add(TcaseClass p)
        {
            MysqlConnect();
            string query = "INSERT INTO `tcase` (`case_id`, `case_id_name`) VALUES(NULL, '" + p.case_id_name + "');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Edit(TcaseClass p)
        {
            MysqlConnect();

            string query = "UPDATE `tcase` SET `case_id_name` = '" + p.case_id_name + "' WHERE `tcase`.`case_id` = " + p.case_id;
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
            string query = "Delete from `tcase`  where case_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }


        public bool isExist(TcaseClass p)
        {
            MysqlConnect();
            string query = "select * from tcase where case_id_name='" + p.case_id_name + "'";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            bool flg = dr.HasRows;
            mysql.Close();
            if (flg) return true;
            return false;
        }
		public bool isRealExist(TcaseClass p)
        {
            MysqlConnect();
            string query = "select * from tcase where retire=0 and case_id_name='" + p.case_id_name + "'";
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