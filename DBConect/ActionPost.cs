using DBConect.Farmework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;
using DBConect.model;
using System.Text.RegularExpressions;

namespace DBConect
{
    public class ActionPost
    {
        private VssDbContext context = null;

        public ActionPost()
        {
            context = new VssDbContext();
        }

        //set acc admin when create responsive persion real talk (truth)

        public void AddAuthUser(string emailVt, string fullName)
        {
            bool checkPersion = false;
            foreach (var item in context.AccountAdmins)
            {
                if (item.EmailVt == emailVt)
                {
                    checkPersion = true;
                }
            }
            if (!checkPersion)
            {
                Regex rg = new Regex(@".*(?=@viettel.com.vn)");
                string username = rg.Match(emailVt).ToString().Trim();
                AccountAdmin DBAdmin = new AccountAdmin();
                DBAdmin.UserName = username;
                DBAdmin.EmailVt = emailVt;
                DBAdmin.FullName = fullName;
                DBAdmin.Role = "RESOLVER_TRUTH";
                context.AccountAdmins.Add(DBAdmin);
                context.SaveChanges();
            }
        }

        public void deletePersionAuth(string emailVt)
        {
            bool checkPersion = false;
            foreach (var item in context.PersionManageRealTalks)
            {
                if (item.EmailManage == emailVt)
                {
                    checkPersion = true;
                }
            }
            if (!checkPersion)
            {
                AccountAdmin Persion = context.AccountAdmins.Where(x => x.EmailVt == emailVt).FirstOrDefault();
                context.AccountAdmins.Remove(Persion);
                context.SaveChanges();
            }
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

        public IEnumerable<GetListPosts> GetListPostsNew(string searchString, int idStatus, int page, int pageSize)
        {
            IEnumerable<GetListPosts> modelHomeAdmin = context.Database.SqlQuery<GetListPosts>("Sp_ListProducts");
            if (idStatus != 0)
            {
                modelHomeAdmin = modelHomeAdmin.Where(x => x.Category == idStatus);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                modelHomeAdmin = modelHomeAdmin.Where(x => x.PostTitle.ToLower().Contains(searchString));
            }
            var res = modelHomeAdmin.OrderByDescending(x => x.CreateTime).ToPagedList(page, pageSize);

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

        public GetListRealTalk DetailRealTalk(int id)
        {
            var post = context.Database.SqlQuery<GetListRealTalk>("Sp_getListRealTalk").Where(x => x.id == id).FirstOrDefault();

            return post;
        }

        //tìm kiếm email người quản lý theo từng lĩnh vực
        public List<PersionManageRealTalk> findPersionResponsible(int idFiledTruth)
        {
            return context.PersionManageRealTalks.Where(x => x.IdField == idFiledTruth).ToList();
        }

        //public OnboadingPost DetailOnBoadingPost(int id)
        //{
        //    var post = context.OnboadingPosts.Find(id);

        //    return post;
        //}
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

        public bool UpdatePost(int id, string postTitle, string postConten, string Description, string userName, string ThumbNail, int Status, int category)
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
            catch (Exception)
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
            catch (Exception)
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

        //real talk

        public bool SendRealTalk(TalkReal model)
        {
            try
            {
                var dateString = "1/1/2000 0:00:00 AM";
                DateTime DateTimeApprove = DateTime.Parse(dateString, System.Globalization.CultureInfo.InvariantCulture);
                TalkReal DB = new TalkReal();
                DB.MailSender = model.MailSender;
                DB.NameSender = model.NameSender;
                DB.DepartmentSender = model.DepartmentSender;
                DB.TitleRealTalk = model.TitleRealTalk;
                DB.Suggestion = model.Suggestion;
                DB.Reality = model.Reality;
                DB.Field = model.Field;
                DB.TimeSend = DateTime.Now;
                DB.TimeApproval = DateTimeApprove;
                DB.TimeUpdateStatus = DateTime.Now;
                DB.Status = 1;
                DB.TruthStatus = 1;
                context.TalkReals.Add(DB);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public List<GetListRealTalk> ListTruth()
        {
            return context.Database.SqlQuery<GetListRealTalk>("Sp_getListRealTalk").ToList();
        }

        public CommentDetailTruth getInfoTruth(int idTruth)
        {
            var list = context.CommentDetailTruths.Where(x => x.IdTruth == idTruth).OrderByDescending(x => x.TimeCmt).ToList();
            CommentDetailTruth res = new CommentDetailTruth();
            if (list != null)
            {
                res = list[0];
            }
            return res;
        }

        public string findDepartment(string email)
        {
            var res = context.PersionManageRealTalks.Where(x => x.EmailManage == email).FirstOrDefault();

            return res.Department;
        }

        public string findPersionHandelToField (int idField)
        {
            List<string> persion = new List<string>();
            var list = context.PersionManageRealTalks.Where(x => x.IdField == idField).ToList();
            if(list != null)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    persion.Add(list[i].FullNameManage);
                }
            }
            else
            {
                return "";
            }

            return string.Join(",", persion);
        }

        public string mailHandelToFiend(int idField)
        {
            List<string> persion = new List<string>();
            var list = context.PersionManageRealTalks.Where(x => x.IdField == idField).ToList();
            if (list != null)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    persion.Add(list[i].EmailManage);
                }
            }
            else
            {
                return "";
            }

            return string.Join(",", persion);
        }

