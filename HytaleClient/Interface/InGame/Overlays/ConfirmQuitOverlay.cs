using System;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.InGame.Overlays
{
	// Token: 0x020008AB RID: 2219
	internal class ConfirmQuitOverlay : InterfaceComponent
	{
		// Token: 0x06004042 RID: 16450 RVA: 0x000B8767 File Offset: 0x000B6967
		public ConfirmQuitOverlay(InGameView inGameView) : base(inGameView.Interface, null)
		{
			this.InGameView = inGameView;
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x000B8780 File Offset: 0x000B6980
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Overlays/ConfirmQuitOverlay.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			uifragment.Get<TextButton>("ReturnToGame").Activating = new Action(this.Dismiss);
			uifragment.Get<TextButton>("QuitToDesktop").Activating = new Action(this.Validate);
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x000B87F0 File Offset: 0x000B69F0
		protected internal override void Dismiss()
		{
			this.InGameView.InGame.TryClosePageOrOverlay();
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x000B8804 File Offset: 0x000B6A04
		protected internal override void Validate()
		{
			this.InGameView.InGame.RequestExit(true);
		}

		// Token: 0x04001EA3 RID: 7843
		public readonly InGameView InGameView;
	}
}
