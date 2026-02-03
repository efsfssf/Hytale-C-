using System;
using HytaleClient.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.Machinima.Settings
{
	// Token: 0x02000918 RID: 2328
	internal class FieldOfViewSetting : KeyframeSetting<float>
	{
		// Token: 0x06004713 RID: 18195 RVA: 0x0010CDB9 File Offset: 0x0010AFB9
		public FieldOfViewSetting(float fov) : base("FieldOfView", fov)
		{
			base.Value = MathHelper.Clamp(base.Value, 1f, 179f);
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x0010CDE8 File Offset: 0x0010AFE8
		public override JObject ToJsonObject(JsonSerializer serializer)
		{
			JObject jobject = new JObject();
			jobject.Add("Value", base.Value);
			return jobject;
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x0010CE18 File Offset: 0x0010B018
		public override IKeyframeSetting Clone()
		{
			return new FieldOfViewSetting(base.Value);
		}

		// Token: 0x040023B9 RID: 9145
		public const string KEY_NAME = "FieldOfView";

		// Token: 0x040023BA RID: 9146
		public static KeyframeSettingType KeyframeType = KeyframeSettingType.FieldOfView;
	}
}
