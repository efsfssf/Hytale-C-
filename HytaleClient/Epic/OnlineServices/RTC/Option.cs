using System;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001CC RID: 460
	public struct Option
	{
		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x000132BE File Offset: 0x000114BE
		// (set) Token: 0x06000D3F RID: 3391 RVA: 0x000132C6 File Offset: 0x000114C6
		public Utf8String Key { get; set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x000132CF File Offset: 0x000114CF
		// (set) Token: 0x06000D41 RID: 3393 RVA: 0x000132D7 File Offset: 0x000114D7
		public Utf8String Value { get; set; }

		// Token: 0x06000D42 RID: 3394 RVA: 0x000132E0 File Offset: 0x000114E0
		internal void Set(ref OptionInternal other)
		{
			this.Key = other.Key;
			this.Value = other.Value;
		}
	}
}
