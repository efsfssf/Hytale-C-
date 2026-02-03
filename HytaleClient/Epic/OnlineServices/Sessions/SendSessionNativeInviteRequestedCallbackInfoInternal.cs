using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000149 RID: 329
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendSessionNativeInviteRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendSessionNativeInviteRequestedCallbackInfo>, ISettable<SendSessionNativeInviteRequestedCallbackInfo>, IDisposable
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0000DD24 File Offset: 0x0000BF24
		// (set) Token: 0x060009F9 RID: 2553 RVA: 0x0000DD45 File Offset: 0x0000BF45
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

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x0000DD58 File Offset: 0x0000BF58
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x0000DD70 File Offset: 0x0000BF70
		// (set) Token: 0x060009FC RID: 2556 RVA: 0x0000DD88 File Offset: 0x0000BF88
		public ulong UiEventId
		{
			get
			{
				return this.m_UiEventId;
			}
			set
			{
				this.m_UiEventId = value;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x0000DD94 File Offset: 0x0000BF94
		// (set) Token: 0x060009FE RID: 2558 RVA: 0x0000DDB5 File Offset: 0x0000BFB5
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

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x0000DDC8 File Offset: 0x0000BFC8
		// (set) Token: 0x06000A00 RID: 2560 RVA: 0x0000DDE9 File Offset: 0x0000BFE9
		public Utf8String TargetNativeAccountType
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TargetNativeAccountType, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetNativeAccountType);
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0000DDFC File Offset: 0x0000BFFC
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0000DE1D File Offset: 0x0000C01D
		public Utf8String TargetUserNativeAccountId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TargetUserNativeAccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserNativeAccountId);
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0000DE30 File Offset: 0x0000C030
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x0000DE51 File Offset: 0x0000C051
		public Utf8String SessionId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SessionId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SessionId);
			}
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0000DE64 File Offset: 0x0000C064
		public void Set(ref SendSessionNativeInviteRequestedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.UiEventId = other.UiEventId;
			this.LocalUserId = other.LocalUserId;
			this.TargetNativeAccountType = other.TargetNativeAccountType;
			this.TargetUserNativeAccountId = other.TargetUserNativeAccountId;
			this.SessionId = other.SessionId;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0000DEC0 File Offset: 0x0000C0C0
		public void Set(ref SendSessionNativeInviteRequestedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.UiEventId = other.Value.UiEventId;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetNativeAccountType = other.Value.TargetNativeAccountType;
				this.TargetUserNativeAccountId = other.Value.TargetUserNativeAccountId;
				this.SessionId = other.Value.SessionId;
			}
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0000DF5B File Offset: 0x0000C15B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetNativeAccountType);
			Helper.Dispose(ref this.m_TargetUserNativeAccountId);
			Helper.Dispose(ref this.m_SessionId);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0000DF9A File Offset: 0x0000C19A
		public void Get(out SendSessionNativeInviteRequestedCallbackInfo output)
		{
			output = default(SendSessionNativeInviteRequestedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000483 RID: 1155
		private IntPtr m_ClientData;

		// Token: 0x04000484 RID: 1156
		private ulong m_UiEventId;

		// Token: 0x04000485 RID: 1157
		private IntPtr m_LocalUserId;

		// Token: 0x04000486 RID: 1158
		private IntPtr m_TargetNativeAccountType;

		// Token: 0x04000487 RID: 1159
		private IntPtr m_TargetUserNativeAccountId;

		// Token: 0x04000488 RID: 1160
		private IntPtr m_SessionId;
	}
}
