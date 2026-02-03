using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200005C RID: 92
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct HideFriendsCallbackInfoInternal : ICallbackInfoInternal, IGettable<HideFriendsCallbackInfo>, ISettable<HideFriendsCallbackInfo>, IDisposable
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00006CAC File Offset: 0x00004EAC
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x00006CC4 File Offset: 0x00004EC4
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00006CD0 File Offset: 0x00004ED0
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x00006CF1 File Offset: 0x00004EF1
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

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00006D04 File Offset: 0x00004F04
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00006D1C File Offset: 0x00004F1C
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x00006D3D File Offset: 0x00004F3D
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

		// Token: 0x060004B0 RID: 1200 RVA: 0x00006D4D File Offset: 0x00004F4D
		public void Set(ref HideFriendsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00006D78 File Offset: 0x00004F78
		public void Set(ref HideFriendsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00006DD1 File Offset: 0x00004FD1
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00006DEC File Offset: 0x00004FEC
		public void Get(out HideFriendsCallbackInfo output)
		{
			output = default(HideFriendsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040001E9 RID: 489
		private Result m_ResultCode;

		// Token: 0x040001EA RID: 490
		private IntPtr m_ClientData;

		// Token: 0x040001EB RID: 491
		private IntPtr m_LocalUserId;
	}
}
