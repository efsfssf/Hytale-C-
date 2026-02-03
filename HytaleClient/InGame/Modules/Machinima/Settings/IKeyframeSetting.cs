using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.Machinima.Settings
{
	// Token: 0x02000919 RID: 2329
	internal interface IKeyframeSetting
	{
		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x06004717 RID: 18199
		string Name { get; }

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x06004718 RID: 18200
		Type ValueType { get; }

		// Token: 0x06004719 RID: 18201
		JObject ToJsonObject(JsonSerializer serializer);

		// Token: 0x0600471A RID: 18202
		IKeyframeSetting Clone();
	}
}
