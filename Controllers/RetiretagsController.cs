using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class RetiretagsController : Controller
    {
        public RetiretagsClass modelClass = new RetiretagsClass();

        // GET: Tags
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
            return View(modelClass.Read());
        }

        // GET: Tags/Retire/5
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

        // POST: Tags/Retire/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelClass.Delete( modelClass.Detail((int)id) );
            return RedirectToAction("Index");
        }

        // GET: Tags/Active/5
        public ActionResult Active(int? id)
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

        // POST: Tags/Active/5
        [HttpPost, ActionName("Active")]
        [ValidateAntiForgeryToken]
        public ActionResult ActiveConfirmed(int id)
        {
            modelClass.Active((int)id);
            return RedirectToAction("Index");
        }
    }
}