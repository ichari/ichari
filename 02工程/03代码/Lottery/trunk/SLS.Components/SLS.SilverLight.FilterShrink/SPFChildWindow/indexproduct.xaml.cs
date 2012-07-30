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
using SLS.SilverLight.FilterShrink.Model;
using System.Windows.Threading;

namespace SLS.SilverLight.FilterShrink.SPFChildWindow
{
    public partial class indexproduct : ChildWindow
    {
        public StackPanel StackPanel;

        public List<List<double>> SelectedRate;

        public string ListStr = string.Empty;//父窗体传来的过滤条件

        DispatcherTimer Timer;
        public indexproduct()
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
            var selfcols = dgMatchInfo.Columns[5];
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

            string Result = Str + "|" + (this.cbo.SelectedItem as ComboBoxItem).Content.ToString() + "|" + this.tbMin.Text.ToString() + "-" + this.tbMax.Text.ToString();

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
            if (string.IsNullOrEmpty(this.tbMin.Text) || string.IsNullOrEmpty(this.tbMax.Text))
            {
                MessageBox.Show("请输入范围！");
                return;
            }
            string Str = string.Empty;
            var selfcols = dgMatchInfo.Columns[5];
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

            string Result = Str + "|" + (this.cbo.SelectedItem as ComboBoxItem).Content.ToString() + "|" + this.tbMin.Text.ToString() + "-" + this.tbMax.Text.ToString();
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
            foreach (Netball ball in NetBallList)
            {
                if (!string.IsNullOrEmpty(ball.Number) && ball.Number.Length >= 3)
                {
                    ball.Number = ball.Number.Substring(ball.Number.Length - 3);
                }
                else
                {
                    ball.Number = "有误";
                }
            }
            dgMatchInfo.ItemsSource = NetBallList;

            lbResult.Items.Clear();
            foreach (string s in ListStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                lbResult.Items.Add(s);
            }
            tbMin.Text = "";
            tbMax.Text = "";

            StackPanel = sp;

            //延时初始化指数和、指数积、奖金
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
        }

        //延时加载的方法
        void Timer_Tick(object sender, EventArgs e)
        {
            var selfcols = dgMatchInfo.Columns[5];
            foreach (var items in dgMatchInfo.ItemsSource)
            {
                var cells1 = selfcols.GetCellContent(items);
                if (cells1 != null)
                {
                    Grid Grid = cells1 as Grid;

                    (Grid.Children[0] as CheckBox).IsChecked = false;
                }
            }

            Timer.Stop();
        }

        void cb1_Click(object sender, RoutedEventArgs e)
        {
            //CheckBox cb = sender as CheckBox;

            var selfcols1 = dgMatchInfo.Columns[5];//复选框列

            double ProductMax = 1.00;
            double ProductMin = 1.00;

            int i = 0;
            foreach (var items in dgMatchInfo.ItemsSource)
            {
                var cells1 = selfcols1.GetCellContent(items);//复选框列

                Grid Grid1 = cells1 as Grid;

                if ((bool)(Grid1.Children[0] as CheckBox).IsChecked)
                {
                    ProductMax *= SelectedRate[i].Max();
                    ProductMin *= SelectedRate[i].Min();
                }
                i++;
            }

            tbStart.Text = string.Format("{0:f2}", ProductMin).ToString();
            tbEnd.Text = string.Format("{0:f2}", ProductMax).ToString();
        }
    }
}
