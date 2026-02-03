using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Data.EntityUI;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.EntityUI
{
	// Token: 0x020008B0 RID: 2224
	internal abstract class EntityUIComponentRenderer<T> where T : ClientEntityUIComponent
	{
		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x06004061 RID: 16481 RVA: 0x000B93DA File Offset: 0x000B75DA
		protected Settings _settings
		{
			get
			{
				return this._inGameView.Interface.App.Settings;
			}
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x000B93F1 File Offset: 0x000B75F1
		protected EntityUIComponentRenderer(InGameView inGameView)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06004063 RID: 16483
		public abstract void Build(Element parent);

		// Token: 0x06004064 RID: 16484
		protected abstract bool ShouldBeVisibleForEntity(Entity entity, int entitiesCount, float distanceToCamera);

		// Token: 0x06004065 RID: 16485
		public abstract void RegisterDrawTasksForEntity(T component, Entity entity, Matrix transformationMatrix, float distanceToCamera, int entitiesCount, ref EntityUIDrawTask[] drawTasks, ref int drawTasksCount);

		// Token: 0x06004066 RID: 16486
		public abstract void PrepareForDraw(T component, EntityUIDrawTask task);

		// Token: 0x06004067 RID: 16487 RVA: 0x000B9410 File Offset: 0x000B7610
		protected void ApplyTransitionState(ValueTuple<int, int> key, float duration, float delay = 0f)
		{
			EntityUIComponentRenderer<T>.TransitionState transitionState;
			bool flag = this._transitionStates.TryGetValue(key, out transitionState);
			if (flag)
			{
				bool flag2 = duration * transitionState.Timer > 0f;
				if (flag2)
				{
					return;
				}
				bool flag3 = transitionState.DelayTimer > 0f;
				if (flag3)
				{
					this._transitionStates.Remove(key);
					return;
				}
				transitionState.Duration = duration;
				transitionState.Timer = ((Math.Abs(transitionState.Timer) > Math.Abs(duration)) ? duration : (-transitionState.Timer));
			}
			else
			{
				transitionState = new EntityUIComponentRenderer<T>.TransitionState
				{
					DelayTimer = delay,
					Duration = duration,
					Timer = duration
				};
			}
			this._transitionStates[key] = transitionState;
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x000B94CC File Offset: 0x000B76CC
		public void Animate(float deltaTime)
		{
			foreach (ValueTuple<int, int> valueTuple in Enumerable.ToList<ValueTuple<int, int>>(this._transitionStates.Keys))
			{
				EntityUIComponentRenderer<T>.TransitionState transitionState = this._transitionStates[valueTuple];
				bool flag = transitionState.DelayTimer > 0f;
				if (flag)
				{
					transitionState.DelayTimer -= deltaTime;
					this._transitionStates[valueTuple] = transitionState;
					break;
				}
				bool flag2 = false;
				bool flag3 = transitionState.Duration < 0f && transitionState.Timer < 0f;
				if (flag3)
				{
					transitionState.Timer += deltaTime;
					bool flag4 = transitionState.Timer >= 0f;
					if (flag4)
					{
						flag2 = true;
					}
				}
				else
				{
					bool flag5 = transitionState.Duration > 0f && transitionState.Timer > 0f;
					if (flag5)
					{
						transitionState.Timer -= deltaTime;
						bool flag6 = transitionState.Timer <= 0f;
						if (flag6)
						{
							flag2 = true;
						}
					}
				}
				bool flag7 = flag2;
				if (flag7)
				{
					this._transitionStates.Remove(valueTuple);
					this.OnTransitionRemoved(valueTuple);
				}
				else
				{
					transitionState.Progress = 1f - transitionState.Timer / transitionState.Duration;
					this._transitionStates[valueTuple] = transitionState;
				}
			}
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x000B965C File Offset: 0x000B785C
		protected virtual void OnTransitionRemoved(ValueTuple<int, int> transitionKey)
		{
		}

		// Token: 0x04001EAF RID: 7855
		protected readonly InGameView _inGameView;

		// Token: 0x04001EB0 RID: 7856
		protected readonly Dictionary<ValueTuple<int, int>, EntityUIComponentRenderer<T>.TransitionState> _transitionStates = new Dictionary<ValueTuple<int, int>, EntityUIComponentRenderer<T>.TransitionState>();

		// Token: 0x02000D7B RID: 3451
		protected struct TransitionState
		{
			// Token: 0x04004213 RID: 16915
			public float DelayTimer;

			// Token: 0x04004214 RID: 16916
			public float Duration;

			// Token: 0x04004215 RID: 16917
			public float Timer;

			// Token: 0x04004216 RID: 16918
			public float Progress;
		}
	}
}
