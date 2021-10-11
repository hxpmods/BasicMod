using ScrollWindowHint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.SaltUI.WindowElements
{
    public class OptionWithButtonWindowElement : AbstractWindowElement
    {
        private static Vector2 baseTextOffset = new Vector2(0.2f, -0.15f/*-0.34f*/);
        private static Vector2 baseButtonOffset = new Vector2(2.5f, 0.0f);

        public string text;
        public Vector2 textOffset = baseTextOffset;
        public Vector2 buttonOffset = baseButtonOffset;



        public override Vector2 AddToWindow(ScrollWindow instance, Vector2 localPosition, HintSection hintSection)
        {
            Reset();
            AutoMultiLine();
            //If anyone knows how to manipulate the textbox rect so we can it to wrap that would be swell c:
            SaltUI.InstantiateText(instance, localPosition, text, hintSection != null, offset: textOffset);           
            return (Vector2)SaltUI.InstantiateButton(instance, localPosition, false, false, offset: buttonOffset);

        }

        public void Reset()
        {
            textOffset = baseTextOffset;
            buttonOffset = baseButtonOffset;
        }

        public void AutoMultiLine()
        {
            string real_text = LocalDict.GetTextFromKey(text);
            if (real_text.Contains("\n"))
            {
                textOffset += new Vector2(0f, 0.2f);
            }
        }
    }
}
