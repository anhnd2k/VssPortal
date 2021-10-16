using DBConect.Farmework;
using DBConect.model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConect
{
    public class ActionCommentLikePost
    {
        private VssDbContext context = null;

        public ActionCommentLikePost()
        {
            context = new VssDbContext();
        }

        public List<CommentIdeaPost> ListCmtPostIdea()
        {
            var res = context.CommentIdeaPosts.OrderBy(x => x.TimeComment).ToList();
            return res;
        }

        //load more cmt
        public List<CommentIdeaPost> GetCmtLoadMore(int idPost, int coutesd)
        {
            var resLoadMore = context.CommentIdeaPosts.Where(x => x.IdPostIdea == idPost).OrderBy(x => x.TimeComment).ToList();
            var rest = new List<CommentIdeaPost>();
            if (coutesd < resLoadMore.Count)
            {
                for (var i = 0; i < coutesd; i++)
                {
                    resLoadMore.RemoveAt(0);
                }
            }
            else
            {
                rest = null;
                return rest;
            }
            if ((coutesd + 4) > (resLoadMore.Count + coutesd))
            {
                rest = resLoadMore;
            }
            else
            {
                rest = resLoadMore.Take(4).ToList();
            }
            return rest;
        }

        public List<LikeCountPostIdea> ListLikePostIdea()
        {
            var res = context.LikeCountPostIdeas.ToList();
            return res;
        }

        public bool LikePost(int idPost, string userNameLike)
        {
            bool unlikes = false;
            var LikePost = context.LikeCountPostIdeas.Where(x => x.IdPostIdea == idPost).FirstOrDefault();
            if(LikePost != null)
            {
                List<string> user = new List<string>();
                if (LikePost.UserNameLike != "")
                {
                    user = LikePost.UserNameLike.Trim().Split(',').ToList();
                    var arrayUserLike = LikePost.UserNameLike.Trim().Split(',');
                    for (var i = 0; i < arrayUserLike.Length; i++)
                    {
                        if (arrayUserLike[i] == userNameLike)
                        {
                            unlikes = true;
                            user.RemoveAt(i);
                            break;
                        }
                    }
                }
                if(unlikes == false)
                {
                    user.Add(userNameLike);
                }
                string outputString = string.Join(",", user);

                LikePost.UserNameLike = outputString;
                context.SaveChanges();
            }
            else
            {
                LikeCountPostIdea actionLike = new LikeCountPostIdea();
                actionLike.IdPostIdea = idPost;
                actionLike.UserNameLike = userNameLike;
                context.LikeCountPostIdeas.Add(actionLike);
                context.SaveChanges();
            }

            return unlikes;
        }

        //add new cmt

        public void AddNewCmt(CommentIdeaPost model)
        {
            var cmtTruth = context.CommentCountTruths.Where(x => x.IdPostTruth == model.IdPostIdea).FirstOrDefault();
            if(cmtTruth != null)
            {
                var userNameCmt = cmtTruth.UserNameComment.Trim().Split(',').ToList();
                userNameCmt.Add(model.UserName);
                string outPutUserCmt = string.Join(",", userNameCmt);
                cmtTruth.UserNameComment = outPutUserCmt;
                context.SaveChanges();
            }
            else
            {
                CommentCountTruth CmtCout = new CommentCountTruth();
                CmtCout.IdPostTruth = model.IdPostIdea;
                CmtCout.UserNameComment = model.UserName;
                context.CommentCountTruths.Add(CmtCout);
                context.SaveChanges();
            }

            CommentIdeaPost dbCmt = new CommentIdeaPost();
            dbCmt.IdPostIdea = model.IdPostIdea;
            dbCmt.FullNameUser = model.FullNameUser;
            dbCmt.UserName = model.UserName;
            dbCmt.CommentValue = model.CommentValue;
            dbCmt.TimeComment = DateTime.Now;
            context.CommentIdeaPosts.Add(dbCmt);
            context.SaveChanges();
        }
    }
}
