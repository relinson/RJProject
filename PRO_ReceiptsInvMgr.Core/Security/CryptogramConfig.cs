using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace PRO_ReceiptsInvMgr.Core.Security
{
    /// <summary>
    /// Summary description for CryptographerConfig
    /// </summary>
    public class CryptogramConfig : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the name of the cryptogram.
        /// </summary>
        /// <value>The name of the cryptogram.</value>
        [ConfigurationProperty("name")]
        public string CryptogramName
        {
            get
            {
                return this["name"] as string;
            }
            set
            {
                this["name"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the cryptogram key.
        /// </summary>
        /// <value>The cryptogram key.</value>
        [ConfigurationProperty("key")]
        public string CryptogramKey
        {
            get
            {
                return this["key"] as string;
            }
            set
            {
                this["key"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the cryptogram.
        /// </summary>
        /// <value>The type of the cryptogram.</value>
        [ConfigurationProperty("type")]
        public string CryptogramType
        {
            get
            {
                return this["type"] as string;
            }
            set
            {
                this["type"] = value;
            }
        }

        /// <summary>
        /// Gets the cryptogram node.
        /// </summary>
        /// <returns>CryptogramConfig.</returns>
        public static CryptogramConfig GetCryptogramNode()
        {
            CryptogramConfig config = ConfigurationManager.GetSection("Cryptogram") as CryptogramConfig;

            if (config != null)
            {
                return config;
            }
            else
            {
                return null;
            }
        }
    }
}
