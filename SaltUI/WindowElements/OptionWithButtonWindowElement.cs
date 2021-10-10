using ScrollWindowHint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.SaltUI.WindowElements
{
    class OptionWithButtonWindowElement : AbstractWindowElement
    {
        public string text;
        public Vector2 textOffset = new Vector2(0.2f, -0.15f/*-0.34f*/);
        public Vector2 buttonOffset = new Vector2(2.5f, 0.0f);
        public override Vector2 AddToWindow(ScrollWindow instance, Vector2 localPosition, HintSection hintSection)
        {
            //If anyone knows how to manipulate the textbox so we can it to wrap that would be swell c:
            SaltUI.InstantiateText(instance, localPosition, text, hintSection != null, offset: textOffset);           
            return (Vector2)SaltUI.InstantiateButton(instance, localPosition, false, false, offset: buttonOffset);

        }
    }
}
