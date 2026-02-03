using System;
using HytaleClient.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.Machinima.Settings
{
	// Token: 0x02000916 RID: 2326
	internal class CurveSetting : KeyframeSetting<Vector3[]>
	{
		// Token: 0x0600470B RID: 18187 RVA: 0x0010CCE0 File Offset: 0x0010AEE0
		public CurveSetting(Vector3[] positions) : base("Curve", positions)
		{
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x0010CCF0 File Offset: 0x0010AEF0
		public override JObject ToJsonObject(JsonSerializer serializer)
		{
			JObject jobject = new JObject();
			jobject.Add("Value", JArray.FromObject(base.Value, serializer));
			return jobject;
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x0010CD20 File Offset: 0x0010AF20
		public override IKeyframeSetting Clone()
		{
			return new CurveSetting(base.Value);
		}

		// Token: 0x040023B5 RID: 9141
		public const string KEY_NAME = "Curve";

		// Token: 0x040023B6 RID: 9142
		public static KeyframeSettingType KeyframeType = KeyframeSettingType.Curve;
	}
}
