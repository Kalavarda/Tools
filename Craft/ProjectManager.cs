using System;
using System.IO;
using Craft.Model;
using Craft.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Craft
{
    public class ProjectManager
    {
        private const string FileDialogFilter = "Craft projects|*.craft|Все файлы|*.*";
        private const string FileDialogDefaultExt = ".craft";

        private Project _project;
        private FileInfo _file;

        public static ProjectManager Instance { get; }

        public Project Project
        {
            get => _project;
            private set
            {
                if (_project == value)
                    return;

                _project = value;
                ProjectChanged?.Invoke();
            }
        }

        public FileInfo File
        {
            get => _file;
            private set
            {
                if (_file == value)
                    return;

                _file = value;
                FileChanged?.Invoke();
            }
        }

        public Action ProjectChanged;
        public Action FileChanged;

        static ProjectManager()
        {
            Instance = new ProjectManager();
        }

        private ProjectManager()
        {
        }

        public void New()
        {
            Project = new Project();
        }

        public void Open(string fileName = null)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                OpenFile(fileName);
                return;
            }

            var fileDialog = new OpenFileDialog
            {
                DefaultExt = FileDialogDefaultExt,
                Filter = FileDialogFilter
            };
            if (fileDialog.ShowDialog() == true)
                OpenFile(fileDialog.FileName);
        }

        private void OpenFile(string fileName)
        {
            var serializer = JsonSerializer.CreateDefault();
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(file))
                Project = serializer.Deserialize<Project>(new JsonTextReader(reader));
            File = new FileInfo(fileName);

            Settings.Default.LastFileName = File.FullName;
            Settings.Default.Save();
        }

        public void Save()
        {
            if (File == null)
            {
                var fileDialog = new SaveFileDialog
                {
                    DefaultExt = FileDialogDefaultExt,
                    Filter = FileDialogFilter
                };
                if (fileDialog.ShowDialog() == true)
                    File = new FileInfo(fileDialog.FileName);
            }

            if (File != null)
            {
                var serializer = JsonSerializer.CreateDefault();
                using (var file = new FileStream(File.FullName, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var writer = new StreamWriter(file))
                    serializer.Serialize(writer, Project);

                Settings.Default.LastFileName = File.FullName;
                Settings.Default.Save();
            }
        }
    }
}
