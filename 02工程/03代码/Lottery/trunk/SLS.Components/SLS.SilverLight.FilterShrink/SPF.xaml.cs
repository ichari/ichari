using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SLS.SilverLight.FilterShrink.Model;
using System.Collections.Generic;
using System.Windows.Data;
using System.ServiceModel;
using SLS.SilverLight.FilterShrink.ServiceReference1;
using System.Text.RegularExpressions;
using System.Windows.Browser;
using System.Net;
using SLS.SilverLight.FilterShrink.SPFChildWindow;
using SLS.SilverLight.FilterShrink.ServiceModel;

namespace SLS.SilverLight.FilterShrink
{
    public partial class SPF : UserControl
    {
        #region 全局变量
        Nums num = new Nums();
        static List<List<obj>> oldNums = new List<List<obj>>();

        string[] GameNumber = null;//得到场次信息id和胜平负信息（全局变量）
        string BuyWays = "";
        int Mulitpe = 1;
        long UserID = 0;

        bool IsBind = false;

        static List<string> ListResult = new List<string>();//(处理后的全部数据)全局变量
        List<Netball> NetBallList = new List<Netball>();//起始页面加载在datagrid的数据集合
        List<LotteryNum> Num = new List<LotteryNum>();  //过滤之后的数据

        #region 弹出窗体对象
        SPFChildWindow.groupsum ChildWindow = new groupsum();//创建弹出的窗体
        SPFChildWindow.grouppoint ChildPoint = new grouppoint();//分组断点的子窗体

        SPFChildWindow.indexsum ChildIndex = new indexsum();//分组指数和的子窗体
        SPFChildWindow.indexproduct ChildProduct = new indexproduct();//分组指数积的子窗体

        SPFChildWindow.gatherfilter ChildGather = new gatherfilter();//集合过滤的子窗体

        ModelChildWindow.SaveChildWindow saveMobile = new SLS.SilverLight.FilterShrink.ModelChildWindow.SaveChildWindow();
        ModelChildWindow.ModelManage modelManage = new SLS.SilverLight.FilterShrink.ModelChildWindow.ModelManage();
        ModelChildWindow.ModelShow modelShow = new SLS.SilverLight.FilterShrink.ModelChildWindow.ModelShow();
        ModelChildWindow.login login = new SLS.SilverLight.FilterShrink.ModelChildWindow.login();
        #endregion

        #endregion

        public SPF()
        {
            // 需要初始化变量
            InitializeComponent();

            #region 条件篮的主导航
            btnRoutine.Click += new RoutedEventHandler(btnRoutine_Click);
            btnFrist.Click += new RoutedEventHandler(btnRoutine_Click);
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
            btnFristPreferred.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristSecond.Click += new RoutedEventHandler(btnRoutineThree_Click);
            btnFristEnd.Click += new RoutedEventHandler(btnRoutineThree_Click);

            //分组过滤

            //指数过滤
            btnIndexSum.Click += new RoutedEventHandler(btnIndexSum_Click);//指数和
            btnIndexProduct.Click += new RoutedEventHandler(btnIndexSum_Click);//指数积
            btnIndexBonus.Click += new RoutedEventHandler(btnIndexBonus_Click);//奖金范围
            btnIndexFrist.Click += new RoutedEventHandler(btnIndexFrist_Click);//第一指数命中
            btnIndexSecond.Click += new RoutedEventHandler(btnIndexFrist_Click);//第二指数命中
            btnIndexThree.Click += new RoutedEventHandler(btnIndexFrist_Click);//第三指数命中

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

            //加载球赛信息 “场次，主场球队，客场球队，让球，赔率等”，加载到datagrid
            BindNetBall();

            //处理按钮
            btnProcess.Click += new RoutedEventHandler(btnProcess_Click);

            //弹出窗体关闭的事件
            ChildWindow.Closed += new EventHandler(group_Closed);
            ChildPoint.Closed += new EventHandler(ChildPoint_Closed);
            ChildIndex.Closed += new EventHandler(ChildIndex_Closed);
            ChildProduct.Closed += new EventHandler(ChildProduct_Closed);
            ChildGather.Closed += new EventHandler(ChildGather_Closed);

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
        }

        //加载球赛信息 “场次，主场球队，客场球队，让球，赔率等”，加载到datagrid
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

            try
            {
                Mulitpe = int.Parse(Number.Trim().Split(';')[2].ToString().Substring(1, Number.Trim().Split(';')[2].ToString().Length - 2).Substring(2).ToString().Trim());
            }
            catch
            {
                Mulitpe = 1;
            }

            if (BuyWays == "")
            {
                BuyWays = Netball.GetBuyWays(GameNumber.Length > 8 ? 8 : GameNumber.Length);
            }

