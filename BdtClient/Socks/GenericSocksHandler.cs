/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Net.Sockets;

using Bdt.Shared.Logs;
using Bdt.Client.Resources;
#endregion

namespace Bdt.Client.Socks
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Gestionnaire générique Socks
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class GenericSocksHandler : LoggedObject
    {

        #region " Constantes "
        // La taille du buffer d'IO
        public const int BUFFER_SIZE = 8192;
        #endregion

        #region " Attributs "
        private int m_version;
        private int m_command;
        private string m_address;
        private int m_remoteport;
        private byte[] m_buffer;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le handler est-il adapté à la requête?
        /// </summary>
        /// -----------------------------------------------------------------------------
        public abstract bool IsHandled
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Les données de réponse
        /// </summary>
        /// -----------------------------------------------------------------------------
        public abstract byte[] Reply
        {
            get;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La version de la requête socks
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected int Version
        {
            get
            {
                return m_version;
            }
            set
            {
                m_version = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La commande de la requête socks
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected int Command
        {
            get
            {
                return m_command;
            }
            set
            {
                m_command = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le port distant
        /// </summary>
        /// -----------------------------------------------------------------------------
        public int RemotePort
        {
            get
            {
                return m_remoteport;
            }
            protected set
            {
                m_remoteport = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// L'adresse distante
        /// </summary>
        /// -----------------------------------------------------------------------------
        public string Address
        {
            get
            {
                return m_address;
            }
            protected set
            {
                m_address = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le buffer de la requête
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected byte[] Buffer
        {
            get
            {
                return m_buffer;
            }
        }

        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="buffer"></param>
        /// -----------------------------------------------------------------------------
        protected GenericSocksHandler(byte[] buffer)
        {
            m_buffer = buffer;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Retourne un gestionnaire adapté à la requête
        /// </summary>
        /// <param name="client">le client TCP</param>
        /// <returns>un gestionnaire adapté</returns>
        /// -----------------------------------------------------------------------------
        public static GenericSocksHandler GetInstance(TcpClient client)
        {
            byte[] buffer = new byte[BUFFER_SIZE];

            NetworkStream stream = client.GetStream();
            int size = stream.Read(buffer, 0, BUFFER_SIZE);
            Array.Resize(ref buffer, size);

            if (size < 3)
            {
                throw (new ArgumentException(Strings.INVALID_SOCKS_HANDSHAKE));
            }

            GenericSocksHandler result;
            result = new Socks4Handler(buffer);
            if (!result.IsHandled)
            {
                result = new Socks4AHandler(buffer);
                if (!result.IsHandled)
                {
                    result = new Socks5Handler(client, buffer);
                    if (!result.IsHandled)
                    {
                        throw (new ArgumentException(Strings.NO_VALID_SOCKS_HANDLER));
                    }
                }
            }

            byte[] reply = result.Reply;
            client.GetStream().Write(reply, 0, reply.Length);

            return result;
        }
        #endregion

    }

}


