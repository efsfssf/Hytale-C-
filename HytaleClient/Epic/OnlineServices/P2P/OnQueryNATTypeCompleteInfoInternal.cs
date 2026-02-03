using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200079C RID: 1948
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnQueryNATTypeCompleteInfoInternal : ICallbackInfoInternal, IGettable<OnQueryNATTypeCompleteInfo>, ISettable<OnQueryNATTypeCompleteInfo>, IDisposable
	{
		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06003278 RID: 12920 RVA: 0x0004B57C File Offset: 0x0004977C
		// (set) Token: 0x06003279 RID: 12921 RVA: 0x0004B594 File Offset: 0x00049794
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

		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x0600327A RID: 12922 RVA: 0x0004B5A0 File Offset: 0x000497A0
		// (set) Token: 0x0600327B RID: 12923 RVA: 0x0004B5C1 File Offset: 0x000497C1
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

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x0600327C RID: 12924 RVA: 0x0004B5D4 File Offset: 0x000497D4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x0600327D RID: 12925 RVA: 0x0004B5EC File Offset: 0x000497EC
		// (set) Token: 0x0600327E RID: 12926 RVA: 0x0004B604 File Offset: 0x00049804
		public NATType NATType
		{
			get
			{
				return this.m_NATType;
			}
			set
			{
				this.m_NATType = value;
			}
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x0004B60E File Offset: 0x0004980E
		public void Set(ref OnQueryNATTypeCompleteInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.NATType = other.NATType;
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x0004B638 File Offset: 0x00049838
		public void Set(ref OnQueryNATTypeCompleteInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.NATType = other.Value.NATType;
			}
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x0004B691 File Offset: 0x00049891
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x0004B6A0 File Offset: 0x000498A0
		public void Get(out OnQueryNATTypeCompleteInfo output)
		{
			output = default(OnQueryNATTypeCompleteInfo);
			output.Set(ref this);
		}

		// Token: 0x040016A3 RID: 5795
		private Result m_ResultCode;

		// Token: 0x040016A4 RID: 5796
		private IntPtr m_ClientData;

		// Token: 0x040016A5 RID: 5797
		private NATType m_NATType;
	}
}
