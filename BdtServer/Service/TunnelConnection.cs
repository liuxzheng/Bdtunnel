/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Net.Sockets;

using Bdt.Shared.Service;
using Bdt.Shared.Request;
using Bdt.Shared.Response;
#endregion

namespace Bdt.Server.Service
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Une connexion au sein du tunnel
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class TunnelConnection
    {

        #region " Attributs "
        protected TcpClient m_tcpClient;
        protected NetworkStream m_stream;
        protected DateTime m_lastAccess;
        protected int m_uid;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le client TCP associé
        /// </summary>
        /// -----------------------------------------------------------------------------
        public TcpClient TcpClient
        {
            get
            {
                return m_tcpClient;
            }
            set
            {
                m_tcpClient = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le flux associé
        /// </summary>
        /// -----------------------------------------------------------------------------
        public NetworkStream Stream
        {
            get
            {
                return m_stream;
            }
            set
            {
                m_stream = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La date de dernière opération I/O
        /// </summary>
        /// -----------------------------------------------------------------------------
        public DateTime LastAccess
        {
            get
            {
                return m_lastAccess;
            }
            set
            {
                m_lastAccess = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le user-id associé
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int Uid
        {
            get
            {
                return m_uid;
            }
            set
            {
                m_uid = value;
            }
        }
        #endregion

    }

}
