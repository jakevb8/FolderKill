using Delimon.Win32.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderKill
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private async void btnKillDirectory_Click(object sender, EventArgs e)
        {
            try
            {               
                DirectoryInfo directoryInfo = new DirectoryInfo(txtDirectory.Text);
                if (directoryInfo.Exists)
                {
                    btnKillDirectory.Enabled = false;
                    btnKillDirectory.Text = "Killing...";
                    await DeleteFilesAsync(directoryInfo, directoryInfo);
                    btnKillDirectory.Enabled = true;
                    btnKillDirectory.Text = "Kill";
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async Task DeleteFilesAsync(DirectoryInfo root, DirectoryInfo currentDirectory)
        {
            await Task.Run(() =>
            {
                DeleteFiles(root, currentDirectory);
                root.Delete(true);
            });
        }
        private void DeleteFiles(DirectoryInfo root, DirectoryInfo currentDirectory)
        {
            FileInfo[] files = currentDirectory.GetFiles();
            foreach (FileInfo fileToMove in files)
            {
                try
                {
                    fileToMove.Delete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            foreach (DirectoryInfo directoryInfo in currentDirectory.GetDirectories())
            {
                DeleteFiles(root, directoryInfo);
            }
        }
    }
}
