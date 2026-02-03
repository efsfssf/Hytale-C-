using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud.StatusEffects
{
	// Token: 0x020008CB RID: 2251
	internal class LastStandSkullStatusEffect : TrinketBuffStatusEffect
	{
		// Token: 0x06004161 RID: 16737 RVA: 0x000C0D24 File Offset: 0x000BEF24
		public LastStandSkullStatusEffect(InGameView InGameView, Desktop desktop, Element parent, string id = "") : base(InGameView, desktop, parent, id)
		{
			this._lastStandSkullEffectId = InGameView.InGame.Instance.EntityStoreModule.EntityEffectIndicesByIds[LastStandSkullStatusEffect._lastStandSkullName];
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x000C0D60 File Offset: 0x000BEF60
		public override void Build()
		{
			base.Build();
			this._renderPercentage = 0f;
			this._targetPercentage = 0f;
			base.SetBuffColor(BaseStatusEffect.BarColor.Green);
			this.SetDisabledIcon();
			base.AnimateBars();
			base.SetArrowVisible(BaseStatusEffect.StatusEffectArrows.BuffDisabled);
			this.SetDisabledBackground();
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x000C0DB0 File Offset: 0x000BEFB0
		private void SetDisabledBackground()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/StatusEffects/StatusEffect.ui", out document);
			PatchStyle statusEffectBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "DisabledBackground");
			base.SetStatusEffectBackground(statusEffectBackground);
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x000C0DF8 File Offset: 0x000BEFF8
		private void SetEnabledBackground()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/StatusEffects/StatusEffect.ui", out document);
			PatchStyle statusEffectBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "BuffBackground");
			base.SetStatusEffectBackground(statusEffectBackground);
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x000C0E40 File Offset: 0x000BF040
		private void SetIcon()
		{
			string texturePath = "InGame/Hud/StatusEffects/Assets/Icons/" + this.Id + ".png";
			this._buffIcon.Background = new PatchStyle(texturePath);
			this._buffIcon.Layout(null, true);
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x000C0E8C File Offset: 0x000BF08C
		private void SetDisabledIcon()
		{
			string texturePath = "InGame/Hud/StatusEffects/Assets/Icons/" + this.Id + "_Disabled.png";
			this._buffIcon.Background = new PatchStyle(texturePath);
			this._buffIcon.Layout(null, true);
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x000C0ED8 File Offset: 0x000BF0D8
		public void OnEffectAdded(int effectIndex)
		{
			bool flag = effectIndex != this._lastStandSkullEffectId;
			if (!flag)
			{
				this._isLastStandSkullActive = true;
				Entity.UniqueEntityEffect? lastStandSkullEffect = this.GetLastStandSkullEffect();
				bool flag2 = lastStandSkullEffect == null;
				if (!flag2)
				{
					this._initialCountdown = lastStandSkullEffect.Value.RemainingDuration;
					base.SetArrowVisible(BaseStatusEffect.StatusEffectArrows.Buff);
					this.SetEnabledBackground();
					this.SetIcon();
					bool flag3 = this._buffStartSound != null;
					if (flag3)
					{
						this.Desktop.Provider.PlaySound(this._buffStartSound);
					}
				}
			}
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x000C0F64 File Offset: 0x000BF164
		public void OnEffectRemoved(int effectIndex)
		{
			bool flag = effectIndex != this._lastStandSkullEffectId;
			if (!flag)
			{
				this._isLastStandSkullActive = false;
				this._targetPercentage = 0f;
				this._renderPercentage = 0f;
				base.AnimateBars();
				base.SetArrowVisible(BaseStatusEffect.StatusEffectArrows.BuffDisabled);
				this.SetDisabledBackground();
				this.SetDisabledIcon();
				bool flag2 = this._buffEffectElapsedSound != null;
				if (flag2)
				{
					this.Desktop.Provider.PlaySound(this._buffEffectElapsedSound);
				}
			}
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x000C0FE4 File Offset: 0x000BF1E4
		protected override void Animate(float deltaTime)
		{
			bool flag = !this._isLastStandSkullActive;
			if (!flag)
			{
				Entity.UniqueEntityEffect? lastStandSkullEffect = this.GetLastStandSkullEffect();
				bool flag2 = lastStandSkullEffect == null;
				if (!flag2)
				{
					this.SetCountdownPercentage(lastStandSkullEffect.Value.RemainingDuration);
					base.Animate(deltaTime);
				}
			}
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x000C1033 File Offset: 0x000BF233
		public void SetCountdownPercentage(float remainingTime)
		{
			this._targetPercentage = remainingTime / this._initialCountdown;
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x000C1044 File Offset: 0x000BF244
		private Entity.UniqueEntityEffect? GetLastStandSkullEffect()
		{
			GameInstance instance = this.InGameView.InGame.Instance;
			PlayerEntity playerEntity = (instance != null) ? instance.LocalPlayer : null;
			bool flag = playerEntity == null;
			Entity.UniqueEntityEffect? result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Entity.UniqueEntityEffect? uniqueEntityEffect = null;
				foreach (Entity.UniqueEntityEffect uniqueEntityEffect2 in playerEntity.EntityEffects)
				{
					bool flag2 = uniqueEntityEffect2.NetworkEffectIndex == this._lastStandSkullEffectId;
					if (flag2)
					{
						uniqueEntityEffect = new Entity.UniqueEntityEffect?(uniqueEntityEffect2);
						break;
					}
				}
				result = uniqueEntityEffect;
			}
			return result;
		}

		// Token: 0x04001FBD RID: 8125
		private bool _isLastStandSkullActive = false;

		// Token: 0x04001FBE RID: 8126
		private float _initialCountdown;

		// Token: 0x04001FBF RID: 8127
		private int _lastStandSkullEffectId;

		// Token: 0x04001FC0 RID: 8128
		public static readonly string _lastStandSkullName = "Trinket_Last_Stand_Skull";
	}
}
