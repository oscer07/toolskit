using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToolkitApp;

namespace ToolkitApp.Views
{
    public partial class InstalledToolsView : UserControl
    {
        private const string ConfigFile = "tools_config.json";
        private ToolkitConfig config;
        private ObservableCollection<ToolItem> displayList;

        public InstalledToolsView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            if (System.IO.File.Exists(ConfigFile))
            {
                string json = System.IO.File.ReadAllText(ConfigFile);
                config = System.Text.Json.JsonSerializer.Deserialize<ToolkitConfig>(json);
            }
            else
            {
                config = new ToolkitConfig();
            }

            displayList = new ObservableCollection<ToolItem>(config.Tools);
            lstInstalledTools.ItemsSource = displayList;
        }

        private void LstInstalledTools_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstInstalledTools.SelectedItem is ToolItem selectedTool)
            {
                var detailsWin = new ToolDetailsWindow(selectedTool);
                bool? result = detailsWin.ShowDialog();
                if (result == true)
                {
                    string json = System.Text.Json.JsonSerializer.Serialize(config, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    System.IO.File.WriteAllText(ConfigFile, json);
                    LoadData();
                }
            }
        }
    }
}
