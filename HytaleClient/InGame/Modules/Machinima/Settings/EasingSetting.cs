using System;
using HytaleClient.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.Machinima.Settings
{
	// Token: 0x02000917 RID: 2327
	internal class EasingSetting : KeyframeSetting<Easing.EasingType>
	{
		// Token: 0x0600470F RID: 18191 RVA: 0x0010CD45 File Offset: 0x0010AF45
		public EasingSetting(Easing.EasingType easing) : base("Easing", easing)
		{
			base.Value = easing;
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x0010CD60 File Offset: 0x0010AF60
		public override JObject ToJsonObject(JsonSerializer serializer)
		{
			JObject jobject = new JObject();
			jobject.Add("Value", JToken.FromObject(base.Value, serializer));
			return jobject;
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x0010CD94 File Offset: 0x0010AF94
		public override IKeyframeSetting Clone()
		{
			return new EasingSetting(base.Value);
		}

		// Token: 0x040023B7 RID: 9143
		public const string KEY_NAME = "Easing";

		// Token: 0x040023B8 RID: 9144
		public static KeyframeSettingType KeyframeType = KeyframeSettingType.Easing;
	}
}
