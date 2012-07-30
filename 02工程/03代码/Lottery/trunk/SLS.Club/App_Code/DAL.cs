using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using Shove.Database;

namespace DAL
{
    /*
    Program Name: Shove.DAL.30
    Program Version: 3.0
    Writer By: 3km.shovesoft.shove (zhou changjun)
    Release Time: 2008.9.1

    System Request: Shove.dll
    All Rights saved.
    */


    // Please Add a Key in Web.config File's appSetting section, Exemple:
    // <add key="ConnectionString" value="server=(local);User id=sa;Pwd=;Database=master" />


    public class Tables
    {
        public class dnt_admingroups : MSSQL.TableBase
        {
            public MSSQL.Field admingid;
            public MSSQL.Field alloweditpost;
            public MSSQL.Field alloweditpoll;
            public MSSQL.Field allowstickthread;
            public MSSQL.Field allowmodpost;
            public MSSQL.Field allowdelpost;
            public MSSQL.Field allowmassprune;
            public MSSQL.Field allowrefund;
            public MSSQL.Field allowcensorword;
            public MSSQL.Field allowviewip;
            public MSSQL.Field allowbanip;
            public MSSQL.Field allowedituser;
            public MSSQL.Field allowmoduser;
            public MSSQL.Field allowbanuser;
            public MSSQL.Field allowpostannounce;
            public MSSQL.Field allowviewlog;
            public MSSQL.Field disablepostctrl;
            public MSSQL.Field allowviewrealname;

            public dnt_admingroups()
            {
                TableName = "dnt_admingroups";

                admingid = new MSSQL.Field(this, "admingid", "admingid", SqlDbType.SmallInt, false);
                alloweditpost = new MSSQL.Field(this, "alloweditpost", "alloweditpost", SqlDbType.TinyInt, false);
                alloweditpoll = new MSSQL.Field(this, "alloweditpoll", "alloweditpoll", SqlDbType.TinyInt, false);
                allowstickthread = new MSSQL.Field(this, "allowstickthread", "allowstickthread", SqlDbType.TinyInt, false);
                allowmodpost = new MSSQL.Field(this, "allowmodpost", "allowmodpost", SqlDbType.TinyInt, false);
                allowdelpost = new MSSQL.Field(this, "allowdelpost", "allowdelpost", SqlDbType.TinyInt, false);
                allowmassprune = new MSSQL.Field(this, "allowmassprune", "allowmassprune", SqlDbType.TinyInt, false);
                allowrefund = new MSSQL.Field(this, "allowrefund", "allowrefund", SqlDbType.TinyInt, false);
                allowcensorword = new MSSQL.Field(this, "allowcensorword", "allowcensorword", SqlDbType.TinyInt, false);
                allowviewip = new MSSQL.Field(this, "allowviewip", "allowviewip", SqlDbType.TinyInt, false);
                allowbanip = new MSSQL.Field(this, "allowbanip", "allowbanip", SqlDbType.TinyInt, false);
                allowedituser = new MSSQL.Field(this, "allowedituser", "allowedituser", SqlDbType.TinyInt, false);
                allowmoduser = new MSSQL.Field(this, "allowmoduser", "allowmoduser", SqlDbType.TinyInt, false);
                allowbanuser = new MSSQL.Field(this, "allowbanuser", "allowbanuser", SqlDbType.TinyInt, false);
                allowpostannounce = new MSSQL.Field(this, "allowpostannounce", "allowpostannounce", SqlDbType.TinyInt, false);
                allowviewlog = new MSSQL.Field(this, "allowviewlog", "allowviewlog", SqlDbType.TinyInt, false);
                disablepostctrl = new MSSQL.Field(this, "disablepostctrl", "disablepostctrl", SqlDbType.TinyInt, false);
                allowviewrealname = new MSSQL.Field(this, "allowviewrealname", "allowviewrealname", SqlDbType.TinyInt, false);
            }
        }

        public class dnt_adminvisitlog : MSSQL.TableBase
        {
            public MSSQL.Field visitid;
            public MSSQL.Field uid;
            public MSSQL.Field username;
            public MSSQL.Field groupid;
            public MSSQL.Field grouptitle;
            public MSSQL.Field ip;
            public MSSQL.Field postdatetime;
            public MSSQL.Field actions;
            public MSSQL.Field others;

            public dnt_adminvisitlog()
            {
                TableName = "dnt_adminvisitlog";

                visitid = new MSSQL.Field(this, "visitid", "visitid", SqlDbType.Int, true);
                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                username = new MSSQL.Field(this, "username", "username", SqlDbType.VarChar, false);
                groupid = new MSSQL.Field(this, "groupid", "groupid", SqlDbType.Int, false);
                grouptitle = new MSSQL.Field(this, "grouptitle", "grouptitle", SqlDbType.VarChar, false);
                ip = new MSSQL.Field(this, "ip", "ip", SqlDbType.VarChar, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                actions = new MSSQL.Field(this, "actions", "actions", SqlDbType.VarChar, false);
                others = new MSSQL.Field(this, "others", "others", SqlDbType.VarChar, false);
            }
        }

        public class dnt_advertisements : MSSQL.TableBase
        {
            public MSSQL.Field advid;
            public MSSQL.Field available;
            public MSSQL.Field type;
            public MSSQL.Field displayorder;
            public MSSQL.Field title;
            public MSSQL.Field targets;
            public MSSQL.Field starttime;
            public MSSQL.Field endtime;
            public MSSQL.Field code;
            public MSSQL.Field parameters;

            public dnt_advertisements()
            {
                TableName = "dnt_advertisements";

                advid = new MSSQL.Field(this, "advid", "advid", SqlDbType.Int, true);
                available = new MSSQL.Field(this, "available", "available", SqlDbType.Int, false);
                type = new MSSQL.Field(this, "type", "type", SqlDbType.NVarChar, false);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                title = new MSSQL.Field(this, "title", "title", SqlDbType.NVarChar, false);
                targets = new MSSQL.Field(this, "targets", "targets", SqlDbType.NVarChar, false);
                starttime = new MSSQL.Field(this, "starttime", "starttime", SqlDbType.DateTime, false);
                endtime = new MSSQL.Field(this, "endtime", "endtime", SqlDbType.DateTime, false);
                code = new MSSQL.Field(this, "code", "code", SqlDbType.NText, false);
                parameters = new MSSQL.Field(this, "parameters", "parameters", SqlDbType.NText, false);
            }
        }

        public class dnt_announcements : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field poster;
            public MSSQL.Field posterid;
            public MSSQL.Field title;
            public MSSQL.Field displayorder;
            public MSSQL.Field starttime;
            public MSSQL.Field endtime;
            public MSSQL.Field message;

            public dnt_announcements()
            {
                TableName = "dnt_announcements";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, true);
                poster = new MSSQL.Field(this, "poster", "poster", SqlDbType.VarChar, false);
                posterid = new MSSQL.Field(this, "posterid", "posterid", SqlDbType.Int, false);
                title = new MSSQL.Field(this, "title", "title", SqlDbType.VarChar, false);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                starttime = new MSSQL.Field(this, "starttime", "starttime", SqlDbType.DateTime, false);
                endtime = new MSSQL.Field(this, "endtime", "endtime", SqlDbType.DateTime, false);
                message = new MSSQL.Field(this, "message", "message", SqlDbType.NText, false);
            }
        }

        public class dnt_attachments : MSSQL.TableBase
        {
            public MSSQL.Field aid;
            public MSSQL.Field uid;
            public MSSQL.Field tid;
            public MSSQL.Field pid;
            public MSSQL.Field postdatetime;
            public MSSQL.Field readperm;
            public MSSQL.Field filename;
            public MSSQL.Field description;
            public MSSQL.Field filetype;
            public MSSQL.Field filesize;
            public MSSQL.Field attachment;
            public MSSQL.Field downloads;

            public dnt_attachments()
            {
                TableName = "dnt_attachments";

                aid = new MSSQL.Field(this, "aid", "aid", SqlDbType.Int, true);
                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                readperm = new MSSQL.Field(this, "readperm", "readperm", SqlDbType.Int, false);
                filename = new MSSQL.Field(this, "filename", "filename", SqlDbType.NChar, false);
                description = new MSSQL.Field(this, "description", "description", SqlDbType.NChar, false);
                filetype = new MSSQL.Field(this, "filetype", "filetype", SqlDbType.NChar, false);
                filesize = new MSSQL.Field(this, "filesize", "filesize", SqlDbType.Int, false);
                attachment = new MSSQL.Field(this, "attachment", "attachment", SqlDbType.NChar, false);
                downloads = new MSSQL.Field(this, "downloads", "downloads", SqlDbType.Int, false);
            }
        }

        public class dnt_attachtypes : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field extension;
            public MSSQL.Field maxsize;

