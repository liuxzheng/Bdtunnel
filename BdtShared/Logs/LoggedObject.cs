/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.ComponentModel;
#endregion

namespace Bdt.Shared.Logs
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Classe de base pour un objet utilisant un flux de log
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable(), TypeConverter(typeof(ExpandableObjectConverter))]
    public class LoggedObject : ILogger
    {

        #region " Attributs "
        protected static BaseLogger m_globalLogger = null;
        protected DateTime startmarker = DateTime.Now;
        protected BaseLogger m_logger = null;
        #endregion

        #region " Propriétés "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe/retourne le loggueur assocé à cet objet
        /// </summary>
        /// <returns>le loggueur assocé à cet objet</returns>
        /// -----------------------------------------------------------------------------
        public BaseLogger Logger
        {
            get
            {
                return m_logger;
            }
            set
            {
                m_logger = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe/retourne le loggueur assocé à tous les objets dérivés.
        /// </summary>
        /// <returns>le loggueur assocé à tous les objets dérivés</returns>
        /// -----------------------------------------------------------------------------
        public static BaseLogger GlobalLogger
        {
            get
            {
                return m_globalLogger;
            }
            set
            {
                m_globalLogger = value;
            }
        }
        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entrée de log. Ne sera pas prise en compte si le log est inactif
        /// ou si le filtre l'impose
        /// </summary>
        /// <param name="message">le message à logger</param>
        /// <param name="severity">la sévérité</param>
        /// -----------------------------------------------------------------------------
        public virtual void Log(string message, ESeverity severity)
        {
            Log(this, message, severity);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entrée de log. Ne sera pas prise en compte si le log est inactif
        /// ou si le filtre l'impose
        /// </summary>
        /// <param name="message">le message à logger</param>
        /// <param name="severity">la sévérité</param>
        /// -----------------------------------------------------------------------------
        public virtual void Log(object sender, string message, ESeverity severity)
        {
            if (m_logger != null)
            {
                m_logger.Log(sender, message, severity);
            }
            if (m_globalLogger != null)
            {
                m_globalLogger.Log(sender, message, severity);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fermeture du logger
        /// </summary>
        /// -----------------------------------------------------------------------------
        public void Close()
        {
            if (m_logger != null)
            {
                m_logger.Close();
            }
        }
        #endregion

    }

}

