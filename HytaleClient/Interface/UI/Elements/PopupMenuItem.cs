using System;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200086D RID: 2157
	internal class PopupMenuItem
	{
		// Token: 0x06003CD4 RID: 15572 RVA: 0x000990D8 File Offset: 0x000972D8
		public PopupMenuItem(string label, Action onActivate, string iconTexturePath = null, SoundStyle activateSound = null)
		{
			this.Label = label;
			this.Activating = onActivate;
			this.IconTexturePath = ((iconTexturePath != null) ? new UIPath(iconTexturePath) : null);
			this.ActivateSound = activateSound;
		}

		// Token: 0x04001C3A RID: 7226
		public readonly string Label;

		// Token: 0x04001C3B RID: 7227
		public readonly Action Activating;

		// Token: 0x04001C3C RID: 7228
		public readonly UIPath IconTexturePath;

		// Token: 0x04001C3D RID: 7229
		public readonly SoundStyle ActivateSound;
	}
}
