using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005EC RID: 1516
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LinkAccountCallbackInfoInternal : ICallbackInfoInternal, IGettable<LinkAccountCallbackInfo>, ISettable<LinkAccountCallbackInfo>, IDisposable
	{
		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06002759 RID: 10073 RVA: 0x0003A3A4 File Offset: 0x000385A4
		// (set) Token: 0x0600275A RID: 10074 RVA: 0x0003A3BC File Offset: 0x000385BC
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

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600275B RID: 10075 RVA: 0x0003A3C8 File Offset: 0x000385C8
		// (set) Token: 0x0600275C RID: 10076 RVA: 0x0003A3E9 File Offset: 0x000385E9
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

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x0600275D RID: 10077 RVA: 0x0003A3FC File Offset: 0x000385FC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x0003A414 File Offset: 0x00038614
		// (set) Token: 0x0600275F RID: 10079 RVA: 0x0003A435 File Offset: 0x00038635
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

		// Token: 0x06002760 RID: 10080 RVA: 0x0003A445 File Offset: 0x00038645
		public void Set(ref LinkAccountCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x0003A470 File Offset: 0x00038670
		public void Set(ref LinkAccountCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x0003A4C9 File Offset: 0x000386C9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x0003A4E4 File Offset: 0x000386E4
		public void Get(out LinkAccountCallbackInfo output)
		{
			output = default(LinkAccountCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001112 RID: 4370
		private Result m_ResultCode;

		// Token: 0x04001113 RID: 4371
		private IntPtr m_ClientData;

		// Token: 0x04001114 RID: 4372
		private IntPtr m_LocalUserId;
	}
}
