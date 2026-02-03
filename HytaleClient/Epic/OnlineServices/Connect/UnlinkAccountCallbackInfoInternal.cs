using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x02000620 RID: 1568
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnlinkAccountCallbackInfoInternal : ICallbackInfoInternal, IGettable<UnlinkAccountCallbackInfo>, ISettable<UnlinkAccountCallbackInfo>, IDisposable
	{
		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x0003B688 File Offset: 0x00039888
		// (set) Token: 0x0600288B RID: 10379 RVA: 0x0003B6A0 File Offset: 0x000398A0
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

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x0600288C RID: 10380 RVA: 0x0003B6AC File Offset: 0x000398AC
		// (set) Token: 0x0600288D RID: 10381 RVA: 0x0003B6CD File Offset: 0x000398CD
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

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x0600288E RID: 10382 RVA: 0x0003B6E0 File Offset: 0x000398E0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x0003B6F8 File Offset: 0x000398F8
		// (set) Token: 0x06002890 RID: 10384 RVA: 0x0003B719 File Offset: 0x00039919
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

		// Token: 0x06002891 RID: 10385 RVA: 0x0003B729 File Offset: 0x00039929
		public void Set(ref UnlinkAccountCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x0003B754 File Offset: 0x00039954
		public void Set(ref UnlinkAccountCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x0003B7AD File Offset: 0x000399AD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x0003B7C8 File Offset: 0x000399C8
		public void Get(out UnlinkAccountCallbackInfo output)
		{
			output = default(UnlinkAccountCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001164 RID: 4452
		private Result m_ResultCode;

		// Token: 0x04001165 RID: 4453
		private IntPtr m_ClientData;

		// Token: 0x04001166 RID: 4454
		private IntPtr m_LocalUserId;
	}
}
