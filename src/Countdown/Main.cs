using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Countdown
{
    public partial class Main : Form
    {
        private readonly int _mainHeigth;
        private readonly int _mainWidth;
        private DateTime _currentDateTime;
        readonly RegistryKey _rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public Main()
        {
            InitializeComponent();
            _mainHeigth = Height;
            _mainWidth = Width;
            _currentDateTime = DateTime.Now;
            // Check to see the current state (running at startup or not)
            if (_rkApp.GetValue("YaniMaritoCountdown") == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                // Add the value in the registry so that the application runs at startup
                _rkApp.SetValue("YaniMaritoCountdown", Application.ExecutablePath);
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            int screenWidth = Screen.GetWorkingArea(new Form()).Width;
            int screenHeight = Screen.GetWorkingArea(new Form()).Height;
            Top = screenHeight - _mainHeigth - 5;
            Left = screenWidth - _mainWidth - 5;
            Hide();
            ShowCountdown();
            WindowState = FormWindowState.Normal;
        }

        private void ShowWindow()
        {
            Opacity = 0;
            Show();
            timerAnimation.Enabled = true;
        }

        private void ShowCountdown()
        {
            if (!Visible)
            {
                ShowWindow();
            }
            _currentDateTime = DateTime.Now;
            DateTime theDate = new DateTime(2007, 11, 24, 21, 0, 0);
            if (theDate > _currentDateTime)
            {
                TimeSpan dtCountdown = theDate.Subtract(_currentDateTime); 
                lblDays.Text = dtCountdown.Days.ToString();
                lblHours.Text = dtCountdown.Hours.ToString();
                lblMinutes.Text = dtCountdown.Minutes.ToString();
            }
            else
            {
                MessageBox.Show("Se cumplió el tiempo!!!");
            }
            pnlCountdown.Show();
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (_currentDateTime.Hour != now.Hour || _currentDateTime.Minute != now.Minute)
            {
                ShowCountdown();
            }
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCountdown();
        }

        private void pnlCountdown_Click(object sender, EventArgs e)
        {
            HideApplication();
        }

        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            if (Opacity < 1.0)
            {
                Opacity += 0.05;
            }
            else
            {
                timerAnimation.Enabled = false;
            }
        }

        private void timerMessage_Tick(object sender, EventArgs e)
        {
            if(!Visible)
            {
                notifyIcon.ShowBalloonTip(15000);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            HideApplication();
        }

        private void HideApplication()
        {
            Hide();
            timer.Enabled = false;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.close01;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.close;
        }
    }
}