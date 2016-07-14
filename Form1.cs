using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace PcapPlus
{
    public partial class Form1 : Form
    {
          string[] files = new string[0];
          string wpath = "";
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DialogResult result = folderBrowserDialog1.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                //
                // The user selected a folder and pressed the OK button.
                // We print the number of files found.
                //
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, comboBox1.SelectedItem.ToString());
                //foreach file as string in files
                //{
                listBox1.Items.AddRange(files);
                //}
                lblStatus.Text = String.Format("{0} {1} files ready to be joined. Press Convert to pcap above...", files.Length, comboBox1.SelectedItem);
            }
            else if (result == DialogResult.Cancel)
            {
                myfunction();    
            }
            if (files.Length > 0)
            {
                this.button2.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myfunction();
        }
        private void myfunction()
        {
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            files = new string[0];
            listBox1.Items.Clear();
            this.button2.Enabled = false;
            lblStatus.Text = "Please browse and select a folder";
            textBox1.Text = "No folder selected";
            button3.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            myfunction();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // "C:\Program Files\Wireshark\editcap.exe"
            // "C:\Program Files(x86)\Wireshark\editcap.exe"
            if(File.Exists("C:\\Program Files\\Wireshark\\editcap.exe"))
            {
                wpath = "C:\\Program Files\\Wireshark";
                convert(wpath);
            }
            else if (File.Exists("C:\\Program Files(x86)\\Wireshark\\editcap.exe"))
            {
                wpath = "C:\\Program Files(x86)\\Wireshark";
                convert(wpath);
            }
            else
            {
                lblStatus.Text = "Wireshark not found to be installed. Please browse for wireshark";
                button3.Enabled = true;
                browseforwireshark();
            }

        }
        private void browseforwireshark()
        {
            DialogResult result = folderBrowserDialog2.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (File.Exists(folderBrowserDialog2.SelectedPath + "\\editcap.exe"))
                {
                    wpath = folderBrowserDialog2.SelectedPath;
                    convert(wpath);
                }
                else
                {
                    lblStatus.Text = "Improper folder selected. Please select Wireshark installation folder";
                }
            }
        }
        private void convert(string path)
        {
            try
            {
                foreach (string file in files)
                {
                    Process.Start(path + "\\editcap.exe", String.Format("{0} {1}", file, file.Replace(comboBox1.SelectedItem.ToString().Replace("*", ""), ".pcap")));
                    File.AppendAllText("PcapPlus.log", String.Format("{0}\\editcap.exe {1}", path, String.Format("{0} {1}", file, file.Replace(comboBox1.SelectedItem.ToString().Replace("*",""), ".pcap"))));
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Exception: " + ex.Message.ToString());
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start(wpath + "\\mergecap.exe", "-F pcap -w merged.pcap " + folderBrowserDialog1.SelectedPath + "\\*.pcap");
        }


     

     

       
    }
}
