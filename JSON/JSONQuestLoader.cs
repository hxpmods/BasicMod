using BasicMod.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.JSON
{


    public static class JSONQuestLoader
    {
        public static List<ModQuestMaker> loadedQuests = new List<ModQuestMaker>();
        public static void Awake()
        {
            ProcessDirectory("BepinEx/Plugins/JSON/Quests");
            QuestFactory.onQuestsPreGenerate += (_, e) =>
            {
                if (loadedQuests.Count > 0)
                {
                    QuestFactory.allQuests.AddRange(loadedQuests);
                }
            };
            

        }

        public static bool IsValidJSON(this string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException ex)
            {
                Debug.Log("Error: " + ex.Message);
                return false;
            }
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static void ProcessFile(string path)
        {
            try
            {
                string json = File.ReadAllText(path);

                if (json.IsValidJSON())
                {
                    ModQuestMaker quest = deserialiseJSONQuests(json);
                    if (quest != null) loadedQuests.Add(quest);
                }
                else
                {
                    Debug.Log(path + " is not a valid JSON file.");
                }


            }
            catch (Exception ex)
            {
                Debug.Log("Error: " + ex.Message);
            }

        }


     

        public static ModQuestMaker deserialiseJSONQuests(string json)
        {
            try
            {
                ModQuestMaker j = JsonConvert.DeserializeObject<ModQuestMaker>(json);
                return j;
            }
            catch (Exception ex)
            {
                Debug.Log("Error: " + ex.Message);
                return null;
            }

        }
    }


    


}
