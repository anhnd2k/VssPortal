using DBConect.Farmework;
using DBConect.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vss_portal_web.Models
{
    public class ModelController
    {
        public List<ListPost> ListPost { get; set; }
        public List<ListPost> OnboadingPost { get; set; }
        public List<ListThankCardImg> ThankCardImg { get; set; }
        public List<Department> ListDepartments { get; set; }
        public List<ThankCard> ListhankCard { get; set; }
        public List<TalkReal> ListTalkReal { get; set; }
        public List<FieldRealTalk> ListFieldRealTalk { get; set; }

        //model đăng ký sáng kiến

        public List<FieldInIdeal> FieldIdea { get; set; }
        public List<UnitApply> UnitApplyModel { get; set; }

        // sáng kiến, ý tưởng

        public List<GetListIdeaRegester> ListIdea { get; set; }

        public List<GetListInitativeRegester> ListInitative { get; set; }


        //comment like post idea

        public List<CommentIdeaPost> ListPostAndCmt { get; set; }
        public List<GetListRealTalk> listPostIdea { get; set; }
        public List<LikeCountPostIdea> listLikePost { get; set; }
    }
}