using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Connect
{
	// Token: 0x0200061C RID: 1564
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct TransferDeviceIdAccountCallbackInfoInternal : ICallbackInfoInternal, IGettable<TransferDeviceIdAccountCallbackInfo>, ISettable<TransferDeviceIdAccountCallbackInfo>, IDisposable
	{
		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x0600286B RID: 10347 RVA: 0x0003B39C File Offset: 0x0003959C
		// (set) Token: 0x0600286C RID: 10348 RVA: 0x0003B3B4 File Offset: 0x000395B4
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

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x0003B3C0 File Offset: 0x000395C0
		// (set) Token: 0x0600286E RID: 10350 RVA: 0x0003B3E1 File Offset: 0x000395E1
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

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600286F RID: 10351 RVA: 0x0003B3F4 File Offset: 0x000395F4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06002870 RID: 10352 RVA: 0x0003B40C File Offset: 0x0003960C
		// (set) Token: 0x06002871 RID: 10353 RVA: 0x0003B42D File Offset: 0x0003962D
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

		// Token: 0x06002872 RID: 10354 RVA: 0x0003B43D File Offset: 0x0003963D
		public void Set(ref TransferDeviceIdAccountCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x0003B468 File Offset: 0x00039668
		public void Set(ref TransferDeviceIdAccountCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x0003B4C1 File Offset: 0x000396C1
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x0003B4DC File Offset: 0x000396DC
		public void Get(out TransferDeviceIdAccountCallbackInfo output)
		{
			output = default(TransferDeviceIdAccountCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001157 RID: 4439
		private Result m_ResultCode;

		// Token: 0x04001158 RID: 4440
		private IntPtr m_ClientData;

		// Token: 0x04001159 RID: 4441
		private IntPtr m_LocalUserId;
	}
}
