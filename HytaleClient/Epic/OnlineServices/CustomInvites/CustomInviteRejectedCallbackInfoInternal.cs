using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x02000589 RID: 1417
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct CustomInviteRejectedCallbackInfoInternal : ICallbackInfoInternal, IGettable<CustomInviteRejectedCallbackInfo>, ISettable<CustomInviteRejectedCallbackInfo>, IDisposable
	{
		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060024AB RID: 9387 RVA: 0x00035F38 File Offset: 0x00034138
		// (set) Token: 0x060024AC RID: 9388 RVA: 0x00035F59 File Offset: 0x00034159
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

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060024AD RID: 9389 RVA: 0x00035F6C File Offset: 0x0003416C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x060024AE RID: 9390 RVA: 0x00035F84 File Offset: 0x00034184
		// (set) Token: 0x060024AF RID: 9391 RVA: 0x00035FA5 File Offset: 0x000341A5
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

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x060024B0 RID: 9392 RVA: 0x00035FB8 File Offset: 0x000341B8
		// (set) Token: 0x060024B1 RID: 9393 RVA: 0x00035FD9 File Offset: 0x000341D9
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

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060024B2 RID: 9394 RVA: 0x00035FEC File Offset: 0x000341EC
		// (set) Token: 0x060024B3 RID: 9395 RVA: 0x0003600D File Offset: 0x0003420D
		public Utf8String CustomInviteId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_CustomInviteId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_CustomInviteId);
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x060024B4 RID: 9396 RVA: 0x00036020 File Offset: 0x00034220
		// (set) Token: 0x060024B5 RID: 9397 RVA: 0x00036041 File Offset: 0x00034241
		public Utf8String Payload
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Payload, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Payload);
			}
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x00036054 File Offset: 0x00034254
		public void Set(ref CustomInviteRejectedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.TargetUserId = other.TargetUserId;
			this.LocalUserId = other.LocalUserId;
			this.CustomInviteId = other.CustomInviteId;
			this.Payload = other.Payload;
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000360A4 File Offset: 0x000342A4
		public void Set(ref CustomInviteRejectedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.TargetUserId = other.Value.TargetUserId;
				this.LocalUserId = other.Value.LocalUserId;
				this.CustomInviteId = other.Value.CustomInviteId;
				this.Payload = other.Value.Payload;
			}
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x00036127 File Offset: 0x00034327
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_TargetUserId);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_CustomInviteId);
			Helper.Dispose(ref this.m_Payload);
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x00036166 File Offset: 0x00034366
		public void Get(out CustomInviteRejectedCallbackInfo output)
		{
			output = default(CustomInviteRejectedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001011 RID: 4113
		private IntPtr m_ClientData;

		// Token: 0x04001012 RID: 4114
		private IntPtr m_TargetUserId;

		// Token: 0x04001013 RID: 4115
		private IntPtr m_LocalUserId;

		// Token: 0x04001014 RID: 4116
		private IntPtr m_CustomInviteId;

		// Token: 0x04001015 RID: 4117
		private IntPtr m_Payload;
	}
}
