using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMod.GameHooks
{
    [HarmonyPatch(typeof(GameManager), "Start")]
    public static class GameStartEvent
    {
        public static EventHandler<GameStartEventArgs> OnGameStart;

        static void Postfix()
        {
            var e = new GameStartEventArgs();
            OnGameStart?.Invoke(null, e);

        }


    }

    public class GameStartEventArgs : EventArgs
    {
    }
}