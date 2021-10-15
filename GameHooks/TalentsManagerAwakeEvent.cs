using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlayerManager;

namespace BasicMod.GameHooks
{
    [HarmonyPatch(typeof(TalentsSubManager), "OnManagerAwake")]
    public static class TalentsManagerAwakeEvent
    {
        public static EventHandler<TalentsManagerAwakeEventArgs> OnTalentsManagerAwake;

        static void Postfix()
        {
            var e = new TalentsManagerAwakeEventArgs();
            OnTalentsManagerAwake?.Invoke(null, e);

        }


    }

    public class TalentsManagerAwakeEventArgs : EventArgs
    {
    }
}