        public IEnumerable<GetListThankCard> GetListThankCard(int page, int pageSize)
        {
            var res = context.Database.SqlQuery<GetListThankCard>("Sp_getListThankCard").OrderByDescending(x => x.SendTime).ToPagedList(page, pageSize);
            return res;
        }

        public IEnumerable<GetListRealTalk> GetListTalkRealAdmin(string searchString, int finterTruthid, int page, int pageSize)
        {
            IEnumerable<GetListRealTalk> modelReal = context.Database.SqlQuery<GetListRealTalk>("Sp_getListRealTalk");

            if (finterTruthid != 0)
            {
                modelReal = modelReal.Where(x => x.TruthStatus == finterTruthid);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                modelReal = modelReal.Where(x => x.TitleRealTalk.ToLower().Contains(searchString));
            }

            //var res = context.TalkReals.ToList().OrderByDescending(x => x.TimeSend).ToPagedList(page, pageSize);
            var res = modelReal.OrderByDescending(x => x.TimeSend).ToPagedList(page, pageSize);
            return res;
        }

        //get list real talk follow email user login
        public List<GetListRealTalk> GetListRealTalkByEmail(string email)
        {
            return context.Database.SqlQuery<GetListRealTalk>("Sp_getListRealTalk").Where(x => x.MailSender == email).ToList();
        }


        public IEnumerable<GetListRealTalk> GetListTruthSuccessForCmt(string searchString, int idTruhStatusFinter, int idInterate, int page, int pageSize)
        {
            IEnumerable<GetListRealTalk> model = context.Database.SqlQuery<GetListRealTalk>("Sp_getListRealTalk");
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Reality.ToLower().Contains(searchString) && (x.ShowInNewFeed) && (x.TruthStatus != 1));
            }
            if (idTruhStatusFinter != 0)
            {
                model = model.Where(x => x.TruthStatus == idTruhStatusFinter && (x.ShowInNewFeed) && (x.TruthStatus != 1));
            }
            else
            {
                model = model.Where(x => (x.ShowInNewFeed) && (x.TruthStatus != 1));
            }
            //sắp xếp theo ngày gần nhất
            var res = model.OrderByDescending(x => x.TimeApproval).ToPagedList(page, pageSize);
            //sắp xếp theo bài viết có nhiều bình luận nhất
            if (idInterate == 1)
            {
                res = model.OrderByDescending(x => x.UserNameComment?.Count()).ToPagedList(page, pageSize);
            }
            //sắp xếp theo bài viết có nhiều like nhất
            if (idInterate == 2)
            {
                res = model.OrderByDescending(x => x.UserNameLike?.Count()).ToPagedList(page, pageSize);
            }

