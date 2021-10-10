using BasicMod.SaltUI.WindowElements;
using Extensions;
using HarmonyLib;
using LocalizationSystem;
using ScrollWindowHint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.SaltUI
{
    public class SaltUI
    {

        public static Vector2 InstantiateText(ScrollWindow SW, Vector2 localPosition, string textKey, bool marginStart = true, bool marginEnd = true, Vector2 offset = new Vector2())
        {
            Vector2 result = (Vector2)Utility.Reflection.InvokePrivateMethod(SW, "InstantiateText", new object[] { localPosition + offset, textKey, marginStart, marginEnd });
            return result - offset;
        }

        public static Vector2 InstantiateTitle(ScrollWindow SW, Vector2 localPosition, Key titleKey, bool marginStart = true, bool marginEnd = true)
        {
            Vector2 result = (Vector2)Utility.Reflection.InvokePrivateMethod(SW, "InstantiateTitle", new object[] { localPosition, titleKey, marginStart, marginEnd });
            return result;
        }

        public static Vector2 InstantiateButton(ScrollWindow SW, Vector2 localPosition, bool marginStart = true, bool marginEnd = true, Vector2 offset = new Vector2())
        {
            Vector2 result = (Vector2)Utility.Reflection.InvokePrivateMethod(SW, "InstantiateButton", new object[] { localPosition + offset, GetButton(SW), marginStart, marginEnd });
            return result - offset;
        }

        public static Vector2 PreventOutOfTheScreenPosition(ScrollWindow SW, ScrollWindowContentController contentController)
        {
            Vector2 result = (Vector2)Utility.Reflection.InvokePrivateMethod(SW, "PreventOutOfTheScreenPosition", new object[] {contentController });
            return result;
        }

        public static GameObject GetButton(ScrollWindow SW)
        {
            GameObject button = Utility.Reflection.GetPrivateField<GameObject>(SW, "okButton");
            return button;
        }

    }

    [HarmonyPatch(typeof(ScrollWindow))]
    [HarmonyPatch("UpdateHintContentBeforeShowing")]
    class ModScrollWindowPatch
    {
        static bool Prefix(ScrollWindow __instance, ScrollWindowContentController scrollWindowContentController)
        {
            var CC = scrollWindowContentController as ModScrollWindowContentController;

            if (CC != null)

            {
               
                __instance.currentContentController = CC;

                Utility.Reflection.InvokePrivateMethod(__instance, "UpdateLayers");
                Transform sectionsContainer = Utility.Reflection.GetPrivateField<Transform>(__instance, "sectionsContainer");
                foreach (Transform item in sectionsContainer)
                {
                    UnityEngine.Object.Destroy(item.gameObject);
                }

                List<HintSection> enabledSections = Utility.Reflection.GetPrivateField<List<HintSection>>(__instance, "enabledSections");
                enabledSections.Clear();

                Vector2 hintPadding = Utility.Reflection.GetPrivateField<Vector2>(__instance, "hintPadding");
                Vector2 localPosition = hintPadding.x * Vector2.down;
                HintSection hintSection = null;

                localPosition.y += ((hintSection != null) ? hintSection.margin.y : 0f) - hintPadding.y;


                //Set title
                
                if (!CC.parameters.titleKey.Equals(string.Empty))
                {
                    Key titleKey = new Key(CC.parameters.titleKey, CC.parameters.titleParameters);
                    localPosition = SaltUI.InstantiateTitle(__instance, localPosition, titleKey, false);
                    hintSection = enabledSections.Last();
                }

                ModHintParameters parameters = CC.parameters as ModHintParameters;
                List<AbstractWindowElement> windowElements = parameters.scrollWindowElements;

                foreach (AbstractWindowElement element in windowElements)
                {
                    localPosition = element.AddToWindow(__instance, localPosition, hintSection);
                    hintSection = enabledSections.Last();

                    if (hintSection is ButtonSection)
                    {
                        Debug.Log("We made it");
                        CC.currentButtons.Add((hintSection as ButtonSection).button);
                    }

                }
                
                localPosition.y += ((hintSection != null) ? hintSection.margin.y : 0f) - hintPadding.y;

                Utility.Reflection.SetPrivateField<float>(__instance, "targetBottomRollYPosition", localPosition.y);
      
                Utility.Reflection.SetPrivateField<Vector2>(__instance, "targetPosition",  SaltUI.PreventOutOfTheScreenPosition(__instance, CC));

                CC.OnHintPreparingEnd();
                return false;

           }


            return true;

        }
    }

    [HarmonyPatch(typeof(GameManager), "StartNewGame")]
    public static class NewGameEvent
    {
        static bool Prefix()
        {
            Managers.Menu.CloseMenu();
            Managers.SaveLoad.LoadNewGameState();
            Managers.SaveLoad.SelectedProgressState = null;




            //Ignore the tutorial and add our window
            //These windows should work in tutorials as is, but also work with ScrollWindow.Open
            ModHintParameters param = new ModHintParameters();

            

            param.anchorPosition = new Vector2(0, 5);
            param.titleKey = "basicmodwelcome";
            LocalDict.AddKeyToDictionary("basicmodwelcome", "Greetings, Alchemist.");

            var text1 = new TextWindowElement();
            text1.text = "basicmoddesc1";
            LocalDict.AddKeyToDictionary("basicmoddesc1", "Welcome back.\n Are you ready to settle into your next life?");

            var text2 = new TextWindowElement();
            text2.text = "basicmoddesc2";
            LocalDict.AddKeyToDictionary("basicmoddesc2", "");

            var button1 = new OptionWithButtonWindowElement();
            button1.text = "This is an option.";
            var button2 = new OptionWithButtonWindowElement();
            button2.text = "This is another option.";

            param.scrollWindowElements.Add(text1);
            param.scrollWindowElements.Add(text2);
            param.scrollWindowElements.Add(button1);
            param.scrollWindowElements.Add(button2);
            param.scrollWindowElements.Add(button1);
            param.scrollWindowElements.Add(button2);
            param.buttonPressedAction = new ButtonPressedAction(Managers.Game.scrollWindow);

            ScrollWindow.Open(new ModScrollWindowContentController(param));
            
            return false;
        }
    }
}