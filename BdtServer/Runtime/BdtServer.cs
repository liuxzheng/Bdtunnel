/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Globalization;

using Bdt.Shared.Runtime;
using Bdt.Shared.Logs;
using Bdt.Server.Service;
using Bdt.Server.Resources;
#endregion

namespace Bdt.Server.Runtime
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Programme côté serveur du tunnel de communication
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class BdtServer : Program
    {

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Point d'entrée du programme BdtClient
        /// </summary>
        /// <param name="args">les arguments de la ligne de commande</param>
        /// -----------------------------------------------------------------------------
        public static void Main(string[] args)
        {
            new BdtServer().Run(args);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe la culture courante
        /// </summary>
        /// <param name="name">le nom de la culture</param>
        /// -----------------------------------------------------------------------------
        public override void SetCulture(String name)
        {
            base.SetCulture(name);
            if ((name != null) && (name != String.Empty))
            {
                Bdt.Server.Resources.Strings.Culture = new CultureInfo(name);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Traitement principal
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected void Run(string[] args)
        {
            try
            {
                LoadConfiguration(args);

                Log(string.Format(Strings.SERVER_TITLE, this.GetType().Assembly.GetName().Version.ToString(3)), ESeverity.INFO);
                Log(FrameworkVersion(), ESeverity.INFO);

                Tunnel.Configuration = m_config;
                Tunnel.Logger = LoggedObject.GlobalLogger;
                m_protocol.ConfigureServer(typeof(Tunnel));
                Log(Strings.SERVER_STARTED, ESeverity.INFO);
                Console.ReadLine();
                Tunnel.DisableChecking();

                UnLoadConfiguration();
            }
            catch (Exception ex)
            {
                if (LoggedObject.GlobalLogger != null)
                {
                    Log(ex.Message, ESeverity.ERROR);
                    Log(ex.ToString(), ESeverity.DEBUG);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        #endregion

    }

}
