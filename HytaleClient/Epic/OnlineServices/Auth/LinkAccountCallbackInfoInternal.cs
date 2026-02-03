using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x0200063D RID: 1597
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LinkAccountCallbackInfoInternal : ICallbackInfoInternal, IGettable<LinkAccountCallbackInfo>, ISettable<LinkAccountCallbackInfo>, IDisposable
	{
		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x0600295E RID: 10590 RVA: 0x0003CF74 File Offset: 0x0003B174
		// (set) Token: 0x0600295F RID: 10591 RVA: 0x0003CF8C File Offset: 0x0003B18C
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

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06002960 RID: 10592 RVA: 0x0003CF98 File Offset: 0x0003B198
		// (set) Token: 0x06002961 RID: 10593 RVA: 0x0003CFB9 File Offset: 0x0003B1B9
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

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06002962 RID: 10594 RVA: 0x0003CFCC File Offset: 0x0003B1CC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06002963 RID: 10595 RVA: 0x0003CFE4 File Offset: 0x0003B1E4
		// (set) Token: 0x06002964 RID: 10596 RVA: 0x0003D005 File Offset: 0x0003B205
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

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06002965 RID: 10597 RVA: 0x0003D018 File Offset: 0x0003B218
		// (set) Token: 0x06002966 RID: 10598 RVA: 0x0003D039 File Offset: 0x0003B239
		public PinGrantInfo? PinGrantInfo
		{
			get
			{
				PinGrantInfo? result;
				Helper.Get<PinGrantInfoInternal, PinGrantInfo>(this.m_PinGrantInfo, out result);
				return result;
			}
			set
			{
				Helper.Set<PinGrantInfo, PinGrantInfoInternal>(ref value, ref this.m_PinGrantInfo);
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06002967 RID: 10599 RVA: 0x0003D04C File Offset: 0x0003B24C
		// (set) Token: 0x06002968 RID: 10600 RVA: 0x0003D06D File Offset: 0x0003B26D
		public EpicAccountId SelectedAccountId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_SelectedAccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SelectedAccountId);
			}
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x0003D080 File Offset: 0x0003B280
		public void Set(ref LinkAccountCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PinGrantInfo = other.PinGrantInfo;
			this.SelectedAccountId = other.SelectedAccountId;
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x0003D0D0 File Offset: 0x0003B2D0
		public void Set(ref LinkAccountCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.PinGrantInfo = other.Value.PinGrantInfo;
				this.SelectedAccountId = other.Value.SelectedAccountId;
			}
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x0003D153 File Offset: 0x0003B353
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_PinGrantInfo);
			Helper.Dispose(ref this.m_SelectedAccountId);
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x0003D186 File Offset: 0x0003B386
		public void Get(out LinkAccountCallbackInfo output)
		{
			output = default(LinkAccountCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040011C9 RID: 4553
		private Result m_ResultCode;

		// Token: 0x040011CA RID: 4554
		private IntPtr m_ClientData;

		// Token: 0x040011CB RID: 4555
		private IntPtr m_LocalUserId;

		// Token: 0x040011CC RID: 4556
		private IntPtr m_PinGrantInfo;

		// Token: 0x040011CD RID: 4557
		private IntPtr m_SelectedAccountId;
	}
}
