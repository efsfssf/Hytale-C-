using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x0200019C RID: 412
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CreatePlayerSanctionAppealCallbackInfoInternal : ICallbackInfoInternal, IGettable<CreatePlayerSanctionAppealCallbackInfo>, ISettable<CreatePlayerSanctionAppealCallbackInfo>, IDisposable
	{
		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x000116F8 File Offset: 0x0000F8F8
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x00011710 File Offset: 0x0000F910
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x0001171C File Offset: 0x0000F91C
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x0001173D File Offset: 0x0000F93D
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00011750 File Offset: 0x0000F950
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00011768 File Offset: 0x0000F968
		// (set) Token: 0x06000BFC RID: 3068 RVA: 0x00011789 File Offset: 0x0000F989
		public Utf8String ReferenceId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ReferenceId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ReferenceId);
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00011799 File Offset: 0x0000F999
		public void Set(ref CreatePlayerSanctionAppealCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.ReferenceId = other.ReferenceId;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x000117C4 File Offset: 0x0000F9C4
		public void Set(ref CreatePlayerSanctionAppealCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.ReferenceId = other.Value.ReferenceId;
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0001181D File Offset: 0x0000FA1D
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_ReferenceId);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00011838 File Offset: 0x0000FA38
		public void Get(out CreatePlayerSanctionAppealCallbackInfo output)
		{
			output = default(CreatePlayerSanctionAppealCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000579 RID: 1401
		private Result m_ResultCode;

		// Token: 0x0400057A RID: 1402
		private IntPtr m_ClientData;

		// Token: 0x0400057B RID: 1403
		private IntPtr m_ReferenceId;
	}
}
