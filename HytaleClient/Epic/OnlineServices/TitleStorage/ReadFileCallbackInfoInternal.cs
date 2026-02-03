using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000BA RID: 186
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReadFileCallbackInfoInternal : ICallbackInfoInternal, IGettable<ReadFileCallbackInfo>, ISettable<ReadFileCallbackInfo>, IDisposable
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x00009B8C File Offset: 0x00007D8C
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x00009BA4 File Offset: 0x00007DA4
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

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x00009BB0 File Offset: 0x00007DB0
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x00009BD1 File Offset: 0x00007DD1
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

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x00009BE4 File Offset: 0x00007DE4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00009BFC File Offset: 0x00007DFC
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x00009C1D File Offset: 0x00007E1D
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

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00009C30 File Offset: 0x00007E30
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x00009C51 File Offset: 0x00007E51
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

		// Token: 0x060006EE RID: 1774 RVA: 0x00009C61 File Offset: 0x00007E61
		public void Set(ref ReadFileCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00009C98 File Offset: 0x00007E98
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

		// Token: 0x060006F0 RID: 1776 RVA: 0x00009D06 File Offset: 0x00007F06
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00009D2D File Offset: 0x00007F2D
		public void Get(out ReadFileCallbackInfo output)
		{
			output = default(ReadFileCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000348 RID: 840
		private Result m_ResultCode;

		// Token: 0x04000349 RID: 841
		private IntPtr m_ClientData;

		// Token: 0x0400034A RID: 842
		private IntPtr m_LocalUserId;

		// Token: 0x0400034B RID: 843
		private IntPtr m_Filename;
	}
}
