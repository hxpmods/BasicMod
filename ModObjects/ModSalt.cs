using ObjectBased.InteractiveItem.InventoryObject;
using ObjectBased.UIElements.Tooltip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace BasicMod
{
    public class ModSalt : Salt
    {
        public string locName;
        public SaltBehaviour saltBehaviour;
        public SaltVisualObject sharedVisualObject;
        public string boxBottomPath= "";
        public string boxTopPath ="";
        public string tooltipIconPath = "";
        public string recipeMarkIconPath ="";
        public string description;

        public string customAssetsPath;

        public override string LocalizationKeyName => locName;

        public ModSalt SetGraphicsPaths(string _boxBottomPath = "", string _boxTopPath = "", string _ttIconPath ="", string _recipeIconPath = "")
        {

            boxBottomPath = _boxBottomPath;
            boxTopPath = _boxTopPath;
            tooltipIconPath = _ttIconPath;
            recipeMarkIconPath = _recipeIconPath;
            return this;
        }


        public ModSalt SetDescription(string _desc)
        {
            description = _desc;
            return this;
        }

        public ModSalt SetBehaviour(SaltBehaviour b)
        {
            saltBehaviour = b;
            return this;
        }

        public new void Initialize()
        {
            locName = name;

            sharedVisualObject = prefab.GetComponentInChildren<SaltVisualObject>();           
            var topsprite = sharedVisualObject.GetComponentInChildren<SpriteRenderer>();

            CheckIfDefaultGraphics();



            sharedVisualObject.boxRenderer.sprite = SpriteLoader.LoadSpriteFromFile(boxBottomPath,customAssetsPath);
            topsprite.sprite = SpriteLoader.LoadSpriteFromFile(boxTopPath,customAssetsPath);
            recipeMarkIcon = SpriteLoader.LoadSpriteFromFile(recipeMarkIconPath, customAssetsPath);
            tooltipIcon = SpriteLoader.LoadSpriteFromFile(tooltipIconPath, customAssetsPath);


            
        }

        public void CheckIfDefaultGraphics()
        {

            if (boxBottomPath == "" || boxBottomPath == null)
            {
                boxBottomPath = "@Default Salt Box Bottom.png";
            }
            if (boxTopPath == "" || boxTopPath == null)
            {
                boxTopPath = "@Default Salt Box Top.png";
            }
            if (recipeMarkIconPath == "" || recipeMarkIconPath == null)
            {
                recipeMarkIconPath = "@Default Salt Recipe Mark.png";
            }
            if (tooltipIconPath == "" || tooltipIconPath == null)
            {
               tooltipIconPath = "@Default Salt Tool Tip.png";
            }
        }
        

        public override void SetUpInventoryObjectIconFor(InventoryObject inventoryObject)
        {


            Transform transform = inventoryObject.iconRenderer.transform;
            GameObject gameObject = UnityEngine.Object.Instantiate(sharedVisualObject.gameObject, transform, worldPositionStays: false);
            SortingGroup component = gameObject.GetComponent<SortingGroup>();
            component.sortingLayerID = inventoryObject.iconRenderer.sortingLayerID;
            component.sortingOrder = inventoryObject.iconRenderer.sortingOrder;
            transform.localScale = Vector3.one * sharedVisualObject.inventoryIconScale;
            gameObject.GetComponent<SaltVisualObject>().SetMaskInteractionLikeIcon(inventoryObject.iconRenderer.maskInteraction);
            transform.localPosition = sharedVisualObject.inventoryIconOffset;
        }

        public override TooltipContent GetTooltipContent(int itemCount, bool anyModifierHeld = false)
        {
            return new TooltipContent
            {
                header = name + ((itemCount == 1 || !anyModifierHeld) ? "" : $"\u00a0<color=#a1743f>({itemCount})</color>"),
                spriteAboveIngredients = tooltipIcon,
                description1 = description
            };
        }



        public override void OnCauldronDissolve()
        {
            base.OnCauldronDissolve();
            if (saltBehaviour != null)
            {
                saltBehaviour.OnCauldronDissolve();
            }
        }

    }
}
