// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
/*--- __ECO__ __PLAYMAKER__ __ACTION__ ---*/
// This Action requires ArrayMaker: https://hutonggames.fogbugz.com/default.asp?W715
// original action http://hutonggames.com/playmakerforum/index.php?topic=10086.msg47735#msg47735


using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Particles")]
	[Tooltip("Remove (and stop) all particles in the particle system.")]
	public class particleSystemClear : FsmStateAction
	{

		[RequiredField]
		[Tooltip("The particle system component owner.")]
		[CheckForComponent(typeof(ParticleSystem))]
		public FsmOwnerDefault gameObject;
		[Tooltip("Remove all particles in the particle system (has to be off or stop). Set to True to turn it on and False to turn it off.")]
		public FsmBool clear;
		[Tooltip("Stop particle sytem + Remove all particles in the particle system. Set to True to turn it on and False to turn it off.")]
		public FsmBool stopWithClear;
		[Tooltip("If 0 it just acts like a switch. Values cause it to Toggle value after delay time (sec).")]
		public FsmFloat delay;
		public FsmEvent finishEvent;
		public bool realTime;
		
		private float startTime;
		private float timer;
		private ParticleSystem ps;
		private GameObject go;
		
		public override void Reset()
		{
			gameObject = null;
			clear = false;
			stopWithClear = false;
			delay = 0f;
			finishEvent = null;
			realTime = false;
			ps = null;
		}
		
		public override void OnEnter()
		{
			ps = Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<ParticleSystem>();


			if (delay.Value <= 0)
			{
				if (clear.Value) {
					stopWithClear.Value = false;
					ps.Clear();
				}

				else { 
					ps.Stop();
					ps.Clear();

				}

				if(finishEvent != null) Fsm.Event(finishEvent);{
				Finish();
				return;
			}
			}
			
			startTime = Time.realtimeSinceStartup;
			timer = 0f;
		}
		
		public override void OnUpdate()
		{
			
			if (realTime)
			{
				timer = Time.realtimeSinceStartup - startTime;
			}
			else
			{
				timer += Time.deltaTime;
			}
			
			if (timer > delay.Value)
			{
				if (clear.Value) {
					stopWithClear.Value = false;
						ps.Clear();
					}
					
					else { 
					ps.Stop();
							ps.Clear();
						
					}
					
					if(finishEvent != null) Fsm.Event(finishEvent);{
						Finish();
						return;
					}
				}
			}
		}
		
	}