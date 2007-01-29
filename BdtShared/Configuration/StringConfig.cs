/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
#endregion

namespace Bdt.Shared.Configuration
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Représente une source de configuration basée sur une ligne de commande
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class StringConfig : BaseConfig
    {

        #region " Attributs "
        private string[] m_args; //Les arguments de la ligne de commande
        #endregion

        #region " Propriétés "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne/Fixe les arguments de la ligne de commande
        /// </summary>
        /// <returns>les arguments de la ligne de commande</returns>
        /// -----------------------------------------------------------------------------
        public string[] Args
        {
            get
            {
                return m_args;
            }
            set
            {
                m_args = value;
            }
        }
        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Création d'une source de donnée basée sur la ligne de commande
        /// </summary>
        /// <param name="args">les arguments de la ligne de commande</param>
        /// <param name="priority">la priorité de cette source (la plus basse=prioritaire)</param>
        /// -----------------------------------------------------------------------------
        public StringConfig(string[] args, int priority)
            : base(priority)
        {
            this.Args = args;
            Rehash();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Force le rechargement de la source de donnée
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void Rehash()
        {
            foreach (string arg in Args)
            {
                int equalIndex = arg.IndexOf(SOURCE_ITEM_EQUALS);
                if ((equalIndex >= 0) && equalIndex + 1 < arg.Length)
                {
                    this.SetValue(arg.Substring(0, equalIndex), null, arg.Substring(equalIndex + 1));
                }
            }
        }
        #endregion

    }

}


