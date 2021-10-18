using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMod.GameHooks
{
    public static class NpcManagerEvents
    {
        public static EventHandler<EventArgs> onInitiateScriptableObjects;

        [HarmonyPatch(typeof(NpcManager), "InitiateScriptableObjects")]
        public static class OnInititateScriptableObjectsPatch
        {
            
            static void Postfix()
            {
                EventArgs e = new EventArgs();
                onInitiateScriptableObjects.Invoke(null, e);
            }
        }

    }
}
