using System;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000068 RID: 104
	public struct OnDisplaySettingsUpdatedCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x00007005 File Offset: 0x00005205
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x0000700D File Offset: 0x0000520D
		public object ClientData { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00007016 File Offset: 0x00005216
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x0000701E File Offset: 0x0000521E
		public bool IsVisible { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00007027 File Offset: 0x00005227
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x0000702F File Offset: 0x0000522F
		public bool IsExclusiveInput { get; set; }

		// Token: 0x060004DA RID: 1242 RVA: 0x00007038 File Offset: 0x00005238
		public Result? GetResultCode()
		{
			return null;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00007053 File Offset: 0x00005253
		internal void Set(ref OnDisplaySettingsUpdatedCallbackInfoInternal other)
		{
			this.ClientData = other.ClientData;
			this.IsVisible = other.IsVisible;
			this.IsExclusiveInput = other.IsExclusiveInput;
		}
	}
}
