using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006ED RID: 1773
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnMessageToServerCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnMessageToServerCallbackInfo>, ISettable<OnMessageToServerCallbackInfo>, IDisposable
	{
		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06002DBD RID: 11709 RVA: 0x000439BC File Offset: 0x00041BBC
		// (set) Token: 0x06002DBE RID: 11710 RVA: 0x000439DD File Offset: 0x00041BDD
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

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06002DBF RID: 11711 RVA: 0x000439F0 File Offset: 0x00041BF0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06002DC0 RID: 11712 RVA: 0x00043A08 File Offset: 0x00041C08
		// (set) Token: 0x06002DC1 RID: 11713 RVA: 0x00043A2F File Offset: 0x00041C2F
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

		// Token: 0x06002DC2 RID: 11714 RVA: 0x00043A45 File Offset: 0x00041C45
		public void Set(ref OnMessageToServerCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.MessageData = other.MessageData;
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x00043A64 File Offset: 0x00041C64
		public void Set(ref OnMessageToServerCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.MessageData = other.Value.MessageData;
			}
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x00043AA8 File Offset: 0x00041CA8
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_MessageData);
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x00043AC3 File Offset: 0x00041CC3
		public void Get(out OnMessageToServerCallbackInfo output)
		{
			output = default(OnMessageToServerCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001431 RID: 5169
		private IntPtr m_ClientData;

		// Token: 0x04001432 RID: 5170
		private IntPtr m_MessageData;

		// Token: 0x04001433 RID: 5171
		private uint m_MessageDataSizeBytes;
	}
}
