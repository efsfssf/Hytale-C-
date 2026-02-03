using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x02000811 RID: 2065
	internal class SliderSettingComponent : SettingComponent<int>
	{
		// Token: 0x06003950 RID: 14672 RVA: 0x0007A83C File Offset: 0x00078A3C
		public SliderSettingComponent(Desktop desktop, Group parent, string name, ISettingView settings, int min, int max, int step) : base(desktop, parent, name, settings)
		{
			Document document;
			UIFragment uifragment = base.Build("SliderSetting.ui", out document);
			this._slider = uifragment.Get<SliderNumberField>("Slider");
			this._slider.Min = min;
			this._slider.Max = max;
			this._slider.Step = step;
			this._slider.ValueChanged = delegate()
			{
				this.OnChange(this._slider.Value);
			};
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x0007A8B8 File Offset: 0x00078AB8
		public override void SetValue(int value)
		{
			this._slider.Value = value;
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._slider.Layout(null, true);
			}
		}

		// Token: 0x0400193B RID: 6459
		private SliderNumberField _slider;
	}
}