            public dnt_attachtypes()
            {
                TableName = "dnt_attachtypes";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.SmallInt, true);
                extension = new MSSQL.Field(this, "extension", "extension", SqlDbType.VarChar, false);
                maxsize = new MSSQL.Field(this, "maxsize", "maxsize", SqlDbType.Int, false);
            }
        }

        public class dnt_bbcodes : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field available;
            public MSSQL.Field tag;
            public MSSQL.Field icon;
            public MSSQL.Field example;
            public MSSQL.Field Params;
            public MSSQL.Field nest;
            public MSSQL.Field explanation;
            public MSSQL.Field replacement;
            public MSSQL.Field paramsdescript;
            public MSSQL.Field paramsdefvalue;

            public dnt_bbcodes()
            {
                TableName = "dnt_bbcodes";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, true);
                available = new MSSQL.Field(this, "available", "available", SqlDbType.Int, false);
                tag = new MSSQL.Field(this, "tag", "tag", SqlDbType.VarChar, false);
                icon = new MSSQL.Field(this, "icon", "icon", SqlDbType.VarChar, false);
                example = new MSSQL.Field(this, "example", "example", SqlDbType.VarChar, false);
                Params = new MSSQL.Field(this, "Params", "Params", SqlDbType.Int, false);
                nest = new MSSQL.Field(this, "nest", "nest", SqlDbType.Int, false);
                explanation = new MSSQL.Field(this, "explanation", "explanation", SqlDbType.NText, false);
                replacement = new MSSQL.Field(this, "replacement", "replacement", SqlDbType.NText, false);
                paramsdescript = new MSSQL.Field(this, "paramsdescript", "paramsdescript", SqlDbType.NText, false);
                paramsdefvalue = new MSSQL.Field(this, "paramsdefvalue", "paramsdefvalue", SqlDbType.NText, false);
            }
        }

        public class dnt_bonuslog : MSSQL.TableBase
        {
            public MSSQL.Field tid;
            public MSSQL.Field authorid;
            public MSSQL.Field answerid;
            public MSSQL.Field answername;
            public MSSQL.Field pid;
            public MSSQL.Field dateline;
            public MSSQL.Field bonus;
            public MSSQL.Field extid;
            public MSSQL.Field isbest;

            public dnt_bonuslog()
            {
                TableName = "dnt_bonuslog";

                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                authorid = new MSSQL.Field(this, "authorid", "authorid", SqlDbType.Int, false);
                answerid = new MSSQL.Field(this, "answerid", "answerid", SqlDbType.Int, false);
                answername = new MSSQL.Field(this, "answername", "answername", SqlDbType.NChar, false);
                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, false);
                dateline = new MSSQL.Field(this, "dateline", "dateline", SqlDbType.DateTime, false);
                bonus = new MSSQL.Field(this, "bonus", "bonus", SqlDbType.Int, false);
                extid = new MSSQL.Field(this, "extid", "extid", SqlDbType.TinyInt, false);
                isbest = new MSSQL.Field(this, "isbest", "isbest", SqlDbType.Int, false);
            }
        }

        public class dnt_creditslog : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field uid;
            public MSSQL.Field fromto;
            public MSSQL.Field sendcredits;
            public MSSQL.Field receivecredits;
            public MSSQL.Field send;
            public MSSQL.Field receive;
            public MSSQL.Field paydate;
            public MSSQL.Field operation;

            public dnt_creditslog()
            {
                TableName = "dnt_creditslog";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, true);
                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                fromto = new MSSQL.Field(this, "fromto", "fromto", SqlDbType.Int, false);
                sendcredits = new MSSQL.Field(this, "sendcredits", "sendcredits", SqlDbType.TinyInt, false);
                receivecredits = new MSSQL.Field(this, "receivecredits", "receivecredits", SqlDbType.TinyInt, false);
                send = new MSSQL.Field(this, "send", "send", SqlDbType.Float, false);
                receive = new MSSQL.Field(this, "receive", "receive", SqlDbType.Float, false);
                paydate = new MSSQL.Field(this, "paydate", "paydate", SqlDbType.DateTime, false);
                operation = new MSSQL.Field(this, "operation", "operation", SqlDbType.TinyInt, false);
            }
        }

        public class dnt_debatediggs : MSSQL.TableBase
        {
            public MSSQL.Field tid;
            public MSSQL.Field pid;
            public MSSQL.Field digger;
            public MSSQL.Field diggerid;
            public MSSQL.Field diggerip;
            public MSSQL.Field diggdatetime;

            public dnt_debatediggs()
            {
                TableName = "dnt_debatediggs";

                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, false);
                digger = new MSSQL.Field(this, "digger", "digger", SqlDbType.NChar, false);
                diggerid = new MSSQL.Field(this, "diggerid", "diggerid", SqlDbType.Int, false);
                diggerip = new MSSQL.Field(this, "diggerip", "diggerip", SqlDbType.NChar, false);
                diggdatetime = new MSSQL.Field(this, "diggdatetime", "diggdatetime", SqlDbType.DateTime, false);
            }
        }

        public class dnt_debates : MSSQL.TableBase
        {
            public MSSQL.Field tid;
            public MSSQL.Field positiveopinion;
            public MSSQL.Field negativeopinion;
            public MSSQL.Field terminaltime;
            public MSSQL.Field positivediggs;
            public MSSQL.Field negativediggs;

            public dnt_debates()
            {
                TableName = "dnt_debates";

                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                positiveopinion = new MSSQL.Field(this, "positiveopinion", "positiveopinion", SqlDbType.VarChar, false);
                negativeopinion = new MSSQL.Field(this, "negativeopinion", "negativeopinion", SqlDbType.VarChar, false);
                terminaltime = new MSSQL.Field(this, "terminaltime", "terminaltime", SqlDbType.DateTime, false);
                positivediggs = new MSSQL.Field(this, "positivediggs", "positivediggs", SqlDbType.Int, false);
                negativediggs = new MSSQL.Field(this, "negativediggs", "negativediggs", SqlDbType.Int, false);
            }
        }

        public class dnt_failedlogins : MSSQL.TableBase
        {
            public MSSQL.Field ip;
            public MSSQL.Field errcount;
            public MSSQL.Field lastupdate;

            public dnt_failedlogins()
            {
                TableName = "dnt_failedlogins";

                ip = new MSSQL.Field(this, "ip", "ip", SqlDbType.Char, false);
                errcount = new MSSQL.Field(this, "errcount", "errcount", SqlDbType.SmallInt, false);
                lastupdate = new MSSQL.Field(this, "lastupdate", "lastupdate", SqlDbType.SmallDateTime, false);
            }
        }

        public class dnt_favorites : MSSQL.TableBase
        {
            public MSSQL.Field uid;
            public MSSQL.Field tid;
            public MSSQL.Field typeid;

            public dnt_favorites()
            {
                TableName = "dnt_favorites";

                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                typeid = new MSSQL.Field(this, "typeid", "typeid", SqlDbType.TinyInt, false);
            }
        }

        public class dnt_forumfields : MSSQL.TableBase
        {
            public MSSQL.Field fid;
            public MSSQL.Field password;
            public MSSQL.Field icon;
            public MSSQL.Field postcredits;
            public MSSQL.Field replycredits;
            public MSSQL.Field redirect;
            public MSSQL.Field attachextensions;
            public MSSQL.Field rules;
            public MSSQL.Field topictypes;
            public MSSQL.Field viewperm;
            public MSSQL.Field postperm;
            public MSSQL.Field replyperm;
            public MSSQL.Field getattachperm;
            public MSSQL.Field postattachperm;
            public MSSQL.Field moderators;
            public MSSQL.Field description;
            public MSSQL.Field applytopictype;
            public MSSQL.Field postbytopictype;
            public MSSQL.Field viewbytopictype;
            public MSSQL.Field topictypeprefix;
            public MSSQL.Field permuserlist;

            public dnt_forumfields()
            {
                TableName = "dnt_forumfields";

                fid = new MSSQL.Field(this, "fid", "fid", SqlDbType.SmallInt, false);
                password = new MSSQL.Field(this, "password", "password", SqlDbType.VarChar, false);
                icon = new MSSQL.Field(this, "icon", "icon", SqlDbType.VarChar, false);
                postcredits = new MSSQL.Field(this, "postcredits", "postcredits", SqlDbType.VarChar, false);
                replycredits = new MSSQL.Field(this, "replycredits", "replycredits", SqlDbType.VarChar, false);
                redirect = new MSSQL.Field(this, "redirect", "redirect", SqlDbType.VarChar, false);
                attachextensions = new MSSQL.Field(this, "attachextensions", "attachextensions", SqlDbType.VarChar, false);
                rules = new MSSQL.Field(this, "rules", "rules", SqlDbType.NText, false);
                topictypes = new MSSQL.Field(this, "topictypes", "topictypes", SqlDbType.Text, false);
                viewperm = new MSSQL.Field(this, "viewperm", "viewperm", SqlDbType.Text, false);
                postperm = new MSSQL.Field(this, "postperm", "postperm", SqlDbType.Text, false);
                replyperm = new MSSQL.Field(this, "replyperm", "replyperm", SqlDbType.Text, false);
                getattachperm = new MSSQL.Field(this, "getattachperm", "getattachperm", SqlDbType.Text, false);
                postattachperm = new MSSQL.Field(this, "postattachperm", "postattachperm", SqlDbType.Text, false);
                moderators = new MSSQL.Field(this, "moderators", "moderators", SqlDbType.NText, false);
                description = new MSSQL.Field(this, "description", "description", SqlDbType.NText, false);
                applytopictype = new MSSQL.Field(this, "applytopictype", "applytopictype", SqlDbType.TinyInt, false);
                postbytopictype = new MSSQL.Field(this, "postbytopictype", "postbytopictype", SqlDbType.TinyInt, false);
                viewbytopictype = new MSSQL.Field(this, "viewbytopictype", "viewbytopictype", SqlDbType.TinyInt, false);
                topictypeprefix = new MSSQL.Field(this, "topictypeprefix", "topictypeprefix", SqlDbType.TinyInt, false);
                permuserlist = new MSSQL.Field(this, "permuserlist", "permuserlist", SqlDbType.NText, false);
            }
        }

        public class dnt_forumlinks : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field displayorder;
            public MSSQL.Field name;
            public MSSQL.Field url;
            public MSSQL.Field note;
            public MSSQL.Field logo;

            public dnt_forumlinks()
            {
                TableName = "dnt_forumlinks";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.SmallInt, true);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                name = new MSSQL.Field(this, "name", "name", SqlDbType.VarChar, false);
                url = new MSSQL.Field(this, "url", "url", SqlDbType.VarChar, false);
                note = new MSSQL.Field(this, "note", "note", SqlDbType.VarChar, false);
                logo = new MSSQL.Field(this, "logo", "logo", SqlDbType.VarChar, false);
            }
        }

        public class dnt_forums : MSSQL.TableBase
        {
            public MSSQL.Field fid;
            public MSSQL.Field parentid;
            public MSSQL.Field layer;
            public MSSQL.Field pathlist;
            public MSSQL.Field parentidlist;
            public MSSQL.Field subforumcount;
            public MSSQL.Field name;
            public MSSQL.Field status;
            public MSSQL.Field colcount;
            public MSSQL.Field displayorder;
            public MSSQL.Field templateid;
            public MSSQL.Field topics;
            public MSSQL.Field curtopics;
            public MSSQL.Field posts;
            public MSSQL.Field todayposts;
            public MSSQL.Field lasttid;
            public MSSQL.Field lasttitle;
            public MSSQL.Field lastpost;
            public MSSQL.Field lastposterid;
            public MSSQL.Field lastposter;
            public MSSQL.Field allowsmilies;
            public MSSQL.Field allowrss;
            public MSSQL.Field allowhtml;
            public MSSQL.Field allowbbcode;
            public MSSQL.Field allowimgcode;
            public MSSQL.Field allowblog;
            public MSSQL.Field istrade;
            public MSSQL.Field allowpostspecial;
            public MSSQL.Field allowspecialonly;
            public MSSQL.Field alloweditrules;
            public MSSQL.Field allowthumbnail;
            public MSSQL.Field allowtag;
            public MSSQL.Field recyclebin;
            public MSSQL.Field modnewposts;
            public MSSQL.Field jammer;
            public MSSQL.Field disablewatermark;
            public MSSQL.Field inheritedmod;
            public MSSQL.Field autoclose;

            public dnt_forums()
            {
                TableName = "dnt_forums";

                fid = new MSSQL.Field(this, "fid", "fid", SqlDbType.Int, true);
                parentid = new MSSQL.Field(this, "parentid", "parentid", SqlDbType.SmallInt, false);
                layer = new MSSQL.Field(this, "layer", "layer", SqlDbType.SmallInt, false);
                pathlist = new MSSQL.Field(this, "pathlist", "pathlist", SqlDbType.NChar, false);
                parentidlist = new MSSQL.Field(this, "parentidlist", "parentidlist", SqlDbType.Char, false);
                subforumcount = new MSSQL.Field(this, "subforumcount", "subforumcount", SqlDbType.Int, false);
                name = new MSSQL.Field(this, "name", "name", SqlDbType.NChar, false);
                status = new MSSQL.Field(this, "status", "status", SqlDbType.Int, false);
                colcount = new MSSQL.Field(this, "colcount", "colcount", SqlDbType.SmallInt, false);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                templateid = new MSSQL.Field(this, "templateid", "templateid", SqlDbType.SmallInt, false);
                topics = new MSSQL.Field(this, "topics", "topics", SqlDbType.Int, false);
                curtopics = new MSSQL.Field(this, "curtopics", "curtopics", SqlDbType.Int, false);
                posts = new MSSQL.Field(this, "posts", "posts", SqlDbType.Int, false);
                todayposts = new MSSQL.Field(this, "todayposts", "todayposts", SqlDbType.Int, false);
                lasttid = new MSSQL.Field(this, "lasttid", "lasttid", SqlDbType.Int, false);
                lasttitle = new MSSQL.Field(this, "lasttitle", "lasttitle", SqlDbType.NChar, false);
                lastpost = new MSSQL.Field(this, "lastpost", "lastpost", SqlDbType.DateTime, false);
                lastposterid = new MSSQL.Field(this, "lastposterid", "lastposterid", SqlDbType.Int, false);
                lastposter = new MSSQL.Field(this, "lastposter", "lastposter", SqlDbType.NChar, false);
                allowsmilies = new MSSQL.Field(this, "allowsmilies", "allowsmilies", SqlDbType.Int, false);
                allowrss = new MSSQL.Field(this, "allowrss", "allowrss", SqlDbType.Int, false);
                allowhtml = new MSSQL.Field(this, "allowhtml", "allowhtml", SqlDbType.Int, false);
                allowbbcode = new MSSQL.Field(this, "allowbbcode", "allowbbcode", SqlDbType.Int, false);
                allowimgcode = new MSSQL.Field(this, "allowimgcode", "allowimgcode", SqlDbType.Int, false);
                allowblog = new MSSQL.Field(this, "allowblog", "allowblog", SqlDbType.Int, false);
                istrade = new MSSQL.Field(this, "istrade", "istrade", SqlDbType.Int, false);
                allowpostspecial = new MSSQL.Field(this, "allowpostspecial", "allowpostspecial", SqlDbType.Int, false);
                allowspecialonly = new MSSQL.Field(this, "allowspecialonly", "allowspecialonly", SqlDbType.Int, false);
                alloweditrules = new MSSQL.Field(this, "alloweditrules", "alloweditrules", SqlDbType.Int, false);
                allowthumbnail = new MSSQL.Field(this, "allowthumbnail", "allowthumbnail", SqlDbType.Int, false);
                allowtag = new MSSQL.Field(this, "allowtag", "allowtag", SqlDbType.Int, false);
                recyclebin = new MSSQL.Field(this, "recyclebin", "recyclebin", SqlDbType.Int, false);
                modnewposts = new MSSQL.Field(this, "modnewposts", "modnewposts", SqlDbType.Int, false);
                jammer = new MSSQL.Field(this, "jammer", "jammer", SqlDbType.Int, false);
                disablewatermark = new MSSQL.Field(this, "disablewatermark", "disablewatermark", SqlDbType.Int, false);
                inheritedmod = new MSSQL.Field(this, "inheritedmod", "inheritedmod", SqlDbType.Int, false);
                autoclose = new MSSQL.Field(this, "autoclose", "autoclose", SqlDbType.SmallInt, false);
            }
        }

        public class dnt_help : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field title;
            public MSSQL.Field message;
            public MSSQL.Field pid;
            public MSSQL.Field orderby;

            public dnt_help()
            {
                TableName = "dnt_help";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, true);
                title = new MSSQL.Field(this, "title", "title", SqlDbType.VarChar, false);
                message = new MSSQL.Field(this, "message", "message", SqlDbType.NText, false);
                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, false);
                orderby = new MSSQL.Field(this, "orderby", "orderby", SqlDbType.Int, false);
            }
        }

        public class dnt_locations : MSSQL.TableBase
        {
            public MSSQL.Field lid;
            public MSSQL.Field city;
            public MSSQL.Field state;
            public MSSQL.Field country;
            public MSSQL.Field zipcode;

            public dnt_locations()
            {
                TableName = "dnt_locations";

                lid = new MSSQL.Field(this, "lid", "lid", SqlDbType.Int, true);
                city = new MSSQL.Field(this, "city", "city", SqlDbType.VarChar, false);
                state = new MSSQL.Field(this, "state", "state", SqlDbType.VarChar, false);
                country = new MSSQL.Field(this, "country", "country", SqlDbType.VarChar, false);
                zipcode = new MSSQL.Field(this, "zipcode", "zipcode", SqlDbType.VarChar, false);
            }
        }

        public class dnt_medals : MSSQL.TableBase
        {
            public MSSQL.Field medalid;
            public MSSQL.Field name;
            public MSSQL.Field available;
            public MSSQL.Field image;

            public dnt_medals()
            {
                TableName = "dnt_medals";

                medalid = new MSSQL.Field(this, "medalid", "medalid", SqlDbType.SmallInt, false);
                name = new MSSQL.Field(this, "name", "name", SqlDbType.VarChar, false);
                available = new MSSQL.Field(this, "available", "available", SqlDbType.Int, false);
                image = new MSSQL.Field(this, "image", "image", SqlDbType.VarChar, false);
            }
        }

        public class dnt_medalslog : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field adminname;
            public MSSQL.Field adminid;
            public MSSQL.Field ip;
            public MSSQL.Field postdatetime;
            public MSSQL.Field username;
            public MSSQL.Field uid;
            public MSSQL.Field actions;
            public MSSQL.Field medals;
            public MSSQL.Field reason;

            public dnt_medalslog()
            {
                TableName = "dnt_medalslog";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, true);
                adminname = new MSSQL.Field(this, "adminname", "adminname", SqlDbType.VarChar, false);
                adminid = new MSSQL.Field(this, "adminid", "adminid", SqlDbType.Int, false);
                ip = new MSSQL.Field(this, "ip", "ip", SqlDbType.VarChar, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                username = new MSSQL.Field(this, "username", "username", SqlDbType.VarChar, false);
                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                actions = new MSSQL.Field(this, "actions", "actions", SqlDbType.VarChar, false);
                medals = new MSSQL.Field(this, "medals", "medals", SqlDbType.Int, false);
                reason = new MSSQL.Field(this, "reason", "reason", SqlDbType.VarChar, false);
            }
        }

        public class dnt_moderatormanagelog : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field moderatoruid;
            public MSSQL.Field moderatorname;
            public MSSQL.Field groupid;
            public MSSQL.Field grouptitle;
            public MSSQL.Field ip;
            public MSSQL.Field postdatetime;
            public MSSQL.Field fid;
            public MSSQL.Field fname;
            public MSSQL.Field tid;
            public MSSQL.Field title;
            public MSSQL.Field actions;
            public MSSQL.Field reason;

            public dnt_moderatormanagelog()
            {
                TableName = "dnt_moderatormanagelog";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, true);
                moderatoruid = new MSSQL.Field(this, "moderatoruid", "moderatoruid", SqlDbType.Int, false);
                moderatorname = new MSSQL.Field(this, "moderatorname", "moderatorname", SqlDbType.NVarChar, false);
                groupid = new MSSQL.Field(this, "groupid", "groupid", SqlDbType.Int, false);
                grouptitle = new MSSQL.Field(this, "grouptitle", "grouptitle", SqlDbType.NVarChar, false);
                ip = new MSSQL.Field(this, "ip", "ip", SqlDbType.VarChar, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                fid = new MSSQL.Field(this, "fid", "fid", SqlDbType.Int, false);
                fname = new MSSQL.Field(this, "fname", "fname", SqlDbType.NVarChar, false);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                title = new MSSQL.Field(this, "title", "title", SqlDbType.VarChar, false);
                actions = new MSSQL.Field(this, "actions", "actions", SqlDbType.VarChar, false);
                reason = new MSSQL.Field(this, "reason", "reason", SqlDbType.NVarChar, false);
            }
        }

        public class dnt_moderators : MSSQL.TableBase
        {
            public MSSQL.Field uid;
            public MSSQL.Field fid;
            public MSSQL.Field displayorder;
            public MSSQL.Field inherited;

            public dnt_moderators()
            {
                TableName = "dnt_moderators";

                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                fid = new MSSQL.Field(this, "fid", "fid", SqlDbType.SmallInt, false);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.SmallInt, false);
                inherited = new MSSQL.Field(this, "inherited", "inherited", SqlDbType.SmallInt, false);
            }
        }

        public class dnt_myattachments : MSSQL.TableBase
        {
            public MSSQL.Field aid;
            public MSSQL.Field uid;
            public MSSQL.Field attachment;
            public MSSQL.Field description;
            public MSSQL.Field postdatetime;
            public MSSQL.Field downloads;
            public MSSQL.Field filename;
            public MSSQL.Field pid;
            public MSSQL.Field tid;
            public MSSQL.Field extname;

            public dnt_myattachments()
            {
                TableName = "dnt_myattachments";

                aid = new MSSQL.Field(this, "aid", "aid", SqlDbType.Int, false);
                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                attachment = new MSSQL.Field(this, "attachment", "attachment", SqlDbType.NChar, false);
                description = new MSSQL.Field(this, "description", "description", SqlDbType.NChar, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                downloads = new MSSQL.Field(this, "downloads", "downloads", SqlDbType.Int, false);
                filename = new MSSQL.Field(this, "filename", "filename", SqlDbType.NChar, false);
                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, false);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                extname = new MSSQL.Field(this, "extname", "extname", SqlDbType.VarChar, false);
            }
        }

        public class dnt_myposts : MSSQL.TableBase
        {
            public MSSQL.Field uid;
            public MSSQL.Field tid;
            public MSSQL.Field pid;
            public MSSQL.Field dateline;

            public dnt_myposts()
            {
                TableName = "dnt_myposts";

                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, false);
                dateline = new MSSQL.Field(this, "dateline", "dateline", SqlDbType.SmallDateTime, false);
            }
        }

        public class dnt_mytopics : MSSQL.TableBase
        {
            public MSSQL.Field uid;
            public MSSQL.Field tid;
            public MSSQL.Field dateline;

            public dnt_mytopics()
            {
                TableName = "dnt_mytopics";

                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                dateline = new MSSQL.Field(this, "dateline", "dateline", SqlDbType.SmallDateTime, false);
            }
        }

        public class dnt_online : MSSQL.TableBase
        {
            public MSSQL.Field olid;
            public MSSQL.Field userid;
            public MSSQL.Field ip;
            public MSSQL.Field username;
            public MSSQL.Field nickname;
            public MSSQL.Field password;
            public MSSQL.Field groupid;
            public MSSQL.Field olimg;
            public MSSQL.Field adminid;
            public MSSQL.Field invisible;
            public MSSQL.Field action;
            public MSSQL.Field lastactivity;
            public MSSQL.Field lastposttime;
            public MSSQL.Field lastpostpmtime;
            public MSSQL.Field lastsearchtime;
            public MSSQL.Field lastupdatetime;
            public MSSQL.Field forumid;
            public MSSQL.Field forumname;
            public MSSQL.Field titleid;
            public MSSQL.Field title;
            public MSSQL.Field verifycode;

            public dnt_online()
            {
                TableName = "dnt_online";

                olid = new MSSQL.Field(this, "olid", "olid", SqlDbType.Int, true);
                userid = new MSSQL.Field(this, "userid", "userid", SqlDbType.Int, false);
                ip = new MSSQL.Field(this, "ip", "ip", SqlDbType.VarChar, false);
                username = new MSSQL.Field(this, "username", "username", SqlDbType.VarChar, false);
                nickname = new MSSQL.Field(this, "nickname", "nickname", SqlDbType.VarChar, false);
                password = new MSSQL.Field(this, "password", "password", SqlDbType.Char, false);
                groupid = new MSSQL.Field(this, "groupid", "groupid", SqlDbType.SmallInt, false);
                olimg = new MSSQL.Field(this, "olimg", "olimg", SqlDbType.VarChar, false);
                adminid = new MSSQL.Field(this, "adminid", "adminid", SqlDbType.SmallInt, false);
                invisible = new MSSQL.Field(this, "invisible", "invisible", SqlDbType.SmallInt, false);
                action = new MSSQL.Field(this, "action", "action", SqlDbType.SmallInt, false);
                lastactivity = new MSSQL.Field(this, "lastactivity", "lastactivity", SqlDbType.SmallInt, false);
                lastposttime = new MSSQL.Field(this, "lastposttime", "lastposttime", SqlDbType.DateTime, false);
                lastpostpmtime = new MSSQL.Field(this, "lastpostpmtime", "lastpostpmtime", SqlDbType.DateTime, false);
                lastsearchtime = new MSSQL.Field(this, "lastsearchtime", "lastsearchtime", SqlDbType.DateTime, false);
                lastupdatetime = new MSSQL.Field(this, "lastupdatetime", "lastupdatetime", SqlDbType.DateTime, false);
                forumid = new MSSQL.Field(this, "forumid", "forumid", SqlDbType.Int, false);
                forumname = new MSSQL.Field(this, "forumname", "forumname", SqlDbType.VarChar, false);
                titleid = new MSSQL.Field(this, "titleid", "titleid", SqlDbType.Int, false);
                title = new MSSQL.Field(this, "title", "title", SqlDbType.VarChar, false);
                verifycode = new MSSQL.Field(this, "verifycode", "verifycode", SqlDbType.VarChar, false);
            }
        }

        public class dnt_onlinelist : MSSQL.TableBase
        {
            public MSSQL.Field groupid;
            public MSSQL.Field displayorder;
            public MSSQL.Field title;
            public MSSQL.Field img;

            public dnt_onlinelist()
            {
                TableName = "dnt_onlinelist";

                groupid = new MSSQL.Field(this, "groupid", "groupid", SqlDbType.SmallInt, false);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                title = new MSSQL.Field(this, "title", "title", SqlDbType.VarChar, false);
                img = new MSSQL.Field(this, "img", "img", SqlDbType.VarChar, false);
            }
        }

        public class dnt_onlinetime : MSSQL.TableBase
        {
            public MSSQL.Field uid;
            public MSSQL.Field thismonth;
            public MSSQL.Field total;
            public MSSQL.Field lastupdate;

            public dnt_onlinetime()
            {
                TableName = "dnt_onlinetime";

                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                thismonth = new MSSQL.Field(this, "thismonth", "thismonth", SqlDbType.SmallInt, false);
                total = new MSSQL.Field(this, "total", "total", SqlDbType.Int, false);
                lastupdate = new MSSQL.Field(this, "lastupdate", "lastupdate", SqlDbType.DateTime, false);
            }
        }

        public class dnt_paymentlog : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field uid;
            public MSSQL.Field tid;
            public MSSQL.Field authorid;
            public MSSQL.Field buydate;
            public MSSQL.Field amount;
            public MSSQL.Field netamount;

            public dnt_paymentlog()
            {
                TableName = "dnt_paymentlog";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, true);
                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                authorid = new MSSQL.Field(this, "authorid", "authorid", SqlDbType.Int, false);
                buydate = new MSSQL.Field(this, "buydate", "buydate", SqlDbType.DateTime, false);
                amount = new MSSQL.Field(this, "amount", "amount", SqlDbType.SmallInt, false);
                netamount = new MSSQL.Field(this, "netamount", "netamount", SqlDbType.SmallInt, false);
            }
        }

        public class dnt_pms : MSSQL.TableBase
        {
            public MSSQL.Field pmid;
            public MSSQL.Field msgfrom;
            public MSSQL.Field msgfromid;
            public MSSQL.Field msgto;
            public MSSQL.Field msgtoid;
            public MSSQL.Field folder;
            public MSSQL.Field New;
            public MSSQL.Field subject;
            public MSSQL.Field postdatetime;
            public MSSQL.Field message;

            public dnt_pms()
            {
                TableName = "dnt_pms";

                pmid = new MSSQL.Field(this, "pmid", "pmid", SqlDbType.Int, true);
                msgfrom = new MSSQL.Field(this, "msgfrom", "msgfrom", SqlDbType.VarChar, false);
                msgfromid = new MSSQL.Field(this, "msgfromid", "msgfromid", SqlDbType.Int, false);
                msgto = new MSSQL.Field(this, "msgto", "msgto", SqlDbType.VarChar, false);
                msgtoid = new MSSQL.Field(this, "msgtoid", "msgtoid", SqlDbType.Int, false);
                folder = new MSSQL.Field(this, "folder", "folder", SqlDbType.SmallInt, false);
                New = new MSSQL.Field(this, "New", "New", SqlDbType.Int, false);
                subject = new MSSQL.Field(this, "subject", "subject", SqlDbType.VarChar, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                message = new MSSQL.Field(this, "message", "message", SqlDbType.NText, false);
            }
        }

        public class dnt_polloptions : MSSQL.TableBase
        {
            public MSSQL.Field polloptionid;
            public MSSQL.Field tid;
            public MSSQL.Field pollid;
            public MSSQL.Field votes;
            public MSSQL.Field displayorder;
            public MSSQL.Field polloption;
            public MSSQL.Field voternames;

            public dnt_polloptions()
            {
                TableName = "dnt_polloptions";

                polloptionid = new MSSQL.Field(this, "polloptionid", "polloptionid", SqlDbType.Int, true);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                pollid = new MSSQL.Field(this, "pollid", "pollid", SqlDbType.Int, false);
                votes = new MSSQL.Field(this, "votes", "votes", SqlDbType.Int, false);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                polloption = new MSSQL.Field(this, "polloption", "polloption", SqlDbType.NVarChar, false);
                voternames = new MSSQL.Field(this, "voternames", "voternames", SqlDbType.NText, false);
            }
        }

        public class dnt_polls : MSSQL.TableBase
        {
            public MSSQL.Field pollid;
            public MSSQL.Field tid;
            public MSSQL.Field displayorder;
            public MSSQL.Field multiple;
            public MSSQL.Field visible;
            public MSSQL.Field maxchoices;
            public MSSQL.Field expiration;
            public MSSQL.Field uid;
            public MSSQL.Field voternames;

            public dnt_polls()
            {
                TableName = "dnt_polls";

                pollid = new MSSQL.Field(this, "pollid", "pollid", SqlDbType.Int, true);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                multiple = new MSSQL.Field(this, "multiple", "multiple", SqlDbType.TinyInt, false);
                visible = new MSSQL.Field(this, "visible", "visible", SqlDbType.TinyInt, false);
                maxchoices = new MSSQL.Field(this, "maxchoices", "maxchoices", SqlDbType.SmallInt, false);
                expiration = new MSSQL.Field(this, "expiration", "expiration", SqlDbType.DateTime, false);
                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                voternames = new MSSQL.Field(this, "voternames", "voternames", SqlDbType.NText, false);
            }
        }

        public class dnt_postdebatefields : MSSQL.TableBase
        {
            public MSSQL.Field tid;
            public MSSQL.Field pid;
            public MSSQL.Field opinion;
            public MSSQL.Field diggs;

            public dnt_postdebatefields()
            {
                TableName = "dnt_postdebatefields";

                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, false);
                opinion = new MSSQL.Field(this, "opinion", "opinion", SqlDbType.Int, false);
                diggs = new MSSQL.Field(this, "diggs", "diggs", SqlDbType.Int, false);
            }
        }

        public class dnt_postid : MSSQL.TableBase
        {
            public MSSQL.Field pid;
            public MSSQL.Field postdatetime;

            public dnt_postid()
            {
                TableName = "dnt_postid";

                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, true);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
            }
        }

        public class dnt_posts1 : MSSQL.TableBase
        {
            public MSSQL.Field pid;
            public MSSQL.Field fid;
            public MSSQL.Field tid;
            public MSSQL.Field parentid;
            public MSSQL.Field layer;
            public MSSQL.Field poster;
            public MSSQL.Field posterid;
            public MSSQL.Field title;
            public MSSQL.Field postdatetime;
            public MSSQL.Field message;
            public MSSQL.Field ip;
            public MSSQL.Field lastedit;
            public MSSQL.Field invisible;
            public MSSQL.Field usesig;
            public MSSQL.Field htmlon;
            public MSSQL.Field smileyoff;
            public MSSQL.Field parseurloff;
            public MSSQL.Field bbcodeoff;
            public MSSQL.Field attachment;
            public MSSQL.Field rate;
            public MSSQL.Field ratetimes;

            public dnt_posts1()
            {
                TableName = "dnt_posts1";

                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, false);
                fid = new MSSQL.Field(this, "fid", "fid", SqlDbType.Int, false);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                parentid = new MSSQL.Field(this, "parentid", "parentid", SqlDbType.Int, false);
                layer = new MSSQL.Field(this, "layer", "layer", SqlDbType.Int, false);
                poster = new MSSQL.Field(this, "poster", "poster", SqlDbType.VarChar, false);
                posterid = new MSSQL.Field(this, "posterid", "posterid", SqlDbType.Int, false);
                title = new MSSQL.Field(this, "title", "title", SqlDbType.VarChar, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.SmallDateTime, false);
                message = new MSSQL.Field(this, "message", "message", SqlDbType.NText, false);
                ip = new MSSQL.Field(this, "ip", "ip", SqlDbType.VarChar, false);
                lastedit = new MSSQL.Field(this, "lastedit", "lastedit", SqlDbType.VarChar, false);
                invisible = new MSSQL.Field(this, "invisible", "invisible", SqlDbType.Int, false);
                usesig = new MSSQL.Field(this, "usesig", "usesig", SqlDbType.Int, false);
                htmlon = new MSSQL.Field(this, "htmlon", "htmlon", SqlDbType.Int, false);
                smileyoff = new MSSQL.Field(this, "smileyoff", "smileyoff", SqlDbType.Int, false);
                parseurloff = new MSSQL.Field(this, "parseurloff", "parseurloff", SqlDbType.Int, false);
                bbcodeoff = new MSSQL.Field(this, "bbcodeoff", "bbcodeoff", SqlDbType.Int, false);
                attachment = new MSSQL.Field(this, "attachment", "attachment", SqlDbType.Int, false);
                rate = new MSSQL.Field(this, "rate", "rate", SqlDbType.Int, false);
                ratetimes = new MSSQL.Field(this, "ratetimes", "ratetimes", SqlDbType.Int, false);
            }
        }

        public class dnt_ratelog : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field pid;
            public MSSQL.Field uid;
            public MSSQL.Field username;
            public MSSQL.Field extcredits;
            public MSSQL.Field postdatetime;
            public MSSQL.Field score;
            public MSSQL.Field reason;

            public dnt_ratelog()
            {
                TableName = "dnt_ratelog";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, true);
                pid = new MSSQL.Field(this, "pid", "pid", SqlDbType.Int, false);
                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                username = new MSSQL.Field(this, "username", "username", SqlDbType.NVarChar, false);
                extcredits = new MSSQL.Field(this, "extcredits", "extcredits", SqlDbType.TinyInt, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                score = new MSSQL.Field(this, "score", "score", SqlDbType.BigInt, false);
                reason = new MSSQL.Field(this, "reason", "reason", SqlDbType.NVarChar, false);
            }
        }

        public class dnt_scheduledevents : MSSQL.TableBase
        {
            public MSSQL.Field scheduleID;
            public MSSQL.Field key;
            public MSSQL.Field lastexecuted;
            public MSSQL.Field servername;

            public dnt_scheduledevents()
            {
                TableName = "dnt_scheduledevents";

                scheduleID = new MSSQL.Field(this, "scheduleID", "scheduleID", SqlDbType.Int, true);
                key = new MSSQL.Field(this, "key", "key", SqlDbType.VarChar, false);
                lastexecuted = new MSSQL.Field(this, "lastexecuted", "lastexecuted", SqlDbType.DateTime, false);
                servername = new MSSQL.Field(this, "servername", "servername", SqlDbType.VarChar, false);
            }
        }

        public class dnt_searchcaches : MSSQL.TableBase
        {
            public MSSQL.Field searchid;
            public MSSQL.Field keywords;
            public MSSQL.Field searchstring;
            public MSSQL.Field ip;
            public MSSQL.Field uid;
            public MSSQL.Field groupid;
            public MSSQL.Field postdatetime;
            public MSSQL.Field expiration;
            public MSSQL.Field topics;
            public MSSQL.Field tids;

            public dnt_searchcaches()
            {
                TableName = "dnt_searchcaches";

                searchid = new MSSQL.Field(this, "searchid", "searchid", SqlDbType.Int, true);
                keywords = new MSSQL.Field(this, "keywords", "keywords", SqlDbType.VarChar, false);
                searchstring = new MSSQL.Field(this, "searchstring", "searchstring", SqlDbType.VarChar, false);
                ip = new MSSQL.Field(this, "ip", "ip", SqlDbType.VarChar, false);
                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                groupid = new MSSQL.Field(this, "groupid", "groupid", SqlDbType.Int, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                expiration = new MSSQL.Field(this, "expiration", "expiration", SqlDbType.DateTime, false);
                topics = new MSSQL.Field(this, "topics", "topics", SqlDbType.Int, false);
                tids = new MSSQL.Field(this, "tids", "tids", SqlDbType.Text, false);
            }
        }

        public class dnt_smilies : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field displayorder;
            public MSSQL.Field type;
            public MSSQL.Field code;
            public MSSQL.Field url;

            public dnt_smilies()
            {
                TableName = "dnt_smilies";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, false);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                type = new MSSQL.Field(this, "type", "type", SqlDbType.Int, false);
                code = new MSSQL.Field(this, "code", "code", SqlDbType.VarChar, false);
                url = new MSSQL.Field(this, "url", "url", SqlDbType.VarChar, false);
            }
        }

        public class dnt_statistics : MSSQL.TableBase
        {
            public MSSQL.Field totaltopic;
            public MSSQL.Field totalpost;
            public MSSQL.Field totalusers;
            public MSSQL.Field lastusername;
            public MSSQL.Field lastuserid;
            public MSSQL.Field highestonlineusercount;
            public MSSQL.Field highestonlineusertime;
            public MSSQL.Field yesterdayposts;
            public MSSQL.Field highestposts;
            public MSSQL.Field highestpostsdate;

            public dnt_statistics()
            {
                TableName = "dnt_statistics";

                totaltopic = new MSSQL.Field(this, "totaltopic", "totaltopic", SqlDbType.Int, false);
                totalpost = new MSSQL.Field(this, "totalpost", "totalpost", SqlDbType.Int, false);
                totalusers = new MSSQL.Field(this, "totalusers", "totalusers", SqlDbType.Int, false);
                lastusername = new MSSQL.Field(this, "lastusername", "lastusername", SqlDbType.VarChar, false);
                lastuserid = new MSSQL.Field(this, "lastuserid", "lastuserid", SqlDbType.Int, false);
                highestonlineusercount = new MSSQL.Field(this, "highestonlineusercount", "highestonlineusercount", SqlDbType.Int, false);
                highestonlineusertime = new MSSQL.Field(this, "highestonlineusertime", "highestonlineusertime", SqlDbType.DateTime, false);
                yesterdayposts = new MSSQL.Field(this, "yesterdayposts", "yesterdayposts", SqlDbType.Int, false);
                highestposts = new MSSQL.Field(this, "highestposts", "highestposts", SqlDbType.Int, false);
                highestpostsdate = new MSSQL.Field(this, "highestpostsdate", "highestpostsdate", SqlDbType.DateTime, false);
            }
        }

        public class dnt_stats : MSSQL.TableBase
        {
            public MSSQL.Field type;
            public MSSQL.Field variable;
            public MSSQL.Field count;

            public dnt_stats()
            {
                TableName = "dnt_stats";

                type = new MSSQL.Field(this, "type", "type", SqlDbType.Char, false);
                variable = new MSSQL.Field(this, "variable", "variable", SqlDbType.Char, false);
                count = new MSSQL.Field(this, "count", "count", SqlDbType.Int, false);
            }
        }

        public class dnt_statvars : MSSQL.TableBase
        {
            public MSSQL.Field type;
            public MSSQL.Field variable;
            public MSSQL.Field value;

            public dnt_statvars()
            {
                TableName = "dnt_statvars";

                type = new MSSQL.Field(this, "type", "type", SqlDbType.Char, false);
                variable = new MSSQL.Field(this, "variable", "variable", SqlDbType.Char, false);
                value = new MSSQL.Field(this, "value", "value", SqlDbType.Text, false);
            }
        }

        public class dnt_tablelist : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field createdatetime;
            public MSSQL.Field description;
            public MSSQL.Field mintid;
            public MSSQL.Field maxtid;

            public dnt_tablelist()
            {
                TableName = "dnt_tablelist";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.Int, true);
                createdatetime = new MSSQL.Field(this, "createdatetime", "createdatetime", SqlDbType.DateTime, false);
                description = new MSSQL.Field(this, "description", "description", SqlDbType.VarChar, false);
                mintid = new MSSQL.Field(this, "mintid", "mintid", SqlDbType.Int, false);
                maxtid = new MSSQL.Field(this, "maxtid", "maxtid", SqlDbType.Int, false);
            }
        }

        public class dnt_tags : MSSQL.TableBase
        {
            public MSSQL.Field tagid;
            public MSSQL.Field tagname;
            public MSSQL.Field userid;
            public MSSQL.Field postdatetime;
            public MSSQL.Field orderid;
            public MSSQL.Field color;
            public MSSQL.Field count;
            public MSSQL.Field fcount;
            public MSSQL.Field pcount;
            public MSSQL.Field scount;
            public MSSQL.Field vcount;
            public MSSQL.Field gcount;

            public dnt_tags()
            {
                TableName = "dnt_tags";

                tagid = new MSSQL.Field(this, "tagid", "tagid", SqlDbType.Int, true);
                tagname = new MSSQL.Field(this, "tagname", "tagname", SqlDbType.NChar, false);
                userid = new MSSQL.Field(this, "userid", "userid", SqlDbType.Int, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                orderid = new MSSQL.Field(this, "orderid", "orderid", SqlDbType.Int, false);
                color = new MSSQL.Field(this, "color", "color", SqlDbType.Char, false);
                count = new MSSQL.Field(this, "count", "count", SqlDbType.Int, false);
                fcount = new MSSQL.Field(this, "fcount", "fcount", SqlDbType.Int, false);
                pcount = new MSSQL.Field(this, "pcount", "pcount", SqlDbType.Int, false);
                scount = new MSSQL.Field(this, "scount", "scount", SqlDbType.Int, false);
                vcount = new MSSQL.Field(this, "vcount", "vcount", SqlDbType.Int, false);
                gcount = new MSSQL.Field(this, "gcount", "gcount", SqlDbType.Int, false);
            }
        }

        public class dnt_templates : MSSQL.TableBase
        {
            public MSSQL.Field templateid;
            public MSSQL.Field directory;
            public MSSQL.Field name;
            public MSSQL.Field author;
            public MSSQL.Field createdate;
            public MSSQL.Field ver;
            public MSSQL.Field fordntver;
            public MSSQL.Field copyright;

            public dnt_templates()
            {
                TableName = "dnt_templates";

                templateid = new MSSQL.Field(this, "templateid", "templateid", SqlDbType.SmallInt, true);
                directory = new MSSQL.Field(this, "directory", "directory", SqlDbType.VarChar, false);
                name = new MSSQL.Field(this, "name", "name", SqlDbType.VarChar, false);
                author = new MSSQL.Field(this, "author", "author", SqlDbType.VarChar, false);
                createdate = new MSSQL.Field(this, "createdate", "createdate", SqlDbType.VarChar, false);
                ver = new MSSQL.Field(this, "ver", "ver", SqlDbType.VarChar, false);
                fordntver = new MSSQL.Field(this, "fordntver", "fordntver", SqlDbType.VarChar, false);
                copyright = new MSSQL.Field(this, "copyright", "copyright", SqlDbType.VarChar, false);
            }
        }

        public class dnt_topicidentify : MSSQL.TableBase
        {
            public MSSQL.Field identifyid;
            public MSSQL.Field name;
            public MSSQL.Field filename;

            public dnt_topicidentify()
            {
                TableName = "dnt_topicidentify";

                identifyid = new MSSQL.Field(this, "identifyid", "identifyid", SqlDbType.Int, true);
                name = new MSSQL.Field(this, "name", "name", SqlDbType.VarChar, false);
                filename = new MSSQL.Field(this, "filename", "filename", SqlDbType.VarChar, false);
            }
        }

        public class dnt_topics : MSSQL.TableBase
        {
            public MSSQL.Field tid;
            public MSSQL.Field fid;
            public MSSQL.Field iconid;
            public MSSQL.Field typeid;
            public MSSQL.Field readperm;
            public MSSQL.Field price;
            public MSSQL.Field poster;
            public MSSQL.Field posterid;
            public MSSQL.Field title;
            public MSSQL.Field postdatetime;
            public MSSQL.Field lastpost;
            public MSSQL.Field lastpostid;
            public MSSQL.Field lastposter;
            public MSSQL.Field lastposterid;
            public MSSQL.Field views;
            public MSSQL.Field replies;
            public MSSQL.Field displayorder;
            public MSSQL.Field highlight;
            public MSSQL.Field digest;
            public MSSQL.Field rate;
            public MSSQL.Field hide;
            public MSSQL.Field poll;
            public MSSQL.Field attachment;
            public MSSQL.Field moderated;
            public MSSQL.Field closed;
            public MSSQL.Field magic;
            public MSSQL.Field identify;
            public MSSQL.Field special;

            public dnt_topics()
            {
                TableName = "dnt_topics";

                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, true);
                fid = new MSSQL.Field(this, "fid", "fid", SqlDbType.SmallInt, false);
                iconid = new MSSQL.Field(this, "iconid", "iconid", SqlDbType.TinyInt, false);
                typeid = new MSSQL.Field(this, "typeid", "typeid", SqlDbType.Int, false);
                readperm = new MSSQL.Field(this, "readperm", "readperm", SqlDbType.Int, false);
                price = new MSSQL.Field(this, "price", "price", SqlDbType.SmallInt, false);
                poster = new MSSQL.Field(this, "poster", "poster", SqlDbType.VarChar, false);
                posterid = new MSSQL.Field(this, "posterid", "posterid", SqlDbType.Int, false);
                title = new MSSQL.Field(this, "title", "title", SqlDbType.VarChar, false);
                postdatetime = new MSSQL.Field(this, "postdatetime", "postdatetime", SqlDbType.DateTime, false);
                lastpost = new MSSQL.Field(this, "lastpost", "lastpost", SqlDbType.DateTime, false);
                lastpostid = new MSSQL.Field(this, "lastpostid", "lastpostid", SqlDbType.Int, false);
                lastposter = new MSSQL.Field(this, "lastposter", "lastposter", SqlDbType.VarChar, false);
                lastposterid = new MSSQL.Field(this, "lastposterid", "lastposterid", SqlDbType.Int, false);
                views = new MSSQL.Field(this, "views", "views", SqlDbType.Int, false);
                replies = new MSSQL.Field(this, "replies", "replies", SqlDbType.Int, false);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                highlight = new MSSQL.Field(this, "highlight", "highlight", SqlDbType.VarChar, false);
                digest = new MSSQL.Field(this, "digest", "digest", SqlDbType.TinyInt, false);
                rate = new MSSQL.Field(this, "rate", "rate", SqlDbType.Int, false);
                hide = new MSSQL.Field(this, "hide", "hide", SqlDbType.Int, false);
                poll = new MSSQL.Field(this, "poll", "poll", SqlDbType.Int, false);
                attachment = new MSSQL.Field(this, "attachment", "attachment", SqlDbType.Int, false);
                moderated = new MSSQL.Field(this, "moderated", "moderated", SqlDbType.TinyInt, false);
                closed = new MSSQL.Field(this, "closed", "closed", SqlDbType.Int, false);
                magic = new MSSQL.Field(this, "magic", "magic", SqlDbType.Int, false);
                identify = new MSSQL.Field(this, "identify", "identify", SqlDbType.Int, false);
                special = new MSSQL.Field(this, "special", "special", SqlDbType.TinyInt, false);
            }
        }

        public class dnt_topictagcaches : MSSQL.TableBase
        {
            public MSSQL.Field tid;
            public MSSQL.Field linktid;
            public MSSQL.Field linktitle;

            public dnt_topictagcaches()
            {
                TableName = "dnt_topictagcaches";

                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
                linktid = new MSSQL.Field(this, "linktid", "linktid", SqlDbType.Int, false);
                linktitle = new MSSQL.Field(this, "linktitle", "linktitle", SqlDbType.NChar, false);
            }
        }

        public class dnt_topictags : MSSQL.TableBase
        {
            public MSSQL.Field tagid;
            public MSSQL.Field tid;

            public dnt_topictags()
            {
                TableName = "dnt_topictags";

                tagid = new MSSQL.Field(this, "tagid", "tagid", SqlDbType.Int, false);
                tid = new MSSQL.Field(this, "tid", "tid", SqlDbType.Int, false);
            }
        }

        public class dnt_topictypes : MSSQL.TableBase
        {
            public MSSQL.Field typeid;
            public MSSQL.Field displayorder;
            public MSSQL.Field name;
            public MSSQL.Field description;

            public dnt_topictypes()
            {
                TableName = "dnt_topictypes";

                typeid = new MSSQL.Field(this, "typeid", "typeid", SqlDbType.Int, true);
                displayorder = new MSSQL.Field(this, "displayorder", "displayorder", SqlDbType.Int, false);
                name = new MSSQL.Field(this, "name", "name", SqlDbType.VarChar, false);
                description = new MSSQL.Field(this, "description", "description", SqlDbType.VarChar, false);
            }
        }

        public class dnt_userfields : MSSQL.TableBase
        {
            public MSSQL.Field uid;
            public MSSQL.Field website;
            public MSSQL.Field icq;
            public MSSQL.Field qq;
            public MSSQL.Field yahoo;
            public MSSQL.Field msn;
            public MSSQL.Field skype;
            public MSSQL.Field location;
            public MSSQL.Field customstatus;
            public MSSQL.Field avatar;
            public MSSQL.Field avatarwidth;
            public MSSQL.Field avatarheight;
            public MSSQL.Field medals;
            public MSSQL.Field bio;
            public MSSQL.Field signature;
            public MSSQL.Field sightml;
            public MSSQL.Field authstr;
            public MSSQL.Field authtime;
            public MSSQL.Field authflag;
            public MSSQL.Field realname;
            public MSSQL.Field idcard;
            public MSSQL.Field mobile;
            public MSSQL.Field phone;

            public dnt_userfields()
            {
                TableName = "dnt_userfields";

                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.Int, false);
                website = new MSSQL.Field(this, "website", "website", SqlDbType.VarChar, false);
                icq = new MSSQL.Field(this, "icq", "icq", SqlDbType.VarChar, false);
                qq = new MSSQL.Field(this, "qq", "qq", SqlDbType.VarChar, false);
                yahoo = new MSSQL.Field(this, "yahoo", "yahoo", SqlDbType.VarChar, false);
                msn = new MSSQL.Field(this, "msn", "msn", SqlDbType.VarChar, false);
                skype = new MSSQL.Field(this, "skype", "skype", SqlDbType.VarChar, false);
                location = new MSSQL.Field(this, "location", "location", SqlDbType.VarChar, false);
                customstatus = new MSSQL.Field(this, "customstatus", "customstatus", SqlDbType.VarChar, false);
                avatar = new MSSQL.Field(this, "avatar", "avatar", SqlDbType.VarChar, false);
                avatarwidth = new MSSQL.Field(this, "avatarwidth", "avatarwidth", SqlDbType.Int, false);
                avatarheight = new MSSQL.Field(this, "avatarheight", "avatarheight", SqlDbType.Int, false);
                medals = new MSSQL.Field(this, "medals", "medals", SqlDbType.VarChar, false);
                bio = new MSSQL.Field(this, "bio", "bio", SqlDbType.VarChar, false);
                signature = new MSSQL.Field(this, "signature", "signature", SqlDbType.VarChar, false);
                sightml = new MSSQL.Field(this, "sightml", "sightml", SqlDbType.VarChar, false);
                authstr = new MSSQL.Field(this, "authstr", "authstr", SqlDbType.VarChar, false);
                authtime = new MSSQL.Field(this, "authtime", "authtime", SqlDbType.SmallDateTime, false);
                authflag = new MSSQL.Field(this, "authflag", "authflag", SqlDbType.TinyInt, false);
                realname = new MSSQL.Field(this, "realname", "realname", SqlDbType.VarChar, false);
                idcard = new MSSQL.Field(this, "idcard", "idcard", SqlDbType.VarChar, false);
                mobile = new MSSQL.Field(this, "mobile", "mobile", SqlDbType.VarChar, false);
                phone = new MSSQL.Field(this, "phone", "phone", SqlDbType.VarChar, false);
            }
        }

        public class dnt_usergroups : MSSQL.TableBase
        {
            public MSSQL.Field groupid;
            public MSSQL.Field radminid;
            public MSSQL.Field type;
            public MSSQL.Field system;
            public MSSQL.Field grouptitle;
            public MSSQL.Field creditshigher;
            public MSSQL.Field creditslower;
            public MSSQL.Field stars;
            public MSSQL.Field color;
            public MSSQL.Field groupavatar;
            public MSSQL.Field readaccess;
            public MSSQL.Field allowvisit;
            public MSSQL.Field allowpost;
            public MSSQL.Field allowreply;
            public MSSQL.Field allowpostpoll;
            public MSSQL.Field allowdirectpost;
            public MSSQL.Field allowgetattach;
            public MSSQL.Field allowpostattach;
            public MSSQL.Field allowvote;
            public MSSQL.Field allowmultigroups;
            public MSSQL.Field allowsearch;
            public MSSQL.Field allowavatar;
            public MSSQL.Field allowcstatus;
            public MSSQL.Field allowuseblog;
            public MSSQL.Field allowinvisible;
            public MSSQL.Field allowtransfer;
            public MSSQL.Field allowsetreadperm;
            public MSSQL.Field allowsetattachperm;
            public MSSQL.Field allowhidecode;
            public MSSQL.Field allowhtml;
            public MSSQL.Field allowcusbbcode;
            public MSSQL.Field allownickname;
            public MSSQL.Field allowsigbbcode;
            public MSSQL.Field allowsigimgcode;
            public MSSQL.Field allowviewpro;
            public MSSQL.Field allowviewstats;
            public MSSQL.Field disableperiodctrl;
            public MSSQL.Field reasonpm;
            public MSSQL.Field maxprice;
            public MSSQL.Field maxpmnum;
            public MSSQL.Field maxsigsize;
            public MSSQL.Field maxattachsize;
            public MSSQL.Field maxsizeperday;
            public MSSQL.Field attachextensions;
            public MSSQL.Field raterange;
            public MSSQL.Field allowspace;
            public MSSQL.Field maxspaceattachsize;
            public MSSQL.Field maxspacephotosize;
            public MSSQL.Field allowdebate;
            public MSSQL.Field allowbonus;
            public MSSQL.Field minbonusprice;
            public MSSQL.Field maxbonusprice;
            public MSSQL.Field allowtrade;
            public MSSQL.Field allowdiggs;

            public dnt_usergroups()
            {
                TableName = "dnt_usergroups";

                groupid = new MSSQL.Field(this, "groupid", "groupid", SqlDbType.SmallInt, true);
                radminid = new MSSQL.Field(this, "radminid", "radminid", SqlDbType.Int, false);
                type = new MSSQL.Field(this, "type", "type", SqlDbType.SmallInt, false);
                system = new MSSQL.Field(this, "system", "system", SqlDbType.SmallInt, false);
                grouptitle = new MSSQL.Field(this, "grouptitle", "grouptitle", SqlDbType.VarChar, false);
                creditshigher = new MSSQL.Field(this, "creditshigher", "creditshigher", SqlDbType.Int, false);
                creditslower = new MSSQL.Field(this, "creditslower", "creditslower", SqlDbType.Int, false);
                stars = new MSSQL.Field(this, "stars", "stars", SqlDbType.Int, false);
                color = new MSSQL.Field(this, "color", "color", SqlDbType.Char, false);
                groupavatar = new MSSQL.Field(this, "groupavatar", "groupavatar", SqlDbType.VarChar, false);
                readaccess = new MSSQL.Field(this, "readaccess", "readaccess", SqlDbType.Int, false);
                allowvisit = new MSSQL.Field(this, "allowvisit", "allowvisit", SqlDbType.Int, false);
                allowpost = new MSSQL.Field(this, "allowpost", "allowpost", SqlDbType.Int, false);
                allowreply = new MSSQL.Field(this, "allowreply", "allowreply", SqlDbType.Int, false);
                allowpostpoll = new MSSQL.Field(this, "allowpostpoll", "allowpostpoll", SqlDbType.Int, false);
                allowdirectpost = new MSSQL.Field(this, "allowdirectpost", "allowdirectpost", SqlDbType.Int, false);
                allowgetattach = new MSSQL.Field(this, "allowgetattach", "allowgetattach", SqlDbType.Int, false);
                allowpostattach = new MSSQL.Field(this, "allowpostattach", "allowpostattach", SqlDbType.Int, false);
                allowvote = new MSSQL.Field(this, "allowvote", "allowvote", SqlDbType.Int, false);
                allowmultigroups = new MSSQL.Field(this, "allowmultigroups", "allowmultigroups", SqlDbType.Int, false);
                allowsearch = new MSSQL.Field(this, "allowsearch", "allowsearch", SqlDbType.Int, false);
                allowavatar = new MSSQL.Field(this, "allowavatar", "allowavatar", SqlDbType.Int, false);
                allowcstatus = new MSSQL.Field(this, "allowcstatus", "allowcstatus", SqlDbType.Int, false);
                allowuseblog = new MSSQL.Field(this, "allowuseblog", "allowuseblog", SqlDbType.Int, false);
                allowinvisible = new MSSQL.Field(this, "allowinvisible", "allowinvisible", SqlDbType.Int, false);
                allowtransfer = new MSSQL.Field(this, "allowtransfer", "allowtransfer", SqlDbType.Int, false);
                allowsetreadperm = new MSSQL.Field(this, "allowsetreadperm", "allowsetreadperm", SqlDbType.Int, false);
                allowsetattachperm = new MSSQL.Field(this, "allowsetattachperm", "allowsetattachperm", SqlDbType.Int, false);
                allowhidecode = new MSSQL.Field(this, "allowhidecode", "allowhidecode", SqlDbType.Int, false);
                allowhtml = new MSSQL.Field(this, "allowhtml", "allowhtml", SqlDbType.Int, false);
                allowcusbbcode = new MSSQL.Field(this, "allowcusbbcode", "allowcusbbcode", SqlDbType.Int, false);
                allownickname = new MSSQL.Field(this, "allownickname", "allownickname", SqlDbType.Int, false);
                allowsigbbcode = new MSSQL.Field(this, "allowsigbbcode", "allowsigbbcode", SqlDbType.Int, false);
                allowsigimgcode = new MSSQL.Field(this, "allowsigimgcode", "allowsigimgcode", SqlDbType.Int, false);
                allowviewpro = new MSSQL.Field(this, "allowviewpro", "allowviewpro", SqlDbType.Int, false);
                allowviewstats = new MSSQL.Field(this, "allowviewstats", "allowviewstats", SqlDbType.Int, false);
                disableperiodctrl = new MSSQL.Field(this, "disableperiodctrl", "disableperiodctrl", SqlDbType.Int, false);
                reasonpm = new MSSQL.Field(this, "reasonpm", "reasonpm", SqlDbType.Int, false);
                maxprice = new MSSQL.Field(this, "maxprice", "maxprice", SqlDbType.SmallInt, false);
                maxpmnum = new MSSQL.Field(this, "maxpmnum", "maxpmnum", SqlDbType.SmallInt, false);
                maxsigsize = new MSSQL.Field(this, "maxsigsize", "maxsigsize", SqlDbType.SmallInt, false);
                maxattachsize = new MSSQL.Field(this, "maxattachsize", "maxattachsize", SqlDbType.Int, false);
                maxsizeperday = new MSSQL.Field(this, "maxsizeperday", "maxsizeperday", SqlDbType.Int, false);
                attachextensions = new MSSQL.Field(this, "attachextensions", "attachextensions", SqlDbType.Char, false);
                raterange = new MSSQL.Field(this, "raterange", "raterange", SqlDbType.NChar, false);
                allowspace = new MSSQL.Field(this, "allowspace", "allowspace", SqlDbType.SmallInt, false);
                maxspaceattachsize = new MSSQL.Field(this, "maxspaceattachsize", "maxspaceattachsize", SqlDbType.Int, false);
                maxspacephotosize = new MSSQL.Field(this, "maxspacephotosize", "maxspacephotosize", SqlDbType.Int, false);
                allowdebate = new MSSQL.Field(this, "allowdebate", "allowdebate", SqlDbType.Int, false);
                allowbonus = new MSSQL.Field(this, "allowbonus", "allowbonus", SqlDbType.Int, false);
                minbonusprice = new MSSQL.Field(this, "minbonusprice", "minbonusprice", SqlDbType.SmallInt, false);
                maxbonusprice = new MSSQL.Field(this, "maxbonusprice", "maxbonusprice", SqlDbType.SmallInt, false);
                allowtrade = new MSSQL.Field(this, "allowtrade", "allowtrade", SqlDbType.Int, false);
                allowdiggs = new MSSQL.Field(this, "allowdiggs", "allowdiggs", SqlDbType.Int, false);
            }
        }

        public class dnt_users : MSSQL.TableBase
        {
            public MSSQL.Field uid;
            public MSSQL.Field username;
            public MSSQL.Field nickname;
            public MSSQL.Field password;
            public MSSQL.Field secques;
            public MSSQL.Field spaceid;
            public MSSQL.Field gender;
            public MSSQL.Field adminid;
            public MSSQL.Field groupid;
            public MSSQL.Field groupexpiry;
            public MSSQL.Field extgroupids;
            public MSSQL.Field regip;
            public MSSQL.Field joindate;
            public MSSQL.Field lastip;
            public MSSQL.Field lastvisit;
            public MSSQL.Field lastactivity;
            public MSSQL.Field lastpost;
            public MSSQL.Field lastpostid;
            public MSSQL.Field lastposttitle;
            public MSSQL.Field posts;
            public MSSQL.Field digestposts;
            public MSSQL.Field oltime;
            public MSSQL.Field pageviews;
            public MSSQL.Field credits;
            public MSSQL.Field extcredits1;
            public MSSQL.Field extcredits2;
            public MSSQL.Field extcredits3;
            public MSSQL.Field extcredits4;
            public MSSQL.Field extcredits5;
            public MSSQL.Field extcredits6;
            public MSSQL.Field extcredits7;
            public MSSQL.Field extcredits8;
            public MSSQL.Field avatarshowid;
            public MSSQL.Field email;
            public MSSQL.Field bday;
            public MSSQL.Field sigstatus;
            public MSSQL.Field tpp;
            public MSSQL.Field ppp;
            public MSSQL.Field templateid;
            public MSSQL.Field pmsound;
            public MSSQL.Field showemail;
            public MSSQL.Field invisible;
            public MSSQL.Field newpm;
            public MSSQL.Field newpmcount;
            public MSSQL.Field accessmasks;
            public MSSQL.Field onlinestate;
            public MSSQL.Field newsletter;
            public MSSQL.Field CpsId;
            public MSSQL.Field Memo;

            public dnt_users()
            {
                TableName = "dnt_users";

                uid = new MSSQL.Field(this, "uid", "uid", SqlDbType.BigInt, true);
                username = new MSSQL.Field(this, "username", "username", SqlDbType.VarChar, false);
                nickname = new MSSQL.Field(this, "nickname", "nickname", SqlDbType.VarChar, false);
                password = new MSSQL.Field(this, "password", "password", SqlDbType.VarChar, false);
                secques = new MSSQL.Field(this, "secques", "secques", SqlDbType.VarChar, false);
                spaceid = new MSSQL.Field(this, "spaceid", "spaceid", SqlDbType.Int, false);
                gender = new MSSQL.Field(this, "gender", "gender", SqlDbType.Int, false);
                adminid = new MSSQL.Field(this, "adminid", "adminid", SqlDbType.Int, false);
                groupid = new MSSQL.Field(this, "groupid", "groupid", SqlDbType.SmallInt, false);
                groupexpiry = new MSSQL.Field(this, "groupexpiry", "groupexpiry", SqlDbType.Int, false);
                extgroupids = new MSSQL.Field(this, "extgroupids", "extgroupids", SqlDbType.VarChar, false);
                regip = new MSSQL.Field(this, "regip", "regip", SqlDbType.VarChar, false);
                joindate = new MSSQL.Field(this, "joindate", "joindate", SqlDbType.SmallDateTime, false);
                lastip = new MSSQL.Field(this, "lastip", "lastip", SqlDbType.VarChar, false);
                lastvisit = new MSSQL.Field(this, "lastvisit", "lastvisit", SqlDbType.DateTime, false);
                lastactivity = new MSSQL.Field(this, "lastactivity", "lastactivity", SqlDbType.DateTime, false);
                lastpost = new MSSQL.Field(this, "lastpost", "lastpost", SqlDbType.DateTime, false);
                lastpostid = new MSSQL.Field(this, "lastpostid", "lastpostid", SqlDbType.Int, false);
                lastposttitle = new MSSQL.Field(this, "lastposttitle", "lastposttitle", SqlDbType.VarChar, false);
                posts = new MSSQL.Field(this, "posts", "posts", SqlDbType.Int, false);
                digestposts = new MSSQL.Field(this, "digestposts", "digestposts", SqlDbType.SmallInt, false);
                oltime = new MSSQL.Field(this, "oltime", "oltime", SqlDbType.Int, false);
                pageviews = new MSSQL.Field(this, "pageviews", "pageviews", SqlDbType.Int, false);
                credits = new MSSQL.Field(this, "credits", "credits", SqlDbType.Int, false);
                extcredits1 = new MSSQL.Field(this, "extcredits1", "extcredits1", SqlDbType.Decimal, false);
                extcredits2 = new MSSQL.Field(this, "extcredits2", "extcredits2", SqlDbType.Decimal, false);
                extcredits3 = new MSSQL.Field(this, "extcredits3", "extcredits3", SqlDbType.Decimal, false);
                extcredits4 = new MSSQL.Field(this, "extcredits4", "extcredits4", SqlDbType.Decimal, false);
                extcredits5 = new MSSQL.Field(this, "extcredits5", "extcredits5", SqlDbType.Decimal, false);
                extcredits6 = new MSSQL.Field(this, "extcredits6", "extcredits6", SqlDbType.Decimal, false);
                extcredits7 = new MSSQL.Field(this, "extcredits7", "extcredits7", SqlDbType.Decimal, false);
                extcredits8 = new MSSQL.Field(this, "extcredits8", "extcredits8", SqlDbType.Decimal, false);
                avatarshowid = new MSSQL.Field(this, "avatarshowid", "avatarshowid", SqlDbType.Int, false);
                email = new MSSQL.Field(this, "email", "email", SqlDbType.VarChar, false);
                bday = new MSSQL.Field(this, "bday", "bday", SqlDbType.VarChar, false);
                sigstatus = new MSSQL.Field(this, "sigstatus", "sigstatus", SqlDbType.Int, false);
                tpp = new MSSQL.Field(this, "tpp", "tpp", SqlDbType.Int, false);
                ppp = new MSSQL.Field(this, "ppp", "ppp", SqlDbType.Int, false);
                templateid = new MSSQL.Field(this, "templateid", "templateid", SqlDbType.SmallInt, false);
                pmsound = new MSSQL.Field(this, "pmsound", "pmsound", SqlDbType.Int, false);
                showemail = new MSSQL.Field(this, "showemail", "showemail", SqlDbType.Int, false);
                invisible = new MSSQL.Field(this, "invisible", "invisible", SqlDbType.Int, false);
                newpm = new MSSQL.Field(this, "newpm", "newpm", SqlDbType.Int, false);
                newpmcount = new MSSQL.Field(this, "newpmcount", "newpmcount", SqlDbType.Int, false);
                accessmasks = new MSSQL.Field(this, "accessmasks", "accessmasks", SqlDbType.Int, false);
                onlinestate = new MSSQL.Field(this, "onlinestate", "onlinestate", SqlDbType.Int, false);
                newsletter = new MSSQL.Field(this, "newsletter", "newsletter", SqlDbType.Int, false);
                CpsId = new MSSQL.Field(this, "CpsId", "CpsId", SqlDbType.BigInt, false);
                Memo = new MSSQL.Field(this, "Memo", "Memo", SqlDbType.VarChar, false);
            }
        }

        public class dnt_words : MSSQL.TableBase
        {
            public MSSQL.Field id;
            public MSSQL.Field admin;
            public MSSQL.Field find;
            public MSSQL.Field replacement;

            public dnt_words()
            {
                TableName = "dnt_words";

                id = new MSSQL.Field(this, "id", "id", SqlDbType.SmallInt, true);
                admin = new MSSQL.Field(this, "admin", "admin", SqlDbType.VarChar, false);
                find = new MSSQL.Field(this, "find", "find", SqlDbType.VarChar, false);
                replacement = new MSSQL.Field(this, "replacement", "replacement", SqlDbType.VarChar, false);
            }
        }

        public class T_News : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field TypeID;
            public MSSQL.Field DateTime;
            public MSSQL.Field Title;
            public MSSQL.Field ImageUrl;
            public MSSQL.Field isShow;
            public MSSQL.Field isHasImage;
            public MSSQL.Field isCanComments;
            public MSSQL.Field isCommend;
            public MSSQL.Field isHot;
            public MSSQL.Field ReadCount;
            public MSSQL.Field Content;
            public MSSQL.Field IsusesId;

            public T_News()
            {
                TableName = "T_News";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.BigInt, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                TypeID = new MSSQL.Field(this, "TypeID", "TypeID", SqlDbType.Int, false);
                DateTime = new MSSQL.Field(this, "DateTime", "DateTime", SqlDbType.DateTime, false);
                Title = new MSSQL.Field(this, "Title", "Title", SqlDbType.VarChar, false);
                ImageUrl = new MSSQL.Field(this, "ImageUrl", "ImageUrl", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isHasImage = new MSSQL.Field(this, "isHasImage", "isHasImage", SqlDbType.Bit, false);
                isCanComments = new MSSQL.Field(this, "isCanComments", "isCanComments", SqlDbType.Bit, false);
                isCommend = new MSSQL.Field(this, "isCommend", "isCommend", SqlDbType.Bit, false);
                isHot = new MSSQL.Field(this, "isHot", "isHot", SqlDbType.Bit, false);
                ReadCount = new MSSQL.Field(this, "ReadCount", "ReadCount", SqlDbType.BigInt, false);
                Content = new MSSQL.Field(this, "Content", "Content", SqlDbType.VarChar, false);
                IsusesId = new MSSQL.Field(this, "IsusesId", "IsusesId", SqlDbType.Int, false);
            }
        }

        public class T_NewsTypes : MSSQL.TableBase
        {
            public MSSQL.Field ID;
            public MSSQL.Field SiteID;
            public MSSQL.Field ParentID;
            public MSSQL.Field Name;
            public MSSQL.Field isShow;
            public MSSQL.Field isSystem;

            public T_NewsTypes()
            {
                TableName = "T_NewsTypes";

                ID = new MSSQL.Field(this, "ID", "ID", SqlDbType.Int, true);
                SiteID = new MSSQL.Field(this, "SiteID", "SiteID", SqlDbType.BigInt, false);
                ParentID = new MSSQL.Field(this, "ParentID", "ParentID", SqlDbType.Int, false);
                Name = new MSSQL.Field(this, "Name", "Name", SqlDbType.VarChar, false);
                isShow = new MSSQL.Field(this, "isShow", "isShow", SqlDbType.Bit, false);
                isSystem = new MSSQL.Field(this, "isSystem", "isSystem", SqlDbType.Bit, false);
            }
        }
    }

    public class Views
    {
    }

    public class Functions
    {
        public static double F_CpsGetCommenderPromoteCpsBonusScale(long OwnerUserID, long LotteryID)
        {
            object Result = MSSQL.ExecuteFunction("F_CpsGetCommenderPromoteCpsBonusScale",
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, OwnerUserID),
                new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CpsGetCommenderPromoteMemberBonusScale(long OwnerUserID, long LotteryID)
        {
            object Result = MSSQL.ExecuteFunction("F_CpsGetCommenderPromoteMemberBonusScale",
                new MSSQL.Parameter("OwnerUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, OwnerUserID),
                new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_CpsGetCpsBonusScale(long CpsID, long LotteryID)
        {
            object Result = MSSQL.ExecuteFunction("F_CpsGetCpsBonusScale",
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("LotteryID", SqlDbType.BigInt, 0, ParameterDirection.Input, LotteryID)
                );

            return System.Convert.ToDouble(Result);
        }

        public static double F_GetBonusScaleByCommenderID(long CommenderID)
        {
            object Result = MSSQL.ExecuteFunction("F_GetBonusScaleByCommenderID",
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommenderID)
                );

            return System.Convert.ToDouble(Result);
        }
    }

    public class Procedures
    {
        public static int dnt_checkemailandsecques(string username, string email, string secques)
        {
            DataSet ds = null;

            return dnt_checkemailandsecques(ref ds, username, email, secques);
        }

        public static int dnt_checkemailandsecques(ref DataSet ds, string username, string email, string secques)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_checkemailandsecques", ref ds, ref Outputs,
                new MSSQL.Parameter("username", SqlDbType.NChar, 0, ParameterDirection.Input, username),
                new MSSQL.Parameter("email", SqlDbType.Char, 0, ParameterDirection.Input, email),
                new MSSQL.Parameter("secques", SqlDbType.Char, 0, ParameterDirection.Input, secques)
                );

            return CallResult;
        }

        public static int dnt_checkpasswordandsecques(string username, string password, string secques)
        {
            DataSet ds = null;

            return dnt_checkpasswordandsecques(ref ds, username, password, secques);
        }

        public static int dnt_checkpasswordandsecques(ref DataSet ds, string username, string password, string secques)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_checkpasswordandsecques", ref ds, ref Outputs,
                new MSSQL.Parameter("username", SqlDbType.NChar, 0, ParameterDirection.Input, username),
                new MSSQL.Parameter("password", SqlDbType.Char, 0, ParameterDirection.Input, password),
                new MSSQL.Parameter("secques", SqlDbType.Char, 0, ParameterDirection.Input, secques)
                );

            return CallResult;
        }

        public static int dnt_checkpasswordbyuid(int uid, string password)
        {
            DataSet ds = null;

            return dnt_checkpasswordbyuid(ref ds, uid, password);
        }

        public static int dnt_checkpasswordbyuid(ref DataSet ds, int uid, string password)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_checkpasswordbyuid", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("password", SqlDbType.Char, 0, ParameterDirection.Input, password)
                );

            return CallResult;
        }

        public static int dnt_checkpasswordbyusername(string username, string password)
        {
            DataSet ds = null;

            return dnt_checkpasswordbyusername(ref ds, username, password);
        }

        public static int dnt_checkpasswordbyusername(ref DataSet ds, string username, string password)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_checkpasswordbyusername", ref ds, ref Outputs,
                new MSSQL.Parameter("username", SqlDbType.NChar, 0, ParameterDirection.Input, username),
                new MSSQL.Parameter("password", SqlDbType.Char, 0, ParameterDirection.Input, password)
                );

            return CallResult;
        }

        public static int dnt_createadmingroup(short admingid, short alloweditpost, short alloweditpoll, short allowstickthread, short allowmodpost, short allowdelpost, short allowmassprune, short allowrefund, short allowcensorword, short allowviewip, short allowbanip, short allowedituser, short allowmoduser, short allowbanuser, short allowpostannounce, short allowviewlog, short disablepostctrl, short allowviewrealname)
        {
            DataSet ds = null;

            return dnt_createadmingroup(ref ds, admingid, alloweditpost, alloweditpoll, allowstickthread, allowmodpost, allowdelpost, allowmassprune, allowrefund, allowcensorword, allowviewip, allowbanip, allowedituser, allowmoduser, allowbanuser, allowpostannounce, allowviewlog, disablepostctrl, allowviewrealname);
        }

        public static int dnt_createadmingroup(ref DataSet ds, short admingid, short alloweditpost, short alloweditpoll, short allowstickthread, short allowmodpost, short allowdelpost, short allowmassprune, short allowrefund, short allowcensorword, short allowviewip, short allowbanip, short allowedituser, short allowmoduser, short allowbanuser, short allowpostannounce, short allowviewlog, short disablepostctrl, short allowviewrealname)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createadmingroup", ref ds, ref Outputs,
                new MSSQL.Parameter("admingid", SqlDbType.SmallInt, 0, ParameterDirection.Input, admingid),
                new MSSQL.Parameter("alloweditpost", SqlDbType.TinyInt, 0, ParameterDirection.Input, alloweditpost),
                new MSSQL.Parameter("alloweditpoll", SqlDbType.TinyInt, 0, ParameterDirection.Input, alloweditpoll),
                new MSSQL.Parameter("allowstickthread", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowstickthread),
                new MSSQL.Parameter("allowmodpost", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowmodpost),
                new MSSQL.Parameter("allowdelpost", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowdelpost),
                new MSSQL.Parameter("allowmassprune", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowmassprune),
                new MSSQL.Parameter("allowrefund", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowrefund),
                new MSSQL.Parameter("allowcensorword", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowcensorword),
                new MSSQL.Parameter("allowviewip", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowviewip),
                new MSSQL.Parameter("allowbanip", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowbanip),
                new MSSQL.Parameter("allowedituser", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowedituser),
                new MSSQL.Parameter("allowmoduser", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowmoduser),
                new MSSQL.Parameter("allowbanuser", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowbanuser),
                new MSSQL.Parameter("allowpostannounce", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowpostannounce),
                new MSSQL.Parameter("allowviewlog", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowviewlog),
                new MSSQL.Parameter("disablepostctrl", SqlDbType.TinyInt, 0, ParameterDirection.Input, disablepostctrl),
                new MSSQL.Parameter("allowviewrealname", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowviewrealname)
                );

            return CallResult;
        }

        public static int dnt_createattachment(int uid, int tid, int pid, DateTime postdatetime, int readperm, string filename, string description, string filetype, int filesize, string attachment, int downloads, string extname)
        {
            DataSet ds = null;

            return dnt_createattachment(ref ds, uid, tid, pid, postdatetime, readperm, filename, description, filetype, filesize, attachment, downloads, extname);
        }

        public static int dnt_createattachment(ref DataSet ds, int uid, int tid, int pid, DateTime postdatetime, int readperm, string filename, string description, string filetype, int filesize, string attachment, int downloads, string extname)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createattachment", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("pid", SqlDbType.Int, 0, ParameterDirection.Input, pid),
                new MSSQL.Parameter("postdatetime", SqlDbType.DateTime, 0, ParameterDirection.Input, postdatetime),
                new MSSQL.Parameter("readperm", SqlDbType.Int, 0, ParameterDirection.Input, readperm),
                new MSSQL.Parameter("filename", SqlDbType.NChar, 0, ParameterDirection.Input, filename),
                new MSSQL.Parameter("description", SqlDbType.NChar, 0, ParameterDirection.Input, description),
                new MSSQL.Parameter("filetype", SqlDbType.NChar, 0, ParameterDirection.Input, filetype),
                new MSSQL.Parameter("filesize", SqlDbType.Int, 0, ParameterDirection.Input, filesize),
                new MSSQL.Parameter("attachment", SqlDbType.NChar, 0, ParameterDirection.Input, attachment),
                new MSSQL.Parameter("downloads", SqlDbType.Int, 0, ParameterDirection.Input, downloads),
                new MSSQL.Parameter("extname", SqlDbType.NVarChar, 0, ParameterDirection.Input, extname)
                );

            return CallResult;
        }

        public static int dnt_createdebatepostexpand(int tid, int pid, int opinion, int diggs)
        {
            DataSet ds = null;

            return dnt_createdebatepostexpand(ref ds, tid, pid, opinion, diggs);
        }

        public static int dnt_createdebatepostexpand(ref DataSet ds, int tid, int pid, int opinion, int diggs)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createdebatepostexpand", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("pid", SqlDbType.Int, 0, ParameterDirection.Input, pid),
                new MSSQL.Parameter("opinion", SqlDbType.Int, 0, ParameterDirection.Input, opinion),
                new MSSQL.Parameter("diggs", SqlDbType.Int, 0, ParameterDirection.Input, diggs)
                );

            return CallResult;
        }

        public static int dnt_createfavorite(int uid, int tid, short type)
        {
            DataSet ds = null;

            return dnt_createfavorite(ref ds, uid, tid, type);
        }

        public static int dnt_createfavorite(ref DataSet ds, int uid, int tid, short type)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createfavorite", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("type", SqlDbType.TinyInt, 0, ParameterDirection.Input, type)
                );

            return CallResult;
        }

        public static int dnt_createpm(int pmid, string msgfrom, string msgto, int msgfromid, int msgtoid, short folder, int New, string subject, DateTime postdatetime, string message, short savetosentbox)
        {
            DataSet ds = null;

            return dnt_createpm(ref ds, pmid, msgfrom, msgto, msgfromid, msgtoid, folder, New, subject, postdatetime, message, savetosentbox);
        }

        public static int dnt_createpm(ref DataSet ds, int pmid, string msgfrom, string msgto, int msgfromid, int msgtoid, short folder, int New, string subject, DateTime postdatetime, string message, short savetosentbox)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createpm", ref ds, ref Outputs,
                new MSSQL.Parameter("pmid", SqlDbType.Int, 0, ParameterDirection.Input, pmid),
                new MSSQL.Parameter("msgfrom", SqlDbType.NVarChar, 0, ParameterDirection.Input, msgfrom),
                new MSSQL.Parameter("msgto", SqlDbType.NVarChar, 0, ParameterDirection.Input, msgto),
                new MSSQL.Parameter("msgfromid", SqlDbType.Int, 0, ParameterDirection.Input, msgfromid),
                new MSSQL.Parameter("msgtoid", SqlDbType.Int, 0, ParameterDirection.Input, msgtoid),
                new MSSQL.Parameter("folder", SqlDbType.SmallInt, 0, ParameterDirection.Input, folder),
                new MSSQL.Parameter("New", SqlDbType.Int, 0, ParameterDirection.Input, New),
                new MSSQL.Parameter("subject", SqlDbType.NVarChar, 0, ParameterDirection.Input, subject),
                new MSSQL.Parameter("postdatetime", SqlDbType.DateTime, 0, ParameterDirection.Input, postdatetime),
                new MSSQL.Parameter("message", SqlDbType.NText, 0, ParameterDirection.Input, message),
                new MSSQL.Parameter("savetosentbox", SqlDbType.SmallInt, 0, ParameterDirection.Input, savetosentbox)
                );

            return CallResult;
        }

        public static int dnt_createpost1(int fid, int tid, int parentid, int layer, string poster, int posterid, string title, string topictitle, string postdatetime, string message, string ip, string lastedit, int invisible, int usesig, int htmlon, int smileyoff, int bbcodeoff, int parseurloff, int attachment, int rate, int ratetimes)
        {
            DataSet ds = null;

            return dnt_createpost1(ref ds, fid, tid, parentid, layer, poster, posterid, title, topictitle, postdatetime, message, ip, lastedit, invisible, usesig, htmlon, smileyoff, bbcodeoff, parseurloff, attachment, rate, ratetimes);
        }

        public static int dnt_createpost1(ref DataSet ds, int fid, int tid, int parentid, int layer, string poster, int posterid, string title, string topictitle, string postdatetime, string message, string ip, string lastedit, int invisible, int usesig, int htmlon, int smileyoff, int bbcodeoff, int parseurloff, int attachment, int rate, int ratetimes)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createpost1", ref ds, ref Outputs,
                new MSSQL.Parameter("fid", SqlDbType.Int, 0, ParameterDirection.Input, fid),
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("parentid", SqlDbType.Int, 0, ParameterDirection.Input, parentid),
                new MSSQL.Parameter("layer", SqlDbType.Int, 0, ParameterDirection.Input, layer),
                new MSSQL.Parameter("poster", SqlDbType.VarChar, 0, ParameterDirection.Input, poster),
                new MSSQL.Parameter("posterid", SqlDbType.Int, 0, ParameterDirection.Input, posterid),
                new MSSQL.Parameter("title", SqlDbType.NVarChar, 0, ParameterDirection.Input, title),
                new MSSQL.Parameter("topictitle", SqlDbType.NVarChar, 0, ParameterDirection.Input, topictitle),
                new MSSQL.Parameter("postdatetime", SqlDbType.Char, 0, ParameterDirection.Input, postdatetime),
                new MSSQL.Parameter("message", SqlDbType.NText, 0, ParameterDirection.Input, message),
                new MSSQL.Parameter("ip", SqlDbType.VarChar, 0, ParameterDirection.Input, ip),
                new MSSQL.Parameter("lastedit", SqlDbType.VarChar, 0, ParameterDirection.Input, lastedit),
                new MSSQL.Parameter("invisible", SqlDbType.Int, 0, ParameterDirection.Input, invisible),
                new MSSQL.Parameter("usesig", SqlDbType.Int, 0, ParameterDirection.Input, usesig),
                new MSSQL.Parameter("htmlon", SqlDbType.Int, 0, ParameterDirection.Input, htmlon),
                new MSSQL.Parameter("smileyoff", SqlDbType.Int, 0, ParameterDirection.Input, smileyoff),
                new MSSQL.Parameter("bbcodeoff", SqlDbType.Int, 0, ParameterDirection.Input, bbcodeoff),
                new MSSQL.Parameter("parseurloff", SqlDbType.Int, 0, ParameterDirection.Input, parseurloff),
                new MSSQL.Parameter("attachment", SqlDbType.Int, 0, ParameterDirection.Input, attachment),
                new MSSQL.Parameter("rate", SqlDbType.Int, 0, ParameterDirection.Input, rate),
                new MSSQL.Parameter("ratetimes", SqlDbType.Int, 0, ParameterDirection.Input, ratetimes)
                );

            return CallResult;
        }

        public static int dnt_createsearchcache(string keywords, string searchstring, string ip, int uid, int groupid, string postdatetime, string expiration, int topics, string tids)
        {
            DataSet ds = null;

            return dnt_createsearchcache(ref ds, keywords, searchstring, ip, uid, groupid, postdatetime, expiration, topics, tids);
        }

        public static int dnt_createsearchcache(ref DataSet ds, string keywords, string searchstring, string ip, int uid, int groupid, string postdatetime, string expiration, int topics, string tids)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createsearchcache", ref ds, ref Outputs,
                new MSSQL.Parameter("keywords", SqlDbType.VarChar, 0, ParameterDirection.Input, keywords),
                new MSSQL.Parameter("searchstring", SqlDbType.VarChar, 0, ParameterDirection.Input, searchstring),
                new MSSQL.Parameter("ip", SqlDbType.VarChar, 0, ParameterDirection.Input, ip),
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("groupid", SqlDbType.Int, 0, ParameterDirection.Input, groupid),
                new MSSQL.Parameter("postdatetime", SqlDbType.VarChar, 0, ParameterDirection.Input, postdatetime),
                new MSSQL.Parameter("expiration", SqlDbType.VarChar, 0, ParameterDirection.Input, expiration),
                new MSSQL.Parameter("topics", SqlDbType.Int, 0, ParameterDirection.Input, topics),
                new MSSQL.Parameter("tids", SqlDbType.Text, 0, ParameterDirection.Input, tids)
                );

            return CallResult;
        }

        public static int dnt_createtags(string tags, int userid, DateTime postdatetime)
        {
            DataSet ds = null;

            return dnt_createtags(ref ds, tags, userid, postdatetime);
        }

        public static int dnt_createtags(ref DataSet ds, string tags, int userid, DateTime postdatetime)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createtags", ref ds, ref Outputs,
                new MSSQL.Parameter("tags", SqlDbType.NVarChar, 0, ParameterDirection.Input, tags),
                new MSSQL.Parameter("userid", SqlDbType.Int, 0, ParameterDirection.Input, userid),
                new MSSQL.Parameter("postdatetime", SqlDbType.DateTime, 0, ParameterDirection.Input, postdatetime)
                );

            return CallResult;
        }

        public static int dnt_createtopic(short fid, short iconid, string title, short typeid, int readperm, short price, string poster, int posterid, DateTime postdatetime, DateTime lastpost, int lastpostid, string lastposter, int views, int replies, int displayorder, string highlight, int digest, int rate, int hide, int attachment, int moderated, int closed, int magic, short special)
        {
            DataSet ds = null;

            return dnt_createtopic(ref ds, fid, iconid, title, typeid, readperm, price, poster, posterid, postdatetime, lastpost, lastpostid, lastposter, views, replies, displayorder, highlight, digest, rate, hide, attachment, moderated, closed, magic, special);
        }

        public static int dnt_createtopic(ref DataSet ds, short fid, short iconid, string title, short typeid, int readperm, short price, string poster, int posterid, DateTime postdatetime, DateTime lastpost, int lastpostid, string lastposter, int views, int replies, int displayorder, string highlight, int digest, int rate, int hide, int attachment, int moderated, int closed, int magic, short special)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createtopic", ref ds, ref Outputs,
                new MSSQL.Parameter("fid", SqlDbType.SmallInt, 0, ParameterDirection.Input, fid),
                new MSSQL.Parameter("iconid", SqlDbType.SmallInt, 0, ParameterDirection.Input, iconid),
                new MSSQL.Parameter("title", SqlDbType.NChar, 0, ParameterDirection.Input, title),
                new MSSQL.Parameter("typeid", SqlDbType.SmallInt, 0, ParameterDirection.Input, typeid),
                new MSSQL.Parameter("readperm", SqlDbType.Int, 0, ParameterDirection.Input, readperm),
                new MSSQL.Parameter("price", SqlDbType.SmallInt, 0, ParameterDirection.Input, price),
                new MSSQL.Parameter("poster", SqlDbType.Char, 0, ParameterDirection.Input, poster),
                new MSSQL.Parameter("posterid", SqlDbType.Int, 0, ParameterDirection.Input, posterid),
                new MSSQL.Parameter("postdatetime", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, postdatetime),
                new MSSQL.Parameter("lastpost", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, lastpost),
                new MSSQL.Parameter("lastpostid", SqlDbType.Int, 0, ParameterDirection.Input, lastpostid),
                new MSSQL.Parameter("lastposter", SqlDbType.Char, 0, ParameterDirection.Input, lastposter),
                new MSSQL.Parameter("views", SqlDbType.Int, 0, ParameterDirection.Input, views),
                new MSSQL.Parameter("replies", SqlDbType.Int, 0, ParameterDirection.Input, replies),
                new MSSQL.Parameter("displayorder", SqlDbType.Int, 0, ParameterDirection.Input, displayorder),
                new MSSQL.Parameter("highlight", SqlDbType.VarChar, 0, ParameterDirection.Input, highlight),
                new MSSQL.Parameter("digest", SqlDbType.Int, 0, ParameterDirection.Input, digest),
                new MSSQL.Parameter("rate", SqlDbType.Int, 0, ParameterDirection.Input, rate),
                new MSSQL.Parameter("hide", SqlDbType.Int, 0, ParameterDirection.Input, hide),
                new MSSQL.Parameter("attachment", SqlDbType.Int, 0, ParameterDirection.Input, attachment),
                new MSSQL.Parameter("moderated", SqlDbType.Int, 0, ParameterDirection.Input, moderated),
                new MSSQL.Parameter("closed", SqlDbType.Int, 0, ParameterDirection.Input, closed),
                new MSSQL.Parameter("magic", SqlDbType.Int, 0, ParameterDirection.Input, magic),
                new MSSQL.Parameter("special", SqlDbType.TinyInt, 0, ParameterDirection.Input, special)
                );

            return CallResult;
        }

        public static int dnt_createtopictags(string tags, int tid, int userid, DateTime postdatetime)
        {
            DataSet ds = null;

            return dnt_createtopictags(ref ds, tags, tid, userid, postdatetime);
        }

        public static int dnt_createtopictags(ref DataSet ds, string tags, int tid, int userid, DateTime postdatetime)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createtopictags", ref ds, ref Outputs,
                new MSSQL.Parameter("tags", SqlDbType.NVarChar, 0, ParameterDirection.Input, tags),
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("userid", SqlDbType.Int, 0, ParameterDirection.Input, userid),
                new MSSQL.Parameter("postdatetime", SqlDbType.DateTime, 0, ParameterDirection.Input, postdatetime)
                );

            return CallResult;
        }

        public static int dnt_createuser(string username, string nickname, string password, string secques, int gender, int adminid, short groupid, int groupexpiry, string extgroupids, string regip, string joindate, string lastip, string lastvisit, string lastactivity, string lastpost, int lastpostid, string lastposttitle, int posts, short digestposts, int oltime, int pageviews, int credits, double extcredits1, double extcredits2, double extcredits3, double extcredits4, double extcredits5, double extcredits6, double extcredits7, double extcredits8, int avatarshowid, string email, string bday, int sigstatus, int tpp, int ppp, short templateid, int pmsound, int showemail, int newsletter, int invisible, int newpm, int accessmasks, string website, string icq, string qq, string yahoo, string msn, string skype, string location, string customstatus, string avatar, int avatarwidth, int avatarheight, string medals, string bio, string signature, string sightml, string authstr, string realname, string idcard, string mobile, string phone, long CpsID, string IMemo)
        {
            DataSet ds = null;

            return dnt_createuser(ref ds, username, nickname, password, secques, gender, adminid, groupid, groupexpiry, extgroupids, regip, joindate, lastip, lastvisit, lastactivity, lastpost, lastpostid, lastposttitle, posts, digestposts, oltime, pageviews, credits, extcredits1, extcredits2, extcredits3, extcredits4, extcredits5, extcredits6, extcredits7, extcredits8, avatarshowid, email, bday, sigstatus, tpp, ppp, templateid, pmsound, showemail, newsletter, invisible, newpm, accessmasks, website, icq, qq, yahoo, msn, skype, location, customstatus, avatar, avatarwidth, avatarheight, medals, bio, signature, sightml, authstr, realname, idcard, mobile, phone, CpsID, IMemo);
        }

        public static int dnt_createuser(ref DataSet ds, string username, string nickname, string password, string secques, int gender, int adminid, short groupid, int groupexpiry, string extgroupids, string regip, string joindate, string lastip, string lastvisit, string lastactivity, string lastpost, int lastpostid, string lastposttitle, int posts, short digestposts, int oltime, int pageviews, int credits, double extcredits1, double extcredits2, double extcredits3, double extcredits4, double extcredits5, double extcredits6, double extcredits7, double extcredits8, int avatarshowid, string email, string bday, int sigstatus, int tpp, int ppp, short templateid, int pmsound, int showemail, int newsletter, int invisible, int newpm, int accessmasks, string website, string icq, string qq, string yahoo, string msn, string skype, string location, string customstatus, string avatar, int avatarwidth, int avatarheight, string medals, string bio, string signature, string sightml, string authstr, string realname, string idcard, string mobile, string phone, long CpsID, string IMemo)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_createuser", ref ds, ref Outputs,
                new MSSQL.Parameter("username", SqlDbType.NChar, 0, ParameterDirection.Input, username),
                new MSSQL.Parameter("nickname", SqlDbType.NChar, 0, ParameterDirection.Input, nickname),
                new MSSQL.Parameter("password", SqlDbType.Char, 0, ParameterDirection.Input, password),
                new MSSQL.Parameter("secques", SqlDbType.Char, 0, ParameterDirection.Input, secques),
                new MSSQL.Parameter("gender", SqlDbType.Int, 0, ParameterDirection.Input, gender),
                new MSSQL.Parameter("adminid", SqlDbType.Int, 0, ParameterDirection.Input, adminid),
                new MSSQL.Parameter("groupid", SqlDbType.SmallInt, 0, ParameterDirection.Input, groupid),
                new MSSQL.Parameter("groupexpiry", SqlDbType.Int, 0, ParameterDirection.Input, groupexpiry),
                new MSSQL.Parameter("extgroupids", SqlDbType.Char, 0, ParameterDirection.Input, extgroupids),
                new MSSQL.Parameter("regip", SqlDbType.Char, 0, ParameterDirection.Input, regip),
                new MSSQL.Parameter("joindate", SqlDbType.Char, 0, ParameterDirection.Input, joindate),
                new MSSQL.Parameter("lastip", SqlDbType.Char, 0, ParameterDirection.Input, lastip),
                new MSSQL.Parameter("lastvisit", SqlDbType.Char, 0, ParameterDirection.Input, lastvisit),
                new MSSQL.Parameter("lastactivity", SqlDbType.Char, 0, ParameterDirection.Input, lastactivity),
                new MSSQL.Parameter("lastpost", SqlDbType.Char, 0, ParameterDirection.Input, lastpost),
                new MSSQL.Parameter("lastpostid", SqlDbType.Int, 0, ParameterDirection.Input, lastpostid),
                new MSSQL.Parameter("lastposttitle", SqlDbType.NChar, 0, ParameterDirection.Input, lastposttitle),
                new MSSQL.Parameter("posts", SqlDbType.Int, 0, ParameterDirection.Input, posts),
                new MSSQL.Parameter("digestposts", SqlDbType.SmallInt, 0, ParameterDirection.Input, digestposts),
                new MSSQL.Parameter("oltime", SqlDbType.Int, 0, ParameterDirection.Input, oltime),
                new MSSQL.Parameter("pageviews", SqlDbType.Int, 0, ParameterDirection.Input, pageviews),
                new MSSQL.Parameter("credits", SqlDbType.Int, 0, ParameterDirection.Input, credits),
                new MSSQL.Parameter("extcredits1", SqlDbType.Float, 0, ParameterDirection.Input, extcredits1),
                new MSSQL.Parameter("extcredits2", SqlDbType.Float, 0, ParameterDirection.Input, extcredits2),
                new MSSQL.Parameter("extcredits3", SqlDbType.Float, 0, ParameterDirection.Input, extcredits3),
                new MSSQL.Parameter("extcredits4", SqlDbType.Float, 0, ParameterDirection.Input, extcredits4),
                new MSSQL.Parameter("extcredits5", SqlDbType.Float, 0, ParameterDirection.Input, extcredits5),
                new MSSQL.Parameter("extcredits6", SqlDbType.Float, 0, ParameterDirection.Input, extcredits6),
                new MSSQL.Parameter("extcredits7", SqlDbType.Float, 0, ParameterDirection.Input, extcredits7),
                new MSSQL.Parameter("extcredits8", SqlDbType.Float, 0, ParameterDirection.Input, extcredits8),
                new MSSQL.Parameter("avatarshowid", SqlDbType.Int, 0, ParameterDirection.Input, avatarshowid),
                new MSSQL.Parameter("email", SqlDbType.Char, 0, ParameterDirection.Input, email),
                new MSSQL.Parameter("bday", SqlDbType.Char, 0, ParameterDirection.Input, bday),
                new MSSQL.Parameter("sigstatus", SqlDbType.Int, 0, ParameterDirection.Input, sigstatus),
                new MSSQL.Parameter("tpp", SqlDbType.Int, 0, ParameterDirection.Input, tpp),
                new MSSQL.Parameter("ppp", SqlDbType.Int, 0, ParameterDirection.Input, ppp),
                new MSSQL.Parameter("templateid", SqlDbType.SmallInt, 0, ParameterDirection.Input, templateid),
                new MSSQL.Parameter("pmsound", SqlDbType.Int, 0, ParameterDirection.Input, pmsound),
                new MSSQL.Parameter("showemail", SqlDbType.Int, 0, ParameterDirection.Input, showemail),
                new MSSQL.Parameter("newsletter", SqlDbType.Int, 0, ParameterDirection.Input, newsletter),
                new MSSQL.Parameter("invisible", SqlDbType.Int, 0, ParameterDirection.Input, invisible),
                new MSSQL.Parameter("newpm", SqlDbType.Int, 0, ParameterDirection.Input, newpm),
                new MSSQL.Parameter("accessmasks", SqlDbType.Int, 0, ParameterDirection.Input, accessmasks),
                new MSSQL.Parameter("website", SqlDbType.VarChar, 0, ParameterDirection.Input, website),
                new MSSQL.Parameter("icq", SqlDbType.VarChar, 0, ParameterDirection.Input, icq),
                new MSSQL.Parameter("qq", SqlDbType.VarChar, 0, ParameterDirection.Input, qq),
                new MSSQL.Parameter("yahoo", SqlDbType.VarChar, 0, ParameterDirection.Input, yahoo),
                new MSSQL.Parameter("msn", SqlDbType.VarChar, 0, ParameterDirection.Input, msn),
                new MSSQL.Parameter("skype", SqlDbType.VarChar, 0, ParameterDirection.Input, skype),
                new MSSQL.Parameter("location", SqlDbType.NVarChar, 0, ParameterDirection.Input, location),
                new MSSQL.Parameter("customstatus", SqlDbType.VarChar, 0, ParameterDirection.Input, customstatus),
                new MSSQL.Parameter("avatar", SqlDbType.VarChar, 0, ParameterDirection.Input, avatar),
                new MSSQL.Parameter("avatarwidth", SqlDbType.Int, 0, ParameterDirection.Input, avatarwidth),
                new MSSQL.Parameter("avatarheight", SqlDbType.Int, 0, ParameterDirection.Input, avatarheight),
                new MSSQL.Parameter("medals", SqlDbType.VarChar, 0, ParameterDirection.Input, medals),
                new MSSQL.Parameter("bio", SqlDbType.NVarChar, 0, ParameterDirection.Input, bio),
                new MSSQL.Parameter("signature", SqlDbType.NVarChar, 0, ParameterDirection.Input, signature),
                new MSSQL.Parameter("sightml", SqlDbType.NVarChar, 0, ParameterDirection.Input, sightml),
                new MSSQL.Parameter("authstr", SqlDbType.VarChar, 0, ParameterDirection.Input, authstr),
                new MSSQL.Parameter("realname", SqlDbType.NVarChar, 0, ParameterDirection.Input, realname),
                new MSSQL.Parameter("idcard", SqlDbType.VarChar, 0, ParameterDirection.Input, idcard),
                new MSSQL.Parameter("mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, mobile),
                new MSSQL.Parameter("phone", SqlDbType.VarChar, 0, ParameterDirection.Input, phone),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("IMemo", SqlDbType.VarChar, 0, ParameterDirection.Input, IMemo)
                );

            return CallResult;
        }

        public static int dnt_deletepost1bypid(int pid, bool chanageposts)
        {
            DataSet ds = null;

            return dnt_deletepost1bypid(ref ds, pid, chanageposts);
        }

        public static int dnt_deletepost1bypid(ref DataSet ds, int pid, bool chanageposts)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_deletepost1bypid", ref ds, ref Outputs,
                new MSSQL.Parameter("pid", SqlDbType.Int, 0, ParameterDirection.Input, pid),
                new MSSQL.Parameter("chanageposts", SqlDbType.Bit, 0, ParameterDirection.Input, chanageposts)
                );

            return CallResult;
        }

        public static int dnt_deleteps(int index, string pmidlist, int userid)
        {
            DataSet ds = null;

            return dnt_deleteps(ref ds, index, pmidlist, userid);
        }

        public static int dnt_deleteps(ref DataSet ds, int index, string pmidlist, int userid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_deleteps", ref ds, ref Outputs,
                new MSSQL.Parameter("index", SqlDbType.Int, 0, ParameterDirection.Input, index),
                new MSSQL.Parameter("pmidlist", SqlDbType.VarChar, 0, ParameterDirection.Input, pmidlist),
                new MSSQL.Parameter("userid", SqlDbType.Int, 0, ParameterDirection.Input, userid)
                );

            return CallResult;
        }

        public static int dnt_deletetopicbytidlist(string tidlist, string posttablename, bool chanageposts)
        {
            DataSet ds = null;

            return dnt_deletetopicbytidlist(ref ds, tidlist, posttablename, chanageposts);
        }

        public static int dnt_deletetopicbytidlist(ref DataSet ds, string tidlist, string posttablename, bool chanageposts)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_deletetopicbytidlist", ref ds, ref Outputs,
                new MSSQL.Parameter("tidlist", SqlDbType.VarChar, 0, ParameterDirection.Input, tidlist),
                new MSSQL.Parameter("posttablename", SqlDbType.VarChar, 0, ParameterDirection.Input, posttablename),
                new MSSQL.Parameter("chanageposts", SqlDbType.Bit, 0, ParameterDirection.Input, chanageposts)
                );

            return CallResult;
        }

        public static int dnt_deletetopictags(int tid)
        {
            DataSet ds = null;

            return dnt_deletetopictags(ref ds, tid);
        }

        public static int dnt_deletetopictags(ref DataSet ds, int tid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_deletetopictags", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid)
                );

            return CallResult;
        }

        public static int dnt_getadmintopiclist(int pagesize, int pageindex, int startnum, string condition)
        {
            DataSet ds = null;

            return dnt_getadmintopiclist(ref ds, pagesize, pageindex, startnum, condition);
        }

        public static int dnt_getadmintopiclist(ref DataSet ds, int pagesize, int pageindex, int startnum, string condition)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getadmintopiclist", ref ds, ref Outputs,
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("startnum", SqlDbType.Int, 0, ParameterDirection.Input, startnum),
                new MSSQL.Parameter("condition", SqlDbType.VarChar, 0, ParameterDirection.Input, condition)
                );

            return CallResult;
        }

        public static int dnt_getalltopiccount(int fid)
        {
            DataSet ds = null;

            return dnt_getalltopiccount(ref ds, fid);
        }

        public static int dnt_getalltopiccount(ref DataSet ds, int fid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getalltopiccount", ref ds, ref Outputs,
                new MSSQL.Parameter("fid", SqlDbType.Int, 0, ParameterDirection.Input, fid)
                );

            return CallResult;
        }

        public static int dnt_getdebatepostlist(int tid, int opinion, int pagesize, int pageindex, string posttablename, string orderby, string ascdesc)
        {
            DataSet ds = null;

            return dnt_getdebatepostlist(ref ds, tid, opinion, pagesize, pageindex, posttablename, orderby, ascdesc);
        }

        public static int dnt_getdebatepostlist(ref DataSet ds, int tid, int opinion, int pagesize, int pageindex, string posttablename, string orderby, string ascdesc)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getdebatepostlist", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("opinion", SqlDbType.Int, 0, ParameterDirection.Input, opinion),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("posttablename", SqlDbType.VarChar, 0, ParameterDirection.Input, posttablename),
                new MSSQL.Parameter("orderby", SqlDbType.VarChar, 0, ParameterDirection.Input, orderby),
                new MSSQL.Parameter("ascdesc", SqlDbType.VarChar, 0, ParameterDirection.Input, ascdesc)
                );

            return CallResult;
        }

        public static int dnt_getfavoritescount(int uid)
        {
            DataSet ds = null;

            return dnt_getfavoritescount(ref ds, uid);
        }

        public static int dnt_getfavoritescount(ref DataSet ds, int uid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getfavoritescount", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid)
                );

            return CallResult;
        }

        public static int dnt_getfavoritescountbytype(int uid, short typeid)
        {
            DataSet ds = null;

            return dnt_getfavoritescountbytype(ref ds, uid, typeid);
        }

        public static int dnt_getfavoritescountbytype(ref DataSet ds, int uid, short typeid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getfavoritescountbytype", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("typeid", SqlDbType.TinyInt, 0, ParameterDirection.Input, typeid)
                );

            return CallResult;
        }

        public static int dnt_getfavoriteslist(int uid, int pagesize, int pageindex)
        {
            DataSet ds = null;

            return dnt_getfavoriteslist(ref ds, uid, pagesize, pageindex);
        }

        public static int dnt_getfavoriteslist(ref DataSet ds, int uid, int pagesize, int pageindex)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getfavoriteslist", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex)
                );

            return CallResult;
        }

        public static int dnt_getfavoriteslistbyalbum(int uid, int pagesize, int pageindex)
        {
            DataSet ds = null;

            return dnt_getfavoriteslistbyalbum(ref ds, uid, pagesize, pageindex);
        }

        public static int dnt_getfavoriteslistbyalbum(ref DataSet ds, int uid, int pagesize, int pageindex)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getfavoriteslistbyalbum", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex)
                );

            return CallResult;
        }

        public static int dnt_getfirstpost1id(int tid)
        {
            DataSet ds = null;

            return dnt_getfirstpost1id(ref ds, tid);
        }

        public static int dnt_getfirstpost1id(ref DataSet ds, int tid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getfirstpost1id", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid)
                );

            return CallResult;
        }

        public static int dnt_getforumnewtopics(int fid)
        {
            DataSet ds = null;

            return dnt_getforumnewtopics(ref ds, fid);
        }

        public static int dnt_getforumnewtopics(ref DataSet ds, int fid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getforumnewtopics", ref ds, ref Outputs,
                new MSSQL.Parameter("fid", SqlDbType.Int, 0, ParameterDirection.Input, fid)
                );

            return CallResult;
        }

        public static int dnt_getlastexecutescheduledeventdatetime(string key, string servername, ref DateTime lastexecuted)
        {
            DataSet ds = null;

            return dnt_getlastexecutescheduledeventdatetime(ref ds, key, servername, ref lastexecuted);
        }

        public static int dnt_getlastexecutescheduledeventdatetime(ref DataSet ds, string key, string servername, ref DateTime lastexecuted)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getlastexecutescheduledeventdatetime", ref ds, ref Outputs,
                new MSSQL.Parameter("key", SqlDbType.VarChar, 0, ParameterDirection.Input, key),
                new MSSQL.Parameter("servername", SqlDbType.VarChar, 0, ParameterDirection.Input, servername),
                new MSSQL.Parameter("lastexecuted", SqlDbType.DateTime, 8, ParameterDirection.Output, lastexecuted)
                );

            try
            {
                lastexecuted = System.Convert.ToDateTime(Outputs["lastexecuted"]);
            }
            catch { }

            return CallResult;
        }

        public static int dnt_getlastpostlist(int tid, int postnum, string posttablename)
        {
            DataSet ds = null;

            return dnt_getlastpostlist(ref ds, tid, postnum, posttablename);
        }

        public static int dnt_getlastpostlist(ref DataSet ds, int tid, int postnum, string posttablename)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getlastpostlist", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("postnum", SqlDbType.Int, 0, ParameterDirection.Input, postnum),
                new MSSQL.Parameter("posttablename", SqlDbType.VarChar, 0, ParameterDirection.Input, posttablename)
                );

            return CallResult;
        }

        public static int dnt_getmyattachments(int uid, int pageindex, int pagesize)
        {
            DataSet ds = null;

            return dnt_getmyattachments(ref ds, uid, pageindex, pagesize);
        }

        public static int dnt_getmyattachments(ref DataSet ds, int uid, int pageindex, int pagesize)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getmyattachments", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize)
                );

            return CallResult;
        }

        public static int dnt_getmyattachmentsbytype(int uid, int pageindex, int pagesize, string extlist)
        {
            DataSet ds = null;

            return dnt_getmyattachmentsbytype(ref ds, uid, pageindex, pagesize, extlist);
        }

        public static int dnt_getmyattachmentsbytype(ref DataSet ds, int uid, int pageindex, int pagesize, string extlist)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getmyattachmentsbytype", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("extlist", SqlDbType.VarChar, 0, ParameterDirection.Input, extlist)
                );

            return CallResult;
        }

        public static int dnt_getmyposts(int uid, int pageindex, int pagesize)
        {
            DataSet ds = null;

            return dnt_getmyposts(ref ds, uid, pageindex, pagesize);
        }

        public static int dnt_getmyposts(ref DataSet ds, int uid, int pageindex, int pagesize)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getmyposts", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize)
                );

            return CallResult;
        }

        public static int dnt_getmytopics(int uid, int pageindex, int pagesize)
        {
            DataSet ds = null;

            return dnt_getmytopics(ref ds, uid, pageindex, pagesize);
        }

        public static int dnt_getmytopics(ref DataSet ds, int uid, int pageindex, int pagesize)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getmytopics", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize)
                );

            return CallResult;
        }

        public static int dnt_getnewtopics(string fidlist)
        {
            DataSet ds = null;

            return dnt_getnewtopics(ref ds, fidlist);
        }

        public static int dnt_getnewtopics(ref DataSet ds, string fidlist)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getnewtopics", ref ds, ref Outputs,
                new MSSQL.Parameter("fidlist", SqlDbType.VarChar, 0, ParameterDirection.Input, fidlist)
                );

            return CallResult;
        }

        public static int dnt_getpmcount(int userid, int folder, int state)
        {
            DataSet ds = null;

            return dnt_getpmcount(ref ds, userid, folder, state);
        }

        public static int dnt_getpmcount(ref DataSet ds, int userid, int folder, int state)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getpmcount", ref ds, ref Outputs,
                new MSSQL.Parameter("userid", SqlDbType.Int, 0, ParameterDirection.Input, userid),
                new MSSQL.Parameter("folder", SqlDbType.Int, 0, ParameterDirection.Input, folder),
                new MSSQL.Parameter("state", SqlDbType.Int, 0, ParameterDirection.Input, state)
                );

            return CallResult;
        }

        public static int dnt_getpmlist(int userid, int folder, int pagesize, int pageindex, string strwhere)
        {
            DataSet ds = null;

            return dnt_getpmlist(ref ds, userid, folder, pagesize, pageindex, strwhere);
        }

        public static int dnt_getpmlist(ref DataSet ds, int userid, int folder, int pagesize, int pageindex, string strwhere)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getpmlist", ref ds, ref Outputs,
                new MSSQL.Parameter("userid", SqlDbType.Int, 0, ParameterDirection.Input, userid),
                new MSSQL.Parameter("folder", SqlDbType.Int, 0, ParameterDirection.Input, folder),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("strwhere", SqlDbType.VarChar, 0, ParameterDirection.Input, strwhere)
                );

            return CallResult;
        }

        public static int dnt_getpost1count(int tid)
        {
            DataSet ds = null;

            return dnt_getpost1count(ref ds, tid);
        }

        public static int dnt_getpost1count(ref DataSet ds, int tid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getpost1count", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid)
                );

            return CallResult;
        }

        public static int dnt_getpost1tree(int tid)
        {
            DataSet ds = null;

            return dnt_getpost1tree(ref ds, tid);
        }

        public static int dnt_getpost1tree(ref DataSet ds, int tid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getpost1tree", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid)
                );

            return CallResult;
        }

        public static int dnt_getpostcountbycondition(string condition, string posttablename)
        {
            DataSet ds = null;

            return dnt_getpostcountbycondition(ref ds, condition, posttablename);
        }

        public static int dnt_getpostcountbycondition(ref DataSet ds, string condition, string posttablename)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getpostcountbycondition", ref ds, ref Outputs,
                new MSSQL.Parameter("condition", SqlDbType.VarChar, 0, ParameterDirection.Input, condition),
                new MSSQL.Parameter("posttablename", SqlDbType.VarChar, 0, ParameterDirection.Input, posttablename)
                );

            return CallResult;
        }

        public static int dnt_getpostlist(int tid, int pagesize, int pageindex, string posttablename)
        {
            DataSet ds = null;

            return dnt_getpostlist(ref ds, tid, pagesize, pageindex, posttablename);
        }

        public static int dnt_getpostlist(ref DataSet ds, int tid, int pagesize, int pageindex, string posttablename)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getpostlist", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("posttablename", SqlDbType.VarChar, 0, ParameterDirection.Input, posttablename)
                );

            return CallResult;
        }

        public static int dnt_getpostlistbycondition(int tid, int pagesize, int pageindex, string condition, string posttablename)
        {
            DataSet ds = null;

            return dnt_getpostlistbycondition(ref ds, tid, pagesize, pageindex, condition, posttablename);
        }

        public static int dnt_getpostlistbycondition(ref DataSet ds, int tid, int pagesize, int pageindex, string condition, string posttablename)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getpostlistbycondition", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("condition", SqlDbType.VarChar, 0, ParameterDirection.Input, condition),
                new MSSQL.Parameter("posttablename", SqlDbType.VarChar, 0, ParameterDirection.Input, posttablename)
                );

            return CallResult;
        }

        public static int dnt_getshortuserinfo(int uid)
        {
            DataSet ds = null;

            return dnt_getshortuserinfo(ref ds, uid);
        }

        public static int dnt_getshortuserinfo(ref DataSet ds, int uid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getshortuserinfo", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid)
                );

            return CallResult;
        }

        public static int dnt_getsinglepost1(int tid, int pid)
        {
            DataSet ds = null;

            return dnt_getsinglepost1(ref ds, tid, pid);
        }

        public static int dnt_getsinglepost1(ref DataSet ds, int tid, int pid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getsinglepost1", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("pid", SqlDbType.Int, 0, ParameterDirection.Input, pid)
                );

            return CallResult;
        }

        public static int dnt_getsitemapnewtopics(string fidlist)
        {
            DataSet ds = null;

            return dnt_getsitemapnewtopics(ref ds, fidlist);
        }

        public static int dnt_getsitemapnewtopics(ref DataSet ds, string fidlist)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getsitemapnewtopics", ref ds, ref Outputs,
                new MSSQL.Parameter("fidlist", SqlDbType.VarChar, 0, ParameterDirection.Input, fidlist)
                );

            return CallResult;
        }

        public static int dnt_gettopiccount(int fid)
        {
            DataSet ds = null;

            return dnt_gettopiccount(ref ds, fid);
        }

        public static int dnt_gettopiccount(ref DataSet ds, int fid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettopiccount", ref ds, ref Outputs,
                new MSSQL.Parameter("fid", SqlDbType.Int, 0, ParameterDirection.Input, fid)
                );

            return CallResult;
        }

        public static int dnt_gettopiccountbycondition(int fid, int state, string condition)
        {
            DataSet ds = null;

            return dnt_gettopiccountbycondition(ref ds, fid, state, condition);
        }

        public static int dnt_gettopiccountbycondition(ref DataSet ds, int fid, int state, string condition)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettopiccountbycondition", ref ds, ref Outputs,
                new MSSQL.Parameter("fid", SqlDbType.Int, 0, ParameterDirection.Input, fid),
                new MSSQL.Parameter("state", SqlDbType.Int, 0, ParameterDirection.Input, state),
                new MSSQL.Parameter("condition", SqlDbType.VarChar, 0, ParameterDirection.Input, condition)
                );

            return CallResult;
        }

        public static int dnt_gettopiccountbytype(string condition)
        {
            DataSet ds = null;

            return dnt_gettopiccountbytype(ref ds, condition);
        }

        public static int dnt_gettopiccountbytype(ref DataSet ds, string condition)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettopiccountbytype", ref ds, ref Outputs,
                new MSSQL.Parameter("condition", SqlDbType.VarChar, 0, ParameterDirection.Input, condition)
                );

            return CallResult;
        }

        public static int dnt_gettopiclist(int fid, int pagesize, int pageindex, int startnum, string condition)
        {
            DataSet ds = null;

            return dnt_gettopiclist(ref ds, fid, pagesize, pageindex, startnum, condition);
        }

        public static int dnt_gettopiclist(ref DataSet ds, int fid, int pagesize, int pageindex, int startnum, string condition)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettopiclist", ref ds, ref Outputs,
                new MSSQL.Parameter("fid", SqlDbType.Int, 0, ParameterDirection.Input, fid),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("startnum", SqlDbType.Int, 0, ParameterDirection.Input, startnum),
                new MSSQL.Parameter("condition", SqlDbType.VarChar, 0, ParameterDirection.Input, condition)
                );

            return CallResult;
        }

        public static int dnt_gettopiclistbydate(int fid, int pagesize, int pageindex, int startnum, string condition, string orderby, int ascdesc)
        {
            DataSet ds = null;

            return dnt_gettopiclistbydate(ref ds, fid, pagesize, pageindex, startnum, condition, orderby, ascdesc);
        }

        public static int dnt_gettopiclistbydate(ref DataSet ds, int fid, int pagesize, int pageindex, int startnum, string condition, string orderby, int ascdesc)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettopiclistbydate", ref ds, ref Outputs,
                new MSSQL.Parameter("fid", SqlDbType.Int, 0, ParameterDirection.Input, fid),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("startnum", SqlDbType.Int, 0, ParameterDirection.Input, startnum),
                new MSSQL.Parameter("condition", SqlDbType.VarChar, 0, ParameterDirection.Input, condition),
                new MSSQL.Parameter("orderby", SqlDbType.VarChar, 0, ParameterDirection.Input, orderby),
                new MSSQL.Parameter("ascdesc", SqlDbType.Int, 0, ParameterDirection.Input, ascdesc)
                );

            return CallResult;
        }

        public static int dnt_gettopiclistbynumber(string orderfield, int pageindex, int pagesize, string strwhere, bool ordertype)
        {
            DataSet ds = null;

            return dnt_gettopiclistbynumber(ref ds, orderfield, pageindex, pagesize, strwhere, ordertype);
        }

        public static int dnt_gettopiclistbynumber(ref DataSet ds, string orderfield, int pageindex, int pagesize, string strwhere, bool ordertype)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettopiclistbynumber", ref ds, ref Outputs,
                new MSSQL.Parameter("orderfield", SqlDbType.VarChar, 0, ParameterDirection.Input, orderfield),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("strwhere", SqlDbType.VarChar, 0, ParameterDirection.Input, strwhere),
                new MSSQL.Parameter("ordertype", SqlDbType.Bit, 0, ParameterDirection.Input, ordertype)
                );

            return CallResult;
        }

        public static int dnt_gettopiclistbytag(int tagid, int pageindex, int pagesize)
        {
            DataSet ds = null;

            return dnt_gettopiclistbytag(ref ds, tagid, pageindex, pagesize);
        }

        public static int dnt_gettopiclistbytag(ref DataSet ds, int tagid, int pageindex, int pagesize)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettopiclistbytag", ref ds, ref Outputs,
                new MSSQL.Parameter("tagid", SqlDbType.Int, 0, ParameterDirection.Input, tagid),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize)
                );

            return CallResult;
        }

        public static int dnt_gettopiclistbytype(int pagesize, int pageindex, int startnum, string condition, int ascdesc)
        {
            DataSet ds = null;

            return dnt_gettopiclistbytype(ref ds, pagesize, pageindex, startnum, condition, ascdesc);
        }

        public static int dnt_gettopiclistbytype(ref DataSet ds, int pagesize, int pageindex, int startnum, string condition, int ascdesc)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettopiclistbytype", ref ds, ref Outputs,
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("startnum", SqlDbType.Int, 0, ParameterDirection.Input, startnum),
                new MSSQL.Parameter("condition", SqlDbType.VarChar, 0, ParameterDirection.Input, condition),
                new MSSQL.Parameter("ascdesc", SqlDbType.Int, 0, ParameterDirection.Input, ascdesc)
                );

            return CallResult;
        }

        public static int dnt_gettopiclistbytypedate(int pagesize, int pageindex, int startnum, string condition, string orderby, int ascdesc)
        {
            DataSet ds = null;

            return dnt_gettopiclistbytypedate(ref ds, pagesize, pageindex, startnum, condition, orderby, ascdesc);
        }

        public static int dnt_gettopiclistbytypedate(ref DataSet ds, int pagesize, int pageindex, int startnum, string condition, string orderby, int ascdesc)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettopiclistbytypedate", ref ds, ref Outputs,
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("startnum", SqlDbType.Int, 0, ParameterDirection.Input, startnum),
                new MSSQL.Parameter("condition", SqlDbType.VarChar, 0, ParameterDirection.Input, condition),
                new MSSQL.Parameter("orderby", SqlDbType.VarChar, 0, ParameterDirection.Input, orderby),
                new MSSQL.Parameter("ascdesc", SqlDbType.Int, 0, ParameterDirection.Input, ascdesc)
                );

            return CallResult;
        }

        public static int dnt_gettoptopiclist(int fid, int pagesize, int pageindex, string tids)
        {
            DataSet ds = null;

            return dnt_gettoptopiclist(ref ds, fid, pagesize, pageindex, tids);
        }

        public static int dnt_gettoptopiclist(ref DataSet ds, int fid, int pagesize, int pageindex, string tids)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_gettoptopiclist", ref ds, ref Outputs,
                new MSSQL.Parameter("fid", SqlDbType.Int, 0, ParameterDirection.Input, fid),
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("tids", SqlDbType.VarChar, 0, ParameterDirection.Input, tids)
                );

            return CallResult;
        }

        public static int dnt_getuserinfo(int uid)
        {
            DataSet ds = null;

            return dnt_getuserinfo(ref ds, uid);
        }

        public static int dnt_getuserinfo(ref DataSet ds, int uid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getuserinfo", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid)
                );

            return CallResult;
        }

        public static int dnt_getuserlist(int pagesize, int pageindex, string orderby)
        {
            DataSet ds = null;

            return dnt_getuserlist(ref ds, pagesize, pageindex, orderby);
        }

        public static int dnt_getuserlist(ref DataSet ds, int pagesize, int pageindex, string orderby)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_getuserlist", ref ds, ref Outputs,
                new MSSQL.Parameter("pagesize", SqlDbType.Int, 0, ParameterDirection.Input, pagesize),
                new MSSQL.Parameter("pageindex", SqlDbType.Int, 0, ParameterDirection.Input, pageindex),
                new MSSQL.Parameter("orderby", SqlDbType.VarChar, 0, ParameterDirection.Input, orderby)
                );

            return CallResult;
        }

        public static int dnt_revisedebatetopicdiggs(int tid, int opinion, ref int count)
        {
            DataSet ds = null;

            return dnt_revisedebatetopicdiggs(ref ds, tid, opinion, ref count);
        }

        public static int dnt_revisedebatetopicdiggs(ref DataSet ds, int tid, int opinion, ref int count)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_revisedebatetopicdiggs", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("opinion", SqlDbType.Int, 0, ParameterDirection.Input, opinion),
                new MSSQL.Parameter("count", SqlDbType.Int, 4, ParameterDirection.Output, count)
                );

            try
            {
                count = System.Convert.ToInt32(Outputs["count"]);
            }
            catch { }

            return CallResult;
        }

        public static int dnt_setlastexecutescheduledeventdatetime(string key, string servername, DateTime lastexecuted)
        {
            DataSet ds = null;

            return dnt_setlastexecutescheduledeventdatetime(ref ds, key, servername, lastexecuted);
        }

        public static int dnt_setlastexecutescheduledeventdatetime(ref DataSet ds, string key, string servername, DateTime lastexecuted)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_setlastexecutescheduledeventdatetime", ref ds, ref Outputs,
                new MSSQL.Parameter("key", SqlDbType.VarChar, 0, ParameterDirection.Input, key),
                new MSSQL.Parameter("servername", SqlDbType.VarChar, 0, ParameterDirection.Input, servername),
                new MSSQL.Parameter("lastexecuted", SqlDbType.DateTime, 0, ParameterDirection.Input, lastexecuted)
                );

            return CallResult;
        }

        public static int dnt_shrinklog(string DBName)
        {
            DataSet ds = null;

            return dnt_shrinklog(ref ds, DBName);
        }

        public static int dnt_shrinklog(ref DataSet ds, string DBName)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_shrinklog", ref ds, ref Outputs,
                new MSSQL.Parameter("DBName", SqlDbType.NChar, 0, ParameterDirection.Input, DBName)
                );

            return CallResult;
        }

        public static int dnt_updateadmingroup(short admingid, short alloweditpost, short alloweditpoll, short allowstickthread, short allowmodpost, short allowdelpost, short allowmassprune, short allowrefund, short allowcensorword, short allowviewip, short allowbanip, short allowedituser, short allowmoduser, short allowbanuser, short allowpostannounce, short allowviewlog, short disablepostctrl, short allowviewrealname)
        {
            DataSet ds = null;

            return dnt_updateadmingroup(ref ds, admingid, alloweditpost, alloweditpoll, allowstickthread, allowmodpost, allowdelpost, allowmassprune, allowrefund, allowcensorword, allowviewip, allowbanip, allowedituser, allowmoduser, allowbanuser, allowpostannounce, allowviewlog, disablepostctrl, allowviewrealname);
        }

        public static int dnt_updateadmingroup(ref DataSet ds, short admingid, short alloweditpost, short alloweditpoll, short allowstickthread, short allowmodpost, short allowdelpost, short allowmassprune, short allowrefund, short allowcensorword, short allowviewip, short allowbanip, short allowedituser, short allowmoduser, short allowbanuser, short allowpostannounce, short allowviewlog, short disablepostctrl, short allowviewrealname)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_updateadmingroup", ref ds, ref Outputs,
                new MSSQL.Parameter("admingid", SqlDbType.SmallInt, 0, ParameterDirection.Input, admingid),
                new MSSQL.Parameter("alloweditpost", SqlDbType.TinyInt, 0, ParameterDirection.Input, alloweditpost),
                new MSSQL.Parameter("alloweditpoll", SqlDbType.TinyInt, 0, ParameterDirection.Input, alloweditpoll),
                new MSSQL.Parameter("allowstickthread", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowstickthread),
                new MSSQL.Parameter("allowmodpost", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowmodpost),
                new MSSQL.Parameter("allowdelpost", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowdelpost),
                new MSSQL.Parameter("allowmassprune", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowmassprune),
                new MSSQL.Parameter("allowrefund", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowrefund),
                new MSSQL.Parameter("allowcensorword", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowcensorword),
                new MSSQL.Parameter("allowviewip", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowviewip),
                new MSSQL.Parameter("allowbanip", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowbanip),
                new MSSQL.Parameter("allowedituser", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowedituser),
                new MSSQL.Parameter("allowmoduser", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowmoduser),
                new MSSQL.Parameter("allowbanuser", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowbanuser),
                new MSSQL.Parameter("allowpostannounce", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowpostannounce),
                new MSSQL.Parameter("allowviewlog", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowviewlog),
                new MSSQL.Parameter("disablepostctrl", SqlDbType.TinyInt, 0, ParameterDirection.Input, disablepostctrl),
                new MSSQL.Parameter("allowviewrealname", SqlDbType.TinyInt, 0, ParameterDirection.Input, allowviewrealname)
                );

            return CallResult;
        }

        public static int dnt_updatepost1(int pid, string title, string message, string lastedit, int invisible, int usesig, int htmlon, int smileyoff, int bbcodeoff, int parseurloff)
        {
            DataSet ds = null;

            return dnt_updatepost1(ref ds, pid, title, message, lastedit, invisible, usesig, htmlon, smileyoff, bbcodeoff, parseurloff);
        }

        public static int dnt_updatepost1(ref DataSet ds, int pid, string title, string message, string lastedit, int invisible, int usesig, int htmlon, int smileyoff, int bbcodeoff, int parseurloff)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_updatepost1", ref ds, ref Outputs,
                new MSSQL.Parameter("pid", SqlDbType.Int, 0, ParameterDirection.Input, pid),
                new MSSQL.Parameter("title", SqlDbType.NVarChar, 0, ParameterDirection.Input, title),
                new MSSQL.Parameter("message", SqlDbType.NText, 0, ParameterDirection.Input, message),
                new MSSQL.Parameter("lastedit", SqlDbType.NVarChar, 0, ParameterDirection.Input, lastedit),
                new MSSQL.Parameter("invisible", SqlDbType.Int, 0, ParameterDirection.Input, invisible),
                new MSSQL.Parameter("usesig", SqlDbType.Int, 0, ParameterDirection.Input, usesig),
                new MSSQL.Parameter("htmlon", SqlDbType.Int, 0, ParameterDirection.Input, htmlon),
                new MSSQL.Parameter("smileyoff", SqlDbType.Int, 0, ParameterDirection.Input, smileyoff),
                new MSSQL.Parameter("bbcodeoff", SqlDbType.Int, 0, ParameterDirection.Input, bbcodeoff),
                new MSSQL.Parameter("parseurloff", SqlDbType.Int, 0, ParameterDirection.Input, parseurloff)
                );

            return CallResult;
        }

        public static int dnt_updatetopic(int tid, short fid, short iconid, string title, short typeid, int readperm, short price, string poster, int posterid, DateTime postdatetime, DateTime lastpost, string lastposter, int replies, int displayorder, string highlight, int digest, int rate, int hide, int special, int attachment, int moderated, int closed, int magic)
        {
            DataSet ds = null;

            return dnt_updatetopic(ref ds, tid, fid, iconid, title, typeid, readperm, price, poster, posterid, postdatetime, lastpost, lastposter, replies, displayorder, highlight, digest, rate, hide, special, attachment, moderated, closed, magic);
        }

        public static int dnt_updatetopic(ref DataSet ds, int tid, short fid, short iconid, string title, short typeid, int readperm, short price, string poster, int posterid, DateTime postdatetime, DateTime lastpost, string lastposter, int replies, int displayorder, string highlight, int digest, int rate, int hide, int special, int attachment, int moderated, int closed, int magic)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_updatetopic", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("fid", SqlDbType.SmallInt, 0, ParameterDirection.Input, fid),
                new MSSQL.Parameter("iconid", SqlDbType.SmallInt, 0, ParameterDirection.Input, iconid),
                new MSSQL.Parameter("title", SqlDbType.NChar, 0, ParameterDirection.Input, title),
                new MSSQL.Parameter("typeid", SqlDbType.SmallInt, 0, ParameterDirection.Input, typeid),
                new MSSQL.Parameter("readperm", SqlDbType.Int, 0, ParameterDirection.Input, readperm),
                new MSSQL.Parameter("price", SqlDbType.SmallInt, 0, ParameterDirection.Input, price),
                new MSSQL.Parameter("poster", SqlDbType.Char, 0, ParameterDirection.Input, poster),
                new MSSQL.Parameter("posterid", SqlDbType.Int, 0, ParameterDirection.Input, posterid),
                new MSSQL.Parameter("postdatetime", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, postdatetime),
                new MSSQL.Parameter("lastpost", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, lastpost),
                new MSSQL.Parameter("lastposter", SqlDbType.Char, 0, ParameterDirection.Input, lastposter),
                new MSSQL.Parameter("replies", SqlDbType.Int, 0, ParameterDirection.Input, replies),
                new MSSQL.Parameter("displayorder", SqlDbType.Int, 0, ParameterDirection.Input, displayorder),
                new MSSQL.Parameter("highlight", SqlDbType.VarChar, 0, ParameterDirection.Input, highlight),
                new MSSQL.Parameter("digest", SqlDbType.Int, 0, ParameterDirection.Input, digest),
                new MSSQL.Parameter("rate", SqlDbType.Int, 0, ParameterDirection.Input, rate),
                new MSSQL.Parameter("hide", SqlDbType.Int, 0, ParameterDirection.Input, hide),
                new MSSQL.Parameter("special", SqlDbType.Int, 0, ParameterDirection.Input, special),
                new MSSQL.Parameter("attachment", SqlDbType.Int, 0, ParameterDirection.Input, attachment),
                new MSSQL.Parameter("moderated", SqlDbType.Int, 0, ParameterDirection.Input, moderated),
                new MSSQL.Parameter("closed", SqlDbType.Int, 0, ParameterDirection.Input, closed),
                new MSSQL.Parameter("magic", SqlDbType.Int, 0, ParameterDirection.Input, magic)
                );

            return CallResult;
        }

        public static int dnt_updatetopicviewcount(int tid, int viewcount)
        {
            DataSet ds = null;

            return dnt_updatetopicviewcount(ref ds, tid, viewcount);
        }

        public static int dnt_updatetopicviewcount(ref DataSet ds, int tid, int viewcount)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_updatetopicviewcount", ref ds, ref Outputs,
                new MSSQL.Parameter("tid", SqlDbType.Int, 0, ParameterDirection.Input, tid),
                new MSSQL.Parameter("viewcount", SqlDbType.Int, 0, ParameterDirection.Input, viewcount)
                );

            return CallResult;
        }

        public static int dnt_updateuserauthstr(int uid, string authstr, int authflag)
        {
            DataSet ds = null;

            return dnt_updateuserauthstr(ref ds, uid, authstr, authflag);
        }

        public static int dnt_updateuserauthstr(ref DataSet ds, int uid, string authstr, int authflag)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_updateuserauthstr", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("authstr", SqlDbType.Char, 0, ParameterDirection.Input, authstr),
                new MSSQL.Parameter("authflag", SqlDbType.Int, 0, ParameterDirection.Input, authflag)
                );

            return CallResult;
        }

        public static int dnt_updateuserforumsetting(int uid, int tpp, int ppp, int invisible, string customstatus, int sigstatus, string signature, string sightml)
        {
            DataSet ds = null;

            return dnt_updateuserforumsetting(ref ds, uid, tpp, ppp, invisible, customstatus, sigstatus, signature, sightml);
        }

        public static int dnt_updateuserforumsetting(ref DataSet ds, int uid, int tpp, int ppp, int invisible, string customstatus, int sigstatus, string signature, string sightml)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_updateuserforumsetting", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("tpp", SqlDbType.Int, 0, ParameterDirection.Input, tpp),
                new MSSQL.Parameter("ppp", SqlDbType.Int, 0, ParameterDirection.Input, ppp),
                new MSSQL.Parameter("invisible", SqlDbType.Int, 0, ParameterDirection.Input, invisible),
                new MSSQL.Parameter("customstatus", SqlDbType.VarChar, 0, ParameterDirection.Input, customstatus),
                new MSSQL.Parameter("sigstatus", SqlDbType.Int, 0, ParameterDirection.Input, sigstatus),
                new MSSQL.Parameter("signature", SqlDbType.NVarChar, 0, ParameterDirection.Input, signature),
                new MSSQL.Parameter("sightml", SqlDbType.NVarChar, 0, ParameterDirection.Input, sightml)
                );

            return CallResult;
        }

        public static int dnt_updateuserpassword(int uid, string password)
        {
            DataSet ds = null;

            return dnt_updateuserpassword(ref ds, uid, password);
        }

        public static int dnt_updateuserpassword(ref DataSet ds, int uid, string password)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_updateuserpassword", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("password", SqlDbType.Char, 0, ParameterDirection.Input, password)
                );

            return CallResult;
        }

        public static int dnt_updateuserpreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        {
            DataSet ds = null;

            return dnt_updateuserpreference(ref ds, uid, avatar, avatarwidth, avatarheight, templateid);
        }

        public static int dnt_updateuserpreference(ref DataSet ds, int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_updateuserpreference", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("avatar", SqlDbType.VarChar, 0, ParameterDirection.Input, avatar),
                new MSSQL.Parameter("avatarwidth", SqlDbType.Int, 0, ParameterDirection.Input, avatarwidth),
                new MSSQL.Parameter("avatarheight", SqlDbType.Int, 0, ParameterDirection.Input, avatarheight),
                new MSSQL.Parameter("templateid", SqlDbType.Int, 0, ParameterDirection.Input, templateid)
                );

            return CallResult;
        }

        public static int dnt_updateuserprofile(int uid, string website, string nickname, int gender, string bday, string icq, string qq, string yahoo, string msn, string skype, string location, string bio, string idcard, string mobile, string phone)
        {
            DataSet ds = null;

            return dnt_updateuserprofile(ref ds, uid, website, nickname, gender, bday, icq, qq, yahoo, msn, skype, location, bio, idcard, mobile, phone);
        }

        public static int dnt_updateuserprofile(ref DataSet ds, int uid, string website, string nickname, int gender, string bday, string icq, string qq, string yahoo, string msn, string skype, string location, string bio, string idcard, string mobile, string phone)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("dnt_updateuserprofile", ref ds, ref Outputs,
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("website", SqlDbType.NVarChar, 0, ParameterDirection.Input, website),
                new MSSQL.Parameter("nickname", SqlDbType.NChar, 0, ParameterDirection.Input, nickname),
                new MSSQL.Parameter("gender", SqlDbType.Int, 0, ParameterDirection.Input, gender),
                new MSSQL.Parameter("bday", SqlDbType.Char, 0, ParameterDirection.Input, bday),
                new MSSQL.Parameter("icq", SqlDbType.VarChar, 0, ParameterDirection.Input, icq),
                new MSSQL.Parameter("qq", SqlDbType.VarChar, 0, ParameterDirection.Input, qq),
                new MSSQL.Parameter("yahoo", SqlDbType.VarChar, 0, ParameterDirection.Input, yahoo),
                new MSSQL.Parameter("msn", SqlDbType.VarChar, 0, ParameterDirection.Input, msn),
                new MSSQL.Parameter("skype", SqlDbType.VarChar, 0, ParameterDirection.Input, skype),
                new MSSQL.Parameter("location", SqlDbType.NVarChar, 0, ParameterDirection.Input, location),
                new MSSQL.Parameter("bio", SqlDbType.NVarChar, 0, ParameterDirection.Input, bio),
                new MSSQL.Parameter("idcard", SqlDbType.VarChar, 0, ParameterDirection.Input, idcard),
                new MSSQL.Parameter("mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, mobile),
                new MSSQL.Parameter("phone", SqlDbType.VarChar, 0, ParameterDirection.Input, phone)
                );

            return CallResult;
        }

        public static int P_ClubLogin(long SiteID, long Userid, string IPAddress, short Description)
        {
            DataSet ds = null;

            return P_ClubLogin(ref ds, SiteID, Userid, IPAddress, Description);
        }

        public static int P_ClubLogin(ref DataSet ds, long SiteID, long Userid, string IPAddress, short Description)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("P_ClubLogin", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Userid", SqlDbType.BigInt, 0, ParameterDirection.Input, Userid),
                new MSSQL.Parameter("IPAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IPAddress),
                new MSSQL.Parameter("Description", SqlDbType.SmallInt, 0, ParameterDirection.Input, Description)
                );

            return CallResult;
        }

        public static int P_NewsAdd(long SiteID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, int IsusesId, ref long NewsID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsAdd(ref ds, SiteID, TypeID, DateTime, Title, Content, ImageUrl, isShow, isHasImage, isCanComments, isCommend, isHot, ReadCount, IsusesId, ref NewsID, ref ReturnDescription);
        }

        public static int P_NewsAdd(ref DataSet ds, long SiteID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, int IsusesId, ref long NewsID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("P_NewsAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("TypeID", SqlDbType.Int, 0, ParameterDirection.Input, TypeID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("ImageUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, ImageUrl),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("isHasImage", SqlDbType.Bit, 0, ParameterDirection.Input, isHasImage),
                new MSSQL.Parameter("isCanComments", SqlDbType.Bit, 0, ParameterDirection.Input, isCanComments),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("isHot", SqlDbType.Bit, 0, ParameterDirection.Input, isHot),
                new MSSQL.Parameter("ReadCount", SqlDbType.BigInt, 0, ParameterDirection.Input, ReadCount),
                new MSSQL.Parameter("IsusesId", SqlDbType.Int, 0, ParameterDirection.Input, IsusesId),
                new MSSQL.Parameter("NewsID", SqlDbType.BigInt, 8, ParameterDirection.Output, NewsID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                NewsID = System.Convert.ToInt64(Outputs["NewsID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_NewsDelete(long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsDelete(ref ds, SiteID, ID, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsDelete(ref DataSet ds, long SiteID, long ID, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("P_NewsDelete", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_NewsEdit(long SiteID, long ID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_NewsEdit(ref ds, SiteID, ID, TypeID, DateTime, Title, Content, ImageUrl, isShow, isHasImage, isCanComments, isCommend, isHot, ReadCount, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_NewsEdit(ref DataSet ds, long SiteID, long ID, int TypeID, DateTime DateTime, string Title, string Content, string ImageUrl, bool isShow, bool isHasImage, bool isCanComments, bool isCommend, bool isHot, long ReadCount, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("P_NewsEdit", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID),
                new MSSQL.Parameter("TypeID", SqlDbType.Int, 0, ParameterDirection.Input, TypeID),
                new MSSQL.Parameter("DateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime),
                new MSSQL.Parameter("Title", SqlDbType.VarChar, 0, ParameterDirection.Input, Title),
                new MSSQL.Parameter("Content", SqlDbType.VarChar, 0, ParameterDirection.Input, Content),
                new MSSQL.Parameter("ImageUrl", SqlDbType.VarChar, 0, ParameterDirection.Input, ImageUrl),
                new MSSQL.Parameter("isShow", SqlDbType.Bit, 0, ParameterDirection.Input, isShow),
                new MSSQL.Parameter("isHasImage", SqlDbType.Bit, 0, ParameterDirection.Input, isHasImage),
                new MSSQL.Parameter("isCanComments", SqlDbType.Bit, 0, ParameterDirection.Input, isCanComments),
                new MSSQL.Parameter("isCommend", SqlDbType.Bit, 0, ParameterDirection.Input, isCommend),
                new MSSQL.Parameter("isHot", SqlDbType.Bit, 0, ParameterDirection.Input, isHot),
                new MSSQL.Parameter("ReadCount", SqlDbType.BigInt, 0, ParameterDirection.Input, ReadCount),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserAdd(long SiteID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, short UserType, short BankType, string BankName, string BankCardNumber, long CommenderID, long CpsID, string AlipayName, string Memo, string VisitSource, ref long UserID, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserAdd(ref ds, SiteID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber, Address, Email, isEmailValided, QQ, IsQQValided, Telephone, Mobile, isMobileValided, isPrivacy, UserType, BankType, BankName, BankCardNumber, CommenderID, CpsID, AlipayName, Memo, VisitSource, ref UserID, ref ReturnDescription);
        }

        public static int P_UserAdd(ref DataSet ds, long SiteID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, short UserType, short BankType, string BankName, string BankCardNumber, long CommenderID, long CpsID, string AlipayName, string Memo, string VisitSource, ref long UserID, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("P_UserAdd", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("RealityName", SqlDbType.VarChar, 0, ParameterDirection.Input, RealityName),
                new MSSQL.Parameter("Password", SqlDbType.VarChar, 0, ParameterDirection.Input, Password),
                new MSSQL.Parameter("PasswordAdv", SqlDbType.VarChar, 0, ParameterDirection.Input, PasswordAdv),
                new MSSQL.Parameter("CityID", SqlDbType.Int, 0, ParameterDirection.Input, CityID),
                new MSSQL.Parameter("Sex", SqlDbType.VarChar, 0, ParameterDirection.Input, Sex),
                new MSSQL.Parameter("BirthDay", SqlDbType.DateTime, 0, ParameterDirection.Input, BirthDay),
                new MSSQL.Parameter("IDCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IDCardNumber),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("isEmailValided", SqlDbType.Bit, 0, ParameterDirection.Input, isEmailValided),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("IsQQValided", SqlDbType.Bit, 0, ParameterDirection.Input, IsQQValided),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, isPrivacy),
                new MSSQL.Parameter("UserType", SqlDbType.SmallInt, 0, ParameterDirection.Input, UserType),
                new MSSQL.Parameter("BankType", SqlDbType.SmallInt, 0, ParameterDirection.Input, BankType),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, BankCardNumber),
                new MSSQL.Parameter("CommenderID", SqlDbType.BigInt, 0, ParameterDirection.Input, CommenderID),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayName),
                new MSSQL.Parameter("Memo", SqlDbType.VarChar, 0, ParameterDirection.Input, Memo),
                new MSSQL.Parameter("VisitSource", SqlDbType.VarChar, 0, ParameterDirection.Input, VisitSource),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 8, ParameterDirection.Output, UserID),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                UserID = System.Convert.ToInt64(Outputs["UserID"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserAddSynchronism(long ISiteID, string IName, string IRealityName, string IPassword, string IPasswordAdv, int ICityID, string ISex, DateTime IBirthDay, string IIDCardNumber, string IAddress, string IEmail, bool IisEmailValided, string IQQ, bool IisQQValided, string ITelephone, string IMobile, bool IisMobileValided, bool IisPrivacy, short IUserType, short IBankType, string IBankName, string IBankCardNumber, long ICommenderID, long ICpsID, string IAlipayName, string IMemo, string IVisitSource, ref long IUserID, ref string IReturnDescription, bool isClub, string username, string nickname, string password, string secques, int gender, int adminid, short groupid, int groupexpiry, string extgroupids, string regip, string joindate, string lastip, DateTime lastvisit, DateTime lastactivity, DateTime lastpost, int lastpostid, string lastposttitle, int posts, short digestposts, int oltime, int pageviews, int credits, double extcredits1, double extcredits2, double extcredits3, double extcredits4, double extcredits5, double extcredits6, double extcredits7, double extcredits8, int avatarshowid, string email, string bday, int sigstatus, int tpp, int ppp, short templateid, int pmsound, int showemail, int newsletter, int invisible, int newpm, int accessmasks, string website, string icq, string qq, string yahoo, string msn, string skype, string location, string customstatus, string avatar, int avatarwidth, int avatarheight, string medals, string bio, string signature, string sightml, string authstr, string realname, string idcard, string mobile, string phone, long CpsID)
        {
            DataSet ds = null;

            return P_UserAddSynchronism(ref ds, ISiteID, IName, IRealityName, IPassword, IPasswordAdv, ICityID, ISex, IBirthDay, IIDCardNumber, IAddress, IEmail, IisEmailValided, IQQ, IisQQValided, ITelephone, IMobile, IisMobileValided, IisPrivacy, IUserType, IBankType, IBankName, IBankCardNumber, ICommenderID, ICpsID, IAlipayName, IMemo, IVisitSource, ref IUserID, ref IReturnDescription, isClub, username, nickname, password, secques, gender, adminid, groupid, groupexpiry, extgroupids, regip, joindate, lastip, lastvisit, lastactivity, lastpost, lastpostid, lastposttitle, posts, digestposts, oltime, pageviews, credits, extcredits1, extcredits2, extcredits3, extcredits4, extcredits5, extcredits6, extcredits7, extcredits8, avatarshowid, email, bday, sigstatus, tpp, ppp, templateid, pmsound, showemail, newsletter, invisible, newpm, accessmasks, website, icq, qq, yahoo, msn, skype, location, customstatus, avatar, avatarwidth, avatarheight, medals, bio, signature, sightml, authstr, realname, idcard, mobile, phone, CpsID);
        }

        public static int P_UserAddSynchronism(ref DataSet ds, long ISiteID, string IName, string IRealityName, string IPassword, string IPasswordAdv, int ICityID, string ISex, DateTime IBirthDay, string IIDCardNumber, string IAddress, string IEmail, bool IisEmailValided, string IQQ, bool IisQQValided, string ITelephone, string IMobile, bool IisMobileValided, bool IisPrivacy, short IUserType, short IBankType, string IBankName, string IBankCardNumber, long ICommenderID, long ICpsID, string IAlipayName, string IMemo, string IVisitSource, ref long IUserID, ref string IReturnDescription, bool isClub, string username, string nickname, string password, string secques, int gender, int adminid, short groupid, int groupexpiry, string extgroupids, string regip, string joindate, string lastip, DateTime lastvisit, DateTime lastactivity, DateTime lastpost, int lastpostid, string lastposttitle, int posts, short digestposts, int oltime, int pageviews, int credits, double extcredits1, double extcredits2, double extcredits3, double extcredits4, double extcredits5, double extcredits6, double extcredits7, double extcredits8, int avatarshowid, string email, string bday, int sigstatus, int tpp, int ppp, short templateid, int pmsound, int showemail, int newsletter, int invisible, int newpm, int accessmasks, string website, string icq, string qq, string yahoo, string msn, string skype, string location, string customstatus, string avatar, int avatarwidth, int avatarheight, string medals, string bio, string signature, string sightml, string authstr, string realname, string idcard, string mobile, string phone, long CpsID)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("P_UserAddSynchronism", ref ds, ref Outputs,
                new MSSQL.Parameter("ISiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, ISiteID),
                new MSSQL.Parameter("IName", SqlDbType.VarChar, 0, ParameterDirection.Input, IName),
                new MSSQL.Parameter("IRealityName", SqlDbType.VarChar, 0, ParameterDirection.Input, IRealityName),
                new MSSQL.Parameter("IPassword", SqlDbType.VarChar, 0, ParameterDirection.Input, IPassword),
                new MSSQL.Parameter("IPasswordAdv", SqlDbType.VarChar, 0, ParameterDirection.Input, IPasswordAdv),
                new MSSQL.Parameter("ICityID", SqlDbType.Int, 0, ParameterDirection.Input, ICityID),
                new MSSQL.Parameter("ISex", SqlDbType.VarChar, 0, ParameterDirection.Input, ISex),
                new MSSQL.Parameter("IBirthDay", SqlDbType.DateTime, 0, ParameterDirection.Input, IBirthDay),
                new MSSQL.Parameter("IIDCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IIDCardNumber),
                new MSSQL.Parameter("IAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IAddress),
                new MSSQL.Parameter("IEmail", SqlDbType.VarChar, 0, ParameterDirection.Input, IEmail),
                new MSSQL.Parameter("IisEmailValided", SqlDbType.Bit, 0, ParameterDirection.Input, IisEmailValided),
                new MSSQL.Parameter("IQQ", SqlDbType.VarChar, 0, ParameterDirection.Input, IQQ),
                new MSSQL.Parameter("IisQQValided", SqlDbType.Bit, 0, ParameterDirection.Input, IisQQValided),
                new MSSQL.Parameter("ITelephone", SqlDbType.VarChar, 0, ParameterDirection.Input, ITelephone),
                new MSSQL.Parameter("IMobile", SqlDbType.VarChar, 0, ParameterDirection.Input, IMobile),
                new MSSQL.Parameter("IisMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, IisMobileValided),
                new MSSQL.Parameter("IisPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, IisPrivacy),
                new MSSQL.Parameter("IUserType", SqlDbType.SmallInt, 0, ParameterDirection.Input, IUserType),
                new MSSQL.Parameter("IBankType", SqlDbType.SmallInt, 0, ParameterDirection.Input, IBankType),
                new MSSQL.Parameter("IBankName", SqlDbType.VarChar, 0, ParameterDirection.Input, IBankName),
                new MSSQL.Parameter("IBankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IBankCardNumber),
                new MSSQL.Parameter("ICommenderID", SqlDbType.BigInt, 0, ParameterDirection.Input, ICommenderID),
                new MSSQL.Parameter("ICpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, ICpsID),
                new MSSQL.Parameter("IAlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, IAlipayName),
                new MSSQL.Parameter("IMemo", SqlDbType.VarChar, 0, ParameterDirection.Input, IMemo),
                new MSSQL.Parameter("IVisitSource", SqlDbType.VarChar, 0, ParameterDirection.Input, IVisitSource),
                new MSSQL.Parameter("IUserID", SqlDbType.BigInt, 8, ParameterDirection.Output, IUserID),
                new MSSQL.Parameter("IReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, IReturnDescription),
                new MSSQL.Parameter("isClub", SqlDbType.Bit, 0, ParameterDirection.Input, isClub),
                new MSSQL.Parameter("username", SqlDbType.VarChar, 0, ParameterDirection.Input, username),
                new MSSQL.Parameter("nickname", SqlDbType.VarChar, 0, ParameterDirection.Input, nickname),
                new MSSQL.Parameter("password", SqlDbType.VarChar, 0, ParameterDirection.Input, password),
                new MSSQL.Parameter("secques", SqlDbType.VarChar, 0, ParameterDirection.Input, secques),
                new MSSQL.Parameter("gender", SqlDbType.Int, 0, ParameterDirection.Input, gender),
                new MSSQL.Parameter("adminid", SqlDbType.Int, 0, ParameterDirection.Input, adminid),
                new MSSQL.Parameter("groupid", SqlDbType.SmallInt, 0, ParameterDirection.Input, groupid),
                new MSSQL.Parameter("groupexpiry", SqlDbType.Int, 0, ParameterDirection.Input, groupexpiry),
                new MSSQL.Parameter("extgroupids", SqlDbType.VarChar, 0, ParameterDirection.Input, extgroupids),
                new MSSQL.Parameter("regip", SqlDbType.VarChar, 0, ParameterDirection.Input, regip),
                new MSSQL.Parameter("joindate", SqlDbType.VarChar, 0, ParameterDirection.Input, joindate),
                new MSSQL.Parameter("lastip", SqlDbType.VarChar, 0, ParameterDirection.Input, lastip),
                new MSSQL.Parameter("lastvisit", SqlDbType.DateTime, 0, ParameterDirection.Input, lastvisit),
                new MSSQL.Parameter("lastactivity", SqlDbType.DateTime, 0, ParameterDirection.Input, lastactivity),
                new MSSQL.Parameter("lastpost", SqlDbType.DateTime, 0, ParameterDirection.Input, lastpost),
                new MSSQL.Parameter("lastpostid", SqlDbType.Int, 0, ParameterDirection.Input, lastpostid),
                new MSSQL.Parameter("lastposttitle", SqlDbType.VarChar, 0, ParameterDirection.Input, lastposttitle),
                new MSSQL.Parameter("posts", SqlDbType.Int, 0, ParameterDirection.Input, posts),
                new MSSQL.Parameter("digestposts", SqlDbType.SmallInt, 0, ParameterDirection.Input, digestposts),
                new MSSQL.Parameter("oltime", SqlDbType.Int, 0, ParameterDirection.Input, oltime),
                new MSSQL.Parameter("pageviews", SqlDbType.Int, 0, ParameterDirection.Input, pageviews),
                new MSSQL.Parameter("credits", SqlDbType.Int, 0, ParameterDirection.Input, credits),
                new MSSQL.Parameter("extcredits1", SqlDbType.Float, 0, ParameterDirection.Input, extcredits1),
                new MSSQL.Parameter("extcredits2", SqlDbType.Float, 0, ParameterDirection.Input, extcredits2),
                new MSSQL.Parameter("extcredits3", SqlDbType.Float, 0, ParameterDirection.Input, extcredits3),
                new MSSQL.Parameter("extcredits4", SqlDbType.Float, 0, ParameterDirection.Input, extcredits4),
                new MSSQL.Parameter("extcredits5", SqlDbType.Float, 0, ParameterDirection.Input, extcredits5),
                new MSSQL.Parameter("extcredits6", SqlDbType.Float, 0, ParameterDirection.Input, extcredits6),
                new MSSQL.Parameter("extcredits7", SqlDbType.Float, 0, ParameterDirection.Input, extcredits7),
                new MSSQL.Parameter("extcredits8", SqlDbType.Float, 0, ParameterDirection.Input, extcredits8),
                new MSSQL.Parameter("avatarshowid", SqlDbType.Int, 0, ParameterDirection.Input, avatarshowid),
                new MSSQL.Parameter("email", SqlDbType.VarChar, 0, ParameterDirection.Input, email),
                new MSSQL.Parameter("bday", SqlDbType.VarChar, 0, ParameterDirection.Input, bday),
                new MSSQL.Parameter("sigstatus", SqlDbType.Int, 0, ParameterDirection.Input, sigstatus),
                new MSSQL.Parameter("tpp", SqlDbType.Int, 0, ParameterDirection.Input, tpp),
                new MSSQL.Parameter("ppp", SqlDbType.Int, 0, ParameterDirection.Input, ppp),
                new MSSQL.Parameter("templateid", SqlDbType.SmallInt, 0, ParameterDirection.Input, templateid),
                new MSSQL.Parameter("pmsound", SqlDbType.Int, 0, ParameterDirection.Input, pmsound),
                new MSSQL.Parameter("showemail", SqlDbType.Int, 0, ParameterDirection.Input, showemail),
                new MSSQL.Parameter("newsletter", SqlDbType.Int, 0, ParameterDirection.Input, newsletter),
                new MSSQL.Parameter("invisible", SqlDbType.Int, 0, ParameterDirection.Input, invisible),
                new MSSQL.Parameter("newpm", SqlDbType.Int, 0, ParameterDirection.Input, newpm),
                new MSSQL.Parameter("accessmasks", SqlDbType.Int, 0, ParameterDirection.Input, accessmasks),
                new MSSQL.Parameter("website", SqlDbType.VarChar, 0, ParameterDirection.Input, website),
                new MSSQL.Parameter("icq", SqlDbType.VarChar, 0, ParameterDirection.Input, icq),
                new MSSQL.Parameter("qq", SqlDbType.VarChar, 0, ParameterDirection.Input, qq),
                new MSSQL.Parameter("yahoo", SqlDbType.VarChar, 0, ParameterDirection.Input, yahoo),
                new MSSQL.Parameter("msn", SqlDbType.VarChar, 0, ParameterDirection.Input, msn),
                new MSSQL.Parameter("skype", SqlDbType.VarChar, 0, ParameterDirection.Input, skype),
                new MSSQL.Parameter("location", SqlDbType.VarChar, 0, ParameterDirection.Input, location),
                new MSSQL.Parameter("customstatus", SqlDbType.VarChar, 0, ParameterDirection.Input, customstatus),
                new MSSQL.Parameter("avatar", SqlDbType.VarChar, 0, ParameterDirection.Input, avatar),
                new MSSQL.Parameter("avatarwidth", SqlDbType.Int, 0, ParameterDirection.Input, avatarwidth),
                new MSSQL.Parameter("avatarheight", SqlDbType.Int, 0, ParameterDirection.Input, avatarheight),
                new MSSQL.Parameter("medals", SqlDbType.VarChar, 0, ParameterDirection.Input, medals),
                new MSSQL.Parameter("bio", SqlDbType.VarChar, 0, ParameterDirection.Input, bio),
                new MSSQL.Parameter("signature", SqlDbType.VarChar, 0, ParameterDirection.Input, signature),
                new MSSQL.Parameter("sightml", SqlDbType.VarChar, 0, ParameterDirection.Input, sightml),
                new MSSQL.Parameter("authstr", SqlDbType.VarChar, 0, ParameterDirection.Input, authstr),
                new MSSQL.Parameter("realname", SqlDbType.VarChar, 0, ParameterDirection.Input, realname),
                new MSSQL.Parameter("idcard", SqlDbType.VarChar, 0, ParameterDirection.Input, idcard),
                new MSSQL.Parameter("mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, mobile),
                new MSSQL.Parameter("phone", SqlDbType.VarChar, 0, ParameterDirection.Input, phone),
                new MSSQL.Parameter("CpsID", SqlDbType.BigInt, 0, ParameterDirection.Input, CpsID)
                );

            try
            {
                IUserID = System.Convert.ToInt64(Outputs["IUserID"]);
            }
            catch { }

            try
            {
                IReturnDescription = System.Convert.ToString(Outputs["IReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserEditByID(long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserEditByID(ref ds, SiteID, UserID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber, Address, Email, isEmailValided, QQ, IsQQValided, Telephone, Mobile, isMobileValided, isPrivacy, isCanLogin, UserType, BankType, BankName, BankCardNumber, ScoringOfSelfBuy, ScoringOfCommendBuy, Level, AlipayID, AlipayName, isAlipayNameValided, PromotionMemberBonusScale, PromotionSiteBonusScale, IsCrossLogin, Reason, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserEditByID(ref DataSet ds, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("P_UserEditByID", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("RealityName", SqlDbType.VarChar, 0, ParameterDirection.Input, RealityName),
                new MSSQL.Parameter("Password", SqlDbType.VarChar, 0, ParameterDirection.Input, Password),
                new MSSQL.Parameter("PasswordAdv", SqlDbType.VarChar, 0, ParameterDirection.Input, PasswordAdv),
                new MSSQL.Parameter("CityID", SqlDbType.Int, 0, ParameterDirection.Input, CityID),
                new MSSQL.Parameter("Sex", SqlDbType.VarChar, 0, ParameterDirection.Input, Sex),
                new MSSQL.Parameter("BirthDay", SqlDbType.DateTime, 0, ParameterDirection.Input, BirthDay),
                new MSSQL.Parameter("IDCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IDCardNumber),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("isEmailValided", SqlDbType.Bit, 0, ParameterDirection.Input, isEmailValided),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("IsQQValided", SqlDbType.Bit, 0, ParameterDirection.Input, IsQQValided),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 0, ParameterDirection.Input, isCanLogin),
                new MSSQL.Parameter("UserType", SqlDbType.SmallInt, 0, ParameterDirection.Input, UserType),
                new MSSQL.Parameter("BankType", SqlDbType.SmallInt, 0, ParameterDirection.Input, BankType),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, BankCardNumber),
                new MSSQL.Parameter("ScoringOfSelfBuy", SqlDbType.Float, 0, ParameterDirection.Input, ScoringOfSelfBuy),
                new MSSQL.Parameter("ScoringOfCommendBuy", SqlDbType.Float, 0, ParameterDirection.Input, ScoringOfCommendBuy),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 0, ParameterDirection.Input, Level),
                new MSSQL.Parameter("AlipayID", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayID),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayName),
                new MSSQL.Parameter("isAlipayNameValided", SqlDbType.Bit, 0, ParameterDirection.Input, isAlipayNameValided),
                new MSSQL.Parameter("PromotionMemberBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, PromotionMemberBonusScale),
                new MSSQL.Parameter("PromotionSiteBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, PromotionSiteBonusScale),
                new MSSQL.Parameter("IsCrossLogin", SqlDbType.Bit, 0, ParameterDirection.Input, IsCrossLogin),
                new MSSQL.Parameter("Reason", SqlDbType.VarChar, 0, ParameterDirection.Input, Reason),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserEditByIDSynchronism(long ISiteID, long IUserID, string IName, string IRealityName, string IPassword, string IPasswordAdv, int ICityID, string ISex, DateTime IBirthDay, string IIDCardNumber, string IAddress, string IEmail, bool IisEmailValided, string IQQ, bool IisQQValided, string ITelephone, string IMobile, bool IisMobileValided, bool IisPrivacy, bool IisCanLogin, short IUserType, short IBankType, string IBankName, string IBankCardNumber, double IScoringOfSelfBuy, double IScoringOfCommendBuy, short ILevel, string IAlipayID, string IAlipayName, bool IisAlipayNameValided, double IPromotionMemberBonusScale, double IPromotionSiteBonusScale, bool IsCrossLogin, string IReason, ref int IReturnValue, ref string IReturnDescription, bool isClub, int uid, string website, string nickname, int gender, string bday, string icq, string qq, string yahoo, string msn, string skype, string location, string bio, string idcard, string mobile, string phone)
        {
            DataSet ds = null;

            return P_UserEditByIDSynchronism(ref ds, ISiteID, IUserID, IName, IRealityName, IPassword, IPasswordAdv, ICityID, ISex, IBirthDay, IIDCardNumber, IAddress, IEmail, IisEmailValided, IQQ, IisQQValided, ITelephone, IMobile, IisMobileValided, IisPrivacy, IisCanLogin, IUserType, IBankType, IBankName, IBankCardNumber, IScoringOfSelfBuy, IScoringOfCommendBuy, ILevel, IAlipayID, IAlipayName, IisAlipayNameValided, IPromotionMemberBonusScale, IPromotionSiteBonusScale, IsCrossLogin, IReason, ref IReturnValue, ref IReturnDescription, isClub, uid, website, nickname, gender, bday, icq, qq, yahoo, msn, skype, location, bio, idcard, mobile, phone);
        }

        public static int P_UserEditByIDSynchronism(ref DataSet ds, long ISiteID, long IUserID, string IName, string IRealityName, string IPassword, string IPasswordAdv, int ICityID, string ISex, DateTime IBirthDay, string IIDCardNumber, string IAddress, string IEmail, bool IisEmailValided, string IQQ, bool IisQQValided, string ITelephone, string IMobile, bool IisMobileValided, bool IisPrivacy, bool IisCanLogin, short IUserType, short IBankType, string IBankName, string IBankCardNumber, double IScoringOfSelfBuy, double IScoringOfCommendBuy, short ILevel, string IAlipayID, string IAlipayName, bool IisAlipayNameValided, double IPromotionMemberBonusScale, double IPromotionSiteBonusScale, bool IsCrossLogin, string IReason, ref int IReturnValue, ref string IReturnDescription, bool isClub, int uid, string website, string nickname, int gender, string bday, string icq, string qq, string yahoo, string msn, string skype, string location, string bio, string idcard, string mobile, string phone)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("P_UserEditByIDSynchronism", ref ds, ref Outputs,
                new MSSQL.Parameter("ISiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, ISiteID),
                new MSSQL.Parameter("IUserID", SqlDbType.BigInt, 0, ParameterDirection.Input, IUserID),
                new MSSQL.Parameter("IName", SqlDbType.VarChar, 0, ParameterDirection.Input, IName),
                new MSSQL.Parameter("IRealityName", SqlDbType.VarChar, 0, ParameterDirection.Input, IRealityName),
                new MSSQL.Parameter("IPassword", SqlDbType.VarChar, 0, ParameterDirection.Input, IPassword),
                new MSSQL.Parameter("IPasswordAdv", SqlDbType.VarChar, 0, ParameterDirection.Input, IPasswordAdv),
                new MSSQL.Parameter("ICityID", SqlDbType.Int, 0, ParameterDirection.Input, ICityID),
                new MSSQL.Parameter("ISex", SqlDbType.VarChar, 0, ParameterDirection.Input, ISex),
                new MSSQL.Parameter("IBirthDay", SqlDbType.DateTime, 0, ParameterDirection.Input, IBirthDay),
                new MSSQL.Parameter("IIDCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IIDCardNumber),
                new MSSQL.Parameter("IAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, IAddress),
                new MSSQL.Parameter("IEmail", SqlDbType.VarChar, 0, ParameterDirection.Input, IEmail),
                new MSSQL.Parameter("IisEmailValided", SqlDbType.Bit, 0, ParameterDirection.Input, IisEmailValided),
                new MSSQL.Parameter("IQQ", SqlDbType.VarChar, 0, ParameterDirection.Input, IQQ),
                new MSSQL.Parameter("IisQQValided", SqlDbType.Bit, 0, ParameterDirection.Input, IisQQValided),
                new MSSQL.Parameter("ITelephone", SqlDbType.VarChar, 0, ParameterDirection.Input, ITelephone),
                new MSSQL.Parameter("IMobile", SqlDbType.VarChar, 0, ParameterDirection.Input, IMobile),
                new MSSQL.Parameter("IisMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, IisMobileValided),
                new MSSQL.Parameter("IisPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, IisPrivacy),
                new MSSQL.Parameter("IisCanLogin", SqlDbType.Bit, 0, ParameterDirection.Input, IisCanLogin),
                new MSSQL.Parameter("IUserType", SqlDbType.SmallInt, 0, ParameterDirection.Input, IUserType),
                new MSSQL.Parameter("IBankType", SqlDbType.SmallInt, 0, ParameterDirection.Input, IBankType),
                new MSSQL.Parameter("IBankName", SqlDbType.VarChar, 0, ParameterDirection.Input, IBankName),
                new MSSQL.Parameter("IBankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IBankCardNumber),
                new MSSQL.Parameter("IScoringOfSelfBuy", SqlDbType.Float, 0, ParameterDirection.Input, IScoringOfSelfBuy),
                new MSSQL.Parameter("IScoringOfCommendBuy", SqlDbType.Float, 0, ParameterDirection.Input, IScoringOfCommendBuy),
                new MSSQL.Parameter("ILevel", SqlDbType.SmallInt, 0, ParameterDirection.Input, ILevel),
                new MSSQL.Parameter("IAlipayID", SqlDbType.VarChar, 0, ParameterDirection.Input, IAlipayID),
                new MSSQL.Parameter("IAlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, IAlipayName),
                new MSSQL.Parameter("IisAlipayNameValided", SqlDbType.Bit, 0, ParameterDirection.Input, IisAlipayNameValided),
                new MSSQL.Parameter("IPromotionMemberBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, IPromotionMemberBonusScale),
                new MSSQL.Parameter("IPromotionSiteBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, IPromotionSiteBonusScale),
                new MSSQL.Parameter("IsCrossLogin", SqlDbType.Bit, 0, ParameterDirection.Input, IsCrossLogin),
                new MSSQL.Parameter("IReason", SqlDbType.VarChar, 0, ParameterDirection.Input, IReason),
                new MSSQL.Parameter("IReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, IReturnValue),
                new MSSQL.Parameter("IReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, IReturnDescription),
                new MSSQL.Parameter("isClub", SqlDbType.Bit, 0, ParameterDirection.Input, isClub),
                new MSSQL.Parameter("uid", SqlDbType.Int, 0, ParameterDirection.Input, uid),
                new MSSQL.Parameter("website", SqlDbType.NVarChar, 0, ParameterDirection.Input, website),
                new MSSQL.Parameter("nickname", SqlDbType.NChar, 0, ParameterDirection.Input, nickname),
                new MSSQL.Parameter("gender", SqlDbType.Int, 0, ParameterDirection.Input, gender),
                new MSSQL.Parameter("bday", SqlDbType.Char, 0, ParameterDirection.Input, bday),
                new MSSQL.Parameter("icq", SqlDbType.VarChar, 0, ParameterDirection.Input, icq),
                new MSSQL.Parameter("qq", SqlDbType.VarChar, 0, ParameterDirection.Input, qq),
                new MSSQL.Parameter("yahoo", SqlDbType.VarChar, 0, ParameterDirection.Input, yahoo),
                new MSSQL.Parameter("msn", SqlDbType.VarChar, 0, ParameterDirection.Input, msn),
                new MSSQL.Parameter("skype", SqlDbType.VarChar, 0, ParameterDirection.Input, skype),
                new MSSQL.Parameter("location", SqlDbType.NVarChar, 0, ParameterDirection.Input, location),
                new MSSQL.Parameter("bio", SqlDbType.NVarChar, 0, ParameterDirection.Input, bio),
                new MSSQL.Parameter("idcard", SqlDbType.VarChar, 0, ParameterDirection.Input, idcard),
                new MSSQL.Parameter("mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, mobile),
                new MSSQL.Parameter("phone", SqlDbType.VarChar, 0, ParameterDirection.Input, phone)
                );

            try
            {
                IReturnValue = System.Convert.ToInt32(Outputs["IReturnValue"]);
            }
            catch { }

            try
            {
                IReturnDescription = System.Convert.ToString(Outputs["IReturnDescription"]);
            }
            catch { }

            return CallResult;
        }

        public static int P_UserEditByName(long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            DataSet ds = null;

            return P_UserEditByName(ref ds, SiteID, UserID, Name, RealityName, Password, PasswordAdv, CityID, Sex, BirthDay, IDCardNumber, Address, Email, isEmailValided, QQ, IsQQValided, Telephone, Mobile, isMobileValided, isPrivacy, isCanLogin, UserType, BankType, BankName, BankCardNumber, ScoringOfSelfBuy, ScoringOfCommendBuy, Level, AlipayID, AlipayName, isAlipayNameValided, PromotionMemberBonusScale, PromotionSiteBonusScale, IsCrossLogin, Reason, ref ReturnValue, ref ReturnDescription);
        }

        public static int P_UserEditByName(ref DataSet ds, long SiteID, long UserID, string Name, string RealityName, string Password, string PasswordAdv, int CityID, string Sex, DateTime BirthDay, string IDCardNumber, string Address, string Email, bool isEmailValided, string QQ, bool IsQQValided, string Telephone, string Mobile, bool isMobileValided, bool isPrivacy, bool isCanLogin, short UserType, short BankType, string BankName, string BankCardNumber, double ScoringOfSelfBuy, double ScoringOfCommendBuy, short Level, string AlipayID, string AlipayName, bool isAlipayNameValided, double PromotionMemberBonusScale, double PromotionSiteBonusScale, bool IsCrossLogin, string Reason, ref int ReturnValue, ref string ReturnDescription)
        {
            MSSQL.OutputParameter Outputs = new MSSQL.OutputParameter();

            int CallResult = MSSQL.ExecuteStoredProcedureWithQuery("P_UserEditByName", ref ds, ref Outputs,
                new MSSQL.Parameter("SiteID", SqlDbType.BigInt, 0, ParameterDirection.Input, SiteID),
                new MSSQL.Parameter("UserID", SqlDbType.BigInt, 0, ParameterDirection.Input, UserID),
                new MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, Name),
                new MSSQL.Parameter("RealityName", SqlDbType.VarChar, 0, ParameterDirection.Input, RealityName),
                new MSSQL.Parameter("Password", SqlDbType.VarChar, 0, ParameterDirection.Input, Password),
                new MSSQL.Parameter("PasswordAdv", SqlDbType.VarChar, 0, ParameterDirection.Input, PasswordAdv),
                new MSSQL.Parameter("CityID", SqlDbType.Int, 0, ParameterDirection.Input, CityID),
                new MSSQL.Parameter("Sex", SqlDbType.VarChar, 0, ParameterDirection.Input, Sex),
                new MSSQL.Parameter("BirthDay", SqlDbType.DateTime, 0, ParameterDirection.Input, BirthDay),
                new MSSQL.Parameter("IDCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, IDCardNumber),
                new MSSQL.Parameter("Address", SqlDbType.VarChar, 0, ParameterDirection.Input, Address),
                new MSSQL.Parameter("Email", SqlDbType.VarChar, 0, ParameterDirection.Input, Email),
                new MSSQL.Parameter("isEmailValided", SqlDbType.Bit, 0, ParameterDirection.Input, isEmailValided),
                new MSSQL.Parameter("QQ", SqlDbType.VarChar, 0, ParameterDirection.Input, QQ),
                new MSSQL.Parameter("IsQQValided", SqlDbType.Bit, 0, ParameterDirection.Input, IsQQValided),
                new MSSQL.Parameter("Telephone", SqlDbType.VarChar, 0, ParameterDirection.Input, Telephone),
                new MSSQL.Parameter("Mobile", SqlDbType.VarChar, 0, ParameterDirection.Input, Mobile),
                new MSSQL.Parameter("isMobileValided", SqlDbType.Bit, 0, ParameterDirection.Input, isMobileValided),
                new MSSQL.Parameter("isPrivacy", SqlDbType.Bit, 0, ParameterDirection.Input, isPrivacy),
                new MSSQL.Parameter("isCanLogin", SqlDbType.Bit, 0, ParameterDirection.Input, isCanLogin),
                new MSSQL.Parameter("UserType", SqlDbType.SmallInt, 0, ParameterDirection.Input, UserType),
                new MSSQL.Parameter("BankType", SqlDbType.SmallInt, 0, ParameterDirection.Input, BankType),
                new MSSQL.Parameter("BankName", SqlDbType.VarChar, 0, ParameterDirection.Input, BankName),
                new MSSQL.Parameter("BankCardNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, BankCardNumber),
                new MSSQL.Parameter("ScoringOfSelfBuy", SqlDbType.Float, 0, ParameterDirection.Input, ScoringOfSelfBuy),
                new MSSQL.Parameter("ScoringOfCommendBuy", SqlDbType.Float, 0, ParameterDirection.Input, ScoringOfCommendBuy),
                new MSSQL.Parameter("Level", SqlDbType.SmallInt, 0, ParameterDirection.Input, Level),
                new MSSQL.Parameter("AlipayID", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayID),
                new MSSQL.Parameter("AlipayName", SqlDbType.VarChar, 0, ParameterDirection.Input, AlipayName),
                new MSSQL.Parameter("isAlipayNameValided", SqlDbType.Bit, 0, ParameterDirection.Input, isAlipayNameValided),
                new MSSQL.Parameter("PromotionMemberBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, PromotionMemberBonusScale),
                new MSSQL.Parameter("PromotionSiteBonusScale", SqlDbType.Float, 0, ParameterDirection.Input, PromotionSiteBonusScale),
                new MSSQL.Parameter("IsCrossLogin", SqlDbType.Bit, 0, ParameterDirection.Input, IsCrossLogin),
                new MSSQL.Parameter("Reason", SqlDbType.VarChar, 0, ParameterDirection.Input, Reason),
                new MSSQL.Parameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.Output, ReturnValue),
                new MSSQL.Parameter("ReturnDescription", SqlDbType.VarChar, 100, ParameterDirection.Output, ReturnDescription)
                );

            try
            {
                ReturnValue = System.Convert.ToInt32(Outputs["ReturnValue"]);
            }
            catch { }

            try
            {
                ReturnDescription = System.Convert.ToString(Outputs["ReturnDescription"]);
            }
            catch { }

            return CallResult;
        }
    }
}
