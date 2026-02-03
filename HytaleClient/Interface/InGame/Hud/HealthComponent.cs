using System;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008BB RID: 2235
	internal class HealthComponent : EntityStatBarComponent
	{
		// Token: 0x060040DA RID: 16602 RVA: 0x000BC909 File Offset: 0x000BAB09
		public HealthComponent(InGameView view) : base(view, "InGame/Hud/Health/Health.ui", false)
		{
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x000BC91A File Offset: 0x000BAB1A
		protected override void UpdateVisibility()
		{
			this.Interface.InGameView.UpdateHealthVisibility(true);
		}
	}
}
