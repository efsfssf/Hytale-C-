using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C2 RID: 1730
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnMessageToClientCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnMessageToClientCallbackInfo>, ISettable<OnMessageToClientCallbackInfo>, IDisposable
	{
		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06002CE9 RID: 11497 RVA: 0x000425B0 File Offset: 0x000407B0
		// (set) Token: 0x06002CEA RID: 11498 RVA: 0x000425D1 File Offset: 0x000407D1
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

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06002CEB RID: 11499 RVA: 0x000425E4 File Offset: 0x000407E4
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06002CEC RID: 11500 RVA: 0x000425FC File Offset: 0x000407FC
		// (set) Token: 0x06002CED RID: 11501 RVA: 0x00042614 File Offset: 0x00040814
		public IntPtr ClientHandle
		{
			get
			{
				return this.m_ClientHandle;
			}
			set
			{
				this.m_ClientHandle = value;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06002CEE RID: 11502 RVA: 0x00042620 File Offset: 0x00040820
		// (set) Token: 0x06002CEF RID: 11503 RVA: 0x00042647 File Offset: 0x00040847
		public ArraySegment<byte> MessageData
		{
			get
			{
				ArraySegment<byte> result;
				Helper.Get(this.m_MessageData, out result, this.m_MessageDataSizeBytes);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_MessageData, out this.m_MessageDataSizeBytes);
			}
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x0004265D File Offset: 0x0004085D
		public void Set(ref OnMessageToClientCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.ClientHandle = other.ClientHandle;
			this.MessageData = other.MessageData;
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x00042688 File Offset: 0x00040888
		public void Set(ref OnMessageToClientCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.ClientHandle = other.Value.ClientHandle;
				this.MessageData = other.Value.MessageData;
			}
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000426E1 File Offset: 0x000408E1
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_ClientHandle);
			Helper.Dispose(ref this.m_MessageData);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x00042708 File Offset: 0x00040908
		public void Get(out OnMessageToClientCallbackInfo output)
		{
			output = default(OnMessageToClientCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040013C3 RID: 5059
		private IntPtr m_ClientData;

		// Token: 0x040013C4 RID: 5060
		private IntPtr m_ClientHandle;

		// Token: 0x040013C5 RID: 5061
		private IntPtr m_MessageData;

		// Token: 0x040013C6 RID: 5062
		private uint m_MessageDataSizeBytes;
	}
}
