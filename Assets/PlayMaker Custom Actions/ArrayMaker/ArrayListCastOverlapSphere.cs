// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// Keywords: overlap sphere cast physics

using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("ArrayMaker/ArrayList")]
    [Tooltip("Cast an overlap sphere and get the objects(by collider) that it hits. Note: This action returns the game objects that have colliders attached. If a hit game object has multiple colliders it will return copies of the game object.")]
	public class ArrayListCastOverlapSphere : ArrayListActions
    {

        [ActionSection("Overlap Sphere Settings")]

        [Tooltip("The gameObject to cast the Overlap sphere from.")]
        public FsmOwnerDefault scanOrigin;

        [Tooltip("The size of the overlap sphere.")]
        public FsmFloat scanRange;

        [ActionSection("ArrayList Info")]

        [RequiredField]
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
        [CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault arrayListOwner;

        [Tooltip("The name of the arrayList you want to store the hit objects in.")]
        public FsmString arrayListReference;

        [ActionSection("Filter")]

        [UIHint(UIHint.Layer)]
        [Tooltip("Pick only from these layers.")]
        public FsmInt[] layerMask;

        [Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
        public FsmBool invertMask;


        [Tooltip("Set to true to ignore colliders set to trigger.")]
        public FsmBool ignoreTriggerColliders;

        public FsmEvent ErrorEvent;

        PlayMakerArrayListProxy colliders;



        public override void Reset()
        {
            arrayListOwner = null;
            arrayListReference = null;

            ErrorEvent = null;
            scanRange = null;
            layerMask = new FsmInt[0];
            invertMask = false;
            ignoreTriggerColliders = false;
        }


        public override void OnEnter()
        {

            if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(arrayListOwner), arrayListReference.Value))
                FindObjectInRange();

            Finish();
        }

        void FindObjectInRange()
        {
            if (!isProxyValid())
            {
                return;
            }

            GameObject go = Fsm.GetOwnerDefaultTarget(scanOrigin);
            GameObject listOwner = Fsm.GetOwnerDefaultTarget(arrayListOwner);

            float range = scanRange.Value;

            if (ignoreTriggerColliders.Value == true)
            {
                Collider[] colliders = Physics.OverlapSphere(go.transform.position, range, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value), QueryTriggerInteraction.Ignore);
                if (colliders.Length == 0)
                {
                    Fsm.Event(ErrorEvent);
                }


                foreach (Collider col in colliders)
                {
                    proxy.Add(col.gameObject, "gameObject");
                }
            } else
            {
                Collider[] colliders = Physics.OverlapSphere(go.transform.position, range, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value), QueryTriggerInteraction.Collide);
                if (colliders.Length == 0)
                {
                    Fsm.Event(ErrorEvent);
                }


                foreach (Collider col in colliders)
                {
                    proxy.Add(col.gameObject, "gameObject");
                }
            }

           // Collider[] colliders = Physics.OverlapSphere(go.transform.position, range, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value),QueryTriggerInteraction.Ignore);


            Finish();
        }
    }
}