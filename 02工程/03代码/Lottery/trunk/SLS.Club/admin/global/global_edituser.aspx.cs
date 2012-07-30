using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑用户
    /// </summary>

    public partial class edituser : AdminPage
    {
        public UserInfo __userinfo = new UserInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if(!AllowEditUser(this.userid,DNTRequest.GetInt("uid",-1)))
                {
                    Response.Write("<script>alert('非创始人身份不能修改其它管理员的信息!');window.location.href='global_usergrid.aspx';</script>");
                    Response.End();
                    return;
                }
                IsEditUserName.Attributes.Add("onclick", "document.getElementById('" + userName.ClientID + "').disabled = !document.getElementById('" + IsEditUserName.ClientID + "').checked;");
            }
        }

        private bool AllowEditUser(int managerUId, int targetUId)
        {
            #region 是否可以编辑用户
            int managerGroupId = Discuz.Forum.Users.GetUserInfo(managerUId).Groupid;
            if(Discuz.Forum.Users.GetUserInfo(managerUId).Adminid == 0)
            {
                return false;
            }
            int targetGroupId = Discuz.Forum.Users.GetUserInfo(targetUId).Groupid;
            int founderUId = BaseConfigs.GetBaseConfig().Founderuid;
            if (managerUId == targetUId)    //可自身修改
                return true;
            else if (managerUId == founderUId)  //创始人可修改
                return true;
            else if (managerGroupId == targetGroupId)   //管理组相同的不能修改
                return false;
            else
                return true;
            #endregion
        }

        public bool AllowEditUserInfo(int uid, bool redirect)
        {
            #region 是否允许编辑用户信息
            if ((BaseConfigs.GetBaseConfig().Founderuid == uid) && (uid == this.userid))
            {
                return true;
            }
            else
            {
                if (BaseConfigs.GetBaseConfig().Founderuid != uid) //当要编辑的用户信息不是创建人的信息时
                {
                    return true;
                }
                else
                {
                    if (redirect)
                    {
                        base.RegisterStartupScript( "", "<script>alert('您要编辑信息是论坛创始人信息,请您以创始人身份登陆后台才能修改!');</script>");
                    }
                    return false;
                }
            }

            #endregion
        }

        public bool IsValidScoreName(int scoreid)
        {
            #region 是否是有效的金币名称

            bool isvalid = false;

            foreach (DataRow dr in Scoresets.GetScoreSet().Rows)
            {
                if ((dr["id"].ToString() != "1") && (dr["id"].ToString() != "2"))
                {
                    if (dr[scoreid + 1].ToString().Trim() != "0")
                    {
                        isvalid = true;
                        break;
                    }
                }
            }
            return isvalid;

            #endregion
        }

        public void LoadScoreInf(string fid, string fieldname)
        {
            #region 加载金币信息

            DataRow dr = Scoresets.GetScoreSet().Rows[0];
            if (dr[2].ToString().Trim() != "")
            {
                extcredits1name.Text = dr[2].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(1))
                {
                    extcredits1.Enabled = false;
                }
            }

            if (dr[3].ToString().Trim() != "")
            {
                extcredits2name.Text = dr[3].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(2))
                {
                    extcredits2.Enabled = false;
                }
            }


            if (dr[4].ToString().Trim() != "")
            {
                extcredits3name.Text = dr[4].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(3))
                {
                    extcredits3.Enabled = false;
                }
            }


            if (dr[5].ToString().Trim() != "")
            {
                extcredits4name.Text = dr[5].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(4))
                {
                    extcredits4.Enabled = false;
                }
            }


            if (dr[6].ToString().Trim() != "")
            {
                extcredits5name.Text = dr[6].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(5))
                {
                    extcredits5.Enabled = false;
                }
            }


            if (dr[7].ToString().Trim() != "")
            {
                extcredits6name.Text = dr[7].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(6))
                {
                    extcredits6.Enabled = false;
                }
            }


            if (dr[8].ToString().Trim() != "")
            {
                extcredits7name.Text = dr[8].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(7))
                {
                    extcredits7.Enabled = false;
                }
            }


            if (dr[9].ToString().Trim() != "")
            {
                extcredits8name.Text = dr[9].ToString().Trim();
            }
            else
            {
                if (!IsValidScoreName(8))
                {
                    extcredits8.Enabled = false;
                }
            }

            #endregion
        }

        public void LoadCurrentUserInfo(int uid)
        {
            #region 加载相关信息

            __userinfo = AdminUsers.GetUserInfo(uid);

            ViewState["username"] = __userinfo.Username;
            userName.Text = __userinfo.Username;

            //只有在当前用户为等待验证用户, 且系统论坛设置为1时才会显示重发EMAIL按钮
            if ((__userinfo.Groupid == 8) && (config.Regverify == 1)) ReSendEmail.Visible = true;
            else ReSendEmail.Visible = false;

            nickname.Text = __userinfo.Nickname;
            accessmasks.SelectedValue = __userinfo.Accessmasks.ToString();
            bday.Text = __userinfo.Bday.Trim();
            credits.Text = __userinfo.Credits.ToString();
            digestposts.Text = __userinfo.Digestposts.ToString();
            email.Text = __userinfo.Email.Trim();
            gender.SelectedValue = __userinfo.Gender.ToString();
            groupexpiry.Text = __userinfo.Groupexpiry.ToString();

            if (__userinfo.Groupid.ToString() == "")
            {
                groupid.SelectedValue = "0";
            }
            else
            {
                try
                {
                    groupid.SelectedValue = __userinfo.Groupid.ToString();
                }
                catch
                {
                    groupid.SelectedValue = UserCredits.GetCreditsUserGroupID(__userinfo.Credits).Groupid.ToString();
                }
            }

            if (uid == BaseConfigs.GetFounderUid)
            {
                groupid.Enabled = false;
            }

            if (__userinfo.Groupid == 4)
            {
                StopTalk.Text = "取消禁言";
                StopTalk.HintInfo = "取消禁言将会把当前用户所在的 \\'系统禁言\\' 组进行系统调整成为非禁言组";
            }

            ViewState["Groupid"] = __userinfo.Groupid.ToString();

            invisible.SelectedValue = __userinfo.Invisible.ToString();
            joindate.Text = __userinfo.Joindate.ToString();
            lastactivity.Text = __userinfo.Lastactivity.ToString();
            lastip.Text = __userinfo.Lastip.Trim();
            lastpost.Text = __userinfo.Lastpost.ToString();
            lastvisit.Text = __userinfo.Lastvisit;
            newpm.SelectedValue = __userinfo.Newpm.ToString();
            switch (__userinfo.Newsletter)
            { 
                case ReceivePMSettingType.ReceiveNone:
                    SetNewsLetter(false, false, false);
                    break;
                case ReceivePMSettingType.ReceiveSystemPM:
                    SetNewsLetter(true, false, false);
                    break;
                case ReceivePMSettingType.ReceiveUserPM:
                    SetNewsLetter(false, true, false);
                    break;
                case ReceivePMSettingType.ReceiveAllPM:
                    SetNewsLetter(true, true, false);
                    break;
                case ReceivePMSettingType.ReceiveSystemPMWithHint:
                    SetNewsLetter(true, false, true);
                    break;
                case ReceivePMSettingType.ReceiveUserPMWithHint:
                    SetNewsLetter(false, true, true);
                    break;
                default:
                    SetNewsLetter(true, true, true);
                    break;
            }
            oltime.Text = __userinfo.Oltime.ToString();
            pageviews.Text = __userinfo.Pageviews.ToString();
            pmsound.Text = __userinfo.Pmsound.ToString();
            posts.Text = __userinfo.Posts.ToString();
            ppp.Text = __userinfo.Ppp.ToString();
            regip.Text = __userinfo.Regip.Trim();

            showemail.SelectedValue = __userinfo.Showemail.ToString();
            sigstatus.SelectedValue = __userinfo.Sigstatus.ToString();

            if ((__userinfo.Templateid.ToString() != "") && (__userinfo.Templateid.ToString() != "0"))
            {
                templateid.SelectedValue = __userinfo.Templateid.ToString();
            }

            tpp.Text = __userinfo.Tpp.ToString();

            extcredits1.Text = __userinfo.Extcredits1.ToString();
            extcredits2.Text = __userinfo.Extcredits2.ToString();
            extcredits3.Text = __userinfo.Extcredits3.ToString();
            extcredits4.Text = __userinfo.Extcredits4.ToString();
            extcredits5.Text = __userinfo.Extcredits5.ToString();
            extcredits6.Text = __userinfo.Extcredits6.ToString();
            extcredits7.Text = __userinfo.Extcredits7.ToString();
            extcredits8.Text = __userinfo.Extcredits8.ToString();


            //用户扩展信息
            website.Text = __userinfo.Website;
            icq.Text = __userinfo.Icq;
            qq.Text = __userinfo.Qq;
            yahoo.Text = __userinfo.Yahoo;
            msn.Text = __userinfo.Msn;
            skype.Text = __userinfo.Skype;
            location.Text = __userinfo.Location;
            customstatus.Text = __userinfo.Customstatus;
            avatar.Text = __userinfo.Avatar;
            avatarheight.Text = __userinfo.Avatarheight.ToString();
            avatarwidth.Text = __userinfo.Avatarwidth.ToString();
            bio.Text = __userinfo.Bio;
            signature.Text = __userinfo.Signature;
            realname.Text = __userinfo.Realname;
            idcard.Text = __userinfo.Idcard;
            mobile.Text = __userinfo.Mobile;
            phone.Text = __userinfo.Phone;

            givenusername.Text = __userinfo.Username;

            if (__userinfo.Medals.Trim() == "")
            {
                __userinfo.Medals = "0";
            }

            string begivenmedals = "," + __userinfo.Medals + ",";
            DataTable dt = DatabaseProvider.GetInstance().GetAvailableMedal();

            if (dt != null)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = "isgiven";
                dc.DataType = Type.GetType("System.Boolean");
                dc.DefaultValue = false;
                dc.AllowDBNull = false;
                dt.Columns.Add(dc);

                foreach (DataRow dr in dt.Rows)
                {
                    if (begivenmedals.IndexOf("," + dr["medalid"].ToString() + ",") >= 0)
                    {
                        dr["isgiven"] = true;
                    }
                }
                medalslist.DataSource = dt;
                medalslist.DataBind();
            }

            #endregion
        }

        private void SetNewsLetter(bool item1, bool item2, bool item3)
        {
            newsletter.Items[0].Selected = item2;
            newsletter.Items[1].Selected = item3;

            if (!item2)
            {
                newsletter.Items[1].Selected = false;
				newsletter.Items[1].Enabled = false;
            }
        }

        private int GetNewsLetter()
        {
            int item2 = 0;
            int item3 = 0;

            if (newsletter.Items[0].Selected)
            {
                item2 = 2;
            }
            if (newsletter.Items[1].Selected)
            {
                item3 = 4;
            }

            return item2 | item3;
        }

        private void IsEditUserName_CheckedChanged(object sender, EventArgs e)
        {
            #region 是否可以编辑用户名
            if (IsEditUserName.Checked)
            {
                userName.Enabled = true;
            }
            else
            {
                userName.Enabled = false;
            }
            #endregion
        }

        public string BeGivenMedal(string isgiven, string medalid)
        {
            #region 勋章的显示方式

            if (isgiven == "True")
            {
                return "<INPUT id=\"medalid\"  type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\" checked>";
            }
            else
            {
                return "<INPUT id=\"medalid\"  type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\">";
            }

            #endregion
        }


        private void GivenMedal_Click(object sender, EventArgs e)
        {
            #region 给予勋章

            if (this.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);
                GivenUserMedal(uid);

                if (DNTRequest.GetString("codition") == "")
                {
                    Discuz.Common.Utils.WriteCookie("codition", "", 0);
                }
                else
                {
                    Discuz.Common.Utils.WriteCookie("codition", DNTRequest.GetString("codition").Replace("^", "'"), 4*60);
                }

                base.RegisterStartupScript( "PAGE",  "window.location.href='global_edituser.aspx?uid=" + uid + "&condition=" + DNTRequest.GetString("condition") + "';");
            }

            #endregion
        }

        private void GivenUserMedal(int uid)
        {
            Discuz.Forum.Users.UpdateMedals(uid, DNTRequest.GetString("medalid"));
            string givenusername = Discuz.Forum.Users.GetUserInfo(uid).Username;
            foreach (string medalid in DNTRequest.GetString("medalid").Split(','))
            {
                if (medalid != "")
                {
                    if (!DatabaseProvider.GetInstance().IsExistMedalAwardRecord(int.Parse(medalid), uid))
                    {
                        DatabaseProvider.GetInstance().AddMedalslog(userid, username, DNTRequest.GetIP(), givenusername, uid, "授予", int.Parse(medalid), reason.Text.Trim());
                    }
                    else
                    {
                        DatabaseProvider.GetInstance().UpdateMedalslog("授予", DateTime.Now, reason.Text.Trim(), "收回", int.Parse(medalid), uid);
                    }
                }
            }

            if (DNTRequest.GetString("medalid") == "")
            {
                DatabaseProvider.GetInstance().UpdateMedalslog("收回", DateTime.Now, reason.Text.Trim(), uid);
            }
            else
            {
                DatabaseProvider.GetInstance().UpdateMedalslog("收回", DateTime.Now, reason.Text.Trim(), "授予", DNTRequest.GetString("medalid"), uid);
            }
        }

        private void ResetUserDigestPost_Click(object sender, EventArgs e)
        {
            #region 重设用户精华帖

            if (this.CheckCookie())
            {
                AdminForumStats.ReSetUserDigestPosts(DNTRequest.GetInt("uid", -1), DNTRequest.GetInt("uid", -1));
                base.RegisterStartupScript( "PAGE","window.location.href='global_edituser.aspx?uid=" + __userinfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';");
            }

            #endregion
        }


        private void ResetUserPost_Click(object sender, EventArgs e)
        {
            #region 重设用户发帖

            if (this.CheckCookie())
            {
                AdminForumStats.ReSetUserPosts(DNTRequest.GetInt("uid", -1), DNTRequest.GetInt("uid", -1));
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_edituser.aspx?uid=" + __userinfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';");
            }

            #endregion
        }


        private void ResetPassWord_Click(object sender, EventArgs e)
        {
            #region 重设用户密码

            if (this.CheckCookie())
            {
                if (!AllowEditUserInfo(DNTRequest.GetInt("uid", -1), true)) return;

                Response.Redirect("global_resetpassword.aspx?uid=" + DNTRequest.GetString("uid"));
            }

            #endregion
        }


        private void StopTalk_Click(object sender, EventArgs e)
        {
            #region 设置禁言

            if (this.CheckCookie())
            {
                __userinfo = AdminUsers.GetUserInfo(DNTRequest.GetInt("uid", -1));

                if (!AllowEditUserInfo(DNTRequest.GetInt("uid", -1), true)) return;

                if (ViewState["Groupid"].ToString() != "4") //当用户不是系统禁言组时
                {
                    if (__userinfo.Uid > 1) //判断是不是当前uid是不是系统初始化时生成的uid
                    {
                        DatabaseProvider.GetInstance().SetStopTalkUser(__userinfo.Uid.ToString());
                        base.RegisterStartupScript( "PAGE", "window.location.href='global_edituser.aspx?uid=" + __userinfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript( "", "<script>alert('操作失败,你要禁言的用户是系统初始化时的用户,因此不能操作!');window.location.href='global_edituser.aspx?uid=" + __userinfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';</script>");
                    }
                }
                else
                {
                    if (UserCredits.GetCreditsUserGroupID(0) != null)
                    {
                        int tmpGroupID = UserCredits.GetCreditsUserGroupID(__userinfo.Credits).Groupid;
                        DatabaseProvider.GetInstance().ChangeUserGroupByUid(tmpGroupID, __userinfo.Uid.ToString());
                        base.RegisterStartupScript( "PAGE", "window.location.href='global_edituser.aspx?uid=" + __userinfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript( "", "<script>alert('操作失败,系统未能找到合适的用户组来调整当前用户所处的组!');window.location.href='global_edituser.aspx?uid=" + __userinfo.Uid + "&condition=" + DNTRequest.GetString("condition") + "';</script>");
                    }
                }
                OnlineUsers.DeleteUserByUid(__userinfo.Uid);
            }

            #endregion
        }


        private void DelPosts_Click(object sender, EventArgs e)
        {
            #region 删除用户帖

            if (this.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);

                if (!AllowEditUserInfo(DNTRequest.GetInt("uid", -1), true)) return;

                //清除用户所发的帖子
                foreach (DataRow dr in DatabaseProvider.GetInstance().GetTableListInfo().Rows)
                {
                    if (dr["id"].ToString() != "")
                    {
                        DatabaseProvider.GetInstance().DeletePostByPosterid(int.Parse(dr["id"].ToString()), uid);
                    }
                }
                DatabaseProvider.GetInstance().DeleteTopicByPosterid(uid);
                DatabaseProvider.GetInstance().ClearPosts(uid);
                base.RegisterStartupScript( "PAGE", "window.location.href='global_edituser.aspx?uid=" + uid + "&condition=" + DNTRequest.GetString("condition") + "';");
            }

            #endregion
        }

        private void ReSendEmail_Click(object sender, EventArgs e)
        {
            #region 发送EMAIL

            string authstr = ForumUtils.CreateAuthStr(20);
            Emails.DiscuzSmtpMail(userName.Text, email.Text, "", authstr);
            string uid = DNTRequest.GetString("uid");
            //DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [Authstr]='" + authstr + "' , [Authtime]='" + DateTime.Now.ToString() + "' ,[Authflag]=1  WHERE [uid]=" + uid);
            DatabaseProvider.GetInstance().UpdateEmailValidateInfo(authstr, DateTime.Now, int.Parse(uid));
            base.RegisterStartupScript( "PAGE", "window.location.href='global_edituser.aspx?uid=" + uid + "&condition=" + DNTRequest.GetString("condition") + "';");

            #endregion
        }

        private void SaveUserInfo_Click(object sender, EventArgs e)
        {
            #region 保存用户信息

            if (this.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);
                string errorInfo = "";

                if (!AllowEditUserInfo(uid, true)) return;

                if (userName.Text != ViewState["username"].ToString())
                {
                    if (AdminUsers.GetUserID(userName.Text) != -1)
                    {
                        base.RegisterStartupScript( "", "<script>alert('您所输入的用户名已被使用过, 请输入其他的用户名!');</script>");
                        return;
                    }
                }

                if (userName.Text == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('用户名不能为空!');</script>");
                    return;
                }

                if (groupid.SelectedValue == "0")
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何用户组!');</script>");
                    return;
                }

                __userinfo = AdminUsers.GetUserInfo(uid);
                __userinfo.Username = userName.Text;
                __userinfo.Nickname = nickname.Text;
                __userinfo.Accessmasks = Convert.ToInt32(accessmasks.SelectedValue);

                //当用户组发生变化时则相应更新用户的管理组字段
                if (__userinfo.Groupid.ToString() != groupid.SelectedValue)
                {
                    __userinfo.Adminid = DatabaseProvider.GetInstance().GetRadminidByGroupid(int.Parse(groupid.SelectedValue));
                }

                __userinfo.Avatarshowid = 0;

                if ((bday.Text == "0000-00-00") || (bday.Text == "0000-0-0") | (bday.Text.Trim() == ""))
                {
                    __userinfo.Bday = "";
                }
                else
                {
                    if (!Utils.IsDateString(bday.Text.Trim()))
                    {
                        base.RegisterStartupScript( "", "<script>alert('用户生日不是有效的日期型数据!');</script>");
                        return;
                    }
                    else
                    {
                        __userinfo.Bday = bday.Text;
                    }
                }

                if (Utils.IsNumeric(credits.Text.Replace("-", "")))
                {
                    __userinfo.Credits = Convert.ToInt32(credits.Text);
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('用户的金币不能为空或大于9位 !');</script>");
                    return;
                }

                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                if (__configinfo.Doublee == 0)
                {
                    int currentuid = AdminUsers.FindUserEmail(email.Text);
                    if ((currentuid != -1) && (currentuid != uid))
                    {
                        base.RegisterStartupScript( "", "<script>alert('当前用户的邮箱地址已被使用过, 请输入其他的邮箱!');</script>");
                        return;
                    }
                }

                __userinfo.Email = email.Text;
                __userinfo.Gender = Convert.ToInt32(gender.SelectedValue);
                __userinfo.Groupexpiry = Convert.ToInt32(groupexpiry.Text);
                __userinfo.Extgroupids = extgroupids.GetSelectString(",");

                if ((groupid.SelectedValue != "1") && (__userinfo.Uid == 1))
                {
                    base.RegisterStartupScript( "", "<script>alert('初始化系统管理员的所属用户组设置不能修改为其它组!');window.location.href='global_edituser.aspx?uid=" + DNTRequest.GetString("uid") + "';</script>");
                    return;
                }

                __userinfo.Groupid = Convert.ToInt32(groupid.SelectedValue);
                __userinfo.Invisible = Convert.ToInt32(invisible.SelectedValue);
                __userinfo.Joindate = joindate.Text;
                __userinfo.Lastactivity = lastactivity.Text;
                __userinfo.Lastip = lastip.Text;
                __userinfo.Lastpost = lastpost.Text;
                __userinfo.Lastvisit = lastvisit.Text;
                __userinfo.Newpm = Convert.ToInt32(newpm.SelectedValue);
                __userinfo.Newsletter = (ReceivePMSettingType)GetNewsLetter();
                __userinfo.Oltime = Convert.ToInt32(oltime.Text);
                __userinfo.Pageviews = Convert.ToInt32(pageviews.Text);
                __userinfo.Pmsound = Convert.ToInt32(pmsound.Text);
                __userinfo.Posts = Convert.ToInt32(posts.Text);
                __userinfo.Ppp = Convert.ToInt32(ppp.Text);
                __userinfo.Regip = regip.Text;
                __userinfo.Digestposts = Convert.ToInt32(digestposts.Text);
            
                if (secques.SelectedValue == "1") __userinfo.Secques = ""; //清空安全码

                __userinfo.Showemail = Convert.ToInt32(showemail.SelectedValue);
                __userinfo.Sigstatus = Convert.ToInt32(sigstatus.SelectedValue);
                __userinfo.Templateid = Convert.ToInt32(templateid.SelectedValue);
                __userinfo.Tpp = Convert.ToInt32(tpp.Text);


                if (Utils.IsNumeric(extcredits1.Text.Replace("-", "")))
                {
                    __userinfo.Extcredits1 = float.Parse(extcredits1.Text);
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('用户扩展金币不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits2.Text.Replace("-", "")))
                {
                    __userinfo.Extcredits2 = float.Parse(extcredits2.Text);
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('用户扩展金币不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits3.Text.Replace("-", "")))
                {
                    __userinfo.Extcredits3 = float.Parse(extcredits3.Text);
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('用户扩展金币不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits4.Text.Replace("-", "")))
                {
                    __userinfo.Extcredits4 = float.Parse(extcredits4.Text);
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('用户扩展金币不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits5.Text.Replace("-", "")))
                {
                    __userinfo.Extcredits5 = float.Parse(extcredits5.Text);
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('用户扩展金币不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits6.Text.Replace("-", "")))
                {
                    __userinfo.Extcredits6 = float.Parse(extcredits6.Text);
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('用户扩展金币不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits7.Text.Replace("-", "")))
                {
                    __userinfo.Extcredits7 = float.Parse(extcredits7.Text);
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('用户扩展金币不能为空或大于7位 !');</script>");
                    return;
                }

                if (Utils.IsNumeric(extcredits8.Text.Replace("-", "")))
                {
                    __userinfo.Extcredits8 = float.Parse(extcredits8.Text);
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('用户扩展金币不能为空或大于7位 !');</script>");
                    return;
                }


                //用户扩展信息
                __userinfo.Website = website.Text;
                __userinfo.Icq = icq.Text;
                __userinfo.Qq = qq.Text;
                __userinfo.Yahoo = yahoo.Text;
                __userinfo.Msn = msn.Text;
                __userinfo.Skype = skype.Text;
                __userinfo.Location = location.Text;
                __userinfo.Customstatus = customstatus.Text;
                __userinfo.Avatar = avatar.Text;
                __userinfo.Avatarheight = Convert.ToInt32(avatarheight.Text);
                __userinfo.Avatarwidth = Convert.ToInt32(avatarwidth.Text);
                __userinfo.Bio = bio.Text;
                if (signature.Text.Length > UserGroups.GetUserGroupInfo(__userinfo.Groupid).Maxsigsize)
                {
                    errorInfo = "更新的签名长度超过 " + UserGroups.GetUserGroupInfo(__userinfo.Groupid).Maxsigsize + " 字符的限制，未能更新。";
                }
                else
                {
                    __userinfo.Signature = signature.Text;
                    //签名UBB转换HTML
                    PostpramsInfo _postpramsinfo = new PostpramsInfo();
                    _postpramsinfo.Showimages = UserGroups.GetUserGroupInfo(__userinfo.Groupid).Allowsigimgcode;
                    _postpramsinfo.Sdetail = signature.Text;
                    __userinfo.Sightml = UBB.UBBToHTML(_postpramsinfo);
                }

                __userinfo.Realname = realname.Text;
                __userinfo.Idcard = idcard.Text;
                __userinfo.Mobile = mobile.Text;
                __userinfo.Phone = phone.Text;
                __userinfo.Medals = DNTRequest.GetString("medalid");

                if (IsEditUserName.Checked)
                {
                    AdminUsers.UserNameChange(__userinfo, ViewState["username"].ToString());
                }

                if (AdminUsers.UpdateUserAllInfo(__userinfo))
                {
                    if (userName.Text != ViewState["username"].ToString())
                    {
                        AdminUsers.UserNameChange(__userinfo, ViewState["username"].ToString());
                    }
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台编辑用户", "用户名:" + userName.Text);
                    if (errorInfo == "")
                    {
                        base.RegisterStartupScript("PAGE", "window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript("PAGE", "alert('" + errorInfo + "');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';");
                    }
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';</script>");
                }
            }

            #endregion
        }

        private void DelUserInfo_Click(object sender, EventArgs e)
        {
            #region 删除指定用户信息

            if (this.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);

                if (!AllowEditUserInfo(uid, true)) return;

                //if (uid > 1) //判断是不是当前uid是不是系统初始化时生成的uid
                if (AllowDeleteUser(this.userid, uid))
                {
                    bool delpost = deltype.SelectedValue.IndexOf("1") >= 0 ? false : true;
                    bool delpms = deltype.SelectedValue.IndexOf("2") >= 0 ? false : true;

                    if (AdminUsers.DelUserAllInf(uid, delpost, delpms))
                    {
                        AdminUsers.UpdateForumsFieldModerators(userName.Text);

                        AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除用户", "用户名:" + userName.Text);
                        base.RegisterStartupScript( "PAGE",  "window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript( "", "<script>alert('操作失败');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';</script>");
                    }
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,你要删除的用户是创始人用户或是其它管理员,因此不能删除!');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';</script>");
                }
            }

            #endregion
        }

        private bool AllowDeleteUser( int managerUId, int byDeleterUId)
        {
            #region 判断将要删除的用户是否是创始人
            int managerGroupId = Discuz.Forum.Users.GetUserInfo(managerUId).Groupid;
            int byDeleterGruopid = Discuz.Forum.Users.GetUserInfo(byDeleterUId).Groupid;
            int founderUId = BaseConfigs.GetBaseConfig().Founderuid;
            if (byDeleterUId == founderUId) //判断被删除人是否为创始人
            {
                return false;
            }
            else if (managerUId != founderUId && managerGroupId == byDeleterGruopid)    //判断被删除人是否为相同组，即是否都是管理员，管理员不能相互删除
            {
                return false;
            }
            else
            {
                return true;
            }
            #endregion
        }

        private void CalculatorScore_Click(object sender, EventArgs e)
        {
            #region 计算金币
            if (this.CheckCookie())
            {
                credits.Text = UserCredits.GetUserCreditsByUid(DNTRequest.GetInt("uid", -1)).ToString();
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.StopTalk.Click += new EventHandler(this.StopTalk_Click);
            this.DelPosts.Click += new EventHandler(this.DelPosts_Click);
            this.SaveUserInfo.Click += new EventHandler(this.SaveUserInfo_Click);
            this.ResetPassWord.Click += new EventHandler(this.ResetPassWord_Click);
            this.IsEditUserName.CheckedChanged += new EventHandler(this.IsEditUserName_CheckedChanged);

            this.DelUserInfo.Click += new EventHandler(this.DelUserInfo_Click);
            this.ReSendEmail.Click += new EventHandler(this.ReSendEmail_Click);
            this.CalculatorScore.Click += new EventHandler(this.CalculatorScore_Click);
            this.ResetUserDigestPost.Click += new EventHandler(this.ResetUserDigestPost_Click);
            this.ResetUserPost.Click += new EventHandler(this.ResetUserPost_Click);

            this.GivenMedal.Click += new EventHandler(this.GivenMedal_Click);

            this.Load += new EventHandler(this.Page_Load);

            extgroupids.AddTableData(DatabaseProvider.GetInstance().GetGroupInfo());

            groupid.AddTableData(DatabaseProvider.GetInstance().GetGroupInfo());
            templateid.AddTableData(DatabaseProvider.GetInstance().GetTemplateInfo());
            templateid.Items[0].Text = "默认";
            TabControl1.InitTabPage();
            if (DNTRequest.GetString("uid") == "")
            {
                Response.Redirect("global_usergrid.aspx");
                return;
            }
            LoadCurrentUserInfo(DNTRequest.GetInt("uid", -1));
            LoadScoreInf(DNTRequest.GetString("uid"), DNTRequest.GetString("fieldname"));

        }

        #endregion

    }
}