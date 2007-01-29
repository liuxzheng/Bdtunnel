/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Collections;
#endregion

namespace Bdt.Shared.Configuration
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Représente une source de configuration générique. Permet de servir de base pour l'élaboration
    /// d'autres sources.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class BaseConfig : IComparable
    {

        #region " Constantes "
        public const string SOURCE_PATH_SEPARATOR = "/";
        public const string SOURCE_ITEM_ATTRIBUTE = "@";
        public const string SOURCE_ITEM_EQUALS = "=";
        public const string SOURCE_SCRAMBLED_START = "[";
        public const string SOURCE_SCRAMBLED_END = "]";
        #endregion

        #region " Attributs "
        protected SortedList m_values = new SortedList(); // Les elements classés par code
        protected int m_priority = 0; // La priorité de cette source
        #endregion

        #region " Propriétés "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne/Fixe la priorité de la source
        /// </summary>
        /// <returns>la priorité de la source</returns>
        /// -----------------------------------------------------------------------------
        public int Priority
        {
            get
            {
                return m_priority;
            }
            set
            {
                m_priority = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne/Fixe la priorité de la source
        /// </summary>
        /// <returns>la priorité de la source</returns>
        /// -----------------------------------------------------------------------------
        public string Value(string code, string defaultValue)
        {
            if (m_values.ContainsKey(code))
            {
                return System.Convert.ToString(m_values[code]);
            }
            else
            {
                return defaultValue;
            }
        }
        public void SetValue(string code, string defaultValue, string Value)
        {
            if (m_values.ContainsKey(code))
            {
                m_values[code] = Value;
            }
            else
            {
                m_values.Add(code, Value);
            }
        }
        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur d'une source avec un décrypteur de données optionnel
        /// Les valeurs entre SOURCE_SCRAMBLED_START et SOURCE_SCRAMBLED_END seront
        /// automatiquement décryptées
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected BaseConfig(int priority)
        {
            this.Priority = priority;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Force le rechargement de la source de donnée
        /// </summary>
        /// -----------------------------------------------------------------------------
        public abstract void Rehash();

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Concatène tous les éléments depuis cette source
        /// </summary>
        /// <returns>le format de chaque ligne est classe,priorité,code,valeur</returns>
        /// -----------------------------------------------------------------------------
        public sealed override string ToString()
        {
            string returnValue;
            returnValue = string.Empty;

            foreach (string key in m_values.Keys)
            {
                returnValue += "   <" + this.GetType().Name + "(" + Priority + ")" + "> [" + key + "] " + SOURCE_ITEM_EQUALS + " [" + Value(key, String.Empty) + "]" + "\r\n";
            }
            return returnValue;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Comparateur par priorité
        /// </summary>
        /// <param name="obj">la config à comparer</param>
        /// <returns>voir IComparable.CompareTo</returns>
        /// -----------------------------------------------------------------------------
        public int CompareTo(object obj)
        {
            if ((obj) is BaseConfig)
            {
                return this.Priority - ((BaseConfig)obj).Priority;
            }
            else
            {
                return 0;
            }
        }
        #endregion

    }
}

