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
using TutorialSystem;
using UnityEngine;

namespace BasicMod.SaltUI
{
    public class SaltUI
    {

        public static ObjectManipulator.Enum[] objectManipButtonGroup = new ObjectManipulator.Enum[] { ObjectManipulator.Enum.RecipeBookButton,
                    ObjectManipulator.Enum.TalentsButton,
                    ObjectManipulator.Enum.LegendaryRecipesButton,
                    ObjectManipulator.Enum.GoalsButton,
                    ObjectManipulator.Enum.ManualButton,
                    ObjectManipulator.Enum.ShopUpgradesButton,
                    ObjectManipulator.Enum.CalendarButton};

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


        public static Vector2 InstantiateSprite(ScrollWindow SW, Vector2 localPosition, Sprite sprite, bool marginStart = true, bool marginEnd = true)
        {
            Vector2 result = (Vector2)Utility.Reflection.InvokePrivateMethod(SW, "InstantiateSprite", new object[] { localPosition, sprite, marginStart, marginEnd });
            return result;
        }

        public static Vector2 InstantiateDivider(ScrollWindow SW, Vector2 localPosition, bool marginStart = true, bool marginEnd = true)
        {
            Sprite sprite = Utility.Reflection.GetPrivateField<Sprite>(SW, "dividerSprite");
            Vector2 result = (Vector2)Utility.Reflection.InvokePrivateMethod(SW, "InstantiateSprite", new object[] { localPosition, sprite, marginStart, marginEnd });
            return result;
        }


        public static Vector2 InstantiateButton(ScrollWindow SW, Vector2 localPosition, bool marginStart = true, bool marginEnd = true, Vector2 offset = new Vector2())
        {
            Vector2 result = (Vector2)Utility.Reflection.InvokePrivateMethod(SW, "InstantiateButton", new object[] { localPosition + offset, GetButton(SW), marginStart, marginEnd });
            return result - offset;
        }

        public static Vector2 PreventOutOfTheScreenPosition(ScrollWindow SW, ScrollWindowContentController contentController)
        {
            Vector2 result = (Vector2)Utility.Reflection.InvokePrivateMethod(SW, "PreventOutOfTheScreenPosition", new object[] { contentController });
            return result;
        }

        public static GameObject GetButton(ScrollWindow SW)
        {
            GameObject button = Utility.Reflection.GetPrivateField<GameObject>(SW, "okButton");
            return button;
        }

        public static void SetObjectsLock(ObjectManipulator.Enum[] objects, bool locked)
        {
            if (objects == null || objects.Length == 0)
            {
                return;
            }
            foreach (ITutorialManipulated @object in ObjectManipulator.GetObjects(objects))
            {
                @object.LockedByTutorial = locked;
            }
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
                        CC.currentButtons.Add((hintSection as ButtonSection).button);
                    }

                }

                localPosition.y += ((hintSection != null) ? hintSection.margin.y : 0f) - hintPadding.y;

                Utility.Reflection.SetPrivateField<float>(__instance, "targetBottomRollYPosition", localPosition.y);

                Utility.Reflection.SetPrivateField<Vector2>(__instance, "targetPosition", SaltUI.PreventOutOfTheScreenPosition(__instance, CC));

                CC.OnHintPreparingEnd();



                //Locks game buttons
                if (parameters.lockGameButtons)
                {
                    SaltUI.SetObjectsLock(SaltUI.objectManipButtonGroup, true);
                }

                return false;

            }


            return true;

        }
    }

    [HarmonyPatch(typeof(ScrollWindow), "Close")]
    class ModScrollWindowClose{

        static void Postfix()
        {
            ScrollWindow scrollWindow = Managers.Game.scrollWindow;
            ModScrollWindowContentController CC = scrollWindow.currentContentController as ModScrollWindowContentController;
            if (CC != null)
            {
                var parameters = CC.parameters as ModHintParameters;
                //Unlocks game buttons
                if (parameters.lockGameButtons)
                {
                    SaltUI.SetObjectsLock(SaltUI.objectManipButtonGroup, false);
                }
            }
        }
    }
}
