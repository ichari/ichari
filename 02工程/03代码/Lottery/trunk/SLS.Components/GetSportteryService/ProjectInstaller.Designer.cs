namespace GetSportteryService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.GetSportteryService = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // GetSportteryService
            // 
            this.GetSportteryService.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.GetSportteryService.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceInstaller1});
            this.GetSportteryService.Password = null;
            this.GetSportteryService.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "竞彩数据抓取服务";
            this.serviceInstaller1.ServiceName = "GetSportteryService";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.GetSportteryService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller GetSportteryService;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}