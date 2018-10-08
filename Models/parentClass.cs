using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Net;

namespace WebApplication5.Models
{
    public class parentClass
    {
        [Required(ErrorMessage = "This field can not be empty.")]
        public int parent_id { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string parent_name { get; set; }
        public MySqlConnection mysql;
        public virtual ICollection<ChildrenClass> c_class { get; set; }

        public void MysqlConnect()
        {
            string mainconn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            mysql = new MySqlConnection(mainconn);
        }

        public string GetTestCasesForParent(int parent_id)
        {
            string code = "";
            MysqlConnect();
            string query = "select tcase.* from tcase join case_tag as ct on ct.f_case_id = tcase.case_id " +
                                               "join tag on tag.tag_id = ct.f_tag_id and tag.retire = 0 " +
                                               "join child on child.child_name = tag.tag_name " +
                                               "join parent as p on p.parent_id = child.f_parent_id " +
                                                "where tcase.retire=0 and p.parent_id = " + parent_id +
                                                " group by case_id "+
                                                " order by case_id_name ";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            while (dr.Read())
            {
                code += "<tr data-real='true' data-id=" + (int)dr["case_id"] + ">" +
                         "<td>" + dr["case_id_name"].ToString() +
                         "</td>" +
                         "</tr>";
            }

            mysql.Close();

            return code;
        }

        public string GetRelationOfTestcaseAndTagCode(string[] child_names) { 
            string code = "";

            Case_tagClass case_TagClass = new Case_tagClass();
            TcaseClass tcaseClass = new TcaseClass();
            TagsClass tagsClass = new TagsClass();

            List<TcaseClass> list_case = tcaseClass.Read();


            string real_code = "";

            foreach (var case_ in list_case)
            {
                Boolean flg = false;
                code = "<tr>";
                code += "<td>";
                code += case_.case_id_name;
                code += "</td>";

                foreach (string child_name in child_names)
                {
                    string r_child_name = child_name.Split('#')[0];
                    string type = child_name.Split('#')[3];


                    TagsClass tc = new TagsClass();
                    if (tagsClass.isRealExist(new TagsClass { tag_name = r_child_name }))
                    {
                        tc = tagsClass.Detail(r_child_name);
                        if (case_TagClass.isExist(new Case_tagClass { f_tag_id = tc.tag_id, f_case_id = case_.case_id }))
                        {
                            code += "<td>Yes</td>";
                            flg = true;
                            continue;
                        }
                        else
                        {
                            code += "<td></td>";
                            if (type == "and")
                            {
                                flg = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        code += "<td>"+"</td>";
                        if (type == "and")
                        {
                            flg = false;
                            break;
                        }
                    }
                }
                code += "</tr>";

                if (flg)
                    real_code += code;
            }
            return real_code;
        }
        public parentClass Detail(int id)
        {
            MysqlConnect();
            string query = "select * from parent where parent_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();

            dr.Read();
            parentClass x= 
            new parentClass
            {
                parent_id = (int)dr["parent_id"],
                parent_name = dr["parent_name"].ToString()
            };
            mysql.Close();

            return x;
        }
        public List<parentClass> Read()
        {
            MysqlConnect();
            List<parentClass> list1 = new List<parentClass>();
            string query = "select * from parent";
            MySqlCommand comm = new MySqlCommand(query);

            comm.Connection = mysql;
            mysql.Open();

            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new parentClass
                {
                    parent_id = (int)dr["parent_id"],
                    parent_name = dr["parent_name"].ToString()
                });
            }
            mysql.Close();
            return list1;
        }

        public void Add(parentClass p)
        {
            MysqlConnect();
            string query = "INSERT INTO `parent` (`parent_id`, `parent_name`) VALUES(NULL, '"+ p.parent_name+"');";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }

        public void Edit(parentClass p)
        {
            MysqlConnect();
            
            string query = "UPDATE `parent` SET `parent_name` = '"+p.parent_name+"' WHERE `parent`.`parent_id` = " + p.parent_id;
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
            string query = "Delete from `parent`  where parent_id=" + id;
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            comm.ExecuteNonQuery();
            mysql.Close();
            return;
        }


        public bool isExist(parentClass p)
        {
            MysqlConnect();
            string query = "select * from parent where parent_name='" + p.parent_name+"'";
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