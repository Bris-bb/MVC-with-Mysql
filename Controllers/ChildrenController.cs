using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;


namespace WebApplication5.Controllers
{
    public class ChildrenController : Controller
    {
        public ChildrenClass modelClass = new ChildrenClass();

        // GET: Children
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
        // GET: Children/Details/5
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

        // GET: Children/Create
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

            parentClass p = new parentClass();
            ViewData["list_parent"] = new SelectList(p.Read(), "parent_id", "parent_name");
            
            return View();
        }

        // POST: Children/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "child_id,child_name, f_parent_id")] ChildrenClass child)
        {
            ViewBag.isExist = "NO";
            if (modelClass.isExist(child))
            {
                //when isExist child_name
                ViewBag.isExist = "Same child Name is exist already.";
            }

            else
            {
                if (ModelState.IsValid)
                {
                    modelClass.Add(child);
                    return RedirectToAction("Index");
                }
            }
            parentClass p = new parentClass();
            ViewData["list_parent"] = new SelectList(p.Read(), "parent_id", "parent_name");

            return View();
        }



        // GET: Children/Delete/5
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

        // POST: Children/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelClass.Delete((int)id);
            return RedirectToAction("Index");
        }


        // GET: Children/Edit/5
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

            //ViewBag.f_parent_id = new SelectList(db.parents, "parent_id", "parent_name", child.f_parent_id);
            return View(modelClass.DetailWithParent((int)id));
        }

        // POST: Children/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "child_id,child_name,list_parent")] ChildrenClass child)
        {
            ViewBag.isExist = "NO";
            if (modelClass.isExist(child))
            {
                ViewBag.isExist = "Same child Name is exist already.";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    modelClass.Edit(child);
                    return RedirectToAction("Index");
                }
            }
            return View(modelClass.DetailWithParent((int)child.child_id));
        }
    }
}