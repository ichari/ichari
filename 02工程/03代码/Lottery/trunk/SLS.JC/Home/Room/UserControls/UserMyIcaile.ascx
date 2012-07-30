<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserMyIcaile.ascx.cs" Inherits="Home_Room_UserControls_UserMyIcaile" %>
<script type="text/javascript">
    $().ready(function(){
        $('#<%= CurrentPage %>').attr("class", "usrCurrent");
    });
</script>
<div class="user_navbar">
<div class="w200 fl">
    <h2>个人中心</h2>
        <div id="usrMenu" class="current">
            <h3><img class="icon_right" src="/Images/_New/space_a.gif" />我的账户</h3>
            <ul>
                <li id="acView"><a href="/Home/Room/ViewAccount.aspx">账户全览</a></li>
                <li id="acMessage"><a href="/Home/Room/Message.aspx">我的站内信</a></li>
                <li id="acTrans"><a href="/Home/Room/AccountDetail.aspx">交易明细</a></li>
                <li id="acDeposit"><a href="/Home/Room/OnlinePay/Default.aspx">帐户充值</a></li>
                <li id="acWidthdraw"><a href="/Home/Room/Distill.aspx">我要提款</a></li>
            </ul>
            <h3><img class="icon_right" src="/Images/_New/space_a.gif" />我的彩票记录</h3>
            <ul>
            	<li id="cpLotto"><a href="/Home/Room/Invest.aspx">投注查询</a></li>
                <li id="cpFollow"><a href="/Home/Room/FollowSchemeHistory.aspx">我的自动跟单</a></li>
                <li id="cpChase"><a href="/Home/Room/ViewChase.aspx">我的追号</a></li>
            </ul>
            <h3><img class="icon_right" src="/Images/_New/space_a.gif" />我的资料</h3>
            <ul>
            	<li id="acInfo"><a href="/Home/Room/UserEdit.aspx">修改基本资料</a></li>
                <%--<li id=""><a href="/Home/Room/UserMobileBind.aspx">手机验证</a></li>--%>
                <li id="acEmail"><a href="/Home/Room/UserEmailBind.aspx">邮箱激活</a></li>
                <%--<li id="acPwd"><a href="/Home/Room/EditPassWord.aspx">修改密码</a></li>--%>
                <li id="acPwd"><a href="<%= System.Configuration.ConfigurationManager.AppSettings["JsHeaderUrl"] %>/account/changepassword">修改密码</a></li>
                <li id="acRecovery"><a href="/Home/Room/SafeSet.aspx?FromUrl=SafeSet.aspx">设置安全问题</a></li>
                <li id="acBankCard"><a href="/Home/Room/BindBankCard.aspx">绑定银行卡</a></li>
            </ul>
            <h3><img class="icon_right" src="/Images/_New/space_a.gif" />我的积分</h3>
            <ul>
            	<li id="ptMyPoints"><a href="/Home/Room/ScoringDetail.aspx">我的积分明细</a></li>
                <%--<li><a href="/Home/Room/ScoringChange.aspx">积分兑换</a></li>--%>
            </ul>
            <%--<h3><img class="icon_right" src="/Images/_New/space_a.gif" />我的推广</h3>
            <ul class="noline">
            	<li id="pmMyPromo"><a href="/Home/Room/MyPromotion/MemberPromotion.aspx">我推广的会员</a></li>
                <li id="pmRedeem"><a href="/Home/Room/Distill.aspx?IsCps=1">佣金提款</a></li>
            </ul>--%>
        </div>
    </div>
</div>