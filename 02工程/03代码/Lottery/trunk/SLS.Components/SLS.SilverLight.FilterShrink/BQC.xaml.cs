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
using System.Windows.Threading;
using SLS.SilverLight.FilterShrink.ServiceReference1;
using System.Text.RegularExpressions;
using SLS.SilverLight.FilterShrink.Model;
using System.Windows.Data;
using System.ServiceModel;
using System.Windows.Browser;
using SLS.SilverLight.FilterShrink.ServiceModel;

namespace SLS.SilverLight.FilterShrink
{
    public partial class BQC : UserControl
    {
        #region 全局变量
        string[] GameNumber = null;//得到场次信息id和胜平负信息（全局变量）
        string BuyWays = "";
        int Mulitpe = 1;
        long UserID = 0;

        List<Netball> NetBallList = new List<Netball>();//起始页面加载在datagrid的数据集合
        static List<string> ListResult = new List<string>();//(处理后的全部数据)全局变量

        DispatcherTimer Timer;

        List<DataGridRow> rows = new List<DataGridRow>();
        List<LotteryNum> Num = new List<LotteryNum>();  //过滤之后的数据

        #region 子窗体
        BQCChildWindow.bqcgroup bqcGroup = new SLS.SilverLight.FilterShrink.BQCChildWindow.bqcgroup();//分组过滤
        BQCChildWindow.bqcindexfilter bqcIndex = new SLS.SilverLight.FilterShrink.BQCChildWindow.bqcindexfilter();
        BQCChildWindow.bqcgather bqcgather = new SLS.SilverLight.FilterShrink.BQCChildWindow.bqcgather();

        ModelChildWindow.SaveChildWindow saveMobile = new SLS.SilverLight.FilterShrink.ModelChildWindow.SaveChildWindow();
        ModelChildWindow.ModelManage modelManage = new SLS.SilverLight.FilterShrink.ModelChildWindow.ModelManage();
        ModelChildWindow.ModelShow modelShow = new SLS.SilverLight.FilterShrink.ModelChildWindow.ModelShow();
        ModelChildWindow.login login = new SLS.SilverLight.FilterShrink.ModelChildWindow.login();
        #endregion

        #endregion

        public BQC()
        {
            InitializeComponent();

            #region 条件篮的主导航
            btnRoutine.Click += new RoutedEventHandler(btnRoutine_Click);
            btnFrist.Click += new RoutedEventHandler(btnRoutine_Click);
            btnOptional.Click += new RoutedEventHandler(btnRoutine_Click);
            btnGroup.Click += new RoutedEventHandler(btnRoutine_Click);
            btnGather.Click += new RoutedEventHandler(btnRoutine_Click);
            btnIndex.Click += new RoutedEventHandler(btnRoutine_Click);
            btnOrder.Click += new RoutedEventHandler(btnRoutine_Click);
            btnRange.Click += new RoutedEventHandler(btnRoutine_Click);
            //btnShrink.Click += new RoutedEventHandler(btnRoutine_Click);
            #endregion

            #region 条件篮的次导航
            //常规过滤
            btnRoutineThree.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineOne.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineZero.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineSum.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutinePoint.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineJoin.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineWin.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineLevel.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineLose.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineNoLoseNum.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineNoLevelNum.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnRoutineNoWinNum.Click += new RoutedEventHandler(btnRoutineThree_Click);

            //首次末选            
            btnFristBCMainS.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristBCMainP.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristBCMainF.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristQCMainS.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristQCMainP.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristQCMainF.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristBQSame.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristBQDiffer.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristReverse.Click += new RoutedEventHandler(btnRoutineThree_Click);

            //选项过滤
            //btnOptionalAll.Click += new RoutedEventHandler(btnOptional_Click);//选项全部
            btnOptional33.Click += new RoutedEventHandler(btnOptional_Click);//33的个数
            btnOptional31.Click += new RoutedEventHandler(btnOptional_Click);//31的个数
            btnOptional30.Click += new RoutedEventHandler(btnOptional_Click);//30的个数
            btnOptional13.Click += new RoutedEventHandler(btnOptional_Click);//13的个数
            btnOptional11.Click += new RoutedEventHandler(btnOptional_Click);//11的个数
            btnOptional10.Click += new RoutedEventHandler(btnOptional_Click);//10的个数
            btnOptional03.Click += new RoutedEventHandler(btnOptional_Click);//03的个数
            btnOptional01.Click += new RoutedEventHandler(btnOptional_Click);//01的个数
            btnOptional00.Click += new RoutedEventHandler(btnOptional_Click);//00的个数

            //指数过滤
            btnIndexSum.Click += new RoutedEventHandler(btnIndexSum_Click);//指数和
            btnIndexProduct.Click += new RoutedEventHandler(btnIndexSum_Click);//指数积
            btnIndexBonus.Click += new RoutedEventHandler(btnIndexBonus_Click);//奖金范围
            btnIndexFrist.Click += new RoutedEventHandler(btnIndexFrist_Click);//第一二三指数命中
            btnIndexSecond.Click += new RoutedEventHandler(btnIndexFrist_Click);//第四五六指数命中
            btnIndexThree.Click += new RoutedEventHandler(btnIndexFrist_Click);//第七八九指数命中

            //排序截取
            btnOrderDesc.Click += new RoutedEventHandler(btnOrderDesc_Click);//赔率高到低排序
            btnOrderAsc.Click += new RoutedEventHandler(btnOrderDesc_Click);//赔率低到高排序

            //范围截取
            btnRangeRandom.Click += new RoutedEventHandler(btnRangeRandom_Click);//随机截取
            btnRangeBonus.Click += new RoutedEventHandler(btnRangeRandom_Click);//奖金最高
            btnRangeProbability.Click += new RoutedEventHandler(btnRangeRandom_Click);//概率最高


            #endregion

            #region 条件篮的全删按钮
            btnDelAllCondition.Click += new RoutedEventHandler(btnDelAllCondition_Click);
            #endregion

            //子窗体关闭事件
            bqcGroup.Closed += new EventHandler(bqcGroup_Closed);
            bqcIndex.Closed += new EventHandler(bqcIndex_Closed);
            bqcgather.Closed += new EventHandler(bqcgather_Closed);

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

            //加载球赛信息 “场次，主场球队，客场球队，让球，赔率等”，加载到datagrid
            BindNetBall();

            //处理按钮
            btnProcess.Click += new RoutedEventHandler(btnProcess_Click);
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
            else if (ConvertInt(GameTypeCount) < List.Count)
            {
                int Difference = List.Count - int.Parse(GameTypeCount);

                ListResult = ListResult.FindAll(t => t.Split(new string[] { "*" }, StringSplitOptions.None).Length == Difference + 1);
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
                        ball.SimpleNum = PassRate[k].MatchNumber.Substring(PassRate[k].MatchNumber.Length - 2, 2);
                        ball.HomeField = PassRate[k].MainTeam;
                        ball.VisitingField = PassRate[k].GuestTeam;
                        ball.SS = (double)PassRate[k].SS;
                        ball.SP = (double)PassRate[k].SP;
                        ball.SF = (double)PassRate[k].SF;
                        ball.PS = (double)PassRate[k].PS;
                        ball.PP = (double)PassRate[k].PP;
                        ball.PF = (double)PassRate[k].PF;
                        ball.FS = (double)PassRate[k].FS;
                        ball.FP = (double)PassRate[k].FP;
                        ball.FF = (double)PassRate[k].FF;

                        ball.Id = PassRate[k].Id;

                        break;
                    }
                }
                #endregion
                NetBallList.Add(ball);
            }

