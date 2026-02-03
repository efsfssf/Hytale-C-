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
	// Token: 0x020008CA RID: 2250
	internal class EntityEffectBuff : BaseStatusEffect
	{
		// Token: 0x0600415B RID: 16731 RVA: 0x000C0B4E File Offset: 0x000BED4E
		public EntityEffectBuff(InGameView inGameView, Desktop desktop, Element parent, Entity.UniqueEntityEffect entityEffect, int id) : base(inGameView, desktop, parent)
		{
			this._initialCountdown = entityEffect.RemainingDuration;
			this._effectId = entityEffect.NetworkEffectIndex;
			this._iconPath = entityEffect.StatusEffectIcon;
			this.Id = id;
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x000C0B8C File Offset: 0x000BED8C
		public override void Build()
		{
			this._renderPercentage = 1f;
			this._targetPercentage = 1f;
			base.Build();
			base.SetBuffColor(BaseStatusEffect.BarColor.Green);
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/StatusEffects/StatusEffect.ui", out document);
			PatchStyle statusEffectBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "BuffBackground");
			base.SetStatusEffectBackground(statusEffectBackground);
			this.SetIcon();
			base.SetArrowVisible(BaseStatusEffect.StatusEffectArrows.Buff);
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x000C0C08 File Offset: 0x000BEE08
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

		// Token: 0x0600415E RID: 16734 RVA: 0x000C0C56 File Offset: 0x000BEE56
		public void SetCountdownPercentage(float remainingTime)
		{
			this._targetPercentage = remainingTime / this._initialCountdown;
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x000C0C67 File Offset: 0x000BEE67
		public void SetInitialCountdown(float initialCountdown)
		{
			this._initialCountdown = initialCountdown;
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x000C0C74 File Offset: 0x000BEE74
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
				bool flag3 = uniqueEntityEffect == null;
				if (!flag3)
				{
					this.SetCountdownPercentage(uniqueEntityEffect.Value.RemainingDuration);
					base.Animate(deltaTime);
				}
			}
		}

		// Token: 0x04001FB9 RID: 8121
		public int Id;

		// Token: 0x04001FBA RID: 8122
		private float _initialCountdown;

		// Token: 0x04001FBB RID: 8123
		private int _effectId;

		// Token: 0x04001FBC RID: 8124
		private string _iconPath;
	}
}
