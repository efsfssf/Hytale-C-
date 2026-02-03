using System;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008C1 RID: 2241
	internal class OxygenComponent : EntityStatBarComponent
	{
		// Token: 0x06004104 RID: 16644 RVA: 0x000BDB45 File Offset: 0x000BBD45
		public OxygenComponent(InGameView view) : base(view, "InGame/Hud/Oxygen/Oxygen.ui", true)
		{
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x000BDB56 File Offset: 0x000BBD56
		protected override void UpdateVisibility()
		{
			this.Interface.InGameView.UpdateOxygenVisibility(true);
		}
	}
}
