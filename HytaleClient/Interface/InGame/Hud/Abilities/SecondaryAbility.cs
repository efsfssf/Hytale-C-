using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;

namespace HytaleClient.Interface.InGame.Hud.Abilities
{
	// Token: 0x020008D4 RID: 2260
	internal class SecondaryAbility : BaseAbility
	{
		// Token: 0x060041B3 RID: 16819 RVA: 0x000C3749 File Offset: 0x000C1949
		public SecondaryAbility(InGameView inGameView, Desktop desktop, Element parent) : base(inGameView, desktop, parent)
		{
			this._iterationType = 1;
			this._inputBidingKey = this.InGameView.Interface.App.Settings.InputBindings.SecondaryItemAction;
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x000C3782 File Offset: 0x000C1982
		public override void Build()
		{
			base.Build();
			base.SetIcon(BaseAbility.IconName.ShieldAbility);
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x000C3794 File Offset: 0x000C1994
		public override void UpdateInputBiding()
		{
			bool flag = this.InGameView == null;
			if (!flag)
			{
				this._inputBidingKey = this.InGameView.Interface.App.Settings.InputBindings.SecondaryItemAction;
				base.UpdateInputBiding();
			}
		}
	}
}