            //一共多少场比赛
            this.tbSessionCount.Text = GameNumber.Length.ToString() + "场";

            string PlayName = string.Empty;

            PlayName = "(" + Netball.GetGameType(BuyWays) + ")";

            if (string.IsNullOrEmpty(PlayName) && GameNumber.Length > 8)
            {
                PlayName = "(8串1)";
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

        //加载球赛信息 “场次，主场球队，客场球队，让球，赔率等”，加载到datagrid(回调函数)
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

                for (int k = 0; k < PassRate.Count; k++)
                {
                    if (PassRate[k].MatchID.ToString() == id)
                    {
                        ball.Number = PassRate[k].MatchNumber;
                        ball.HomeField = PassRate[k].MainTeam;
                        ball.ConcessionBall = Convert.ToInt32(PassRate[k].MainLoseball);
                        ball.VisitingField = PassRate[k].GuestTeam;
                        ball.FristOdds = Convert.ToDouble(PassRate[k].Win);
                        ball.NextOdds = Convert.ToDouble(PassRate[k].Flat);
                        ball.LastOdds = Convert.ToDouble(PassRate[k].Lose);
                        ball.Id = PassRate[k].Id;
                        break;
                    }
                }

                if (InfoLength == 3)
                {
                    ball.FristField = GetCount(otherinfo[0].ToString());
                    ball.NextField = GetCount(otherinfo[1].ToString());
                    ball.LastField = GetCount(otherinfo[2].ToString());
                }
                else if (InfoLength == 2)
                {
                    ball.FristField = GetCount(otherinfo[0].ToString());
                    ball.NextField = GetCount(otherinfo[1].ToString());
                }
                else
                {
                    ball.FristField = GetCount(otherinfo[0].ToString());
                }

                NetBallList.Add(ball);
            }

            this.DataGrid1.ItemsSource = NetBallList;

            //计算赔率
            CountOdds();

            //List<Netball> List = DataGrid1.ItemsSource as List<Netball>;

        }

        //计算赔率
        void CountOdds()
        {
            List<Netball> List = DataGrid1.ItemsSource as List<Netball>;

            string Frist = string.Empty;//首
            string Next = string.Empty;//次
            string Last = string.Empty;//末

            List<Odds> ListOdd = new List<Odds>();
            for (int i = 0; i < List.Count; i++)
            {
                //胜平负的赔率类
                Odds Odd = new Odds();

                Frist = List[i].FristField;//首场是3 or 1or 0
                Next = List[i].NextField;//次场是3 or 1or 0
                Last = List[i].LastField;//末场是3 or 1or 0                

                #region 判断首次末场比较的胜负平情况
                switch (Frist)
                {
                    case "3":
                        Odd.Frist = List[i].FristOdds;//胜
                        break;
                    case "1":
                        Odd.Frist = List[i].NextOdds;//平
                        break;
                    case "0":
                        Odd.Frist = List[i].LastOdds;//负
                        break;
                    default:
                        Odd.Frist = 0.00;
                        break;
                }
                switch (Next)
                {
                    case "3":
                        Odd.Next = List[i].FristOdds;//胜
                        break;
                    case "1":
                        Odd.Next = List[i].NextOdds;//平
                        break;
                    case "0":
                        Odd.Next = List[i].LastOdds;//负
                        break;
                    default:
                        Odd.Next = 0.00;
                        break;
                }
                switch (Last)
                {
                    case "3":
                        Odd.Last = List[i].FristOdds;//胜
                        break;
                    case "1":
                        Odd.Last = List[i].NextOdds;//平
                        break;
                    case "0":
                        Odd.Last = List[i].LastOdds;//负
                        break;
                    default:
                        Odd.Last = 0.00;
                        break;
                }
                #endregion

                Odd.Id = List[i].Id;
                ListOdd.Add(Odd);
            }

            double SumMax = 0.00;//指数和最大值
            double SumMin = 0.00;//指数和最小值

            double ProductMax = 1.00;//指数积最大值
            double ProductMin = 1.00;//指数积最小值

            //计算赔率和值范围，赔率乘积范围
            for (int j = 0; j < ListOdd.Count; j++)
            {
                double Max = GetMaxValue(ListOdd[j].Frist, ListOdd[j].Next, ListOdd[j].Last);
                double Min = GetMinValue(ListOdd[j].Frist, ListOdd[j].Next, ListOdd[j].Last);

                SumMax += Max;
                SumMin += Min;

                ProductMax *= Max;
                ProductMin *= Min;
            }

            this.tbSum.Text = SumMin.ToString() + "～" + SumMax.ToString();
            this.tbProduct.Text = string.Format("{0:f2}", ProductMin) + "～" + string.Format("{0:f2}", ProductMax);
            this.tbPremium.Text = string.Format("{0:f2}:", 2 * ProductMin) + "～" + string.Format("{0:f2}", 2 * ProductMax);
        }

