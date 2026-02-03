using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.Settings
{
	// Token: 0x0200080D RID: 2061
	internal interface ISettingView
	{
		// Token: 0x06003920 RID: 14624
		void SetHoveredSetting<T>(string setting, SettingComponent<T> component);

		// Token: 0x06003921 RID: 14625
		bool TryGetDocument(string path, out Document document);
	}
}
