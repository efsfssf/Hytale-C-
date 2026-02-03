using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000575 RID: 1397
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AcceptRequestToJoinCallbackInfoInternal : ICallbackInfoInternal, IGettable<AcceptRequestToJoinCallbackInfo>, ISettable<AcceptRequestToJoinCallbackInfo>, IDisposable
	{
		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x00035A74 File Offset: 0x00033C74
		// (set) Token: 0x06002472 RID: 9330 RVA: 0x00035A8C File Offset: 0x00033C8C
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

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06002473 RID: 9331 RVA: 0x00035A98 File Offset: 0x00033C98
		// (set) Token: 0x06002474 RID: 9332 RVA: 0x00035AB9 File Offset: 0x00033CB9
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

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x00035ACC File Offset: 0x00033CCC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06002476 RID: 9334 RVA: 0x00035AE4 File Offset: 0x00033CE4
		// (set) Token: 0x06002477 RID: 9335 RVA: 0x00035B05 File Offset: 0x00033D05
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

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x00035B18 File Offset: 0x00033D18
		// (set) Token: 0x06002479 RID: 9337 RVA: 0x00035B39 File Offset: 0x00033D39
		public ProductUserId TargetUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00035B49 File Offset: 0x00033D49
		public void Set(ref AcceptRequestToJoinCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x00035B80 File Offset: 0x00033D80
		public void Set(ref AcceptRequestToJoinCallbackInfo? other)
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

		// Token: 0x0600247C RID: 9340 RVA: 0x00035BEE File Offset: 0x00033DEE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x00035C15 File Offset: 0x00033E15
		public void Get(out AcceptRequestToJoinCallbackInfo output)
		{
			output = default(AcceptRequestToJoinCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000FFB RID: 4091
		private Result m_ResultCode;

		// Token: 0x04000FFC RID: 4092
		private IntPtr m_ClientData;

		// Token: 0x04000FFD RID: 4093
		private IntPtr m_LocalUserId;

		// Token: 0x04000FFE RID: 4094
		private IntPtr m_TargetUserId;
	}
}
