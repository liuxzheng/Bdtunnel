/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

using Bdt.Server.Resources;
using Bdt.Shared.Service;
using Bdt.Shared.Configuration;
using Bdt.Shared.Request;
using Bdt.Shared.Response;
using Bdt.Shared.Logs;
#endregion

namespace Bdt.Server.Service
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Le tunnel, assure les services de l'interface ITunnel
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class Tunnel : MarshalByRefObject, ITunnel, ILogger
    {

        #region " Constantes "
        public const int BUFFER_SIZE = 8192;
        public const int POLLING_TIME = 1000;
        // Le test de la connexion effective
        public const int SOCKET_TEST_POLLING_TIME = 100;

        protected const string CONFIG_USER_TEMPLATE = "users/{0}@";
        protected const string CONFIG_USER_ENABLED = "enabled";
        protected const string CONFIG_USER_PASSWORD = "password";
        #endregion

        #region " Attributs "
        protected Dictionary<int, TunnelConnection> m_connections = new Dictionary<int, TunnelConnection>();
        protected Dictionary<int, TunnelUser> m_users = new Dictionary<int, TunnelUser>();
        protected const int m_timeout = 60 * 60;
        protected static ManualResetEvent m_mre = new ManualResetEvent(false);
        protected static ConfigPackage m_configuration;
        protected static ILogger m_logger = null;
        #endregion

        #region " Proprietes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La configuration serveur (d'après le fichier xml + ligne de commande)
        /// </summary>
        /// -----------------------------------------------------------------------------
        public static ConfigPackage Configuration
        {
            get
            {
                return m_configuration;
            }
            set
            {
                m_configuration = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Le loggeur
        /// </summary>
        /// -----------------------------------------------------------------------------
        public static ILogger Logger
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
        #endregion

        #region " Méthodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// -----------------------------------------------------------------------------
        public Tunnel()
        {
            Thread thr = new Thread(new System.Threading.ThreadStart(CheckerThread));
            thr.Start();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Generation d'id non encore présent dans une table de hash
        /// </summary>
        /// <param name="hash">la table à analyser</param>
        /// <returns>un entier unique</returns>
        /// -----------------------------------------------------------------------------
        protected int GetNewId(System.Collections.IDictionary hash)
        {
            Random rnd = new Random();
            int key = rnd.Next(0, int.MaxValue);
            while (hash.Contains(key))
            {
                key += 1;
            }
            return key;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Generation d'un identifiant utilisateur unique
        /// </summary>
        /// <returns>un entier unique</returns>
        /// -----------------------------------------------------------------------------
        protected int GetNewUid()
        {
            return GetNewId(m_users);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Generation d'un identifiant de connexion unique
        /// </summary>
        /// <returns>un entier unique</returns>
        /// -----------------------------------------------------------------------------
        protected int GetNewCid()
        {
            return GetNewId(m_connections);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Destructeur
        /// </summary>
        /// -----------------------------------------------------------------------------
        ~Tunnel()
        {
            DisableChecking();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Annule la vérification
        /// </summary>
        /// -----------------------------------------------------------------------------
        public static void DisableChecking()
        {
            m_mre.Set();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Fixe une durée de vie au tunnel
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Traitement principal de thread de vérification des connexions
        /// </summary>
        /// -----------------------------------------------------------------------------
        protected void CheckerThread()
        {
            while (!m_mre.WaitOne(POLLING_TIME, false))
            {
                List<int> deletes = new List<int>();
                foreach (int cid in m_connections.Keys)
                {
                    TunnelConnection connection = m_connections[cid];
                    if (connection != null)
                    {
                        if (DateTime.Now.Subtract(connection.LastAccess).TotalSeconds > m_timeout)
                        {
                            deletes.Add(cid);
                        }
                    }
                }
                foreach (int cid in deletes)
                {
                    TunnelConnection connection = m_connections[cid];
                    try
                    {
                        Log(String.Format(Strings.CONNECTION_TIMEOUT, connection.TcpClient.Client.RemoteEndPoint), ESeverity.INFO);
                        connection.Stream.Flush();
                        connection.Stream.Close();
                        connection.TcpClient.Close();
                        connection.Stream = null;
                        connection.TcpClient = null;
                    }
                    finally
                    {
                        // Suppression d'une entrée dans la table des connexions
                        m_connections.Remove(cid);
                    }
                }
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Vérification de l'authentification d'un utilisateur associé à une requête
        /// </summary>
        /// <param name="request">la requête</param>
        /// <param name="response">la réponse à préparer</param>
        /// <returns>True si l'utilisateur est authentifié</returns>
        /// -----------------------------------------------------------------------------
        public bool CheckUser<I, O> (ref I request, ref O response)
            where I : IGenericRequest
            where O : IGenericResponse
        {
            TunnelUser user = m_users[request.Uid];
            if (user == null)
            {
                response.Success = false;
                response.Message = Strings.SERVER_SIDE + Strings.UID_NOT_FOUND;
            }
            else
            {
                response.Success = true;
                response.Message = string.Empty;
            }
            return response.Success;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Vérification de la connexion associée à une requête
        /// </summary>
        /// <param name="request">la requête</param>
        /// <param name="response">la réponse à préparer</param>
        /// <returns>true si la connexion est valide</returns>
        /// -----------------------------------------------------------------------------
        public TunnelConnection CheckConnection<I, O> (ref I request, ref O response)
            where I : IGenericRequest
            where O : IGenericResponse
        {
            TunnelConnection connection = m_connections[request.Cid];
            if (connection == null)
            {
                response.Success = false;
                response.Message = Strings.SERVER_SIDE + Strings.CID_NOT_FOUND;
                return null;
            }
            else
            {
                connection.LastAccess = DateTime.Now;
                try
                {
                    response.Connected = (!(connection.TcpClient.Client.Poll(SOCKET_TEST_POLLING_TIME, System.Net.Sockets.SelectMode.SelectRead) && connection.TcpClient.Client.Available == 0));
                    response.DataAvailable = connection.TcpClient.Client.Available > 0;
                }
                catch (Exception)
                {
                    response.Connected = false;
                    response.DataAvailable = false;
                }
                response.Success = true;
                response.Message = string.Empty;
                return connection;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Authentification d'un utilisateur
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>une réponse AuthenticateResponse avec un uid</returns>
        /// -----------------------------------------------------------------------------
        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            bool enabled = Configuration.ValueBool(string.Format(CONFIG_USER_TEMPLATE, request.Username) + CONFIG_USER_ENABLED, false);
            string password = Configuration.Value(string.Format(CONFIG_USER_TEMPLATE, request.Username) + CONFIG_USER_PASSWORD, String.Empty);
            string message = String.Empty;
            bool success = false;
            int uid = -1;

            if (!enabled)
            {
                message = Strings.SERVER_SIDE + String.Format(Strings.ACCESS_DENIED,request.Username);
            }
            else
            {
                if (password == request.Password)
                {
                    TunnelUser user = new TunnelUser();
                    user.Logon = DateTime.Now;
                    user.Uid = GetNewUid();
                    user.Username = request.Username;
                    m_users.Add(user.Uid, user);
                    message = Strings.SERVER_SIDE + string.Format(Strings.ACCESS_GRANTED, request.Username);
                    uid = user.Uid;
                    success = true;
                }
                else
                {
                    message = Strings.SERVER_SIDE + string.Format(Strings.ACCESS_DENIED_BAD_PASSWORD, request.Username);
                }
            }

            Log(message, ESeverity.INFO);
            return new AuthenticateResponse(success, message, uid);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La version du serveur
        /// </summary>
        /// <returns>La version de l'assembly</returns>
        /// -----------------------------------------------------------------------------
        public GenericResponse Version()
        {
            System.Reflection.AssemblyName name = this.GetType().Assembly.GetName();
            return new GenericResponse(true, Strings.SERVER_SIDE + string.Format("{0} v{1}, {2}", name.Name, name.Version.ToString(3), Bdt.Shared.Runtime.Program.FrameworkVersion()));
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Etablissement d'une nouvelle connexion
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>le cid de connexion</returns>
        /// -----------------------------------------------------------------------------
        public ConnectResponse Connect(ConnectRequest request)
        {
            ConnectResponse response = new ConnectResponse();

            if (CheckUser(ref request, ref response))
            {
                // Création d'une entrée dans la table des connexions
                TunnelConnection connection = new TunnelConnection();
                try
                {
                    connection.TcpClient = new TcpClient(request.Address, request.Port);
                    connection.Stream = connection.TcpClient.GetStream();
                    connection.LastAccess = DateTime.Now;
                    connection.Uid = request.Uid;
                    response.Connected = true;
                    response.DataAvailable = connection.Stream.DataAvailable;
                    int cid = GetNewCid();
                    m_connections.Add(cid, connection);

                    // Ok
                    response.Success = true;
                    response.Message = Strings.SERVER_SIDE + string.Format(Strings.CONNECTED, connection.TcpClient.Client.RemoteEndPoint, request.Address);
                    response.Cid = cid;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = Strings.SERVER_SIDE + string.Format(Strings.CONNECTION_REFUSED, request.Address, request.Port, ex.Message);
                    response.Cid = -1;
                }
            }

            Log(response.Message, ESeverity.INFO);
            return response;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Déconnexion
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>une reponse générique</returns>
        /// -----------------------------------------------------------------------------
        public GenericResponse Disconnect(GenericRequest request)
        {
            GenericResponse response = new GenericResponse();

            if (CheckUser(ref request, ref response))
            {
                TunnelConnection connection = CheckConnection(ref request, ref response);
                if (connection != null)
                {
                    try
                    {
                        response.Message = Strings.SERVER_SIDE + string.Format(Strings.DISCONNECTED, connection.TcpClient.Client.RemoteEndPoint.ToString());

                        connection.Stream.Flush();
                        connection.Stream.Close();
                        connection.TcpClient.Close();
                        connection.Stream = null;
                        connection.TcpClient = null;

                        // Ok
                        response.Connected = false;
                        response.DataAvailable = false;
                        response.Success = true;

                        // Suppression d'une entrée dans la table des connexions
                        m_connections.Remove(request.Cid);

                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Message = Strings.SERVER_SIDE + ex.Message;
                    }
                }
            }

            Log(response.Message, ESeverity.INFO);
            return response;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Lecture de données
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>les données lues dans response.Data</returns>
        /// -----------------------------------------------------------------------------
        public ReadResponse Read(GenericRequest request)
        {
            ReadResponse response = new ReadResponse();

            if (CheckUser(ref request, ref response))
            {
                TunnelConnection connection = CheckConnection(ref request, ref response);
                if (connection != null)
                {
                    if (response.Connected && response.DataAvailable)
                    {
                        // Données disponibles
                        try
                        {
                            byte[] buffer = new byte[BUFFER_SIZE];
                            int count = connection.Stream.Read(buffer, 0, BUFFER_SIZE);
                            if (count > 0)
                            {
                                Array.Resize(ref buffer, count);
                                response.Success = true;
                                response.Message = string.Empty;
                                Bdt.Shared.Runtime.Program.StaticXorEncoder(ref buffer, request.Cid);
                                response.Data = buffer;
                            }
                            else
                            {
                                response.Success = false;
                                response.Data = null;
                                response.Message = Strings.SERVER_SIDE + Strings.DISCONNECTION_DETECTED;
                            }
                        }
                        catch (Exception ex)
                        {
                            response.Success = false;
                            response.Data = null;
                            response.Message = Strings.SERVER_SIDE + ex.Message;
                        }
                    }
                    else
                    {
                        // Pas de données disponibles
                        response.Success = true;
                        response.Message = string.Empty;
                        response.Data = new byte[] { };
                    }
                }
            }

            return response;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture de données
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>une réponse générique</returns>
        /// -----------------------------------------------------------------------------
        public GenericResponse Write(WriteRequest request)
        {
            GenericResponse response = new GenericResponse();

            if (CheckUser(ref request, ref response))
            {
                TunnelConnection connection = CheckConnection(ref request, ref response);
                if (connection != null)
                {
                    try
                    {
                        byte[] result = request.Data;
                        Bdt.Shared.Runtime.Program.StaticXorEncoder(ref result, request.Cid);
                        connection.Stream.Write(result, 0, result.Length);
                        response.Success = true;
                        response.Message = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Message = Strings.SERVER_SIDE + ex.Message;
                    }
                }
            }

            return response;
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
        public void Log(object sender, string message, ESeverity severity)
        {
            if (m_logger != null)
            {
                m_logger.Log(sender, message, severity);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture d'une entrée de log. Ne sera pas prise en compte si le log est inactif
        /// ou si le filtre l'impose
        /// </summary>
        /// <param name="message">le message à logger</param>
        /// <param name="severity">la sévérité</param>
        /// -----------------------------------------------------------------------------
        public void Log(string message, ESeverity severity)
        {
            Log(this, message, severity);
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


