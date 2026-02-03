using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200018E RID: 398
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct UnregisterPlayersCallbackInfoInternal : ICallbackInfoInternal, IGettable<UnregisterPlayersCallbackInfo>, ISettable<UnregisterPlayersCallbackInfo>, IDisposable
	{
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00011040 File Offset: 0x0000F240
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x00011058 File Offset: 0x0000F258
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

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x00011064 File Offset: 0x0000F264
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x00011085 File Offset: 0x0000F285
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

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x00011098 File Offset: 0x0000F298
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x000110B0 File Offset: 0x0000F2B0
		// (set) Token: 0x06000BAC RID: 2988 RVA: 0x000110D7 File Offset: 0x0000F2D7
		public ProductUserId[] UnregisteredPlayers
		{
			get
			{
				ProductUserId[] result;
				Helper.GetHandle<ProductUserId>(this.m_UnregisteredPlayers, out result, this.m_UnregisteredPlayersCount);
				return result;
			}
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_UnregisteredPlayers, out this.m_UnregisteredPlayersCount);
			}
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x000110ED File Offset: 0x0000F2ED
		public void Set(ref UnregisterPlayersCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.UnregisteredPlayers = other.UnregisteredPlayers;
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00011118 File Offset: 0x0000F318
		public void Set(ref UnregisterPlayersCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.UnregisteredPlayers = other.Value.UnregisteredPlayers;
			}
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00011171 File Offset: 0x0000F371
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_UnregisteredPlayers);
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0001118C File Offset: 0x0000F38C
		public void Get(out UnregisterPlayersCallbackInfo output)
		{
			output = default(UnregisterPlayersCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000559 RID: 1369
		private Result m_ResultCode;

		// Token: 0x0400055A RID: 1370
		private IntPtr m_ClientData;

		// Token: 0x0400055B RID: 1371
		private IntPtr m_UnregisteredPlayers;

		// Token: 0x0400055C RID: 1372
		private uint m_UnregisteredPlayersCount;
	}
}
