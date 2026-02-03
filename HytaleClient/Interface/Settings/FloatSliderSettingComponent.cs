using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x0200080A RID: 2058
	internal class FloatSliderSettingComponent : SettingComponent<float>
	{
		// Token: 0x06003912 RID: 14610 RVA: 0x0007722C File Offset: 0x0007542C
		public FloatSliderSettingComponent(Desktop desktop, Group parent, string name, ISettingView settings, float min, float max, float step) : base(desktop, parent, name, settings)
		{
			Document document;
			UIFragment uifragment = base.Build("FloatSliderSetting.ui", out document);
			this._slider = uifragment.Get<FloatSliderNumberField>("Slider");
			this._slider.Min = min;
			this._slider.Max = max;
			this._slider.Step = step;
			this._slider.ValueChanged = delegate()
			{
				this.OnChange(this._slider.Value);
			};
			this._slider.NumberFieldMaxDecimalPlaces = 2;
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x000772B4 File Offset: 0x000754B4
		public override void SetValue(float value)
		{
			this._slider.Value = value;
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._slider.Layout(null, true);
			}
		}

		// Token: 0x040018C3 RID: 6339
		private FloatSliderNumberField _slider;
	}
}
