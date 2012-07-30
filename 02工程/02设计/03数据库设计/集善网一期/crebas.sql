/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2012/5/29 22:02:01                           */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Drawings') and o.name = 'FK_DRAWINGS_奖品-抽奖记录_PRIZE')
alter table Drawings
   drop constraint "FK_DRAWINGS_奖品-抽奖记录_PRIZE"
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Drawings') and o.name = 'FK_DRAWINGS_爱心捐赠-抽奖记录_LOVECHAN')
alter table Drawings
   drop constraint "FK_DRAWINGS_爱心捐赠-抽奖记录_LOVECHAN"
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('OrderDetail') and o.name = 'FK_ORDERDET_订单-订单明细_ORDER')
alter table OrderDetail
   drop constraint "FK_ORDERDET_订单-订单明细_ORDER"
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Prize') and o.name = 'FK_PRIZE_类别-奖品_CATEGORY')
alter table Prize
   drop constraint "FK_PRIZE_类别-奖品_CATEGORY"
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Account')
            and   type = 'U')
   drop table Account
go

if exists (select 1
            from  sysobjects
           where  id = object_id('AccountLog')
            and   type = 'U')
   drop table AccountLog
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Category')
            and   type = 'U')
   drop table Category
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Content')
            and   type = 'U')
   drop table Content
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Drawings')
            and   type = 'U')
   drop table Drawings
go

if exists (select 1
            from  sysobjects
           where  id = object_id('FriendLink')
            and   type = 'U')
   drop table FriendLink
go

if exists (select 1
            from  sysobjects
           where  id = object_id('LoveChange')
            and   type = 'U')
   drop table LoveChange
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"Order"')
            and   type = 'U')
   drop table "Order"
go

if exists (select 1
            from  sysobjects
           where  id = object_id('OrderDetail')
            and   type = 'U')
   drop table OrderDetail
go

if exists (select 1
            from  sysobjects
           where  id = object_id('PayLog')
            and   type = 'U')
   drop table PayLog
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Prize')
            and   type = 'U')
   drop table Prize
go

if exists (select 1
            from  sysobjects
           where  id = object_id('UserInfo')
            and   type = 'U')
   drop table UserInfo
go

