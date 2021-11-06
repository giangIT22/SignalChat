using SignalRChat.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalRChat
{
    public partial class Home1 : System.Web.UI.Page
    {
        public User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack) {
                currentUser = new User();
                var user = (User)Session[AppConst.SessionCurrentUserKey];
                
                if (user == null)
                {
                    Response.Redirect("Login.aspx");
                }

                currentUser = user;
                rptUsers.DataSource = GetList();
                rptUsers.DataBind();
            }
            
        }

        public List<UserViewDto> GetList()
        {
            var states = new List<int>();
            if (ckbFriendsRequest.Checked)
                states.Add(1);
            if (ckbFriendsBlock.Checked)
                states.Add(4);
            if (ckbFriends.Checked)
                states.Add(3);
            var src = User_UserFunc.GetList(currentUser.Id, states);
            return src;
        }

        public int GetState(object DataItem)
        {
            return ((UserViewDto)DataItem).State;
        }
        public int GetUserId(object DataItem)
        {
            return ((UserViewDto)DataItem).UserId;
        }



        protected void ckbFriendsRequest_CheckedChanged(object sender, EventArgs e)
        {


        }

        protected void ckbFriends_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void ckbFriendsBlock_CheckedChanged(object sender, EventArgs e)
        {

        }


        protected void UserBtnHandler(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            currentUser = (User)Session[AppConst.SessionCurrentUserKey];

            string msg = "";
            bool IsSuccess = false;

            switch (btn.CommandName)
            {
                case "AddFriendRequest":
                    (IsSuccess, msg) = User_UserFunc.AddFriendRequest(currentUser.Id, int.Parse(btn.CommandArgument.ToString()));
                    break;
                case "BlockFriend":
                    (IsSuccess, msg) = User_UserFunc.BlockUser_User(currentUser.Id, int.Parse(btn.CommandArgument.ToString()));
                    break;
                case "DeleteFriendRequest":
                    (IsSuccess, msg) = User_UserFunc.DeleteFriendRequest(currentUser.Id, int.Parse(btn.CommandArgument.ToString()));

                    break;
                case "AcceptFriendRequest":
                    (IsSuccess, msg) = User_UserFunc.AcceptFriendRequest(currentUser.Id, int.Parse(btn.CommandArgument.ToString()));
                    break;
                case "ReverseDeleteFriendRequest":
                    (IsSuccess, msg) = User_UserFunc.DeleteFriendRequest(int.Parse(btn.CommandArgument.ToString()), currentUser.Id);
                    break;
                case "UnFriend":
                    (IsSuccess, msg) = User_UserFunc.UnFriendUser_User(currentUser.Id, int.Parse(btn.CommandArgument.ToString()));
                    break;
                case "UnBlock":
                    (IsSuccess, msg) = User_UserFunc.UnBlockUser_User(currentUser.Id, int.Parse(btn.CommandArgument.ToString()));
                    break;
            }

            if (IsSuccess)
            {
                var src = GetList();
                rptUsers.DataSource = src;
                rptUsers.DataBind();
            }

        }


    }
}