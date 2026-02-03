using System;
using System.Collections.Generic;
using HytaleClient.Data.EntityUI;
using HytaleClient.Data.EntityUI.CombatText;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame.EntityUI
{
	// Token: 0x020008AE RID: 2222
	internal class CombatTextUIComponentRenderer : EntityUIComponentRenderer<ClientCombatTextUIComponent>
	{
		// Token: 0x06004055 RID: 16469 RVA: 0x000B8B9A File Offset: 0x000B6D9A
		public CombatTextUIComponentRenderer(InGameView inGameView) : base(inGameView)
		{
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x000B8BBC File Offset: 0x000B6DBC
		public override void Build(Element parent)
		{
			Document document;
			this._inGameView.Interface.TryGetDocument("InGame/EntityUI/CombatText.ui", out document);
			this._uiFragment = document.Instantiate(parent.Desktop, parent);
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x000B8BF8 File Offset: 0x000B6DF8
		protected override bool ShouldBeVisibleForEntity(Entity entity, int entitiesCount, float distanceToCamera)
		{
			int entityUIMaxEntities = base._settings.EntityUIMaxEntities;
			float entityUIMaxDistance = base._settings.EntityUIMaxDistance;
			bool flag = entitiesCount >= entityUIMaxEntities;
			bool flag2 = distanceToCamera > entityUIMaxDistance;
			return !flag && !flag2;
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x000B8C3C File Offset: 0x000B6E3C
		public override void RegisterDrawTasksForEntity(ClientCombatTextUIComponent component, Entity entity, Matrix transformationMatrix, float distanceToCamera, int entitiesCount, ref EntityUIDrawTask[] drawTasks, ref int drawTasksCount)
		{
			bool flag = !this.ShouldBeVisibleForEntity(entity, entitiesCount, distanceToCamera);
			if (!flag)
			{
				bool flag2 = entity.CombatTextsCount > 0;
				if (flag2)
				{
					float num = Math.Abs(component.MinRandomPositionOffset.X);
					float num2 = Math.Abs(component.MinRandomPositionOffset.Y);
					float num3 = Math.Abs(component.MaxRandomPositionOffset.X);
					float num4 = Math.Abs(component.MaxRandomPositionOffset.Y);
					for (int i = 0; i < entity.CombatTextsCount; i++)
					{
						int num5 = this._combatTextIndex;
						this._combatTextIndex = num5 + 1;
						int num6 = num5;
						ValueTuple<int, int> key = new ValueTuple<int, int>(entity.NetworkId, num6);
						base.ApplyTransitionState(key, component.Duration, 0f);
						switch (num6 % 4)
						{
						case 0:
							num = -num;
							num3 = -num3;
							break;
						case 1:
							num2 = -num2;
							num4 = -num4;
							break;
						case 2:
							num = -num;
							num3 = -num3;
							num2 = -num2;
							num4 = -num4;
							break;
						}
						float num7 = this._random.NextFloat(num, num3);
						float num8 = this._random.NextFloat(num2, num4);
						Entity.CombatText combatText = entity.CombatTexts[i];
						this._combatTexts[key] = new CombatTextUIComponentRenderer.CombatText
						{
							HitAngleModifier = (float)combatText.HitAngleDeg * component.HitAngleModifierStrength,
							Text = combatText.Text,
							PositionOffset = new Vector2f(num7, num8)
						};
					}
					entity.ClearCombatTexts();
				}
				float num9 = (float)this._inGameView.Desktop.ViewportRectangle.Width;
				float num10 = (float)this._inGameView.Desktop.ViewportRectangle.Height;
				float num11 = num9 / 2f - component.ViewportMargin;
				float num12 = num10 / 2f - component.ViewportMargin;
				foreach (KeyValuePair<ValueTuple<int, int>, EntityUIComponentRenderer<ClientCombatTextUIComponent>.TransitionState> keyValuePair in this._transitionStates)
				{
					bool flag3 = entity.NetworkId != keyValuePair.Key.Item1;
					if (!flag3)
					{
						CombatTextUIComponentRenderer.CombatText combatText2 = this._combatTexts[keyValuePair.Key];
						float progress = keyValuePair.Value.Progress;
						int num5 = drawTasksCount;
						drawTasksCount = num5 + 1;
						int num13 = num5;
						EntityUIDrawTask entityUIDrawTask = drawTasks[num13];
						entityUIDrawTask.ComponentId = component.Id;
						entityUIDrawTask.StringValue = combatText2.Text;
						entityUIDrawTask.TransformationMatrix = component.ApplyHitboxOffset(transformationMatrix);
						float num14 = entityUIDrawTask.TransformationMatrix.M41 + combatText2.PositionOffset.X;
						float num15 = entityUIDrawTask.TransformationMatrix.M42 + combatText2.PositionOffset.Y;
						num14 = MathHelper.Clamp(num14, -num11, num11);
						num15 = MathHelper.Clamp(num15, -num12, num12);
						entityUIDrawTask.TransformationMatrix.M41 = num14;
						entityUIDrawTask.TransformationMatrix.M42 = num15;
						foreach (AnimationEvent animationEvent in component.AnimationEvents)
						{
							animationEvent.ApplyAnimationState(ref entityUIDrawTask, progress);
						}
						entityUIDrawTask.TransformationMatrix.M41 = entityUIDrawTask.TransformationMatrix.M41 + combatText2.HitAngleModifier * progress;
						drawTasks[num13] = entityUIDrawTask;
					}
				}
			}
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x000B8FDC File Offset: 0x000B71DC
		public override void PrepareForDraw(ClientCombatTextUIComponent component, EntityUIDrawTask task)
		{
			Label label = this._uiFragment.Get<Label>("Text");
			label.Text = task.StringValue;
			label.Style.FontSize = component.FontSize;
			bool flag = task.Scale != null;
			if (flag)
			{
				label.Style.FontSize *= task.Scale.Value;
			}
			label.Style.TextColor = component.TextColorUInt32;
			Group group = this._uiFragment.Get<Group>("Container");
			group.Layout(null, true);
			group.PrepareForDraw();
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x000B9082 File Offset: 0x000B7282
		protected override void OnTransitionRemoved(ValueTuple<int, int> transitionKey)
		{
			this._combatTexts.Remove(transitionKey);
		}

		// Token: 0x04001EA9 RID: 7849
		private Dictionary<ValueTuple<int, int>, CombatTextUIComponentRenderer.CombatText> _combatTexts = new Dictionary<ValueTuple<int, int>, CombatTextUIComponentRenderer.CombatText>();

		// Token: 0x04001EAA RID: 7850
		private int _combatTextIndex;

		// Token: 0x04001EAB RID: 7851
		private UIFragment _uiFragment;

		// Token: 0x04001EAC RID: 7852
		private readonly Random _random = new Random();

		// Token: 0x02000D7A RID: 3450
		private struct CombatText
		{
			// Token: 0x04004210 RID: 16912
			public float HitAngleModifier;

			// Token: 0x04004211 RID: 16913
			public string Text;

			// Token: 0x04004212 RID: 16914
			public Vector2f PositionOffset;
		}
	}
}
