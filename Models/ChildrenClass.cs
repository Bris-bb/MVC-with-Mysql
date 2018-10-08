using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Net;
using System.Web.Mvc;

namespace WebApplication5.Models
{
    public class ChildrenClass
    {
        public int child_id { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string child_name { get; set; }
        public int f_parent_id { get; set; }
        public string parent_name { get; private set; }
        public virtual parentClass p_class { get; set; }
        public IEnumerable<SelectListItem> list_parent { get; set; }

        public MySqlConnection mysql;

        public void MysqlConnect()
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            mysql = new MySqlConnection(mainconn);
        }

        //ViewBag.f_parent_id = new SelectList(db.parents, "parent_id", "parent_name", child.f_parent_id);
        //ViewBag.f_parent_id = modelClass.parent_list();

        public List<ChildrenClass> GetChildren(int f_parent_id)
        {
            MysqlConnect();
            List<ChildrenClass> list1 = new List<ChildrenClass>();
            string query = "select * from child join parent on parent.parent_id = child.f_parent_id where f_parent_id=" + f_parent_id;
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new ChildrenClass
                {
                    child_id = (int)dr["child_id"],
                    child_name = dr["child_name"].ToString(),
                    f_parent_id = (int)dr["f_parent_id"],
                    parent_name = dr["parent_name"].ToString()
                });
            }
            mysql.Close();
            return list1;
        }


        public ChildrenClass Detail(int id)
        {
            MysqlConnect();
            string query = "select * from child join parent on parent.parent_id = child.f_parent_id where child_id = "+id;
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();

            ChildrenClass x=  new ChildrenClass
            {
                child_id = (int)dr["child_id"],
                f_parent_id = (int)dr["f_parent_id"],
                child_name = dr["child_name"].ToString(),
                    parent_name = dr["parent_name"].ToString()
            };
            mysql.Close();
            return x;
        }
        public ChildrenClass DetailWithParent(int id)
        {
            MysqlConnect();
            string query = "select * from child join parent on parent.parent_id = child.f_parent_id where child_id = " + id;
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();

            parentClass p = new parentClass();
            ChildrenClass x = new ChildrenClass
            {
                child_id = (int)dr["child_id"],
                f_parent_id = (int)dr["f_parent_id"],
                child_name = dr["child_name"].ToString(),
                parent_name = dr["parent_name"].ToString(),
                list_parent = new SelectList(p.Read(), "parent_id", "parent_name", (int)dr["f_parent_id"])
            };
            mysql.Close();
            return x;
        }

        public List<ChildrenClass> Read()
        {
            MysqlConnect();
            List<ChildrenClass> list1 = new List<ChildrenClass>();
            string query = "select * from child join parent on parent.parent_id = child.f_parent_id";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new ChildrenClass
                {
                    child_id = (int)dr["child_id"],
                    child_name = dr["child_name"].ToString(),
                    f_parent_id = (int)dr["f_parent_id"],
                    parent_name = dr["parent_name"].ToString()
                });
            }
            mysql.Close();
            return list1;
        }

        public void Add(ChildrenClass p)
        {
            MysqlConnect();
            string query = "INSERT INTO `child` (`child_id`, `child_name`, `f_parent_id`) VALUES(NULL, '" + p.child_name + "', "+ p.f_parent_id+")";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Edit(ChildrenClass p)
        {
            MysqlConnect();

            string query = "UPDATE `child` SET `child_name` = '" + p.child_name + "' WHERE `child`.`child_id` = " + p.child_id;
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
            string query = "Delete from `child`  where child_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }


        public bool isExist(ChildrenClass p)
        {
            MysqlConnect();
            string query = "select * from child where child_name='" + p.child_name + "'";
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