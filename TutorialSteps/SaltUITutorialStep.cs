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
    public class SaltUITutorialStep :  TutorialStep
    {
		public ModHintParameters hintParameters;

		protected override void OnStepStart()
		{
			Debug.Log("We are here?");
			ScrollWindow.Open(new ModScrollWindowContentController(hintParameters));
		}
		public override bool DoneCondition(object input = null)
		{
			return false;
		}

	}
}
