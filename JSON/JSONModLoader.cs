using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.JSON
{
    class JSONModLoader
    {

        public static List<JSONMod> loadedMods = new List<JSONMod>();

        public static void Awake()
        {
            Debug.Log("Scanning for mod.json");
            ProcessDirectory("BepinEx/Plugins");
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, targetDirectory);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static void ProcessFile(string fileName, string inDirectory)
        {
            //Debug.Log(fileName);
            if (fileName.EndsWith("mod.json"))
            {
                string json = File.ReadAllText(fileName);

                if (json.IsValidJSON())
                {
                    JSONMod mod = JsonConvert.DeserializeObject<JSONMod>(json);
                    mod.directory = inDirectory;
                    loadedMods.Add(mod);
                    Debug.Log("Mod Loaded");
                }
            }
        }
    }
}
