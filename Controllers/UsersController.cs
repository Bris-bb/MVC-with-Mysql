using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class UsersController : Controller
    {
        public UserClass modelClass = new UserClass();

        [HttpGet]
        public ActionResult NoRole()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if ((Object)Session["Login"] != null)
            {
                return RedirectToAction("Index", "Filter");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "user_name,pwd")] UserClass user)
        {
            if (modelClass.isUser(user))
            {
                user = modelClass.Detail(user.user_name);
                Session["Login"] = user.user_id;
                Session["user_name"] = user.user_name;
                Session["user_id"] = user.user_id;
                Session["role"] = user.role;
                if (user.role == "SuperAdmin")
                {
                    Session["sa_role"] = user.role;
                    Session["as_role"] = user.role;
                }
                else if (user.role == "AdminSenior")
                    Session["as_role"] = user.role;
                else if (user.role == "AdminJunior")
                    Session["aj_role"] = user.role;

                return RedirectToAction("Index", "Filter");
            }
            else
            {
                Session["Login"] = null;
                Session["user_name"] = null;
                Session["user_id"] = null;
                Session["role"] = null;

                Session["sa_role"] = null;
                Session["as_role"] = null;
                Session["aj_role"] = null;

                ModelState.AddModelError("", "Login data is incorrect!");
            }
            return View(user);
        }
        public ActionResult Logout()
        {
            Session["Login"] = null;
            Session["user_name"] = null;
            Session["user_id"] = null;
            Session["role"] = null;

            Session["sa_role"] = null;
            Session["as_role"] = null;
            Session["aj_role"] = null;
            return RedirectToAction("Login", "Users");
        }

        // GET: users
        public ActionResult Index()
        {
            if ((Object)Session["Login"] == null || (Object)Session["sa_role"] == null)
            {
                return RedirectToAction("Login", "Users");
            }

            return View(modelClass.Read());
        }
        // GET: users/Details/5
        public ActionResult Details(int? id)
        {
            if ((Object)Session["Login"] == null || (Object)Session["sa_role"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            return View(modelClass.Detail((int)id));
        }

        // GET: users/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,user_name,pwd")] UserClass user)
        {
            ViewBag.isExist = "NO";
            if (modelClass.isExist(user))
            {
                //when isExist user_name
                ViewBag.isExist = "Same user name is exist already.";
            }

            else
            {
                if (ModelState.IsValid)
                {
                    modelClass.Add(user);
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: users/Delete/5
        public ActionResult Delete(int? id)
        {
            if ((Object)Session["Login"] == null || (Object)Session["sa_role"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            return View(modelClass.Detail((int)id));
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelClass.Delete((int)id);
            return RedirectToAction("Index");
        }


        // GET: users/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((Object)Session["Login"] == null)
            {
                return RedirectToAction("Login", "Users");
            }
            return View(modelClass.Detail((int)id));
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,user_name,pwd,role")] UserClass user)
        {
            ViewBag.isExist = "NO";
            if (modelClass.isExist(user))
            {
                ViewBag.isExist = "Same User Name is exist already.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    modelClass.Edit(user);
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }
    }
}