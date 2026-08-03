using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ToolkitApp.Views
{
    public partial class ToolsView : UserControl
    {
        private const string ConfigFile = "tools_config.json";
        private ToolkitConfig config;
        private List<ToolItem> displayList;

        public ToolsView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfiguration();
        }

        public void LoadConfiguration()
        {
            if (File.Exists(ConfigFile))
            {
                try
                {
                    string json = File.ReadAllText(ConfigFile);
                    config = JsonSerializer.Deserialize<ToolkitConfig>(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading config: " + ex.Message);
                    config = new ToolkitConfig();
                }
            }
            else
            {
                config = new ToolkitConfig();
                config.Tools.Add(new ToolItem { Name = "Ping Google", Command = "ping google.com", Shell = "cmd" });
                config.Tools.Add(new ToolItem { Name = "IP Configuration", Command = "ipconfig /all", Shell = "cmd" });
                config.Tools.Add(new ToolItem { Name = "System Info", Command = "systeminfo", Shell = "cmd" });
                config.Tools.Add(new ToolItem { Name = "Get Services (PS)", Command = "Get-Service | Select-Object -First 20", Shell = "powershell" });
                SaveConfiguration();
            }

            FilterList("");
        }

        private void SaveConfiguration()
        {
            try
            {
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigFile, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving config: " + ex.Message);
            }
        }

        private void FilterList(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                displayList = config.Tools.ToList();
            }
            else
            {
                displayList = config.Tools.Where(t => t.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                                                      t.Command.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            
            lstTools.ItemsSource = displayList;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblSearchHint.Visibility = string.IsNullOrEmpty(txtSearch.Text) ? Visibility.Visible : Visibility.Hidden;
            FilterList(txtSearch.Text);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewName.Text) || string.IsNullOrWhiteSpace(txtNewCommand.Text))
            {
                MessageBox.Show("Please enter a tool name and command.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newTool = new ToolItem
            {
                Name = txtNewName.Text,
                Command = txtNewCommand.Text,
                Shell = ((ComboBoxItem)cmbShell.SelectedItem).Content.ToString()
            };

            config.Tools.Add(newTool);
            SaveConfiguration();
            
            txtSearch.Text = "";
            FilterList("");

            txtNewName.Clear();
            txtNewCommand.Clear();
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectedTool(admin: false);
        }

        private void BtnRunAdmin_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSelectedTool(admin: true);
        }

        private void ExecuteSelectedTool(bool admin)
        {
            if (lstTools.SelectedItem == null)
            {
                MessageBox.Show("Please select a tool from the list.", "No tool selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ToolItem tool = (ToolItem)lstTools.SelectedItem;
            txtTerminal.Document.Blocks.Clear();
            AppendOutput($"--- Executing: {tool.Name} (Admin: {admin}) ---\r\n", Brushes.DodgerBlue);

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                
                if (tool.Shell.Equals("powershell", StringComparison.OrdinalIgnoreCase))
                {
                    psi.FileName = "powershell.exe";
                    psi.Arguments = $"-Command \"{tool.Command}\"";
                }
                else
                {
                    psi.FileName = "cmd.exe";
                    psi.Arguments = $"/c \"{tool.Command}\"";
                }

                if (admin)
                {
                    psi.Verb = "runas";
                    psi.UseShellExecute = true;
                    psi.CreateNoWindow = false;
                    AppendOutput("Note: Elevated command launched in a separate window due to UAC restrictions.\r\n", Brushes.Yellow);
                    Process.Start(psi);
                }
                else
                {
                    psi.UseShellExecute = false;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardError = true;
                    psi.CreateNoWindow = true;
                    
                    Process process = new Process { StartInfo = psi, EnableRaisingEvents = true };
                    
                    process.OutputDataReceived += (s, ev) => {
                        if (ev.Data != null)
                            Dispatcher.Invoke(() => AppendOutput(ev.Data + "\r\n", Brushes.LightGray));
                    };
                    
                    process.ErrorDataReceived += (s, ev) => {
                        if (ev.Data != null)
                            Dispatcher.Invoke(() => AppendOutput("ERROR: " + ev.Data + "\r\n", Brushes.Tomato));
                    };

                    process.Exited += (s, ev) => {
                        Dispatcher.Invoke(() => {
                            AppendOutput("--- Execution Finished ---\r\n", Brushes.DodgerBlue);
                            btnRun.IsEnabled = true;
                            btnRunAdmin.IsEnabled = true;
                        });
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    
                    btnRun.IsEnabled = false;
                    btnRunAdmin.IsEnabled = false;
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                AppendOutput("Operation cancelled by user (UAC prompt declined) or executable not found.\r\n", Brushes.Tomato);
                btnRun.IsEnabled = true;
                btnRunAdmin.IsEnabled = true;
            }
            catch (Exception ex)
            {
                AppendOutput($"Exception: {ex.Message}\r\n", Brushes.Tomato);
                btnRun.IsEnabled = true;
                btnRunAdmin.IsEnabled = true;
            }
        }

        private void AppendOutput(string text, Brush color)
        {
            var paragraph = new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run(text))
            {
                Foreground = color,
                Margin = new Thickness(0)
            };
            txtTerminal.Document.Blocks.Add(paragraph);
            txtTerminal.ScrollToEnd();
        }
        private void LstTools_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        string name = Path.GetFileNameWithoutExtension(file);
                        string ext = Path.GetExtension(file).ToLower();
                        string command = $"\"{file}\"";
                        string shell = ext == ".ps1" ? "powershell" : "cmd";

                        config.Tools.Add(new ToolItem
                        {
                            Name = name,
                            Command = command,
                            Shell = shell
                        });
                    }
                    SaveConfiguration();
                    FilterList(txtSearch.Text);
                    MessageBox.Show($"Added {files.Length} tool(s) via Drag and Drop!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void LstTools_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstTools.SelectedItem is ToolItem tool)
            {
                ToolDetailsWindow detailsWin = new ToolDetailsWindow(tool);
                detailsWin.ShowDialog();
                // Refresh list if edited
                SaveConfiguration();
                FilterList(txtSearch.Text);
            }
        }
    }
}
