using System;
using System.Diagnostics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x0200080F RID: 2063
	internal abstract class SettingComponent<T> : Element
	{
		// Token: 0x06003925 RID: 14629 RVA: 0x00077899 File Offset: 0x00075A99
		protected SettingComponent(Desktop desktop, Group parent, string name, ISettingView settings) : base(desktop, parent)
		{
			this._name = name;
			this._settings = settings;
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x000778B4 File Offset: 0x00075AB4
		protected UIFragment Build(string template, out Document doc)
		{
			this._settings.TryGetDocument(template, out doc);
			UIFragment uifragment = doc.Instantiate(this.Desktop, this);
			uifragment.Get<Label>("Name").Text = this.Desktop.Provider.GetText(this._name, null, true);
			return uifragment;
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x00077910 File Offset: 0x00075B10
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			bool flag = !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = (base.HitTest(position) ?? this);
			}
			return result;
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x00077954 File Offset: 0x00075B54
		protected override void OnMouseEnter()
		{
			string key = this._name + ".description";
			string text = this.Desktop.Provider.GetText(key, null, false);
			bool flag = text == null;
			if (!flag)
			{
				this._settings.SetHoveredSetting<T>(text, this);
			}
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x0007799F File Offset: 0x00075B9F
		protected override void OnMouseLeave()
		{
			this._settings.SetHoveredSetting<T>(null, this);
		}

		// Token: 0x0600392A RID: 14634
		public abstract void SetValue(T value);

		// Token: 0x040018C7 RID: 6343
		protected string _name;

		// Token: 0x040018C8 RID: 6344
		protected ISettingView _settings;

		// Token: 0x040018C9 RID: 6345
		public Action<T> OnChange;
	}
}
