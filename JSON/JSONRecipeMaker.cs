using BasicMod.Factories;
using BasicMod.JSON;
using BasicMod.ModObjects;
using Newtonsoft.Json;
using Npc.Parts;
using Npc.Parts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


/*
CitizenQuests
FemaleQuests
NecromancyQuests
RogueQuests
WarriorQuests
*/

namespace BasicMod
{
    public class JSONRecipeMaker
    {
        public string name { get; set; }

        public string description { get; set; }
        public bool knownAtStart { get; set; }

        public bool unlockedByDefault { get; set; }
        public string recipeIconPath { get; set; }
        public string recipeBookmarkPath { get; set; }

        public int amount { get; set; }

        public JSONRecipeSlots slots { get; set; }

        public ModLegendaryRecipe recipe;

        //Use salt name for now
        public string result { get; set; }
        public bool usePotionColors { get; set; }

        public JSONColor[] colors { get; set; }

        public JSONMod modSettings;

        public class JSONColor
        {
            public string color { get; set; }

            public Color getUnityColor()
            {
                Color outcol;
                ColorUtility.TryParseHtmlString(color, out outcol);
                return outcol;
            }
    }


        public ModLegendaryRecipe CreateRecipe()
        {
            ModLegendaryRecipe rep = LegendaryRecipeFactory.CreateRecipe(name);


            if( modSettings != null)
            {
                rep.customAssetsPath = modSettings.directory + "/Assets/";
            }


            LegendarySaltPile resultPile = LegendaryRecipeFactory.CreateAlchemyMachineProduct(name +" Result");
            ConfigureResultPile(resultPile);
            LocalDict.AddKeyToDictionary(MakeKey(name) + "_title", name);
            LocalDict.AddKeyToDictionary(MakeKey(name) + "_description", description);

            rep.SetGraphicsPaths(recipeIconPath,recipeBookmarkPath);
            rep.SetUnlockedByDefault(unlockedByDefault).SetResultItem(resultPile).SetKnownAtStart(knownAtStart);
            rep.AddRecipesToUnlock(rep);

            recipe = rep;
            return rep;

        }

        public void ConfigureResultPile( LegendarySaltPile resultPile)
        {
            resultPile.convertToSaltOnPickup = Salt.GetByName(result);
            resultPile.getColorsFromMachine = usePotionColors;
            resultPile.amountOfSalt = amount;

            //Debug.Log(colours);

            AlchemyMachineProduct.ColorsList colorsList = new AlchemyMachineProduct.ColorsList();
            foreach( JSONColor color in colors)
            {
                colorsList.colors.Add(color.getUnityColor());
            }


            resultPile.colors.Add(colorsList);
            resultPile.colors.Add(colorsList);
            resultPile.colors.Add(colorsList);
            resultPile.colors.Add(colorsList);
            resultPile.colors.Add(colorsList);
            resultPile.colors.Add(colorsList);
            resultPile.colors.Add(colorsList);
            resultPile.colors.Add(colorsList);
            resultPile.colors.Add(colorsList);
            resultPile.colors.Add(colorsList);
        }

        public string MakeKey(string longName)
        {
            return longName.ToLower().Replace(" ", "_");
        }

        public void ConfigureRecipe()
        {
            slots.SetRecipesSlots(recipe);
        }

    }

}
