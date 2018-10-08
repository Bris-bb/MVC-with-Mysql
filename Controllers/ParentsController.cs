using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Net;

namespace WebApplication5.Controllers
{
    public class ParentsController : Controller
    {
        public parentClass modelClass = new parentClass();

        // GET: Parents
        public ActionResult Index()
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    return RedirectToAction("NoRole", "Users");
                }
            }
            return View(modelClass.Read());
        }
        // GET: parents/Details/5
        public ActionResult Details(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    return RedirectToAction("NoRole", "Users");
                }
            }
            return View(modelClass.Detail((int)id));
        }

        // GET: parents/Create
     
        public ActionResult Create()
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    return RedirectToAction("NoRole", "Users");
                }
            }
            return View();
        }

        // POST: parents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "parent_id,parent_name")] parentClass parent)
        {

            ViewBag.isExist = "NO";
            if (modelClass.isExist(parent))
            {

                //when isExist parent_name
                ViewBag.isExist = "Same Parent Name is exist already.";
            }

            else
            {

                if (ModelState.IsValid)
                {
                    modelClass.Add(parent);
                    return RedirectToAction("Index");
                }
            }
            return View(parent);
        }



        // GET: parents/Delete/5
        public ActionResult Delete(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    return RedirectToAction("NoRole", "Users");
                }
            }
            return View(modelClass.Detail((int)id));
        }

        // POST: parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelClass.Delete((int)id);
            return RedirectToAction("Index");
        }


        // GET: parents/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((Object)Session["as_role"] == null)
            {
                if ((Object)Session["Login"] == null)
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    return RedirectToAction("NoRole", "Users");
                }
            }
            return View(modelClass.Detail((int)id));
        }

        // POST: parents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "parent_id,parent_name")] parentClass parent)
        {
            ViewBag.isExist = "NO";
            if (modelClass.isExist(parent))
            {
                ViewBag.isExist = "Same Parent Name is exist already.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    modelClass.Edit(parent);
                    return RedirectToAction("Index");
                }
            }
            return View(parent);
        }
    }
}