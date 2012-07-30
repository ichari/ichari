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
using SLS.SilverLight.FilterShrink.ServiceReference1;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.ServiceModel;
using SLS.SilverLight.FilterShrink.Model;
using System.Windows.Data;
using System.Windows.Browser;
using SLS.SilverLight.FilterShrink.ServiceModel;

namespace SLS.SilverLight.FilterShrink
{
    public partial class BF : UserControl
    {
        #region 全局变量
        string[] GameNumber = null;//得到场次信息id和胜平负信息（全局变量）
        string BuyWays = "";
        int Mulitpe = 1;
        long UserID = 0;

        List<Netball> NetBallList = new List<Netball>();//起始页面加载在datagrid的数据集合

        static List<string> ListResult = new List<string>();//(处理后的全部数据)全局变量
        List<LotteryNum> Num = new List<LotteryNum>();  //过滤之后的数据
        DispatcherTimer Timer;

        #region 子窗体
        BFChildWindow.bfgatherfilter GatherFilter = new SLS.SilverLight.FilterShrink.BFChildWindow.bfgatherfilter();

        BFChildWindow.bfScoreCount ScoreCount = new SLS.SilverLight.FilterShrink.BFChildWindow.bfScoreCount();

        BFChildWindow.bfgrounpfilter GrounpFilter = new SLS.SilverLight.FilterShrink.BFChildWindow.bfgrounpfilter();


        ModelChildWindow.SaveChildWindow saveMobile = new SLS.SilverLight.FilterShrink.ModelChildWindow.SaveChildWindow();
        ModelChildWindow.ModelManage modelManage = new SLS.SilverLight.FilterShrink.ModelChildWindow.ModelManage();
        ModelChildWindow.ModelShow modelShow = new SLS.SilverLight.FilterShrink.ModelChildWindow.ModelShow();
        ModelChildWindow.login login = new SLS.SilverLight.FilterShrink.ModelChildWindow.login();

        #endregion

        #endregion

        public BF()
        {
            InitializeComponent();

            #region 条件篮的主导航
            btnRoutine.Click += new RoutedEventHandler(btnRoutine_Click);
            btnGroup.Click += new RoutedEventHandler(btnRoutine_Click);
            btnGather.Click += new RoutedEventHandler(btnRoutine_Click);
            btnIndex.Click += new RoutedEventHandler(btnRoutine_Click);
            btnOrder.Click += new RoutedEventHandler(btnRoutine_Click);
            btnRange.Click += new RoutedEventHandler(btnRoutine_Click);
            btnScore.Click += new RoutedEventHandler(btnRoutine_Click);
            btnDifference.Click += new RoutedEventHandler(btnRoutine_Click);
            #endregion

            #region 条件篮的全删按钮
            btnDelAllCondition.Click += new RoutedEventHandler(btnDelAllCondition_Click);
            #endregion

            //子窗体关闭后事件
            GatherFilter.Closed += new EventHandler(GatherFilter_Closed);
            ScoreCount.Closed += new EventHandler(ScoreCount_Closed);
            GrounpFilter.Closed += new EventHandler(GrounpFilter_Closed);

            modelManage.Closed += new EventHandler(modelManage_Closed);
            modelShow.Closed += new EventHandler(modelShow_Closed);
            login.Closed += new EventHandler(login_Closed);

            try
            {
                HtmlElement input = HtmlPage.Document.GetElementById("hinUserID");

                UserID = long.Parse(input.GetAttribute("value"));
            }
            catch
            {
                UserID = -1;
            }

            //隐藏datagrid的表头
            this.DataGrid1.HeadersVisibility = DataGridHeadersVisibility.None;

            //加载球赛信息 “场次，主场球队，客场球队，让球，赔率等”，加载到datagrid
            BindNetBall();
        }

        //datagrid初始化
        void BindNetBall()
        {
            string Number = "";

            try
            {
                HtmlElement input = HtmlPage.Document.GetElementById("Number");

                Number = input.GetAttribute("value");
            }
            catch { }

            if (string.IsNullOrEmpty(Number))
            {
                MessageBox.Show("参数错误，请重新发起请求！");

                return;
            }

            int SchemeLength = Number.Split(';').Length;
            if (SchemeLength != 3)
            {
                return;
            }

            //玩法类型
            string strPlayType = Number.Split(';')[0].ToString();

            //场次信息id字符串,带[]包起来的
            string BuyNumber = Number.Split(';')[1].ToString();

            //去除[]
            string Numbers = BuyNumber.Substring(1, BuyNumber.Length - 1).Substring(0, BuyNumber.Length - 2).ToString().Trim();
            if (Numbers == "")
            {
                return;
            }

            //得到场次信息id和胜平负信息（全局变量）
            GameNumber = Numbers.Split('|');

            //[AB1] --> AB玩法几串几，1数字表示注数
            BuyWays = Number.Trim().Split(';')[2].ToString().Substring(1, Number.Trim().Split(';')[2].ToString().Length - 1).Substring(0, 2).ToString().Trim();
            if (BuyWays == "")
            {
                BuyWays = Netball.GetBuyWays(GameNumber.Length > 3 ? 3 : GameNumber.Length);
            }

            try
            {
                Mulitpe = int.Parse(Number.Trim().Split(';')[2].ToString().Substring(1, Number.Trim().Split(';')[2].ToString().Length - 2).Substring(2).ToString().Trim());
            }
            catch
            {
                Mulitpe = 1;
            }

            //一共多少场比赛
            this.tbSessionCount.Text = GameNumber.Length.ToString() + "场";

            string PlayName = string.Empty;

            PlayName = "(" + Netball.GetGameType(BuyWays) + ")";

            if (string.IsNullOrEmpty(PlayName) && GameNumber.Length > 3)
            {
                PlayName = "(3串1)";
            }

            this.tbGameType1.Text = PlayName;
            this.tbGameType2.Text = PlayName;
            this.tbGameType3.Text = PlayName;
            this.tbGameType4.Text = PlayName;
            this.tbGameType5.Text = PlayName;

            #region 页面初始状态，计算一共多少注，和总金额
            //计算一共多少注，和总金额
            string TempName = string.Empty;
            double ZhuCount = 1;

            List<string[]> List = new List<string[]>();//数组集合
            string TempStr = "";

            for (int k = 0; k < GameNumber.Length; k++)
            {
                TempName = GameNumber[k].ToString();

                string[] ZhuArr = TempName.Substring(TempName.IndexOf('(') + 1, TempName.LastIndexOf(')') - TempName.IndexOf('(') - 1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                TempStr = "";

                for (int i = 0; i < ZhuArr.Length; i++)
                {
                    TempStr += ZhuArr[i] + ",";
                }

                TempStr += "*";

                string[] arr = TempStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List.Add(arr);
            }

            GetList(List, "", List.Count - 1);//递归方法,获取未处理前得全部数组

            string GameTypeCount = PlayName.Substring(1, 1);

            if (GameTypeCount == List.Count.ToString()) //默认玩法 n串1，n就等于比赛场次
            {
                string Reg2 = @"\*";
                ListResult = ListResult.FindAll(t => !Regex.Match(t, Reg2).Success);
            }
            else
            {
                int Difference = List.Count - int.Parse(GameTypeCount);

                string Reg3 = @"\*{" + Difference + "}";

                ListResult = ListResult.FindAll(t => t.Split('*').Length == (Difference + 1));
            }

            ZhuCount = ListResult.Count;

            this.tbZhuCount.Text = ZhuCount + "注";
            this.tbSumMoney.Text = (ZhuCount * 2 * Mulitpe).ToString();
            this.tbMultiple.Text = Mulitpe.ToString();
            #endregion

            string StrId = string.Empty;
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            ServiceClient sc = new ServiceClient(binding, new EndpointAddress(new Uri(Application.Current.Host.Source, "Service.svc/T_PassRate")));
            for (int i = 0; i < GameNumber.Length; i++)
            {
                string str = GameNumber[i].ToString();
                StrId += str.Split('(')[0].ToString() + ",";//表t_SingleRate的id

            }

            //通过id获取详细信息    
            sc.GetPassRateAsync(StrId);
            sc.GetPassRateCompleted += new EventHandler<GetPassRateCompletedEventArgs>(sc_GetPassRateCompleted);
        }

        //加载球赛信息 “场次，主场球队，客场球队等”，加载到datagrid(回调函数)
        void sc_GetPassRateCompleted(object sender, GetPassRateCompletedEventArgs e)
        {
            List<T_PassRate> PassRate = new List<T_PassRate>(e.Result);

            string id = string.Empty;
            for (int j = 0; j < GameNumber.Length; j++)
            {
                Netball ball = new Netball();
                string str = GameNumber[j].ToString();

                id = str.Split('(')[0].ToString();//取出字符串里的一个id

                string[] otherinfo = str.Split('(')[1].Substring(0, str.Split('(')[1].Length - 1).Split(',');//胜负平信息数组
                int InfoLength = otherinfo.Length;

                #region
                for (int k = 0; k < PassRate.Count; k++)
                {
                    if (PassRate[k].MatchID.ToString() == id)
                    {
                        ball.Number = PassRate[k].MatchNumber;
                        ball.HomeField = PassRate[k].MainTeam;
                        ball.VisitingField = PassRate[k].GuestTeam;
                        ball.F01 = (double)PassRate[k].F01;
                        ball.F02 = (double)PassRate[k].F02;
                        ball.F03 = (double)PassRate[k].F03;
                        ball.F04 = (double)PassRate[k].F04;
                        ball.F05 = (double)PassRate[k].F05;
                        ball.F12 = (double)PassRate[k].F12;
                        ball.F13 = (double)PassRate[k].F13;
                        ball.F14 = (double)PassRate[k].F14;
                        ball.F15 = (double)PassRate[k].F15;
                        ball.F23 = (double)PassRate[k].F23;
                        ball.F24 = (double)PassRate[k].F24;
                        ball.F25 = (double)PassRate[k].F25;
                        ball.Fother = (double)PassRate[k].Fother;

                        ball.P00 = (double)PassRate[k].P00;
                        ball.P11 = (double)PassRate[k].P11;
                        ball.P22 = (double)PassRate[k].P22;
                        ball.P33 = (double)PassRate[k].P33;
                        ball.Pother = (double)PassRate[k].Pother;

                        ball.S10 = (double)PassRate[k].S10;
                        ball.S20 = (double)PassRate[k].S20;
                        ball.S21 = (double)PassRate[k].S21;
                        ball.S30 = (double)PassRate[k].S30;
                        ball.S31 = (double)PassRate[k].S31;
                        ball.S32 = (double)PassRate[k].S32;
                        ball.S40 = (double)PassRate[k].S40;
                        ball.S41 = (double)PassRate[k].S41;
                        ball.S42 = (double)PassRate[k].S42;
                        ball.S50 = (double)PassRate[k].S50;
                        ball.S51 = (double)PassRate[k].S51;
                        ball.S52 = (double)PassRate[k].S52;
                        ball.Sother = (double)PassRate[k].Sother;
                        ball.Id = PassRate[k].Id;

                        break;
                    }
                }
                #endregion

                NetBallList.Add(ball);
            }

            this.DataGrid1.ItemsSource = NetBallList;

            //延时初始化指数和、指数积、奖金
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }

        //初始化指数和、指数积、奖金的方法
        void Timer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            string[] ZhuArr = null;
            string ButtonContext = string.Empty;
            int i = 0;

            double SumMinOdds = 0.00;//指数和 最小
            double SumMaxOdds = 0.00;//指数和 最大

            double ProductMinOdds = 1.00;//指数积 最小
            double ProductMaxOdds = 1.00;//指数积 最大

            List<double> DoubleList = new List<double>();

            var selfcols = this.DataGrid1.Columns[0];

            foreach (var item in DataGrid1.ItemsSource)
            {
                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    //--单元格所包含的元素
                    Grid grid = cells as Grid;

                    StackPanel Sp = grid.Children[0] as StackPanel;

                    StackPanel Sp1 = Sp.Children[1] as StackPanel;

                    ZhuArr = GameNumber[i].Substring(GameNumber[i].IndexOf('(') + 1, GameNumber[i].LastIndexOf(')') - GameNumber[i].IndexOf('(') - 1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string s in ZhuArr)
                    {
                        Button b = ReturnShowNum(Sp1.Children[0] as StackPanel, s);

                        if (b == null)
                        {
                            b = ReturnShowNum(Sp1.Children[1] as StackPanel, s);
                            if (b == null)
                            {
                                b = ReturnShowNum(Sp1.Children[2] as StackPanel, s);
                            }
                        }

                        if (b != null)
                        {
                            b.Background = new SolidColorBrush(Colors.Red);//改变按钮颜色

                            DoubleList.Add(Netball.RetuntDouble(ToolTipService.GetToolTip(b).ToString()));
                        }
                    }

                    SumMinOdds += DoubleList.Min();
                    SumMaxOdds += DoubleList.Max();

                    ProductMinOdds *= DoubleList.Min();
                    ProductMaxOdds *= DoubleList.Max();
                }

                i++;
                DoubleList.Clear();

            }

