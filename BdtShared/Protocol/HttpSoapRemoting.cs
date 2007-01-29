/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Runtime.Remoting.Channels.Http;
#endregion

namespace Bdt.Shared.Protocol
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Protocole de communication basé sur le remoting .NET et sur le protocole HTTP
    /// Utilise un formateur SOAP pour les données
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class HttpSoapRemoting : GenericHttpRemoting
    {

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le canal de communication côté client
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override System.Runtime.Remoting.Channels.IChannel ClientChannel
        {
            get
            {
                if (m_clientchannel == null)
                {
                    m_clientchannel = new HttpChannel();
                }
                return m_clientchannel;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le canal de communication côté serveur
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected override System.Runtime.Remoting.Channels.IChannel ServerChannel
        {
            get
            {
                if (m_serverchannel == null)
                {
                    m_serverchannel = new HttpChannel(Port);
                }
                return m_serverchannel;
            }
        }
        #endregion

    }

}

