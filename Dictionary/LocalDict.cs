using HarmonyLib;
using LocalizationSystem;
using System.Collections.Generic;
using UnityEngine;

namespace BasicMod
{
    public class LocalDict
    {
        public static Dictionary<string, string> eng_us = new Dictionary<string, string>();

        public static void AddKeyToDictionary( string key, string text)
        {
            eng_us[key] = text;
        }

        public static string GetTextFromKey(string key)
        {
           return eng_us[key];
        }

    }


    [HarmonyPatch(typeof(Key))]
    [HarmonyPatch("GetText")]
    class LocPatch
    {
        static bool Prefix(Key __instance, ref string __result)
        {
            string key = Traverse.Create(__instance).Field("key").GetValue() as string;
            if (LocalDict.eng_us.ContainsKey(key))
            {
                string text = LocalDict.GetTextFromKey(key);
                //Debug.Log(key);
                __result = text;
                return false;
            }

            return true;
        }
    }


}
