using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;


namespace wallUpdate
{
    public partial class RedditWallerConfig : Form
    {
        private bool enabled = false;
        string config;
        public RedditWallerConfig()
        {
            InitializeComponent();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
                all.Text += comboBox1.Text + ";";
            comboBox1.SelectedIndex = 0;
        }


        private void update_Click(object sender, EventArgs e)
        {
            if (all.Text == "")
                ErrorLabel.Text = "You must select one+ subreddit";
            else
            {
                if (enabled)
                {
                    foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
                        if (myProc.ProcessName == "WallUpdate")
                        {
                            myProc.Kill();
                        }
                }
                string[] lines = { all.Text, numericUpDown1.Value.ToString(), enabled.ToString(), nsfw.Checked.ToString() };
                bool shouldTry = true;
                while (shouldTry)
                {
                    try
                    {
                        System.IO.File.WriteAllLines(config, lines);
                        shouldTry = false;
                    }
                    catch (Exception Te) { shouldTry = true; }
                }
                if (enabled)
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.FileName = System.IO.Path.GetFullPath("WallRunner/WallUpdate.exe");
                    process.Start();
                }
            }
        }
 

        private void RedditWallerConfig_Load(object sender, EventArgs e)
        {
            bool en;
            config = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("RedditWaller.exe","WallRunner\\config");
            try
            {
                StreamReader sr = new StreamReader(config);

                all.Text = sr.ReadLine();
                numericUpDown1.Value = Convert.ToInt32(sr.ReadLine());
                en = Convert.ToBoolean(sr.ReadLine());
                nsfw.Checked = Convert.ToBoolean(sr.ReadLine());
                sr.Close();
                checkBox1.Checked = en;}
            catch (Exception le) {   }
            finally{
                    string startUp = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                    using (StreamWriter writer = new StreamWriter(startUp + "\\" + "RedditWaller" + ".url"))
                    {
                        string app = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("RedditWaller.exe", "WallRunner/Start.exe");
                        writer.WriteLine("[InternetShortcut]");
                        writer.WriteLine("URL=file:///" + app);
                        writer.WriteLine("IconIndex=0");
                        string icon = app.Replace('\\', '/');
                        writer.WriteLine("IconFile=" + icon);
                        writer.Flush();
                    }
                }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
                enabled = checkBox1.Checked;
                if (enabled)
                    checkBox1.Text = "enable";
                else
                    checkBox1.Text = "disabled";
                if (enabled)
                {
                    if (all.Text == "")
                        ErrorLabel.Text = "You must select one+ subreddit";
                    else
                    {
                        bool isRunning = false;
                        foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
                            if (myProc.ProcessName == "WallUpdate")
                            {
                                isRunning = true;
                            }
                        ErrorLabel.Text = "";
                        string[] lines = { all.Text, numericUpDown1.Value.ToString(), enabled.ToString(), nsfw.Checked.ToString() };
                        System.IO.File.WriteAllLines(config, lines);
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        if (!isRunning)
                        {
                            process.StartInfo.RedirectStandardOutput = true;
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.CreateNoWindow = true;
                            process.StartInfo.FileName = config.Replace("config", "WallUpdate.exe");
                            process.Start();
                        }
                    }
                }
                else
                {
                    foreach (System.Diagnostics.Process myProc in System.Diagnostics.Process.GetProcesses())
                        if (myProc.ProcessName == "WallUpdate")
                        {
                            myProc.Kill();
                        }
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            all.Text = "";
        }

        private void nsfw_CheckedChanged(object sender, EventArgs e)
        {
            if (nsfw.Checked)
                consent.Text = "*You consent to being 18+";
            else
                consent.Text = "";
        }

        private void Donate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String url = "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=9Z2MV7BDVEDYW";

            System.Diagnostics.Process.Start(url);
        }

    }
        
}
