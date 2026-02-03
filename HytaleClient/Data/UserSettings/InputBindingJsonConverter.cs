using System;
using HytaleClient.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SDL2;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD3 RID: 2771
	internal class InputBindingJsonConverter : JsonConverter
	{
		// Token: 0x06005770 RID: 22384 RVA: 0x001A7D78 File Offset: 0x001A5F78
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			InputBinding inputBinding = (InputBinding)value;
			JObject jobject = new JObject();
			bool flag = inputBinding.Type == InputBinding.BindingType.Keycode;
			if (flag)
			{
				jobject.Add("Keycode", JToken.FromObject(inputBinding.Keycode.Value));
			}
			else
			{
				jobject.Add("MouseButton", JToken.FromObject(inputBinding.MouseButton));
			}
			jobject.WriteTo(writer, Array.Empty<JsonConverter>());
		}

		// Token: 0x06005771 RID: 22385 RVA: 0x001A7DF0 File Offset: 0x001A5FF0
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jobject = serializer.Deserialize(reader) as JObject;
			bool flag = jobject == null;
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = jobject["Keycode"] != null;
				InputBinding inputBinding;
				if (flag2)
				{
					inputBinding = new InputBinding(null)
					{
						Keycode = new SDL.SDL_Keycode?((SDL.SDL_Keycode)((int)jobject["Keycode"]))
					};
				}
				else
				{
					bool flag3 = jobject["keycode"] != null;
					if (flag3)
					{
						inputBinding = new InputBinding(null)
						{
							Keycode = new SDL.SDL_Keycode?((SDL.SDL_Keycode)((int)jobject["keycode"]))
						};
					}
					else
					{
						bool flag4 = jobject["MouseButton"] != null;
						if (flag4)
						{
							inputBinding = new InputBinding(null)
							{
								MouseButton = new Input.MouseButton?((Input.MouseButton)((int)jobject["MouseButton"]))
							};
						}
						else
						{
							inputBinding = new InputBinding(null)
							{
								MouseButton = new Input.MouseButton?((Input.MouseButton)((int)jobject["mouseButton"]))
							};
						}
					}
				}
				result = inputBinding;
			}
			return result;
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x001A7EF4 File Offset: 0x001A60F4
		public override bool CanConvert(Type objectType)
		{
			return typeof(InputBinding).IsAssignableFrom(objectType);
		}
	}
}
