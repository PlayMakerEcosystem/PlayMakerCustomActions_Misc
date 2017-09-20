// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.
// License: Attribution 4.0 International (CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Navmesh Extended")]
	[Tooltip("Add a nav mesh source tag component to game object to be recognized for realtime nav mesh updates.")]

	public class addNavmeshSourceTag : FsmStateAction

	{
		[RequiredField]
		[Tooltip("The GameObject to add the Navmesh Source Tag to.")]
		[Title("Gameobject")]  
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			
			gameObject = null;

		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			addTag();
			Finish();

		}

		void addTag()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			
			NavMeshSourceTag mesh = go.AddComponent(typeof(NavMeshSourceTag)) as NavMeshSourceTag;

		}
	}
}
