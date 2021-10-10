using HarmonyLib;
using System.Collections.Generic;
using TMPAtlasGenerationSystem;
using UnityEngine;

namespace BasicMod
{
    public class SaltFactory
    {
        //All modded salts
        public static List<ModSalt> allSalts = new List<ModSalt>();

        //These will be loaded when vanilla salts are 
        public static GameObject salt_prefab;
        public static GameObject salt_amountText;
        public static GameObject salt_pile;
        public static ItemFromInventoryPreset salt_soundPreset;

        public static void CopyGameObjectsFromSalt(Salt salt)
        {
            //Copies relevant game objects and prefabs
            salt_prefab = salt.prefab;
            salt_amountText = salt.amountText;
            salt_pile = salt.pile;
            salt_soundPreset = salt.soundPreset;
        }

        public static ModSalt CreateSalt(string _name) //Creates a new salt instance to be modified
        {
            //Create a new blank instance of ModSalt
            ModSalt salt = ScriptableObject.CreateInstance<ModSalt>();
            salt.name = _name;
            salt.description = "Describe Me!";

            //Copy a unique prefab for this salt
            salt.prefab = UnityEngine.Object.Instantiate(salt_prefab);
            salt.prefab.SetActive(false);

            //Sets these to our copies game objects
            salt.pile = salt_pile;
            salt.amountText = salt_amountText;
            salt.soundPreset = salt_soundPreset;

           

            //Add salt to our factory (there's a chance we might not want to do this)
            AddSalt(salt);

            return salt;
        }

        public static void AddSalt(ModSalt salt)
        {
            //Adds a salt to this SaltFactory
            allSalts.Add(salt);
        }

        public static void RegisterAllSalts()
        {
            foreach (ModSalt salt in allSalts)
            {
                Salt.allSalts.Add(salt); //Adds salt to the games registry
                salt.Initialize(); //Calls custom init that loads textures for salt
            }
        }


    }

    //Harmony patches start

    [HarmonyPatch(typeof(Salt))]
    [HarmonyPatch("Initialize")]
    class _SaltFactoryInitPatch
    {
        static void Postfix()
        {
            //Copy the data from the first salt object, then add our salts.
            SaltFactory.CopyGameObjectsFromSalt((Salt.allSalts[1]));
            //BasicMod.LogSalts();
            //BasicMod.AddSalts();



        }
    }

    [HarmonyPatch(typeof(GameManager))]
    [HarmonyPatch("Start")]
    class SaltFactoryGameManagerPatch
    {
        static void Postfix()
        {
            SaltFactory.RegisterAllSalts();
        }

    }

    [HarmonyPatch(typeof(SaltItem))]
    [HarmonyPatch("SpawnNewItem")]
    class SaltItemPatch
    {
        //This patch makes sure new SaltItems are set to active in the tree. This means we can set the main prefab to inactive.
        static void Postfix(ref SaltItem __result)
        {
            __result.gameObject.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(IngredientsAtlasGenerator))]
    [HarmonyPatch("GetAtlasSpriteName")]
    static class UseDefaultSmallIconPatch
    {
        static bool Prefix(ref string __result, ScriptableObject scriptableObject)
        {
            if (scriptableObject is ModSalt)
            {
                __result = "Moon Salt SmallIcon";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
