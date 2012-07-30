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
using SLS.SilverLight.FilterShrink.ServiceModel;
using System.ServiceModel;

namespace SLS.SilverLight.FilterShrink.ModelChildWindow
{
    public partial class ModelShow : ChildWindow
    {
        public long ID = 0;
        public T_Model Model;

        public ModelShow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            ModelClient mc = new ModelClient(binding, new EndpointAddress(new Uri(Application.Current.Host.Source, "../Silverlight/Model.svc")));

            //通过id获取详细信息    
            mc.GetModelByIDAsync(ID);
            mc.GetModelByIDCompleted += new EventHandler<GetModelByIDCompletedEventArgs>(sc_GetModelByIDCompleted);
        }

        //加载球赛信息 “场次，主场球队，客场球队等”，加载到datagrid(回调函数)
        void sc_GetModelByIDCompleted(object sender, GetModelByIDCompletedEventArgs e)
        {
            Model = new List<T_Model>(e.Result)[0];

            if (Model == null)
            {
                return;
            }

            Grid ModelGrid = new Grid();
            ModelGrid.Width = 325;
            ModelGrid.Height = 352;

            ColumnDefinition cd1 = new ColumnDefinition();
            cd1.Width = new GridLength(108);
            ColumnDefinition cd2 = new ColumnDefinition();
            cd2.Width = new GridLength(108);
            ColumnDefinition cd3 = new ColumnDefinition();
            cd3.Width = new GridLength(108);
            ColumnDefinition cd4 = new ColumnDefinition();
            cd4.Width = new GridLength(108);
            ModelGrid.ColumnDefinitions.Add(cd1);
            ModelGrid.ColumnDefinitions.Add(cd2);
            ModelGrid.ColumnDefinitions.Add(cd3);
            ModelGrid.ColumnDefinitions.Add(cd4);

            RowDefinition rda = new RowDefinition();
            rda.Height = new GridLength(20);
            ModelGrid.RowDefinitions.Add(rda);

            TextBlock lba = new TextBlock();
            lba.Text = "步骤";
            lba.Margin = new Thickness(2);
            lba.VerticalAlignment = VerticalAlignment.Center;
            lba.FontSize = 13;

            Grid.SetColumn(lba, 0);
            Grid.SetRow(lba, 0);
            ModelGrid.Children.Add(lba);

            RowDefinition rda1 = new RowDefinition();
            rda1.Height = new GridLength(20);
            ModelGrid.RowDefinitions.Add(rda1);

            TextBlock lba1 = new TextBlock();
            lba1.Text = "条件名称";
            lba1.Margin = new Thickness(2);
            lba1.VerticalAlignment = VerticalAlignment.Center;
            lba1.FontSize = 13;

            Grid.SetColumn(lba1, 1);
            Grid.SetRow(lba1, 0);
            ModelGrid.Children.Add(lba1);

            RowDefinition rda2 = new RowDefinition();
            rda2.Height = new GridLength(20);
            ModelGrid.RowDefinitions.Add(rda2);

            TextBlock lba2 = new TextBlock();
            lba2.Text = "条件设置";
            lba2.Margin = new Thickness(2);
            lba2.VerticalAlignment = VerticalAlignment.Center;
            lba2.FontSize = 13;

            Grid.SetColumn(lba2, 2);
            Grid.SetRow(lba2, 0);
            ModelGrid.Children.Add(lba2);

            ModleName.Content = Model.Name;
            ModleContent.Content = Model.Descption;

            string[] strContent = Model.Content.Split('b');
            int i = 1;

            foreach (string str in strContent)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }

                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(20);
                ModelGrid.RowDefinitions.Add(rd);

                Label lb = new Label();
                lb.Content = "第 " + str.Substring(0, str.IndexOf('a')) + " 步";
                lb.BorderThickness = new Thickness(1, 0, 0, 0);
                lb.VerticalAlignment = VerticalAlignment.Center;
                lb.FontSize = 11;

                string tempstr = str.Substring(str.IndexOf('a') + 1);

                Grid.SetColumn(lb, 0);
                Grid.SetRow(lb, i);
                Grid.SetRowSpan(lb, tempstr.Split(']').Length);

                ModelGrid.Children.Add(lb);

                string[] strCondent = tempstr.Split(']');

                if (strCondent.Length < 1)
                {
                    continue;
                }

                Brush[] brushs = new Brush[] { new SolidColorBrush(Color.FromArgb(128, 242, 242, 242)), new SolidColorBrush(Color.FromArgb(255, 00, 00, 00)) };

                for (int j = 0; j < strCondent.Length; j++)
                {
                    if (string.IsNullOrEmpty(strCondent[j]))
                    {
                        continue;
                    }

                    RowDefinition rd1 = new RowDefinition();
                    rd1.Height = new GridLength(20);
                    ModelGrid.RowDefinitions.Add(rd1);

                    Label lb1 = new Label();
                    lb1.BorderThickness = new Thickness(1, 0, 0, 0);
                    lb1.VerticalAlignment = VerticalAlignment.Center;
                    lb1.FontSize = 11;
                    //lb1.Background = brushs[0];
                    //lb1.BorderBrush = brushs[1];
                    Grid.SetColumn(lb1, 1);
                    Grid.SetRow(lb1, i);

                    string[] strResult = strCondent[j].Split('@');

                    if (strResult.Length < 2)
                    {
                        continue;
                    }

                    lb1.Content = strResult[0].Replace("[", "");
                    ModelGrid.Children.Add(lb1);

                    Label lb2 = new Label();
                    lb2.BorderThickness = new Thickness(1, 0, 0, 0);
                    lb2.VerticalAlignment = VerticalAlignment.Center;
                    lb2.FontSize = 11;
                    //lb2.Background = brushs[0];
                    //lb2.BorderBrush = brushs[1];
                    Grid.SetColumn(lb2, 2);
                    Grid.SetRow(lb2, i);

                    if (strResult[1].IndexOf(";") >= 0)
                    {
                        lb2.Content = "共设置了 " + strResult[1].Split(';').Length + " 个条件";
                    }
                    else
                    {
                        lb2.Content = strResult[1];
                    }

                    ModelGrid.Children.Add(lb2);

                    i++;
                }
            }

            scorllTable.Content = ModelGrid;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

