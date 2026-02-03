using System;
using System.Collections.Generic;
using HytaleClient.InGame.Modules.Shortcuts;
using Newtonsoft.Json;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD8 RID: 2776
	internal class ShortcutSettings
	{
		// Token: 0x0600578C RID: 22412 RVA: 0x001A9648 File Offset: 0x001A7848
		public ShortcutSettings Clone()
		{
			return new ShortcutSettings
			{
				MacroShortcuts = new Dictionary<string, MacroShortcut>(this.MacroShortcuts),
				KeybindShortcuts = new Dictionary<string, KeybindShortcut>(this.KeybindShortcuts)
			};
		}

		// Token: 0x040035DB RID: 13787
		[JsonProperty(PropertyName = "Macros")]
		public Dictionary<string, MacroShortcut> MacroShortcuts = new Dictionary<string, MacroShortcut>();

		// Token: 0x040035DC RID: 13788
		[JsonProperty(PropertyName = "Keybinds")]
		public Dictionary<string, KeybindShortcut> KeybindShortcuts = new Dictionary<string, KeybindShortcut>();
	}
}
