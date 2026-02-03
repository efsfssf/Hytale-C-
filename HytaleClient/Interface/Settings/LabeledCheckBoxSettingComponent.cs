using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x0200080E RID: 2062
	internal class LabeledCheckBoxSettingComponent : SettingComponent<bool>
	{
		// Token: 0x06003922 RID: 14626 RVA: 0x000777F0 File Offset: 0x000759F0
		public LabeledCheckBoxSettingComponent(Desktop desktop, Group parent, string name, ISettingView settings) : base(desktop, parent, name, settings)
		{
			Document document;
			UIFragment uifragment = base.Build("LabeledCheckBoxSetting.ui", out document);
			this._checkBox = uifragment.Get<LabeledCheckBox>("CheckBox");
			this._checkBox.ValueChanged = delegate()
			{
				this.OnChange(this._checkBox.Value);
			};
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x00077840 File Offset: 0x00075A40
		public override void SetValue(bool value)
		{
			this._checkBox.Value = value;
			bool isMounted = this._checkBox.IsMounted;
			if (isMounted)
			{
				this._checkBox.Layout(null, true);
			}
		}

		// Token: 0x040018C6 RID: 6342
		private LabeledCheckBox _checkBox;
	}
}
