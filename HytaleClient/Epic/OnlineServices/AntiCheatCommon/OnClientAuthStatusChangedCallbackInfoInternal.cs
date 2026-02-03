using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C0 RID: 1728
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnClientAuthStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnClientAuthStatusChangedCallbackInfo>, ISettable<OnClientAuthStatusChangedCallbackInfo>, IDisposable
	{
		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x000423F0 File Offset: 0x000405F0
		// (set) Token: 0x06002CD7 RID: 11479 RVA: 0x00042411 File Offset: 0x00040611
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

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06002CD8 RID: 11480 RVA: 0x00042424 File Offset: 0x00040624
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06002CD9 RID: 11481 RVA: 0x0004243C File Offset: 0x0004063C
		// (set) Token: 0x06002CDA RID: 11482 RVA: 0x00042454 File Offset: 0x00040654
		public IntPtr ClientHandle
		{
			get
			{
				return this.m_ClientHandle;
			}
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06002CDB RID: 11483 RVA: 0x00042460 File Offset: 0x00040660
		// (set) Token: 0x06002CDC RID: 11484 RVA: 0x00042478 File Offset: 0x00040678
		public AntiCheatCommonClientAuthStatus ClientAuthStatus
		{
			get
			{
				return this.m_ClientAuthStatus;
			}
			set
			{
				this.m_ClientAuthStatus = value;
			}
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x00042482 File Offset: 0x00040682
		public void Set(ref OnClientAuthStatusChangedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.ClientHandle = other.ClientHandle;
			this.ClientAuthStatus = other.ClientAuthStatus;
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000424AC File Offset: 0x000406AC
		public void Set(ref OnClientAuthStatusChangedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.ClientHandle = other.Value.ClientHandle;
				this.ClientAuthStatus = other.Value.ClientAuthStatus;
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x00042505 File Offset: 0x00040705
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_ClientHandle);
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x00042520 File Offset: 0x00040720
		public void Get(out OnClientAuthStatusChangedCallbackInfo output)
		{
			output = default(OnClientAuthStatusChangedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040013BD RID: 5053
		private IntPtr m_ClientData;

		// Token: 0x040013BE RID: 5054
		private IntPtr m_ClientHandle;

		// Token: 0x040013BF RID: 5055
		private AntiCheatCommonClientAuthStatus m_ClientAuthStatus;
	}
}
