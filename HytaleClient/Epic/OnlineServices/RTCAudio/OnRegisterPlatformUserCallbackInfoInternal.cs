using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200023B RID: 571
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnRegisterPlatformUserCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnRegisterPlatformUserCallbackInfo>, ISettable<OnRegisterPlatformUserCallbackInfo>, IDisposable
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x00017214 File Offset: 0x00015414
		// (set) Token: 0x0600100A RID: 4106 RVA: 0x0001722C File Offset: 0x0001542C
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

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x00017238 File Offset: 0x00015438
		// (set) Token: 0x0600100C RID: 4108 RVA: 0x00017259 File Offset: 0x00015459
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

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600100D RID: 4109 RVA: 0x0001726C File Offset: 0x0001546C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x00017284 File Offset: 0x00015484
		// (set) Token: 0x0600100F RID: 4111 RVA: 0x000172A5 File Offset: 0x000154A5
		public Utf8String PlatformUserId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_PlatformUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_PlatformUserId);
			}
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x000172B5 File Offset: 0x000154B5
		public void Set(ref OnRegisterPlatformUserCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.PlatformUserId = other.PlatformUserId;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x000172E0 File Offset: 0x000154E0
		public void Set(ref OnRegisterPlatformUserCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.PlatformUserId = other.Value.PlatformUserId;
			}
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00017339 File Offset: 0x00015539
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_PlatformUserId);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00017354 File Offset: 0x00015554
		public void Get(out OnRegisterPlatformUserCallbackInfo output)
		{
			output = default(OnRegisterPlatformUserCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400070F RID: 1807
		private Result m_ResultCode;

		// Token: 0x04000710 RID: 1808
		private IntPtr m_ClientData;

		// Token: 0x04000711 RID: 1809
		private IntPtr m_PlatformUserId;
	}
}
