/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.IO;
using System.ComponentModel;

using Bdt.Shared.Configuration;
#endregion

namespace Bdt.Shared.Logs
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Classe de base pour la génération d'un log
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable(), TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class BaseLogger : ILogger
    {

        #region " Constantes "
        public const string DEFAULT_DATE_FORMAT = "dd/MM/yyyy HH:mm:ss";
        public const string CONFIG_ENABLED = "enabled";
        public const string CONFIG_FILTER = "filter";
        public const string CONFIG_DATE_FORMAT = "dateformat";
        public const string CONFIG_STRING_FORMAT = "stringformat";
        public const string CONFIG_TAG_START = "{";
        public const string CONFIG_TAG_END = "}";
        #endregion

        #region " Enumerations "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ces tags permettent de construire une chaine de sortie personnalisée
        /// </summary>
        /// -----------------------------------------------------------------------------
        public enum ETags
        {
            TIMESTAMP = 0,
            SEVERITY = 1,
            TYPE = 2,
            MESSAGE = 3
        }
        #endregion

        #region " Attributs "
        protected TextWriter m_writer = null;
        protected string m_stringFormat = CONFIG_TAG_START + ETags.TIMESTAMP + CONFIG_TAG_END + " " + CONFIG_TAG_START + ETags.SEVERITY + CONFIG_TAG_END + " [" + CONFIG_TAG_START + ETags.TYPE + CONFIG_TAG_END + "] " + CONFIG_TAG_START + ETags.MESSAGE + CONFIG_TAG_END;
        protected string m_dateFormat = "dd/MM/yyyy HH:mm:ss";
        protected ESeverity m_filter = ESeverity.DEBUG;
        protected bool m_enabled = true;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe/retourne l'état d'activation du log
        /// </summary>
        /// <returns>l'état d'activation du log</returns>
        /// -----------------------------------------------------------------------------
        public bool Enabled
        {
            get
            {
                return m_enabled;
            }
            set
            {
                m_enabled = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe/retourne la chaine personnalisée pour la sortie des logs.
        /// Utiliser les constantes ETags entre accolades {}
        /// </summary>
        /// <returns>la chaine personnalisée pour la sortie des logs</returns>
        /// -----------------------------------------------------------------------------
        public string StringFormat
        {
            get
            {
                return m_stringFormat;
            }
            set
            {
                m_stringFormat = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne/Fixe le format des dates de timestamp
        /// </summary>
        /// <returns>le format des dates de timestamp</returns>
        /// -----------------------------------------------------------------------------
        public string DateFormat
        {
            get
            {
                return m_dateFormat;
            }
            set
            {
                m_dateFormat = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne/fixe le niveau de filtrage pour la sortie des logs
        /// </summary>
        /// <returns>le niveau de filtrage pour la sortie des logs</returns>
        /// -----------------------------------------------------------------------------
        public ESeverity Filter
        {
            get
            {
                return m_filter;
            }
            set
            {
                m_filter = value;
            }
        }
        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur pour un log vierge
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected BaseLogger ()
        {
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur pour un log à partir des données fournies dans une configuration
        /// </summary>
        /// <param name="writer">le textwriter pour la sortie des logs</param>
        /// <param name="prefix">le prefixe dans la configuration ex: application/log</param>
        /// <param name="config">la configuration pour la lecture des parametres</param>
        /// -----------------------------------------------------------------------------
        public BaseLogger(TextWriter writer, string prefix, Bdt.Shared.Configuration.ConfigPackage config)
        {
            m_writer = writer;

            m_enabled = config.ValueBool(prefix + Bdt.Shared.Configuration.BaseConfig.SOURCE_ITEM_ATTRIBUTE + CONFIG_ENABLED, m_enabled);
            m_filter = ((ESeverity)ESeverity.Parse(typeof(ESeverity), config.Value(prefix + Bdt.Shared.Configuration.BaseConfig.SOURCE_ITEM_ATTRIBUTE + CONFIG_FILTER, m_filter.ToString())));
            m_dateFormat = config.Value(prefix + Bdt.Shared.Configuration.BaseConfig.SOURCE_ITEM_ATTRIBUTE + CONFIG_DATE_FORMAT, m_dateFormat);
            m_stringFormat = config.Value(prefix + Bdt.Shared.Configuration.BaseConfig.SOURCE_ITEM_ATTRIBUTE + CONFIG_STRING_FORMAT, m_stringFormat);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur pour un log
        /// </summary>
        /// <param name="writer">le textwriter pour la sortie des logs</param>
        /// <param name="dateFormat">le format des dates de timestamp</param>
        /// <param name="filter">le niveau de filtrage pour la sortie des logs</param>
        /// -----------------------------------------------------------------------------
        public BaseLogger(TextWriter writer, string dateFormat, ESeverity filter)
        {
            m_writer = writer;
            m_dateFormat = dateFormat;
            m_filter = filter;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entrée de log. Ne sera pas prise en compte si le log est inactif
        /// ou si le filtre l'impose
        /// </summary>
        /// <param name="sender">l'emetteur</param>
        /// <param name="message">le message à logger</param>
        /// <param name="severity">la sévérité</param>
        /// -----------------------------------------------------------------------------
        public virtual void Log(object sender, string message, ESeverity severity)
        {
            if ((m_enabled) && (severity >= m_filter) && (m_writer != null))
            {
                // Remplacement des id chaines de ETags en leur équivalent integer
                string format = m_stringFormat;
                foreach (ETags tag in ETags.GetValues(typeof(ETags)))
                {
                    format = format.Replace(tag.ToString(), System.Convert.ToInt32(tag).ToString());
                }
                m_writer.WriteLine(string.Format(format, DateTime.Now.ToString(m_dateFormat), severity, sender.GetType().Name, message));
                m_writer.Flush();
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fermeture du logger
        /// </summary>
        /// -----------------------------------------------------------------------------
        public virtual void Close()
        {
            if (m_writer != null)
            {
                try
                {
                    m_writer.Close();
                }
                catch (Exception)
                {
                }
                m_writer = null;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Destruction d'un log
        /// </summary>
        /// -----------------------------------------------------------------------------
        ~BaseLogger()
        {
            Close();
        }
        #endregion

    }

}


