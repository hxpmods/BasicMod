using Books.GoalsBook;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMod.GameHooks
{
    [HarmonyPatch(typeof(GoalsLoader), "GetGoalByName")]
    public static class GoalsLoaderGetByName
    {
        public static EventHandler<GoalsLoaderGetByNameEventArgs> GoalsLoaderGetByNameEvent;

        static bool Prefix(string name, ref Goal __result)
        {
            var e = new GoalsLoaderGetByNameEventArgs(name);
            GoalsLoaderGetByNameEvent?.Invoke(null, e);


            if( e.handled ){
                __result = e.result;
                return false;
            }

            return true;
        }


    }

    public class GoalsLoaderGetByNameEventArgs : EventArgs
    {
        public bool handled = false;
        public string name;
        public Goal result;

        public GoalsLoaderGetByNameEventArgs(string name)
        {
            this.name = name;
        }
    }
}