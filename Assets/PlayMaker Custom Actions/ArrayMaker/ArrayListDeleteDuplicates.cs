// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/
// This Action requires ArrayMaker: https://hutonggames.fogbugz.com/default.asp?W715
// original action http://hutonggames.com/playmakerforum/index.php?topic=10190.msg48178#msg48178

//V2.0

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("ArrayMaker/ArrayList")]
	[Tooltip("Delete duplicate array")]
	public class ArrayListDeleteDuplicates : ArrayListActions
	{
		
		[ActionSection("Array Setup")]

		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;

		[ActionSection("Data Setup")]
		[UIHint(UIHint.FsmInt)]
		[Tooltip("Sort array before action")]
		[TitleAttribute("Sort array first")]
		public bool sortOn;
		[Tooltip("Replace duplicates with null value - Array length remains the same")]
		[TitleAttribute("Set value to null")]
		public bool nullOn;
		[ActionSection("Event")]
		[UIHint(UIHint.FsmEvent)]
		[Tooltip("The event to trigger if the action fails (likely and index is out of range exception)")]
		public FsmEvent failureEvent;


		private int atIndex;
		private int atIndex_iplus;
		private int countRef;
		private int c;
		private bool restartbool;
		FsmVar objType;

		public override void Reset()
		{
			gameObject = null;
			reference = null;
			sortOn = true;
			nullOn = false;
			atIndex = -1;
			failureEvent = null;
			atIndex_iplus = 0;
			countRef = 0;
			c = 0;
			restartbool = false;
		}
		
		
		public override void OnEnter()
		{
			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				doesArrayListContainsCount();
			
			
			Finish();
		}
		
		
		public void doesArrayListContainsCount()
		{

			if (! isProxyValid() ) {
				return;
			}

			c = proxy.arrayList.Count;
			countRef =0;

			if (c < 1) {
				Debug.Log("Array is = 1 or below. Problem ? ");
				Fsm.Event(failureEvent);
			}


			if (sortOn & !nullOn){
				try{
				proxy.arrayList.Sort();
				}catch(System.Exception e){
					Debug.Log(e.Message);
					Debug.LogWarning("!!!!! Can't Sort this type in Array - Please untick option - Did not delete Duplicates *** Nothing happened");
					}
				SortOnDelete();
			}

			else {

				ArrayDelete();
			}
		}

			public void SortOnDelete(){
			object elementTemp = proxy.arrayList[0];
			var varTemp = proxy.preFillType.ToString();
	
RESTART:
				atIndex = -1;
				countRef++;

				for(int i = 0; i<c;i++){

		
					atIndex++;
					atIndex_iplus = atIndex+1;

					if (atIndex_iplus ==c){
						break;
					}


	

				bool elementContained = false;
					object element = null;
					object element_b = null;
					
					try{
						element = proxy.arrayList[atIndex];
						element_b = proxy.arrayList[atIndex_iplus];
					}catch(System.Exception e){
						Debug.Log(e.Message);
						string fullLabel = Fsm.GetFullFsmLabel(this.Fsm);
						string name = Fsm.ActiveStateName;
						Debug.Log("Fsm Path= "+fullLabel+" : "+name);
						Fsm.Event(failureEvent);
						return;
					}

				switch (varTemp) {
				case "Int":
					FsmInt fsmVarI = System.Convert.ToInt32(element);
					int tempInt = System.Convert.ToInt32(element_b);
					elementContained = fsmVarI.Value == tempInt;
					break;
		
					
				case "Float":
					FsmFloat fsmVarF = (float)element;
					float tempFloat = (float)(element_b); 
					elementContained = fsmVarF.Value == tempFloat;
					break;
					
				case "Bool":
					FsmBool fsmVarB = (bool)element;
					bool tempBool = (bool)(element_b); 
					elementContained = fsmVarB.Value == tempBool;
					break;
					
				case "Color":
					FsmColor fsmVarC = (Color)element;
					Color tempColor = (Color)(element_b); 
					elementContained = fsmVarC.Value == tempColor;
					break;
					
				case "Quaternion":
					FsmQuaternion fsmVarQ = (Quaternion)element;
					Quaternion tempQuaternion = (Quaternion)(element_b); 
					elementContained = fsmVarQ.Value == tempQuaternion;
					break;
					
				case "Rect":
					FsmRect fsmVarR = (Rect)element;
					Rect tempRect = (Rect)(element_b);
					elementContained = fsmVarR.Value == tempRect;
					break;
					
				case "Vector2":
					FsmVector2 fsmVarV2 = (Vector2)element;
					Vector2 tempV2 = (Vector2)(element_b);
					elementContained = fsmVarV2.Value == tempV2;
					break;
					
				case "Vector3":
					FsmVector3 fsmVarV3 = (Vector3)element;
					Vector3 tempV3 = (Vector3)(element_b);
					elementContained = fsmVarV3.Value == tempV3;
					break;
					
				case "String":
					FsmString fsmVarString = (string)element;
					string tempString = (string)(element_b);
					elementContained = fsmVarString.Value == tempString;
					break;
					
				case "GameObject":
					FsmGameObject fsmVarGameObject = (GameObject)element;
					GameObject tempGameObject = (GameObject)(element_b);
					elementContained = fsmVarGameObject.Value == tempGameObject;
					break;
					
				case "Material":
					Material fsmVarMaterial = (Material)element;
					Material tempMaterial = (Material)(element_b);
					elementContained = fsmVarMaterial == tempMaterial;
					break;
					
				case "Texture":
					Texture fsmVarTexture = (Texture)element;
					Texture tempTexture = (Texture)(element_b);
					elementContained = fsmVarTexture == tempTexture;
					break;
					
				case "AudioClip":
					var fsmVarUnknown = element;
					var tempUnknown = element_b;
					elementContained = fsmVarUnknown == tempUnknown;
					break;
					
				default:
					Debug.Log ("ERROR");
					break;
				}


				if (elementContained){

				c=c-1;

				proxy.arrayList.RemoveAt(atIndex_iplus);
					
				goto RESTART;

			}
				
			}

			Finish();	
	}


		public void ArrayDelete(){

			if (sortOn & nullOn){
				proxy.arrayList.Sort();
			}

			object elementTemp = proxy.arrayList[0];
			var varTemp = proxy.preFillType.ToString();


			atIndex = -1;
			atIndex_iplus=0;


			for(int a = 0; a<c;a++){

				atIndex++;
				atIndex_iplus = atIndex;

				for(int i = 0; i<c;i++){
IGNORE:				
					if (restartbool){
						atIndex_iplus = atIndex;
						restartbool = false;
					}
				
				
				atIndex_iplus++;

		
					if (atIndex_iplus == c){
						break;
					}

				bool elementContained = false;
				object element = null;
				object element_b = null;
				
				try{
					element = proxy.arrayList[atIndex];
					element_b = proxy.arrayList[atIndex_iplus];
				}catch(System.Exception e){
					Debug.Log(e.Message);
						string fullLabel = Fsm.GetFullFsmLabel(this.Fsm);
						string name = Fsm.ActiveStateName;
					Debug.Log("Fsm Path= "+fullLabel+" : "+name);
					Fsm.Event(failureEvent);
					return;
				}
				
					if (element == null){
						break;
					}

					if (element_b == null){
						goto IGNORE;
					}

					switch (varTemp) {
					case "Int":
						FsmInt fsmVarI = System.Convert.ToInt32(element);
						int tempInt = System.Convert.ToInt32(element_b);
						elementContained = fsmVarI.Value == tempInt;
						break;
						
						
					case "Float":
						FsmFloat fsmVarF = (float)element;
						float tempFloat = (float)(element_b); 
						elementContained = fsmVarF.Value == tempFloat;
						break;
						
					case "Bool":
						FsmBool fsmVarB = (bool)element;
						bool tempBool = (bool)(element_b); 
						elementContained = fsmVarB.Value == tempBool;
						break;
						
					case "Color":
						FsmColor fsmVarC = (Color)element;
						Color tempColor = (Color)(element_b); 
						elementContained = fsmVarC.Value == tempColor;
						break;
						
					case "Quaternion":
						FsmQuaternion fsmVarQ = (Quaternion)element;
						Quaternion tempQuaternion = (Quaternion)(element_b); 
						elementContained = fsmVarQ.Value == tempQuaternion;
						break;
						
					case "Rect":
						FsmRect fsmVarR = (Rect)element;
						Rect tempRect = (Rect)(element_b);
						elementContained = fsmVarR.Value == tempRect;
						break;
						
					case "Vector2":
						FsmVector2 fsmVarV2 = (Vector2)element;
						Vector2 tempV2 = (Vector2)(element_b);
						elementContained = fsmVarV2.Value == tempV2;
						break;
						
					case "Vector3":
						FsmVector3 fsmVarV3 = (Vector3)element;
						Vector3 tempV3 = (Vector3)(element_b);
						elementContained = fsmVarV3.Value == tempV3;
						break;
						
					case "String":
						FsmString fsmVarString = (string)element;
						string tempString = (string)(element_b);
						elementContained = fsmVarString.Value == tempString;
						break;
						
					case "GameObject":
						FsmGameObject fsmVarGameObject = (GameObject)element;
						GameObject tempGameObject = (GameObject)(element_b);
						elementContained = fsmVarGameObject.Value == tempGameObject;
						break;
						
					case "Material":
						Material fsmVarMaterial = (Material)element;
						Material tempMaterial = (Material)(element_b);
						elementContained = fsmVarMaterial == tempMaterial;
						break;
						
					case "Texture":
						Texture fsmVarTexture = (Texture)element;
						Texture tempTexture = (Texture)(element_b);
						elementContained = fsmVarTexture == tempTexture;
						break;
						
					case "AudioClip":
						var fsmVarUnknown = element;
						var tempUnknown = element_b;
						elementContained = fsmVarUnknown == tempUnknown;
						break;
						
					default:
						Debug.Log ("ERROR");
						break;
					}

					if (elementContained){
						
						if (!nullOn){
							proxy.arrayList.RemoveAt(atIndex_iplus);
						}
						
						else {
							int indexOfResult = PlayMakerUtils_Extensions.IndexOf(proxy.arrayList,element_b);
							element_b = PlayMakerUtils.GetValueFromFsmVar(this.Fsm,null);
							proxy.arrayList[atIndex_iplus] = element_b;
						}
							i = 0;
							c = proxy.arrayList.Count;
							restartbool = true;

					}

			}
			
		}
			Finish();	
}



	}
}