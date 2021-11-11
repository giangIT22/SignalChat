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
        public int currentUserState;
        public bool ShowingDetail = false;
        public string currentTab = "user";
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
                // User list
                rptUsers.DataSource = GetUsersList();
                rptUsers.DataBind();

                // Group list
                rptGroups.DataSource = GetGroupList();
                rptGroups.DataBind();


                // test
                //twGroupMembers.DataSource = new List<string>() { "a", "b", "c" };
                //twGroupMembers.DataBind();

            }
            else
            {
                ShowingDetail = btnExitDetailMode.Text == "X";
            }
        }
        public List<User_Group> GetGroupList()
        {
            currentUser = (User)Session[AppConst.SessionCurrentUserKey];
            var src = User_GroupFunc.GetList(currentUser.Id);
            return src;
        }
        public List<UserViewDto> GetUsersList()
        {
            var states = new List<int>();
            if (ckbFriendsRequest.Checked)
                states.Add(1);
            if (ckbFriendsBlock.Checked)
                states.Add(4);
            if (ckbFriends.Checked)
                states.Add(3);
            currentUser = (User)Session[AppConst.SessionCurrentUserKey];
            var src = User_UserFunc.GetList(currentUser.Id, states);

            string filter = txtUserSearch.Text.Trim();
            if (!String.IsNullOrEmpty(filter))
            {
                src = src.Where(e => e.Name.Contains(filter)).ToList();
            }

            return src;
        }

        // ============= USER =============
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
            currentTab = "user";
            var src = GetUsersList();
            rptUsers.DataSource = src;
            rptUsers.DataBind();

        }

        protected void ckbFriends_CheckedChanged(object sender, EventArgs e)
        {
            currentTab = "user";
            var src = GetUsersList();
            rptUsers.DataSource = src;
            rptUsers.DataBind();
        }

        protected void ckbFriendsBlock_CheckedChanged(object sender, EventArgs e)
        {
            currentTab = "user";
            var src = GetUsersList();
            rptUsers.DataSource = src;
            rptUsers.DataBind();
        }

        protected void UserBtnHandler(object sender, EventArgs e)
        {
            currentTab = "user";
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
                var src = GetUsersList();
                rptUsers.DataSource = src;
                rptUsers.DataBind();
            }

        }

        protected void btnSearchUser_Click(object sender, EventArgs e)
        {
            currentTab = "user";
            var src = GetUsersList();
            rptUsers.DataSource = src;
            rptUsers.DataBind();
        }



        // ============= GROUP =============
        public int GetGroupState(object DataItem)
        {
            return ((User_Group)DataItem).State;
        }
        public int GetGroupId(object DataItem)
        {
            return ((User_Group)DataItem).GroupId;
        }
        protected void ckbGroupRequest_CheckedChanged(object sender, EventArgs e)
        {
            ReLoadGroupList();
        }

        protected void ckbJoinedGroup_CheckedChanged(object sender, EventArgs e)
        {
            ReLoadGroupList();
        }
        
        public void ReLoadGroupList()
        {
            currentTab = "group";
            List<int> states = new List<int>();
            if (ckbGroupRequest.Checked) states.Add(3);
            if (ckbJoinedGroup.Checked) {
                states.Add(3);
                states.Add(1);
            }
            var src = GetGroupList();
            string filter = txtGroupSearch.Text.Trim();
            if (!String.IsNullOrEmpty(filter))
            {
                src = src.Where(e => e.Name.Contains(filter)).ToList();
            }

            // Group list
            rptGroups.DataSource = src.Where(e => states.Any(sube => sube == e.State) || states.Count == 0).ToList();
            rptGroups.DataBind();
        }

        protected void GroupBtnHandler(object sender, EventArgs e)
        {
            currentTab = "group";
            Button btn = (Button)sender;
            currentUser = (User)Session[AppConst.SessionCurrentUserKey];

            string msg = "";
            bool IsSuccess = false;

            switch (btn.CommandName)
            {
                case "AddGroupRequest":
                    (IsSuccess, msg) = User_GroupFunc.AddGroupRequest(int.Parse(btn.CommandArgument), currentUser.Id);
                    break;
                case "DeleteGroupRequest":
                    (IsSuccess, msg) = User_GroupFunc.DeleteGroupRequest(int.Parse(btn.CommandArgument), currentUser.Id, currentUser.Id);
                    break;
                case "RemoveGroupMember":
                    (IsSuccess, msg) = User_GroupFunc.RemoveGroupMember(int.Parse(btn.CommandArgument), currentUser.Id, currentUser.Id);
                    break;
                case "DeleteGroup":
                    GroupFunc.DeleteGroup(int.Parse(btn.CommandArgument), currentUser.Id);
                    break;
                case "DetailsOfGroup":
                    ShowDetailOfGroup(int.Parse(btn.CommandArgument));
                    break;

                    
            }

            ReLoadGroupList();
        }

        protected void btnCreateGroup_Click(object sender, EventArgs e)
        {
            string groupName = txtGroupName.Text;
            currentUser = (User)Session[AppConst.SessionCurrentUserKey];
            bool IsSuccess = false;
            string msg = "";
            (IsSuccess,msg) = GroupFunc.AddGroup(
                new Group { 
                    Name = groupName,
                    Description ="",
                },
                currentUser.Id);
            // Add group to Current List
            rptGroups.DataSource = GetGroupList();
            rptGroups.DataBind();
            
        }

        public void ShowDetailOfGroup(int GroupId)
        {
            currentUser = (User)Session[AppConst.SessionCurrentUserKey];
            var group = GroupFunc.GetGroup(GroupId, currentUser.Id);
            if (group == null)
            {
                lblExitDetailMode.Text = "" ;
            }
            else
            {
                lblExitDetailMode.Text = group.Name;
            }
            
            btnExitDetailMode.Text = "X";
            this.ShowingDetail = true;
            var src = User_GroupFunc.GetMembers(currentUser.Id,GroupId);
            
            rptGroupMembers.DataSource = src.Where(e=>e.State == 1 || e.State == 2).ToList();
            var currentUTG = src.FirstOrDefault(e => e.UserId == currentUser.Id);
            
            currentUserState = currentUTG == null ? 0 : currentUTG.State;

            rptGroupMembers.DataBind();
            rptGroupPendingMembers.DataSource = src.Where(e => e.State == 3).ToList();
            rptGroupPendingMembers.DataBind();
            rptGroupBannedMembers.DataSource = src.Where(e => e.State == 4).ToList();
            rptGroupBannedMembers.DataBind();

        }
        protected void btnExitDetailMode_Click(object sender, EventArgs e)
        {
            if (this.ShowingDetail)
            {
                this.ShowingDetail = false;
                lblExitDetailMode.Text = "Thêm mới";
                btnExitDetailMode.Text = "+";
            }
            else
            {

            }
            
        }

        // UTG = UserToGroup
        public string UTG_GetUserToUserArgs(object DataItem)
        {
            return ((UserToGroup)DataItem).GroupId.ToString() + " " + ((UserToGroup)DataItem).UserId.ToString();
        }

        public int UTG_GetUserState(object DataItem)
        {
            return ((UserToGroup)DataItem).State;
        }
        public int UTG_GetCurrentUserState(object DataItem)
        {
            return ((UserToGroup)DataItem).State;
        }

        protected void UserToGroupBtnHandler(object sender, EventArgs even)
        {
            currentTab = "group";
            Button btn = (Button)sender;
            currentUser = (User)Session[AppConst.SessionCurrentUserKey];

            string msg = "";
            bool IsSuccess = false;

            var agrs = btn.CommandArgument.Split(' ').ToList();
            var GroupId = int.Parse(agrs[0]);
            var TargetUserId = int.Parse(agrs[1]);

            switch (btn.CommandName)
            {
                case "AcceptGroupRequest":
                    (IsSuccess, msg) = User_GroupFunc.AcceptGroupRequest(GroupId, currentUser.Id, TargetUserId);
                    break;
                case "DeleteGroupRequest":
                    (IsSuccess, msg) = User_GroupFunc.DeleteGroupRequest(GroupId, currentUser.Id, TargetUserId);
                    break;
                case "RemoveGroupMember":
                    (IsSuccess, msg) = User_GroupFunc.RemoveGroupMember(GroupId, currentUser.Id, TargetUserId);
                    break;
                case "BanGroupMember":
                    (IsSuccess, msg) = User_GroupFunc.BanGroupMember(GroupId, currentUser.Id, TargetUserId);
                    break;
                case "UnBanGroupMember":
                    (IsSuccess, msg) = User_GroupFunc.UnBanGroupMember(GroupId, currentUser.Id, TargetUserId);
                    break;
            }

            // Group list
            ShowDetailOfGroup(GroupId);
        }

        protected void btnGroupSearch_Click(object sender, EventArgs e)
        {
            ReLoadGroupList();
        }
    }
}