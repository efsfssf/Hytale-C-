using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.InGame.Hud.Abilities
{
	// Token: 0x020008D1 RID: 2257
	internal class AbilityCharge : Element
	{
		// Token: 0x06004192 RID: 16786 RVA: 0x000C20EB File Offset: 0x000C02EB
		public AbilityCharge(InGameView inGameView, Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06004193 RID: 16787 RVA: 0x000C20F8 File Offset: 0x000C02F8
		public void Build()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/Abilities/AbilityCharge.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._chargeEmtpy = uifragment.Get<Group>("ChargeEmpty");
			this._chargeFull = uifragment.Get<Group>("ChargeFull");
			this._abilityChargeContainer = uifragment.Get<Group>("AbilityChargeContainer");
		}

		// Token: 0x06004194 RID: 16788 RVA: 0x000C2160 File Offset: 0x000C0360
		public void SetEmpty()
		{
			this._chargeEmtpy.Visible = true;
			this._chargeFull.Visible = false;
			this._abilityChargeContainer.Layout(null, true);
		}

		// Token: 0x06004195 RID: 16789 RVA: 0x000C21A0 File Offset: 0x000C03A0
		public void SetFull()
		{
			this._chargeEmtpy.Visible = false;
			this._chargeFull.Visible = true;
			this._abilityChargeContainer.Layout(null, true);
		}

		// Token: 0x04001FDA RID: 8154
		private Group _chargeEmtpy;

		// Token: 0x04001FDB RID: 8155
		private Group _chargeFull;

		// Token: 0x04001FDC RID: 8156
		private Group _abilityChargeContainer;
	}
}
