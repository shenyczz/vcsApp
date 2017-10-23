using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
//using System.Windows.Forms;

namespace Hdwas
{
    public sealed class MrConfiguration
    {
        #region Constructors

        public MrConfiguration()
        {
            _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        #endregion

        #region Fields

        private Configuration _configuration;

        #endregion

        #region App.config

        #region --区域代码--

        /// <summary>
        /// 省级代码
        /// </summary>
        public String AreaCode
        {
            get
            {
                return _configuration.AppSettings.Settings["AreaCode"] == null
                    ? ""
                    : _configuration.AppSettings.Settings["AreaCode"].Value;
            }
            set
            {
                if (_configuration.AppSettings.Settings["AreaCode"] != null)
                    _configuration.AppSettings.Settings["AreaCode"].Value = value.ToString();
            }
        }

        #endregion

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            //_configuration.Save();
        }

        #endregion
    }
}
