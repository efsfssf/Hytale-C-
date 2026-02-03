using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Hud.StatusEffects
{
	// Token: 0x020008C8 RID: 2248
	internal abstract class BaseStatusEffect : Element
	{
		// Token: 0x06004148 RID: 16712 RVA: 0x000C0344 File Offset: 0x000BE544
		public BaseStatusEffect(InGameView inGameView, Desktop desktop, Element parent) : base(desktop, parent)
		{
			this.InGameView = inGameView;
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x000C03A4 File Offset: 0x000BE5A4
		public virtual void Build()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/StatusEffects/StatusEffect.ui", out document);
			this._statusEffectBarFillGreen = document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "StatusEffectBarFillGreen");
			this._statusEffectBarFillOrange = document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "StatusEffectBarFillOrange");
			this._statusEffectBarFillRed = document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "StatusEffectBarFillRed");
			this._statusEffectBarFillGrey = document.ResolveNamedValue<UIPath>(this.Desktop.Provider, "StatusEffectBarFillGrey");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._statusEffectContainer = uifragment.Get<Group>("StatusEffect");
			this._buffIcon = uifragment.Get<Group>("StatusEffectIcon");
			this._cooldownBar = uifragment.Get<ProgressBar>("Cooldown");
			this._cooldownContainer = uifragment.Get<Group>("CooldownContainer");
			this._statusEffectArrowContainer = uifragment.Get<Group>("StatusEffectArrows");
			this._statusEffectsArrows = new Dictionary<BaseStatusEffect.StatusEffectArrows, Group>();
			this._statusEffectsArrows.Add(BaseStatusEffect.StatusEffectArrows.Buff, uifragment.Get<Group>("ArrowBuff"));
			this._statusEffectsArrows.Add(BaseStatusEffect.StatusEffectArrows.Debuff, uifragment.Get<Group>("ArrowDebuff"));
			this._statusEffectsArrows.Add(BaseStatusEffect.StatusEffectArrows.BuffDisabled, uifragment.Get<Group>("ArrowBuffDisabled"));
			this._statusEffectArrowContainer = uifragment.Get<Group>("StatusEffectArrows");
			this._statusEffectsArrows = new Dictionary<BaseStatusEffect.StatusEffectArrows, Group>();
			this._statusEffectsArrows.Add(BaseStatusEffect.StatusEffectArrows.Buff, uifragment.Get<Group>("ArrowBuff"));
			this._statusEffectsArrows.Add(BaseStatusEffect.StatusEffectArrows.Debuff, uifragment.Get<Group>("ArrowDebuff"));
			this._statusEffectsArrows.Add(BaseStatusEffect.StatusEffectArrows.BuffDisabled, uifragment.Get<Group>("ArrowBuffDisabled"));
			Document document2;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/StatusEffects/StatusEffectProgressDecrease.ui", out document2);
			UIFragment uifragment2 = document2.Instantiate(this.Desktop, uifragment.Get<Group>("StatusEffectProgressBarsContainer"));
			this._progressBars = new ProgressBar[5];
			for (int i = 0; i < 5; i++)
			{
				this._progressBars[i] = uifragment2.Get<ProgressBar>("Bar" + (i + 1).ToString());
			}
			this._barContainer = uifragment2.Get<Group>("StatusEffectProgressBars");
			this.AnimateBars();
			this.AnimateCooldown();
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x000C05F0 File Offset: 0x000BE7F0
		protected void SetBuffColor(BaseStatusEffect.BarColor barColor)
		{
			UIPath barTexturePath = this._statusEffectBarFillGreen;
			bool flag = barColor == BaseStatusEffect.BarColor.Orange;
			if (flag)
			{
				barTexturePath = this._statusEffectBarFillOrange;
			}
			else
			{
				bool flag2 = barColor == BaseStatusEffect.BarColor.Red;
				if (flag2)
				{
					barTexturePath = this._statusEffectBarFillRed;
				}
				else
				{
					bool flag3 = barColor == BaseStatusEffect.BarColor.Grey;
					if (flag3)
					{
						barTexturePath = this._statusEffectBarFillGrey;
					}
				}
			}
			for (int i = 0; i < this._progressBars.Length; i++)
			{
				this._progressBars[i].BarTexturePath = barTexturePath;
			}
			this._barContainer.Layout(null, true);
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x000C067C File Offset: 0x000BE87C
		protected void SetArrowVisible(BaseStatusEffect.StatusEffectArrows statusEffectArrowName)
		{
			foreach (KeyValuePair<BaseStatusEffect.StatusEffectArrows, Group> keyValuePair in this._statusEffectsArrows)
			{
				keyValuePair.Value.Visible = (keyValuePair.Key == statusEffectArrowName);
			}
			this._statusEffectArrowContainer.Layout(null, true);
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x000C06FC File Offset: 0x000BE8FC
		protected void SetStatusEffectBackground(PatchStyle statusEffectBackground)
		{
			this._statusEffectContainer.Background = statusEffectBackground;
			this._statusEffectContainer.Layout(null, true);
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x000C072C File Offset: 0x000BE92C
		protected void AnimateBars()
		{
			for (int i = 0; i < BaseStatusEffect._percentageByIndex.Length - 1; i++)
			{
				bool flag = this._renderPercentage >= BaseStatusEffect._percentageByIndex[i + 1];
				if (flag)
				{
					this.ClearBars(i);
					this._progressBars[i].Value = this.GetCurrentBarPercentage(i + 1);
					break;
				}
			}
			this._barContainer.Layout(null, true);
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x000C07A4 File Offset: 0x000BE9A4
		private void ClearBars(int index)
		{
			for (int i = 0; i < this._progressBars.Length; i++)
			{
				ProgressBar progressBar = this._progressBars[i];
				bool flag = i <= index;
				if (flag)
				{
					progressBar.Value = 0f;
				}
				else
				{
					bool flag2 = i > index;
					if (flag2)
					{
						progressBar.Value = 1f;
					}
				}
			}
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x000C0808 File Offset: 0x000BEA08
		private float GetCurrentBarPercentage(int index)
		{
			float num = BaseStatusEffect._percentageByIndex[index - 1] - BaseStatusEffect._percentageByIndex[index];
			float num2 = this._renderPercentage - BaseStatusEffect._percentageByIndex[index];
			return num2 / num;
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x000C083E File Offset: 0x000BEA3E
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x000C0859 File Offset: 0x000BEA59
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x000C0874 File Offset: 0x000BEA74
		protected virtual void Animate(float deltaTime)
		{
			bool flag = this._targetPercentage != this._renderPercentage;
			if (flag)
			{
				this._renderPercentage = MathHelper.Lerp(this._renderPercentage, this._targetPercentage, deltaTime * this.animationSpeed);
				this.AnimateBars();
			}
			bool flag2 = this._cooldownTargetPercentage != this._cooldownRenderPercentage;
			if (flag2)
			{
				this._cooldownRenderPercentage = MathHelper.Lerp(this._cooldownRenderPercentage, this._cooldownTargetPercentage, deltaTime * this.animationSpeed);
				this.AnimateCooldown();
			}
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x000C08FC File Offset: 0x000BEAFC
		protected void AnimateCooldown()
		{
			this._cooldownBar.Value = this._cooldownRenderPercentage;
			this._cooldownContainer.Layout(null, true);
		}

		// Token: 0x04001FA1 RID: 8097
		public readonly InGameView InGameView;

		// Token: 0x04001FA2 RID: 8098
		private static readonly float[] _percentageByIndex = new float[]
		{
			1f,
			0.875f,
			0.625f,
			0.375f,
			0.125f,
			0f
		};

		// Token: 0x04001FA3 RID: 8099
		protected float _targetPercentage = 0f;

		// Token: 0x04001FA4 RID: 8100
		protected float _renderPercentage = 0f;

		// Token: 0x04001FA5 RID: 8101
		private ProgressBar[] _progressBars;

		// Token: 0x04001FA6 RID: 8102
		private Group _statusEffectContainer;

		// Token: 0x04001FA7 RID: 8103
		private Group _barContainer;

		// Token: 0x04001FA8 RID: 8104
		protected Group _buffIcon;

		// Token: 0x04001FA9 RID: 8105
		private UIPath _statusEffectBarFillGreen;

		// Token: 0x04001FAA RID: 8106
		private UIPath _statusEffectBarFillOrange;

		// Token: 0x04001FAB RID: 8107
		private UIPath _statusEffectBarFillRed;

		// Token: 0x04001FAC RID: 8108
		private UIPath _statusEffectBarFillGrey;

		// Token: 0x04001FAD RID: 8109
		private float animationSpeed = 10f;

		// Token: 0x04001FAE RID: 8110
		private ProgressBar _cooldownBar;

		// Token: 0x04001FAF RID: 8111
		private Group _cooldownContainer;

		// Token: 0x04001FB0 RID: 8112
		protected float _cooldownTargetPercentage = 0f;

		// Token: 0x04001FB1 RID: 8113
		protected float _cooldownRenderPercentage = 0f;

		// Token: 0x04001FB2 RID: 8114
		private Group _statusEffectArrowContainer;

		// Token: 0x04001FB3 RID: 8115
		private Dictionary<BaseStatusEffect.StatusEffectArrows, Group> _statusEffectsArrows = new Dictionary<BaseStatusEffect.StatusEffectArrows, Group>();

		// Token: 0x02000D84 RID: 3460
		protected enum StatusEffectArrows
		{
			// Token: 0x04004243 RID: 16963
			Buff,
			// Token: 0x04004244 RID: 16964
			Debuff,
			// Token: 0x04004245 RID: 16965
			BuffDisabled
		}

		// Token: 0x02000D85 RID: 3461
		protected enum BarColor
		{
			// Token: 0x04004247 RID: 16967
			Green,
			// Token: 0x04004248 RID: 16968
			Orange,
			// Token: 0x04004249 RID: 16969
			Red,
			// Token: 0x0400424A RID: 16970
			Grey
		}
	}
}
