using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x0200080C RID: 2060
	internal class InputBindingSettingComponent : SettingComponent<string>
	{
		// Token: 0x0600391D RID: 14621 RVA: 0x00077764 File Offset: 0x00075964
		public InputBindingSettingComponent(Desktop desktop, Group parent, string name, ISettingView settings) : base(desktop, parent, name, settings)
		{
			this._name = "ui.settings.bindings." + name;
			Document document;
			UIFragment uifragment = base.Build("InputBindingSetting.ui", out document);
			this._button = uifragment.Get<TextButton>("Button");
			this._button.Activating = delegate()
			{
				this.OnChange(null);
			};
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x000777C5 File Offset: 0x000759C5
		public override void SetValue(string value)
		{
			this._button.Text = (value ?? "");
		}

		// Token: 0x040018C5 RID: 6341
		private TextButton _button;
	}
}
