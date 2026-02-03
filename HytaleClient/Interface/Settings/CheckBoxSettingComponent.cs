using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x02000808 RID: 2056
	internal class CheckBoxSettingComponent : SettingComponent<bool>
	{
		// Token: 0x0600390A RID: 14602 RVA: 0x000770E0 File Offset: 0x000752E0
		public CheckBoxSettingComponent(Desktop desktop, Group parent, string name, ISettingView settings) : base(desktop, parent, name, settings)
		{
			Document document;
			UIFragment uifragment = base.Build("CheckBoxSetting.ui", out document);
			this._checkBox = uifragment.Get<CheckBox>("CheckBox");
			this._checkBox.ValueChanged = delegate()
			{
				this.OnChange(this._checkBox.Value);
			};
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x00077130 File Offset: 0x00075330
		public override void SetValue(bool value)
		{
			this._checkBox.Value = value;
			bool isMounted = this._checkBox.IsMounted;
			if (isMounted)
			{
				this._checkBox.Layout(null, true);
			}
		}

		// Token: 0x040018C1 RID: 6337
		private CheckBox _checkBox;
	}
}
