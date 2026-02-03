using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x02000809 RID: 2057
	internal class DropdownSettingComponent : SettingComponent<string>
	{
		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x0600390D RID: 14605 RVA: 0x00077189 File Offset: 0x00075389
		public DropdownBox Dropdown { get; }

		// Token: 0x0600390E RID: 14606 RVA: 0x00077194 File Offset: 0x00075394
		public DropdownSettingComponent(Desktop desktop, Group parent, string name, ISettingView settings, List<DropdownBox.DropdownEntryInfo> values) : base(desktop, parent, name, settings)
		{
			Document document;
			UIFragment uifragment = base.Build("DropdownSetting.ui", out document);
			this.Dropdown = uifragment.Get<DropdownBox>("Dropdown");
			this.Dropdown.Entries = values;
			this.Dropdown.ValueChanged = delegate()
			{
				this.OnChange(this.Dropdown.Value);
			};
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x000771F2 File Offset: 0x000753F2
		public override void SetValue(string value)
		{
			this.Dropdown.Value = value;
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x00077201 File Offset: 0x00075401
		public void SetEntries(List<DropdownBox.DropdownEntryInfo> values)
		{
			this.Dropdown.Entries = values;
		}
	}
}
