using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;
using System.Windows.Browser;
using System.Json;
using SLS.SilverLight.FilterShrink.Model;

namespace SLS.SilverLight.FilterShrink.ModelChildWindow
{
    public partial class login : ChildWindow
    {
        string Message = "";

        public login()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string Name = "";
            string PassWord = "";

            if (string.IsNullOrEmpty(tbName.Text))
            {
                lbMessage.Content = "用户名不能为空！";

                return;
            }

            Name = tbName.Text.Trim();

            if (string.IsNullOrEmpty(tbPwd.Password))
            {
                lbMessage.Content = "密码不能为空！";

                return;
            }

            PassWord = tbPwd.Password.Trim();

            short Result = Login(Name, PassWord);
        }

        public short Login(string Name, string Password)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return -2;
            }

            if (string.IsNullOrEmpty(Password))
            {
                return -3;
            }

            Uri endpoint = new Uri(Application.Current.Host.Source, "/ajax/UserLogin.ashx?UserName=" + Name + "&Password=" + Password);

            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(endpoint);

            return 0;
        }

        public short CheckLogin()
        {
            Uri endpoint = new Uri(Application.Current.Host.Source, "/ajax/UserLogin.ashx");

            WebClient client = new WebClient();
            client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(endpoint);

            return 0;
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                HtmlElement input = HtmlPage.Document.GetElementById("hinUserID");
                JsonObject jsonObject = (JsonObject)JsonObject.Load(e.Result);

                long userid;

                try
                {
                    userid = Convert.ToInt64(jsonObject["message"].ToString().Replace("\"", ""));
                }
                catch
                {
                    lbMessage.Content = jsonObject["message"].ToString();

                    userid = -1;
                }

                if (userid > 0)
                {
                    this.Close();
                }

                input.SetAttribute("value", userid.ToString());

                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }
        }
    }
}

