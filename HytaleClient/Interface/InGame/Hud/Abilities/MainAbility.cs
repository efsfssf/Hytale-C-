using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;

namespace HytaleClient.Interface.InGame.Hud.Abilities
{
	// Token: 0x020008D3 RID: 2259
	internal class MainAbility : BaseAbility
	{
		// Token: 0x060041B0 RID: 16816 RVA: 0x000C36D8 File Offset: 0x000C18D8
		public MainAbility(InGameView inGameView, Desktop desktop, Element parent) : base(inGameView, desktop, parent)
		{
			this._iterationType = 0;
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x000C36EC File Offset: 0x000C18EC
		public override void Build()
		{
			base.Build();
			base.SetIcon(BaseAbility.IconName.SwordChargeUpAttack);
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x000C3700 File Offset: 0x000C1900
		public override void UpdateInputBiding()
		{
			bool flag = this.InGameView == null;
			if (!flag)
			{
				this._inputBidingKey = this.InGameView.Interface.App.Settings.InputBindings.PrimaryItemAction;
				base.UpdateInputBiding();
			}
		}
	}
}
