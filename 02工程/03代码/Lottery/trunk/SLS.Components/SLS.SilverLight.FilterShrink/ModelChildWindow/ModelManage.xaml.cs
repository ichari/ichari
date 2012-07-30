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
    public partial class ModelManage : ChildWindow
    {
        public long UserID;
        public string PlayTypeID;
        public string TypeName;
        public long ModelID = 0;

        public ModelManage()
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
            mc.GetModelAsync(UserID, PlayTypeID, TypeName);
            mc.GetModelCompleted += new EventHandler<GetModelCompletedEventArgs>(sc_GetModelCompleted);
        }

        //加载球赛信息 “场次，主场球队，客场球队等”，加载到datagrid(回调函数)
        void sc_GetModelCompleted(object sender, GetModelCompletedEventArgs e)
        {
            List<T_Model> Model = new List<T_Model>(e.Result);

            this.DataGrid1.ItemsSource = Model;
        }

        private void DataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_Model bindData = (T_Model)e.Row.DataContext;
            Button btn = DataGrid1.Columns[2].GetCellContent(e.Row).FindName("Button1") as Button;
            btn.Tag = bindData.id;

            btn = DataGrid1.Columns[3].GetCellContent(e.Row).FindName("btn_Del") as Button;
            btn.Tag = bindData.id;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string t = b.Tag.ToString();
            ModelID = long.Parse(t);

            this.DialogResult = true;
        }

        private void btn_Del_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string t = b.Tag.ToString();

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            ModelClient mc = new ModelClient(binding, new EndpointAddress(new Uri(Application.Current.Host.Source, "../Silverlight/Model.svc")));

            //通过id获取详细信息    
            mc.DelModelByIDAsync(long.Parse(t));
            mc.DelModelByIDCompleted += new EventHandler<DelModelByIDCompletedEventArgs>(sc_DelModelByIDCompleted);
        }

        void sc_DelModelByIDCompleted(object sender, DelModelByIDCompletedEventArgs e)
        {
            if (e.Result < 0)
            {
                MessageBox.Show("删除失败！");

                return;
            }

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            ModelClient mc = new ModelClient(binding, new EndpointAddress(new Uri(Application.Current.Host.Source, "../Silverlight/Model.svc")));

            //通过id获取详细信息    
            mc.GetModelAsync(UserID, PlayTypeID, TypeName);
            mc.GetModelCompleted += new EventHandler<GetModelCompletedEventArgs>(sc_GetModelCompleted);
        }
    }
}

