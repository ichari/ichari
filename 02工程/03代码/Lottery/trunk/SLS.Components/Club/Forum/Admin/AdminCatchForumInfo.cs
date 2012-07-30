using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System;
using System.Reflection;

using Discuz.Forum;
using Discuz.Config;

namespace Discuz.Forum
{
	
	

	/// <summary>
	/// 此类为引用webservice时系统自动生成
	/// </summary>
	[DebuggerStepThrough()]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="CatchSoftInfoSoap", Namespace="http://tempuri.org/")]
	public class CatchSoftInfo : SoapHttpClientProtocol
	{
		public AuthHeaderCS AuthHeaderCSValue;

		/// <remarks/>
		public CatchSoftInfo()
		{
			this.Url = "http://nt.discuz.net/DiscuzWebService/CatchForumInfo.asmx";
            //this.Url = "http://localhost:87/CatchForumInfo.asmx";

		}

		/// <remarks/>
		[SoapHeader("AuthHeaderCSValue")]
		[SoapDocumentMethod("http://tempuri.org/SetupSoftInfo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void SetupSoftInfo(string bbname, int member, int topics, int posts, string serversoft, int dotnetver, int dbtype, int major, int minor, int revision, int build, string osversion, string serverip, string servername)
		{
			this.Invoke("SetupSoftInfo", new object[]
				{
					bbname,
					member,
					topics,
					posts,
					serversoft,
					dotnetver,
					dbtype,
					major,
					minor,
					revision,
					build,
					osversion,
					serverip,
					servername
				});
		}

		/// <remarks/>
		public IAsyncResult BeginSetupSoftInfo(
			string bbname,
			int member,
			int topics,
			int posts,
			string serversoft,
			int dotnetver,
			int dbtype,
			int major,
			int minor,
			int revision,
			int build,
			string osversion,
			string serverip,
			string servername,
			AsyncCallback callback,
			object asyncState)
		{
			return this.BeginInvoke("SetupSoftInfo", new object[]
				{
					bbname,
					member,
					topics,
					posts,
					serversoft,
					dotnetver,
					dbtype,
					major,
					minor,
					revision,
					build,
					osversion,
					serverip,
					servername
				}, callback, asyncState);
		}

		/// <remarks/>
		public void EndSetupSoftInfo(IAsyncResult asyncResult)
		{
			this.EndInvoke(asyncResult);
		}

		/// <remarks/>
		[SoapHeader("AuthHeaderCSValue")]
		[SoapDocumentMethod("http://tempuri.org/InsertSoftInfo", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void InsertSoftInfo(string bbname, int member, int topics, int posts, string serversoft, int major, int minor, int revision, int build, string osversion, string serverip, string servername)
		{
			this.Invoke("InsertSoftInfo", new object[]
				{
					bbname,
					member,
					topics,
					posts,
					serversoft,
					major,
					minor,
					revision,
					build,
					osversion,
					serverip,
					servername
				});
		}

		/// <remarks/>
		public IAsyncResult BeginInsertSoftInfo(string bbname, int member, int topics, int posts, string serversoft, int major, int minor, int revision, int build, string osversion, string serverip, string servername, AsyncCallback callback, object asyncState)
		{
			return this.BeginInvoke("InsertSoftInfo", new object[]
				{
					bbname,
					member,
					topics,
					posts,
					serversoft,
					major,
					minor,
					revision,
					build,
					osversion,
					serverip,
					servername
				}, callback, asyncState);
		}

		/// <remarks/>
		public void EndInsertSoftInfo(IAsyncResult asyncResult)
		{
			this.EndInvoke(asyncResult);
		}

		/// <remarks/>
		[SoapDocumentMethod("http://tempuri.org/HelloDiscuzCustomer", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string HelloDiscuzCustomer()
		{
			object[] results = this.Invoke("HelloDiscuzCustomer", new object[0]);
			return ((string) (results[0]));
		}

		/// <remarks/>
		public IAsyncResult BeginHelloDiscuzCustomer(AsyncCallback callback, object asyncState)
		{
			return this.BeginInvoke("HelloDiscuzCustomer", new object[0], callback, asyncState);
		}

		/// <remarks/>
		public string EndHelloDiscuzCustomer(IAsyncResult asyncResult)
		{
			object[] results = this.EndInvoke(asyncResult);
			return ((string) (results[0]));
		}
	}

	/// <remarks/>
	[XmlType(Namespace="http://tempuri.org/")]
	[XmlRoot(Namespace="http://tempuri.org/", IsNullable=false)]
	public class AuthHeaderCS : SoapHeader
	{
		/// <remarks/>
		public string Username;

		/// <remarks/>
		public string Password;

		/// <remarks/>
		public string HttpLink;
	}


	/// <summary>
	/// 系统信息类
	/// </summary>
	public class SoftInfo
	{
		public static string serversoftware = null;
		public static string serverip = null;
		public static string servername = null;
		public static string bbname = null;

		static SoftInfo()
		{
			if (serversoftware == null)
			{
				//获取服务器及网站相关信息
				serversoftware = HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
				serverip = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
				servername = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                bbname = GeneralConfigs.GetConfig().Forumtitle;
			}
		}


		//异步执行并加载系统信息
		public static void LoadSoftInfo()
		{
			aysncallback = new delegateLoadSoftInfo(SendPostData);
			AsyncCallback myCallBack = new AsyncCallback(CallBack);
			aysncallback.BeginInvoke(myCallBack, ""); //
		}

		private delegate void delegateLoadSoftInfo();

		//异步建立索引并进行填充的代理
		private static delegateLoadSoftInfo aysncallback;

		public static void CallBack(IAsyncResult e)
		{
			aysncallback.EndInvoke(e);
		}


		//发送数据
		public static void SendPostData()
		{
			try
			{
				Thread.Sleep(50000);
                //Thread.Sleep(1000);


				//得到系统相关信息
				int member = Convert.ToInt32(Statistics.GetStatisticsRowItem("totalusers"));
				int topcis = Convert.ToInt32(Statistics.GetStatisticsRowItem("totaltopic"));
				int posts = Convert.ToInt32(Statistics.GetStatisticsRowItem("totalpost"));

                int build = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileBuildPart; //Environment.Version.Build;
				string osversion = Environment.OSVersion.ToString();
				int major = Environment.Version.Major;
				int minor = Environment.Version.Minor;
				int revision = Environment.Version.Revision;

				CatchSoftInfo csi = new CatchSoftInfo();

				//产生webservice的认证信息
				AuthHeaderCS myHeader = new AuthHeaderCS();
				myHeader.Username = "Z0TZVFB406";
				myHeader.Password = "QWERTYUIOP";
				csi.AuthHeaderCSValue = myHeader;
                //csi.InsertSoftInfo(bbname, member, topcis, posts, serversoftware, major, minor, revision, build, osversion, serverip, servername);
				int dbtype = 0;
				switch(Discuz.Config.BaseConfigs.GetDbType.ToLower())
				{
					case "sqlserver":
					{
						dbtype = 0;
						break;
					}
					case "access":
					{
						dbtype = 101;
						break;
					}
					case "mysql":
					{
						dbtype = 201;
						break;
					}
				}


			    int productmarjorpart = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductMajorPart;
                csi.SetupSoftInfo(bbname, member, topcis, posts, serversoftware, productmarjorpart, dbtype, major, minor, revision, build, osversion, serverip, servername);

			}
			catch
			{
				;
			}
			finally
			{
				Thread.CurrentThread.Abort();
			}
		}



	}
}