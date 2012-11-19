namespace Cybozu.CrossSync
{
    partial class SettingForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.secondGroupBox = new System.Windows.Forms.GroupBox();
            this.secondPostfixLabel = new System.Windows.Forms.Label();
            this.secondPostfix = new System.Windows.Forms.TextBox();
            this.secondPassword = new System.Windows.Forms.TextBox();
            this.secondPasswordLabel = new System.Windows.Forms.Label();
            this.secondUsername = new System.Windows.Forms.TextBox();
            this.secondUsernameLabel = new System.Windows.Forms.Label();
            this.secondUrl = new System.Windows.Forms.TextBox();
            this.secondUrlLabel = new System.Windows.Forms.Label();
            this.firstGroupBox = new System.Windows.Forms.GroupBox();
            this.firstPostfixLabel = new System.Windows.Forms.Label();
            this.firstPostfix = new System.Windows.Forms.TextBox();
            this.firstPassword = new System.Windows.Forms.TextBox();
            this.firstPasswordLabel = new System.Windows.Forms.Label();
            this.firstUsername = new System.Windows.Forms.TextBox();
            this.firstUsernameLabel = new System.Windows.Forms.Label();
            this.firstUrl = new System.Windows.Forms.TextBox();
            this.firstUrlLabel = new System.Windows.Forms.Label();
            this.optionsTabPage = new System.Windows.Forms.TabPage();
            this.syncButton = new System.Windows.Forms.Button();
            this.lastSync = new System.Windows.Forms.Label();
            this.lastSyncLabel = new System.Windows.Forms.Label();
            this.syncIntervalUnit = new System.Windows.Forms.Label();
            this.syncInterval = new System.Windows.Forms.ComboBox();
            this.syncIntervalLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.qualifiedEvents = new System.Windows.Forms.CheckBox();
            this.privateEvents = new System.Windows.Forms.CheckBox();
            this.temporaryEvents = new System.Windows.Forms.CheckBox();
            this.bannerEvents = new System.Windows.Forms.CheckBox();
            this.allDayEvents = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.productName = new System.Windows.Forms.Label();
            this.versionNumber = new System.Windows.Forms.Label();
            this.notifyIconMenu.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.secondGroupBox.SuspendLayout();
            this.firstGroupBox.SuspendLayout();
            this.optionsTabPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconMenu;
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // notifyIconMenu
            // 
            this.notifyIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.notifyIconMenu.Name = "notifyIconMenu";
            resources.ApplyResources(this.notifyIconMenu, "notifyIconMenu");
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            resources.ApplyResources(this.quitToolStripMenuItem, "quitToolStripMenuItem");
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.generalTabPage);
            this.tabControl.Controls.Add(this.optionsTabPage);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.secondGroupBox);
            this.generalTabPage.Controls.Add(this.firstGroupBox);
            resources.ApplyResources(this.generalTabPage, "generalTabPage");
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.UseVisualStyleBackColor = true;
            // 
            // secondGroupBox
            // 
            this.secondGroupBox.Controls.Add(this.secondPostfixLabel);
            this.secondGroupBox.Controls.Add(this.secondPostfix);
            this.secondGroupBox.Controls.Add(this.secondPassword);
            this.secondGroupBox.Controls.Add(this.secondPasswordLabel);
            this.secondGroupBox.Controls.Add(this.secondUsername);
            this.secondGroupBox.Controls.Add(this.secondUsernameLabel);
            this.secondGroupBox.Controls.Add(this.secondUrl);
            this.secondGroupBox.Controls.Add(this.secondUrlLabel);
            resources.ApplyResources(this.secondGroupBox, "secondGroupBox");
            this.secondGroupBox.Name = "secondGroupBox";
            this.secondGroupBox.TabStop = false;
            // 
            // secondPostfixLabel
            // 
            resources.ApplyResources(this.secondPostfixLabel, "secondPostfixLabel");
            this.secondPostfixLabel.Name = "secondPostfixLabel";
            // 
            // secondPostfix
            // 
            this.secondPostfix.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Cybozu.CrossSync.Properties.Settings.Default, "SecondPostfix", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.secondPostfix, "secondPostfix");
            this.secondPostfix.Name = "secondPostfix";
            this.secondPostfix.Text = global::Cybozu.CrossSync.Properties.Settings.Default.SecondPostfix;
            // 
            // secondPassword
            // 
            this.secondPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Cybozu.CrossSync.Properties.Settings.Default, "SecondPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.secondPassword, "secondPassword");
            this.secondPassword.Name = "secondPassword";
            this.secondPassword.Text = global::Cybozu.CrossSync.Properties.Settings.Default.SecondPassword;
            // 
            // secondPasswordLabel
            // 
            resources.ApplyResources(this.secondPasswordLabel, "secondPasswordLabel");
            this.secondPasswordLabel.Name = "secondPasswordLabel";
            // 
            // secondUsername
            // 
            this.secondUsername.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Cybozu.CrossSync.Properties.Settings.Default, "SecondUsername", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.secondUsername, "secondUsername");
            this.secondUsername.Name = "secondUsername";
            this.secondUsername.Text = global::Cybozu.CrossSync.Properties.Settings.Default.SecondUsername;
            // 
            // secondUsernameLabel
            // 
            resources.ApplyResources(this.secondUsernameLabel, "secondUsernameLabel");
            this.secondUsernameLabel.Name = "secondUsernameLabel";
            // 
            // secondUrl
            // 
            this.secondUrl.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Cybozu.CrossSync.Properties.Settings.Default, "SecondUrl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.secondUrl, "secondUrl");
            this.secondUrl.Name = "secondUrl";
            this.secondUrl.Text = global::Cybozu.CrossSync.Properties.Settings.Default.SecondUrl;
            // 
            // secondUrlLabel
            // 
            resources.ApplyResources(this.secondUrlLabel, "secondUrlLabel");
            this.secondUrlLabel.Name = "secondUrlLabel";
            // 
            // firstGroupBox
            // 
            this.firstGroupBox.Controls.Add(this.firstPostfixLabel);
            this.firstGroupBox.Controls.Add(this.firstPostfix);
            this.firstGroupBox.Controls.Add(this.firstPassword);
            this.firstGroupBox.Controls.Add(this.firstPasswordLabel);
            this.firstGroupBox.Controls.Add(this.firstUsername);
            this.firstGroupBox.Controls.Add(this.firstUsernameLabel);
            this.firstGroupBox.Controls.Add(this.firstUrl);
            this.firstGroupBox.Controls.Add(this.firstUrlLabel);
            resources.ApplyResources(this.firstGroupBox, "firstGroupBox");
            this.firstGroupBox.Name = "firstGroupBox";
            this.firstGroupBox.TabStop = false;
            // 
            // firstPostfixLabel
            // 
            resources.ApplyResources(this.firstPostfixLabel, "firstPostfixLabel");
            this.firstPostfixLabel.Name = "firstPostfixLabel";
            // 
            // firstPostfix
            // 
            this.firstPostfix.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Cybozu.CrossSync.Properties.Settings.Default, "FirstPostfix", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.firstPostfix, "firstPostfix");
            this.firstPostfix.Name = "firstPostfix";
            this.firstPostfix.Text = global::Cybozu.CrossSync.Properties.Settings.Default.FirstPostfix;
            // 
            // firstPassword
            // 
            this.firstPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Cybozu.CrossSync.Properties.Settings.Default, "FirstPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.firstPassword, "firstPassword");
            this.firstPassword.Name = "firstPassword";
            this.firstPassword.Text = global::Cybozu.CrossSync.Properties.Settings.Default.FirstPassword;
            // 
            // firstPasswordLabel
            // 
            resources.ApplyResources(this.firstPasswordLabel, "firstPasswordLabel");
            this.firstPasswordLabel.Name = "firstPasswordLabel";
            // 
            // firstUsername
            // 
            this.firstUsername.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Cybozu.CrossSync.Properties.Settings.Default, "FirstUsername", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.firstUsername, "firstUsername");
            this.firstUsername.Name = "firstUsername";
            this.firstUsername.Text = global::Cybozu.CrossSync.Properties.Settings.Default.FirstUsername;
            // 
            // firstUsernameLabel
            // 
            resources.ApplyResources(this.firstUsernameLabel, "firstUsernameLabel");
            this.firstUsernameLabel.Name = "firstUsernameLabel";
            // 
            // firstUrl
            // 
            this.firstUrl.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Cybozu.CrossSync.Properties.Settings.Default, "FirstUrl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.firstUrl, "firstUrl");
            this.firstUrl.Name = "firstUrl";
            this.firstUrl.Text = global::Cybozu.CrossSync.Properties.Settings.Default.FirstUrl;
            // 
            // firstUrlLabel
            // 
            resources.ApplyResources(this.firstUrlLabel, "firstUrlLabel");
            this.firstUrlLabel.Name = "firstUrlLabel";
            // 
            // optionsTabPage
            // 
            this.optionsTabPage.Controls.Add(this.syncButton);
            this.optionsTabPage.Controls.Add(this.lastSync);
            this.optionsTabPage.Controls.Add(this.lastSyncLabel);
            this.optionsTabPage.Controls.Add(this.syncIntervalUnit);
            this.optionsTabPage.Controls.Add(this.syncInterval);
            this.optionsTabPage.Controls.Add(this.syncIntervalLabel);
            this.optionsTabPage.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.optionsTabPage, "optionsTabPage");
            this.optionsTabPage.Name = "optionsTabPage";
            this.optionsTabPage.UseVisualStyleBackColor = true;
            // 
            // syncButton
            // 
            resources.ApplyResources(this.syncButton, "syncButton");
            this.syncButton.Name = "syncButton";
            this.syncButton.UseVisualStyleBackColor = true;
            this.syncButton.Click += new System.EventHandler(this.syncButton_Click);
            // 
            // lastSync
            // 
            resources.ApplyResources(this.lastSync, "lastSync");
            this.lastSync.Name = "lastSync";
            // 
            // lastSyncLabel
            // 
            resources.ApplyResources(this.lastSyncLabel, "lastSyncLabel");
            this.lastSyncLabel.Name = "lastSyncLabel";
            // 
            // syncIntervalUnit
            // 
            resources.ApplyResources(this.syncIntervalUnit, "syncIntervalUnit");
            this.syncIntervalUnit.Name = "syncIntervalUnit";
            // 
            // syncInterval
            // 
            this.syncInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.syncInterval.FormattingEnabled = true;
            resources.ApplyResources(this.syncInterval, "syncInterval");
            this.syncInterval.Name = "syncInterval";
            // 
            // syncIntervalLabel
            // 
            resources.ApplyResources(this.syncIntervalLabel, "syncIntervalLabel");
            this.syncIntervalLabel.Name = "syncIntervalLabel";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.qualifiedEvents);
            this.groupBox1.Controls.Add(this.privateEvents);
            this.groupBox1.Controls.Add(this.temporaryEvents);
            this.groupBox1.Controls.Add(this.bannerEvents);
            this.groupBox1.Controls.Add(this.allDayEvents);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // qualifiedEvents
            // 
            resources.ApplyResources(this.qualifiedEvents, "qualifiedEvents");
            this.qualifiedEvents.Checked = global::Cybozu.CrossSync.Properties.Settings.Default.Qualified;
            this.qualifiedEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.qualifiedEvents.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Cybozu.CrossSync.Properties.Settings.Default, "Qualified", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.qualifiedEvents.Name = "qualifiedEvents";
            this.qualifiedEvents.UseVisualStyleBackColor = true;
            // 
            // privateEvents
            // 
            resources.ApplyResources(this.privateEvents, "privateEvents");
            this.privateEvents.Checked = global::Cybozu.CrossSync.Properties.Settings.Default.Private;
            this.privateEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.privateEvents.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Cybozu.CrossSync.Properties.Settings.Default, "Private", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.privateEvents.Name = "privateEvents";
            this.privateEvents.UseVisualStyleBackColor = true;
            // 
            // temporaryEvents
            // 
            resources.ApplyResources(this.temporaryEvents, "temporaryEvents");
            this.temporaryEvents.Checked = global::Cybozu.CrossSync.Properties.Settings.Default.Temporary;
            this.temporaryEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.temporaryEvents.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Cybozu.CrossSync.Properties.Settings.Default, "Temporary", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.temporaryEvents.Name = "temporaryEvents";
            this.temporaryEvents.UseVisualStyleBackColor = true;
            // 
            // bannerEvents
            // 
            resources.ApplyResources(this.bannerEvents, "bannerEvents");
            this.bannerEvents.Checked = global::Cybozu.CrossSync.Properties.Settings.Default.Banner;
            this.bannerEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bannerEvents.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Cybozu.CrossSync.Properties.Settings.Default, "Banner", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bannerEvents.Name = "bannerEvents";
            this.bannerEvents.UseVisualStyleBackColor = true;
            // 
            // allDayEvents
            // 
            resources.ApplyResources(this.allDayEvents, "allDayEvents");
            this.allDayEvents.Checked = global::Cybozu.CrossSync.Properties.Settings.Default.AllDay;
            this.allDayEvents.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allDayEvents.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Cybozu.CrossSync.Properties.Settings.Default, "AllDay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.allDayEvents.Name = "allDayEvents";
            this.allDayEvents.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // applyButton
            // 
            resources.ApplyResources(this.applyButton, "applyButton");
            this.applyButton.Name = "applyButton";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // productName
            // 
            resources.ApplyResources(this.productName, "productName");
            this.productName.Name = "productName";
            // 
            // versionNumber
            // 
            resources.ApplyResources(this.versionNumber, "versionNumber");
            this.versionNumber.Name = "versionNumber";
            // 
            // SettingForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.versionNumber);
            this.Controls.Add(this.productName);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.notifyIconMenu.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.secondGroupBox.ResumeLayout(false);
            this.secondGroupBox.PerformLayout();
            this.firstGroupBox.ResumeLayout(false);
            this.firstGroupBox.PerformLayout();
            this.optionsTabPage.ResumeLayout(false);
            this.optionsTabPage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenu;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.TabPage optionsTabPage;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.GroupBox secondGroupBox;
        private System.Windows.Forms.GroupBox firstGroupBox;
        private System.Windows.Forms.Label firstPostfixLabel;
        private System.Windows.Forms.TextBox firstPostfix;
        private System.Windows.Forms.TextBox firstPassword;
        private System.Windows.Forms.Label firstPasswordLabel;
        private System.Windows.Forms.TextBox firstUsername;
        private System.Windows.Forms.Label firstUsernameLabel;
        private System.Windows.Forms.TextBox firstUrl;
        private System.Windows.Forms.Label firstUrlLabel;
        private System.Windows.Forms.Label secondPostfixLabel;
        private System.Windows.Forms.TextBox secondPostfix;
        private System.Windows.Forms.TextBox secondPassword;
        private System.Windows.Forms.Label secondPasswordLabel;
        private System.Windows.Forms.TextBox secondUsername;
        private System.Windows.Forms.Label secondUsernameLabel;
        private System.Windows.Forms.TextBox secondUrl;
        private System.Windows.Forms.Label secondUrlLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox qualifiedEvents;
        private System.Windows.Forms.CheckBox privateEvents;
        private System.Windows.Forms.CheckBox temporaryEvents;
        private System.Windows.Forms.CheckBox bannerEvents;
        private System.Windows.Forms.CheckBox allDayEvents;
        private System.Windows.Forms.Button syncButton;
        private System.Windows.Forms.Label lastSync;
        private System.Windows.Forms.Label lastSyncLabel;
        private System.Windows.Forms.Label syncIntervalUnit;
        private System.Windows.Forms.ComboBox syncInterval;
        private System.Windows.Forms.Label syncIntervalLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label productName;
        private System.Windows.Forms.Label versionNumber;
    }
}

