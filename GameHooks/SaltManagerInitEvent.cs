using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMod.GameHooks
{
    [HarmonyPatch(typeof(Salt), "Initialize")]
    public static class SaltManagerInitEvent
    {
        public static EventHandler<SaltManagerInitEventArgs> OnSaltManagerInit;

        static void Postfix()
        {
            var e = new SaltManagerInitEventArgs();
            OnSaltManagerInit?.Invoke(null, e);

        }


    }

    public class SaltManagerInitEventArgs : EventArgs
    {
    }
}