using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ServerSync;
using UnityEngine;

namespace SoulsLantern
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class SoulsLanternMod : BaseUnityPlugin
    {
        internal const string ModName = "SoulsLanternMod";
        internal const string ModVersion = "1.0";
        private const string ModGUID = "SoulsLanternMod";
        private static Harmony harmony = null!;

        #region ConfigSync
        ConfigSync configSync = new(ModGUID) 
            { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion};
        internal static ConfigEntry<bool> ServerConfigLocked = null!;
        ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
        ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

        

        #endregion

        internal static ConfigEntry<Dictionary<string, string>> possibleTranslations;

        internal static GameObject ItemHolder;
        internal static GameObject soulsLantern;
        internal static Recipe soulsRecipe;
            
        
        public void Awake()
        {
            SoulsLanternMod.ItemHolder = new GameObject("Holder");
            SoulsLanternMod.ItemHolder.SetActive(false);
            ItemHolder.transform.SetParent(null);
            Object.DontDestroyOnLoad(SoulsLanternMod.ItemHolder);
            Assembly assembly = Assembly.GetExecutingAssembly();
            harmony = new(ModGUID);
            harmony.PatchAll(assembly);
            ServerConfigLocked = config("1 - General", "Lock Configuration", true, "If on, the configuration is locked and can be changed by server admins only.");
            configSync.AddLockingConfigEntry(ServerConfigLocked);
            
        }
    }
}