/// -----------------------------------------------------------------------------
/// BoutDuTunnel
/// Sebastien LEBRETON
/// sebastien.lebreton[-at-]free.fr
/// -----------------------------------------------------------------------------

#region " Inclusions "
using System;
using Bdt.Shared.Request;
using Bdt.Shared.Response;
#endregion

namespace Bdt.Shared.Service
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// Les services proposés par une instance de Tunnel
    /// </summary>
    /// -----------------------------------------------------------------------------
    public interface ITunnel
    {

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Authentification d'un utilisateur
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>une réponse AuthenticateResponse avec un uid</returns>
        /// -----------------------------------------------------------------------------
        AuthenticateResponse Authenticate(AuthenticateRequest request);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// La version du serveur
        /// </summary>
        /// <returns>La version de l'assembly</returns>
        /// -----------------------------------------------------------------------------
        GenericResponse Version();

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Etablissement d'une nouvelle connexion
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>le cid de connexion</returns>
        /// -----------------------------------------------------------------------------
        ConnectResponse Connect(ConnectRequest request);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Déconnexion
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>une reponse générique</returns>
        /// -----------------------------------------------------------------------------
        GenericResponse Disconnect(GenericRequest request);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Lecture de données
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>les données lues dans response.Data</returns>
        /// -----------------------------------------------------------------------------
        ReadResponse Read(GenericRequest request);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Ecriture de données
        /// </summary>
        /// <param name="request">la requête</param>
        /// <returns>une réponse générique</returns>
        /// -----------------------------------------------------------------------------
        GenericResponse Write(WriteRequest request);

    }

}


