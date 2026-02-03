using System;
using System.Collections.Generic;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.UI
{
	// Token: 0x02000828 RID: 2088
	public interface IUIProvider
	{
		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x06003A7E RID: 14974
		TextureArea WhitePixel { get; }

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x06003A7F RID: 14975
		TextureArea MissingTexture { get; }

		// Token: 0x06003A80 RID: 14976
		TextureArea MakeTextureArea(string path);

		// Token: 0x06003A81 RID: 14977
		bool TryGetDocument(string path, out Document document);

		// Token: 0x06003A82 RID: 14978
		FontFamily GetFontFamily(string name);

		// Token: 0x06003A83 RID: 14979
		string GetText(string key, Dictionary<string, string> parameters = null, bool returnFallback = true);

		// Token: 0x06003A84 RID: 14980
		string FormatNumber(int value);

		// Token: 0x06003A85 RID: 14981
		string FormatNumber(float value);

		// Token: 0x06003A86 RID: 14982
		string FormatNumber(double value);

		// Token: 0x06003A87 RID: 14983
		string FormatRelativeTime(DateTime time);

		// Token: 0x06003A88 RID: 14984
		void PlaySound(SoundStyle sound);
	}
}
