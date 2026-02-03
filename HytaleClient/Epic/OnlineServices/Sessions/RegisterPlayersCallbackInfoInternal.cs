using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200013D RID: 317
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RegisterPlayersCallbackInfoInternal : ICallbackInfoInternal, IGettable<RegisterPlayersCallbackInfo>, ISettable<RegisterPlayersCallbackInfo>, IDisposable
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0000D514 File Offset: 0x0000B714
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0000D52C File Offset: 0x0000B72C
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

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0000D538 File Offset: 0x0000B738
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0000D559 File Offset: 0x0000B759
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

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0000D56C File Offset: 0x0000B76C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0000D584 File Offset: 0x0000B784
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0000D5AB File Offset: 0x0000B7AB
		public ProductUserId[] RegisteredPlayers
		{
			get
			{
				ProductUserId[] result;
				Helper.GetHandle<ProductUserId>(this.m_RegisteredPlayers, out result, this.m_RegisteredPlayersCount);
				return result;
			}
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_RegisteredPlayers, out this.m_RegisteredPlayersCount);
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0000D5C4 File Offset: 0x0000B7C4
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x0000D5EB File Offset: 0x0000B7EB
		public ProductUserId[] SanctionedPlayers
		{
			get
			{
				ProductUserId[] result;
				Helper.GetHandle<ProductUserId>(this.m_SanctionedPlayers, out result, this.m_SanctionedPlayersCount);
				return result;
			}
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_SanctionedPlayers, out this.m_SanctionedPlayersCount);
			}
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0000D601 File Offset: 0x0000B801
		public void Set(ref RegisterPlayersCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.RegisteredPlayers = other.RegisteredPlayers;
			this.SanctionedPlayers = other.SanctionedPlayers;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0000D638 File Offset: 0x0000B838
		public void Set(ref RegisterPlayersCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.RegisteredPlayers = other.Value.RegisteredPlayers;
				this.SanctionedPlayers = other.Value.SanctionedPlayers;
			}
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0000D6A6 File Offset: 0x0000B8A6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_RegisteredPlayers);
			Helper.Dispose(ref this.m_SanctionedPlayers);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0000D6CD File Offset: 0x0000B8CD
		public void Get(out RegisterPlayersCallbackInfo output)
		{
			output = default(RegisterPlayersCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400045D RID: 1117
		private Result m_ResultCode;

		// Token: 0x0400045E RID: 1118
		private IntPtr m_ClientData;

		// Token: 0x0400045F RID: 1119
		private IntPtr m_RegisteredPlayers;

		// Token: 0x04000460 RID: 1120
		private uint m_RegisteredPlayersCount;

		// Token: 0x04000461 RID: 1121
		private IntPtr m_SanctionedPlayers;

		// Token: 0x04000462 RID: 1122
		private uint m_SanctionedPlayersCount;
	}
}
