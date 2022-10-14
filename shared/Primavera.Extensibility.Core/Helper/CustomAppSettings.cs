using System.Configuration;

namespace Primavera.Extensibility.Core.Helper
{
    public class CustomAppSettings
    {
        private Configuration _config;

        public CustomAppSettings()
        {
            string appDirectory = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            string settingsPath = System.IO.Path.Combine(appDirectory, "CustomApp.config");

            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = settingsPath;

            _config = ConfigurationManager.OpenMappedExeConfiguration(
                configMap,ConfigurationUserLevel.None);
        }

        /// <summary>
        /// Get the last manifest version.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string ReadSetting(string key)
        {
            return _config.AppSettings.Settings[key].Value;
        }

        /// <summary>
        /// Save the setting to disk.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void WriteSetting(string key, string value)
        {
            _config.AppSettings.Settings[key].Value= value;
            _config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