            this.tbSum.Text = string.Format("{0:f2}", SumMinOdds) + "～" + string.Format("{0:f2}", SumMaxOdds);
            this.tbProduct.Text = string.Format("{0:f2}", ProductMinOdds) + "～" + string.Format("{0:f2}", ProductMaxOdds);
            this.tbPremium.Text = string.Format("{0:f2}", ProductMinOdds * 2) + "～" + string.Format("{0:f2}", ProductMaxOdds * 2);


            Timer.Stop();
        }

        //条件篮的全部删除事件
        void btnDelAllCondition_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            spCondition.Children.Clear();
            spCondition.Height = 250;
        }

        #region 点击过滤按钮加载条件
        //点击常规过滤(全部) 操作类型1
        private void btnRoutineThree_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;

            AddRoutineThree(NowButton.Content.ToString(), 0, 0);
        }

        void AddRoutineThree(string StrName, int Min, int Max)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 30;
            sp.Width = 470;

            sp.VerticalAlignment = VerticalAlignment.Center;
            sp.HorizontalAlignment = HorizontalAlignment.Left;
            sp.Margin = new Thickness(0, 0, 0, 0);

            //创建一个删除按钮
            Button bt = new Button();
            bt.Height = 22;
            bt.Width = 40;
            bt.Content = "删除";
            bt.HorizontalAlignment = HorizontalAlignment.Left;
            bt.Margin = new Thickness(15.0, 2.0, 0.0, 0.0);
            bt.Click += new RoutedEventHandler(bt_Click);

            //创建第一个下拉框
            ComboBox cb1 = new ComboBox();
            cb1.Width = 60;
            cb1.Height = 22;
            cb1.HorizontalAlignment = HorizontalAlignment.Left;
            cb1.Margin = new Thickness(50, 2, 0, 0);


            //创建一个textblock显示条件名称
            TextBlock tb = new TextBlock();
            tb.Height = 25;
            tb.FontSize = 12;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextAlignment = TextAlignment.Center;
            tb.Margin = new Thickness(10, 2, 0, 0);
            tb.Text = " <= " + StrName + " <= ";

            //创建第二个下拉框
            ComboBox cb2 = new ComboBox();
            cb2.HorizontalAlignment = HorizontalAlignment.Left;
            cb2.Height = 22;
            cb2.Width = 60;
            cb2.Margin = new Thickness(10, 2, 0, 0);

            //隐藏域
            TextBox txtHid = new TextBox();
            txtHid.Text = "1";
            txtHid.Visibility = Visibility.Collapsed;

            #region 设置下拉框里的数据内容

            int Count = 0;
            //根据选择不同的过滤条件，下拉框的数值不一样
            switch (StrName)
            {
                case "0的个数":
                case "1的个数":
                case "2的个数":
                case "3的个数":
                case "0的最大连续":
                case "1的最大连续":
                case "2的最大连续":
                case "3的最大连续":
                    Count = NetBallList.Count * 2;
                    break;
                case "4的个数":
                case "4的最大连续":
                case "主胜场数":
                case "主平场数":
                case "主负场数":
                case "大球个数":
                case "小球个数":
                    Count = NetBallList.Count;
                    break;
                case "和值区间":
                    Count = NetBallList.Count * 6;
                    break;
                case "断点区间":
                    Count = NetBallList.Count * 2 - 1;
                    break;
                default:
                    Count = 0;
                    break;
            }

            for (int i = 0; i <= Count; i++)
            {
                cb1.Items.Add(i.ToString());
                cb2.Items.Add(i.ToString());
            }

            cb1.SelectedIndex = Min;
            cb2.SelectedIndex = Max;
            #endregion

            sp.Children.Add(txtHid);
            sp.Children.Add(bt);
            sp.Children.Add(cb1);
            sp.Children.Add(tb);
            sp.Children.Add(cb2);


            spCondition.Children.Add(sp);

            if (spCondition.Children.Count > 8)
            {
                spCondition.Height += 30;
            }
        }

        //点击指数过滤的（指数和、指数积）按钮 操作类型2
        void btnIndexSum_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;
            AddIndexSum(NowButton.Content.ToString(), "0", "0");
        }

        void AddIndexSum(string StrName, string MinOdds, string MaxOdds)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 30;
            sp.Width = 470;

            sp.VerticalAlignment = VerticalAlignment.Center;
            sp.HorizontalAlignment = HorizontalAlignment.Left;
            sp.Margin = new Thickness(0, 0, 0, 0);

            //创建一个删除按钮
            Button bt = new Button();
            bt.Height = 22;
            bt.Width = 40;
            bt.Content = "删除";
            bt.HorizontalAlignment = HorizontalAlignment.Left;
            bt.Margin = new Thickness(15.0, 2.0, 0.0, 0.0);
            bt.Click += new RoutedEventHandler(bt_Click);

            //添加第一个文本框
            TextBox txt = new TextBox();
            txt.Width = 60;
            txt.Height = 22;
            txt.FontSize = 12;
            txt.HorizontalAlignment = HorizontalAlignment.Left;
            txt.Margin = new Thickness(10, 2, 0, 0);
            txt.Text = MinOdds;

            //创建一个textblock显示"《=" 符号
            TextBlock tb = new TextBlock();
            tb.Height = 22;
            tb.FontSize = 12;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextAlignment = TextAlignment.Center;
            tb.Margin = new Thickness(5, 5, 0, 0);
            tb.Text = " <= ";

            //创建第一个下拉框
            ComboBox cb1 = new ComboBox();
            cb1.Width = 80;
            cb1.Height = 22;
            cb1.HorizontalAlignment = HorizontalAlignment.Left;
            cb1.Margin = new Thickness(10, 2, 0, 0);
            cb1.Items.Add("竞彩赔率");
            cb1.SelectedIndex = 0;

            //创建一个textblock显示条件名称
            TextBlock tb1 = new TextBlock();
            tb1.Height = 22;
            tb1.FontSize = 12;
            tb1.HorizontalAlignment = HorizontalAlignment.Left;
            tb1.TextAlignment = TextAlignment.Center;
            tb1.Margin = new Thickness(10, 5, 0, 0);
            tb1.Text = StrName + " <= ";

            //添加第二个文本框
            TextBox txt1 = new TextBox();
            txt1.Width = 60;
            txt1.Height = 22;
            txt1.FontSize = 12;
            txt1.HorizontalAlignment = HorizontalAlignment.Left;
            txt1.Margin = new Thickness(10, 2, 0, 0);
            txt1.Text = MaxOdds;

            //隐藏域
            TextBox txtHid = new TextBox();
            txtHid.Text = "2";
            txtHid.Visibility = Visibility.Collapsed;

            sp.Children.Add(txtHid);//文本域
            sp.Children.Add(bt);//删除按钮
            sp.Children.Add(txt);//第一个文本框
            sp.Children.Add(tb);//TextBlock显示"<="
            sp.Children.Add(cb1);//下拉框
            sp.Children.Add(tb1);//textblock显示"<="
            sp.Children.Add(txt1);//显示第二个文本框

            spCondition.Children.Add(sp);

            if (spCondition.Children.Count > 8)
            {
                spCondition.Height += 30;
            }
        }

        //点击指数过滤的（奖金范围）按钮 操作类型3
        void btnIndexBonus_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;
            AddIndexBonus(NowButton.Content.ToString(), "0", "0");
        }

        void AddIndexBonus(string StrName, string MinOdds, string MaxOdds)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 30;
            sp.Width = 470;

            sp.VerticalAlignment = VerticalAlignment.Center;
            sp.HorizontalAlignment = HorizontalAlignment.Left;
            sp.Margin = new Thickness(0, 0, 0, 0);

            //创建一个删除按钮
            Button bt = new Button();
            bt.Height = 22;
            bt.Width = 40;
            bt.Content = "删除";
            bt.HorizontalAlignment = HorizontalAlignment.Left;
            bt.Margin = new Thickness(15.0, 2.0, 0.0, 0.0);
            bt.Click += new RoutedEventHandler(bt_Click);

            //添加第一个文本框
            TextBox txt = new TextBox();
            txt.Width = 60;
            txt.Height = 22;
            txt.FontSize = 12;
            txt.Text = "0";
            txt.HorizontalAlignment = HorizontalAlignment.Left;
            txt.Margin = new Thickness(10, 2, 0, 0);
            txt.Text = MinOdds;

            //创建一个textblock显示条件名称
            TextBlock tb1 = new TextBlock();
            tb1.Height = 22;
            tb1.FontSize = 12;
            tb1.HorizontalAlignment = HorizontalAlignment.Left;
            tb1.TextAlignment = TextAlignment.Center;
            tb1.Margin = new Thickness(10, 5, 0, 0);
            tb1.Text = "<= " + StrName + " <= ";

            //添加第二个文本框
            TextBox txt1 = new TextBox();
            txt1.Width = 60;
            txt1.Height = 22;
            txt1.FontSize = 12;
            txt1.Text = "0";
            txt1.HorizontalAlignment = HorizontalAlignment.Left;
            txt1.Margin = new Thickness(10, 2, 0, 0);
            txt1.Text = MaxOdds;

            //隐藏域
            TextBox txtHid = new TextBox();
            txtHid.Text = "3";
            txtHid.Visibility = Visibility.Collapsed;

            sp.Children.Add(txtHid);//隐藏域
            sp.Children.Add(bt);//删除按钮
            sp.Children.Add(txt);//第一个文本框
            sp.Children.Add(tb1);//textblock显示"文本"
            sp.Children.Add(txt1);//显示第二个文本框


            spCondition.Children.Add(sp);

            if (spCondition.Children.Count > 8)
            {
                spCondition.Height += 30;
            }
        }

        //点击指数过滤的(最低SP命中，最高sp命中) 操作类型4
        void btnIndexMost_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;
            AddIndexMost(NowButton.Content.ToString(), 0, 0);
        }

        void AddIndexMost(string StrName, int Min, int Max)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 30;
            sp.Width = 470;

            sp.VerticalAlignment = VerticalAlignment.Center;
            sp.HorizontalAlignment = HorizontalAlignment.Left;
            sp.Margin = new Thickness(0, 0, 0, 0);

            //创建一个删除按钮
            Button bt = new Button();
            bt.Height = 22;
            bt.Width = 40;
            bt.Content = "删除";
            bt.HorizontalAlignment = HorizontalAlignment.Left;
            bt.Margin = new Thickness(15.0, 2.0, 0.0, 0.0);
            bt.Click += new RoutedEventHandler(bt_Click);

            //添加第一个下拉框           
            ComboBox cb1 = new ComboBox();
            cb1.Width = 40;
            cb1.Height = 22;
            cb1.HorizontalAlignment = HorizontalAlignment.Left;
            cb1.Margin = new Thickness(10, 2, 0, 0);


            //创建一个textblock显示"《=" 符号
            TextBlock tb = new TextBlock();
            tb.Height = 22;
            tb.FontSize = 12;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.TextAlignment = TextAlignment.Center;
            tb.Margin = new Thickness(5, 5, 0, 0);
            tb.Text = " <= ";

            //创建第二个下拉框
            ComboBox cb2 = new ComboBox();
            cb2.Width = 80;
            cb2.Height = 22;
            cb2.HorizontalAlignment = HorizontalAlignment.Left;
            cb2.Margin = new Thickness(10, 2, 0, 0);
            cb2.Items.Add("竞彩赔率");
            cb2.SelectedIndex = 0;

            //创建一个textblock显示条件名称
            TextBlock tb1 = new TextBlock();
            tb1.Height = 22;
            tb1.FontSize = 12;
            tb1.HorizontalAlignment = HorizontalAlignment.Left;
            tb1.TextAlignment = TextAlignment.Center;
            tb1.Margin = new Thickness(10, 5, 0, 0);
            tb1.Text = StrName + " <= ";

            //添加第三个文本框
            ComboBox cb3 = new ComboBox();
            cb3.Width = 40;
            cb3.Height = 22;
            cb3.HorizontalAlignment = HorizontalAlignment.Left;
            cb3.Margin = new Thickness(10, 2, 0, 0);

            //隐藏域
            TextBox txtHid = new TextBox();
            txtHid.Text = "4";
            txtHid.Visibility = Visibility.Collapsed;

            //List<Netball> List = new List<Netball>();//DataGrid1.ItemsSource as List<Netball>;
            for (int i = 0; i <= NetBallList.Count; i++)
            {
                cb1.Items.Add(i.ToString());
                cb3.Items.Add(i.ToString());
            }

            cb1.SelectedIndex = Min;
            cb3.SelectedIndex = Max;

            sp.Children.Add(txtHid);//隐藏域
            sp.Children.Add(bt);//删除按钮
            sp.Children.Add(cb1);//第一个下拉框
            sp.Children.Add(tb);//TextBlock显示"<="
            sp.Children.Add(cb2);//第二个下拉框
            sp.Children.Add(tb1);//textblock显示"<="
            sp.Children.Add(cb3);//显示第三个文本框

            spCondition.Children.Add(sp);

            if (spCondition.Children.Count > 8)
            {
                spCondition.Height += 30;
            }
        }

        //排序截取的（赔率高到底排序、赔率低到高排序）
        void btnOrderDesc_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;
            AddOrderDesc(NowButton.Content.ToString(), 0, 0);
        }

        void AddOrderDesc(string StrName, int Min, int Max)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 30;
            sp.Width = 470;

            sp.VerticalAlignment = VerticalAlignment.Center;
            sp.HorizontalAlignment = HorizontalAlignment.Left;
            sp.Margin = new Thickness(0, 0, 0, 0);

            //创建一个删除按钮
            Button bt = new Button();
            bt.Height = 22;
            bt.Width = 40;
            bt.Content = "删除";
            bt.HorizontalAlignment = HorizontalAlignment.Left;
            bt.Margin = new Thickness(15.0, 2.0, 0.0, 0.0);
            bt.Click += new RoutedEventHandler(bt_Click);

            //添加第一个下拉框           
            ComboBox cb1 = new ComboBox();
            cb1.Width = 80;
            cb1.Height = 22;
            cb1.HorizontalAlignment = HorizontalAlignment.Left;
            cb1.Margin = new Thickness(10, 2, 0, 0);
            cb1.Items.Add("竞彩赔率");
            cb1.SelectedIndex = 0;


            //创建第一个textblock显示"赔率高到底排序取第"文字
            TextBlock tb1 = new TextBlock();
            tb1.Height = 22;
            tb1.FontSize = 12;
            tb1.HorizontalAlignment = HorizontalAlignment.Left;
            tb1.TextAlignment = TextAlignment.Center;
            tb1.Margin = new Thickness(5, 5, 0, 0);
            tb1.Text = StrName + "排序取第 ";

            //创建第一个文本框
            TextBox txt1 = new TextBox();
            txt1.Width = 50;
            txt1.Height = 22;
            txt1.FontSize = 12;
            txt1.Text = "0";
            txt1.HorizontalAlignment = HorizontalAlignment.Left;
            txt1.Margin = new Thickness(5, 2, 0, 0);
            txt1.Text = Min.ToString();


            //创建第二个textblock显示"注--第"
            TextBlock tb2 = new TextBlock();
            tb2.Height = 22;
            tb2.FontSize = 12;
            tb2.HorizontalAlignment = HorizontalAlignment.Left;
            tb2.TextAlignment = TextAlignment.Center;
            tb2.Margin = new Thickness(5, 5, 0, 0);
            tb2.Text = "注--第";

            //添加第二个文本框
            TextBox txt2 = new TextBox();
            txt2.Width = 50;
            txt2.Height = 22;
            txt2.FontSize = 12;
            txt2.Text = "0";
            txt2.HorizontalAlignment = HorizontalAlignment.Left;
            txt2.Margin = new Thickness(5, 2, 0, 0);
            txt2.Text = Max.ToString();

            //创建第三个textblock显示"注"
            TextBlock tb3 = new TextBlock();
            tb3.Height = 22;
            tb3.FontSize = 12;
            tb3.HorizontalAlignment = HorizontalAlignment.Left;
            tb3.TextAlignment = TextAlignment.Center;
            tb3.Margin = new Thickness(5, 5, 0, 0);
            tb3.Text = "注";

            //隐藏域
            TextBox txtHid = new TextBox();
            txtHid.Text = "5";
            txtHid.Visibility = Visibility.Collapsed;

            sp.Children.Add(txtHid);//隐藏域
            sp.Children.Add(bt);//删除按钮
            sp.Children.Add(cb1);//第一个下拉框
            sp.Children.Add(tb1);//TextBlock显示"赔率高到底排序取第"
            sp.Children.Add(txt1);//第一个文本框
            sp.Children.Add(tb2);//textblock显示"注--第"
            sp.Children.Add(txt2);//显示第二个文本框
            sp.Children.Add(tb3);//textBlock显示"注"

            spCondition.Children.Add(sp);

            if (spCondition.Children.Count > 8)
            {
                spCondition.Height += 30;
            }
        }

        //范围截取的(随机截取、奖金最高、概率最高)
        void btnRangeRandom_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;
            AddRangeRandom(NowButton.Content.ToString(), "");
        }

        void AddRangeRandom(string StrName, string term)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 30;
            sp.Width = 470;

            sp.VerticalAlignment = VerticalAlignment.Center;
            sp.HorizontalAlignment = HorizontalAlignment.Left;
            sp.Margin = new Thickness(0, 0, 0, 0);

            //创建一个删除按钮
            Button bt = new Button();
            bt.Height = 22;
            bt.Width = 40;
            bt.Content = "删除";
            bt.HorizontalAlignment = HorizontalAlignment.Left;
            bt.Margin = new Thickness(15.0, 2.0, 0.0, 0.0);
            bt.Click += new RoutedEventHandler(bt_Click);

            //第一个textblock显示
            TextBlock tb1 = new TextBlock();
            tb1.Height = 22;
            tb1.FontSize = 12;
            tb1.HorizontalAlignment = HorizontalAlignment.Left;
            tb1.TextAlignment = TextAlignment.Center;
            tb1.Margin = new Thickness(5, 5, 0, 0);
            tb1.Text = StrName;

            //创建第一个文本框
            TextBox txt1 = new TextBox();
            txt1.Width = 70;
            txt1.Height = 22;
            txt1.FontSize = 12;
            txt1.HorizontalAlignment = HorizontalAlignment.Left;
            txt1.Margin = new Thickness(5, 2, 0, 0);
            txt1.Text = term;

            //第二个textblock显示
            TextBlock tb2 = new TextBlock();
            tb2.Height = 22;
            tb2.FontSize = 12;
            tb2.HorizontalAlignment = HorizontalAlignment.Left;
            tb2.TextAlignment = TextAlignment.Center;
            tb2.Margin = new Thickness(5, 5, 0, 0);
            tb2.Text = "注";

            //隐藏域
            TextBox txtHid = new TextBox();
            txtHid.Text = "6";
            txtHid.Visibility = Visibility.Collapsed;

            sp.Children.Add(txtHid);//隐藏域
            sp.Children.Add(bt);//删除按钮
            sp.Children.Add(tb1);//TextBlock显示
            sp.Children.Add(txt1);//第一个文本框
            sp.Children.Add(tb2);//textblock显示"注--第"
            spCondition.Children.Add(sp);

            if (spCondition.Children.Count > 8)
            {
                spCondition.Height += 30;
            }
        }

        //分组过滤/比分个数/集合过滤的所有子导航的按钮事件
        void btnGroupSum_Click(object sender, RoutedEventArgs e)
        {
            //当前按钮对象
            Button NowButton = sender as Button;
            AddGroupSum(NowButton.Content.ToString(), "");
        }

        void AddGroupSum(string StrName, string term)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Height = 30;
            sp.Width = 470;

            sp.VerticalAlignment = VerticalAlignment.Center;
            sp.HorizontalAlignment = HorizontalAlignment.Left;
            sp.Margin = new Thickness(0, 0, 0, 0);

            //创建一个删除按钮
            Button bt = new Button();
            bt.Height = 22;
            bt.Width = 40;
            bt.Content = "删除";
            bt.HorizontalAlignment = HorizontalAlignment.Left;
            bt.Margin = new Thickness(15.0, 2.0, 0.0, 0.0);
            bt.Click += new RoutedEventHandler(bt_Click);

            //创建第一个textblock显示
            TextBlock tb1 = new TextBlock();
            tb1.Height = 22;
            tb1.FontSize = 12;
            tb1.HorizontalAlignment = HorizontalAlignment.Left;
            tb1.TextAlignment = TextAlignment.Center;
            tb1.Margin = new Thickness(15, 5, 0, 0);
            tb1.Text = StrName;

            //创建一个textbox
            TextBox txt1 = new TextBox();
            txt1.Width = 150;
            txt1.Height = 22;
            txt1.FontSize = 12;
            txt1.HorizontalAlignment = HorizontalAlignment.Left;
            txt1.Margin = new Thickness(5, 2, 0, 0);

            txt1.Text = "请点击设置按钮设定条件。";

            if (!string.IsNullOrEmpty(term))
            {
                txt1.Text = "共选择了" + term.Split(';').Length.ToString() + "个条件";
            }

            //创建一个按钮"设置"
            Button bt1 = new Button();
            bt1.Height = 22;
            bt1.Width = 40;
            bt1.Content = "设置";
            bt1.HorizontalAlignment = HorizontalAlignment.Left;
            bt1.Margin = new Thickness(15.0, 2.0, 0.0, 0.0);
            bt1.Click += new RoutedEventHandler(bt1_Click);

            //隐藏域
            TextBox txtHid = new TextBox();
            txtHid.Text = "7";
            txtHid.Visibility = Visibility.Collapsed;

            //隐藏域（子窗体传来的条件）
            TextBox txtHid1 = new TextBox();
            txtHid1.Visibility = Visibility.Collapsed;
            txtHid1.Text = term;

            sp.Children.Add(txtHid);//隐藏域
            sp.Children.Add(bt);//删除按钮
            sp.Children.Add(tb1);//TextBlock显示
            sp.Children.Add(txt1);//第一个文本框
            sp.Children.Add(bt1);//"设置"按钮
            sp.Children.Add(txtHid1);
            spCondition.Children.Add(sp);

            if (spCondition.Children.Count > 8)
            {
                spCondition.Height += 30;
            }
        }

        #endregion

        #region 打开子窗体事件
        void bt1_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            Button btn = sender as Button;
            StackPanel sp = btn.Parent as StackPanel;
            string Title = (sp.Children[2] as TextBlock).Text.ToString();

            switch (Title)
            {
                case "命中场次":
                case "冷门过滤":
                case "叠加过滤":
                    GatherFilter.Title = Title;
                    GatherFilter.OverlayBrush = new SolidColorBrush(Colors.White);
                    GatherFilter.Opacity = 1;
                    GatherFilter.HasCloseButton = true;
                    GatherFilter.Foreground = new SolidColorBrush(Colors.Red);
                    GatherFilter.FontSize = 14;
                    GatherFilter.OverlayOpacity = 0.5;
                    //group.IsEnabled = false; 
                    GatherFilter.ListStr = (sp.Children[5] as TextBox).Text.ToString();

                    GatherFilter.SelectedRate = getSelectNum();
                    GatherFilter.Show();//打开子窗体
                    GatherFilter.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
                case "比分个数":
                    ScoreCount.Title = Title;
                    ScoreCount.OverlayBrush = new SolidColorBrush(Colors.White);
                    ScoreCount.Opacity = 1;
                    ScoreCount.HasCloseButton = true;
                    ScoreCount.Foreground = new SolidColorBrush(Colors.Red);
                    ScoreCount.FontSize = 14;
                    ScoreCount.OverlayOpacity = 0.5;
                    //group.IsEnabled = false; 
                    //ScoreCount.ListStr = (sp.Children[5] as TextBox).Text.ToString();

                    //ScoreCount.SelectedRate = getSelectNum();
                    ScoreCount.Show();//打开子窗体
                    ScoreCount.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
                case "分组主胜":
                case "分组主平":
                case "分组主负":
                case "分组大球":
                case "分组小球":
                case "分组断点":
                case "分组和值":
                case "分组连号":
                case "分组最长连号":
                case "进球差值":
                    GrounpFilter.Title = Title;
                    GrounpFilter.OverlayBrush = new SolidColorBrush(Colors.White);
                    GrounpFilter.Opacity = 1;
                    GrounpFilter.HasCloseButton = true;
                    GrounpFilter.Foreground = new SolidColorBrush(Colors.Red);
                    GrounpFilter.FontSize = 14;
                    GrounpFilter.OverlayOpacity = 0.5;
                    //group.IsEnabled = false; 
                    GrounpFilter.ListStr = (sp.Children[5] as TextBox).Text.ToString();

                    //ScoreCount.SelectedRate = getSelectNum();
                    GrounpFilter.Show();//打开子窗体
                    GrounpFilter.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
            }
        }
        #endregion

        #region 子窗体关闭后事件
        void GatherFilter_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (GatherFilter.DialogResult == true)
            {
                string LbList = string.Format("{0}", GatherFilter.HidResult.Text.ToString());
                (GatherFilter.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (GatherFilter.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }

        void ScoreCount_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (ScoreCount.DialogResult == true)
            {
                string LbList = string.Format("{0}", ScoreCount.HidResult.Text.ToString());
                (ScoreCount.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (ScoreCount.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }

        void GrounpFilter_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (GrounpFilter.DialogResult == true)
            {
                string LbList = string.Format("{0}", GrounpFilter.HidResult.Text.ToString());
                (GrounpFilter.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (GrounpFilter.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }
        #endregion


        //条件篮的删除按钮
        void bt_Click(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            StackPanel sp = bt.Parent as StackPanel;

            spCondition.Children.Remove(sp);
            if (spCondition.Children.Count >= 8)
            {
                spCondition.Height -= 30;
            }
        }

        //条件篮的主导航按钮
        void btnRoutine_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;//当前按钮对象
            StackPanel sp = (b.Parent as StackPanel).Parent as StackPanel;//最外层stackpanel对象

            for (int i = 0; i < sp.Children.Count; i++)
            {
                StackPanel sp1 = sp.Children[i] as StackPanel;
                if (sp1 == b.Parent as StackPanel)//是当前点击的按钮Stackpanel 
                {
                    if (sp1.Children[1].Visibility == Visibility)//如果初始是显示状态
                    {
                        sp1.Children[1].Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        for (int j = 0; j < sp.Children.Count; j++)
                        {
                            StackPanel sp2 = sp.Children[j] as StackPanel;
                            sp2.Children[1].Visibility = Visibility.Collapsed;
                        }
                        sp1.Children[1].Visibility = Visibility;
                    }
                }
            }
        }

        //datagrid行加载事件（可以删除的）----------------------
        private void DataGrid1_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            // TODO: Add event handler implementation here.            
            //Grid gd = DataGrid1.Columns[0].GetCellContent(e.Row) as Grid;

            //StackPanel Sp = gd.Children[0] as StackPanel;

            //StackPanel Sp1 = Sp.Children[1] as StackPanel;

            //string TempName = string.Empty;

            //TempName = GameNumber[e.Row.GetIndex()].ToString();

            //string[] ZhuArr = TempName.Substring(TempName.IndexOf('(') + 1, TempName.LastIndexOf(')') - TempName.IndexOf('(') - 1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            //foreach (string s in ZhuArr)
            //{

            //    Button b = ReturnShowNum(Sp1.Children[0] as StackPanel, s);

            //    if (b == null)
            //    {
            //        b = ReturnShowNum(Sp1.Children[1] as StackPanel, s);
            //        if (b == null)
            //        {
            //            b = ReturnShowNum(Sp1.Children[2] as StackPanel, s);
            //        }
            //    }

            //    if (b != null)
            //    {
            //        b.Background = new SolidColorBrush(Colors.Red);
            //    }
            //}
        }

        //传过来的字符串拆分后的比分信息，获得按钮对象
        public Button ReturnShowNum(StackPanel Sp, string Str)
        {
            Str = Netball.GetScoreInfo(Str);

            foreach (Button b in Sp.Children)
            {
                if ((b.Content.ToString()) == Str)
                {
                    return b;
                }
            }
            return null;
        }

        //比分的小按钮事件
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.

            Button button = sender as Button;
            SolidColorBrush b_Brush = (SolidColorBrush)(button.Background);
            int ColorCount = 0;

            if (b_Brush.Color.ToString() == "#FFFF0000")
            {
                foreach (StackPanel Stack in ((button.Parent as StackPanel).Parent as StackPanel).Children)
                {
                    foreach (Button button2 in Stack.Children)
                    {
                        SolidColorBrush b_Brush2 = (SolidColorBrush)(button2.Background);
                        if (b_Brush2.Color.ToString() == "#FFFF0000")
                        {
                            ColorCount++;
                        }
                    }
                }

                if (ColorCount > 1)
                {
                    button.Background = new SolidColorBrush(Colors.White);
                }
            }
            else
            {
                button.Background = new SolidColorBrush(Colors.Red);
            }

            double SumMinOdds = 0.00;//指数和 最小
            double SumMaxOdds = 0.00;//指数和 最大

            double ProductMinOdds = 1.00;//指数积 最小
            double ProductMaxOdds = 1.00;//指数积 最大

            int ZhuCount = 1;
            List<double> DoubleList = new List<double>();

            var selfcols = this.DataGrid1.Columns[0];

            foreach (var item in DataGrid1.ItemsSource)
            {
                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    //--单元格所包含的元素
                    Grid grid = cells as Grid;

                    StackPanel Sp = grid.Children[0] as StackPanel;

                    StackPanel Sp1 = Sp.Children[1] as StackPanel;

                    foreach (StackPanel StackPanel in Sp1.Children)
                    {
                        foreach (Button button1 in StackPanel.Children)
                        {
                            SolidColorBrush b_Brush1 = (SolidColorBrush)(button1.Background);

                            if (b_Brush1.Color.ToString() == "#FFFF0000")
                            {
                                DoubleList.Add(Netball.RetuntDouble(ToolTipService.GetToolTip(button1).ToString()));
                            }
                        }
                    }

                    ZhuCount *= DoubleList.Count;

                    SumMinOdds += DoubleList.Min();
                    SumMaxOdds += DoubleList.Max();

                    ProductMinOdds *= DoubleList.Min();
                    ProductMaxOdds *= DoubleList.Max();
                }
                DoubleList.Clear();
            }

            this.tbSum.Text = string.Format("{0:f2}", SumMinOdds) + "～" + string.Format("{0:f2}", SumMaxOdds);
            this.tbProduct.Text = string.Format("{0:f2}", ProductMinOdds) + "～" + string.Format("{0:f2}", ProductMaxOdds);
            this.tbPremium.Text = string.Format("{0:f2}", ProductMinOdds * 2) + "～" + string.Format("{0:f2}", ProductMaxOdds * 2);
        }

        private void btnChoose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            HtmlWindow html = HtmlPage.Window;
            html.Navigate(new Uri("../JCZC/Buy_ZJQS.aspx", UriKind.Relative), "_self");
        }

        void SuBmit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Num.Count > 1000)
            {
                MessageBox.Show("投注注数超过 1000 注，请重新缩水！");

                return;
            }

            string[] LotteryNumbers = new string[Num.Count];
            string LotteryNumber = "";
            string str = "";

            for (int i = 0; i < Num.Count; i++)
            {
                LotteryNumber = "7202;[";

                for (int j = 0; j < GameNumber.Length; j++)
                {
                    if (Num[i].LotteryNums.Substring(j * 2, 2).Equals("**"))
                    {
                        continue;
                    }

                    LotteryNumber += GameNumber[j].Substring(0, GameNumber[j].IndexOf('(')) + "(" + Netball.GetLotterNumber(Num[i].LotteryNums.Substring(j * 2, 2)) + ")|";
                }

                if (LotteryNumber.EndsWith("|"))
                {
                    LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
                }

                LotteryNumber += "];[" + BuyWays + Mulitpe.ToString() + "]";

                str += LotteryNumber + "\n\r";

                LotteryNumbers[i] = LotteryNumber;
            }

            try
            {
                WebClient Appclient = new WebClient();

                Appclient.UploadStringAsync(new Uri("../JCZC/FilterShrink.ashx", UriKind.Relative), str);
                Appclient.UploadStringCompleted += new UploadStringCompletedEventHandler(Appclient_UploadStringCompleted);
            }
            catch { }

        }

        void Appclient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Result.Equals("true"))
            {
                HtmlWindow html = HtmlPage.Window;
                html.Navigate(new Uri("../JCZC/BuyConfirm.aspx", UriKind.Relative), "_self");
            }
            else
            {
                MessageBox.Show("提交投注结果出现异常，请重新提交，谢谢！");
                return;
            }
        }


        //获取未处理前得全部数组（递归方法）
        static void GetList(List<string[]> lists, string str, int n)
        {
            if (n == 0)
            {
                for (int i = 0; i < lists[n].Length; i++)
                {
                    ListResult.Add(lists[n][i] + str);
                }
            }
            else
            {
                for (int i = 0; i < lists[n].Length; i++)
                {
                    GetList(lists, lists[n][i] + str, n - 1);
                }
            }
        }

        //处理按钮------------------------------
        private void btnProcess_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            ListResult.Clear();

            var selfcols = DataGrid1.Columns[0];//第1列 是首次末的按钮

            List<Netball> NetBall = DataGrid1.ItemsSource as List<Netball>;

            string GameTypeCount = this.tbGameType1.Text.Substring(1, 1).ToString();//几串几，数字
            if (GameTypeCount[0] > "3"[0]) { GameTypeCount = "3"; }

            List<string[]> List = new List<string[]>();//数组集合           

            foreach (var item in DataGrid1.ItemsSource) //遍历第1列
            {
                string TempStr = string.Empty;
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    Grid Grid = cells as Grid;

                    StackPanel Sp = Grid.Children[0] as StackPanel;

                    StackPanel Sp1 = Sp.Children[1] as StackPanel;

                    foreach (Button button1 in (Sp1.Children[0] as StackPanel).Children)
                    {
                        SolidColorBrush b_Brush1 = (SolidColorBrush)(button1.Background);
                        if (b_Brush1.Color.ToString() == "#FFFF0000")
                        {
                            TempStr += button1.Content.ToString() + ",";
                        }
                    }

                    foreach (Button button2 in (Sp1.Children[1] as StackPanel).Children)
                    {
                        SolidColorBrush b_Brush2 = (SolidColorBrush)(button2.Background);
                        if (b_Brush2.Color.ToString() == "#FFFF0000")
                        {
                            TempStr += button2.Content.ToString() + ",";
                        }
                    }

                    foreach (Button button3 in (Sp1.Children[2] as StackPanel).Children)
                    {
                        SolidColorBrush b_Brush3 = (SolidColorBrush)(button3.Background);
                        if (b_Brush3.Color.ToString() == "#FFFF0000")
                        {
                            TempStr += button3.Content.ToString() + ",";
                        }
                    }

                    TempStr += "**";

                    string[] arr = TempStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    List.Add(arr);
                }
            }


            if (this.spCondition.Children.Count == 0)//没有设置筛选条件
            {
                MessageBox.Show("还没有设置筛选条件！");
            }
            else
            {
                GetList(List, "", List.Count - 1);//递归方法,获取未处理前得全部数组               

                if (GameTypeCount == List.Count.ToString()) //默认玩法 n串1，n就等于比赛场次
                {
                    string Reg2 = @"\*\*";
                    ListResult = ListResult.FindAll(t => !Regex.Match(t, Reg2).Success);
                }
                else if (ConvertInt(GameTypeCount) < List.Count)
                {
                    int Difference = List.Count - int.Parse(GameTypeCount);

                    ListResult = ListResult.FindAll(t => t.Split(new string[] { "**" }, StringSplitOptions.None).Length == Difference + 1);
                }
                else
                {
                    MessageBox.Show("玩法选择错误！");
                    return;
                }

                List<string> TempList = new List<string>();

                string ProcessType = string.Empty;//处理类型                
                string TempStr = string.Empty;

                for (int i = 0; i < this.spCondition.Children.Count; i++)
                {
                    StackPanel StackPanel = this.spCondition.Children[i] as StackPanel;

                    ProcessType = (StackPanel.Children[0] as TextBox).Text.ToString();

                    int Min = 0;
                    int Max = 0;

                    double MinOdds = 0.00;
                    double MaxOdds = 0.00;

                    string term = string.Empty;

                    switch (ProcessType)
                    {
                        case "1"://常规过滤和首次末过滤的所有过滤条件
                            Min = int.Parse((StackPanel.Children[2] as ComboBox).SelectedItem.ToString());
                            Max = int.Parse((StackPanel.Children[4] as ComboBox).SelectedItem.ToString());

                            TempStr = (StackPanel.Children[3] as TextBlock).Text;
                            TempStr = TempStr.Substring(4, TempStr.Length - 8);
                            break;
                        case "2"://点击指数过滤的（指数和、指数积）                           
                            double.TryParse((StackPanel.Children[2] as TextBox).Text, out MinOdds);
                            double.TryParse((StackPanel.Children[6] as TextBox).Text, out MaxOdds);

                            TempStr = (StackPanel.Children[5] as TextBlock).Text;
                            TempStr = TempStr.Substring(0, 3);
                            break;
                        case "3"://点击指数过滤的（奖金范围）
                            double.TryParse((StackPanel.Children[2] as TextBox).Text, out MinOdds);
                            double.TryParse((StackPanel.Children[4] as TextBox).Text, out MaxOdds);
                            TempStr = (StackPanel.Children[3] as TextBlock).Text;
                            TempStr = TempStr.Substring(3, TempStr.Length - 7);

                            break;
                        case "4"://点击指数过滤的(最低SP命中，最高SP命中)
                            Min = int.Parse((StackPanel.Children[2] as ComboBox).SelectedItem.ToString());
                            Max = int.Parse((StackPanel.Children[6] as ComboBox).SelectedItem.ToString());
                            TempStr = (StackPanel.Children[5] as TextBlock).Text;

                            TempStr = TempStr.Substring(0, TempStr.Length - 4);
                            break;
                        case "5"://排序截取的(赔率高到低，赔率低到高)
                            Min = int.Parse((StackPanel.Children[4] as TextBox).Text);
                            Max = int.Parse((StackPanel.Children[6] as TextBox).Text);
                            TempStr = (StackPanel.Children[3] as TextBlock).Text;

                            TempStr = TempStr.Substring(0, TempStr.Length - 1);
                            break;
                        case "6"://范围截取的(随机截取，奖金最高，概率最高)
                            Min = int.Parse((StackPanel.Children[3] as TextBox).Text);

                            TempStr = (StackPanel.Children[2] as TextBlock).Text;
                            break;

                        case "7"://集合过滤的(命中场次，冷门过滤，叠加过滤)
                            term = (StackPanel.Children[5] as TextBox).Text;
                            TempStr = (StackPanel.Children[2] as TextBlock).Text;
                            break;
                    }

                    List<string> TempListStr = new List<string>();

                    //筛选的条件不同，筛选不同的方法
                    if (i == 0)
                    {
                        TempList = ListResult;
                    }

                    #region 根据过滤的条件不同，选择不同的过滤方法

                    switch (TempStr)
                    {
                        case "0的个数":
                            TempListStr = GetZeroCounts(Min, Max, "0", TempList);
                            TempList = TempListStr;
                            break;
                        case "1的个数":
                            TempListStr = GetZeroCounts(Min, Max, "1", TempList);
                            TempList = TempListStr;
                            break;
                        case "2的个数":
                            TempListStr = GetZeroCounts(Min, Max, "2", TempList);
                            TempList = TempListStr;
                            break;
                        case "3的个数":
                            TempListStr = GetZeroCounts(Min, Max, "3", TempList);
                            TempList = TempListStr;
                            break;
                        case "4的个数":
                            TempListStr = GetZeroCounts(Min, Max, "4", TempList);
                            TempList = TempListStr;
                            break;
                        case "0的最大连续":
                            TempListStr = GetMaxLian(Min, Max, '0', TempList);
                            TempList = TempListStr;
                            break;
                        case "1的最大连续":
                            TempListStr = GetMaxLian(Min, Max, '1', TempList);
                            TempList = TempListStr;
                            break;
                        case "2的最大连续":
                            TempListStr = GetMaxLian(Min, Max, '2', TempList);
                            TempList = TempListStr;
                            break;
                        case "3的最大连续":
                            TempListStr = GetMaxLian(Min, Max, '3', TempList);
                            TempList = TempListStr;
                            break;
                        case "4的最大连续":
                            TempListStr = GetMaxLian(Min, Max, '4', TempList);
                            TempList = TempListStr;
                            break;
                        case "主胜场数":
                            TempListStr = GetMainWin(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "主平场数":
                            TempListStr = GetMainLevel(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "主负场数":
                            TempListStr = GetMainLose(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "大球个数":
                            TempListStr = GetBigBall(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "小球个数":
                            TempListStr = GetSmallBall(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "和值区间":
                            TempListStr = GetSumValue(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "断点区间":
                            TempListStr = GetBreakPoint(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "指数和":
                            TempListStr = GetIndexSum(MinOdds, MaxOdds, TempList);
                            TempList = TempListStr;
                            break;
                        case "指数积":
                            TempListStr = GetIndexProduct(MinOdds, MaxOdds, TempList);
                            TempList = TempListStr;
                            break;
                        case "奖金范围":
                            TempListStr = GetIndexBonus(MinOdds, MaxOdds, TempList);
                            TempList = TempListStr;
                            break;
                        case "最低SP命中":
                            TempListStr = GetMinSP(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "最高SP命中":
                            TempListStr = GetMaxSP(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "赔率高到低排序取第":
                            TempListStr = GetOrder(Min, Max, 1, TempList);
                            TempList = TempListStr;
                            break;
                        case "赔率底到高排序取第":
                            TempListStr = GetOrder(Min, Max, 2, TempList);
                            TempList = TempListStr;
                            break;
                        case "随机截取":
                            TempListStr = GetRangeRandom(Min, TempList);
                            TempList = TempListStr;
                            break;
                        case "奖金最高":
                            TempListStr = GetRangeBonus(Min, 1, TempList);
                            TempList = TempListStr;
                            break;
                        case "概率最高":
                            TempListStr = GetRangeBonus(Min, 2, TempList);
                            TempList = TempListStr;
                            break;
                        case "命中场次":
                        case "冷门过滤":
                        case "叠加过滤":
                            TempListStr = GetGoupHitTarget(term, TempList);
                            TempList = TempListStr;
                            break;
                        case "比分个数":
                            TempListStr = GetScoreNumber(term, TempList);
                            TempList = TempListStr;
                            break;
                        case "分组主胜":
                        case "分组主平":
                        case "分组主负":
                        case "分组大球":
                        case "分组小球":
                        case "分组断点":
                        case "分组和值":
                        case "分组连号":
                        case "分组最长连号":
                        case "进球差值":
                            TempListStr = GetGoupNumbers(term, TempStr, TempList);
                            TempList = TempListStr;
                            break;
                    }
                    #endregion
                }

                #region 处理后的数据和分页

                Num = new List<LotteryNum>();

                for (int j = 0; j < TempList.Count; j++)
                {
                    LotteryNum LotteryNum = new LotteryNum();
                    LotteryNum.Id = j + 1;
                    LotteryNum.LotteryNums = TempList[j].ToString();
                    Num.Add(LotteryNum);
                }

                //分页控件需要的
                PagedCollectionView view = new PagedCollectionView(Num);

                this.DataGrid2.ItemsSource = view;
                DataPager1.PageSize = 4;
                DataPager1.Source = view;
                #endregion

                this.tbCathectic.Text = TempList.Count.ToString();
                this.tbCathecticSum.Text = (TempList.Count * 2).ToString();
            }
        }

        #region  过滤的方法
        //常规过滤的（0的个数,1的个数，2的个数，3的个数，4的个数）
        List<string> GetZeroCounts(int Min, int Max, string Num, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            string Reg1 = "";//匹配最小值正则
            string Reg = "";//匹配最大值正则
            if (Min == 0)
            {
                Reg1 = @"(\d|\*)*";
            }
            else
            {
                for (int i = 0; i < Min; i++)
                {
                    Reg1 += @"(\d|\*)*" + Num + @"(\d|\*)*";
                }
            }

            if (Max == 0)
            {
                Reg = @"(\d|\*)*" + Num + @"(\d|\*)*";
            }
            else
            {
                for (int j = 0; j < Max; j++)
                {
                    Reg += @"(\d|\*)*" + Num + @"(\d|\*)*";
                }

                Reg += @"(\d|\*)*" + Num + @"(\d|\*)*";
            }

            ListStr = ListR.FindAll(t => Regex.Match(t, Reg1).Success);

            ListStr = ListStr.FindAll(t => !Regex.Match(t, Reg).Success);
            return ListStr;
        }

        //常规过滤的(0的最大连续，1的最大连续，2的最大连续，3的最大连续，4的最大连续)
        List<string> GetMaxLian(int Min, int Max, char Temp, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            int MaxCount = 0;

            foreach (string s in ListR)
            {
                Count = 0;
                MaxCount = 0;

                foreach (char c in s)
                {
                    if (c == Temp)
                    {
                        Count++;
                    }
                    else
                    {
                        if (Count > MaxCount)
                        {
                            MaxCount = Count;
                        }
                        Count = 0;
                    }
                }
                MaxCount = Math.Max(MaxCount, Count);

                if (MaxCount >= Min && MaxCount <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //常规过滤的（主胜场数）GetMainLevel
        List<string> GetMainWin(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();
            int Count = 0;
            foreach (string s in ListR)
            {
                Count = 0;
                for (int j = 0; j < s.Length; j += 2)
                {
                    if (ConvertInt(s[j].ToString()) > ConvertInt(s[j + 1].ToString()))
                    {
                        Count++;
                    }
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //常规过滤的（主平场数）
        List<string> GetMainLevel(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();
            int Count = 0;
            foreach (string s in ListR)
            {
                Count = 0;
                for (int j = 0; j < s.Length; j += 2)
                {
                    if (s[j].ToString() != "W" && s[j].ToString() != "D" && s[j].ToString() != "L" && s[j].ToString() != "*")
                    {
                        if (ConvertInt(s[j].ToString()) == ConvertInt(s[j + 1].ToString()))
                        {
                            Count++;
                        }
                    }
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //常规过滤的（主负场数）
        List<string> GetMainLose(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();
            int Count = 0;
            foreach (string s in ListR)
            {
                Count = 0;
                for (int j = 0; j < s.Length; j += 2)
                {
                    if (ConvertInt(s[j].ToString()) < ConvertInt(s[j + 1].ToString()))
                    {
                        Count++;
                    }
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //常规过滤的（大球个数）
        List<string> GetBigBall(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            int SumCount = 0;
            foreach (string s in ListR)
            {
                Count = 0;
                SumCount = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    SumCount = ConvertInt(s[i].ToString()) + ConvertInt(s[i + 1].ToString());
                    if (SumCount >= 3)
                    {
                        Count++;
                    }
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //常规过滤的（小球个数）
        List<string> GetSmallBall(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            int SumCount = 0;
            foreach (string s in ListR)
            {
                Count = 0;
                SumCount = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    if (s[i].ToString() != "W" && s[i].ToString() != "D" && s[i].ToString() != "L" && s[i].ToString() != "*")
                    {
                        SumCount = ConvertInt(s[i].ToString()) + ConvertInt(s[i + 1].ToString());
                    }

                    if (SumCount < 3)
                    {
                        Count++;
                    }
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //常规过滤的（和值区间）
        List<string> GetSumValue(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Sum = 0;
            foreach (string s in ListR)
            {
                Sum = 0;
                foreach (char c in s)
                {
                    Sum += ConvertInt(c.ToString());
                }
                if (Sum >= Min && Sum <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //常规过滤的（断点个数）
        List<string> GetBreakPoint(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            foreach (string s in ListR)
            {
                Count = -1;
                string s2 = s.Replace("*", "");
                char Temp = 'x';
                foreach (char c in s2)
                {
                    if (Temp != c)
                    {
                        Temp = c;
                        Count++;
                    }
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }

            return ListStr;
        }

        //指数过滤的(指数和)
        List<string> GetIndexSum(double Min, double Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            double SumOdd = 0.00;
            double Odd = 0.00;

            int k = 0;
            foreach (string s in ListR)
            {
                SumOdd = 0.00;
                k = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    Odd = ReturnOdds(s[i].ToString() + s[i + 1].ToString(), k);
                    SumOdd += Odd;

                    k++;
                }

                if (SumOdd >= Min && SumOdd <= Max)
                {
                    ListStr.Add(s);
                }
            }

            return ListStr;
        }

        //指数过滤的(指数积)
        List<string> GetIndexProduct(double Min, double Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            double ProductOdd = 1.00;
            double Odd = 0.00;

            int k = 0;
            foreach (string s in ListR)
            {
                ProductOdd = 1.00;
                k = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    Odd = ReturnOdds(s[i].ToString() + s[i + 1].ToString(), k);
                    ProductOdd *= Odd;

                    k++;
                }

                if (ProductOdd >= Min && ProductOdd <= Max)
                {
                    ListStr.Add(s);
                }
            }

            return ListStr;
        }

        //指数过滤的(奖金范围)
        List<string> GetIndexBonus(double Min, double Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            double ProductOdd = 1.00;
            double Odd = 0.00;

            int k = 0;
            foreach (string s in ListR)
            {
                ProductOdd = 1.00;
                k = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    Odd = ReturnOdds(s[i].ToString() + s[i + 1].ToString(), k);
                    ProductOdd *= Odd;

                    k++;
                }

                if (ProductOdd * 2 >= Min && ProductOdd * 2 <= Max)
                {
                    ListStr.Add(s);
                }
            }

            return ListStr;
        }

        //指数过滤的(最低SP命中)
        List<string> GetMinSP(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            List<string> ListTemp = new List<string>();

            ListTemp = ReturnMostOdds(1);//1取赔率最小的单号
            int j = 0;
            int Count = 0;
            foreach (string s in ListR)
            {
                Count = 0;
                j = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    if (s[i].ToString() + s[i + 1].ToString() == ListTemp[j].ToString())
                    {
                        Count++;
                    }
                    j++;
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //指数过滤的(最高SP命中)
        List<string> GetMaxSP(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            List<string> ListTemp = new List<string>();

            ListTemp = ReturnMostOdds(2);//1取赔率最小的单号 2赔率取最大的单号
            int j = 0;
            int Count = 0;
            foreach (string s in ListR)
            {
                Count = 0;
                j = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    if (s[i].ToString() + s[i + 1].ToString() == ListTemp[j].ToString())
                    {
                        Count++;
                    }
                    j++;
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //排序截取的(赔率高到低排序、赔率低到高排序)Types =1 是赔率高到低，2是赔率低到高
        List<string> GetOrder(int Min, int Max, int Types, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            List<NumOrders> ListNumOrder = new List<NumOrders>();

            double ProductOdd = 1.00;
            double Odd = 0.00;

            int k = 0;
            foreach (string s in ListR)
            {
                NumOrders Orders = new NumOrders();
                ProductOdd = 1.00;
                k = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    Odd = ReturnOdds(s[i].ToString() + s[i + 1].ToString(), k);
                    ProductOdd *= Odd;

                    k++;
                }

                Orders.Nums = s;
                Orders.Odds = ProductOdd;

                ListNumOrder.Add(Orders);//ListNumOrder里放的是 投注号码和号码赔率
            }

            ListNumOrder.Sort(delegate(NumOrders order1, NumOrders order2) { return Comparer<double>.Default.Compare(order1.Odds, order2.Odds); });
            if (Types == 1)
            {
                int j = 1;
                for (int i = ListNumOrder.Count - 1; i >= 0; i--)
                {
                    if (j >= Min && j <= Max)
                    {
                        ListStr.Add(ListNumOrder[i].Nums);
                    }
                    j++;
                }
            }
            else
            {
                for (int i = 0; i < ListNumOrder.Count; i++)
                {
                    if (i + 1 >= Min && i + 1 <= Max)
                    {
                        ListStr.Add(ListNumOrder[i].Nums);
                    }
                }
            }
            return ListStr;
        }

        //范围截取的（随机截取）
        List<string> GetRangeRandom(int Min, List<string> ListR)
        {
            Random rand = new Random();

            List<string> ListStr = new List<string>();
            string NewNum = string.Empty;

            if (Min > ListR.Count)
            {
                Min = ListR.Count;
            }

            while (ListStr.Count < Min)
            {
                NewNum = ListR[rand.Next(ListR.Count)].ToString();
                bool b = ListStr.Contains(NewNum);
                if (!b) { ListStr.Add(NewNum); }
            }
            return ListStr;
        }

        //范围截取的(奖金最高、概率最高)Types = 1 是奖金最高 Types = 2是概率最高
        List<string> GetRangeBonus(int Min, int Types, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            List<NumOrders> ListNumOrder = new List<NumOrders>();

            double ProductOdd = 1.00;
            double Odd = 0.00;

            int k = 0;
            foreach (string s in ListR)
            {
                NumOrders Orders = new NumOrders();
                ProductOdd = 1.00;
                k = 0;
                for (int i = 0; i < s.Length; i += 2)
                {
                    Odd = ReturnOdds(s[i].ToString() + s[i + 1].ToString(), k);
                    ProductOdd *= Odd;

                    k++;
                }

                Orders.Nums = s;
                Orders.Odds = ProductOdd;

                ListNumOrder.Add(Orders);//ListNumOrder里放的是 投注号码和号码赔率
            }

            ListNumOrder.Sort(delegate(NumOrders order1, NumOrders order2) { return Comparer<double>.Default.Compare(order1.Odds, order2.Odds); });
            if (Types == 1)
            {
                int j = 1;
                for (int i = ListNumOrder.Count - 1; i >= 0; i--)
                {
                    if (j <= Min)
                    {
                        ListStr.Add(ListNumOrder[i].Nums);
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < ListNumOrder.Count; i++)
                {
                    if (i < Min)
                    {
                        ListStr.Add(ListNumOrder[i].Nums);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return ListStr;
        }

        //命中场次 冷门过滤 叠加过滤
        List<string> GetGoupHitTarget(string terms, List<string> ListR)
        {
            List<string> ListStr = new List<string>();
            string[] arrTerms = terms.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string term in arrTerms)
            {
                string nums = term.Split('|')[0];
                string nums2 = term.Split('|')[1];
                string[] strs = nums.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                List<int> number = new List<int>();
                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i] != "#")
                    {
                        number.Add(i);
                    }
                }

                double Min = double.Parse(nums2.Split('-')[0]);
                double Max = double.Parse(nums2.Split('-')[1]);

                int Count = 0;

                foreach (string s in ListR)
                {
                    Count = 0;
                    string c = string.Empty;
                    bool isOk = false;

                    for (int i = 0; i < number.Count; i++)
                    {
                        c = s.Substring(number[i] * 2, 2);
                        if (c != "**")
                        {
                            if (strs[number[i]].IndexOf(c) % 2 == 0)
                            {
                                Count++;
                            }
                        }
                        else
                        {
                            isOk = true;
                            break;
                        }
                    }

                    //for (int i = 0, j = 0; i < s.Length; i += 2, j++)
                    //{
                    //    c = s.Substring(i, 2);
                    //    if (strs[j].IndexOf(c) % 2 == 0)
                    //    {
                    //        Count++;
                    //    }
                    //}

                    if (Count >= Min && Count <= Max || isOk)
                    {
                        ListStr.Add(s);
                    }
                }
                ListR.Clear();
                ListR.AddRange(ListStr);
                ListStr.Clear();
            }

            ListStr = ListR;

            return ListStr;
        }

        //比分个数
        List<string> GetScoreNumber(string terms, List<string> ListR)
        {

            List<string> ListStr = new List<string>();
            string[] arrTerms = terms.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string term in arrTerms)
            {
                string nums = term.Split('|')[0];
                string nums1 = term.Split('|')[1];
                string nums2 = term.Split('|')[2];
                string[] select = nums1.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] strs = nums.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                List<int> number = new List<int>();
                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i] == "Y")
                    {
                        number.Add(i);
                    }
                }

                double Min = double.Parse(nums2.Split('-')[0]);
                double Max = double.Parse(nums2.Split('-')[1]);

                int Count = 0;

                foreach (string s in ListR)
                {
                    Count = 0;
                    string c = string.Empty;
                    bool isOk = false;

                    for (int i = 0; i < number.Count; i++)
                    {
                        c = s.Substring(number[i] * 2, 2);
                        if (c != "**")
                        {
                            if (select.Contains(c))
                            {
                                Count++;
                            }
                        }
                        else
                        {
                            isOk = true;
                            break;
                        }
                    }


                    if (Count >= Min && Count <= Max || isOk)
                    {
                        ListStr.Add(s);
                    }
                }
                ListR.Clear();
                ListR.AddRange(ListStr);
                ListStr.Clear();
            }

            ListStr = ListR;

            return ListStr;
        }

        //分组
        List<string> GetGoupNumbers(string terms, string type, List<string> ListR)
        {
            List<string> ListStr = new List<string>();
            string[] arrTerms = terms.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string term in arrTerms)
            {
                string nums = term.Split('|')[0];
                string nums2 = term.Split('|')[1];
                string[] strs = nums.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> number = new List<int>();
                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i] == "Y")
                    {
                        number.Add(i);
                    }
                }
                double Min = 0;
                double Max = 0;
                if (type != "进球差值")
                {
                    Min = double.Parse(nums2.Split('-')[0]);
                    Max = double.Parse(nums2.Split('-')[1]);
                }
                else
                {
                    int start1 = nums2.IndexOf("(") + 1;
                    int len1 = nums2.IndexOf(")") - start1;
                    int start2 = nums2.LastIndexOf("(") + 1;
                    int len2 = nums2.LastIndexOf(")") - start2;
                    if (len1 < 0 || len2 < 0)
                    {
                        return ListR;
                    }
                    Min = double.Parse(nums2.Substring(start1, len1));
                    Max = double.Parse(nums2.Substring(start2, len2));
                }

                switch (type)
                {
                    case "分组主胜":
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int count = 0;
                            foreach (int i in number)
                            {
                                string c = t.Substring(i * 2, 2);
                                if (c != "**")
                                {
                                    if (c[0] > c[1] || c.ToLower() == "ww")
                                    {
                                        count++;
                                    }
                                }
                                else
                                {
                                    isOK = true;
                                    break;
                                }
                            }

                            return isOK || (Min <= count && count <= Max);
                        });
                        break;
                    case "分组主平":
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int count = 0;
                            foreach (int i in number)
                            {
                                string c = t.Substring(i * 2, 2);
                                if (c != "**")
                                {
                                    if (c[0] == c[1] || c.ToLower() == "dd")
                                    {
                                        count++;
                                    }
                                }
                                else
                                {
                                    isOK = true;
                                    break;
                                }
                            }

                            return isOK || (Min <= count && count <= Max);
                        });
                        break;
                    case "分组主负":
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int count = 0;
                            foreach (int i in number)
                            {
                                string c = t.Substring(i * 2, 2);
                                if (c != "**")
                                {
                                    if (c[0] < c[1] || c.ToLower() == "ll")
                                    {
                                        count++;
                                    }
                                }
                                else
                                {
                                    isOK = true;
                                    break;
                                }
                            }

                            return isOK || (Min <= count && count <= Max);
                        });
                        break;
                    case "分组大球":
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int count = 0;
                            foreach (int i in number)
                            {
                                string c = t.Substring(i * 2, 2);
                                if (c != "**")
                                {
                                    if (c.ToLower() == "ww"
                                        || c.ToLower() == "dd"
                                        || c.ToLower() == "ll"
                                        )
                                    {
                                        count++;
                                    }
                                    else
                                    {
                                        if (int.Parse(c[0].ToString()) + int.Parse(c[1].ToString()) >= 3)
                                        {
                                            count++;
                                        }
                                    }
                                }
                                else
                                {
                                    isOK = true;
                                    break;
                                }
                            }

                            return isOK || (Min <= count && count <= Max);
                        });
                        break;
                    case "分组小球":
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int count = 0;
                            foreach (int i in number)
                            {
                                string c = t.Substring(i * 2, 2);
                                if (c != "**")
                                {
                                    if (c.ToLower() == "ww"
                                       || c.ToLower() == "dd"
                                       || c.ToLower() == "ll"
                                       )
                                    {
                                        continue;
                                    }
                                    if (int.Parse(c[0].ToString()) + int.Parse(c[1].ToString()) < 3)
                                    {
                                        count++;
                                    }
                                }
                                else
                                {
                                    isOK = true;
                                    break;
                                }
                            }

                            return isOK || (Min <= count && count <= Max);
                        });
                        break;
                    case "分组断点":
                        #region
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int count = 0;
                            string Temp = "xx";
                            for (int i0 = 0; i0 < number.Count; i0++)
                            {
                                int i = number[i0];
                                string c = t.Substring(i * 2, 2);
                                if (i0 > 0)
                                {
                                    Temp = t.Substring(number[i0 - 1] * 2, 2);
                                }

                                if (c.ToLower() == "ww"
                                   || c.ToLower() == "dd"
                                   || c.ToLower() == "ll"
                                    || c == "**"
                                   )
                                {
                                    isOK = true;
                                    break;
                                }

                                if (c[0] != c[1])
                                {
                                    count++;
                                }

                                if (c[0] != Temp[1] && Temp != "xx")
                                {
                                    count++;
                                }
                            }

                            return isOK || (Min <= count && count <= Max);
                        });
                        break;
                        #endregion
                    case "分组和值":
                        #region
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int sum = 0;
                            foreach (int i in number)
                            {
                                string c = t.Substring(i * 2, 2);

                                if (c.ToLower() == "ww"
                                   || c.ToLower() == "dd"
                                   || c.ToLower() == "ll"
                                    || c == "**"
                                   )
                                {
                                    isOK = true;
                                    break;
                                }

                                sum += int.Parse(c[0].ToString()) + int.Parse(c[1].ToString());

                            }

                            return isOK || (Min <= sum && sum <= Max);
                        });
                        break;
                        #endregion
                    case "分组连号":
                        #region
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int count = 0;
                            string Temp = "xx";
                            for (int i0 = 0; i0 < number.Count; i0++)
                            {
                                int i = number[i0];
                                string c = t.Substring(i * 2, 2);
                                if (i0 > 0)
                                {
                                    Temp = t.Substring(number[i0 - 1] * 2, 2);
                                }

                                if (c.ToLower() == "ww"
                                   || c.ToLower() == "dd"
                                   || c.ToLower() == "ll"
                                    || c == "**"
                                   )
                                {
                                    isOK = true;
                                    break;
                                }

                                if (c[0] == c[1])
                                {
                                    count++;
                                }

                                if (c[0] == Temp[1])
                                {
                                    count++;
                                }

                            }

                            return isOK || (Min <= count && count <= Max);
                        });
                        #endregion
                        break;
                    case "分组最长连号":
                        #region
                        ListR = ListR.FindAll(t =>
                        {
                            if (t == "10**2110")
                            {

                            }
                            bool isOK = false;
                            int count = 0;
                            int maxCount = 0;
                            string Temp = "xx";
                            for (int i0 = 0; i0 < number.Count; i0++)
                            {
                                int i = number[i0];
                                string c = t.Substring(i * 2, 2);
                                if (i0 > 0)
                                {
                                    Temp = t.Substring(number[i0 - 1] * 2, 2);
                                }

                                if (c.ToLower() == "ww"
                                   || c.ToLower() == "dd"
                                   || c.ToLower() == "ll"
                                    || c == "**"
                                   )
                                {
                                    isOK = true;
                                    break;
                                }

                                if (c[0] == Temp[1])
                                {
                                    count++;
                                }
                                else
                                {
                                    maxCount = Math.Max(count, maxCount);
                                    count = 0;
                                }

                                if (c[0] == c[1])
                                {
                                    count++;
                                }
                                else
                                {
                                    maxCount = Math.Max(count, maxCount);
                                    count = 0;
                                }


                            }

                            maxCount = Math.Max(count, maxCount);

                            return isOK || (Min <= maxCount && maxCount <= Max);
                        });
                        #endregion
                        break;
                    case "进球差值":
                        #region
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int sum = 0;
                            foreach (int i in number)
                            {
                                string c = t.Substring(i * 2, 2);

                                if (c.ToLower() == "ww"
                                   || c.ToLower() == "dd"
                                   || c.ToLower() == "ll"
                                    || c == "**"
                                   )
                                {
                                    isOK = true;
                                    break;
                                }

                                sum += int.Parse(c[0].ToString()) - int.Parse(c[1].ToString());

                            }

                            return isOK || (Min <= sum && sum <= Max);
                        });
                        #endregion
                        break;
                }


            }

            ListStr = ListR;

            return ListStr;
        }
        #endregion

        //转换为数字，转换失败返回0
        int ConvertInt(string Num)
        {
            int Results = 0;

            int.TryParse(Num, out Results);

            return Results;
        }

        //获取按钮上的赔率值(指数过滤的时候用到)
        double ReturnOdds(string Num, int indexrow)
        {
            double Odds = 0.00;

            int i = 0;
            var selfcols = DataGrid1.Columns[0];//第1列 是首次末的按钮
            foreach (var item in DataGrid1.ItemsSource) //遍历第1列
            {
                if (i == indexrow && Num != "**")
                {
                    Odds = 0.00;
                    var cells = selfcols.GetCellContent(item);
                    if (cells != null)
                    {
                        Grid Grid = cells as Grid;

                        StackPanel Sp = Grid.Children[0] as StackPanel;

                        StackPanel Sp1 = Sp.Children[1] as StackPanel;

                        foreach (Button button1 in (Sp1.Children[0] as StackPanel).Children)
                        {
                            if (button1.Content.ToString() == Num)
                            {
                                Odds = Netball.RetuntDouble(ToolTipService.GetToolTip(button1).ToString());
                                return Odds;
                            }
                        }

                        foreach (Button button2 in (Sp1.Children[1] as StackPanel).Children)
                        {
                            if (button2.Content.ToString() == Num)
                            {
                                Odds = Netball.RetuntDouble(ToolTipService.GetToolTip(button2).ToString());
                                return Odds;
                            }
                        }

                        foreach (Button button3 in (Sp1.Children[2] as StackPanel).Children)
                        {
                            if (button3.Content.ToString() == Num)
                            {
                                Odds = Netball.RetuntDouble(ToolTipService.GetToolTip(button3).ToString());
                                return Odds;
                            }
                        }
                    }
                }
                i++;
            }
            return 0.00;
        }

        //指数过滤的(最低SP命中和最高SP命中)
        List<string> ReturnMostOdds(int MostType)
        {
            List<NumOrders> ListTemp = new List<NumOrders>();

            List<string> ListStr = new List<string>();

            var selfcols = DataGrid1.Columns[0];//第1列 是首次末的按钮
            foreach (var item in DataGrid1.ItemsSource) //遍历第1列
            {
                ListTemp.Clear();
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    Grid Grid = cells as Grid;

                    StackPanel Sp = Grid.Children[0] as StackPanel;

                    StackPanel Sp1 = Sp.Children[1] as StackPanel;

                    foreach (Button button1 in (Sp1.Children[0] as StackPanel).Children)
                    {
                        SolidColorBrush b_Brush = (SolidColorBrush)(button1.Background);
                        if (b_Brush.Color.ToString() == "#FFFF0000")
                        {
                            NumOrders Orders = new NumOrders();

                            Orders.Odds = Netball.RetuntDouble(ToolTipService.GetToolTip(button1).ToString());
                            Orders.Nums = button1.Content.ToString();
                            ListTemp.Add(Orders);
                        }
                    }

                    foreach (Button button2 in (Sp1.Children[1] as StackPanel).Children)
                    {
                        SolidColorBrush b_Brush = (SolidColorBrush)(button2.Background);
                        if (b_Brush.Color.ToString() == "#FFFF0000")
                        {
                            NumOrders Orders = new NumOrders();

                            Orders.Odds = Netball.RetuntDouble(ToolTipService.GetToolTip(button2).ToString());
                            Orders.Nums = button2.Content.ToString();
                            ListTemp.Add(Orders);
                        }
                    }

                    foreach (Button button3 in (Sp1.Children[2] as StackPanel).Children)
                    {
                        SolidColorBrush b_Brush = (SolidColorBrush)(button3.Background);
                        if (b_Brush.Color.ToString() == "#FFFF0000")
                        {
                            NumOrders Orders = new NumOrders();

                            Orders.Odds = Netball.RetuntDouble(ToolTipService.GetToolTip(button3).ToString());
                            Orders.Nums = button3.Content.ToString();
                            ListTemp.Add(Orders);
                        }
                    }
                }

                ListTemp.Sort(delegate(NumOrders order1, NumOrders order2) { return Comparer<double>.Default.Compare(order1.Odds, order2.Odds); });

                if (MostType == 1)
                {
                    ListStr.Add(ListTemp.First().Nums.ToString());//取赔率最小的单号
                }
                else if (MostType == 2)
                {
                    ListStr.Add(ListTemp.Last().Nums.ToString());//取赔率最小的单号
                }
            }
            return ListStr;
        }

        //得到选中的号码（传到子窗体的方法）
        public List<List<string>> getSelectNum()
        {
            List<List<string>> Result = new List<List<string>>();
            List<string> Result2 = null;

            var selfcols = this.DataGrid1.Columns[0];

            string First = string.Empty;

            foreach (var item in DataGrid1.ItemsSource)
            {
                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);

                if (cells != null)
                {
                    Result2 = new List<string>();
                    //--单元格所包含的元素
                    Grid grid = cells as Grid;

                    StackPanel Sp = grid.Children[0] as StackPanel;

                    StackPanel Sp1 = Sp.Children[1] as StackPanel;

                    foreach (StackPanel Sp2 in Sp1.Children)
                    {
                        foreach (Button b in Sp2.Children)
                        {
                            SolidColorBrush Brush = (SolidColorBrush)(b.Background);
                            if (Brush.Color.ToString() == "#FFFF0000")
                            {
                                Result2.Add(b.Content.ToString());
                            }
                        }
                    }
                    Result.Add(Result2);
                }
            }
            return Result;
        }

        private void btnModel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.spCondition.Children.Count == 0)//没有设置筛选条件
            {
                MessageBox.Show("还没有设置筛选条件！");

                return;
            }

            if (UserID < 1)
            {
                login.Show(); //登录窗口

                return;
            }

            string ProcessType = string.Empty;//处理类型                
            string TempStr = string.Empty;

            saveMobile.ModlePlayTypeName = "竞彩比分";
            saveMobile.UserID = UserID;

            saveMobile.TypeName = BuyWays;
            saveMobile.PlayType = "7203";

            saveMobile.dictionary = new Dictionary<int, List<string>>();

            List<string> tempDictionary1 = new List<string>();
            List<string> tempDictionary2 = new List<string>();
            List<string> tempDictionary3 = new List<string>();
            List<string> tempDictionary4 = new List<string>();
            List<string> tempDictionary5 = new List<string>();
            List<string> tempDictionary6 = new List<string>();
            List<string> tempDictionary7 = new List<string>();

            for (int i = 0; i < this.spCondition.Children.Count; i++)
            {
                StackPanel StackPanel = this.spCondition.Children[i] as StackPanel;

                ProcessType = (StackPanel.Children[0] as TextBox).Text.ToString();

                int Min = 0;
                int Max = 0;

                double MinOdds = 0.00;
                double MaxOdds = 0.00;

                string term = string.Empty;

                switch (ProcessType)
                {
                    case "1"://常规过滤和首次末过滤的所有过滤条件
                        Min = int.Parse((StackPanel.Children[2] as ComboBox).SelectedItem.ToString());
                        Max = int.Parse((StackPanel.Children[4] as ComboBox).SelectedItem.ToString());

                        TempStr = (StackPanel.Children[3] as TextBlock).Text;
                        TempStr = TempStr.Substring(4, TempStr.Length - 8);
                        break;
                    case "2"://点击指数过滤的（指数和、指数积）                           
                        double.TryParse((StackPanel.Children[2] as TextBox).Text, out MinOdds);
                        double.TryParse((StackPanel.Children[6] as TextBox).Text, out MaxOdds);

                        TempStr = (StackPanel.Children[5] as TextBlock).Text;
                        TempStr = TempStr.Substring(0, 3);
                        break;
                    case "3"://点击指数过滤的（奖金范围）
                        double.TryParse((StackPanel.Children[2] as TextBox).Text, out MinOdds);
                        double.TryParse((StackPanel.Children[4] as TextBox).Text, out MaxOdds);

                        TempStr = (StackPanel.Children[3] as TextBlock).Text;
                        TempStr = TempStr.Substring(3, TempStr.Length - 7);
                        break;
                    case "4"://点击指数过滤的(最低SP命中，最高SP命中)
                        Min = int.Parse((StackPanel.Children[2] as ComboBox).SelectedItem.ToString());
                        Max = int.Parse((StackPanel.Children[6] as ComboBox).SelectedItem.ToString());

                        TempStr = (StackPanel.Children[5] as TextBlock).Text;
                        TempStr = TempStr.Substring(0, TempStr.Length - 4);
                        break;
                    case "5"://排序截取的(赔率高到低，赔率低到高)
                        Min = int.Parse((StackPanel.Children[4] as TextBox).Text);
                        Max = int.Parse((StackPanel.Children[6] as TextBox).Text);

                        TempStr = (StackPanel.Children[3] as TextBlock).Text;
                        TempStr = TempStr.Substring(0, TempStr.Length - 1);
                        break;
                    case "6"://范围截取的(随机截取，奖金最高，概率最高)
                        Min = int.Parse((StackPanel.Children[3] as TextBox).Text);

                        TempStr = (StackPanel.Children[2] as TextBlock).Text;
                        break;

                    case "7"://集合过滤的(命中场次，冷门过滤，叠加过滤)
                        term = (StackPanel.Children[5] as TextBox).Text;
                        TempStr = (StackPanel.Children[2] as TextBlock).Text;
                        break;
                }

                if (string.IsNullOrEmpty(term) && ProcessType == "7")
                {
                    continue;
                }

                switch (TempStr)
                {
                    case "0的个数":
                    case "1的个数":
                    case "2的个数":
                    case "3的个数":
                    case "4的个数":
                    case "0的最大连续":
                    case "1的最大连续":
                    case "2的最大连续":
                    case "3的最大连续":
                    case "4的最大连续":
                    case "主胜场数":
                    case "主平场数":
                    case "主负场数":
                    case "大球个数":
                    case "小球个数":
                    case "和值区间":
                    case "断点区间":
                        tempDictionary1.Add(TempStr + "@" + Min.ToString() + "_" + Max.ToString());
                        break;
                    case "指数和":
                    case "指数积":
                        tempDictionary2.Add(TempStr + "@" + MinOdds.ToString() + "_" + MaxOdds.ToString());
                        break;
                    case "奖金范围":
                        tempDictionary3.Add(TempStr + "@" + term);
                        break;
                    case "最低SP命中":
                    case "最高SP命中":
                        tempDictionary4.Add(TempStr + "@" + Min.ToString() + "_" + Max.ToString());
                        break;
                    case "赔率高到低排序取第":
                    case "赔率底到高排序取第":
                        tempDictionary5.Add(TempStr + "@" + Min.ToString() + "_" + Max.ToString());
                        break;
                    case "随机截取":
                    case "奖金最高":
                    case "概率最高":
                        tempDictionary6.Add(TempStr + "@" + Min.ToString());
                        break;
                    case "命中场次":
                    case "冷门过滤":
                    case "叠加过滤":
                    case "比分个数":
                    case "分组主胜":
                    case "分组主平":
                    case "分组主负":
                    case "分组大球":
                    case "分组小球":
                    case "分组断点":
                    case "分组和值":
                    case "分组连号":
                    case "分组最长连号":
                    case "进球差值":
                        tempDictionary7.Add(TempStr + "@" + term);
                        break;
                }
            }

            if (tempDictionary1.Count > 0)
            {
                saveMobile.dictionary.Add(1, tempDictionary1);
            }

            if (tempDictionary2.Count > 0)
            {
                saveMobile.dictionary.Add(2, tempDictionary2);
            }

            if (tempDictionary3.Count > 0)
            {
                saveMobile.dictionary.Add(3, tempDictionary3);
            }

            if (tempDictionary4.Count > 0)
            {
                saveMobile.dictionary.Add(4, tempDictionary4);
            }

            if (tempDictionary5.Count > 0)
            {
                saveMobile.dictionary.Add(5, tempDictionary5);
            }

            if (tempDictionary6.Count > 0)
            {
                saveMobile.dictionary.Add(6, tempDictionary6);
            }

            if (tempDictionary7.Count > 0)
            {
                saveMobile.dictionary.Add(7, tempDictionary7);
            }

            saveMobile.Show();
        }

        private void btnMyModel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (UserID < 1)
            {
                login.Show();

                return;
            }

            modelManage.UserID = UserID;
            modelManage.PlayTypeID = "7203";
            modelManage.TypeName = BuyWays;
            modelManage.Show();
        }

        private void modelManage_Closed(object sender, EventArgs e)
        {
            if ((bool)modelManage.DialogResult)
            {
                modelShow.ID = modelManage.ModelID;
                modelShow.Show();
            }
        }

        private void modelShow_Closed(object sender, EventArgs e)
        {
            if (!(bool)modelShow.DialogResult)
            {
                modelManage.Show();
                return;
            }

            T_Model Model = modelShow.Model;

            if (Model == null)
            {
                return;
            }

            string[] strContent = Model.Content.Split('b');

            foreach (string str in strContent)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }

                string Type = str.Substring(0, str.IndexOf('a'));

                string tempstr = str.Substring(str.IndexOf('a') + 1);

                string[] strCondent = tempstr.Split(']');

                if (strCondent.Length < 1)
                {
                    continue;
                }

                for (int j = 0; j < strCondent.Length; j++)
                {
                    if (string.IsNullOrEmpty(strCondent[j]))
                    {
                        continue;
                    }

                    string[] strResult = strCondent[j].Split('@');

                    if (strResult.Length < 2)
                    {
                        continue;
                    }

                    int Min = 0;
                    int Max = 0;
                    string Minodds = "";
                    string Maxodds = "";

                    switch (Type)
                    {
                        case "1":
                            Min = int.Parse(strResult[1].Split('_')[0]);
                            Max = int.Parse(strResult[1].Split('_')[1]);
                            AddRoutineThree(strResult[0].Replace("[", ""), Min, Max);
                            break;
                        case "2":
                            Minodds = strResult[1].Split('_')[0];
                            Maxodds = strResult[1].Split('_')[1];
                            AddIndexSum(strResult[0].Replace("[", ""), Minodds, Maxodds);
                            break;
                        case "3":
                            Minodds = strResult[1].Split('_')[0];
                            Maxodds = strResult[1].Split('_')[1];
                            AddIndexBonus(strResult[0].Replace("[", ""), Minodds, Maxodds);
                            break;
                        case "4":
                            Min = int.Parse(strResult[1].Split('_')[0]);
                            Max = int.Parse(strResult[1].Split('_')[1]);
                            AddIndexMost(strResult[0].Replace("[", ""), Min, Max);
                            break;
                        case "5":
                            Min = int.Parse(strResult[1].Split('_')[0]);
                            Max = int.Parse(strResult[1].Split('_')[1]);
                            AddOrderDesc(strResult[0].Replace("[", ""), Min, Max);
                            break;
                        case "6":
                            AddRangeRandom(strResult[0].Replace("[", ""), strResult[1]);
                            break;
                        case "7":
                            AddGroupSum(strResult[0].Replace("[", ""), strResult[1]);
                            break;
                        default:
                            break;
                    }
                }
            }

            modelShow.Close();
        }

        private void login_Closed(object sender, EventArgs e)
        {
            if ((bool)login.DialogResult)
            {
                try
                {
                    HtmlElement input = HtmlPage.Document.GetElementById("hinUserID");

                    UserID = long.Parse(input.GetAttribute("value"));
                }
                catch
                {
                    UserID = -1;
                }
            }
        }
    }
}