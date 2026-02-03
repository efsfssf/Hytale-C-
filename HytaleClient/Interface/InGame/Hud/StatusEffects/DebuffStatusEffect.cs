using System;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud.StatusEffects
{
	// Token: 0x020008C9 RID: 2249
	internal class DebuffStatusEffect : BaseStatusEffect
	{
		// Token: 0x06004155 RID: 16725 RVA: 0x000C0949 File Offset: 0x000BEB49
		public DebuffStatusEffect(InGameView inGameView, Desktop desktop, Element parent, Entity.UniqueEntityEffect entityEffect, int id) : base(inGameView, desktop, parent)
		{
			this._initialCountdown = entityEffect.RemainingDuration;
			this._effectId = entityEffect.NetworkEffectIndex;
			this._iconPath = entityEffect.StatusEffectIcon;
			this.Id = id;
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x000C0988 File Offset: 0x000BEB88
		public override void Build()
		{
			this._renderPercentage = 1f;
			this._targetPercentage = 1f;
			base.Build();
			base.SetBuffColor(BaseStatusEffect.BarColor.Red);
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/StatusEffects/StatusEffect.ui", out document);
			PatchStyle statusEffectBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "DebuffBackground");
			base.SetStatusEffectBackground(statusEffectBackground);
			this.SetIcon();
			base.SetArrowVisible(BaseStatusEffect.StatusEffectArrows.Debuff);
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "DebuffStartSound", out this._debuffStartSound);
			bool flag = this._debuffStartSound != null;
			if (flag)
			{
				this.Desktop.Provider.PlaySound(this._debuffStartSound);
			}
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x000C0A44 File Offset: 0x000BEC44
		private void SetIcon()
		{
			TextureArea missingTexture;
			bool flag = !this.InGameView.TryMountAssetTexture(this._iconPath, out missingTexture);
			if (flag)
			{
				missingTexture = this.Desktop.Provider.MissingTexture;
			}
			PatchStyle background = new PatchStyle(missingTexture);
			this._buffIcon.Background = background;
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x000C0A92 File Offset: 0x000BEC92
		public void SetCountdownPercentage(float remainingTime)
		{
			this._targetPercentage = remainingTime / this._initialCountdown;
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x000C0AA3 File Offset: 0x000BECA3
		public void SetInitialCountdown(float initialCountdown)
		{
			this._initialCountdown = initialCountdown;
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x000C0AB0 File Offset: 0x000BECB0
		protected override void Animate(float deltaTime)
		{
			GameInstance instance = this.InGameView.InGame.Instance;
			PlayerEntity playerEntity = (instance != null) ? instance.LocalPlayer : null;
			bool flag = playerEntity == null;
			if (!flag)
			{
				Entity.UniqueEntityEffect? uniqueEntityEffect = null;
				foreach (Entity.UniqueEntityEffect uniqueEntityEffect2 in playerEntity.EntityEffects)
				{
					bool flag2 = uniqueEntityEffect2.NetworkEffectIndex == this._effectId;
					if (flag2)
					{
						uniqueEntityEffect = new Entity.UniqueEntityEffect?(uniqueEntityEffect2);
					}
				}
				this.SetCountdownPercentage(uniqueEntityEffect.Value.RemainingDuration);
				base.Animate(deltaTime);
			}
		}

		// Token: 0x04001FB4 RID: 8116
		public int Id;

		// Token: 0x04001FB5 RID: 8117
		private float _initialCountdown;

		// Token: 0x04001FB6 RID: 8118
		private int _effectId;

		// Token: 0x04001FB7 RID: 8119
		private string _iconPath;

		// Token: 0x04001FB8 RID: 8120
		private SoundStyle _debuffStartSound;
	}
}
