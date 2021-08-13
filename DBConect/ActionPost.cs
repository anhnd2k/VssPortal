using DBConect.Farmework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;
using DBConect.model;

namespace DBConect
{
    public class ActionPost
    {
        private VssDbContext context = null;

        public ActionPost()
        {
            context = new VssDbContext();
        }

        public List<ListPost> GetListPost()
        {
            //var res = context.ListPosts.ToList();
            var res = context.Database.SqlQuery<ListPost>("sp_getlistpost").ToList();
            return res;
        }

        public List<ListPost> ListPostOnboading()
        {
            var res = context.Database.SqlQuery<ListPost>("sp_getlistpost").Where(x => x.Status == 1).ToList();
            return res;
        }

        public IEnumerable<GetListPosts> GetListPostsAllPostHome(string searchString, int page, int pageSize)
        {
            IEnumerable<GetListPosts> model = context.Database.SqlQuery<GetListPosts>("Sp_ListProducts");
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.PostTitle.ToLower().Contains(searchString));
            }
            var res = model.OrderByDescending(x => x.CreateTime).ToPagedList(page, pageSize);

            return res;
        }

        public IEnumerable<GetListPosts> GetListPostsNew(int page, int pageSize)
        {
            var res = context.Database.SqlQuery<GetListPosts>("Sp_ListProducts").OrderByDescending(x => x.CreateTime).ToPagedList(page, pageSize);

            return res;
        }

        public List<ListPost> HomeGetLimedPost()
        {
            var listPost = context.Database.SqlQuery<ListPost>("Sp_HomeGetListPost").ToList();

            return listPost;
        }

        public ListPost DetailPost(int id)
        {
            var post = context.ListPosts.Find(id);

            return post;
        }

        public OnboadingPost DetailOnBoadingPost(int id)
        {
            var post = context.OnboadingPosts.Find(id);

            return post;
        }
        public void AddPost(string postTitle, string postConten, string userName, string ThumbNail)
        {
            ListPost Obj = new ListPost();
            Obj.PostTitle = postTitle;
            Obj.PostConten = postConten;
            Obj.Author = userName;
            Obj.ThumbNail = ThumbNail;
            Obj.CreateTime = DateTime.Now;
            context.ListPosts.Add(Obj);
            context.SaveChanges();
        }

        public void AddPostsNew(string postTitle, string postConten, string userName, string ThumbNail, string Description, int Status, int category)
        {
            ListPost Obj = new ListPost();
            Obj.PostTitle = postTitle;
            Obj.PostConten = postConten;
            Obj.Author = userName;
            Obj.ThumbNail = ThumbNail;
            Obj.CreateTime = DateTime.Now;
            Obj.Description = Description;
            Obj.Status = Status;
            Obj.Category = category;
            context.ListPosts.Add(Obj);
            context.SaveChanges();
        }

        public bool UpdatePost(int id ,string postTitle, string postConten, string Description, string userName, string ThumbNail, int Status, int category)
        {
            try
            {
                var post = context.ListPosts.Find(id);
                post.PostTitle = postTitle;
                post.PostConten = postConten;
                post.Description = Description;
                post.Author = userName;
                post.ThumbNail = ThumbNail;
                post.Status = Status;
                post.Category = category;
                post.TImeEdit = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var post = context.ListPosts.Find(id);
                context.ListPosts.Remove(post);
                context.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public List<CategoryPost> getCategory()
        {
            return context.CategoryPosts.ToList();
        }

        // thank card
        public void SendThankCard(bool Incognito, string Sender, string MailSender, string Receiver, string MailReceiver, int Department, string TitleCard, string CardImg, string ContenCard)
        {
            ThankCard DB = new ThankCard();
            DB.Incognito = Incognito;
            DB.Sender = Sender;
            DB.MailSender = MailSender;
            DB.Receiver = Receiver;
            DB.MailReceiver = MailReceiver;
            DB.Department = Department;
            DB.TitleCard = TitleCard;
            DB.CardImg = CardImg;
            DB.ContenCard = ContenCard;
            DB.SendTime = DateTime.Now;
            context.ThankCards.Add(DB);
            context.SaveChanges();
        }

        public IEnumerable<GetListThankCard> GetListThankCard(int page, int pageSize)
        {
            var res = context.Database.SqlQuery<GetListThankCard>("Sp_getListThankCard").OrderByDescending(x => x.SendTime).ToPagedList(page, pageSize);
            return res;
        }

        public List<ThankCard> getCountThankCard()
        {
            return context.ThankCards.ToList();
        }

        public bool DeleteFeedBack(int id)
        {
            try
            {
                var feedback = context.ThankCards.Find(id);
                context.ThankCards.Remove(feedback);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //getImg thank card

        public List<ListThankCardImg> GetListImgThankCard()
        {
            return context.ListThankCardImgs.ToList();
        }

        //department

        public List<Department> getListDepartment()
        {
            return context.Departments.ToList();
        }

        //find employee
        public Employee findEmployee(string mail)
        {
            //var res = context.Employees.Find(mail);
            var res = context.Employees.Where(x => x.Email == mail).FirstOrDefault();

            return res;
        }

        //find Employee flow id department
        public List<Employee> findEmployeetoDepartment(string TextDepartment)
        {
            var res = context.Employees.Where(x => x.Department == TextDepartment).ToList();

            return res;
        }
    }
}
