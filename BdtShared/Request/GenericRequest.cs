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
    /// Une demande générique
    /// </summary>
    /// -----------------------------------------------------------------------------
    [Serializable()]
    public struct GenericRequest : IGenericRequest
    {

        #region " Attributs "
        private int m_cid;
        private int m_uid;
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
        #endregion

        #region " Methodes "
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="uid">Le jeton utilisateur</param>
        /// <param name="cid">Le jeton de connexion</param>
        /// -----------------------------------------------------------------------------
        public GenericRequest (int uid, int cid)
        {
            this.m_uid = uid;
            this.m_cid = cid;
        }
        #endregion

    }

}

