// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;
using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Convert an int to an arbitrary letter, defined by the user or else, taken from the western alphabet, case insensitive")]
	public class ConvertIntToLetter : FsmStateAction
	{
		[Tooltip("The index to get the associated letter")]
		public FsmInt index;

		[Tooltip("If true, index 0 means the first Letter, if false, index 1 means the first Letter")]
		public bool zeroBasedIndex;

		[Tooltip("Leave to none to use the western alphabet, abcdefgh..")]
		public FsmString letters;

		[ActionSection("Result")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString letter;

		public FsmEvent indexOutOfRange;

		public bool everyFrame;

		const string _defaultLetters = "abcdefghijklmnopqrstuvwxyz";

		public override void Reset()
		{
			index = null;
			zeroBasedIndex = false;
			letters =  new FsmString() {UseVariable=true} ;
			letter = null;
			indexOutOfRange = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoGetLetter();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoGetLetter();
		}
		
		void DoGetLetter()
		{
			int _index = zeroBasedIndex?index.Value:index.Value-1;

			if (letters.IsNone)
			{
				if (_index>=_defaultLetters.Length || _index<0)
				{
					Fsm.Event(indexOutOfRange);
				}else{
					letter.Value = _defaultLetters[_index].ToString();
				}
			}else{
				if (_index>=letters.Value.Length || _index<0)
				{
					Fsm.Event(indexOutOfRange);
				}else{
					letter.Value = letters.Value[_index].ToString();
				}
			}
		}
	}
}