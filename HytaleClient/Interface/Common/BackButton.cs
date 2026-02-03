using System;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.Common
{
	// Token: 0x020008D9 RID: 2265
	internal class BackButton : Button
	{
		// Token: 0x060041F3 RID: 16883 RVA: 0x000C6164 File Offset: 0x000C4364
		public BackButton(Interface @interface, Action onActivate) : base(@interface.Desktop, null)
		{
			this._interface = @interface;
			this.Activating = onActivate;
			Document document;
			this.Desktop.Provider.TryGetDocument("Common/BackButton.ui", out document);
			this._escContainerBackground = document.ResolveNamedValue<PatchStyle>(this._interface, "EscButtonBackground");
			this._escContainerBackgroundHovered = document.ResolveNamedValue<PatchStyle>(this._interface, "EscButtonBackgroundHovered");
			this._escContainerBackgroundPressed = document.ResolveNamedValue<PatchStyle>(this._interface, "EscButtonBackgroundPressed");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._escContainer = uifragment.Get<Group>("EscContainer");
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x000C620C File Offset: 0x000C440C
		protected override void ApplyStyles()
		{
			int? capturedMouseButton = base.CapturedMouseButton;
			long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
			long num2 = (long)((ulong)1);
			bool flag = num.GetValueOrDefault() == num2 & num != null;
			if (flag)
			{
				this._escContainer.Background = this._escContainerBackgroundPressed;
			}
			else
			{
				bool isHovered = base.IsHovered;
				if (isHovered)
				{
					this._escContainer.Background = this._escContainerBackgroundHovered;
				}
				else
				{
					this._escContainer.Background = this._escContainerBackground;
				}
			}
			base.ApplyStyles();
		}

		// Token: 0x04002036 RID: 8246
		private readonly Interface _interface;

		// Token: 0x04002037 RID: 8247
		private Group _escContainer;

		// Token: 0x04002038 RID: 8248
		private PatchStyle _escContainerBackground;

		// Token: 0x04002039 RID: 8249
		private PatchStyle _escContainerBackgroundHovered;

		// Token: 0x0400203A RID: 8250
		private PatchStyle _escContainerBackgroundPressed;
	}
}
