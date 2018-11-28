using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tech_Forum.Models;

namespace Tech_Forum.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(string UserID,string Password,string Login)
        {
            if (Login != null)
            {
                using (DbAccessEntity db = new DbAccessEntity())
                {
                    var user = db.Admin_Table.Where(x => x.Username == UserID && x.Password == Password).FirstOrDefault();

                    if (user != null)
                    {
                        return View("GenerateUserReport");
                    }
                    else
                    {
                        return View("Index", null,model: "Wrong Credentials");
                    }
                }
            }
            else
            {
                return View();
            }

            
        }

        public ActionResult GenerateOverallReport()
        {
            return View();
        }

        public ActionResult GenerateCategoryReport(string domlist, TestUser u, String show, string showtech, string technolist)
        {
            if (show != null)
            {
                DbAccessEntity tec = new DbAccessEntity();
                var domainlist = (from p in tec.Post_Table
                                  where p.domain == domlist
                                  select new
                                  {
                                      domain = p.domain,
                                      Title = p.title,

                                      UserID = p.userid

                                  }).ToList();
                Session["domainlist"] = domlist;
                u.domainsession = Session["domainlist"].ToString();
                List<QuestionBank> li = new List<QuestionBank>();
                int i = 0;
                foreach (var p in domainlist)
                {

                    u.Domainlist[i] = p.domain;
                    u.Title[i] = p.Title;
                    u.UserId[i] = p.UserID;
                    i++;
                }
                u.count = i;
                var techlist = (from p in tec.Technology_Table
                                join q in tec.Domain_Table
                                on p.did equals q.did
                                where q.domain == domlist
                                select new
                                {
                                    Technology = p.technology
                                }).ToList();
                i = 0;
                foreach (var p in techlist)
                {
                    u.Technologydroplist[i] = p.Technology;
                    i++;
                }
                u.techcount = i;


                i = 0;

                var domainvar = (from p in tec.Domain_Table
                                 select p.domain);
                var count = (from p in tec.Domain_Table
                             select p.domain).Count();
                u.Domaincount = count;
                foreach (var p in domainvar)
                {
                    u.Domaindroplist[i] = p;
                    i++;
                }
                return View("GenerateCategoryReport", u);


            }
            else if (showtech != null)
            {
                DbAccessEntity tec = new DbAccessEntity();
                u.techsession = technolist;
                var techlist = (from p in tec.Technology_Table
                                join q in tec.Post_Table
                                on p.technology equals q.technology
                                where p.technology == technolist
                                select new
                                {
                                    technology = p.technology,
                                    UserID = q.userid,
                                    Title = q.title,


                                }).ToList();

                List<QuestionBank> li = new List<QuestionBank>();
                int i = 0;
                foreach (var p in techlist)
                {

                    u.Technologylist[i] = p.technology;
                    u.UserId[i] = p.UserID;
                    u.Title[i] = p.Title;

                    i++;
                }
                u.count = i;

                i = 0;

                var domainvar = (from p in tec.Domain_Table
                                 select p.domain);
                var count = (from p in tec.Domain_Table
                             select p.domain).Count();
                u.Domaincount = count;
                foreach (var p in domainvar)
                {
                    u.Domaindroplist[i] = p;
                    i++;
                }
                u.domainsession = Session["domainlist"].ToString();
                var techdropdownlist = (from p in tec.Domain_Table
                                        join q in tec.Technology_Table
                                        on p.did equals q.did
                                        where p.domain == u.domainsession
                                        select new
                                        {
                                            Technology = q.technology
                                        }).ToList();
                i = 0;
                foreach (var p in techdropdownlist)
                {
                    u.Technologydroplist[i] = p.Technology;
                    i++;
                }
                u.techcount = i;

                return View("GenerateCategoryReport", u);

            }
            else
            {

                int i = 0;
                DbAccessEntity tz = new DbAccessEntity();
                var domainvar = (from p in tz.Domain_Table
                                 select p.domain);
                var count = (from p in tz.Domain_Table
                             select p.domain).Count();
                u.Domaincount = count;
                foreach (var p in domainvar)
                {
                    u.Domaindroplist[i] = p;
                    i++;
                }
                ViewBag.domainlistname = u.Domainlist;
                return View(u);
            }
        }


        public ActionResult DomainSelect(string domlist, TestUser u, String show, string showtech, string technolist)
        {
            if (show != null)
            {
                DbAccessEntity tec = new DbAccessEntity();
                var domainlist = (from p in tec.Domain_Table
                                  join q in tec.Test_Table
                                  on p.did equals q.DomainID
                                  where p.domain == domlist
                                  select new
                                  {
                                      domain = p.domain,
                                      UserID = q.UserId,
                                      TestID = q.TestId,
                                      Score = q.Score

                                  }).ToList();
                Session["domainlist"] = domlist;
                u.domainsession = Session["domainlist"].ToString();
                List<QuestionBank> li = new List<QuestionBank>();
                int i = 0;
                foreach (var p in domainlist)
                {

                    u.Domainlist[i] = p.domain;
                    u.UserId[i] = p.UserID;
                    u.TestId[i] = p.TestID;
                    u.Score[i] = p.Score;
                    i++;
                }
                u.count = i;
                var techlist = (from p in tec.Domain_Table
                                join q in tec.Technology_Table
                                on p.did equals q.did
                                where p.domain == domlist
                                select new
                                {
                                    Technology = q.technology
                                }).ToList();
                i = 0;
                foreach (var p in techlist)
                {
                    u.Technologydroplist[i] = p.Technology;
                    i++;
                }
                u.techcount = i;


                i = 0;

                var domainvar = (from p in tec.Domain_Table
                                 select p.domain);
                var count = (from p in tec.Domain_Table
                             select p.domain).Count();
                u.Domaincount = count;
                foreach (var p in domainvar)
                {
                    u.Domaindroplist[i] = p;
                    i++;
                }
                return View("GenerateTestReport", u);


            }
            else if (showtech != null)
            {
                DbAccessEntity tec = new DbAccessEntity();
                u.techsession = technolist;
                var techlist = (from p in tec.Technology_Table
                                join q in tec.Test_Table
                                on p.tid equals q.TechnologyID
                                where p.technology == technolist
                                select new
                                {
                                    technology = p.technology,
                                    UserID = q.UserId,
                                    TestID = q.TestId,
                                    Score = q.Score

                                }).ToList();

                List<QuestionBank> li = new List<QuestionBank>();
                int i = 0;
                foreach (var p in techlist)
                {

                    u.Technologylist[i] = p.technology;
                    u.UserId[i] = p.UserID;
                    u.TestId[i] = p.TestID;
                    u.Score[i] = p.Score;
                    i++;
                }
                u.count = i;

                i = 0;

                var domainvar = (from p in tec.Domain_Table
                                 select p.domain);
                var count = (from p in tec.Domain_Table
                             select p.domain).Count();
                u.Domaincount = count;
                foreach (var p in domainvar)
                {
                    u.Domaindroplist[i] = p;
                    i++;
                }
                u.domainsession = Session["domainlist"].ToString();
                var techdropdownlist = (from p in tec.Domain_Table
                                        join q in tec.Technology_Table
                                        on p.did equals q.did
                                        where p.domain == u.domainsession
                                        select new
                                        {
                                            Technology = q.technology
                                        }).ToList();
                i = 0;
                foreach (var p in techdropdownlist)
                {
                    u.Technologydroplist[i] = p.Technology;
                    i++;
                }
                u.techcount = i;

                return View("GenerateTestReport", u);

            }
            return View("GenerateTestReport");
        }

        public ActionResult GenerateTestReport(TestUser u, string answer, string inp, string answercat, string Search)
        {


            int i = 0;
            DbAccessEntity tec = new DbAccessEntity();
            var domainvar = (from p in tec.Domain_Table
                             select p.domain);
            var count = (from p in tec.Domain_Table
                         select p.domain).Count();
            u.Domaincount = count;
            foreach (var p in domainvar)
            {
                u.Domaindroplist[i] = p;
                i++;
            }
            ViewBag.domainlistname = u.Domainlist;
            return View(u);


        }

        public ActionResult GenerateUserReport(string answer, string inp, Subscriber_Table si,Post_Table ai, TestUser u)
        {
            if (answer != null)
            {
                DbAccessEntity te = new DbAccessEntity();

                var testsub = (from p in te.Subscriber_Table
                               join t in te.Test_Table
                               on p.userid equals t.UserId
                               where p.userid == inp
                               select new
                               {
                                   ID = p.userid,
                                   Domain = t.DomainID,
                                   Score = t.Score,
                                   Technology = t.TechnologyID,
                                   TesTID = t.TestId,


                               }).ToList();
                int i = 0;
                foreach (var p in testsub)
                {
                    u.Name = p.ID;
                    u.Domain[i] = p.Domain;
                    u.UserId[i] = p.ID;
                    u.Score[i] = p.Score;
                    u.TestId[i] = p.TesTID;

                    if (u.Score[i] <= (0.4) * 10)
                    {
                        u.fail = u.fail + 1;
                    }
                    else
                    {
                        u.pass = u.pass + 1;
                    }
                    i++;

                }
                u.counttest = i;


                var artsubq = (from p in te.Subscriber_Table
                               join e in te.Post_Table
                               on p.userid equals e.userid
                               where p.userid == inp
                               select new
                               {
                                   ID = p.userid,
                                   Name=p.name,
                                   Title = e.title,
                                   Rating = e.rating,
                                   Technology = e.technology
                               }).ToList();
                i = 0;
                double sum = 0;
                foreach (var p in artsubq)
                {


                    u.Title[i] = p.Title;
                    u.Technologylist[i] = p.Technology;
                    
                    i++;
                }
                u.countarticle = i;
          
                if (u.countarticle > 0)
                    u.averagerating = (sum / u.countarticle);
                else
                    u.averagerating = 0;
                return View(u);


            }
            return View();
        }

       
    }
}