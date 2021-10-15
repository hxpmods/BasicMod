using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMod.GameHooks
{
    [HarmonyPatch(typeof(GoalsManager), "Start")]
    public static class GoalManagerStartEvent
    {
        public static EventHandler<GoalManagerStartEventArgs> OnGoalManagerStart;

        static void Postfix()
        {
            var e = new GoalManagerStartEventArgs();
            OnGoalManagerStart?.Invoke(null, e);

        }


    }

    public class GoalManagerStartEventArgs: EventArgs
    {
    }

    [HarmonyPatch(typeof(GoalsManager), "Start")]
    public static class GoalManagerLoadEvent
    {
        public static EventHandler<GoalManagerLoadEventArgs> OnGoalManagerLoad;

        static void Prefix()
        {
            var e = new GoalManagerLoadEventArgs();
            OnGoalManagerLoad?.Invoke(null, e);

        }


    }

    public class GoalManagerLoadEventArgs : EventArgs
    {
    }

}
