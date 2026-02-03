using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200055A RID: 1370
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryOffersCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOffersCallbackInfo>, ISettable<QueryOffersCallbackInfo>, IDisposable
	{
		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x060023B1 RID: 9137 RVA: 0x000347B8 File Offset: 0x000329B8
		// (set) Token: 0x060023B2 RID: 9138 RVA: 0x000347D0 File Offset: 0x000329D0
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

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x060023B3 RID: 9139 RVA: 0x000347DC File Offset: 0x000329DC
		// (set) Token: 0x060023B4 RID: 9140 RVA: 0x000347FD File Offset: 0x000329FD
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

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x060023B5 RID: 9141 RVA: 0x00034810 File Offset: 0x00032A10
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060023B6 RID: 9142 RVA: 0x00034828 File Offset: 0x00032A28
		// (set) Token: 0x060023B7 RID: 9143 RVA: 0x00034849 File Offset: 0x00032A49
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x00034859 File Offset: 0x00032A59
		public void Set(ref QueryOffersCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x00034884 File Offset: 0x00032A84
		public void Set(ref QueryOffersCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000348DD File Offset: 0x00032ADD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000348F8 File Offset: 0x00032AF8
		public void Get(out QueryOffersCallbackInfo output)
		{
			output = default(QueryOffersCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000FA6 RID: 4006
		private Result m_ResultCode;

		// Token: 0x04000FA7 RID: 4007
		private IntPtr m_ClientData;

		// Token: 0x04000FA8 RID: 4008
		private IntPtr m_LocalUserId;
	}
}
