using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud.StatusEffects
{
	// Token: 0x020008CD RID: 2253
	internal class TrinketBuffStatusEffect : BaseStatusEffect
	{
		// Token: 0x06004178 RID: 16760 RVA: 0x000C18A6 File Offset: 0x000BFAA6
		public TrinketBuffStatusEffect(InGameView InGameView, Desktop desktop, Element parent, string id = "") : base(InGameView, desktop, parent)
		{
			this.Id = id;
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x000C18BC File Offset: 0x000BFABC
		public override void Build()
		{
			this._renderPercentage = 1f;
			this._targetPercentage = 1f;
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/StatusEffects/StatusEffect.ui", out document);
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "BuffStartSound", out this._buffStartSound);
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "BuffEffectElapsed", out this._buffEffectElapsedSound);
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "BuffCooldownCompleted", out this._buffCooldownCompletedSound);
			base.Build();
			base.SetBuffColor(BaseStatusEffect.BarColor.Green);
			this.SetIcon();
			base.SetArrowVisible(BaseStatusEffect.StatusEffectArrows.Buff);
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x000C1970 File Offset: 0x000BFB70
		private void SetIcon()
		{
			string texturePath = "InGame/Hud/StatusEffects/Assets/Icons/" + this.Id + ".png";
			this._buffIcon.Background = new PatchStyle(texturePath);
		}

		// Token: 0x04001FCC RID: 8140
		public string Id;

		// Token: 0x04001FCD RID: 8141
		protected SoundStyle _buffStartSound;

		// Token: 0x04001FCE RID: 8142
		protected SoundStyle _buffEffectElapsedSound;

		// Token: 0x04001FCF RID: 8143
		protected SoundStyle _buffCooldownCompletedSound;
	}
}
