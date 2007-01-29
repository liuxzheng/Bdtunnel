/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
#endregion

namespace Bdt.Shared.Request
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Une demande d'authentification
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable()]
    public struct AuthenticateRequest : IGenericRequest 
    {

        #region " Attributs "
        private int m_cid;
        private int m_uid;
        private string m_username;
        private string m_password;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le jeton de connexion
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Cid
        {
            get
            {
                return m_cid;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le jeton utilisateur
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Uid
        {
            get
            {
                return m_uid;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le nom de l'utilisateur
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Username
        {
            get
            {
                return m_username;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le mot de passe de l'utilisateur
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Password
        {
            get
            {
                return m_password;
            }
        }
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="username">Le nom de l'utilisateur</param>
        /// <param name="password">Le mot de passe de l'utilisateur</param>
        /// -----------------------------------------------------------------------------
        public AuthenticateRequest(string username, string password)
        {
            this.m_uid = -1;
            this.m_cid = -1;
            this.m_username = username;
            this.m_password = password;
        }
        #endregion

    }

}

