using System;
using HytaleClient.Data.EntityUI.CombatText;
using HytaleClient.Graphics;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.InGame.EntityUI;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.EntityUI
{
	// Token: 0x02000B01 RID: 2817
	internal class ClientCombatTextUIComponent : ClientEntityUIComponent
	{
		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x06005868 RID: 22632 RVA: 0x001AFABE File Offset: 0x001ADCBE
		public UInt32Color TextColorUInt32
		{
			get
			{
				return UInt32Color.FromRGBA((byte)this.TextColor.Red, (byte)this.TextColor.Green, (byte)this.TextColor.Blue, byte.MaxValue);
			}
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x001AFAEE File Offset: 0x001ADCEE
		private ClientCombatTextUIComponent()
		{
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x001AFAF8 File Offset: 0x001ADCF8
		public ClientCombatTextUIComponent(int id, EntityUIComponent component, EntityUIComponentRenderer<ClientCombatTextUIComponent> renderer) : base(id, component)
		{
			RangeVector2f combatTextRandomPositionOffsetRange = component.CombatTextRandomPositionOffsetRange;
			this.MinRandomPositionOffset = new Vector2f(combatTextRandomPositionOffsetRange.X.Min, combatTextRandomPositionOffsetRange.Y.Min);
			this.MaxRandomPositionOffset = new Vector2f(combatTextRandomPositionOffsetRange.X.Max, combatTextRandomPositionOffsetRange.Y.Max);
			this.ViewportMargin = component.CombatTextViewportMargin;
			this.Duration = component.CombatTextDuration;
			this.HitAngleModifierStrength = component.CombatTextHitAngleModifierStrength;
			this.FontSize = component.CombatTextFontSize;
			this.TextColor = component.CombatTextColor;
			this.AnimationEvents = new AnimationEvent[component.CombatTextAnimationEvents.Length];
			for (int i = 0; i < component.CombatTextAnimationEvents.Length; i++)
			{
				CombatTextEntityUIComponentAnimationEvent combatTextEntityUIComponentAnimationEvent = component.CombatTextAnimationEvents[i];
				switch (combatTextEntityUIComponentAnimationEvent.Type)
				{
				case 0:
					this.AnimationEvents[i] = new ScaleAnimationEvent(component.CombatTextAnimationEvents[i]);
					break;
				case 1:
					this.AnimationEvents[i] = new PositionAnimationEvent(component.CombatTextAnimationEvents[i]);
					break;
				case 2:
					this.AnimationEvents[i] = new OpacityAnimationEvent(component.CombatTextAnimationEvents[i]);
					break;
				}
			}
			this.Renderer = renderer;
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x001AFC3C File Offset: 0x001ADE3C
		public override ClientEntityUIComponent Clone()
		{
			return new ClientCombatTextUIComponent
			{
				Id = this.Id,
				HitboxOffset = this.HitboxOffset,
				Unknown = this.Unknown,
				Renderer = this.Renderer,
				MinRandomPositionOffset = this.MinRandomPositionOffset,
				MaxRandomPositionOffset = this.MaxRandomPositionOffset,
				ViewportMargin = this.ViewportMargin,
				Duration = this.Duration,
				HitAngleModifierStrength = this.HitAngleModifierStrength,
				FontSize = this.FontSize,
				TextColor = this.TextColor,
				AnimationEvents = this.AnimationEvents
			};
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x001AFCE5 File Offset: 0x001ADEE5
		public override void RegisterDrawTasksForEntity(Entity entity, Matrix transformationMatrix, float distanceToCamera, int entitiesCount, ref EntityUIDrawTask[] drawTasks, ref int drawTasksCount)
		{
			this.Renderer.RegisterDrawTasksForEntity(this, entity, transformationMatrix, distanceToCamera, entitiesCount, ref drawTasks, ref drawTasksCount);
		}

		// Token: 0x0600586D RID: 22637 RVA: 0x001AFCFE File Offset: 0x001ADEFE
		public override void PrepareForDraw(EntityUIDrawTask task)
		{
			this.Renderer.PrepareForDraw(this, task);
		}

		// Token: 0x040036E0 RID: 14048
		public Vector2f MinRandomPositionOffset;

		// Token: 0x040036E1 RID: 14049
		public Vector2f MaxRandomPositionOffset;

		// Token: 0x040036E2 RID: 14050
		public float ViewportMargin;

		// Token: 0x040036E3 RID: 14051
		public float Duration;

		// Token: 0x040036E4 RID: 14052
		public float HitAngleModifierStrength;

		// Token: 0x040036E5 RID: 14053
		public float FontSize;

		// Token: 0x040036E6 RID: 14054
		public Color TextColor;

		// Token: 0x040036E7 RID: 14055
		public AnimationEvent[] AnimationEvents;

		// Token: 0x040036E8 RID: 14056
		public EntityUIComponentRenderer<ClientCombatTextUIComponent> Renderer;
	}
}
