using System;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200055D RID: 1373
	public struct QueryOwnershipBySandboxIdsCallbackInfo : ICallbackInfo
	{
		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060023C5 RID: 9157 RVA: 0x000349D6 File Offset: 0x00032BD6
		// (set) Token: 0x060023C6 RID: 9158 RVA: 0x000349DE File Offset: 0x00032BDE
		public Result ResultCode { get; set; }

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x060023C7 RID: 9159 RVA: 0x000349E7 File Offset: 0x00032BE7
		// (set) Token: 0x060023C8 RID: 9160 RVA: 0x000349EF File Offset: 0x00032BEF
		public object ClientData { get; set; }

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x060023C9 RID: 9161 RVA: 0x000349F8 File Offset: 0x00032BF8
		// (set) Token: 0x060023CA RID: 9162 RVA: 0x00034A00 File Offset: 0x00032C00
		public EpicAccountId LocalUserId { get; set; }

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x060023CB RID: 9163 RVA: 0x00034A09 File Offset: 0x00032C09
		// (set) Token: 0x060023CC RID: 9164 RVA: 0x00034A11 File Offset: 0x00032C11
		public SandboxIdItemOwnership[] SandboxIdItemOwnerships { get; set; }

		// Token: 0x060023CD RID: 9165 RVA: 0x00034A1C File Offset: 0x00032C1C
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x00034A39 File Offset: 0x00032C39
		internal void Set(ref QueryOwnershipBySandboxIdsCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.SandboxIdItemOwnerships = other.SandboxIdItemOwnerships;
		}
	}
}
