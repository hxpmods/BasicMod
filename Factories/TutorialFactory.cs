using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorialSystem;
using UnityEngine;

namespace BasicMod.Factories
{
    public class TutorialFactory
    {
        public static List<TutorialSet> allTutorials = new List<TutorialSet>();

        public static TutorialSet createTutorial(string _name)
        {
            var tut = ScriptableObject.CreateInstance<TutorialSet>();
            tut.name = _name;
            return tut;

        }

        public static void addTutorial(TutorialSet tut)
        {
            //Adds a salt to this SaltFactory
            allTutorials.Add(tut);
        }

        public static void setSteps(TutorialSet tut, TutorialStep[] tutorialSteps)
        {
            tut.steps = tutorialSteps;
        }

    }
}
