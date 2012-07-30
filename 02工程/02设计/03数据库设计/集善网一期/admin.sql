/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2012/5/18 11:39:26                           */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('Actions')
            and   type = 'U')
   drop table Actions
go

if exists (select 1
            from  sysobjects
           where  id = object_id('RelRoleAction')
            and   type = 'U')
   drop table RelRoleAction
go

if exists (select 1
            from  sysobjects
           where  id = object_id('RelUserRole')
            and   type = 'U')
   drop table RelUserRole
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SysRole')
            and   type = 'U')
   drop table SysRole
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SysUser')
            and   type = 'U')
   drop table SysUser
go

/*==============================================================*/
/* Table: Actions                                               */
/*==============================================================*/
create table Actions (
   ID                   int                  not null,
   MenuCode             varchar(100)         null,
   MenuName             varchar(50)          not null,
   ParentID             int                  null,
   IsMenu               bit                  not null,
   IsAjax               bit                  not null,
   ResourceKey          varchar(100)         null,
   IsNeedAuth           bit                  not null,
   SortNumber           int                  null,
   constraint PK_ACTIONS primary key (ID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Controller_Action',
   'user', @CurrentUser, 'table', 'Actions', 'column', 'MenuCode'
go

/*==============================================================*/
/* Table: RelRoleAction                                         */
/*==============================================================*/
create table RelRoleAction (
   RoleId               int                  not null,
   ActionId             int                  not null,
   constraint PK_RELROLEACTION primary key (RoleId, ActionId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '½ÇÉ«ID',
   'user', @CurrentUser, 'table', 'RelRoleAction', 'column', 'RoleId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '¹¦ÄÜID',
   'user', @CurrentUser, 'table', 'RelRoleAction', 'column', 'ActionId'
go

/*==============================================================*/
/* Table: RelUserRole                                           */
/*==============================================================*/
create table RelUserRole (
   UserId               int                  not null,
   RoleId               int                  not null,
   constraint PK_RELUSERROLE primary key (UserId, RoleId)
)
go

/*==============================================================*/
/* Table: SysRole                                               */
/*==============================================================*/
create table SysRole (
   Id                   int                  identity,
   RoleName             nvarchar(50)         not null,
   constraint PK_SYSROLE primary key (Id)
)
go

/*==============================================================*/
/* Table: SysUser                                               */
/*==============================================================*/
create table SysUser (
   Id                   int                  identity,
   LogonName            varchar(20)          not null,
   TrueName             varchar(20)          not null,
   Password             varchar(100)         not null,
   Phone                varchar(20)          null,
   Email                varchar(100)         null,
   constraint PK_SYSUSER primary key (Id)
)
go