/*==============================================================*/
/* Table: Account                                               */
/*==============================================================*/
create table Account (
   Id                   bigint               identity,
   UserId               bigint               not null,
   Amount               decimal(18,2)        not null,
   UserLevel            int                  not null,
   Score                int                  not null,
   IsStop               bit                  not null,
   CreateTime           datetime             not null,
   UpdateTime           datetime             not null,
   FrozenAmount         decimal(18,2)        not null,
   constraint PK_ACCOUNT primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '帐户ID',
   'user', @CurrentUser, 'table', 'Account', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户ID',
   'user', @CurrentUser, 'table', 'Account', 'column', 'UserId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '帐户余额',
   'user', @CurrentUser, 'table', 'Account', 'column', 'Amount'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户级别',
   'user', @CurrentUser, 'table', 'Account', 'column', 'UserLevel'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '积分',
   'user', @CurrentUser, 'table', 'Account', 'column', 'Score'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否停用',
   'user', @CurrentUser, 'table', 'Account', 'column', 'IsStop'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'Account', 'column', 'CreateTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '更新时间',
   'user', @CurrentUser, 'table', 'Account', 'column', 'UpdateTime'
go

/*==============================================================*/
/* Table: AccountLog                                            */
/*==============================================================*/
create table AccountLog (
   LogId                int                  identity,
   UserId               bigint               not null,
   OrderId              uniqueidentifier     not null,
   Amount               decimal(18,2)        not null,
   AccountWay           int                  not null,
   Ip                   varchar(15)          not null,
   AdminRemark          varchar(300)         null,
   CreateTime           datetime             null,
   PayWay               int                  null,
   constraint PK_ACCOUNTLOG primary key (LogId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '日志ID',
   'user', @CurrentUser, 'table', 'AccountLog', 'column', 'LogId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户ID',
   'user', @CurrentUser, 'table', 'AccountLog', 'column', 'UserId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '订单ID',
   'user', @CurrentUser, 'table', 'AccountLog', 'column', 'OrderId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '金额',
   'user', @CurrentUser, 'table', 'AccountLog', 'column', 'Amount'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '收支途径',
   'user', @CurrentUser, 'table', 'AccountLog', 'column', 'AccountWay'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '访问IP',
   'user', @CurrentUser, 'table', 'AccountLog', 'column', 'Ip'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '管理备注',
   'user', @CurrentUser, 'table', 'AccountLog', 'column', 'AdminRemark'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'AccountLog', 'column', 'CreateTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '支付方式与订单表的支付方式一致',
   'user', @CurrentUser, 'table', 'AccountLog', 'column', 'PayWay'
go

/*==============================================================*/
/* Table: Category                                              */
/*==============================================================*/
create table Category (
   ID                   bigint               not null,
   Name                 varchar(Max)         not null,
   constraint PK_CATEGORY primary key (ID)
)
go

/*==============================================================*/
/* Table: Content                                               */
/*==============================================================*/
create table Content (
   Id                   int                  identity,
   Title                nvarchar(100)        null,
   Description          nvarchar(500)        null,
   LinkUrl              varchar(1000)        null,
   PicUrl               varchar(200)         null,
   AltName              nvarchar(100)        null,
   TagName              varchar(20)          null,
   ContentType          int                  not null,
   constraint PK_CONTENT primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用于输出文字链、图文链、图片链等不同类型的内容',
   'user', @CurrentUser, 'table', 'Content'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1 - 文字链
   2 - 图文链
   3 - 图片链',
   'user', @CurrentUser, 'table', 'Content', 'column', 'ContentType'
go

/*==============================================================*/
/* Table: Drawings                                              */
/*==============================================================*/
create table Drawings (
   ID                   bigint               not null,
   PrizeID              bigint               null,
   DonationID           bigint               null,
   isConfirrmed         bit                  null,
   isWinner             bit                  null,
   constraint PK_DRAWINGS primary key (ID)
)
go

/*==============================================================*/
/* Table: FriendLink                                            */
/*==============================================================*/
create table FriendLink (
   Id                   numeric              identity,
   Title                nvarchar(200)        not null,
   LinkUrl              varchar(500)         not null,
   PicUrl               varchar(500)         null,
   OrderNo              int                  not null,
   Type                 int                  not null,
   constraint PK_FRIENDLINK primary key (Id)
)
go

/*==============================================================*/
/* Table: LoveChange                                            */
/*==============================================================*/
create table LoveChange (
   Id                   bigint               identity,
   UnionOrder           varchar(20)          not null,
   Amount               decimal(6,2)         not null,
   TrueName             nvarchar(20)         null,
   IsSuccess            bit                  null,
   IsGame               bit                  not null,
   CreateTime           datetime             not null,
   GameTime             datetime             null,
   UserId               bigint               null,
   constraint PK_LOVECHANGE primary key (Id)
)
go

/*==============================================================*/
/* Table: "Order"                                               */
/*==============================================================*/
create table "Order" (
   OrderId              uniqueidentifier     not null,
   OrderType            int                  not null,
   UserId               bigint               not null,
   Email                varchar(50)          null,
   TrueName             nvarchar(20)         null,
   Mobile               varchar(20)          null,
   IdentityCard         varchar(20)          null,
   Total                decimal(18,2)        not null,
   PayWay               int                  not null,
   Ip                   varchar(15)          not null,
   Status               int                  not null,
   CreateTime           datetime             not null,
   PayTime              datetime             null,
   Remark               varchar(300)         null,
   AdminRemark          varchar(300)         null,
   UpdateTime           datetime             null,
   constraint PK_ORDER primary key (OrderId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '订单ID',
   'user', @CurrentUser, 'table', 'Order', 'column', 'OrderId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1：爱心捐赠
   ',
   'user', @CurrentUser, 'table', 'Order', 'column', 'OrderType'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户ID',
   'user', @CurrentUser, 'table', 'Order', 'column', 'UserId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户Email',
   'user', @CurrentUser, 'table', 'Order', 'column', 'Email'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '订单总金额',
   'user', @CurrentUser, 'table', 'Order', 'column', 'Total'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0：账户余额
   1：中国银联',
   'user', @CurrentUser, 'table', 'Order', 'column', 'PayWay'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '客户IP',
   'user', @CurrentUser, 'table', 'Order', 'column', 'Ip'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '-20=交易失败
   -10=已取消
   0=新订单
   10=已付款
   20=已完成',
   'user', @CurrentUser, 'table', 'Order', 'column', 'Status'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'Order', 'column', 'CreateTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '支付时间',
   'user', @CurrentUser, 'table', 'Order', 'column', 'PayTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '订单备注',
   'user', @CurrentUser, 'table', 'Order', 'column', 'Remark'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '管理员备注',
   'user', @CurrentUser, 'table', 'Order', 'column', 'AdminRemark'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '更新时间',
   'user', @CurrentUser, 'table', 'Order', 'column', 'UpdateTime'
go

/*==============================================================*/
/* Table: OrderDetail                                           */
/*==============================================================*/
create table OrderDetail (
   OrderDetailId        bigint               identity,
   OrderId              uniqueidentifier     not null,
   ProductId            int                  not null,
   ProductName          nvarchar(200)        not null,
   Price                decimal(18,2)        not null,
   ProductCount         int                  not null,
   constraint PK_ORDERDETAIL primary key (OrderDetailId)
)
go

/*==============================================================*/
/* Table: PayLog                                                */
/*==============================================================*/
create table PayLog (
   PayId                int                  identity,
   OrderId              uniqueidentifier     not null,
   TransactionId        varchar(50)          null,
   UserId               bigint               not null,
   PayWay               int                  not null,
   PayMoney             decimal(18,2)        not null,
   PayUrl               varchar(Max)         null,
   BackUrl              varchar(Max)         null,
   PayResult            varchar(Max)         null,
   CreateTime           datetime             not null,
   constraint PK_PAYLOG primary key (PayId)
)
go

/*==============================================================*/
/* Table: Prize                                                 */
/*==============================================================*/
create table Prize (
   ID                   bigint               not null,
   CategoryID           bigint               not null,
   Name                 varchar(Max)         not null,
   ImgURL               varchar(Max)         null,
   constraint PK_PRIZE primary key (ID)
)
go

/*==============================================================*/
/* Table: UserInfo                                              */
/*==============================================================*/
create table UserInfo (
   Id                   bigint               identity,
   UserName             nvarchar(20)         not null,
   Password             varchar(100)         not null,
   Email                varchar(100)         null,
   TrueName             nvarchar(10)         null,
   IdentityCardNo       varchar(18)          null,
   Tel                  varchar(20)          null,
   Phone                varchar(11)          null,
   RegFrom              int                  not null,
   CreateTime           datetime             not null,
   UpdateTime           datetime             not null,
   constraint PK_USERINFO primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户信息表',
   'user', @CurrentUser, 'table', 'UserInfo'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户ID',
   'user', @CurrentUser, 'table', 'UserInfo', 'column', 'Id'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '真实姓名',
   'user', @CurrentUser, 'table', 'UserInfo', 'column', 'TrueName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '身份证号码',
   'user', @CurrentUser, 'table', 'UserInfo', 'column', 'IdentityCardNo'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0 - 集善网直接注册
   1 - 银联爱心支付跳转
   2 - 爱心捐赠',
   'user', @CurrentUser, 'table', 'UserInfo', 'column', 'RegFrom'
go

alter table Drawings
   add constraint "FK_DRAWINGS_奖品-抽奖记录_PRIZE" foreign key (PrizeID)
      references Prize (ID)
go

alter table Drawings
   add constraint "FK_DRAWINGS_爱心捐赠-抽奖记录_LOVECHAN" foreign key (DonationID)
      references LoveChange (Id)
go

alter table OrderDetail
   add constraint "FK_ORDERDET_订单-订单明细_ORDER" foreign key (OrderId)
      references "Order" (OrderId)
go

alter table Prize
   add constraint "FK_PRIZE_类别-奖品_CATEGORY" foreign key (CategoryID)
      references Category (ID)
go

