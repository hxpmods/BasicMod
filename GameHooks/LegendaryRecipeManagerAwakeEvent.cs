using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IngredientManager;

namespace BasicMod.GameHooks
{
    [HarmonyPatch(typeof(LegendaryRecipeSubManager))]
    [HarmonyPatch("OnManagerAwake")]
    public static class LegendaryRecipeManagerAwakeEvent
    {
        public static EventHandler<LegendaryRecipeManagerAwakeEventArgs> onLegendaryRecipeManagerAwake;

        static void Postfix()
        {
            var e = new LegendaryRecipeManagerAwakeEventArgs();
            onLegendaryRecipeManagerAwake?.Invoke(null, e);

        }


    }

    public class LegendaryRecipeManagerAwakeEventArgs : EventArgs
    {
    }
}