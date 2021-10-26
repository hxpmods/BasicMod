using BasicMod.Factories;
using QuestSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMod.JSON
{
    public class JSONRecipeSlots
    {
        //Potion slots start
        public string[] doubleVessel { get; set; }
        public string[] floorVessel { get; set; }
        public string[] leftDripper { get; set; }
        public string[] leftRetort { get; set; }
        public string[] rhombusVessel { get; set; }
        public string[] rightDripper { get; set; }
        public string[] rightRetort { get; set; }
        public string[] spiralVessel { get; set; }
        public string[] triangularVessel { get; set; }
        public string[] tripletVesselLeft { get; set; }
        public string[] tripletVesselRight { get; set; }
        public string[] tripletVesselCenter { get; set; }


        //Item slots start
        public string rightFurnace { get; set; }
        public string leftFurnace { get; set; }

        //Item slots end


        public void SetRecipesSlots( ModLegendaryRecipe recipe)
        {

            //Potion slots start

            var doubleVesselPotion = GetPotionEffects(doubleVessel);

            if( doubleVesselPotion != null)
            {
                recipe.doubleVessel = LegendaryRecipeFactory.CreateDesiredPotion(doubleVesselPotion);
            }

            var floorVesselPotion = GetPotionEffects(floorVessel);

            if (floorVesselPotion != null)
            {
                recipe.floorVessel = LegendaryRecipeFactory.CreateDesiredPotion(floorVesselPotion);
            }

            var leftDripperPotion = GetPotionEffects(leftDripper);

            if (leftDripperPotion != null)
            {
                recipe.leftDripper = LegendaryRecipeFactory.CreateDesiredPotion(leftDripperPotion);
            }

            var leftRetortPotion = GetPotionEffects(leftRetort);

            if (leftRetortPotion != null)
            {
                recipe.leftRetort = LegendaryRecipeFactory.CreateDesiredPotion(leftRetortPotion);
            }

            var rhombusVesselPotion = GetPotionEffects(rhombusVessel);

            if (rhombusVesselPotion != null)
            {
                recipe.rhombusVessel = LegendaryRecipeFactory.CreateDesiredPotion(rhombusVesselPotion);
            }

            var rightDripperPotion = GetPotionEffects(rightDripper);

            if (rightDripperPotion != null)
            {
                recipe.rightDripper = LegendaryRecipeFactory.CreateDesiredPotion(rightDripperPotion);
            }

            var rightRetortPotion = GetPotionEffects(rightRetort);

            if (rightRetortPotion != null)
            {
                recipe.rightRetort = LegendaryRecipeFactory.CreateDesiredPotion(rightRetortPotion);
            }

            var spiralVesselPotion = GetPotionEffects(spiralVessel);

            if (spiralVesselPotion != null)
            {
                recipe.spiralVessel = LegendaryRecipeFactory.CreateDesiredPotion(spiralVesselPotion);
            }

            var triangularVesselPotion = GetPotionEffects(triangularVessel);

            if (triangularVesselPotion != null)
            {
                recipe.triangularVessel = LegendaryRecipeFactory.CreateDesiredPotion(triangularVesselPotion);
            }

            var tripletVesselLeftPotion = GetPotionEffects(tripletVesselLeft);

            if (tripletVesselLeftPotion != null)
            {
                recipe.tripletVesselLeft = LegendaryRecipeFactory.CreateDesiredPotion(tripletVesselLeftPotion);
            }

            var tripletVesselRightPotion = GetPotionEffects(tripletVesselRight);

            if (tripletVesselRightPotion != null)
            {
                recipe.tripletVesselRight = LegendaryRecipeFactory.CreateDesiredPotion(tripletVesselRightPotion);
            }

            var tripletVesselCenterPotion = GetPotionEffects(tripletVesselCenter);

            if (tripletVesselCenterPotion != null)
            {
                recipe.tripletVesselCenter = LegendaryRecipeFactory.CreateDesiredPotion(tripletVesselCenterPotion);
            }

            //Potion slots end


            if (leftFurnace != null)
            {
                recipe.leftFurnace = DesiredItem.GetByName(leftFurnace);
            }

            if (rightFurnace != null)
            {
                recipe.rightFurnace = DesiredItem.GetByName(rightFurnace);
            }


        }


        public PotionEffect[] GetPotionEffects(string[] stringEffects)
        {
            PotionEffect[] desiredEffects = null;
            if (stringEffects != null)
            {

                List<PotionEffect> effectslist = new List<PotionEffect>();
                //if (desiredeffects == null) desiredeffects = new string[] { "Fire"};
                foreach (string effect in stringEffects)
                {
                    //Debug.Log(PotionEffect.GetByName(effect));
                    effectslist.Add(PotionEffect.GetByName(effect));
                }

                if (effectslist != null) desiredEffects = effectslist.ToArray();
            }

            return desiredEffects;
        }

    }
}
