using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod
{
    public class ModLegendaryRecipe : LegendaryRecipe
    {

        public bool knownAtStart = false;
        public string recipeIconPath = "Default Salt Recipe Icon.png";
        public string recipeBookmarkPath = "Default Salt Recipe Bookmark.png";
        public Sprite recipeIcon;
        public string customAssetsPath;

        public ModLegendaryRecipe SetUnlockedByDefault(bool _b)
        {
            unlockedByDefault = _b;
            return this;
        }

        public ModLegendaryRecipe SetKnownAtStart(bool _b)
        {
            knownAtStart = _b;
            return this;
        }


        public ModLegendaryRecipe SetGraphicsPaths(string iconPath, string bookmarkPath)
        {
            recipeIconPath = iconPath;
            recipeBookmarkPath = bookmarkPath;

            return this;

        }

        public ModLegendaryRecipe SetResultItem(AlchemyMachineProduct _result)
        {
            resultItem = _result;

            return this;
        }

        public void AddRecipesToUnlock(LegendaryRecipe recipe)
        {
            //Debug.Log(recipe);
            if (recipesToUnlock == null)
            {
                recipesToUnlock = new List<LegendaryRecipe>();
            }
            this.recipesToUnlock.Add(recipe);
            //return this;
        }

        public Sprite GetRecipeIcon() 
        {
            if (customAssetsPath != null)
            {
                Sprite sprite = SpriteLoader.LoadSpriteFromFile(recipeIconPath, customAssetsPath);
                return sprite;
            }
            else
            {

                Sprite sprite = SpriteLoader.LoadSpriteFromFile(recipeIconPath);
                return sprite;
            }
        }

        public Sprite GetRecipeBookmark()
        {
            if (customAssetsPath != null)
            {
                Sprite sprite = SpriteLoader.LoadSpriteFromFile(recipeIconPath, customAssetsPath);
                return sprite;
            }
            else
            {

                Sprite sprite = SpriteLoader.LoadSpriteFromFile(recipeBookmarkPath);
                return sprite;
            }
        }

    }
}
