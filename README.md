<div align="center">
  <img src="ToolkitApp/icon.png" alt="Tool Kit Logo" width="120"/>
  <h1>Tool Kit</h1>
  <p><strong>A modern, extensible WPF application for managing and executing CLI tools.</strong></p>

  <p>
    <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8"/>
    <img src="https://img.shields.io/badge/Platform-Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white" alt="Windows"/>
    <img src="https://img.shields.io/badge/Version-0.1--beta-brightgreen?style=for-the-badge" alt="Version 0.1-beta"/>
    <img src="https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge" alt="License"/>
  </p>
</div>

<br/>

## 📖 Overview

**Tool Kit** is a developer-centric GUI designed to unify your workflow. Stop hunting for scattered scripts or memorizing complex terminal commands. With Tool Kit, you can seamlessly organize, discover, and run your CLI tools from a beautiful, grid-based dashboard.

Built entirely on **.NET 8.0** and **WPF**, it leverages modern desktop capabilities to provide a fluid, responsive experience.

## ✨ Key Features

- 🚀 **One-Click Execution**: Run `cmd` or `powershell` scripts directly from the UI.
- 🛡️ **Privilege Escalation**: Easily launch tools with Administrator privileges when required.
- 🌐 **GitHub Integration**: Discover and clone developer tools directly from GitHub repositories without leaving the app.
- 🖱️ **Drag & Drop**: Instantly onboard new tools by dragging `.ps1`, `.bat`, or executable files into the workspace.
- 🗂️ **Dedicated Library**: Manage your installed scripts in a clean, categorized "Installed Tools" view.
- 📝 **Built-in Editor**: Review and modify tool configurations, execution arguments, and readmes on the fly.

---

## 🚀 Quick Start

### Installation

Tool Kit is distributed as a **self-contained executable**, meaning you don't even need the .NET runtime installed on your machine!

1. Navigate to the **[Releases](../../releases)** page.
2. Download the latest `ToolKit-Release-v0.1-beta.zip` or `.tar.gz`.
3. Extract the archive to your preferred directory.
4. Run `ToolKit.exe`.

### Adding Your First Tool

There are three intuitive ways to populate your toolkit:
1. **Manual Configuration**: Click **+ Add Tool** to define a custom shell command.
2. **Drag & Drop**: Drag any script directly onto the application window.
3. **GitHub Search**: Press `Ctrl+Q` or use the UI to search and install tools from GitHub.

---

## 💻 Development & Build Instructions

Interested in contributing or building the app from source? 

### Prerequisites
- Visual Studio 2022 (or Rider / VS Code)
- .NET 8.0 SDK

### Building from Source

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/ToolKit.git
   ```
2. Navigate to the project directory:
   ```bash
   cd ToolKit/ToolkitApp
   ```
3. Restore dependencies and build:
   ```bash
   dotnet build -c Release
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! 
Feel free to check out the [issues page](../../issues).

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📜 License

Distributed under the MIT License. See `LICENSE` for more information.

<div align="center">
  <i>Built with ❤️ for developers, by developers.</i>
</div>
