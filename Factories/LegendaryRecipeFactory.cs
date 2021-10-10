using HarmonyLib;
using LegendaryRecipes;
using QuestSystem.DesiredItems;
using System.Collections.Generic;
using UnityEngine;

namespace BasicMod
{
    public class LegendaryRecipeFactory
    {
        public static List<ModLegendaryRecipe> allRecipes = new List<ModLegendaryRecipe>();
        public static GameObject legendarySaltPilePrefab;
        public static ItemFromInventoryPreset saltPile_soundPreset;

        public static void CopyGameObjects()
        {
            Debug.Log("Copying recipe game objects");
            legendarySaltPilePrefab = AlchemyMachineProduct.allProducts[5].prefab;
            saltPile_soundPreset = AlchemyMachineProduct.allProducts[5].soundPreset;
        }

        public static ModLegendaryRecipe CreateRecipe(string _name) //Creates a new legendary recipe instance to be modified
        {
            ModLegendaryRecipe legendaryRecipe = ScriptableObject.CreateInstance<ModLegendaryRecipe>();
            legendaryRecipe.name = _name;
            legendaryRecipe.unlockedByDefault = false;
            AddRecipe(legendaryRecipe);
            return legendaryRecipe;
        }

        public static LegendarySaltPile CreateAlchemyMachineProduct(string _name) 
        {
            //Just salts for now
            LegendarySaltPile result = ScriptableObject.CreateInstance<LegendarySaltPile>();

           result.soundPreset = saltPile_soundPreset;
           
           // result.visualObject = result.prefab.GetComponentInChildren<AlchemyMachineProductVisualObject>();
            return result;


        }

        public static void AddRecipe(ModLegendaryRecipe recipe)
        {
            //Adds a recipe to this recipeFactory
            allRecipes.Add(recipe);
        }


        public static DesiredItemEffect CreateDesiredPotion(PotionEffect[] arrayOfEffects)
        {
            DesiredItemEffect potion = ScriptableObject.CreateInstance<DesiredItemEffect>();
            potion.effects = arrayOfEffects;
            return potion;
        }


        public static void RegisterAllRecipes()
        {
            foreach (ModLegendaryRecipe recipe in allRecipes)
            {

                Managers.Ingredient.legendaryRecipeSubManager.allLegendaryRecipes.Add(recipe);
            }
        }

        public static void RegisterRecipesAsKnownAtStart()
        {
            foreach (ModLegendaryRecipe recipe in allRecipes)
            {
                if (recipe.knownAtStart)
                {
                    Debug.Log("Registering " + recipe.name + " as known at start.");
                    Managers.Ingredient.settings.knownLegendaryRecipesOnStart.Add(recipe);
                }
            }
        }

        public static void InitializeVisualObjects()
        {
            foreach (ModLegendaryRecipe recipe in allRecipes)
            {

                //Assumedly, we want to take a similar approach to how SaltFactory handles VisualObjects, but this should do for now.
                //Debug.Log(legendarySaltPilePrefab.GetComponentInChildren<AlchemyMachineProductVisualObject>());
                recipe.resultItem.prefab = legendarySaltPilePrefab;
                recipe.resultItem.soundPreset = saltPile_soundPreset;

                //recipe.resultItem.customDescription = "Write Me";
                //recipe.resultItem.customTitle = "Write Me";
                //recipe.resultItem.useCustomKeyAsTitle = false;

                //Reflection via Harmony's Traverse.Create to set private icon fields.
                Traverse.Create(recipe.resultItem).Field("recipeIcon").SetValue(recipe.GetRecipeIcon());
                Traverse.Create(recipe.resultItem).Field("activeMarkerIcon").SetValue(recipe.GetRecipeBookmark());
                Traverse.Create(recipe.resultItem).Field("idleMarkerIcon").SetValue(recipe.GetRecipeBookmark());
                Traverse.Create(recipe.resultItem).Field("damagedMarkerIcon").SetValue(recipe.GetRecipeBookmark());
                //recipe.resultItem.visualObject = recipe.resultItem.prefab.GetComponentInChildren<AlchemyMachineProductVisualObject>();
            }
        }

        public static void RegisterAllStartingRecipes()
        {
            Debug.Log("Begin registering starting recipes to LegendaryRecipeObject");

            foreach (ModLegendaryRecipe recipe in allRecipes)
            {
                //Debug.Log(recipe);
                if (recipe.knownAtStart)
                {
                    Debug.Log("Adding " + recipe.name + " to LegendaryRecipeObject at start.");
                    Managers.Ingredient.legendaryRecipeSubManager.legendaryRecipeObject.AddLegendaryRecipe(recipe);
                }
            }
        }


    }

    //Harmony patches start

    [HarmonyPatch(typeof(LegendaryRecipeObject))]
    [HarmonyPatch("Start")]
    class AddRecipeAtStartPatch
    {
        static void Postfix()
        {
            Debug.Log("Registering Recipes as known at start.");
            LegendaryRecipeFactory.RegisterRecipesAsKnownAtStart();
            //Make modded recipes known in Legendary Recipe book at start up
            LegendaryRecipeFactory.RegisterAllStartingRecipes();

        }
    }

    [HarmonyPatch(typeof(LegendaryRecipeObject))]
    [HarmonyPatch("OnLoad")]
    class AddNewRecipesOnload
    {
        //If a recipe in this mod would be added on start, attempt to add it on load to ensure all recipes get added.
        //The game already checks if a recipe is known to avoid duplicate legendary recipes.

        static void Postfix()
        {
            foreach (LegendaryRecipe recipe in LegendaryRecipeFactory.allRecipes)
            {
                if (Managers.Ingredient.settings.knownLegendaryRecipesOnStart.Contains(recipe))
                {
                    Managers.Ingredient.legendaryRecipeSubManager.legendaryRecipeObject.AddLegendaryRecipe(recipe);
                    if (recipe.unlockedByDefault) { Managers.Ingredient.legendaryRecipeSubManager.UnlockRecipe(recipe); }
                }
            }
        }

    }


    [HarmonyPatch(typeof(PotionManager))]
    [HarmonyPatch("InitiateScriptableObjects")]
    class RegisterRecipesOnEffectLoadPatch
    {
        static void Postfix()
        {
            //BasicMod.LogEffects();
            //Recipes are then configured after the Potion Manager, to ensure we have full access to all potion effects.
            //This is where you define the recipes themselves, and further configure the result.
            LegendaryRecipeFactory.RegisterAllRecipes();
        }
    }

    [HarmonyPatch(typeof(RecipeMapManager))]
    [HarmonyPatch("OnMapObjectAwake")]
    class SetProductVisualObjectsPatch
    {
        static void Postfix()
        {
            //This makes sure all visuals work as intended. There is a better place to do this, but I haven't found it yet.
            LegendaryRecipeFactory.CopyGameObjects();
            LegendaryRecipeFactory.InitializeVisualObjects();
        }
    }

    //Harmony patches end
}


