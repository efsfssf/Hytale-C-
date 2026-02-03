using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200010F RID: 271
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinSessionAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinSessionAcceptedCallbackInfo>, ISettable<JoinSessionAcceptedCallbackInfo>, IDisposable
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0000CC3C File Offset: 0x0000AE3C
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x0000CC5D File Offset: 0x0000AE5D
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

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0000CC70 File Offset: 0x0000AE70
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0000CC88 File Offset: 0x0000AE88
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x0000CCA9 File Offset: 0x0000AEA9
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0000CCBC File Offset: 0x0000AEBC
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x0000CCD4 File Offset: 0x0000AED4
		public ulong UiEventId
		{
			get
			{
				return this.m_UiEventId;
			}
			set
			{
				this.m_UiEventId = value;
			}
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0000CCDE File Offset: 0x0000AEDE
		public void Set(ref JoinSessionAcceptedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.UiEventId = other.UiEventId;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0000CD08 File Offset: 0x0000AF08
		public void Set(ref JoinSessionAcceptedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.UiEventId = other.Value.UiEventId;
			}
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0000CD61 File Offset: 0x0000AF61
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0000CD7C File Offset: 0x0000AF7C
		public void Get(out JoinSessionAcceptedCallbackInfo output)
		{
			output = default(JoinSessionAcceptedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400042D RID: 1069
		private IntPtr m_ClientData;

		// Token: 0x0400042E RID: 1070
		private IntPtr m_LocalUserId;

		// Token: 0x0400042F RID: 1071
		private ulong m_UiEventId;
	}
}
