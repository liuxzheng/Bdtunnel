﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :2.0.50727.42
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bdt.Server.Resources {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bdt.Server.Resources.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Access denied for {0}: inactive or nonexistent account.
        /// </summary>
        internal static string ACCESS_DENIED {
            get {
                return ResourceManager.GetString("ACCESS_DENIED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Access denied for {0}: bad password.
        /// </summary>
        internal static string ACCESS_DENIED_BAD_PASSWORD {
            get {
                return ResourceManager.GetString("ACCESS_DENIED_BAD_PASSWORD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Access granted to {0}.
        /// </summary>
        internal static string ACCESS_GRANTED {
            get {
                return ResourceManager.GetString("ACCESS_GRANTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Cid not found.
        /// </summary>
        internal static string CID_NOT_FOUND {
            get {
                return ResourceManager.GetString("CID_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Connected to {0} ({1}).
        /// </summary>
        internal static string CONNECTED {
            get {
                return ResourceManager.GetString("CONNECTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Connection refused to {0}:{1} {2}.
        /// </summary>
        internal static string CONNECTION_REFUSED {
            get {
                return ResourceManager.GetString("CONNECTION_REFUSED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Connection timeout: {0}.
        /// </summary>
        internal static string CONNECTION_TIMEOUT {
            get {
                return ResourceManager.GetString("CONNECTION_TIMEOUT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Disconnected from {0}.
        /// </summary>
        internal static string DISCONNECTED {
            get {
                return ResourceManager.GetString("DISCONNECTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Disconnection detected.
        /// </summary>
        internal static string DISCONNECTION_DETECTED {
            get {
                return ResourceManager.GetString("DISCONNECTION_DETECTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à [server] .
        /// </summary>
        internal static string SERVER_SIDE {
            get {
                return ResourceManager.GetString("SERVER_SIDE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Server started, press enter key to exit....
        /// </summary>
        internal static string SERVER_STARTED {
            get {
                return ResourceManager.GetString("SERVER_STARTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à -=[ BoutDuTunnel Server v{0} - Sebastien LEBRETON ]=-.
        /// </summary>
        internal static string SERVER_TITLE {
            get {
                return ResourceManager.GetString("SERVER_TITLE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Uid not found.
        /// </summary>
        internal static string UID_NOT_FOUND {
            get {
                return ResourceManager.GetString("UID_NOT_FOUND", resourceCulture);
            }
        }
    }
}
