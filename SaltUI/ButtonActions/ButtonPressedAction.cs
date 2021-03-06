using ScrollWindowHint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BasicMod.SaltUI
{
    public class ButtonPressedAction
    {
        public bool closeWindow = true;
        public ScrollWindow window;

        public ButtonPressedAction(ScrollWindow window)
        {
            this.window = window;
        }

        public virtual void Press( int buttonId)
        {
            if (closeWindow)
            {
                window.isOkButtonClicked = true;
                ScrollWindow.Close();
            }
        }
    }
}
