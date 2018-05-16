using System;
using System.Configuration;
using System.IO;
using System.Security.Permissions;
using System.Security.AccessControl;
using System.Security.Principal;

namespace STARS.Applications.VETS.Plugins.ShiftTableCalculator
{
    public static class Config
    {
        public static string Part1;
        public static string Part1Reduced;
        public static string Part2;
        public static string Part2Reduced;
        public static string Part3;
        public static string Part3Reduced;
        public static string LogFolder;

        private static bool wmtcMode;
        public static bool WmtcMode
        {
            get { return wmtcMode; }
            set { SetField("WmtcMode", value.ToString()); }
        }

        private static void UpdateFields()
        {
            wmtcMode = TypeCast.ToBool(AppConfig("WmtcMode"));
            Part1 = AppConfig("Part1");
            Part1Reduced = AppConfig("Part1Reduced");
            Part2 = AppConfig("Part2");
            Part2Reduced = AppConfig("Part2Reduced");
            Part3 = AppConfig("Part3");
            Part3Reduced = AppConfig("Part3Reduced");
            LogFolder = FormatPath(AppConfig("LogFolder"));
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        static Config()
        {
            string path = typeof(Config).Assembly.Location + ".config";
            string dir = Path.GetDirectoryName(path);
            string file = Path.GetFileName(path);

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = dir;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = file;
            watcher.Changed += new FileSystemEventHandler(OnAppConfigChanged);
            watcher.EnableRaisingEvents = true;

            UpdateFields();
        }

        private static void OnAppConfigChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                UpdateFields();
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("being used by another process"))
                {
                    throw ex;
                }
            }
        }

        private static string FormatPath(string path)
        {
            if (!path.EndsWith(@"\"))
            {
                return path + @"\";
            }
            return path;
        }

        private static string AppConfig(string key)
        {
            Configuration config = null;
            string exeConfigPath = typeof(Config).Assembly.Location;
            config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            if (config == null || config.AppSettings.Settings.Count == 0)
            {
                throw new Exception(String.Format("Config file {0}.config is missing or could not be loaded.", exeConfigPath));
            }

            KeyValueConfigurationElement element = config.AppSettings.Settings[key];
            if (element != null)
            {
                string value = element.Value;
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            return string.Empty;
        }

        private static void SetField(string key, string value)
        {
            Configuration config = null;
            string exeConfigPath = typeof(Config).Assembly.Location;
            config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            if (config == null || config.AppSettings.Settings.Count == 0)
            {
                throw new Exception(String.Format("Config file {0}.config is missing or could not be loaded.", exeConfigPath));
            }

            KeyValueConfigurationElement element = config.AppSettings.Settings[key];
            if (element == null) return;
            element.Value = value;
            config.Save();

            UpdateFields();
        }

    }
}
