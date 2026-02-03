using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000247 RID: 583
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnUnregisterPlatformUserCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnUnregisterPlatformUserCallbackInfo>, ISettable<OnUnregisterPlatformUserCallbackInfo>, IDisposable
	{
		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x00017784 File Offset: 0x00015984
		// (set) Token: 0x0600105B RID: 4187 RVA: 0x0001779C File Offset: 0x0001599C
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

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x000177A8 File Offset: 0x000159A8
		// (set) Token: 0x0600105D RID: 4189 RVA: 0x000177C9 File Offset: 0x000159C9
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

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x000177DC File Offset: 0x000159DC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x000177F4 File Offset: 0x000159F4
		// (set) Token: 0x06001060 RID: 4192 RVA: 0x00017815 File Offset: 0x00015A15
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

		// Token: 0x06001061 RID: 4193 RVA: 0x00017825 File Offset: 0x00015A25
		public void Set(ref OnUnregisterPlatformUserCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.PlatformUserId = other.PlatformUserId;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00017850 File Offset: 0x00015A50
		public void Set(ref OnUnregisterPlatformUserCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.PlatformUserId = other.Value.PlatformUserId;
			}
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x000178A9 File Offset: 0x00015AA9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_PlatformUserId);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000178C4 File Offset: 0x00015AC4
		public void Get(out OnUnregisterPlatformUserCallbackInfo output)
		{
			output = default(OnUnregisterPlatformUserCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000721 RID: 1825
		private Result m_ResultCode;

		// Token: 0x04000722 RID: 1826
		private IntPtr m_ClientData;

		// Token: 0x04000723 RID: 1827
		private IntPtr m_PlatformUserId;
	}
}
