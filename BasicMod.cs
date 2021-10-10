using BepInEx;
using UnityEngine;
using HarmonyLib;

namespace BasicMod
{
	[BepInPlugin(pluginGuid, pluginName, pluginVersion)]
	public class BasicMod : BaseUnityPlugin
	{
		public const string pluginGuid = "potioncraft.basicmod";
		public const string pluginName = "Basic Mod";
		public const string pluginVersion = "0.0.0.1";

		public void Awake()
		{
			DoPatching();
			Debug.Log("Loaded BasicMod!");
			DuplicateRecipes.Awake();
		}


		//HarmonyPatching
		private void DoPatching()
		{
			var harmony = new HarmonyLib.Harmony("hxp.basicmod");
			harmony.PatchAll();
		}

		public static void LogSalts()
		{
			Debug.Log("Called LogSalts");
			foreach (InventoryItem allSalt in Salt.allSalts)
			{
				Debug.Log(allSalt.name);
			}
		}

		public static void LogEffects()
		{
			foreach (PotionEffect effect in PotionEffect.allPotionEffects)
			{
				Debug.Log(effect.name);
			}
		}

		//Adds and intialises custom salts
		public static void RegisterSalts()
		{
			//Default salt start
			ModSalt dsalt = SaltFactory.CreateSalt("Default Salt");
			//Default salt end

		}

	}


	[HarmonyPatch(typeof(TradableUpgrade))]
	[HarmonyPatch("Initialize")]
	class SaltPatch
	{
		static void Postfix()
		{
			//Add our salts here
			BasicMod.RegisterSalts();

		}
	}
}
