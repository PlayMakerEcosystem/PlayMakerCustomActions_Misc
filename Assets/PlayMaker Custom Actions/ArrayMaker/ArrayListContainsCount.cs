// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ARRAYMAKER__ __ACTION__ ---*/
// This Action requires ArrayMaker: https://hutonggames.fogbugz.com/default.asp?W715
// Source http://hutonggames.com/playmakerforum/index.php?topic=9998.msg47800#msg47800

//v2.0

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Count if an item is contained in a particular PlayMaker ArrayList Proxy component. Can be also be used to find duplicates in a 'dyi' manner")]
	public class ArrayListContainsCount : ArrayListActions
	{
		
		[ActionSection("Setup")]

		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;
		
		[ActionSection("Data")]
		[CompoundArray("Size", "Variable", "count")]
		[RequiredField]
		[Tooltip("The variable to check.")]
		public FsmVar[] variable;

		[UIHint(UIHint.FsmInt)]
		[Tooltip("Store the count value")]
		public FsmInt[] count;

		[ActionSection("Other info")]
		[UIHint(UIHint.FsmInt)]
		[Tooltip("Store the total count value")]
		public FsmInt totalArrayCount;
		[ActionSection("Event")]
		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails ( likely and index is out of range exception)")]
		public FsmEvent failureEvent;


		private int atIndex;


		public override void Reset()
		{
			gameObject = null;
			reference = null;
			variable = new FsmVar[1];
			count = new FsmInt[1];
			totalArrayCount = 0;
			atIndex = -1;

		}
		
		
		public override void OnEnter()
		{
			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				doesArrayListContainsCount();
			
			
			Finish();
		}
		
		
		public void doesArrayListContainsCount()
		{

			if (! isProxyValid() ) 
				return;

			int c = proxy.arrayList.Count;
			totalArrayCount.Value = c;
			FsmVar setType = variable[0];

			for(int a = 0; a<variable.Length;a++){

			count[a].Value = 0;
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


					switch (setType.Type) {
					case VariableType.Int:
						FsmInt fsmVarI = System.Convert.ToInt32(element);
						int tempInt = System.Convert.ToInt32(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a]));
						elementContained = fsmVarI.Value == tempInt;
						break;
						
					case VariableType.Float:
						FsmFloat fsmVarF = (float)element;
						float tempFloat = (float)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a])); 
						elementContained = fsmVarF.Value == tempFloat;
						break;
						
					case VariableType.Bool:
						FsmBool fsmVarB = (bool)element;
						bool tempBool = (bool)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a])); 
						elementContained = fsmVarB.Value == tempBool;
						break;
						
					case VariableType.Color:
						FsmColor fsmVarC = (Color)element;
						Color tempColor = (Color)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a])); 
						elementContained = fsmVarC.Value == tempColor;
						break;
						
					case VariableType.Quaternion:
						FsmQuaternion fsmVarQ = (Quaternion)element;
						Quaternion tempQuaternion = (Quaternion)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a])); 
						elementContained = fsmVarQ.Value == tempQuaternion;
						break;
						
					case VariableType.Rect:
						FsmRect fsmVarR = (Rect)element;
						Rect tempRect = (Rect)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a]));
						elementContained = fsmVarR.Value == tempRect;
						break;
						
					case VariableType.Vector2:
						FsmVector2 fsmVarV2 = (Vector2)element;
						Vector2 tempV2 = (Vector2)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a]));
						elementContained = fsmVarV2.Value == tempV2;
						break;
						
					case VariableType.Vector3:
						FsmVector3 fsmVarV3 = (Vector3)element;
						Vector3 tempV3 = (Vector3)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a]));
						elementContained = fsmVarV3.Value == tempV3;
						break;
						
					case VariableType.String:
						FsmString fsmVarString = (string)element;
						string tempString = (string)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a]));
						elementContained = fsmVarString.Value == tempString;
						break;

					case VariableType.GameObject:
						FsmGameObject fsmVarGameObject = (GameObject)element;
						GameObject tempGameObject = (GameObject)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a]));
						elementContained = fsmVarGameObject.Value == tempGameObject;
						break;
						
					case VariableType.Material:
						Material fsmVarMaterial = (Material)element;
						Material tempMaterial = (Material)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a]));
						elementContained = fsmVarMaterial == tempMaterial;
						break;
						
					case VariableType.Texture:
						Texture fsmVarTexture = (Texture)element;
						Texture tempTexture = (Texture)(PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a]));
						elementContained = fsmVarTexture == tempTexture;
						break;

					case VariableType.Unknown:
						Debug.Log ("ERROR");
						break;

					case VariableType.Object:
						var fsmVarUnknown = element;
						var tempUnknown = (PlayMakerUtils.GetValueFromFsmVar(this.Fsm,variable[a]));
						elementContained = fsmVarUnknown == tempUnknown;
						break;
						
					default:
						Debug.Log ("ERROR");
						break;
					}


					if (elementContained){
						count[a].Value++;
						}
			}

		}

			Finish();
		
		}




	
	}
		
		
}
