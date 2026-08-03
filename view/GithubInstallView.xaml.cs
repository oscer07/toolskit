using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ToolkitApp.Views
{
    public class GithubRepo
    {
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("stargazers_count")]
        public int StargazersCount { get; set; }
        
        [JsonPropertyName("clone_url")]
        public string CloneUrl { get; set; }

        public string StarsText => $"⭐ {StargazersCount:N0}";
    }

    public class GithubSearchResponse
    {
        [JsonPropertyName("items")]
        public List<GithubRepo> Items { get; set; }
    }

    public partial class GithubInstallView : UserControl
    {
        private const string ConfigFile = "tools_config.json";
        private const string GitHubToken = "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";

        public GithubInstallView()
        {
            InitializeComponent();
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string query = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(query)) return;

            btnSearch.IsEnabled = false;
            lblStatus.Text = "Searching GitHub...";
            lstResults.ItemsSource = null;

            try
            {
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ToolkitApp", "1.0"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GitHubToken);
                
                string url = $"https://api.github.com/search/repositories?q={Uri.EscapeDataString(query)}";
                
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<GithubSearchResponse>(json);
                    
                    if (result?.Items != null && result.Items.Count > 0)
                    {
                        lstResults.ItemsSource = result.Items;
                        lblStatus.Text = $"Found {result.Items.Count} repositories.";
                    }
                    else
                    {
                        lblStatus.Text = "No repositories found for that query.";
                    }
                }
                else
                {
                    lblStatus.Text = $"Search failed: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
            finally
            {
                btnSearch.IsEnabled = true;
            }
        }

        private void LstResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnInstall.IsEnabled = lstResults.SelectedItem != null;
        }

        private void BtnInstall_Click(object sender, RoutedEventArgs e)
        {
            if (lstResults.SelectedItem is GithubRepo repo)
            {
                string command = $"git clone {repo.CloneUrl}";
                AddToolToConfig($"GitHub: {repo.FullName}", command);
                lblStatus.Text = $"Successfully added '{repo.FullName}' to Tools Workspace!";
                lstResults.SelectedItem = null;
            }
        }

        private void AddToolToConfig(string name, string command)
        {
            ToolkitConfig config;
            if (File.Exists(ConfigFile))
            {
                try
                {
                    string json = File.ReadAllText(ConfigFile);
                    config = JsonSerializer.Deserialize<ToolkitConfig>(json) ?? new ToolkitConfig();
                }
                catch
                {
                    config = new ToolkitConfig();
                }
            }
            else
            {
                config = new ToolkitConfig();
            }

            config.Tools.Add(new ToolItem
            {
                Name = name,
                Command = command,
                Shell = "cmd"
            });

            File.WriteAllText(ConfigFile, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
