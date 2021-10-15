using BasicMod.GameHooks;
using Books.GoalsBook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Bookmarks.BookmarkControllersGroupController;

namespace BasicMod.Factories
{

    public class PreRegisterTalentsEventArgs : EventArgs { };

    public static class TalentFactory
    {
        public static List<Talent> allTalents = new List<Talent>();
        public static TalentHaggle lastHaggleTalent;
        public static TalentTrading lastTradingTalent;

        public static EventHandler<PreRegisterTalentsEventArgs> onPreRegisterTalentsEvent;


        public static void Awake()
        {

            TalentsManagerAwakeEvent.OnTalentsManagerAwake += (sender, e) =>
            {
                GetLastTalents();
                var a = new PreRegisterTalentsEventArgs();
                onPreRegisterTalentsEvent?.Invoke(null, a);

                RegisterTalents();
                //LogTalents();
            };

        }


        private static void GetLastTalents()
        {
            foreach (Talent talent in Managers.Player.talents.allTalents)
            {
                if (talent.name.StartsWith("4-6")) lastHaggleTalent = (TalentHaggle)talent;
                if (talent.name.StartsWith("3-9")) lastTradingTalent = (TalentTrading)talent;
            }
            //Debug.Log(lastHaggleTalent);
            //Debug.Log(lastTradingTalent);

        }

        public static void AddTalent(Talent talent)
        {
            allTalents.Add(talent);
        }

      
        public static void RegisterTalents() {
         
            foreach (Talent talent in allTalents)
            {
                Managers.Player.talents.allTalents.Add(talent);
            }
        }


        public static void LogTalents()
        {
            Debug.Log("Beginning talent Log");
            foreach (Talent talent in Managers.Player.talents.allTalents)
            {
                Debug.Log(talent);


            }
        }

    }
}
