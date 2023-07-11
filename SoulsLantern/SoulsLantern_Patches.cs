using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace SoulsLantern
{
    public class SoulsLantern_Patches
    {
        [HarmonyPatch(typeof(Console), nameof(Console.Awake))]
        public static class MenuScenePatch
        {
            internal static bool hasRanPatch = false;
            public static void Postfix(Console __instance)
            {
                if(hasRanPatch)return;
                SoulsLanternMod.soulsLantern = Object.Instantiate(Resources.FindObjectsOfTypeAll<GameObject>().ToList()
                    .Find(x => x.name == "Lantern"), SoulsLanternMod.ItemHolder.transform, false);
                SoulsLanternMod.soulsLantern.name = "Souls_Lantern";
                Object.DestroyImmediate(SoulsLanternMod.soulsLantern.transform.Find("attach_back").gameObject);
                SoulsLanternMod.soulsLantern.transform.Find("attach").name = "attach_Hips";
                SoulsLanternMod.soulsLantern.transform.Find("attach_Hips/equiped").localScale =
                    new Vector3(0.75f, 0.75f, 0.75f);
                SoulsLanternMod.soulsLantern.transform.Find("attach_Hips/equiped/").transform.position =
                    new Vector3(-0.165f, 0.4f, 0.165f);
                SoulsLanternMod.soulsLantern.transform.Find("attach_Hips/").transform.position =
                    new Vector3(0.0025f, -0.014f, -0.0025f);
                var id =  SoulsLanternMod.soulsLantern.GetComponent<ItemDrop>();
                id.m_itemData.m_shared.m_name = "$souls_lantern";
                id.m_itemData.m_shared.m_description = "$souls_lantern_description";
                id.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.Utility;
                SoulsLanternMod.soulsRecipe = ScriptableObject.CreateInstance<SoulsLanternRecipe>();
                SoulsLanternMod.soulsRecipe.name = "SoulsLantern_Recipe";
                SoulsLanternMod.soulsRecipe.m_amount = 1;
                SoulsLanternMod.soulsRecipe.m_enabled = true;
                SoulsLanternMod.soulsRecipe.m_item = SoulsLanternMod.soulsLantern.GetComponent<ItemDrop>();
                SoulsLanternMod.soulsRecipe.m_resources = new Piece.Requirement[]
                {
                    new Piece.Requirement
                    {
                        m_amount = 1,
                        m_recover = true,
                        m_resItem = Resources.FindObjectsOfTypeAll<GameObject>().ToList().Find(x => x.name == "SurtlingCore")
                            .GetComponent<ItemDrop>(),
                        m_amountPerLevel = 1,
                    },
                    new Piece.Requirement
                    {
                        m_amount = 1,
                        m_recover = true,
                        m_resItem = Resources.FindObjectsOfTypeAll<GameObject>().ToList().Find(x => x.name == "Bronze")
                            .GetComponent<ItemDrop>(),
                        m_amountPerLevel = 2,
                    },
                    new Piece.Requirement
                    {
                        m_amount = 1,
                        m_recover = true,
                        m_resItem = Resources.FindObjectsOfTypeAll<GameObject>().ToList().Find(x => x.name == "Crystal")
                            .GetComponent<ItemDrop>(),
                        m_amountPerLevel = 1,
                    }
                };
                hasRanPatch = true;
            }
        }
        [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
        public static class InsertLanternPatch
        {
            public static void Prefix(ZNetScene __instance)
            {
                if(__instance.m_prefabs.Count <=0)return;
                __instance.m_prefabs.Add(SoulsLanternMod.soulsLantern);
            }
        }

        [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.Awake))]
        public static class InsertLanternObjPatch
        {
            public static void Prefix(ObjectDB __instance)
            {
                if(__instance.m_items.Count <=0 || __instance.GetItemPrefab("Amber") == null)return;
                __instance.m_items.Add(SoulsLanternMod.soulsLantern);
                __instance.m_recipes.Add(SoulsLanternMod.soulsRecipe);
            }
        }

        [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.SetupGui))]
        public static class localizationPatch
        {
            public static void Prefix()
            {
                Localization.instance.AddWord("souls_lantern", "HipLantern");
                Localization.instance.AddWord("souls_lantern_description", "A Dverger lantern that seems to hang from the hip");
            }
        }

    }
}