            return res;
        }

        public List<GetListRealTalk> GetListTruthv2()
        {
            var res = context.Database.SqlQuery<GetListRealTalk>("Sp_getListRealTalk").Where(x => (x.Status != 3) && (x.TruthStatus != 1)).ToList();
            return res;
        }


        public List<ThankCard> getCountThankCard()
        {
            return context.ThankCards.ToList();
        }


        public List<TalkReal> getListTalkReal()
        {
            return context.TalkReals.ToList();
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

        //field real Talk

        public List<FieldRealTalk> getListFieldRealTalk()
        {
            return context.FieldRealTalks.ToList();
        }

        //get list persion manage real talk 
        public List<PersionManageRealTalk> GetListPersionManageRealTalk()
        {
            return context.PersionManageRealTalks.ToList();
        }

        //delete persion in field

        public bool DeletePersionInField(int id)
        {
            try
            {
                var persion = context.PersionManageRealTalks.Find(id);
                if (persion != null)
                {
                    context.PersionManageRealTalks.Remove(persion);
                    context.SaveChanges();
                    deletePersionAuth(persion.EmailManage);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        // thêm mới người quản lý từng lĩnh vực trong real talk
        public int addNewPersionInFieldRealTalk(PersionManageRealTalk model)
        {
            // status = 0 : đã tồn tại người quản lý
            // status = 1 : thêm mới người quản lý thành công
            // status = 2 : có lỗi
            try
            {
                PersionManageRealTalk resPersion = context.PersionManageRealTalks.Where(x => x.IdField == model.IdField && x.EmailManage == model.EmailManage).FirstOrDefault();
                if (resPersion != null)
                {
                    return 0;
                }
                PersionManageRealTalk persion = new PersionManageRealTalk();
                persion.IdField = model.IdField;
                persion.FullNameManage = model.FullNameManage;
                persion.EmailManage = model.EmailManage;
                persion.Department = model.Department;
                context.PersionManageRealTalks.Add(persion);
                context.SaveChanges();
                AddAuthUser(model.EmailManage, model.FullNameManage);
                return 1;
            }
            catch
            {
                return 2;
            }
        }

        //get list truth status
        public List<TruthStatu> getListTruthStatus()
        {
            return context.TruthStatus.ToList();
        }

        //find employee
        public Employee findEmployee(string mail)
        {
            //var res = context.Employees.Find(mail);
            var res = context.Employees.Where(x => x.Email == mail).FirstOrDefault();

            return res;
        }

        public List<Employee> autocompleteEmail(string mail)
        {
            //var res = context.Employees.Find(mail);
            //var listEmployee = context.Employees.ToList();
            //var res = (from N in listEmployee where N.Email.StartsWith(mail) select new { N.Email }).ToList();
            var res = context.Employees.Where(x => x.Email.Contains(mail)).ToList();

            return res;
        }

        public Employee findNameByEmail(string mail)
        {
            var res = context.Employees.Where(x => x.Email == mail).FirstOrDefault();

            return res;
        }

        //find Employee flow id department
        public List<Employee> findEmployeetoDepartment(string TextDepartment)
        {
            var res = context.Employees.Where(x => x.Department == TextDepartment).ToList();

            return res;
        }

        //Idea sáng kiến ý tưởng

        public void PostResigterIdea(ManageRegisterIdea model)
        {
            var dateString = "1/1/2000 0:00:00 AM";
            DateTime DateTimeApprove = DateTime.Parse(dateString, System.Globalization.CultureInfo.InvariantCulture);
            ManageRegisterIdea DBIdea = new ManageRegisterIdea();
            DBIdea.NameIdea = model.NameIdea;
            DBIdea.contenIdea = model.contenIdea;
            DBIdea.StatusIdeaBefore = model.StatusIdeaBefore;
            DBIdea.EmailIndividual = model.EmailIndividual;
            DBIdea.LimitApply = model.LimitApply;
            DBIdea.Effective = model.Effective;
            DBIdea.Field = model.Field;
            DBIdea.DocumentIdea = model.DocumentIdea;
            DBIdea.DetailMore = model.DetailMore;
            DBIdea.IdeaOfDepartment = model.IdeaOfDepartment;
            DBIdea.IndividualIdea = model.IndividualIdea;
            DBIdea.AuthorEmail = model.AuthorEmail;
            DBIdea.AuthorFullName = model.AuthorFullName;
            DBIdea.AuthorDepartment = model.AuthorDepartment;
            DBIdea.AuthorUserId = model.AuthorUserId;
            DBIdea.Status = 1;
            DBIdea.DateSend = DateTime.Now;
            DBIdea.DateTimeApprove = DateTimeApprove;
            context.ManageRegisterIdeas.Add(DBIdea);
            context.SaveChanges();
        }

        //get status idea

        public List<StatusIdeaInitative> getStatusIdea()
        {
            return context.StatusIdeaInitatives.ToList();
        }

        //get danh sách đăng ký ý tưởng
        public IEnumerable<GetListIdeaRegester> GetListIdeaRegester(string searchString, int idStatus, int page, int pageSize)
        {
            IEnumerable<GetListIdeaRegester> modelGetListIdea = context.Database.SqlQuery<GetListIdeaRegester>("Sp_getListIdeaRegester");
            if (idStatus != 0)
            {
                modelGetListIdea = modelGetListIdea.Where(x => x.Status == idStatus);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                modelGetListIdea = modelGetListIdea.Where(x => x.NameIdea.ToLower().Contains(searchString));
            }
            var res = modelGetListIdea.OrderByDescending(x => x.DateSend).ToPagedList(page, pageSize);
            return res;
        }

        //get tất cả danh sách đăng ký ý tưởng 
        public List<GetListIdeaRegester> GetAllListIdeaRegester()
        {
            var res = context.Database.SqlQuery<GetListIdeaRegester>("Sp_getListIdeaRegester").ToList();
            return res;
        }

        //get danh sách đăng ký sáng kiến
        public IEnumerable<GetListInitativeRegester> GetListInitativeRegester(string searchString, int idStatus, int page, int pageSize)
        {
            IEnumerable<GetListInitativeRegester> modelGetListIdea = context.Database.SqlQuery<GetListInitativeRegester>("Sp_getListInitative");
            if (idStatus != 0)
            {
                modelGetListIdea = modelGetListIdea.Where(x => x.Status == idStatus);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                modelGetListIdea = modelGetListIdea.Where(x => x.NameInitative.ToLower().Contains(searchString));
            }

            var res = modelGetListIdea.OrderByDescending(x => x.TimeSendInitative).ToPagedList(page, pageSize);
            return res;
        }

        //get all danh sách đăng ký sáng kiến
        public List<GetListInitativeRegester> GetAllListInitativeRegester()
        {
            var res = context.Database.SqlQuery<GetListInitativeRegester>("Sp_getListInitative").ToList();
            return res;
        }

        //arrpore Truth - nói thật đê

        public bool ApproveTruth(int id)
        {
            try
            {
                var Truth = context.TalkReals.Find(id);
                Truth.Status = 2;
                Truth.TruthStatus = 2;
                Truth.TimeApproval = DateTime.Now;
                Truth.TimeUpdateStatus = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //change doing truth
        public bool DoingTruth(int id)
        {
            try
            {
                var truth = context.TalkReals.Find(id);
                truth.TruthStatus = 3;
                truth.TimeUpdateStatus = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //change close truth
        public bool CloseTruth(int id)
        {
            try
            {
                var truth = context.TalkReals.Find(id);
                truth.TruthStatus = 4;
                truth.TimeUpdateStatus = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //change thanks truth
        public bool ThanksTruth(int id)
        {
            try
            {
                var truth = context.TalkReals.Find(id);
                truth.TruthStatus = 5;
                truth.TimeUpdateStatus = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //create comment truth

        public void CreateCommentTruth(int idTruth, string commentDetail, int truthStatus, string namePersion, string mailPersion)
        {
            CommentDetailTruth cmt = new CommentDetailTruth();
            cmt.IdTruth = idTruth;
            cmt.StatusTruth = truthStatus;
            cmt.CommentTruth = commentDetail;
            cmt.NameManagerCmt = namePersion;
            cmt.EmailManagerCmt = mailPersion;
            cmt.TimeCmt = DateTime.Now;
            context.CommentDetailTruths.Add(cmt);
            context.SaveChanges();
        }

        //get cmt truth

        public List<CommentDetailTruth> GetCmtProcess(int id)
        {
            return context.CommentDetailTruths.Where(x => x.IdTruth == id).ToList();
        }

        //arrpore idea 
        public bool ApproveIdea(int id)
        {
            try
            {
                var DBIdea = context.ManageRegisterIdeas.Find(id);
                DBIdea.Status = 2;
                DBIdea.DateTimeApprove = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //arrpore initative 
        public bool ApproveInitative(int id)
        {
            try
            {
                var DBIdea = context.InitativeManages.Find(id);
                DBIdea.Status = 2;
                DBIdea.TimeApprove = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //reject Truth nói thật đê
        public bool RejectTruth(int id)
        {
            try
            {
                var Truth = context.TalkReals.Find(id);
                Truth.Status = 3;
                Truth.TruthStatus = 5;
                Truth.TimeApproval = DateTime.Now;
                Truth.TimeUpdateStatus = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //set hiển thị truh trên bảng tin
        public bool setStatusFlag(int id)
        {
            try
            {
                var Truth = context.TalkReals.Find(id);
                bool nowStatus = Truth.ShowInNewFeed;
                Truth.ShowInNewFeed = !nowStatus;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //thay đổi tên lĩnh vực
        public bool changeNameField(int id , string newNameField)
        {
            try
            {
                var field = context.FieldRealTalks.Find(id);
                field.NameFieldRealTalk = newNameField;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        //thêm mới lĩnh vực

        public bool addNewField (string nameFiled)
        {
            try
            {
                FieldRealTalk listField = new FieldRealTalk();
                listField.NameFieldRealTalk = nameFiled;
                context.FieldRealTalks.Add(listField);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //admin update field (lĩnh vực) truth
        public bool AdminUpdateField(int id, int newField)
        {
            try
            {
                var truth = context.TalkReals.Find(id);
                truth.Field = newField;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //reject idea
        public bool RejectIdea (int id)
        {
            try
            {
                var DBIdea = context.ManageRegisterIdeas.Find(id);
                DBIdea.Status = 3;
                DBIdea.DateTimeApprove = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //reject initative
        public bool RejectInitative(int id)
        {
            try
            {
                var DBIdea = context.InitativeManages.Find(id);
                DBIdea.Status = 3;
                DBIdea.TimeApprove = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //detai idea
        public GetListIdeaRegester DetailIdeaRegester(int id)
        {
            return context.Database.SqlQuery<GetListIdeaRegester>("Sp_getListIdeaRegester").Where(x => x.Id == id).FirstOrDefault();
        }

        //detai initative regester
        public GetListInitativeRegester DetailInitativeRegester(int id)
        {
            return context.Database.SqlQuery<GetListInitativeRegester>("Sp_getListInitative").Where(x => x.Id == id).FirstOrDefault();
        }

        //find idea regestered 

        public List<GetListIdeaRegester> findIdeaRegestered(string username)
        {
            return context.Database.SqlQuery<GetListIdeaRegester>("Sp_getListIdeaRegester").Where(x => x.AuthorUserId == username).ToList();
        }

        //tìm kiếm sáng kiến
        public List<GetListInitativeRegester> findIinitativeRegestered(string username)
        {
            return context.Database.SqlQuery<GetListInitativeRegester>("Sp_getListInitative").Where(x => x.UserSend == username).ToList();
        }


        public List<ManageRegisterIdea> getListIdea()
        {
            return context.ManageRegisterIdeas.ToList();
        }

        public List<FieldInIdeal> GetField()
        {
            return context.FieldInIdeals.ToList();
        }

        public List<UnitApply> getUnitApply()
        {
            return context.UnitApplies.ToList();
        }

        //đăng ký sáng kiến
        //post list đăng ký sáng kiến

        public void CreateInitativeRegester (InitativeManage model)
        {
            var dateString = "1/1/2000 0:00:00 AM";
            DateTime DateTimeApprove = DateTime.Parse(dateString, System.Globalization.CultureInfo.InvariantCulture);
            InitativeManage DBInitative = new InitativeManage();
            DBInitative.UserSend = model.UserSend;
            DBInitative.UserSendFullName = model.UserSendFullName;
            DBInitative.UserSenDepartment = model.UserSenDepartment;
            DBInitative.EmailUserSend = model.EmailUserSend;
            DBInitative.PersonalInitative = model.PersonalInitative;
            DBInitative.MailOfEach = model.MailOfEach;
            DBInitative.InitativeOfDepartment = model.InitativeOfDepartment;
            DBInitative.NameInitative = model.NameInitative;
            DBInitative.IdUnitApply = model.IdUnitApply;
            DBInitative.IdField = model.IdField;
            DBInitative.TechnicalCondition = model.TechnicalCondition;
            DBInitative.EffectiveApply = model.EffectiveApply;
            DBInitative.Applicability = model.Applicability;
            DBInitative.NewPoint = model.NewPoint;
            DBInitative.ContentInitative = model.ContentInitative;
            DBInitative.NameDocument = model.NameDocument;
            DBInitative.DetailMore = model.DetailMore;
            DBInitative.TimeSendInitative = DateTime.Now;
            DBInitative.TimeApprove = DateTimeApprove;
            DBInitative.Status = 1;
            context.InitativeManages.Add(DBInitative);
            context.SaveChanges();
        }
    }
}
