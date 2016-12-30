// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ARRAYMAKER__ __ACTION__ ---*/
// This Action requires ArrayMaker: https://hutonggames.fogbugz.com/default.asp?W715
// original action http://hutonggames.com/playmakerforum/index.php?topic=10013.msg32820

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Tests if all the given Array Bool Variables are equal to True or False.")]
	public class ArrayListMultiBoolTest : ArrayListActions
	{
		[ActionSection("Setup")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
		public FsmString reference;

		[ActionSection("Set Data")]
		[UIHint(UIHint.FsmBool)]
		[Tooltip("True or False criteria")]
		public FsmBool boolStates;

		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[ActionSection("Events")]
		[Tooltip("Send event if all True")]
		public FsmEvent trueEvent;
		[Tooltip("Send event if all False")]
		public FsmEvent falseEvent;
		[Tooltip("Send event if Error")]
		public FsmEvent errorEvent;

		private int c;

		
		public override void Reset()
		{

			gameObject = null;
			boolStates = null;
			trueEvent = null;
			falseEvent = null;
			errorEvent = null;
			storeResult = null;
		}
		
		public override void OnEnter()
		{
			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				CheckBoolArray();
		}


		public override void OnUpdate()
		{
			CheckBoolArray();
		}


		void CheckBoolArray()
		{

			if (! isProxyValid() ) 
				Fsm.Event (errorEvent);

			c = proxy.arrayList.Count;

			if (c <= 0) Error();


			int index = 0;
			bool elementContained;
			bool allTrue = true;
			
			for (int i = 0; i < c; i++)
			{

				elementContained = proxy.arrayList[index].Equals(boolStates.Value);

				if (!elementContained)
				{
					allTrue = false;
					storeResult.Value = allTrue;
					Fsm.Event (falseEvent);
				}
				index++;
			}
			
			storeResult.Value = allTrue;
				Fsm.Event(trueEvent);

		}


		void Error()
		{
			Fsm.Event (errorEvent);
		}
	}
}