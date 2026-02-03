using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000320 RID: 800
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReadFileCallbackInfoInternal : ICallbackInfoInternal, IGettable<ReadFileCallbackInfo>, ISettable<ReadFileCallbackInfo>, IDisposable
	{
		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x060015C7 RID: 5575 RVA: 0x0001F9B0 File Offset: 0x0001DBB0
		// (set) Token: 0x060015C8 RID: 5576 RVA: 0x0001F9C8 File Offset: 0x0001DBC8
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

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x060015C9 RID: 5577 RVA: 0x0001F9D4 File Offset: 0x0001DBD4
		// (set) Token: 0x060015CA RID: 5578 RVA: 0x0001F9F5 File Offset: 0x0001DBF5
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

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x060015CB RID: 5579 RVA: 0x0001FA08 File Offset: 0x0001DC08
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x0001FA20 File Offset: 0x0001DC20
		// (set) Token: 0x060015CD RID: 5581 RVA: 0x0001FA41 File Offset: 0x0001DC41
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

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x0001FA54 File Offset: 0x0001DC54
		// (set) Token: 0x060015CF RID: 5583 RVA: 0x0001FA75 File Offset: 0x0001DC75
		public Utf8String Filename
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Filename, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0001FA85 File Offset: 0x0001DC85
		public void Set(ref ReadFileCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0001FABC File Offset: 0x0001DCBC
		public void Set(ref ReadFileCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
			}
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x0001FB2A File Offset: 0x0001DD2A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0001FB51 File Offset: 0x0001DD51
		public void Get(out ReadFileCallbackInfo output)
		{
			output = default(ReadFileCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000974 RID: 2420
		private Result m_ResultCode;

		// Token: 0x04000975 RID: 2421
		private IntPtr m_ClientData;

		// Token: 0x04000976 RID: 2422
		private IntPtr m_LocalUserId;

		// Token: 0x04000977 RID: 2423
		private IntPtr m_Filename;
	}
}
