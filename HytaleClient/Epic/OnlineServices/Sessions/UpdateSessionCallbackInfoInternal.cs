using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000192 RID: 402
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateSessionCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateSessionCallbackInfo>, ISettable<UpdateSessionCallbackInfo>, IDisposable
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0001130C File Offset: 0x0000F50C
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x00011324 File Offset: 0x0000F524
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

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x00011330 File Offset: 0x0000F530
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x00011351 File Offset: 0x0000F551
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

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00011364 File Offset: 0x0000F564
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x0001137C File Offset: 0x0000F57C
		// (set) Token: 0x06000BCA RID: 3018 RVA: 0x0001139D File Offset: 0x0000F59D
		public Utf8String SessionName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SessionName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x000113B0 File Offset: 0x0000F5B0
		// (set) Token: 0x06000BCC RID: 3020 RVA: 0x000113D1 File Offset: 0x0000F5D1
		public Utf8String SessionId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SessionId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SessionId);
			}
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x000113E1 File Offset: 0x0000F5E1
		public void Set(ref UpdateSessionCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.SessionName = other.SessionName;
			this.SessionId = other.SessionId;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00011418 File Offset: 0x0000F618
		public void Set(ref UpdateSessionCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.SessionName = other.Value.SessionName;
				this.SessionId = other.Value.SessionId;
			}
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00011486 File Offset: 0x0000F686
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_SessionName);
			Helper.Dispose(ref this.m_SessionId);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x000114AD File Offset: 0x0000F6AD
		public void Get(out UpdateSessionCallbackInfo output)
		{
			output = default(UpdateSessionCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000567 RID: 1383
		private Result m_ResultCode;

		// Token: 0x04000568 RID: 1384
		private IntPtr m_ClientData;

		// Token: 0x04000569 RID: 1385
		private IntPtr m_SessionName;

		// Token: 0x0400056A RID: 1386
		private IntPtr m_SessionId;
	}
}
