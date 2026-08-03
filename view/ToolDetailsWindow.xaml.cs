using System;
using System.IO;
using System.Windows;

namespace ToolkitApp.Views
{
    public partial class ToolDetailsWindow : Window
    {
        private ToolItem _tool;

        public ToolDetailsWindow(ToolItem tool)
        {
            InitializeComponent();
            _tool = tool;

            // Load data into UI
            txtName.Text = _tool.Name;
            txtCommand.Text = _tool.Command;
            
            if (_tool.Shell.Equals("powershell", StringComparison.OrdinalIgnoreCase))
                cmbShell.SelectedIndex = 1;
            else
                cmbShell.SelectedIndex = 0;

            LoadReadme();
        }

        private void LoadReadme()
        {
            txtReadme.Text = "Attempting to locate README documentation...\n\n";

            try
            {
                string command = _tool.Command.Trim('"', ' ');
                string targetDir = null;

                // Scenario 1: It's a GitHub clone command (e.g. 'git clone https://github.com/user/repo')
                if (command.StartsWith("git clone ", StringComparison.OrdinalIgnoreCase))
                {
                    string url = command.Substring(10).Trim();
                    string[] parts = url.Split('/');
                    if (parts.Length > 0)
                    {
                        string repoName = parts[parts.Length - 1];
                        if (repoName.EndsWith(".git")) repoName = repoName.Substring(0, repoName.Length - 4);
                        
                        targetDir = Path.Combine(Environment.CurrentDirectory, repoName);
                    }
                }
                // Scenario 2: It's an absolute path to an executable
                else if (File.Exists(command))
                {
                    targetDir = Path.GetDirectoryName(command);
                }

                if (!string.IsNullOrEmpty(targetDir) && Directory.Exists(targetDir))
                {
                    string readmePath = Path.Combine(targetDir, "README.md");
                    if (!File.Exists(readmePath))
                        readmePath = Path.Combine(targetDir, "readme.txt");
                    if (!File.Exists(readmePath))
                        readmePath = Path.Combine(targetDir, "README.txt");

                    if (File.Exists(readmePath))
                    {
                        txtReadme.Text = File.ReadAllText(readmePath);
                        return;
                    }
                }

                txtReadme.Text += "No local README.md or documentation file found for this tool.\n\n";
                txtReadme.Text += "If this is a cloned GitHub repository, ensure you have clicked 'Run' at least once to clone it locally.";
            }
            catch (Exception ex)
            {
                txtReadme.Text = "Error loading README: " + ex.Message;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtCommand.Text))
            {
                MessageBox.Show("Name and Command cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _tool.Name = txtName.Text;
            _tool.Command = txtCommand.Text;
            _tool.Shell = ((System.Windows.Controls.ComboBoxItem)cmbShell.SelectedItem).Content.ToString();
            
            this.DialogResult = true;
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
