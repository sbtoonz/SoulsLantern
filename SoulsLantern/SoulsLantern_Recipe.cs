using UnityEngine;

namespace SoulsLantern
{
    public class SoulsLanternRecipe : Recipe
    {
        public static SoulsLanternRecipe CreateInstance(ItemDrop item, 
            int amount, 
            bool enabled,
            float quality,
            CraftingStation station, 
            CraftingStation repair,
            int minstationlev,
            bool req1,
            Piece.Requirement[] resources)
        {
            var t = ScriptableObject.CreateInstance<SoulsLanternRecipe>();
            t.m_item = item;
            t.m_amount = amount;
            t.m_enabled = enabled;
            t.m_qualityResultAmountMultiplier = quality;
            t.m_craftingStation = station;
            t.m_repairStation = repair;
            t.m_minStationLevel = minstationlev;
            t.m_requireOnlyOneIngredient = req1;
            t.m_resources = resources;
            return t;
        }
    }
}