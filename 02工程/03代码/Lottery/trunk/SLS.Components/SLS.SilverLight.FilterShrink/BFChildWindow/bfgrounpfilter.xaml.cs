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
using SLS.SilverLight.FilterShrink.Model;

namespace SLS.SilverLight.FilterShrink.BFChildWindow
{
    public partial class bfgrounpfilter : ChildWindow
    {
        public StackPanel StackPanel;

        public List<List<string>> SelectedRate;

        DispatcherTimer Timer;

        public string ListStr = string.Empty;//父窗体传来的过滤条件
        public bfgrounpfilter()
        {
            InitializeComponent();
        }

        //替换
        void btnShift_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            int Index = lbResult.SelectedIndex;
            if (Index == -1) return;

            string Str = string.Empty;

            var selfcols = dgMatchInfo.Columns[4];

            foreach (var item in dgMatchInfo.ItemsSource)
            {

                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    //--单元格所包含的元素
                    Grid Grid = cells as Grid;

                    if ((bool)(Grid.Children[0] as CheckBox).IsChecked)
                    {
                        Str += "Y,";
                    }
                    else
                    {
                        Str += "n,";
                    }
                }
            }

            Str = Str.Substring(0, Str.Length - 1);

            string Result = string.Empty;
            if (this.Title.ToString() == "进球差值")
            {
                Result = Str + "|(" + this.cbStart.SelectedItem.ToString() + ")-(" + this.cbEnd.SelectedItem.ToString() + ")";
            }
            else
            {
                Result = Str + "|" + this.cbStart.SelectedItem.ToString() + "-" + this.cbEnd.SelectedItem.ToString();
            }

            lbResult.Items.RemoveAt(Index);
            lbResult.Items.Insert(Index, Result);
        }

        //删除单个
        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            int Index = lbResult.SelectedIndex;
            if (Index == -1) return;
            lbResult.Items.RemoveAt(Index);
        }

        //全部删除
        void btnAllDel_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            lbResult.Items.Clear();
        }

        //添加按钮
        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string Str = string.Empty;

            var selfcols = dgMatchInfo.Columns[4];

            foreach (var item in dgMatchInfo.ItemsSource)
            {

                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    //--单元格所包含的元素
                    Grid Grid = cells as Grid;

                    if ((bool)(Grid.Children[0] as CheckBox).IsChecked)
                    {
                        Str += "Y,";
                    }
                    else
                    {
                        Str += "n,";
                    }
                }
            }

            Str = Str.Substring(0, Str.Length - 1);

            string Result = string.Empty;
            if (this.Title.ToString() == "进球差值")
            {
                Result = Str + "|(" + this.cbStart.SelectedItem.ToString() + ")-(" + this.cbEnd.SelectedItem.ToString() + ")";
            }
            else
            {
                Result = Str + "|" + this.cbStart.SelectedItem.ToString() + "-" + this.cbEnd.SelectedItem.ToString();
            }
            lbResult.Items.Add(Result);
        }

        //确定按钮
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string Str = string.Empty;

            if (lbResult.Items.Count > 0)
            {
                foreach (string s in lbResult.Items)
                {
                    Str += s + ";";
                }
                Str = Str.Substring(0, Str.Length - 1);
                HidResult.Text = Str;
            }
            else
            {
                MessageBox.Show("没有任何数据！");
                return;
            }
            this.DialogResult = true;
        }

        //取消按钮
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        //打开该窗体后，加载datagrid
        public void BindDataGrid(List<Netball> NetBallList, StackPanel sp)
        {
            cbStart.Items.Clear();
            cbEnd.Items.Clear();

            cbStart.Items.Add("0");
            cbEnd.Items.Add("0");

            dgMatchInfo.ItemsSource = NetBallList;
            StackPanel = sp;

            cbStart.SelectedIndex = 0;
            cbEnd.SelectedIndex = 0;

            tbName.Text = "<=所选场次" + Title + "<=";


            //延时初始化指数和、指数积、奖金
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }

        //复选框的点击时间事件
        private void CheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.

            var selfcols = dgMatchInfo.Columns[4];
            int Count = 0;
            foreach (var item in dgMatchInfo.ItemsSource)
            {
                //--对象所在的单元格
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    //--单元格所包含的元素
                    Grid Grid = cells as Grid;

                    if ((bool)(Grid.Children[0] as CheckBox).IsChecked)
                    {
                        Count++;
                    }
                }
            }

            string Title = this.Title.ToString();
            switch (Title)
            {
                case "分组断点":
                case "分组连号":
                case "分组最长连号":
                    Count = Count * 2;
                    break;
                case "分组和值":
                    Count = Count * 3;
                    break;
                default:
                    break;
            }

            cbStart.Items.Clear();
            cbEnd.Items.Clear();

            if (Title == "进球差值")
            {
                for (int j = Count * -4; j <= Count * 4; j++)
                {
                    cbStart.Items.Add(j.ToString());
                    cbEnd.Items.Add(j.ToString());
                }
            }
            else
            {
                for (int i = 0; i <= Count; i++)
                {
                    cbStart.Items.Add(i.ToString());
                    cbEnd.Items.Add(i.ToString());
                }
            }

            cbStart.SelectedItem = "0";
            cbEnd.SelectedItem = "0";
        }

        //延迟加载的方法
        void Timer_Tick(object sender, EventArgs e)
        {
            var selfcols = dgMatchInfo.Columns[4];
            foreach (var item in dgMatchInfo.ItemsSource)
            {
                //--对象所在的单元格                    
                var cells = selfcols.GetCellContent(item);
                if (cells != null)
                {
                    Grid grid = cells as Grid;
                    (grid.Children[0] as CheckBox).IsChecked = false;
                }
            }

            lbResult.Items.Clear();
            foreach (string s in ListStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                lbResult.Items.Add(s);
            }

            Timer.Stop();
        }
    }
}
