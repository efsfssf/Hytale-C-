using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x020005F4 RID: 1524
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct LoginStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<LoginStatusChangedCallbackInfo>, ISettable<LoginStatusChangedCallbackInfo>, IDisposable
	{
		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06002797 RID: 10135 RVA: 0x0003A978 File Offset: 0x00038B78
		// (set) Token: 0x06002798 RID: 10136 RVA: 0x0003A999 File Offset: 0x00038B99
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

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06002799 RID: 10137 RVA: 0x0003A9AC File Offset: 0x00038BAC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x0003A9C4 File Offset: 0x00038BC4
		// (set) Token: 0x0600279B RID: 10139 RVA: 0x0003A9E5 File Offset: 0x00038BE5
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

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x0600279C RID: 10140 RVA: 0x0003A9F8 File Offset: 0x00038BF8
		// (set) Token: 0x0600279D RID: 10141 RVA: 0x0003AA10 File Offset: 0x00038C10
		public LoginStatus PreviousStatus
		{
			get
			{
				return this.m_PreviousStatus;
			}
			set
			{
				this.m_PreviousStatus = value;
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x0600279E RID: 10142 RVA: 0x0003AA1C File Offset: 0x00038C1C
		// (set) Token: 0x0600279F RID: 10143 RVA: 0x0003AA34 File Offset: 0x00038C34
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

		// Token: 0x060027A0 RID: 10144 RVA: 0x0003AA3E File Offset: 0x00038C3E
		public void Set(ref LoginStatusChangedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PreviousStatus = other.PreviousStatus;
			this.CurrentStatus = other.CurrentStatus;
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x0003AA78 File Offset: 0x00038C78
		public void Set(ref LoginStatusChangedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.PreviousStatus = other.Value.PreviousStatus;
				this.CurrentStatus = other.Value.CurrentStatus;
			}
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x0003AAE6 File Offset: 0x00038CE6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x0003AB01 File Offset: 0x00038D01
		public void Get(out LoginStatusChangedCallbackInfo output)
		{
			output = default(LoginStatusChangedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400112B RID: 4395
		private IntPtr m_ClientData;

		// Token: 0x0400112C RID: 4396
		private IntPtr m_LocalUserId;

		// Token: 0x0400112D RID: 4397
		private LoginStatus m_PreviousStatus;

		// Token: 0x0400112E RID: 4398
		private LoginStatus m_CurrentStatus;
	}
}
