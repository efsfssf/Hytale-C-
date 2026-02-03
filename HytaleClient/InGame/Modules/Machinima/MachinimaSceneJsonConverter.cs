using System;
using HytaleClient.InGame.Modules.Machinima.Actors;
using HytaleClient.InGame.Modules.Machinima.Events;
using HytaleClient.InGame.Modules.Machinima.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.Machinima
{
	// Token: 0x0200090F RID: 2319
	internal class MachinimaSceneJsonConverter : JsonConverter
	{
		// Token: 0x060046A6 RID: 18086 RVA: 0x00109150 File Offset: 0x00107350
		public MachinimaSceneJsonConverter(GameInstance gameInstance, MachinimaModule machinimaModule)
		{
			this._gameInstance = gameInstance;
			this._machinimaModule = machinimaModule;
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x00109168 File Offset: 0x00107368
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			bool flag = value is IKeyframeSetting;
			if (flag)
			{
				((IKeyframeSetting)value).ToJsonObject(serializer).WriteTo(writer, Array.Empty<JsonConverter>());
			}
			bool flag2 = value is KeyframeEvent;
			if (flag2)
			{
				((KeyframeEvent)value).ToJsonObject().WriteTo(writer, Array.Empty<JsonConverter>());
			}
			bool flag3 = value is SceneActor;
			if (flag3)
			{
				((SceneActor)value).WriteToJsonObject(serializer, writer);
			}
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x001091E0 File Offset: 0x001073E0
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = typeof(SceneActor).IsAssignableFrom(objectType);
			object result;
			if (flag)
			{
				SerializedSceneObject actorData = (SerializedSceneObject)serializer.Deserialize(reader, typeof(SerializedSceneObject));
				result = SceneActor.ConvertJsonObject(this._gameInstance, actorData);
			}
			else
			{
				bool flag2 = objectType == typeof(IKeyframeSetting);
				if (flag2)
				{
					JObject jsonData = (JObject)serializer.Deserialize(reader);
					string keyName = reader.Path.Substring(reader.Path.LastIndexOf(".") + 1);
					result = KeyframeSetting<object>.ConvertJsonObject(keyName, jsonData);
				}
				else
				{
					bool flag3 = objectType == typeof(KeyframeEvent);
					if (flag3)
					{
						JObject jsonData2 = (JObject)serializer.Deserialize(reader);
						result = KeyframeEvent.ConvertJsonObject(jsonData2);
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x001092B4 File Offset: 0x001074B4
		public override bool CanConvert(Type objectType)
		{
			return typeof(SceneActor).IsAssignableFrom(objectType) || typeof(KeyframeEvent).IsAssignableFrom(objectType) || typeof(IKeyframeSetting).IsAssignableFrom(objectType);
		}

		// Token: 0x0400238C RID: 9100
		private GameInstance _gameInstance;

		// Token: 0x0400238D RID: 9101
		private MachinimaModule _machinimaModule;
	}
}
