using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006E7 RID: 1767
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnClientIntegrityViolatedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnClientIntegrityViolatedCallbackInfo>, ISettable<OnClientIntegrityViolatedCallbackInfo>, IDisposable
	{
		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06002D9C RID: 11676 RVA: 0x00043810 File Offset: 0x00041A10
		// (set) Token: 0x06002D9D RID: 11677 RVA: 0x00043831 File Offset: 0x00041A31
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

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06002D9E RID: 11678 RVA: 0x00043844 File Offset: 0x00041A44
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06002D9F RID: 11679 RVA: 0x0004385C File Offset: 0x00041A5C
		// (set) Token: 0x06002DA0 RID: 11680 RVA: 0x00043874 File Offset: 0x00041A74
		public AntiCheatClientViolationType ViolationType
		{
			get
			{
				return this.m_ViolationType;
			}
			set
			{
				this.m_ViolationType = value;
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06002DA1 RID: 11681 RVA: 0x00043880 File Offset: 0x00041A80
		// (set) Token: 0x06002DA2 RID: 11682 RVA: 0x000438A1 File Offset: 0x00041AA1
		public Utf8String ViolationMessage
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ViolationMessage, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ViolationMessage);
			}
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000438B1 File Offset: 0x00041AB1
		public void Set(ref OnClientIntegrityViolatedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.ViolationType = other.ViolationType;
			this.ViolationMessage = other.ViolationMessage;
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000438DC File Offset: 0x00041ADC
		public void Set(ref OnClientIntegrityViolatedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.ViolationType = other.Value.ViolationType;
				this.ViolationMessage = other.Value.ViolationMessage;
			}
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x00043935 File Offset: 0x00041B35
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_ViolationMessage);
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x00043950 File Offset: 0x00041B50
		public void Get(out OnClientIntegrityViolatedCallbackInfo output)
		{
			output = default(OnClientIntegrityViolatedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400142C RID: 5164
		private IntPtr m_ClientData;

		// Token: 0x0400142D RID: 5165
		private AntiCheatClientViolationType m_ViolationType;

		// Token: 0x0400142E RID: 5166
		private IntPtr m_ViolationMessage;
	}
}
