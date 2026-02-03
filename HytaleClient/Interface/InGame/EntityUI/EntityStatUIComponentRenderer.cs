using System;
using System.Collections.Generic;
using HytaleClient.Data.EntityUI;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.EntityUI
{
	// Token: 0x020008AF RID: 2223
	internal class EntityStatUIComponentRenderer : EntityUIComponentRenderer<ClientEntityStatUIComponent>
	{
		// Token: 0x0600405B RID: 16475 RVA: 0x000B9091 File Offset: 0x000B7291
		public EntityStatUIComponentRenderer(InGameView inGameView) : base(inGameView)
		{
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x000B90A8 File Offset: 0x000B72A8
		public override void Build(Element parent)
		{
			Document document;
			this._inGameView.Interface.TryGetDocument("InGame/EntityUI/HealthBar.ui", out document);
			this._healthBarFragment = document.Instantiate(parent.Desktop, parent);
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x000B90E4 File Offset: 0x000B72E4
		protected override bool ShouldBeVisibleForEntity(Entity entity, int entitiesCount, float distanceToCamera)
		{
			int entityUIMaxEntities = base._settings.EntityUIMaxEntities;
			float entityUIMaxDistance = base._settings.EntityUIMaxDistance;
			bool visibilityPrediction = entity.VisibilityPrediction;
			bool debugUI = this._inGameView.InGame.Instance.EntityStoreModule.CurrentSetup.DebugUI;
			bool flag = entity.SmoothHealth == -1f;
			bool flag2 = entity.SmoothHealth == 1f;
			bool flag3 = entitiesCount >= entityUIMaxEntities;
			bool flag4 = distanceToCamera > entityUIMaxDistance;
			return visibilityPrediction && !flag3 && !flag4 && !flag && (!flag2 || debugUI);
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x000B9180 File Offset: 0x000B7380
		public override void RegisterDrawTasksForEntity(ClientEntityStatUIComponent component, Entity entity, Matrix transformationMatrix, float distanceToCamera, int entitiesCount, ref EntityUIDrawTask[] drawTasks, ref int drawTasksCount)
		{
			bool flag = this.ShouldBeVisibleForEntity(entity, entitiesCount, distanceToCamera);
			float entityUIHideDelay = base._settings.EntityUIHideDelay;
			float entityUIFadeInDuration = base._settings.EntityUIFadeInDuration;
			float entityUIFadeOutDuration = base._settings.EntityUIFadeOutDuration;
			ValueTuple<int, int> key = new ValueTuple<int, int>(entity.NetworkId, 0);
			bool flag2 = this._entitiesWithComponent.Contains(entity.NetworkId);
			bool flag3 = flag2;
			if (flag3)
			{
				if (!flag)
				{
					base.ApplyTransitionState(key, entityUIFadeOutDuration, entityUIHideDelay);
					this._entitiesWithComponent.Remove(entity.NetworkId);
				}
			}
			else if (flag)
			{
				base.ApplyTransitionState(key, -entityUIFadeInDuration, 0f);
				this._entitiesWithComponent.Add(entity.NetworkId);
			}
			EntityUIComponentRenderer<ClientEntityStatUIComponent>.TransitionState state;
			bool flag4 = this._transitionStates.TryGetValue(key, out state);
			bool flag5 = !this._entitiesWithComponent.Contains(entity.NetworkId) && !flag4;
			if (!flag5)
			{
				int num = flag4 ? this.GetTransitionOpacity(state) : 255;
				bool flag6 = num == 0;
				if (!flag6)
				{
					int num2 = drawTasksCount;
					drawTasksCount = num2 + 1;
					int num3 = num2;
					EntityUIDrawTask entityUIDrawTask = drawTasks[num3];
					entityUIDrawTask.FloatValue = entity.SmoothHealth;
					entityUIDrawTask.ComponentId = component.Id;
					entityUIDrawTask.TransformationMatrix = component.ApplyHitboxOffset(transformationMatrix);
					entityUIDrawTask.Opacity = new byte?(Convert.ToByte(num));
					drawTasks[num3] = entityUIDrawTask;
				}
			}
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x000B92F4 File Offset: 0x000B74F4
		private int GetTransitionOpacity(EntityUIComponentRenderer<ClientEntityStatUIComponent>.TransitionState state)
		{
			bool flag = state.DelayTimer > 0f;
			int result;
			if (flag)
			{
				result = ((state.Duration > 0f) ? 255 : 0);
			}
			else
			{
				bool flag2 = state.Duration > 0f;
				if (flag2)
				{
					result = (int)(state.Timer / state.Duration * 255f);
				}
				else
				{
					bool flag3 = state.Duration < 0f;
					if (flag3)
					{
						result = 255 - (int)(state.Timer / state.Duration * 255f);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x000B9388 File Offset: 0x000B7588
		public override void PrepareForDraw(ClientEntityStatUIComponent component, EntityUIDrawTask task)
		{
			this._healthBarFragment.Get<ProgressBar>("Fill").Value = task.FloatValue;
			Group group = this._healthBarFragment.Get<Group>("Container");
			group.Layout(null, true);
			group.PrepareForDraw();
		}

		// Token: 0x04001EAD RID: 7853
		private UIFragment _healthBarFragment;

		// Token: 0x04001EAE RID: 7854
		private HashSet<int> _entitiesWithComponent = new HashSet<int>();
	}
}
