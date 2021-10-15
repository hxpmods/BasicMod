using HarmonyLib;
using LegendaryRecipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMod.GameHooks
{
    class LegendaryRecipeObjectEvents
    {
        public static EventHandler<EventArgs> OnLoad;
        public static EventHandler<EventArgs> OnStart;

        [HarmonyPatch(typeof(LegendaryRecipeObject))]
        [HarmonyPatch("Start")]
        class onStartPatch
        {
            static void Postfix()
            {
                EventArgs e = new EventArgs();
                OnStart?.Invoke(null, e);
            }
        }

        [HarmonyPatch(typeof(LegendaryRecipeObject))]
        [HarmonyPatch("OnLoad")]
        class OnLoadPatch
        {
            static void Postfix()
            {
                EventArgs e = new EventArgs();
                OnLoad?.Invoke(null, e);
            }

        }

    }
}
