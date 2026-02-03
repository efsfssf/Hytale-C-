using System;
using Coherent.UI.Binding;
using HytaleClient.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.Machinima.Settings
{
	// Token: 0x0200091A RID: 2330
	[CoherentType]
	internal abstract class KeyframeSetting<T> : IKeyframeSetting
	{
		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x0600471B RID: 18203 RVA: 0x0010CE3D File Offset: 0x0010B03D
		[CoherentProperty("name")]
		public string Name { get; }

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x0600471C RID: 18204 RVA: 0x0010CE45 File Offset: 0x0010B045
		// (set) Token: 0x0600471D RID: 18205 RVA: 0x0010CE4D File Offset: 0x0010B04D
		public T Value { get; set; }

		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x0600471E RID: 18206 RVA: 0x0010CE58 File Offset: 0x0010B058
		[CoherentProperty("value")]
		public string ValueStringified
		{
			get
			{
				JsonSerializer jsonSerializer = new JsonSerializer();
				jsonSerializer.Converters.Add(new StringEnumConverter());
				return this.ToJsonObject(jsonSerializer).ToString();
			}
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x0600471F RID: 18207 RVA: 0x0010CE8D File Offset: 0x0010B08D
		[CoherentProperty("type")]
		public string ValueTypeName
		{
			get
			{
				return this.ValueType.ToString();
			}
		}

		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x06004720 RID: 18208 RVA: 0x0010CE9A File Offset: 0x0010B09A
		public Type ValueType
		{
			get
			{
				return typeof(T);
			}
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x0010CEA6 File Offset: 0x0010B0A6
		public KeyframeSetting(string name, T value)
		{
			this.Name = name;
			this.Value = value;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x0010CEC0 File Offset: 0x0010B0C0
		public virtual JObject ToJsonObject(JsonSerializer serializer)
		{
			return JObject.FromObject(this.Value, serializer);
		}

		// Token: 0x06004723 RID: 18211
		public abstract IKeyframeSetting Clone();

		// Token: 0x06004724 RID: 18212 RVA: 0x0010CEE8 File Offset: 0x0010B0E8
		public static IKeyframeSetting ConvertJsonObject(string keyName, JObject jsonData)
		{
			IKeyframeSetting result;
			if (!(keyName == "Position"))
			{
				if (!(keyName == "Rotation"))
				{
					if (!(keyName == "Look"))
					{
						if (!(keyName == "FieldOfView"))
						{
							if (!(keyName == "Curve"))
							{
								if (!(keyName == "Easing"))
								{
									result = null;
								}
								else
								{
									result = new EasingSetting(jsonData["Value"].ToObject<Easing.EasingType>());
								}
							}
							else
							{
								result = new CurveSetting(jsonData["Value"].ToObject<Vector3[]>());
							}
						}
						else
						{
							result = new FieldOfViewSetting(jsonData["Value"].ToObject<float>());
						}
					}
					else
					{
						result = new LookSetting(jsonData.ToObject<Vector3>());
					}
				}
				else
				{
					result = new RotationSetting(jsonData.ToObject<Vector3>());
				}
			}
			else
			{
				result = new PositionSetting(jsonData.ToObject<Vector3>());
			}
			return result;
		}
	}
}
