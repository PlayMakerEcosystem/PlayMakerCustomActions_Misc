// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ARRAYMAKER__ __ACTION__ ---*/
// This Action requires ArrayMaker: https://hutonggames.fogbugz.com/default.asp?W715
// original action http://hutonggames.com/playmakerforum/index.php?topic=10190.msg48178#msg48178



using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Set an item at the next empty (Null) position in a PlayMaker array List component")]
	public class ArrayListSetNextEmpty : ArrayListActions
	{
		
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;
		
		public bool everyFrame;
		
		[ActionSection("Data")]
		
		[Tooltip("The variable to add.")]
		public FsmVar variable;

		[ActionSection("Store Index")]
		[UIHint(UIHint.FsmInt)]
		[Tooltip("The index it was added at")]
		public FsmInt indexResult;

		[ActionSection("Event")]
		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails (likely and index is out of range exception).")]
		public FsmEvent failureEvent;

		[UIHint(UIHint.FsmEvent)]
		[Tooltip("No empty space in array list.")]
		[TitleAttribute("None Empty")]
		public FsmEvent noEmptyEvent;

		
		private int c;
		private int atIndex;
		private bool nothing;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			variable = null;
			everyFrame = false;
			indexResult = 0;
			failureEvent = null;
			noEmptyEvent = null;
			c = 0;
			atIndex = 0;
			nothing = false;
		}
		
		
		public override void OnEnter()
		{
			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				SetToArrayList();
			
			if (!everyFrame){
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			SetToArrayList();
		}
		
		public void SetToArrayList()
		{

			if (! isProxyValid() ) 
				return;
			
			int c = proxy.arrayList.Count;
			nothing = true;

			if (c < 1) {
				Debug.Log("Array is = 1 or below. Problem ? ");
				Fsm.Event(failureEvent);
			}
			
				atIndex = -1;
				
				
				for(int i = 0; i<c;i++){
					
					
					atIndex++;
					
					bool elementContained = false;
					object element = null;
					
					try{
						element = proxy.arrayList[atIndex];
					}catch(System.Exception e){
						Debug.Log(e.Message);
						Fsm.Event(failureEvent);
						return;
					}

				elementContained = element == null;
					
					
					if (elementContained){
						proxy.Set(atIndex,PlayMakerUtils.GetValueFromFsmVar(Fsm,variable),variable.Type.ToString());
						indexResult.Value = i;
						nothing = false;
						break;
					}
				}
				
			if (nothing) Fsm.Event(noEmptyEvent);
			Finish();

		}
		
		
	}
}