        //条件篮的全部删除事件
        void btnDelAllCondition_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            spCondition.Children.Clear();
            spCondition.Height = 250;
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

        //传过来的是胜平负信息,数据库里存的是1,2,3分别代表胜、平、负,传出去的是3,1,0
        public string GetCount(string No)
        {
            switch (No)
            {
                case "1":
                    return "3";
                case "2":
                    return "1";
                case "3":
                    return "0";
                default:
                    return "";
            }
        }

        //传过来的是胜平负信息,数据库里存的是1,2,3分别代表胜、平、负,传出去的是3,1,0
        public string GetNo(string count)
        {
            switch (count)
            {
                case "3":
                    return "1";
                case "1":
                    return "2";
                case "0":
                    return "3";
                default:
                    return "";
            }
        }

        //首、次、末按钮
        void btn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsBind)
            {
                IsBind = true;

                var selfcols = DataGrid1.Columns[4];//第5列 是首次末的按钮

                foreach (var items in DataGrid1.ItemsSource) //遍历第5列
                {
                    List<obj> ListObj = new List<obj>();
                    var cells1 = selfcols.GetCellContent(items);

                    if (cells1 != null)
                    {
                        Grid Grid = cells1 as Grid;
                        for (int i = 0; i < 3; i++)
                        {
                            obj obj = new obj();

                            obj.lb = Grid.Children[i] as Button;
                            obj.value = (Grid.Children[i] as Button).Content.ToString();
                            ListObj.Add(obj);
                        }
                    }

                    oldNums.Add(ListObj);
                }
            }
            int index = DataGrid1.SelectedIndex;//当前行的索引

            Button b = sender as Button;
            Grid s = b.Parent as Grid;

            if (oldNums[index][2].lb == b && oldNums[index][1].value == "")
            {
                return;
            }


            bool clear = false;
            for (int i = 0; i < s.Children.Count; i++)
            {
                if ((s.Children[i] as Button) == b)
                {
                    clear = true;
                    if (i == 0)
                    {
                        oldNums[index][i].value = "";
                    }
                }
                else
                {
                    if (clear)
                    {
                        (s.Children[i] as Button).Content = "";
                        if (oldNums[index].Count > i)
                        {
                            oldNums[index][i].value = "";
                        }
                    }
                }
            }

            //MessageBox.Show(oldNums[index][2].value.ToString());

            obj ob = new obj();
            do
            {
                num.CurrentNumstr = b.Content.ToString();

                b.Content = num.nextNum().ToString();

                ob = oldNums[index].Finds<obj>(t => (t.value.ToString()) == (b.Content.ToString()));

                if (oldNums[index][0].lb == b && b.Content.ToString() == "")
                {
                    b.Content = num.nextNum().ToString();
                }
            } while ((ob != null && ob.lb != b)
                    && b.Content.ToString() != ""
                );



            ob = oldNums[index].Finds<obj>(t => t.lb == b);

            if (ob == null)
            {
                obj ob1 = new obj();
                ob1.lb = b;
                ob1.value = b.Content.ToString();
                oldNums[index].Add(ob1);
            }
            else
            {
                ob.value = b.Content.ToString();
            }

            //计算赔率 传一个当前的按钮对象
            CountOdds(b);
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
                LotteryNumber = "7201;[";

                for (int j = 0; j < GameNumber.Length; j++)
                {
                    if (Num[i].LotteryNums.Substring(j, 1).Equals("*"))
                    {
                        continue;
                    }

                    LotteryNumber += GameNumber[j].Substring(0, GameNumber[j].IndexOf('(')) + "(" + GetNo(Num[i].LotteryNums.Substring(j, 1)) + ")|";
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

        #region 弹出窗体相关的事件
        //弹出窗体groupsum.xaml关闭后事件
        void group_Closed(object sender, EventArgs e)
        {
            if (ChildWindow.DialogResult == true)
            {
                string LbList = string.Format("{0}", ChildWindow.HidResult.Text.ToString());
                (ChildWindow.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (ChildWindow.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数                       

            }
        }

        //弹出窗体childpoint.xaml关闭后事件
        void ChildPoint_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (ChildPoint.DialogResult == true)
            {
                string LbList = string.Format("{0}", ChildPoint.HidResult.Text.ToString());
                (ChildPoint.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (ChildPoint.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }

        //弹出窗体childIndex.xaml关闭后事件
        void ChildIndex_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (ChildIndex.DialogResult == true)
            {
                string LbList = string.Format("{0}", ChildIndex.HidResult.Text.ToString());
                (ChildIndex.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (ChildIndex.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }

        //弹出窗体childProduct.xaml关闭后事件
        void ChildProduct_Closed(object sender, EventArgs e)
        {
            if (ChildProduct.DialogResult == true)
            {
                string LbList = string.Format("{0}", ChildProduct.HidResult.Text.ToString());
                (ChildProduct.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (ChildProduct.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }

        void ChildGather_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (ChildGather.DialogResult == true)
            {
                string LbList = string.Format("{0}", ChildGather.HidResult.Text.ToString());
                (ChildGather.StackPanel.Children[5] as TextBox).Text = LbList;//存放子窗体传过来的条件值
                (ChildGather.StackPanel.Children[3] as TextBox).Text = "共选择了" + LbList.Split(';').Length.ToString() + "个条件";//显示条件的个数
            }
        }
        #endregion

        #region 打开子窗体的事件代码
        //点击分组过滤的(分组和值的设置按钮弹出窗体的事件)
        void bt1_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            StackPanel sp = btn.Parent as StackPanel;
            string Title = (sp.Children[2] as TextBlock).Text.ToString();
            switch (Title)
            {
                case "分组和值":
                    ChildWindow.Title = Title;
                    ChildWindow.OverlayBrush = new SolidColorBrush(Colors.White);
                    ChildWindow.Opacity = 1;
                    ChildWindow.HasCloseButton = true;
                    ChildWindow.Foreground = new SolidColorBrush(Colors.Red);
                    ChildWindow.FontSize = 14;
                    ChildWindow.Height = 410;
                    ChildWindow.Width = 650;
                    ChildWindow.OverlayOpacity = 0.5;
                    //group.IsEnabled = false;
                    ChildWindow.ListStr = (sp.Children[5] as TextBox).Text;
                    ChildWindow.Show();//打开子窗体
                    ChildWindow.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
                case "分组断点":
                    ChildPoint.Title = Title;
                    ChildPoint.OverlayBrush = new SolidColorBrush(Colors.White);
                    ChildPoint.Opacity = 1;
                    ChildPoint.HasCloseButton = true;
                    ChildPoint.Foreground = new SolidColorBrush(Colors.Red);
                    ChildPoint.FontSize = 14;
                    ChildPoint.Height = 410;
                    ChildPoint.Width = 650;
                    ChildPoint.OverlayOpacity = 0.5;
                    //group.IsEnabled = false;       
                    ChildPoint.ListStr = (sp.Children[5] as TextBox).Text;
                    ChildPoint.Show();//打开子窗体
                    ChildPoint.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
                case "分组指数和值":
                    ChildIndex.Title = Title;
                    ChildIndex.OverlayBrush = new SolidColorBrush(Colors.White);
                    ChildIndex.Opacity = 1;
                    ChildIndex.HasCloseButton = true;
                    ChildIndex.Foreground = new SolidColorBrush(Colors.Red);
                    ChildIndex.FontSize = 14;
                    ChildIndex.Height = 410;
                    ChildIndex.Width = 650;
                    ChildIndex.OverlayOpacity = 0.5;
                    //group.IsEnabled = false; 
                    ChildIndex.ListStr = (sp.Children[5] as TextBox).Text;
                    ChildIndex.SelectedRate = getSelectRate();
                    ChildIndex.Show();//打开子窗体
                    ChildIndex.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
                case "分组指数积值":
                    ChildProduct.Title = Title;
                    ChildProduct.OverlayBrush = new SolidColorBrush(Colors.White);
                    ChildProduct.Opacity = 1;
                    ChildProduct.HasCloseButton = true;
                    ChildProduct.Foreground = new SolidColorBrush(Colors.Red);
                    ChildProduct.FontSize = 14;
                    ChildProduct.Height = 410;
                    ChildProduct.Width = 650;
                    ChildProduct.OverlayOpacity = 0.5;
                    //group.IsEnabled = false; 
                    ChildProduct.ListStr = (sp.Children[5] as TextBox).Text;
                    ChildProduct.SelectedRate = getSelectRate();
                    ChildProduct.Show();//打开子窗体
                    ChildProduct.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
                case "命中场次":
                case "冷门过滤":
                case "叠加过滤":
                    ChildGather.Title = Title;
                    ChildGather.OverlayBrush = new SolidColorBrush(Colors.White);
                    ChildGather.Opacity = 1;
                    ChildGather.HasCloseButton = true;
                    ChildGather.Foreground = new SolidColorBrush(Colors.Red);
                    ChildGather.FontSize = 14;
                    ChildGather.Height = 400;
                    ChildGather.Width = 700;
                    ChildGather.OverlayOpacity = 0.5;
                    //group.IsEnabled = false; 
                    ChildGather.ListStr = (sp.Children[5] as TextBox).Text.ToString();

                    ChildGather.SelectedRate = getSelectNum();
                    ChildGather.Show();//打开子窗体
                    ChildGather.BindDataGrid(NetBallList, sp);//条用子窗体的方法，把起始页的数据集合传到子窗体显示
                    break;
            }
        }
        #endregion

        #region 点击过滤按钮加载条件
        //点击常规过滤或者首次末选的条件按钮 操作类型1
        void btnRoutineThree_Click(object sender, RoutedEventArgs e)
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
                    Count = List.Count;
                    break;
                case "号码和值":
                    Count = List.Count * 3;
                    break;
                case "断点个数":
                case "连号个数":
                    Count = List.Count - 1;
                    break;
                case "主场连胜":
                case "主场连平":
                case "主场连负":
                    Count = List.Count;
                    break;
                case "不败连续个数":
                case "不平连续个数":
                case "不胜连续个数":
                    Count = List.Count;
                    break;
                case "首选命中":
                case "次选命中":
                case "末选命中":
                    Count = List.Count;
                    break;
                default:
                    Count = List.Count;
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
        void btnIndexBonus_Click(object sender, RoutedEventArgs e)
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
        void btnIndexFrist_Click(object sender, RoutedEventArgs e)
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

        //分组过滤的（分组和值、分组断点、分组指数和值、分组指数积值）/集合过滤的(命中场次、冷门过滤、叠加过滤)
        void btnGroupSum_Click(object sender, RoutedEventArgs e)
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

        //过滤处理
        void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            ListResult.Clear();

            var selfcols = DataGrid1.Columns[4];//第三列 是首次末的按钮

            List<Netball> NetBall = DataGrid1.ItemsSource as List<Netball>;

            string GameTypeCount = this.tbGameType1.Text.Substring(1, 1).ToString();//几串几，数字

            List<string[]> List = new List<string[]>();//数组集合

            string Frist = string.Empty;//首场 竖排数据
            string Next = string.Empty;//次场 竖排数据 
            string Last = string.Empty;//末场 竖排数据

            foreach (var item in DataGrid1.ItemsSource) //遍历第三列
            {
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    Grid Grid = cells as Grid;

                    string TempStr = (Grid.Children[0] as Button).Content.ToString() + "," +
                                     (Grid.Children[1] as Button).Content.ToString() + "," +
                                     (Grid.Children[2] as Button).Content.ToString() + "," +
                                     "*";
                    Frist += (Grid.Children[0] as Button).Content.ToString();
                    Next += (Grid.Children[1] as Button).Content.ToString();
                    Last += (Grid.Children[2] as Button).Content.ToString();

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
                string Filter = string.Empty;
                for (int i = 0; i < this.spCondition.Children.Count; i++)
                {
                    StackPanel StackPanel = this.spCondition.Children[i] as StackPanel;

                    ProcessType = (StackPanel.Children[0] as TextBox).Text.ToString();

                    int Min = 0;
                    int Max = 0;

                    double MinOdds = 0.00;
                    double MaxOdds = 0.00;
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
                        case "7"://分组过滤的(分组和值、分组积值、分组指数和值、分组指数积值)
                            Filter = (StackPanel.Children[5] as TextBox).Text;

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
                        case "号码和值":
                            TempListStr = GetSumValue(Min, Max, TempList);
                            TempList = TempListStr;
                            break;
                        case "断点个数":
                            TempListStr = GetBreakPoint(Min, Max, TempList);
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
                        case "首选命中":
                            TempListStr = GetFristHit(Min, Max, Frist, TempList);
                            TempList = TempListStr;
                            break;
                        case "次选命中":
                            TempListStr = GetFristHit(Min, Max, Next, TempList);
                            TempList = TempListStr;
                            break;
                        case "末选命中":
                            TempListStr = GetFristHit(Min, Max, Last, TempList);
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
                        case "第一指数命中":
                            TempListStr = GetPointHit(Min, Max, 0, TempList);
                            TempList = TempListStr;
                            break;
                        case "第二指数命中":
                            TempListStr = GetPointHit(Min, Max, 1, TempList);
                            TempList = TempListStr;
                            break;
                        case "第三指数命中":
                            TempListStr = GetPointHit(Min, Max, 2, TempList);
                            TempList = TempListStr;
                            break;
                        case "分组和值":
                            TempListStr = GetGroupSum(Filter, TempList);
                            TempList = TempListStr;
                            break;
                        case "分组断点":
                            TempListStr = GetGoupBreakpoint(Filter, TempList);
                            TempList = TempListStr;
                            break;
                        case "分组指数和值":
                        case "分组指数积值":
                            int t = TempStr == "分组指数和值" ? 1 : 2;
                            TempListStr = GetGoupIndexSumProduct(Filter, t, TempList);
                            TempList = TempListStr;
                            break;
                        case "命中场次":
                        case "冷门过滤":
                        case "叠加过滤":
                            TempListStr = GetGoupHitTarget(Filter, TempList);
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
        List<string> GetThreeCounts(int Min, int Max, string Num, List<string> ListR)
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

        //号码和值
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

        //断点个数
        List<string> GetBreakPoint(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            foreach (string s in ListR)
            {
                Count = -1;

                char Temp = 'x';
                string s1 = s.Replace("*", "");
                foreach (char c in s1)
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
        List<string> GetConsecutive(int Min, int Max, List<string> ListR)
        {
            List<string> ListStr = new List<string>();

            int Count = 0;
            int i = 1;
            char Temp = 'x';

            foreach (string s in ListR)
            {
                Count = 0;
                i = 1;
                string s1 = s.Replace("*", "");
                foreach (char c in s1)
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
        List<string> GetMainCount(int Min, int Max, char Temp, List<string> ListR)
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
        List<string> GetFalseCount(int Min, int Max, char Temp, List<string> ListR)
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
        List<string> GetFristHit(int Min, int Max, string FristStr, List<string> ListR)
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
        List<string> GetIndexSumProduct(double Min, double Max, int Types, List<string> ListR)
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
        List<string> GetIndexBonus(double Min, double Max, List<string> ListR)
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
        List<string> GetOrder(int Min, int Max, int Types, List<string> ListR)
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

        //范围截取的(奖金最高、 概率最高)Types = 1 是奖金最高 Types = 2是概率最高
        List<string> GetRangeBonus(int Min, int Types, List<string> ListR)
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

        //指数过滤的(第一指数命中，第二指数命中，第三指数命中)Type =0第一 Types=1第二 Types = 2 第三
        List<string> GetPointHit(int Min, int Max, int Types, List<string> ListR)
        {
            List<List<NumOrders>> NumOrderList = new List<List<NumOrders>>();//按钮上的文字对应相应的赔率

            for (int k = 0; k < NetBallList.Count; k++)
            {
                List<NumOrders> OrderList = new List<NumOrders>();
                OrderList.Add(new NumOrders { Nums = "3", Odds = NetBallList[k].FristOdds });
                OrderList.Add(new NumOrders { Nums = "1", Odds = NetBallList[k].NextOdds });
                OrderList.Add(new NumOrders { Nums = "0", Odds = NetBallList[k].LastOdds });

                OrderList.Sort(delegate(NumOrders order1, NumOrders order2) { return Comparer<double>.Default.Compare(order1.Odds, order2.Odds); });
                NumOrderList.Add(OrderList);
            }

            List<string> ListStr = new List<string>();

            int i = 0;
            int Count = 0;
            foreach (string s in ListR)
            {
                Count = 0;
                i = 0;

                foreach (char c in s)
                {
                    if (c.ToString() == NumOrderList[i][Types].Nums.ToString() && c.ToString() != "*")
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

        //分组过滤的（分组和值)
        List<string> GetGroupSum(string terms, List<string> ListR)
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

                double Min = double.Parse(nums2.Split('-')[0]);
                double max = double.Parse(nums2.Split('-')[1]);

                ListR = ListR.FindAll(t =>
                {
                    bool isOK = false;
                    int sum = 0;
                    foreach (int i in number)
                    {
                        if (t[i].ToString() != "*")
                        {
                            sum += int.Parse(t[i].ToString());
                        }
                        else
                        {
                            isOK = true;
                        }
                    }

                    return isOK || (Min <= sum && sum <= max);
                });
            }
            ListStr.Clear();
            ListStr.AddRange(ListR);

            return ListStr;
        }

        //分组过滤的分组断点
        List<string> GetGoupBreakpoint(string terms, List<string> ListR)
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

                double Min = double.Parse(nums2.Split('-')[0]);
                double Max = double.Parse(nums2.Split('-')[1]);

                int Count = 0;
                foreach (string s in ListR)
                {
                    bool isOk = false;
                    Count = 0;
                    string Temp = "x";
                    string c = string.Empty;
                    for (int i = 0; i < number.Count; i++)
                    {
                        c = s[number[i]].ToString();
                        if (c != "*")
                        {
                            if (Temp != c)
                            {
                                Temp = c;

                                if (i > 0
                                    //   && number[i] - number[i - 1] == 1
                                    )
                                {
                                    Count++;
                                }
                            }
                        }
                        else
                        {
                            isOk = true;
                        }
                    }

                    if (Count == -1) Count = 0;
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

        //分组过滤的(分组指数和、分组指数积)
        List<string> GetGoupIndexSumProduct(string terms, int Types, List<string> ListR)
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
                        if (s[i] != '*')
                        {
                            isOk = false;
                            if (Types == 1)//Types = 1指数和
                            {
                                switch (s[i])
                                {
                                    case '3':
                                        Odds += ListOdd[i].Frist;
                                        break;
                                    case '1':
                                        Odds += ListOdd[i].Next;
                                        break;
                                    case '0':
                                        Odds += ListOdd[i].Last;
                                        break;
                                    default:
                                        Odds += 0.00;
                                        break;
                                }
                                j++;
                            }
                            else
                            {
                                switch (s[i])
                                {
                                    case '3':
                                        Odds *= ListOdd[i].Frist;
                                        break;
                                    case '1':
                                        Odds *= ListOdd[i].Next;
                                        break;
                                    case '0':
                                        Odds *= ListOdd[i].Last;
                                        break;
                                    default:
                                        Odds *= 1.00;
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
                        c = s[number[i]].ToString();
                        if (c != "*")
                        {
                            if (strs[number[i]].Contains(c))
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

                    //for (int i = 0; i < s.Length; i++)
                    //{
                    //    c = s[i].ToString();
                    //    if (strs[i].Contains(c))
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
        #endregion

        #region
        //计算赔率 传一个当前的按钮对象
        void CountOdds(Button b)
        {
            List<Netball> List = DataGrid1.ItemsSource as List<Netball>;

            List<Odds> ListOdd = new List<Odds>();

            string Frist = string.Empty;//首
            string Next = string.Empty;//次
            string Last = string.Empty;//末

            var selfcols = DataGrid1.Columns[4];//第5列 是首次末的按钮

            int i = 0;

            int ZhuCount = 1;
            foreach (var item in DataGrid1.ItemsSource) //遍历第5列
            {
                //Netball ball = item as Netball;

                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    Grid Grid = cells as Grid;

                    Frist = (Grid.Children[0] as Button).Content.ToString();//首场是3 or 1or 0 or ""
                    Next = (Grid.Children[1] as Button).Content.ToString();//次场是3 or 1or 0 or ""
                    Last = (Grid.Children[2] as Button).Content.ToString();//末场是3 or 1or 0 or ""  

                    //重新计算 总注数和投注金额
                    string[] Temp = (Frist + "," + Next + "," + Last).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    ZhuCount *= Temp.Length;

                    //胜平负的赔率类
                    Odds Odd = new Odds();

                    #region 判断首次末场比较的胜负平情况
                    switch (Frist)
                    {
                        case "3":
                            Odd.Frist = List[i].FristOdds;//胜
                            break;
                        case "1":
                            Odd.Frist = List[i].NextOdds;//平
                            break;
                        case "0":
                            Odd.Frist = List[i].LastOdds;//负
                            break;
                        default:
                            Odd.Frist = 0.00;
                            break;
                    }
                    switch (Next)
                    {
                        case "3":
                            Odd.Next = List[i].FristOdds;//胜
                            break;
                        case "1":
                            Odd.Next = List[i].NextOdds;//平
                            break;
                        case "0":
                            Odd.Next = List[i].LastOdds;//负
                            break;
                        default:
                            Odd.Next = 0.00;
                            break;
                    }
                    switch (Last)
                    {
                        case "3":
                            Odd.Last = List[i].FristOdds;//胜
                            break;
                        case "1":
                            Odd.Last = List[i].NextOdds;//平
                            break;
                        case "0":
                            Odd.Last = List[i].LastOdds;//负
                            break;
                        default:
                            Odd.Last = 0.00;
                            break;
                    }
                    #endregion

                    Odd.Id = List[i].Id;
                    ListOdd.Add(Odd);
                }
                i++;
            }
            this.tbZhuCount.Text = ZhuCount + "注";
            this.tbSumMoney.Text = (ZhuCount * 2 * Mulitpe).ToString();
            this.tbMultiple.Text = Mulitpe.ToString();


            double SumMax = 0.00;//指数和最大值
            double SumMin = 0.00;//指数和最小值

            double ProductMax = 1.00;//指数积最大值
            double ProductMin = 1.00;//指数积最小值

            //计算赔率和值范围，赔率乘积范围
            for (int j = 0; j < ListOdd.Count; j++)
            {

                double Max = GetMaxValue(ListOdd[j].Frist, ListOdd[j].Next, ListOdd[j].Last);
                double Min = GetMinValue(ListOdd[j].Frist, ListOdd[j].Next, ListOdd[j].Last);

                SumMax += Max;
                SumMin += Min;

                ProductMax *= Max;
                ProductMin *= Min;
            }

            this.tbSum.Text = string.Format("{0:f2}", SumMin).ToString() + "～" + string.Format("{0:f2}", SumMax).ToString();
            this.tbProduct.Text = string.Format("{0:f2}", ProductMin).ToString() + "～" + string.Format("{0:f2}", ProductMax).ToString();
            this.tbPremium.Text = string.Format("{0:f2}", (2 * ProductMin)).ToString() + "～" + string.Format("{0:f2}", (2 * ProductMax)).ToString();
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

        //获取最大值
        public double GetMaxValue(double Frist, double Next, double Last)
        {
            return Math.Max(Math.Max(Frist, Next), Last);
        }

        //获取最小值
        public double GetMinValue(double Frist, double Next, double Last)
        {
            if (Frist != 0.00 && Next != 0.00 && Last != 0.00)
            {
                return Math.Min(Math.Min(Frist, Next), Last);
            }

            else if (Frist != 0.00 && Next != 0.00 && Last == 0.00)
            {
                return Math.Min(Frist, Next);
            }
            else
            {
                return Frist;
            }
        }

        //转换为数字，转换失败返回0
        int ConvertInt(string Num)
        {
            int Results = 0;

            int.TryParse(Num, out Results);

            return Results;
        }

        //得到选中的赔率（传到子窗体的方法）
        public List<List<double>> getSelectRate()
        {
            List<List<double>> Result = new List<List<double>>();
            List<double> Result2 = null;

            var selfcols = this.DataGrid1.Columns[4];
            var selfcols1 = this.DataGrid1.Columns[5];

            string First = string.Empty;

            foreach (var item in DataGrid1.ItemsSource)
            {
                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);
                var cells1 = selfcols1.GetCellContent(item);

                if (cells != null)
                {
                    Result2 = new List<double>();
                    //--单元格所包含的元素
                    Grid grid = cells as Grid;
                    Grid grid1 = cells1 as Grid;

                    foreach (Button b in grid.Children)
                    {
                        First = b.Content.ToString();

                        switch (First)
                        {
                            case "3":
                                Result2.Add(Netball.RetuntDouble((grid1.Children[0] as TextBlock).Text.ToString()));
                                break;
                            case "1":
                                Result2.Add(Netball.RetuntDouble((grid1.Children[1] as TextBlock).Text.ToString()));
                                break;
                            case "0":
                                Result2.Add(Netball.RetuntDouble((grid1.Children[2] as TextBlock).Text.ToString()));
                                break;
                            default:
                                break;
                        }
                    }
                    Result.Add(Result2);
                }
            }
            return Result;
        }

        //得到选中的号码（传到子窗体的方法）
        public List<List<string>> getSelectNum()
        {
            List<List<string>> Result = new List<List<string>>();
            List<string> Result2 = null;

            var selfcols = this.DataGrid1.Columns[4];

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

                    foreach (Button b in grid.Children)
                    {
                        First = b.Content.ToString();
                        if (!string.IsNullOrEmpty(First))
                        {
                            Result2.Add(First);
                        }
                    }
                    Result.Add(Result2);
                }
            }
            return Result;
        }
        #endregion

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

            saveMobile.ModlePlayTypeName = "竞彩胜平负";
            saveMobile.UserID = UserID;

            saveMobile.TypeName = BuyWays;
            saveMobile.PlayType = "7201";

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
                    case "7"://分组过滤的(分组和值、分组积值、分组指数和值、分组指数积值)
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
                    case "首选命中":
                    case "次选命中":
                    case "末选命中":
                        tempDictionary1.Add(TempStr + "@" + Min.ToString() + "_" + Max.ToString());
                        break;
                    case "指数和":
                    case "指数积":
                        tempDictionary2.Add(TempStr + "@" + MinOdds.ToString() + "_" + MaxOdds.ToString());
                        break;

                    case "奖金范围":
                        tempDictionary3.Add(TempStr + "@" + term);
                        break;
                    case "第一指数命中":
                    case "第二指数命中":
                    case "第三指数命中":
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
            modelManage.PlayTypeID = "7201";
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


    class obj
    {
        public Button lb;
        public string value;

    }

    //赔率排序类
    class NumOrders
    {
        public string Nums;
        public double Odds;
    }

    //List的Find扩展方法
    public static class Find
    {
        public static T Finds<T>(this List<T> list, Func<T, bool> comparison)
        {
            foreach (T item in list)
            {
                if (comparison(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        public static List<T> FindAll<T>(this List<T> list, Func<T, bool> comparison)
        {
            List<T> lists = new List<T>();
            foreach (T item in list)
            {
                if (comparison(item))
                {
                    lists.Add(item);
                }
            }
            return lists;
        }

    }


}