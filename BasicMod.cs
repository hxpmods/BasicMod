using BepInEx;
using UnityEngine;
using HarmonyLib;
using BasicMod.GameHooks;
using BasicMod.SaltUI;
using BasicMod.SaltUI.WindowElements;
using ScrollWindowHint;

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


			/*
			NewGameEvent.OnNewGame += (_, e) =>
			{
				// Mark the event handled so the game's own code doesnt run.
				e.Handled = true;

				//Do the StartNewGame stuff minus the tutorial
				Managers.Menu.CloseMenu();
				Managers.SaveLoad.LoadNewGameState();
				Managers.SaveLoad.SelectedProgressState = null;

				//Basic Mod welcome message start

				ModHintParameters param = new ModHintParameters();
				param.darkScreen = true; //Locks out the rest of the screen

				//Set window anchor position
				param.anchorPosition = new Vector2(0, 5);

				//Scroll windows want keys, so we use LocalDict
				LocalDict.AddKeyToDictionary($"{pluginGuid}:welcometitle", "Basic Mod");
				param.titleKey = $"{pluginGuid}:welcometitle";


				//Create a text window element to display body text
				var text1 = new TextWindowElement();
				LocalDict.AddKeyToDictionary($"{pluginGuid}:welcometext", $"Welcome to Basic Mod version {pluginVersion}.\nWould you like to play the base game tutorial?");
				text1.text = $"{pluginGuid}:welcometext";



				LocalDict.AddKeyToDictionary($"{pluginGuid}:tutorialyes", "Yes, please.");
				LocalDict.AddKeyToDictionary($"{pluginGuid}:tutorialno", "No, I'm a master alchemist.\nWhy would you even ask me that?");
				//OptionWithButtonWindowElement displays text with a button to the right of it. If necessary, you can change the elements offsets to line things up how you like.
				var button1 = new OptionWithButtonWindowElement();
				button1.text = $"{pluginGuid}:tutorialyes";
				var button2 = new OptionWithButtonWindowElement();
				button2.textOffset += new Vector2(0, 0.2f);
				button2.text = $"{pluginGuid}:tutorialno";

				param.scrollWindowElements.Add(text1);
				param.scrollWindowElements.Add(button1);
				param.scrollWindowElements.Add(button2);


				//See start tutorial button class 
				param.buttonPressedAction = new StartTutorialButtonAction(Managers.Game.scrollWindow);

				ScrollWindow.Open(new ModScrollWindowContentController(param));
			

			};*/

			

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