            this.DataGrid1.ItemsSource = NetBallList;
            this.DataGrid3.ItemsSource = NetBallList;

            //延时初始化指数和、指数积、奖金
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }

        //初始化指数和，指数积，奖金范围
        void Timer_Tick(object sender, EventArgs e)
        {
            CountOdds();

            Timer.Stop();
        }

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

        //datagrid行加载事件
        private void DataGrid1_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            // TODO: Add event handler implementation here.
            Grid gd = DataGrid1.Columns[3].GetCellContent(e.Row) as Grid;

            string TempName = string.Empty;

            TempName = GameNumber[e.Row.GetIndex()].ToString();

            string[] ZhuArr = TempName.Substring(TempName.IndexOf('(') + 1, TempName.LastIndexOf(')') - TempName.IndexOf('(') - 1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string ButtonContent = string.Empty;
            foreach (string s in ZhuArr)
            {
                ButtonContent = Netball.GetHalfAllField(s);
                foreach (Button b in gd.Children)
                {
                    if (b.Content.ToString() == ButtonContent)
                    {
                        b.Background = new SolidColorBrush(Colors.Red);
                        break;
                    }
                }
            }
        }

        //进球数量的小按钮事件
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            SolidColorBrush b_Brush = (SolidColorBrush)(button.Background);

            int ColorCount = 0;
            if (b_Brush.Color.ToString() == "#FFFF0000")
            {

                Grid gd = button.Parent as Grid;
                foreach (Button b in gd.Children)
                {
                    SolidColorBrush b_Brush2 = (SolidColorBrush)(b.Background);
                    if (b_Brush2.Color.ToString() == "#FFFF0000")
                    {
                        ColorCount++;
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

            CountOdds();
        }

        private void btnChoose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            HtmlWindow html = HtmlPage.Window;
            html.Navigate(new Uri("../JCZC/Buy_ZJQS.aspx", UriKind.Relative), "_self");
        }

        private void SuBmit_Click(object sender, System.Windows.RoutedEventArgs e)
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
                LotteryNumber = "7204;[";

                for (int j = 0; j < GameNumber.Length; j++)
                {
                    if (Num[i].LotteryNums.Substring(j * 2, 2).Equals("**"))
                    {
                        continue;
                    }

                    LotteryNumber += GameNumber[j].Substring(0, GameNumber[j].IndexOf('(')) + "(" + Netball.GetHalfAllScore(Num[i].LotteryNums.Substring(j * 2, 2)) + ")|";
                }

                if (LotteryNumber.EndsWith("|"))
                {
                    LotteryNumber = LotteryNumber.Substring(0, LotteryNumber.Length - 1);
                }

                LotteryNumber += "];[" + BuyWays + Mulitpe.ToString() + "]";

                str += LotteryNumber + "\n\r";
                LotteryNumbers[i] = LotteryNumber;
            }

            WebClient Appclient = new WebClient();

            try
            {
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

        //条件篮的全部删除事件
        private void btnDelAllCondition_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            spCondition.Children.Clear();
            spCondition.Height = 250;
        }

        #region 点击过滤按钮加载条件
        //点击常规过滤或者首次末选的条件按钮 操作类型1
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

            //添加一个删除的图片
            //Image image = new Image();
            //image.Source = "../image/";

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

            List<Netball> List = DataGrid1.ItemsSource as List<Netball>; //DataGrid1.ItemsSource as List<Netball>;

            #region 设置下拉框里的数据内容

            int Count = 0;
            //根据选择不同的过滤条件，下拉框的数值不一样
            switch (StrName)
            {
                case "3的个数":
                case "1的个数":
                case "0的个数":
                    Count = List.Count * 2;
                    break;
                case "号码和值":
                    Count = List.Count * 3 * 2;
                    break;
                case "断点个数":
                case "连号个数":
                    Count = List.Count * 2 - 1;
                    break;
                case "主场连胜":
                case "主场连平":
                case "主场连负":
                    Count = List.Count * 2;
                    break;
                case "不败连续个数":
                case "不平连续个数":
                case "不胜连续个数":
                    Count = List.Count * 2;
                    break;
                case "半场主胜个数":
                case "半场主平个数":
                case "半场主负个数":
                case "全场主胜个数":
                case "全场主平个数":
                case "全场主负个数":
                case "半全相同个数":
                case "半全不同个数":
                case "逆转场次个数":
                    Count = List.Count;
                    break;
                default:
                    Count = List.Count * 2;
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
        private void btnIndexSum_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;
            AddIndexSum(NowButton.Content.ToString(), "", "");


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
        private void btnIndexBonus_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;
            AddIndexBonus(NowButton.Content.ToString(), "", "");
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

        //点击指数过滤的(第一指数命中、第二指数命中、第三指数命中) 操作类型4
        private void btnIndexFrist_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;
            AddIndexFrist(NowButton.Content.ToString(), 0, 0);
        }

        void AddIndexFrist(string StrName, int Min, int Max)
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

            List<Netball> List = DataGrid1.ItemsSource as List<Netball>;
            for (int i = 0; i <= List.Count; i++)
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
        private void btnOrderDesc_Click(object sender, RoutedEventArgs e)
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
        private void btnRangeRandom_Click(object sender, RoutedEventArgs e)
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

        //选项过滤的
        private void btnOptional_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //当前按钮对象
            Button NowButton = sender as Button;
            AddOptional(NowButton.Content.ToString(), 0, 0);
        }

        void AddOptional(string StrName, int Min, int Max)
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

            //添加一个删除的图片
            //Image image = new Image();
            //image.Source = "../image/";

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

            List<Netball> List = DataGrid1.ItemsSource as List<Netball>; //DataGrid1.ItemsSource as List<Netball>;

            #region 设置下拉框里的数据内容

            int Count = 0;
            //根据选择不同的过滤条件，下拉框的数值不一样
            Count = List.Count;

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

        //分组过滤的（分组和值、分组断点、分组最长连号、分组指数和值、分组指数积值）
        private void btnGroupSum_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
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
            txtHid.Text = term;

            //隐藏域，存放条件内容
            TextBox txt2 = new TextBox();
            txt2.Visibility = Visibility.Collapsed;

            sp.Children.Add(txtHid);//隐藏域
            sp.Children.Add(bt);//删除按钮
            sp.Children.Add(tb1);//TextBlock显示
            sp.Children.Add(txt1);//第一个文本框
            sp.Children.Add(bt1);//"设置"按钮
            sp.Children.Add(txt2);

            spCondition.Children.Add(sp);

            if (spCondition.Children.Count > 8)
            {
                spCondition.Height += 30;
            }
        }

        #endregion

        #region 打开子窗体的事件代码
        void bt1_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            Button btn = sender as Button;
            StackPanel sp = btn.Parent as StackPanel;
            string Title = (sp.Children[2] as TextBlock).Text.ToString();

            switch (Title)
            {
                case "分组和值":
                case "分组断点":
                case "分组最长连号":
                    bqcGroup.Title = Title;
                    bqcGroup.OverlayBrush = new SolidColorBrush(Colors.White);
                    bqcGroup.Opacity = 1;
                    bqcGroup.HasCloseButton = true;
                    bqcGroup.Foreground = new SolidColorBrush(Colors.Red);
                    bqcGroup.FontSize = 14;
                    bqcGroup.OverlayOpacity = 0.5;
                    //group.IsEnabled = false; 
                    bqcGroup.ListStr = (sp.Children[5] as TextBox).Text.ToString();

                    bqcGroup.Show();//打开子窗体
                    bqcGroup.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
                case "分组指数和值":
                case "分组指数积值":
                    bqcIndex.Title = Title;
                    bqcIndex.OverlayBrush = new SolidColorBrush(Colors.White);
                    bqcIndex.Opacity = 1;
                    bqcIndex.HasCloseButton = true;
                    bqcIndex.Foreground = new SolidColorBrush(Colors.Red);
                    bqcIndex.FontSize = 14;
                    bqcIndex.OverlayOpacity = 0.5;
                    //group.IsEnabled = false; 
                    bqcIndex.ListStr = (sp.Children[5] as TextBox).Text.ToString();

                    bqcIndex.SelectedRate = getSelectOdds();
                    bqcIndex.Show();//打开子窗体
                    bqcIndex.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
                case "命中场次":
                case "冷门过滤":
                case "叠加过滤":
                    bqcgather.Title = Title;
                    bqcgather.OverlayBrush = new SolidColorBrush(Colors.White);
                    bqcgather.Opacity = 1;
                    bqcgather.HasCloseButton = true;
                    bqcgather.Foreground = new SolidColorBrush(Colors.Red);
                    bqcgather.FontSize = 14;
                    bqcgather.OverlayOpacity = 0.5;
                    //group.IsEnabled = false; 
                    bqcgather.ListStr = (sp.Children[5] as TextBox).Text.ToString();

                    bqcgather.SelectedRate = getSelectButton();
                    bqcgather.Show();//打开子窗体
                    bqcgather.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
            }
        }
        #endregion

        #region 子窗体关闭事件代码
        void bqcGroup_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (bqcGroup.DialogResult == true)
            {
                string LbList = string.Format("{0}", bqcGroup.HidResult.Text.ToString());
                (bqcGroup.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (bqcGroup.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }

        void bqcIndex_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (bqcIndex.DialogResult == true)
            {
                string LbList = string.Format("{0}", bqcIndex.HidResult.Text.ToString());
                (bqcIndex.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (bqcIndex.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }

        void bqcgather_Closed(object sender, EventArgs e)
        {
            if (bqcgather.DialogResult == true)
            {
                string LbList = string.Format("{0}", bqcgather.HidResult.Text.ToString());
                (bqcgather.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (bqcgather.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }
        #endregion

        //过滤处理
        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            ListResult.Clear();

            var selfcols = DataGrid1.Columns[3];//第三列 是首次末的按钮

            List<Netball> NetBall = DataGrid1.ItemsSource as List<Netball>;

            string GameTypeCount = "";

            if (!string.IsNullOrEmpty(tbGameType1.Text))
            {
                GameTypeCount = this.tbGameType1.Text.Substring(1, 1).ToString();//几串几，数字
            }

            List<string[]> List = new List<string[]>();//数组集合

            foreach (var item in DataGrid1.ItemsSource) //遍历第三列
            {
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    Grid Grid = cells as Grid;

                    Button bt = new Button();

                    string TempStr = "";

                    for (int i = 0; i < 9; i++)
                    {
                        bt = (Button)Grid.Children[i];
                        SolidColorBrush b_Brush = (SolidColorBrush)(bt.Background);
                        if (b_Brush.Color.ToString() == "#FFFF0000")
                        {
                            TempStr += bt.Content.ToString() + ",";
                        }
                    }

                    TempStr += "*";

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
                    string Reg2 = @"\*";
                    ListResult = ListResult.FindAll(t => !Regex.Match(t, Reg2).Success);
                }
                else
                {
                    int Difference = List.Count - int.Parse(GameTypeCount);

                    string Reg3 = @"\*{" + Difference + "}";

                    ListResult = ListResult.FindAll(t => t.Split('*').Length == (Difference + 1));
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
                        case "4"://点击指数过滤的(第一指数命中、第二指数命中、第三指数命中)
                            Min = ConvertInt((StackPanel.Children[2] as ComboBox).SelectedItem.ToString());
                            Max = ConvertInt((StackPanel.Children[6] as ComboBox).SelectedItem.ToString());

                            TempStr = (StackPanel.Children[5] as TextBlock).Text;
                            TempStr = TempStr.Substring(0, TempStr.Length - 4);
                            break;
                        case "5"://点击排序截取的(赔率高到低排序、赔率低到高排序)
                            Min = ConvertInt((StackPanel.Children[4] as TextBox).Text);
                            Max = ConvertInt((StackPanel.Children[6] as TextBox).Text);

                            TempStr = (StackPanel.Children[3] as TextBlock).Text;
                            TempStr = TempStr.Substring(0, 5);
                            break;
                        case "6"://范围截取的(随机截取、奖金最高、概率最高)
                            Min = ConvertInt((StackPanel.Children[3] as TextBox).Text);

                            TempStr = (StackPanel.Children[2] as TextBlock).Text;
                            break;
                        case "7"://弹出的子窗体
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
                        case "3的个数":
                            TempListStr = GetThreeCounts(Min, Max, "3", TempList);
                            TempList = TempListStr;
                            break;
                        case "1的个数":
                            TempListStr = GetThreeCounts(Min, Max, "1", TempList);
                            TempList = TempListStr;
                            break;
                        case "0的个数":
                            TempListStr = GetThreeCounts(Min, Max, "0", TempList);
                            TempList = TempListStr;
                            break;
                        case "33个数":
                            TempListStr = GetThreeCounts(Min, Max, "33", TempList);
                            TempList = TempListStr;
                            break;
                        case "31个数":
                            TempListStr = GetThreeCounts(Min, Max, "31", TempList);
                            TempList = TempListStr;
                            break;
                        case "30个数":
                            TempListStr = GetThreeCounts(Min, Max, "30", TempList);
                            TempList = TempListStr;
                            break;
                        case "13个数":
                            TempListStr = GetThreeCounts(Min, Max, "13", TempList);
                            TempList = TempListStr;
                            break;
                        case "11个数":
                            TempListStr = GetThreeCounts(Min, Max, "11", TempList);
                            TempList = TempListStr;
                            break;
                        case "10个数":
                            TempListStr = GetThreeCounts(Min, Max, "10", TempList);
                            TempList = TempListStr;
                            break;
                        case "03个数":
                            TempListStr = GetThreeCounts(Min, Max, "03", TempList);
                            TempList = TempListStr;
                            break;
                        case "01个数":
                            TempListStr = GetThreeCounts(Min, Max, "01", TempList);
                            TempList = TempListStr;
                            break;
                        case "00个数":
                            TempListStr = GetThreeCounts(Min, Max, "00", TempList);
                            TempList = TempListStr;
                            break;
                        case "号码和值":
                            TempListStr = GetSumValue(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "断点个数":
                            TempListStr = GetBreakPoint(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "半场主胜个数":
                            TempListStr = GetBQCThreeCounts(Min, Max, "3", 1, TempList);
                            TempList = TempListStr;
                            break;
                        case "半场主平个数":
                            TempListStr = GetBQCThreeCounts(Min, Max, "1", 1, TempList);
                            TempList = TempListStr;
                            break;
                        case "半场主负个数":
                            TempListStr = GetBQCThreeCounts(Min, Max, "0", 1, TempList);
                            TempList = TempListStr;
                            break;
                        case "全场主胜个数":
                            TempListStr = GetBQCThreeCounts(Min, Max, "3", 2, TempList);
                            TempList = TempListStr;
                            break;
                        case "全场主平个数":
                            TempListStr = GetBQCThreeCounts(Min, Max, "1", 2, TempList);
                            TempList = TempListStr;
                            break;
                        case "全场主负个数":
                            TempListStr = GetBQCThreeCounts(Min, Max, "0", 2, TempList);
                            TempList = TempListStr;
                            break;
                        case "半全相同个数":
                            TempListStr = GetBQCThreeCounts(Min, Max, 1, TempList);
                            TempList = TempListStr;
                            break;
                        case "半全不同个数":
                            TempListStr = GetBQCThreeCounts(Min, Max, 2, TempList);
                            TempList = TempListStr;
                            break;
                        case "逆转场次个数":
                            TempListStr = GetBQCReverse(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "连号个数":
                            TempListStr = GetConsecutive(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "主场连胜":
                            TempListStr = GetMainCount(Min, Max, '3', TempList);
                            TempList = TempListStr;
                            break;
                        case "主场连平":
                            TempListStr = GetMainCount(Min, Max, '1', TempList);
                            TempList = TempListStr;
                            break;
                        case "主场连负":
                            TempListStr = GetMainCount(Min, Max, '0', TempList);
                            TempList = TempListStr;
                            break;
                        case "不败连续个数":
                            TempListStr = GetFalseCount(Min, Max, '0', TempList);
                            TempList = TempListStr;
                            break;
                        case "不平连续个数":
                            TempListStr = GetFalseCount(Min, Max, '1', TempList);
                            TempList = TempListStr;
                            break;
                        case "不胜连续个数":
                            TempListStr = GetFalseCount(Min, Max, '3', TempList);
                            TempList = TempListStr;
                            break;
                        case "指数和":
                            TempListStr = GetIndexSumProduct(MinOdds, MaxOdds, 1, TempList);
                            TempList = TempListStr;
                            break;
                        case "指数积":
                            TempListStr = GetIndexSumProduct(MinOdds, MaxOdds, 2, TempList);
                            TempList = TempListStr;
                            break;
                        case "奖金范围":
                            TempListStr = GetIndexBonus(MinOdds, MaxOdds, TempList);
                            TempList = TempListStr;
                            break;
                        case "赔率高到低":
                            TempListStr = GetOrder(Min, Max, 1, TempList);
                            TempList = TempListStr;
                            break;
                        case "赔率底到高":
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
                        case "第一二三指数命中":
                        case "第四五六指数命中":
                        case "第七八九指数命中":
                            TempListStr = GetIndex(Min, Max, TempStr, TempList);
                            TempList = TempListStr;
                            break;
                        case "命中场次":
                        case "冷门过滤":
                        case "叠加过滤":
                            TempListStr = GetGoupHitTarget(term, TempList);
                            TempList = TempListStr;
                            break;
                        case "分组和值":
                        case "分组断点":
                        case "分组最长连号":
                            TempListStr = GetGoupNumbers(term, TempStr, TempList);
                            TempList = TempListStr;
                            break;
                        case "分组指数和值":
                        case "分组指数积值":
                            int type = TempStr == "分组指数和值" ? 1 : 2;
                            TempListStr = GetGoupIndexSumProduct(term, type, TempList);
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

        #region 筛选条件
        //3的个数/1的个数/0的个数
        private List<string> GetThreeCounts(int Min, int Max, string Num, List<string> ListR)
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

        //半场3的个数/1的个数/0的个数
        private List<string> GetBQCThreeCounts(int Min, int Max, string Num, int Type, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            string str = "";

            foreach (string s in ListR)
            {
                Count = 0;

                str = s.Replace("*", "");

                for (int i = 0; i < str.Length; i++)
                {
                    if ((Type == 1) && ((i) % 2 == 0) && str.Substring(i, 1).Equals(Num))
                    {
                        Count++;
                    }
                    else if ((Type == 2) && ((i + 1) % 2 == 0) && str.Substring(i, 1).Equals(Num))
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

        //半全场相同或者不同的个数
        private List<string> GetBQCThreeCounts(int Min, int Max, int Type, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            string str = "";

            foreach (string s in ListR)
            {
                Count = 0;

                str = s.Replace("*", "");

                for (int i = 0; i < str.Length; i = i + 2)
                {
                    if ((Type == 1) && (str.Substring(i, 1) == str.Substring(i + 1, 1)))
                    {
                        Count++;
                    }
                    else if ((Type == 2) && (str.Substring(i, 1) != str.Substring(i + 1, 1)))
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

        //逆转的场次个数
        private List<string> GetBQCReverse(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            string str = "";

            foreach (string s in ListR)
            {
                Count = 0;

                str = s.Replace("*", "");

                for (int i = 0; i < str.Length; i = i + 2)
                {
                    if (str.Substring(i, 1).Equals("1") && str.Substring(i + 1, 1).Equals("3"))
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

        //号码和值
        private List<string> GetSumValue(int Min, int Max, List<string> ListR)
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

        //断点个数
        private List<string> GetBreakPoint(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            foreach (string s in ListR)
            {
                Count = -1;

                char Temp = 'x';
                foreach (char c in s)
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

        //连号个数
        private List<string> GetConsecutive(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            int i = 1;
            char Temp = 'x';

            foreach (string s in ListR)
            {
                Count = 0;
                i = 1;

                foreach (char c in s)
                {

                    if (i == 1)
                    {
                        Temp = c;
                    }
                    else
                    {
                        if (Temp == c)
                        {
                            Count++;
                        }
                        Temp = c;
                    }
                    i++;
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }

            return ListStr;
        }

        //主场连胜，主场连负，主场连平
        private List<string> GetMainCount(int Min, int Max, char Temp, List<string> ListR)
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

        //不败连续个数，不平连续个数，不胜连续个数
        private List<string> GetFalseCount(int Min, int Max, char Temp, List<string> ListR)
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
                    if (c != Temp)
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

        //首选命中/次选命中/末选命中
        private List<string> GetFristHit(int Min, int Max, string FristStr, List<string> ListR)
        {
            List<string> ListStr = new List<string>();
            int i = 0;
            int Count = 0;

            foreach (string s in ListR)
            {
                i = 0;
                Count = 0;
                foreach (char c in s)
                {
                    if (c == FristStr[i])
                    {
                        Count++;
                    }

                    i++;
                }

                if (Count >= Min && Count <= Max)
                {
                    ListStr.Add(s);
                }
            }
            return ListStr;
        }

        //指数过滤的(指数和、指数积)
        private List<string> GetIndexSumProduct(double Min, double Max, int Types, List<string> ListR)
        {
            List<Netball> List = DataGrid1.ItemsSource as List<Netball>;

            List<Odds> ListOdd = new List<Odds>();
            for (int i = 0; i < List.Count; i++)
            {
                //胜平负的赔率类
                Odds Odd = new Odds();

                Odd.Frist = List[i].FristOdds;
                Odd.Next = List[i].NextOdds;
                Odd.Last = List[i].LastOdds;

                Odd.Id = List[i].Id;
                ListOdd.Add(Odd);
            }

            List<string> ListStr = new List<string>();

            int j = 0;
            double Odds = 0.00;
            #region
            foreach (string s in ListR)
            {
                if (Types == 1)//Types = 1指数和
                {
                    foreach (char c in s)
                    {
                        switch (c)
                        {
                            case '3':
                                Odds += ListOdd[j].Frist;
                                break;
                            case '1':
                                Odds += ListOdd[j].Next;
                                break;
                            case '0':
                                Odds += ListOdd[j].Last;
                                break;
                            default:
                                Odds += 0.00;
                                break;
                        }
                        j++;
                    }
                }
                else//指数积
                {
                    Odds = 1.00;
                    foreach (char c in s)
                    {
                        switch (c)
                        {
                            case '3':
                                Odds *= ListOdd[j].Frist;
                                break;
                            case '1':
                                Odds *= ListOdd[j].Next;
                                break;
                            case '0':
                                Odds *= ListOdd[j].Last;
                                break;
                            default:
                                Odds *= 1.00;
                                break;
                        }
                        j++;
                    }
                }
                if (Odds >= Min && Odds <= Max)
                {
                    ListStr.Add(s);
                }

                j = 0;
                Odds = 0.00;
            }
            #endregion

            return ListStr;
        }

        //指数过的的(奖金范围)
        private List<string> GetIndexBonus(double Min, double Max, List<string> ListR)
        {
            List<Netball> List = DataGrid1.ItemsSource as List<Netball>;

            List<Odds> ListOdd = new List<Odds>();
            for (int i = 0; i < List.Count; i++)
            {
                //胜平负的赔率类
                Odds Odd = new Odds();

                Odd.Frist = List[i].FristOdds;
                Odd.Next = List[i].NextOdds;
                Odd.Last = List[i].LastOdds;

                Odd.Id = List[i].Id;
                ListOdd.Add(Odd);
            }

            List<string> ListStr = new List<string>();

            int j = 0;
            double Odds = 1.00;
            #region
            foreach (string s in ListR)
            {
                foreach (char c in s)
                {
                    switch (c)
                    {
                        case '3':
                            Odds *= ListOdd[j].Frist;
                            break;
                        case '1':
                            Odds *= ListOdd[j].Next;
                            break;
                        case '0':
                            Odds *= ListOdd[j].Last;
                            break;
                        default:
                            Odds *= 1.00;
                            break;
                    }
                    j++;
                }
                if (Odds * 2 >= Min && Odds * 2 <= Max)
                {
                    ListStr.Add(s);
                }

                j = 0;
                Odds = 1.00;
            }
            #endregion
            return ListStr;
        }

        //排序截取的(赔率高到低排序、赔率低到高排序)Types =1 是赔率高到低，2是赔率低到高
        private List<string> GetOrder(int Min, int Max, int Types, List<string> ListR)
        {
            List<Netball> List = DataGrid1.ItemsSource as List<Netball>;

            List<Odds> ListOdd = new List<Odds>();
            for (int i = 0; i < List.Count; i++)
            {
                //胜平负的赔率类
                Odds Odd = new Odds();

                Odd.Frist = List[i].FristOdds;
                Odd.Next = List[i].NextOdds;
                Odd.Last = List[i].LastOdds;

                Odd.Id = List[i].Id;
                ListOdd.Add(Odd);
            }

            List<string> ListStr = new List<string>();
            List<NumOrders> ListNumOrder = new List<NumOrders>();

            int j = 0;
            double Odds = 0.00;
            #region
            foreach (string s in ListR)
            {
                NumOrders Orders = new NumOrders();
                foreach (char c in s)
                {
                    switch (c)
                    {
                        case '3':
                            Odds += ListOdd[j].Frist;
                            break;
                        case '1':
                            Odds += ListOdd[j].Next;
                            break;
                        case '0':
                            Odds += ListOdd[j].Last;
                            break;
                        default:
                            Odds += 0.00;
                            break;
                    }
                    j++;
                }

                Orders.Nums = s;
                Orders.Odds = Odds;
                ListNumOrder.Add(Orders);//ListNumOrder里放的是 投注号码和号码赔率

                j = 0;
                Odds = 0.00;
            }
            #endregion

            ListNumOrder.Sort(delegate(NumOrders order1, NumOrders order2) { return Comparer<double>.Default.Compare(order1.Odds, order2.Odds); });
            if (Types == 1)
            {
                int k = 1;
                for (int i = ListNumOrder.Count - 1; i >= 0; i--)
                {
                    if (k >= Min && k <= Max)
                    {
                        ListStr.Add(ListNumOrder[i].Nums);
                    }
                    k++;
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
        private List<string> GetRangeRandom(int Min, List<string> ListR)
        {
            Random rand = new Random();

            List<string> ListStr = new List<string>();
            string NewNum = string.Empty;
            while (ListStr.Count < Min)
            {
                NewNum = ListR[rand.Next(ListR.Count)].ToString();
                bool b = ListStr.Contains(NewNum);
                if (!b) { ListStr.Add(NewNum); }
            }
            return ListStr;
        }

        //范围截取的(奖金最高、 概率最高)Types = 1 是奖金最高 Types = 2是概率最高
        private List<string> GetRangeBonus(int Min, int Types, List<string> ListR)
        {
            List<Netball> List = DataGrid1.ItemsSource as List<Netball>;

            List<Odds> ListOdd = new List<Odds>();
            for (int i = 0; i < List.Count; i++)
            {
                //胜平负的赔率类
                Odds Odd = new Odds();

                Odd.Frist = List[i].FristOdds;
                Odd.Next = List[i].NextOdds;
                Odd.Last = List[i].LastOdds;

                Odd.Id = List[i].Id;
                ListOdd.Add(Odd);
            }

            List<string> ListStr = new List<string>();
            List<NumOrders> ListNumOrder = new List<NumOrders>();

            int j = 0;
            double Odds = 1.00;
            #region
            foreach (string s in ListR)
            {
                NumOrders Orders = new NumOrders();
                foreach (char c in s)
                {
                    switch (c)
                    {
                        case '3':
                            Odds *= ListOdd[j].Frist;
                            break;
                        case '1':
                            Odds *= ListOdd[j].Next;
                            break;
                        case '0':
                            Odds *= ListOdd[j].Last;
                            break;
                        default:
                            Odds *= 1.00;
                            break;
                    }
                    j++;
                }

                Orders.Nums = s;
                Orders.Odds = Odds * 2;
                ListNumOrder.Add(Orders);//ListNumOrder里放的是 投注号码和号码赔率

                j = 0;
                Odds = 1.00;
            }
            #endregion

            ListNumOrder.Sort(delegate(NumOrders order1, NumOrders order2) { return Comparer<double>.Default.Compare(order1.Odds, order2.Odds); });

            if (Types == 1)
            {
                int k = 1;
                for (int i = ListNumOrder.Count - 1; i >= 0; i--)
                {
                    if (k <= Min)
                    {
                        ListStr.Add(ListNumOrder[i].Nums);
                        k++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int n = 0; n < ListNumOrder.Count; n++)
                {
                    if (n < Min)
                    {
                        ListStr.Add(ListNumOrder[n].Nums);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return ListStr;
        }

        // 指数命中
        private List<string> GetIndex(int Min, int Max, string Types, List<string> ListR)
        {
            List<Netball> List = DataGrid1.ItemsSource as List<Netball>;

            List<List<NumOrders2>> listSort = new List<List<NumOrders2>>();
            List<HalfAll> ListOdd = new List<HalfAll>();
            for (int i = 0; i < List.Count; i++)
            {
                HalfAll Odd = new HalfAll();
                List<NumOrders2> Num = new List<NumOrders2>();

                Num.Add(new NumOrders2 { Nums = "1", Odds = Odd.I1 });
                Num.Add(new NumOrders2 { Nums = "2", Odds = Odd.I2 });
                Num.Add(new NumOrders2 { Nums = "3", Odds = Odd.I3 });
                Num.Add(new NumOrders2 { Nums = "4", Odds = Odd.I4 });
                Num.Add(new NumOrders2 { Nums = "5", Odds = Odd.I5 });
                Num.Add(new NumOrders2 { Nums = "6", Odds = Odd.I6 });
                Num.Add(new NumOrders2 { Nums = "7", Odds = Odd.I7 });
                Num.Add(new NumOrders2 { Nums = "8", Odds = Odd.I8 });
                Num.Add(new NumOrders2 { Nums = "9", Odds = Odd.I9 });
                Num.Sort(sortMethond);
                Odd.id = List[i].Id;
                ListOdd.Add(Odd);

                listSort.Add(Num);
            }

            List<string> ListStr = new List<string>();

            int j = 0;
            #region

            string intdex1 = string.Empty;
            string intdex2 = string.Empty;
            string intdex3 = string.Empty;

            List<List<NumOrders2>> listSelect = new List<List<NumOrders2>>();
            foreach (string s in ListR)
            {
                j = 0;
                int count = 0;
                foreach (char c in s)
                {

                    switch (Types)
                    {
                        case "第一二三指数命中":
                            intdex1 = listSort[j][0].Nums;
                            intdex2 = listSort[j][1].Nums;
                            intdex3 = listSort[j][2].Nums;
                            break;

                        case "第四五六指数命中":
                            intdex1 = listSort[j][3].Nums;
                            intdex2 = listSort[j][4].Nums;
                            intdex3 = listSort[j][5].Nums;
                            break;

                        case "第七八九指数命中":
                            intdex1 = listSort[j][6].Nums;
                            intdex2 = listSort[j][7].Nums;
                            intdex3 = listSort[j][8].Nums;
                            break;
                    }

                    if (c == intdex1[0] || c == intdex2[0])
                    {
                        count++;
                    }

                    //累加
                    j++;
                }

                if (count >= Min && count <= Max)
                {
                    ListStr.Add(s);
                }

            }
            #endregion

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
                    case "分组和值":
                        #region
                        ListR = ListR.FindAll(t =>
                        {
                            bool isOK = false;
                            int sum = 0;
                            foreach (int i in number)
                            {
                                string c = t.Substring(i * 2, 2);
                                if (c == "**")
                                {
                                    isOK = true;
                                    break;
                                }

                                sum += int.Parse(c[0].ToString()) + int.Parse(c[1].ToString());
                            }

                            return isOK || (Min <= sum && sum <= Max);
                        });
                        #endregion
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

                                if (c == "**")
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

                    case "分组最长连号":
                        #region
                        ListR = ListR.FindAll(t =>
                        {

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

                                if (c == "**")
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

                }


            }

            ListStr = ListR;

            return ListStr;
        }

        //分组指数过滤的(分组指数和、分组指数积)
        List<string> GetGoupIndexSumProduct(string terms, int Types, List<string> ListR)
        {
            List<Netball> List = DataGrid1.ItemsSource as List<Netball>;

            List<bqc> ListOdd = new List<bqc>();
            for (int i = 0; i < List.Count; i++)
            {
                //胜平负的赔率类
                bqc Odd = new bqc();

                Odd.ss = List[i].SS;
                Odd.sp = List[i].SP;
                Odd.sf = List[i].SF;
                Odd.ps = List[i].PS;
                Odd.pp = List[i].PP;
                Odd.pf = List[i].PF;
                Odd.fs = List[i].FS;
                Odd.fp = List[i].FP;
                Odd.ff = List[i].FF;

                Odd.id = List[i].Id;
                ListOdd.Add(Odd);
            }

            List<string> ListStr = new List<string>();

            string[] arrTerms = terms.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string term in arrTerms)
            {
                string nums = term.Split('|')[0];
                string nums2 = term.Split('|')[2];
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

                double Odds = 0;
                int j = 0;
                foreach (string s in ListR)
                {
                    if (s == "113333")
                    { }

                    bool isOk = false;
                    j = 0;
                    if (Types == 1)//Types = 1指数和
                    {
                        Odds = 0;
                    }
                    else
                    {
                        Odds = 1;
                    }
                    foreach (int i in number)
                    {
                        string str = s.Substring(i * 2, 2).ToLower();
                        if (str != "**")
                        {
                            isOk = false;
                            if (Types == 1)//Types = 1指数和
                            {
                                switch (str)
                                {
                                    case "33":
                                        Odds += ListOdd[i].ss;
                                        break;
                                    case "31":
                                        Odds += ListOdd[i].sp;
                                        break;
                                    case "30":
                                        Odds += ListOdd[i].sf;
                                        break;
                                    case "13":
                                        Odds += ListOdd[i].ps;
                                        break;
                                    case "11":
                                        Odds += ListOdd[i].pp;
                                        break;
                                    case "10":
                                        Odds += ListOdd[i].pf;
                                        break;
                                    case "03":
                                        Odds += ListOdd[i].fs;
                                        break;
                                    case "01":
                                        Odds += ListOdd[i].fp;
                                        break;
                                    case "00":
                                        Odds += ListOdd[i].ff;
                                        break;
                                    default:
                                        Odds += 0.00;
                                        break;
                                }
                                j++;
                            }
                            else
                            {
                                switch (str)
                                {
                                    case "33":
                                        Odds *= ListOdd[i].ss;
                                        break;
                                    case "31":
                                        Odds *= ListOdd[i].sp;
                                        break;
                                    case "30":
                                        Odds *= ListOdd[i].sf;
                                        break;
                                    case "13":
                                        Odds *= ListOdd[i].ps;
                                        break;
                                    case "11":
                                        Odds *= ListOdd[i].pp;
                                        break;
                                    case "10":
                                        Odds *= ListOdd[i].pf;
                                        break;
                                    case "03":
                                        Odds *= ListOdd[i].fs;
                                        break;
                                    case "01":
                                        Odds *= ListOdd[i].fp;
                                        break;
                                    case "00":
                                        Odds *= ListOdd[i].ff;
                                        break;
                                    default:
                                        Odds *= 1;
                                        break;
                                }
                                j++;
                            }
                        }
                        else
                        {
                            isOk = true;
                        }
                    }


                    if (isOk || Odds >= Min && Odds <= Max)
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

        private int sortMethond(NumOrders2 n1, NumOrders2 n2)
        {
            double d = n1.Odds - n2.Odds;
            if (d > 0)
            {
                return 1;
            }
            {
                if (d == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }

        }
        #endregion

        //获取未处理前得全部数组（递归方法）
        private static void GetList(List<string[]> lists, string str, int n)
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

        //（公共方法）计算指数积、指数和、奖金的方法
        private void CountOdds()
        {
            double SumMinOdds = 0.00;//指数和 最小
            double SumMaxOdds = 0.00;//指数和 最大

            double ProductMinOdds = 1.00;//指数积 最小
            double ProductMaxOdds = 1.00;//指数积 最大

            List<double> DoubleList = new List<double>();

            var selfcols = this.DataGrid1.Columns[3];

            foreach (var item in DataGrid1.ItemsSource)
            {
                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    //--单元格所包含的元素
                    Grid grid = cells as Grid;
                    foreach (Button button1 in grid.Children)
                    {
                        SolidColorBrush b_Brush1 = (SolidColorBrush)(button1.Background);

                        if (b_Brush1.Color.ToString() == "#FFFF0000")
                        {
                            DoubleList.Add(Netball.RetuntDouble(ToolTipService.GetToolTip(button1).ToString()));
                        }
                    }

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

        //转换为数字，转换失败返回0
        private int ConvertInt(string Num)
        {
            int Results = 0;

            int.TryParse(Num, out Results);

            return Results;
        }

        //得到选中的赔率（传到子窗体的方法）
        public List<List<double>> getSelectOdds()
        {
            List<List<double>> Result = new List<List<double>>();
            List<double> Result2 = null;

            var selfcols = this.DataGrid1.Columns[3];

            string First = string.Empty;

            foreach (var item in DataGrid1.ItemsSource)
            {
                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);

                if (cells != null)
                {
                    Result2 = new List<double>();
                    //--单元格所包含的元素
                    Grid grid = cells as Grid;

                    foreach (Button b in grid.Children)
                    {
                        SolidColorBrush b_Brush1 = (SolidColorBrush)(b.Background);

                        if (b_Brush1.Color.ToString() == "#FFFF0000")
                        {
                            Result2.Add(Netball.RetuntDouble(ToolTipService.GetToolTip(b).ToString()));
                        }
                    }
                    Result.Add(Result2);
                }
            }
            return Result;
        }

        //得到选中的按钮（传到子窗体的方法）
        public List<List<string>> getSelectButton()
        {
            List<List<string>> Result = new List<List<string>>();
            List<string> Result2 = null;
            var selfcols = this.DataGrid1.Columns[3];

            foreach (var item in DataGrid1.ItemsSource)
            {
                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    Result2 = new List<string>();
                    //--单元格所包含的元素
                    Grid grid = cells as Grid;
                    foreach (Button button1 in grid.Children)
                    {
                        SolidColorBrush b_Brush1 = (SolidColorBrush)(button1.Background);

                        if (b_Brush1.Color.ToString() == "#FFFF0000")
                        {
                            Result2.Add(button1.Content.ToString());
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

            saveMobile.ModlePlayTypeName = "竞彩半全场";
            saveMobile.UserID = UserID;

            saveMobile.TypeName = BuyWays;
            saveMobile.PlayType = "7204";

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
                    case "4"://点击指数过滤的(第一指数命中、第二指数命中、第三指数命中)
                        Min = ConvertInt((StackPanel.Children[2] as ComboBox).SelectedItem.ToString());
                        Max = ConvertInt((StackPanel.Children[6] as ComboBox).SelectedItem.ToString());

                        TempStr = (StackPanel.Children[5] as TextBlock).Text;
                        TempStr = TempStr.Substring(0, TempStr.Length - 4);
                        break;
                    case "5"://点击排序截取的(赔率高到底排序、赔率低到高排序)
                        Min = ConvertInt((StackPanel.Children[4] as TextBox).Text);
                        Max = ConvertInt((StackPanel.Children[6] as TextBox).Text);

                        TempStr = (StackPanel.Children[3] as TextBlock).Text;
                        TempStr = TempStr.Substring(0, 5);
                        break;
                    case "6"://范围截取的(随机截取、奖金最高、概率最高)
                        Min = ConvertInt((StackPanel.Children[3] as TextBox).Text);

                        TempStr = (StackPanel.Children[2] as TextBlock).Text;
                        break;
                    case "7"://弹出的子窗体
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
                    case "3的个数":
                    case "1的个数":
                    case "0的个数":
                    case "号码和值":
                    case "断点个数":
                    case "连号个数":
                    case "主场连胜":
                    case "主场连平":
                    case "主场连负":
                    case "不败连续个数":
                    case "不平连续个数":
                    case "不胜连续个数":
                    case "半场主胜个数":
                    case "半场主平个数":
                    case "半场主负个数":
                    case "全场主胜个数":
                    case "全场主平个数":
                    case "全场主负个数":
                    case "半全相同个数":
                    case "半全不同个数":
                    case "逆转场次个数":
                    case "33个数":
                    case "31个数":
                    case "30个数":
                    case "13个数":
                    case "11个数":
                    case "10个数":
                    case "03个数":
                    case "01个数":
                    case "00个数":
                        tempDictionary1.Add(TempStr + "@" + Min.ToString() + "_" + Max.ToString());
                        break;
                    case "指数和":
                    case "指数积":
                        tempDictionary2.Add(TempStr + "@" + MinOdds.ToString() + "_" + MaxOdds.ToString());
                        break;
                    case "奖金范围":
                        tempDictionary3.Add(TempStr + "@" + term);
                        break;
                    case "第一二三指数命中":
                    case "第四五六指数命中":
                    case "第七八九指数命中":
                        tempDictionary4.Add(TempStr + "@" + Min.ToString() + "_" + Max.ToString());
                        break;
                    case "赔率高到底":
                    case "赔率底到高":
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
                    case "分组和值":
                    case "分组断点":
                    case "分组最长连号":
                    case "分组指数和值":
                    case "分组指数积值":
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
            modelManage.PlayTypeID = "7204";
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
                            AddIndexFrist(strResult[0].Replace("[", ""), Min, Max);
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

    class NumOrders2
    {
        public string Nums { get; set; }
        public double Odds { get; set; }
    }

    public class bqc
    {
        public double ss = 0.00;
        public double sp = 0.00;
        public double sf = 0.00;
        public double ps = 0.00;
        public double pp = 0.00;
        public double pf = 0.00;
        public double fs = 0.00;
        public double fp = 0.00;
        public double ff = 0.00;
        public int id = -1;
    }
}
