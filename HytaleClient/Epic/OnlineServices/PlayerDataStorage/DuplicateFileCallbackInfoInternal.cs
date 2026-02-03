using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002F8 RID: 760
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DuplicateFileCallbackInfoInternal : ICallbackInfoInternal, IGettable<DuplicateFileCallbackInfo>, ISettable<DuplicateFileCallbackInfo>, IDisposable
	{
		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060014C7 RID: 5319 RVA: 0x0001E504 File Offset: 0x0001C704
		// (set) Token: 0x060014C8 RID: 5320 RVA: 0x0001E51C File Offset: 0x0001C71C
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

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x060014C9 RID: 5321 RVA: 0x0001E528 File Offset: 0x0001C728
		// (set) Token: 0x060014CA RID: 5322 RVA: 0x0001E549 File Offset: 0x0001C749
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

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x0001E55C File Offset: 0x0001C75C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x0001E574 File Offset: 0x0001C774
		// (set) Token: 0x060014CD RID: 5325 RVA: 0x0001E595 File Offset: 0x0001C795
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

		// Token: 0x060014CE RID: 5326 RVA: 0x0001E5A5 File Offset: 0x0001C7A5
		public void Set(ref DuplicateFileCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0001E5D0 File Offset: 0x0001C7D0
		public void Set(ref DuplicateFileCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0001E629 File Offset: 0x0001C829
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0001E644 File Offset: 0x0001C844
		public void Get(out DuplicateFileCallbackInfo output)
		{
			output = default(DuplicateFileCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000921 RID: 2337
		private Result m_ResultCode;

		// Token: 0x04000922 RID: 2338
		private IntPtr m_ClientData;

		// Token: 0x04000923 RID: 2339
		private IntPtr m_LocalUserId;
	}
}
