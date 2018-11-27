using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tech_Forum.Models;
using DbAccess;
using Newtonsoft.Json;
using System.Data.Entity.Migrations;

namespace Tech_Forum.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        
        public ActionResult Index()
        {
            return View();
        }

        // GET: Post/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        public ActionResult BrowseArticle()
        {
            DbAccessService dbs = new DbAccessService();
            List<Domain_Table> DomainList = dbs.GetDomainList();
            ViewBag.DomainList = new SelectList(DomainList, "did", "domain");
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult BrowseArticle(Post_Table post)
        {
            using (DbAccessEntity db = new DbAccessEntity())
            {
                DbAccessService dbs = new DbAccessService();
                List<Domain_Table> DomainList = dbs.GetDomainList();
                ViewBag.DomainList = new SelectList(DomainList, "did", "domain");

                db.Configuration.ProxyCreationEnabled = false;

                List<Technology_Table> TechnologyList = dbs.GetTechforDomain(post.domain);
                ViewBag.TechnologyList = new SelectList(TechnologyList, "tid", "technology");

                int did = Convert.ToInt32(post.domain);
                int tid = Convert.ToInt32(post.technology);

                
                var d = db.Domain_Table.Where(x => x.did == did).FirstOrDefault();
                post.domain = d.domain;
                var t = db.Technology_Table.Where(x => x.tid == tid).FirstOrDefault();
                post.technology = t.technology;

                List<Post_Table> browsearticle = db.Post_Table.Where(x => x.domain == post.domain && x.technology == post.technology && x.category == true).ToList();
                if (browsearticle.Count > 0)
                {
                    ViewData["browsearticle"] = browsearticle;
                }
                else
                {
                    ViewData["browsearticle"] = null;
                    ViewBag.Message = "No article found";
                }
            }


            return View();
        }

        // GET: Post/Create
        [Authorize]
        public ActionResult Create()
        {
            DbAccessService dbs = new DbAccessService();
            List<Domain_Table> DomainList = dbs.GetDomainList();
            ViewBag.DomainList = new SelectList(DomainList, "did", "domain");
            return View();

        }

        public JsonResult GetTechList(int did)
        {
            using (DbAccessEntity db = new DbAccessEntity())
            {
                db.Configuration.ProxyCreationEnabled = false;

                List<Technology_Table> TechList = db.Technology_Table.Where(x => x.did == did).ToList();
                return Json(TechList, JsonRequestBehavior.AllowGet);
            }

        }

        // POST: Post/Create
        [HttpPost]
        public ActionResult Create(Post_Table post,Article article)
        {
            try
            {
                using(DbAccessEntity db = new DbAccessEntity())
                {

                    DbAccessService dbs = new DbAccessService();
                    List<Domain_Table> DomainList = dbs.GetDomainList();
                    ViewBag.DomainList = new SelectList(DomainList, "did", "domain");

                    db.Configuration.ProxyCreationEnabled = false;

                    List<Technology_Table> TechnologyList = dbs.GetTechforDomain(post.domain);
                    ViewBag.TechnologyList = new SelectList(TechnologyList, "tid", "technology");

                    //Get the Domain ID
                    int did = Convert.ToInt32(article.Post.domain);
                    //Get the Technology ID
                    int tid = Convert.ToInt32(article.Post.technology);


                    //Convert the Domain ID to Domain Name
                    var d = db.Domain_Table.Where(x => x.did == did).FirstOrDefault();
                    post.domain = d.domain;

                    //Convert the Technology ID to Technology Name
                    var t = db.Technology_Table.Where(x => x.tid == tid).FirstOrDefault();
                    post.technology = t.technology;

                    //Check for same title
                    int count = db.Post_Table.Where(x => x.title == article.Post.title && x.category == true).Count();

                    if(count > 0)
                    {
                        ViewBag.ErrorMessage = "Please modify the TITLE, Article found with same TITLE";
                        return View(article);
                    }
                    else
                    {
                        post.title = article.Post.title;
                        post.tags = article.Post.tags;
                        post.content_ = article.Post.content_;
                        post.date = DateTime.Now;
                        post.category = true;
                        post.userid = Session["userid"].ToString();

                        ViewData["Article"] = post;
                        db.Post_Table.Add(post);
                        db.SaveChanges();
                        
                        return View("ResultView");
                    }
                   
                }

                    
            }
            catch
            {
                return View();
            }
        }

        public ActionResult PostComment(Post_Table postTable, Comment comment, Article article, Rate rate, string postId, string commentId, string commentContent)
        {
            try
            {
                using (DbAccessEntity pe = new DbAccessEntity())
                {
                    var postIdInt = Convert.ToInt32(postId);

                    var post = pe.Post_Table.Where(x => x.postid == postIdInt).FirstOrDefault();

                    if (post.comments != null)
                    {
                        article.comments = JsonConvert.DeserializeObject<List<Comment>>(post.comments);
                    }

                    if (post.rating != null)
                    {
                        Rate.rateList.Clear();
                        Rate.rateList = JsonConvert.DeserializeObject<List<Rate>>(post.rating);
                        rate.rating = Rate.getUserRating(Rate.rateList, Session["userid"].ToString());
                        rate.userId = Session["userid"].ToString();
                    }

                    ViewData["Rate"] = rate;
                    ViewData["AverageRating"] = Rate.calculateAverageRating(Rate.rateList);
                    ViewData["Article"] = post;
                    ViewData["Comments"] = article;

                    comment.userid = Session["userid"].ToString();
                    comment.date = DateTime.Now;

                    if (commentContent == null)
                    {
                        if (comment.content_ != null)
                        {
                            if (article.comments.Count() < 10)
                            {
                                comment.postid = "0" + (article.comments.Count() + 1);
                            }
                            else
                            {
                                comment.postid = (article.comments.Count() + 1).ToString();
                            }
                            article.comments.Add(comment);
                        }

                        else
                        {
                            ViewData["EmptyComment"] = "Put something atleast!";
                            return View("ResultView");
                        }
                    }

                    else if (commentContent != null)
                    {
                        var foundParentComment = false;
                        while (!foundParentComment)
                        {
                            foundParentComment = AddCommentToParentComment(article.comments, commentId, commentContent);
                        }
                    }

                    var jsonCommentList = JsonConvert.SerializeObject(article.comments);

                    post.comments = jsonCommentList;

                    pe.Post_Table.AddOrUpdate(post);
                    pe.SaveChanges();

                    return View("ResultView");
                }
            }

            //TODO: Print exception in log
            catch (Exception e)
            {
                return Content(e.ToString());
            }
        }


        public bool AddCommentToParentComment(List<Comment> comments, string id, string commentContent)
        {
            foreach (var item in comments)
            {
                if (item.postid == id)
                {
                    Comment comment = new Comment();

                    comment.content_ = commentContent;
                    comment.userid = Session["userid"].ToString();
                    comment.date = DateTime.Now;

                    if (item.comments.Count < 10)
                    {
                        comment.postid = id + "0" + (item.comments.Count + 1);
                    }
                    else
                    {
                        comment.postid = id + (item.comments.Count + 1);
                    }

                    item.comments.Add(comment);
                    return true;
                }
                if (AddCommentToParentComment(item.comments, id, commentContent))
                {
                    return true;
                }
            }
            return false;
        }

        public ActionResult PostRating(Article article, Rate rate, string rating, string postId)
        {
            using (DbAccessEntity postEntity = new DbAccessEntity())
            {
                var postIdInt = Convert.ToInt32(postId);

                var post = postEntity.Post_Table.Where(x => x.postid == postIdInt).FirstOrDefault();

                if (post.comments != null)
                {
                    article.comments = JsonConvert.DeserializeObject<List<Comment>>(post.comments);
                }

                Rate.rateList.Clear();

                if (post.rating != null)
                {
                    Rate.rateList = JsonConvert.DeserializeObject<List<Rate>>(post.rating);
                    rate.rating = Rate.getUserRating(Rate.rateList, Session["userid"].ToString());
                    rate.userId = Session["userid"].ToString();
                }

                if (rate.rating == 0)
                {
                    rate = new Rate();
                    rate.rating = Convert.ToInt32(rating);
                    rate.userId = Session["userid"].ToString();

                    Rate.rateList.Add(rate);

                    var jsonRatingList = JsonConvert.SerializeObject(Rate.rateList);

                    post.rating = jsonRatingList;

                    postEntity.Post_Table.AddOrUpdate(post);
                    postEntity.SaveChanges();
                }

                ViewData["Article"] = post;
                ViewData["Comments"] = article;
                ViewData["Rate"] = rate;
                ViewData["AverageRating"] = Rate.calculateAverageRating(Rate.rateList);
            }
            return View("ResultView");
        }


        //To search for the post/author/tags based on input
        public ActionResult SearchPost(string term)
        {
            using (DbAccessEntity db = new DbAccessEntity())
            {
                List<Post_Table> searchlist = db.Post_Table.Where(x => x.tags.Contains(term) || x.title.Contains(term) || x.userid.Contains(term)).ToList();

                if (searchlist.Count > 0)
                {
                    ViewData["searchlist"] = searchlist;
                }
                else
                {
                    ViewData["searchlist"] = null;
                    ViewBag.SearchMessage = "No results found";
                }
               
            }
            return View("ResultView");
        }


        // GET: Post/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Post/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Post/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Post/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
