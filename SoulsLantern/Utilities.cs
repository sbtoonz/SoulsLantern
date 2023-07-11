using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SoulsLantern
{
    public static class Utilities
    {
	    internal enum ConnectionState
	    {
		    Server,
		    Client,
		    Local,
		    Unknown
	    }
	    internal static ConnectionState GetConnectionState()
	    {
			if (ZNet.instance == null) return ConnectionState.Local;
		    if (ZNet.instance.IsServer() && ZNet.instance.IsDedicated()) //server
		    {
			    return ConnectionState.Server;
		    }
		    
		    if (ZNet.m_isServer && ZNet.m_openServer) // Local server
		    {
			    return ConnectionState.Server;
		    }
		    if (!ZNet.instance.IsServer() && !ZNet.instance.IsDedicated()) //client
		    {
			    return ConnectionState.Client;
		    }

		    if (ZNet.IsSinglePlayer) 
		    {
			    return ConnectionState.Local;
		    }

		    return ConnectionState.Unknown;
	    }
        internal static AssetBundle? LoadAssetBundle(string bundleName)
        {
            var resource = typeof(SoulsLanternMod).Assembly.GetManifestResourceNames().Single
                (s => s.EndsWith(bundleName));
            using var stream = typeof(SoulsLanternMod).Assembly.GetManifestResourceStream(resource);
            return AssetBundle.LoadFromStream(stream);
        }

        internal static void LoadAssets(AssetBundle? bundle, ZNetScene zNetScene)
        {
            var tmp = bundle?.LoadAllAssets();
            if (zNetScene.m_prefabs.Count <= 0) return;
            if (tmp == null) return;
            foreach (var o in tmp)
            {
                var obj = (GameObject)o;
                zNetScene.m_prefabs.Add(obj);
                var hashcode = obj.GetHashCode();
                zNetScene.m_namedPrefabs.Add(hashcode, obj);
            }
        }
       
    }
}