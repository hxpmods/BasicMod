using BasicMod.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BasicMod.JSON
{


    public static class JSONRecipeLoader
    {
        public static List<JSONRecipeMaker> loadedRecipes = new List<JSONRecipeMaker>();
        public static void Awake()
        {
            foreach(var mod in JSONModLoader.loadedMods)
            {
                string directory = mod.directory + "/JSON/Recipes";
                ProcessDirectory(directory, mod);
            }

            ProcessDirectory("BepinEx/Plugins/JSON/Recipes");

            LegendaryRecipeFactory.onPreRegisterRecipes += (_,e) =>
            {
                if (loadedRecipes.Count > 0)
                {
                    foreach (var JSONRep in loadedRecipes)
                    {
                        JSONRep.CreateRecipe();
                    }
                    var recipes = from JSONRep in loadedRecipes select JSONRep.recipe;
                    LegendaryRecipeFactory.allRecipes.AddRange(recipes);
                }
            };

            LegendaryRecipeFactory.onPreConfigureRecipes += (_, e) =>
            {
                foreach(var JSONRep in loadedRecipes)
                {
                    JSONRep.ConfigureRecipe();
                }
            };

        }


        public static void ProcessDirectory(string targetDirectory, JSONMod modSettings = null)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, modSettings);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static void ProcessFile(string path, JSONMod modSettings = null)
        {
            try
            {
                string json = File.ReadAllText(path);

                if (json.IsValidJSON())
                {
                    JSONRecipeMaker recipe = deserialiseJSONRecipes(json);
                    recipe.modSettings = modSettings;
                    if (recipe != null) loadedRecipes.Add(recipe);
                }
                else
                {
                    Debug.Log(path + " is not a valid JSON file.");
                }


            }
            catch (Exception ex)
            {
                Debug.Log("Error while reading: " + ex.Message);
            }

        }




        public static JSONRecipeMaker deserialiseJSONRecipes(string json)
        {
            try
            {
                JSONRecipeMaker j = JsonConvert.DeserializeObject<JSONRecipeMaker>(json);
                return j;
            }
            catch (Exception ex)
            {
                Debug.Log("Error while deserialising: " + ex.Message);
                return null;
            }

        }
    }


    


}
