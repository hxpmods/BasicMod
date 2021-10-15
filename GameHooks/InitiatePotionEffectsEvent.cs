using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMod.GameHooks
{
    [HarmonyPatch(typeof(PotionManager))]
    [HarmonyPatch("InitiateScriptableObjects")]
    public class InitiatePotionEffectsEvent
    {
        public static EventHandler<EventArgs> OnInitiate;

        static void Postfix()
        {
            EventArgs e = new EventArgs();
            OnInitiate?.Invoke(null, e);
        }
    }
}
