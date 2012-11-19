using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;

using CBLabs.CybozuConnect;

namespace Cybozu.CrossSync
{
    public partial class SettingForm : Form
    {
        public readonly string[] intervals = { "0.5", "1", "2", "3", "6", "12" };

        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            Properties.Settings settings = Properties.Settings.Default;

            if (!settings.Upgraded)
            {
                settings.Upgrade();
                settings.Upgraded = true;
                settings.Save();
                settings.Reload();
            }

            SetTimerInterval(settings);
            this.timer.Start();

            CybozuException ex;
            if (Program.CanSync(out ex))
            {
                this.syncButton.Enabled = true;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                ShowMessageIfLicenseError(ex);
            }

            this.syncInterval.Items.AddRange(intervals);
            syncInterval_Reload();

            lastSync_Reload();

            // product name, version number
            this.productName.Text = Application.ProductName;
            this.versionNumber.Text = string.Format("Version {0}", Application.ProductVersion);
        }

        private void SetTimerInterval(Properties.Settings settings)
        {
            this.timer.Interval = Math.Max(30, (int)settings.SyncInterval) * 60 * 1000;
        }

        private void syncInterval_Reload()
        {
            Properties.Settings settings = Properties.Settings.Default;

            int interval = settings.SyncInterval / 60;
            for (int i = 0; i < intervals.Length; i++)
            {
                if (i == 0 && interval < 1)
                {
                    this.syncInterval.SelectedIndex = i;
                }
                else if (i > 0 && interval > 0 && int.Parse(intervals[i]) == interval)
                {
                    this.syncInterval.SelectedIndex = i;
                }
            }
        }

        private void lastSync_Reload()
        {
            Properties.Settings settings = Properties.Settings.Default;

            DateTime lastSynchronized;
            if (!string.IsNullOrEmpty(settings.LastSynchronized) && DateTime.TryParse(settings.LastSynchronized, out lastSynchronized))
            {
                this.lastSync.Text = lastSynchronized.ToString();
            }
            else
            {
                this.lastSync.Text = Resources.NotSynchronized;
            }
        }

        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Visible = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Visible = true;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon.Visible = false;
            Application.Exit();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (Apply())
            {
                this.Visible = false;
            }
        }

        private bool Apply()
        {
            if (string.IsNullOrEmpty(this.firstUrl.Text) && string.IsNullOrEmpty(this.secondUrl.Text))
            {
                this.syncButton.Enabled = false;
                Properties.Settings.Default.Save();
                return true;
            }

            string url1 = TrimUrl(this.firstUrl.Text);
            string url2 = TrimUrl(this.secondUrl.Text);
            App app;

            try
            {
                app = new App(url1);
                app.Auth(this.firstUsername.Text, this.firstPassword.Text);
            }
            catch (CybozuException ex)
            {
                if (!ShowMessageIfLicenseError(ex))
                {
                    MessageBox.Show(string.Format(Resources.Account1Error, ex.Message), Resources.ProductName);
                }
                return false;
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            if (string.IsNullOrEmpty(this.firstPostfix.Text))
            {
                MessageBox.Show(string.Format(Resources.Account1Error, Resources.PostfixIsNecessary), Resources.ProductName);
                return false;
            }

            try
            {
                app = new App(url2);
                app.Auth(this.secondUsername.Text, this.secondPassword.Text);
            }
            catch (CybozuException ex)
            {
                if (!ShowMessageIfLicenseError(ex))
                {
                    MessageBox.Show(string.Format(Resources.Account2Error, ex.Message), Resources.ProductName);
                }
                return false;
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            if (string.IsNullOrEmpty(this.secondPostfix.Text))
            {
                MessageBox.Show(string.Format(Resources.Account2Error, Resources.PostfixIsNecessary), Resources.ProductName);
                return false;
            }

            this.firstUrl.Text = url1;
            this.secondUrl.Text = url2;

            Properties.Settings settings = Properties.Settings.Default;

            int intervalIndex = this.syncInterval.SelectedIndex;
            if (intervalIndex >= 0)
            {
                int nextInterval = (int)(double.Parse(intervals[intervalIndex]) * 60);
                if (settings.SyncInterval != nextInterval)
                {
                    settings.SyncInterval = nextInterval;
                    SetTimerInterval(settings);
                }
            }

            this.syncButton.Enabled = true;
            settings.Save();

            return true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Properties.Settings.Default.Reload();
            syncInterval_Reload();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void syncButton_Click(object sender, EventArgs e)
        {
            Sync(true);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Sync(false);
        }

        protected void Sync(bool showError)
        {
            try
            {
                if (Program.Sync())
                {
                    lastSync_Reload();
                }
            }
            catch (CybozuException ex)
            {
                if (!ShowMessageIfLicenseError(ex) && showError)
                {
                    MessageBox.Show(string.Format("{0} ({1})", ex.Message, ex.Code), Resources.ProductName);
                }
            }
            catch (WebException ex)
            {
                if (showError)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        protected string TrimUrl(string url)
        {
            string[] EndMarks = { ".exe", ".cgi", ".cybozu.com/g/" };
            foreach (string endMark in EndMarks)
            {
                if (url.EndsWith(endMark)) return url;
                int pos = url.IndexOf(endMark);
                if (pos < 0) continue;
                return url.Substring(0, pos + endMark.Length);
            }

            return string.Empty;
        }

        protected bool ShowMessageIfLicenseError(CybozuException ex)
        {
            if (ex == null)
            {
                return false;
            }
            else if (ex.Code == App.ErrorCode.OfficeLicenseError)
            {
                MessageBox.Show(Resources.OfficeLicenseExpires, Resources.ProductName);
            }
            else if (ex.Code == App.ErrorCode.GaroonLicenseError)
            {
                MessageBox.Show(Resources.GaroonLicenseExpires, Resources.ProductName);
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
