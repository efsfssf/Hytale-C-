using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200008F RID: 143
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ShowNativeProfileCallbackInfoInternal : ICallbackInfoInternal, IGettable<ShowNativeProfileCallbackInfo>, ISettable<ShowNativeProfileCallbackInfo>, IDisposable
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x00008150 File Offset: 0x00006350
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x00008168 File Offset: 0x00006368
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00008174 File Offset: 0x00006374
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x00008195 File Offset: 0x00006395
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

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x000081A8 File Offset: 0x000063A8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x000081C0 File Offset: 0x000063C0
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x000081E1 File Offset: 0x000063E1
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

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x000081F4 File Offset: 0x000063F4
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x00008215 File Offset: 0x00006415
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

		// Token: 0x060005D4 RID: 1492 RVA: 0x00008225 File Offset: 0x00006425
		public void Set(ref ShowNativeProfileCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0000825C File Offset: 0x0000645C
		public void Set(ref ShowNativeProfileCallbackInfo? other)
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

		// Token: 0x060005D6 RID: 1494 RVA: 0x000082CA File Offset: 0x000064CA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000082F1 File Offset: 0x000064F1
		public void Get(out ShowNativeProfileCallbackInfo output)
		{
			output = default(ShowNativeProfileCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040002D4 RID: 724
		private Result m_ResultCode;

		// Token: 0x040002D5 RID: 725
		private IntPtr m_ClientData;

		// Token: 0x040002D6 RID: 726
		private IntPtr m_LocalUserId;

		// Token: 0x040002D7 RID: 727
		private IntPtr m_TargetUserId;
	}
}
