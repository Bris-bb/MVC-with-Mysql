using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using WebApplication5.Models;
using Newtonsoft.Json;
using RestSharp;

namespace WebApplication5.Controllers
{
    public class TcasesController : Controller
    {
        public TcaseClass modelClass = new TcaseClass();
        public Case_tagClass case_tagClass = new Case_tagClass();
        public TcaseClass tcases = new TcaseClass();
        
		public ActionResult Retire(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }

            }
            return View(modelClass.Detail((int)id));
        }


        [HttpPost, ActionName("Retire")]
        [ValidateAntiForgeryToken]
        public ActionResult RetireConfirmed(int id)
        {
            modelClass.Retire(modelClass.Detail((int)id));
            return RedirectToAction("Index");
        }
        public class Test
        {
            public int id { get; set; }
            public string title { get; set; }
            public int section_id { get; set; }
            public int type_id { get; set; }
            public int priority_id { get; set; }
            public string milestone_id { get; set; }
            public string refs { get; set; }
            public int created_by { get; set; }
            public int created_on { get; set; }
            public int updated_by { get; set; }
            public int updated_on { get; set; }
            public string estimate { get; set; }
            public string estimate_forecast { get; set; }
            public int suite_id { get; set; }
            public bool custom_isautomated { get; set; }
            public string custom_preconds { get; set; }
            public string custom_steps { get; set; }
            public string custom_expected { get; set; }

        }

        public ActionResult RefreshResults()
        {
            Dictionary<int, string> _ApiIDs = new Dictionary<int, string>();
            Dictionary<int, string> dbResults = new Dictionary<int, string>();
            int increment1 = 0;
            int increment2 = 0;

            var oldIDslst = new List<string>();
            var newIDslst = new List<string>();

            //var client = new RestClient("http://thelab/testrail/index.php?%2Fapi%2Fv2%2Fget_cases%2F17593=&suite_id=195455");
            //var request = new RestRequest(Method.GET);
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("authorization", "Basic" );
            //request.AddHeader("content-type", "application/json");
            //IRestResponse response = client.Execute(request);

            string testRailURL = "http://thelab/testrail/index.php?%2Fapi%2Fv2%2Fget_cases%2F17593=&suite_id=195455";
            var webRequest = (HttpWebRequest)WebRequest.Create(testRailURL);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/json";

            string auth = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    String.Format(
                        "{0}:{1}",
                        "Shaylen.Ramoutar@derivco.co.za",
                        "EaODbGbT6QQ1vk4vHiSN-kffBbMguuuWvV7TJEpKG"

                    )
                )
            );

            webRequest.Headers.Add("Authorization", "Basic " + auth);

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            string text = "";
            if (response != null)
            {
                var reader = new StreamReader(
                    response.GetResponseStream(),
                    Encoding.UTF8
                );

                using (reader)
                {
                    text = reader.ReadToEnd();
                }
            }

            dynamic _ApiRawData = JsonConvert.DeserializeObject<List<Test>>(text);

            foreach (Test item in _ApiRawData)
            {
                _ApiIDs.Add(increment1, "C" + item.id);
                increment1++;
            }

            
            foreach (var item in tcases.Read())
            {
                dbResults.Add(increment2, item.case_id_name);
                increment2++;
            }

            foreach (var item in dbResults.Values)
            {
                if (!_ApiIDs.ContainsValue(item))
                {
                    tcases.RetireTcase(item);
                    oldIDslst.Add(item);
                }

            }

            foreach (var item in _ApiIDs.Values)
            {
                if (!dbResults.ContainsValue(item))
                {
                    tcases.AddTCase(item);
                    newIDslst.Add(item);
                }

            }

            if (oldIDslst.Count != 0)
            {
                string alertmsg = string.Join<string>(",", oldIDslst);
                TempData["oldAlertMessage"] = string.Format("The Following Testcase IDs have been Retired: {0}" ,alertmsg);
            }

            if (newIDslst.Count != 0)
            {
                string alertmsg = string.Join<string>("", newIDslst);
                TempData["NewAlertMessage"] = string.Format("The Following Testcase IDs have been Added: {0}", alertmsg);
            }


         return RedirectToAction("Index", "Tcases");
        }

        [HttpPost]
        public JsonResult GetUntaggedTcases()
        {
            return Json(modelClass.GetUntaggedTcases(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTestCase()
        {
            return Json(modelClass.Read(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SaveRelationTagsAndTcases(int case_id, int tag_id, string state)
        {
            if (state == "true")
            {
                case_tagClass.Add(new Case_tagClass
                {
                    f_case_id = case_id,
                    f_tag_id = tag_id
                });
            }
            else
            {
                case_tagClass.Delete(new Case_tagClass
                {
                    f_case_id = case_id,
                    f_tag_id = tag_id
                });
            }
            return;
        }

        
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "Filter");
            //}

            //var case_tag = db.case_tag.ToList();
            //db.case_tag.RemoveRange(case_tag);
            //db.SaveChanges();
            Case_tagClass ct = new Case_tagClass();
            ct.DeleteAll();

            string filePath = string.Empty;
            if (postedFile != null)
            {
                //table tcase drop
                modelClass.DeleteAll();

                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);

                int len = 0;
                int i = 0;
                foreach (string row in csvData.Split('\n'))
                {
                    if (row.Contains("\""))
                    {
                        string[] str = row.Split('\"');
                        len += str.Length;
                    }

                    if (i != 0)
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            //dt.Rows.Add();
                            //Execute a loop over the columns.
                            string[] cell = row.Split(',');
                            //dt.Rows[dt.Rows.Count - 1][0] = cell[0];
                            string ss = len.ToString();
                            modelClass.Add(new TcaseClass
                            {
                                case_id_name = cell[0]
                            });
                        }
                    }
                    i++;
                }
            }

            return View(modelClass.Read());
        }

        // GET: cases
        public ActionResult Index()
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            TagsClass tc = new TagsClass();
            ViewBag.Tags = tc.Read();
            //ViewBag.Case_tags = case_tagClass.Read();

            //case_tagClass.Read().Any(cus => cus.f_case_id == 1);
            return View(modelClass.Read());
        }
        public JsonResult GetData()
        {
            return Json(new { data = modelClass.GetData() }, JsonRequestBehavior.AllowGet);
        }
            // GET: cases/Details/5
            public ActionResult Details(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            return View(modelClass.Detail((int)id));
        }

        // GET: cases/Create
        public ActionResult Create()
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            return View();
        }

        // POST: cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "case_id,case_id_name")] TcaseClass tcase)
        {
            ViewBag.isExist = "NO";
            if (modelClass.isExist(tcase))
            {
                //when isExist case_id_name
                ViewBag.isExist = "Same case Name is exist already.";
            }

            else
            {
                if (ModelState.IsValid)
                {
                    modelClass.Add(tcase);
                    return RedirectToAction("Index");
                }
            }
            return View(tcase);
        }



        // GET: cases/Delete/5
        public ActionResult Delete(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            return View(modelClass.Detail((int)id));
        }

        // POST: cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelClass.Delete((int)id);
            return RedirectToAction("Index");
        }


        // GET: cases/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                //else
                //{
                //    return RedirectToAction("NoRole", "Users");
                //}
            }
            return View(modelClass.Detail((int)id));
        }

        // POST: cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "case_id,case_id_name")] TcaseClass tcase)
        {
            ViewBag.isExist = "NO";
            if (modelClass.isExist(tcase))
            {
                ViewBag.isExist = "Same case Name is exist already.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    modelClass.Edit(tcase);
                    return RedirectToAction("Index");
                }
            }
            return View(tcase);
        }
    }
}