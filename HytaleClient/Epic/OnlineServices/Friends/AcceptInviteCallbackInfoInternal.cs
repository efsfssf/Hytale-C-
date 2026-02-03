using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Friends
{
	// Token: 0x020004D0 RID: 1232
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AcceptInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<AcceptInviteCallbackInfo>, ISettable<AcceptInviteCallbackInfo>, IDisposable
	{
		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x0600201E RID: 8222 RVA: 0x0002F0C0 File Offset: 0x0002D2C0
		// (set) Token: 0x0600201F RID: 8223 RVA: 0x0002F0D8 File Offset: 0x0002D2D8
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

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06002020 RID: 8224 RVA: 0x0002F0E4 File Offset: 0x0002D2E4
		// (set) Token: 0x06002021 RID: 8225 RVA: 0x0002F105 File Offset: 0x0002D305
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

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x0002F118 File Offset: 0x0002D318
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06002023 RID: 8227 RVA: 0x0002F130 File Offset: 0x0002D330
		// (set) Token: 0x06002024 RID: 8228 RVA: 0x0002F151 File Offset: 0x0002D351
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x0002F164 File Offset: 0x0002D364
		// (set) Token: 0x06002026 RID: 8230 RVA: 0x0002F185 File Offset: 0x0002D385
		public EpicAccountId TargetUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0002F195 File Offset: 0x0002D395
		public void Set(ref AcceptInviteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0002F1CC File Offset: 0x0002D3CC
		public void Set(ref AcceptInviteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0002F23A File Offset: 0x0002D43A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0002F261 File Offset: 0x0002D461
		public void Get(out AcceptInviteCallbackInfo output)
		{
			output = default(AcceptInviteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000DF9 RID: 3577
		private Result m_ResultCode;

		// Token: 0x04000DFA RID: 3578
		private IntPtr m_ClientData;

		// Token: 0x04000DFB RID: 3579
		private IntPtr m_LocalUserId;

		// Token: 0x04000DFC RID: 3580
		private IntPtr m_TargetUserId;
	}
}
