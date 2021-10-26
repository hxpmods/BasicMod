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


    public static class JSONSaltLoader
    {
        public static List<JSONSaltMaker> loadedSalts = new List<JSONSaltMaker>();
        public static void Awake()
        {

            foreach (var mod in JSONModLoader.loadedMods)
            {
                string directory = mod.directory + "/JSON/Salts";
                ProcessDirectory(directory, mod);
            }

            ProcessDirectory("BepinEx/Plugins/JSON/Salts");

            SaltFactory.onPreRegisterSaltEvent += (_, e) =>
            {
                if (loadedSalts.Count > 0)
                {
                    foreach (var JSONSalt in loadedSalts)
                    {
                        JSONSalt.CreateSalt();
                    }
                    //var salts = from JSONSalt in loadedSalts select JSONSalt.salt;
                    //SaltFactory.allSalts.AddRange(salts);
                }
            };


        }


        public static void ProcessDirectory(string targetDirectory,JSONMod modSettings = null)
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

        public static void ProcessFile(string path,JSONMod modSettings = null)
        {
            try
            {
                string json = File.ReadAllText(path);

                if (json.IsValidJSON())
                {
                    JSONSaltMaker salt = deserialiseJSONRecipes(json);
                    salt.modSettings = modSettings;
                    if (salt != null) loadedSalts.Add(salt);
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


     

        public static JSONSaltMaker deserialiseJSONRecipes(string json)
        {
            try
            {
                JSONSaltMaker j = JsonConvert.DeserializeObject<JSONSaltMaker>(json, settings: new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
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
