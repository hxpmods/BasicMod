using BasicMod.Factories;
using BasicMod.JSON;
using BasicMod.ModObjects;
using MoreSalts;
using Newtonsoft.Json;
using Npc.Parts;
using Npc.Parts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace BasicMod
{
    public class JSONSaltMaker
    {
        public string name { get; set; }

        public string description { get; set; }

        public JSONColor pileColor { get; set; }
        public JSONColor particleColor { get; set; }

        public string boxBottomPath { get; set; }
        public string boxTopPath { get; set; }
        public string tooltipIconPath { get; set; }
        public string recipeMarkIconPath { get; set; }

        public SaltBehaviour behavior { get; set; }

        public ModSalt salt;

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


        public ModSalt CreateSalt()
        {
            salt = SaltFactory.CreateSalt(name);

            if (modSettings != null)
            {
                salt.customAssetsPath = modSettings.directory + "/Assets/";
            }


            salt.SetGraphicsPaths(boxBottomPath, boxTopPath, tooltipIconPath, recipeMarkIconPath);
            salt.SetDescription(description);
            salt.SetBehaviour(behavior);

            salt.particleBgColor = particleColor.getUnityColor();
            salt.pileBgColor= pileColor.getUnityColor();

            /*
            CardinalSaltBehaviour b = new CardinalSaltBehaviour(Vector2.one, 0.1f);
            string s = JsonConvert.SerializeObject(b, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            Debug.Log(s + " logged");
            */



            return salt;
        }

       

    }

}
