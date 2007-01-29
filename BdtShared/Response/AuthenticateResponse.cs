/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
#endregion

namespace Bdt.Shared.Response
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Une réponse d'authentification
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable()]
    public struct AuthenticateResponse
    {

        #region " Attributs "
        private int m_uid;
        private bool m_success;
        private string m_message;
        private bool m_dataAvailable;
        private bool m_connected;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Des données sont-elles disponibles?
        /// </summary>
        /// -----------------------------------------------------------------------------
        public bool DataAvailable
        {
            get
            {
                return m_dataAvailable;
            }
            set
            {
                m_dataAvailable = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La connexion est-elle effective?
        /// </summary>
        /// -----------------------------------------------------------------------------
        public bool Connected
        {
            get
            {
                return m_connected;
            }
            set
            {
                m_connected = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La requête a aboutie/échoué ?
        /// </summary>
        /// -----------------------------------------------------------------------------
        public bool Success
        {
            get
            {
                return m_success;
            }
            set
            {
                m_success = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le message d'information
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Message
        {
            get
            {
                return m_message;
            }
            set
            {
                m_message = value;
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
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="success">Authentification réussie/échouée</param>
        /// <param name="message">Le message d'information</param>
        /// <param name="uid">Le jeton utilisateur affecté</param>
        /// -----------------------------------------------------------------------------
        public AuthenticateResponse(bool success, string message, int uid)
        {
            this.m_connected = false;
            this.m_dataAvailable = false;
            this.m_success = success;
            this.m_message = message;
            this.m_uid = uid;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="success">Authentification réussie/échouée</param>
        /// <param name="uid">Le jeton utilisateur affecté</param>
        /// -----------------------------------------------------------------------------
        public AuthenticateResponse(bool success, int uid)
        {
            this.m_connected = false;
            this.m_dataAvailable = false;
            this.m_success = success;
            this.m_message = string.Empty;
            this.m_uid = uid;
        }
        #endregion

    }

}


