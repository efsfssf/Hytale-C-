using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.KWS
{
	// Token: 0x020004AE RID: 1198
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UpdateParentEmailCallbackInfoInternal : ICallbackInfoInternal, IGettable<UpdateParentEmailCallbackInfo>, ISettable<UpdateParentEmailCallbackInfo>, IDisposable
	{
		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x0002DD10 File Offset: 0x0002BF10
		// (set) Token: 0x06001F50 RID: 8016 RVA: 0x0002DD28 File Offset: 0x0002BF28
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

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x0002DD34 File Offset: 0x0002BF34
		// (set) Token: 0x06001F52 RID: 8018 RVA: 0x0002DD55 File Offset: 0x0002BF55
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

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x0002DD68 File Offset: 0x0002BF68
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x0002DD80 File Offset: 0x0002BF80
		// (set) Token: 0x06001F55 RID: 8021 RVA: 0x0002DDA1 File Offset: 0x0002BFA1
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

		// Token: 0x06001F56 RID: 8022 RVA: 0x0002DDB1 File Offset: 0x0002BFB1
		public void Set(ref UpdateParentEmailCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0002DDDC File Offset: 0x0002BFDC
		public void Set(ref UpdateParentEmailCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0002DE35 File Offset: 0x0002C035
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0002DE50 File Offset: 0x0002C050
		public void Get(out UpdateParentEmailCallbackInfo output)
		{
			output = default(UpdateParentEmailCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000D98 RID: 3480
		private Result m_ResultCode;

		// Token: 0x04000D99 RID: 3481
		private IntPtr m_ClientData;

		// Token: 0x04000D9A RID: 3482
		private IntPtr m_LocalUserId;
	}
}
