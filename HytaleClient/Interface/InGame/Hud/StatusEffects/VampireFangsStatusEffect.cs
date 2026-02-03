using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud.StatusEffects
{
	// Token: 0x020008CF RID: 2255
	internal class VampireFangsStatusEffect : TrinketBuffStatusEffect
	{
		// Token: 0x0600417C RID: 16764 RVA: 0x000C19B4 File Offset: 0x000BFBB4
		public VampireFangsStatusEffect(InGameView InGameView, Desktop desktop, Element parent, string id = "") : base(InGameView, desktop, parent, id)
		{
			this._satiatedEffectId = InGameView.InGame.Instance.EntityStoreModule.EntityEffectIndicesByIds[this._satiatedEffectName];
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x000C1A08 File Offset: 0x000BFC08
		public override void Build()
		{
			base.Build();
			this._renderPercentage = 1f;
			this._targetPercentage = 1f;
			base.SetBuffColor(BaseStatusEffect.BarColor.Green);
			base.AnimateBars();
			base.SetArrowVisible(BaseStatusEffect.StatusEffectArrows.Buff);
			this.SetIcon();
			this.SetEnabledBackground();
			Entity.UniqueEntityEffect? vampireFangsEffect = this.GetVampireFangsEffect();
			bool flag = vampireFangsEffect == null;
			if (!flag)
			{
				this.OnEffectAdded(vampireFangsEffect.Value.NetworkEffectIndex);
			}
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x000C1A84 File Offset: 0x000BFC84
		private void SetDisabledBackground()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/StatusEffects/StatusEffect.ui", out document);
			PatchStyle statusEffectBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "DisabledBackground");
			base.SetStatusEffectBackground(statusEffectBackground);
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x000C1ACC File Offset: 0x000BFCCC
		private void SetEnabledBackground()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/StatusEffects/StatusEffect.ui", out document);
			PatchStyle statusEffectBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "BuffBackground");
			base.SetStatusEffectBackground(statusEffectBackground);
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x000C1B14 File Offset: 0x000BFD14
		private void SetIcon()
		{
			string texturePath = "InGame/Hud/StatusEffects/Assets/Icons/" + VampireFangsStatusEffect.VampireFangsName + ".png";
			this._buffIcon.Background = new PatchStyle(texturePath);
			this._buffIcon.Layout(null, true);
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x000C1B60 File Offset: 0x000BFD60
		private void SetDisabledIcon()
		{
			string texturePath = "InGame/Hud/StatusEffects/Assets/Icons/" + VampireFangsStatusEffect.VampireFangsName + "_Disabled.png";
			this._buffIcon.Background = new PatchStyle(texturePath);
			this._buffIcon.Layout(null, true);
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x000C1BAC File Offset: 0x000BFDAC
		public void OnEffectAdded(int effectIndex)
		{
			bool flag = effectIndex != this._satiatedEffectId;
			if (!flag)
			{
				this._isSatiated = true;
				Entity.UniqueEntityEffect? vampireFangsEffect = this.GetVampireFangsEffect();
				bool flag2 = vampireFangsEffect == null;
				if (!flag2)
				{
					this._initialCountdown = vampireFangsEffect.Value.RemainingDuration;
					base.SetArrowVisible(BaseStatusEffect.StatusEffectArrows.BuffDisabled);
					base.SetBuffColor(BaseStatusEffect.BarColor.Grey);
					this.SetDisabledBackground();
					this.SetDisabledIcon();
				}
			}
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x000C1C1C File Offset: 0x000BFE1C
		public void OnEffectRemoved(int effectIndex)
		{
			bool flag = effectIndex != this._satiatedEffectId;
			if (!flag)
			{
				this._isSatiated = false;
				this._cooldownTargetPercentage = 0f;
				this._cooldownRenderPercentage = 0f;
				base.AnimateCooldown();
				base.SetArrowVisible(BaseStatusEffect.StatusEffectArrows.Buff);
				base.SetBuffColor(BaseStatusEffect.BarColor.Green);
				this.SetEnabledBackground();
				this.SetIcon();
				this.Desktop.Provider.PlaySound(this._buffCooldownCompletedSound);
			}
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x000C1C98 File Offset: 0x000BFE98
		protected override void Animate(float deltaTime)
		{
			bool flag = !this._isSatiated;
			if (!flag)
			{
				Entity.UniqueEntityEffect? vampireFangsEffect = this.GetVampireFangsEffect();
				bool flag2 = vampireFangsEffect == null;
				if (!flag2)
				{
					this.SetCountdownPercentage(vampireFangsEffect.Value.RemainingDuration);
					base.Animate(deltaTime);
				}
			}
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x000C1CE7 File Offset: 0x000BFEE7
		public void SetCountdownPercentage(float remainingTime)
		{
			this._cooldownTargetPercentage = 1f - remainingTime / this._initialCountdown;
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x000C1D00 File Offset: 0x000BFF00
		private Entity.UniqueEntityEffect? GetVampireFangsEffect()
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
					bool flag2 = uniqueEntityEffect2.NetworkEffectIndex == this._satiatedEffectId;
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

		// Token: 0x04001FD0 RID: 8144
		private bool _isSatiated = false;

		// Token: 0x04001FD1 RID: 8145
		private float _initialCountdown;

		// Token: 0x04001FD2 RID: 8146
		private int _satiatedEffectId;

		// Token: 0x04001FD3 RID: 8147
		private string _satiatedEffectName = "Trinket_Vampire_Fangs_Satiated";

		// Token: 0x04001FD4 RID: 8148
		public static readonly string VampireFangsName = "Trinket_Vampire_Fangs";
	}
}
