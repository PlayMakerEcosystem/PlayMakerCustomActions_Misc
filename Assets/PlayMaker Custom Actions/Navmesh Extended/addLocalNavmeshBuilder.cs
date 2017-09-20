// (c) Copyright HutongGames, LLC 2010-2017. All rights reserved.
// License: Attribution 4.0 International (CC BY 4.0)
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Navmesh Extended")]
	[Tooltip("Add a local navmesh builder component to a gameobject for local realtime navmesh updates.")]
	public class addLocalNavmeshBuilder : FsmStateAction

	{
		[RequiredField]
		[Tooltip("The GameObject to add the Navmesh Builder to.")]
		[Title("Gameobject")]  
		public FsmOwnerDefault gameObject;
		
		[ActionSection("Optional")]
		
		[Title("Builder Size")]  
		[Tooltip("The size of the build bounds in vector3.")]
		public FsmVector3 size;
		
		[Title("Tracked Gameobject")]  
		[Tooltip("Set the center of the builder.")]
		public FsmGameObject trackedObject;
		

		public override void Reset()
		{
			
			gameObject = null;
			size = new FsmVector3 (){UseVariable=true};
			trackedObject = new FsmGameObject (){UseVariable=true};
		}

		public override void OnEnter()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);

			doBuilder();
			Finish();

		}

		void doBuilder()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				return;
			}
			
			LocalNavMeshBuilder mesh = go.AddComponent(typeof(LocalNavMeshBuilder)) as LocalNavMeshBuilder;
			
			if (!size.IsNone)
			{
				mesh.m_Size = size.Value;
			}
			
			if (!trackedObject.IsNone)
			{
				mesh.m_Tracked = trackedObject.Value.transform;
			}
			
		}
	}
}
