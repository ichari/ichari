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

using System.Collections;
using SLS.SilverLight.FilterShrink.ServiceModel;
using System.ServiceModel;

namespace SLS.SilverLight.FilterShrink.ModelChildWindow
{
    public partial class SaveChildWindow : ChildWindow
    {
        public Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
        public string ModlePlayTypeName = "";
        public string TypeName = "";
        public string strContent = "";
        public long UserID = 0;
        public string PlayType = "";

        public SaveChildWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            int i = 1;
            int M = 1;
            strContent = "";

            Grid SaveModelGrid = new Grid();
            SaveModelGrid.Width = 325;
            SaveModelGrid.Height = 352;

            ColumnDefinition cd1 = new ColumnDefinition();
            cd1.Width = new GridLength(108);
            ColumnDefinition cd2 = new ColumnDefinition();
            cd2.Width = new GridLength(108);
            ColumnDefinition cd3 = new ColumnDefinition();
            cd3.Width = new GridLength(108);
            ColumnDefinition cd4 = new ColumnDefinition();
            cd4.Width = new GridLength(108);
            SaveModelGrid.ColumnDefinitions.Add(cd1);
            SaveModelGrid.ColumnDefinitions.Add(cd2);
            SaveModelGrid.ColumnDefinitions.Add(cd3);
            SaveModelGrid.ColumnDefinitions.Add(cd4);

            RowDefinition rda = new RowDefinition();
            rda.Height = new GridLength(20);
            SaveModelGrid.RowDefinitions.Add(rda);

            TextBlock lba = new TextBlock();
            lba.Text = "步骤";
            lba.Margin = new Thickness(2);
            lba.VerticalAlignment = VerticalAlignment.Center;
            lba.FontSize = 13;

            Grid.SetColumn(lba, 0);
            Grid.SetRow(lba, 0);
            SaveModelGrid.Children.Add(lba);

            RowDefinition rda1 = new RowDefinition();
            rda1.Height = new GridLength(20);
            SaveModelGrid.RowDefinitions.Add(rda1);

            TextBlock lba1 = new TextBlock();
            lba1.Text = "条件名称";
            lba1.Margin = new Thickness(2);
            lba1.VerticalAlignment = VerticalAlignment.Center;
            lba1.FontSize = 13;

            Grid.SetColumn(lba1, 1);
            Grid.SetRow(lba1, 0);
            SaveModelGrid.Children.Add(lba1);

            RowDefinition rda2 = new RowDefinition();
            rda2.Height = new GridLength(20);
            SaveModelGrid.RowDefinitions.Add(rda2);

            TextBlock lba2 = new TextBlock();
            lba2.Text = "条件设置";
            lba2.Margin = new Thickness(2);
            lba2.VerticalAlignment = VerticalAlignment.Center;
            lba2.FontSize = 13;

            Grid.SetColumn(lba2, 2);
            Grid.SetRow(lba2, 0);
            SaveModelGrid.Children.Add(lba2);

            foreach (KeyValuePair<int, List<string>> kv in dictionary)
            {
                if (kv.Value.Count < 1)
                {
                    continue;
                }

                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(20);
                SaveModelGrid.RowDefinitions.Add(rd);

                Label lb = new Label();
                lb.Content = "第 " + M.ToString() + " 步";
                lb.BorderThickness = new Thickness(1, 0, 0, 0);
                lb.VerticalAlignment = VerticalAlignment.Center;
                lb.FontSize = 11;

                Grid.SetColumn(lb, 0);
                Grid.SetRow(lb, i);
                Grid.SetRowSpan(lb, kv.Value.Count);

                SaveModelGrid.Children.Add(lb);

                strContent += kv.Key.ToString() + "a";

                for (int j = 0; j < kv.Value.Count; j++)
                {
                    RowDefinition rd1 = new RowDefinition();
                    rd1.Height = new GridLength(20);
                    SaveModelGrid.RowDefinitions.Add(rd1);

                    Label lb1 = new Label();
                    lb1.BorderThickness = new Thickness(1, 0, 0, 0);
                    lb1.VerticalAlignment = VerticalAlignment.Center;
                    lb1.FontSize = 11;
                    Grid.SetColumn(lb1, 1);
                    Grid.SetRow(lb1, i);

                    lb1.Content = kv.Value[j].Split('@')[0];
                    SaveModelGrid.Children.Add(lb1);

                    Label lb2 = new Label();
                    lb2.BorderThickness = new Thickness(1, 0, 0, 0);
                    lb2.VerticalAlignment = VerticalAlignment.Center;
                    lb2.FontSize = 11;
                    Grid.SetColumn(lb2, 2);
                    Grid.SetRow(lb2, i);

                    if (kv.Value[j].Split('@')[1].IndexOf(";") >= 0)
                    {
                        lb2.Content = "共设置了 " + kv.Value[j].Split('@')[1].Split(';').Length + " 个条件";
                    }
                    else
                    {
                        lb2.Content = kv.Value[j].Split('@')[1];
                    }

                    strContent += "[" + kv.Value[j].Split('@')[0] + "@" + kv.Value[j].Split('@')[1] + "]";

                    SaveModelGrid.Children.Add(lb2);

                    i++;
                }

                if (strContent.EndsWith("*"))
                {
                    strContent = strContent.Substring(0, strContent.Length - 1);
                }

                strContent += "b";

                M++;
            }

            if (strContent.EndsWith("b"))
            {
                strContent = strContent.Substring(0, strContent.Length - 1);
            }

            scorllTable.Content = SaveModelGrid;
        }

        //确定按钮
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ModleName.Text))
            {
                MessageBox.Show("请输入模型名字！");
                return;
            }

            if (string.IsNullOrEmpty(ModleContent.Text))
            {
                MessageBox.Show("请输入模型描述内容！");
                return;
            }

            if (ModleName.Text.Trim().Length > 400)
            {
                MessageBox.Show("输入模型名字字数太长，请重新输入！");
                return;
            }

            if (ModleName.Text.Trim().Length > 3000)
            {
                MessageBox.Show("模型描述内容字数太长，请重新输入！");
                return;
            }


            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            ModelClient mc = new ModelClient(binding, new EndpointAddress(new Uri(Application.Current.Host.Source, "../Silverlight/Model.svc")));

            //通过id获取详细信息
            mc.InsertModelAsync(UserID, PlayType, ModleName.Text.Trim(), strContent, ModleContent.Text.Trim(), TypeName);
            mc.InsertModelCompleted += new EventHandler<InsertModelCompletedEventArgs>(mc_InsertModelCompleted);

            this.DialogResult = true;
        }

        //加载球赛信息 “场次，主场球队，客场球队等”，加载到datagrid(回调函数)
        void mc_InsertModelCompleted(object sender, InsertModelCompletedEventArgs e)
        {
            if (e.Result >= 0)
            {
                MessageBox.Show("添加成功！");
            }
            else
            {
                MessageBox.Show("添加失败，请重新添加！");
            }
        }

        //取消按钮
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
