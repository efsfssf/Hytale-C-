using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000648 RID: 1608
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LoginStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginStatusChangedCallbackInfo>, ISettable<LoginStatusChangedCallbackInfo>, IDisposable
	{
		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x060029B2 RID: 10674 RVA: 0x0003D834 File Offset: 0x0003BA34
		// (set) Token: 0x060029B3 RID: 10675 RVA: 0x0003D855 File Offset: 0x0003BA55
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

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x0003D868 File Offset: 0x0003BA68
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x0003D880 File Offset: 0x0003BA80
		// (set) Token: 0x060029B6 RID: 10678 RVA: 0x0003D8A1 File Offset: 0x0003BAA1
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

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x0003D8B4 File Offset: 0x0003BAB4
		// (set) Token: 0x060029B8 RID: 10680 RVA: 0x0003D8CC File Offset: 0x0003BACC
		public LoginStatus PrevStatus
		{
			get
			{
				return this.m_PrevStatus;
			}
			set
			{
				this.m_PrevStatus = value;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x060029B9 RID: 10681 RVA: 0x0003D8D8 File Offset: 0x0003BAD8
		// (set) Token: 0x060029BA RID: 10682 RVA: 0x0003D8F0 File Offset: 0x0003BAF0
		public LoginStatus CurrentStatus
		{
			get
			{
				return this.m_CurrentStatus;
			}
			set
			{
				this.m_CurrentStatus = value;
			}
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x0003D8FA File Offset: 0x0003BAFA
		public void Set(ref LoginStatusChangedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PrevStatus = other.PrevStatus;
			this.CurrentStatus = other.CurrentStatus;
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x0003D934 File Offset: 0x0003BB34
		public void Set(ref LoginStatusChangedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.PrevStatus = other.Value.PrevStatus;
				this.CurrentStatus = other.Value.CurrentStatus;
			}
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x0003D9A2 File Offset: 0x0003BBA2
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x0003D9BD File Offset: 0x0003BBBD
		public void Get(out LoginStatusChangedCallbackInfo output)
		{
			output = default(LoginStatusChangedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040011FD RID: 4605
		private IntPtr m_ClientData;

		// Token: 0x040011FE RID: 4606
		private IntPtr m_LocalUserId;

		// Token: 0x040011FF RID: 4607
		private LoginStatus m_PrevStatus;

		// Token: 0x04001200 RID: 4608
		private LoginStatus m_CurrentStatus;
	}
}
