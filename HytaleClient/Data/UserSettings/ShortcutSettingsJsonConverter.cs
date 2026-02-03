using System;
using HytaleClient.InGame.Modules.Shortcuts;
using Newtonsoft.Json;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD9 RID: 2777
	internal class ShortcutSettingsJsonConverter : JsonConverter
	{
		// Token: 0x0600578E RID: 22414 RVA: 0x001A96A4 File Offset: 0x001A78A4
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			bool flag = value is Shortcut;
			if (flag)
			{
				((Shortcut)value).ToJsonObject().WriteTo(writer, Array.Empty<JsonConverter>());
			}
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x001A96D8 File Offset: 0x001A78D8
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			SerializedShortcutSetting serializedShortcutSetting = (SerializedShortcutSetting)serializer.Deserialize(reader, typeof(SerializedShortcutSetting));
			bool flag = serializedShortcutSetting.name == null && serializedShortcutSetting.command == null;
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = objectType == typeof(MacroShortcut);
				if (flag2)
				{
					result = new MacroShortcut(serializedShortcutSetting.name, serializedShortcutSetting.command);
				}
				else
				{
					try
					{
						bool flag3 = objectType == typeof(KeybindShortcut);
						if (flag3)
						{
							return new KeybindShortcut(serializedShortcutSetting.name, serializedShortcutSetting.command);
						}
					}
					catch (Exception)
					{
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x001A978C File Offset: 0x001A798C
		public override bool CanConvert(Type objectType)
		{
			return typeof(MacroShortcut).IsAssignableFrom(objectType) || typeof(KeybindShortcut).IsAssignableFrom(objectType);
		}
	}
}
