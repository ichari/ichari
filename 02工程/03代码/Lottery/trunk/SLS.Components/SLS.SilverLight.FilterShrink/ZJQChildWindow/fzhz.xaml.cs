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

namespace SLS.SilverLight.FilterShrink.ZJQChildWindow
{
    public partial class fzhz : ChildWindow
    {
        public StackPanel StackPanel;

        public fzhz()
        {
            InitializeComponent();

            #region 按钮的事件绑定
            btnAdd.Click += new RoutedEventHandler(btnAdd_Click);

            btnAllDel.Click += new RoutedEventHandler(btnAllDel_Click);

            btnDelete.Click += new RoutedEventHandler(btnDelete_Click);

            btnShift.Click += new RoutedEventHandler(btnShift_Click);
            #endregion
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
            string Start = cbStart.SelectedItem.ToString();
            string End = cbEnd.SelectedItem.ToString();

            string Result = Str + "|" + Start + "-" + End;

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
            //throw new NotImplementedException();
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
            string Start = cbStart.SelectedItem.ToString();
            string End = cbEnd.SelectedItem.ToString();

            string Result = Str + "|" + Start + "-" + End;
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
            dgMatchInfo.ItemsSource = NetBallList;
            StackPanel = sp;

            this.tbName.Text = "<=所选场次" + Title.ToString() + "<=";
        }

        //取消GataGrid复选框选中
        public void ReSetGataGrid(string Result)
        {
            var selfcols = dgMatchInfo.Columns[4];
            foreach (var items in dgMatchInfo.ItemsSource)
            {
                var cells1 = selfcols.GetCellContent(items);
                Grid Grid = cells1 as Grid;
                (Grid.Children[0] as CheckBox).IsChecked = false;
            }

            cbStart.Items.Clear();
            cbEnd.Items.Clear();
            cbStart.Items.Add("0");
            cbEnd.Items.Add("0");
            cbStart.SelectedIndex = 0;
            cbEnd.SelectedIndex = 0;

            lbResult.Items.Clear();
            string[] Results = Result.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string res in Results)
            {
                lbResult.Items.Add(res);
            }
        }

        void cb1_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            var selfcols = dgMatchInfo.Columns[4];
            int Count = 0;
            foreach (var items in dgMatchInfo.ItemsSource)
            {
                var cells1 = selfcols.GetCellContent(items);
                Grid Grid = cells1 as Grid;

                if ((bool)(Grid.Children[0] as CheckBox).IsChecked)
                {
                    Count++;
                }
            }

            cbStart.Items.Clear();
            cbEnd.Items.Clear();
            for (int i = 0; i <= Count * 7; i++)
            {
                cbStart.Items.Add(i);
                cbEnd.Items.Add(i);
            }
            cbStart.SelectedIndex = 0;
            cbEnd.SelectedIndex = 0;
        }
    }
}

