using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000672 RID: 1650
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IOSCredentialsSystemAuthCredentialsOptionsInternal : IGettable<IOSCredentialsSystemAuthCredentialsOptions>, ISettable<IOSCredentialsSystemAuthCredentialsOptions>, IDisposable
	{
		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x0003F294 File Offset: 0x0003D494
		// (set) Token: 0x06002AF9 RID: 11001 RVA: 0x0003F2AC File Offset: 0x0003D4AC
		public IntPtr PresentationContextProviding
		{
			get
			{
				return this.m_PresentationContextProviding;
			}
			set
			{
				this.m_PresentationContextProviding = value;
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06002AFA RID: 11002 RVA: 0x0003F2B8 File Offset: 0x0003D4B8
		public static IOSCreateBackgroundSnapshotViewInternal CreateBackgroundSnapshotView
		{
			get
			{
				bool flag = IOSCredentialsSystemAuthCredentialsOptionsInternal.s_CreateBackgroundSnapshotView == null;
				if (flag)
				{
					IOSCredentialsSystemAuthCredentialsOptionsInternal.s_CreateBackgroundSnapshotView = new IOSCreateBackgroundSnapshotViewInternal(AuthInterface.IOSCreateBackgroundSnapshotViewInternalImplementation);
				}
				return IOSCredentialsSystemAuthCredentialsOptionsInternal.s_CreateBackgroundSnapshotView;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06002AFB RID: 11003 RVA: 0x0003F2F0 File Offset: 0x0003D4F0
		// (set) Token: 0x06002AFC RID: 11004 RVA: 0x0003F308 File Offset: 0x0003D508
		public IntPtr CreateBackgroundSnapshotViewContext
		{
			get
			{
				return this.m_CreateBackgroundSnapshotViewContext;
			}
			set
			{
				this.m_CreateBackgroundSnapshotViewContext = value;
			}
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x0003F314 File Offset: 0x0003D514
		public void Set(ref IOSCredentialsSystemAuthCredentialsOptions other)
		{
			this.m_ApiVersion = 2;
			this.PresentationContextProviding = other.PresentationContextProviding;
			this.m_CreateBackgroundSnapshotView = ((other.CreateBackgroundSnapshotView != null) ? Marshal.GetFunctionPointerForDelegate<IOSCreateBackgroundSnapshotViewInternal>(IOSCredentialsSystemAuthCredentialsOptionsInternal.CreateBackgroundSnapshotView) : IntPtr.Zero);
			this.CreateBackgroundSnapshotViewContext = other.CreateBackgroundSnapshotViewContext;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x0003F364 File Offset: 0x0003D564
		public void Set(ref IOSCredentialsSystemAuthCredentialsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.PresentationContextProviding = other.Value.PresentationContextProviding;
				this.m_CreateBackgroundSnapshotView = ((other.Value.CreateBackgroundSnapshotView != null) ? Marshal.GetFunctionPointerForDelegate<IOSCreateBackgroundSnapshotViewInternal>(IOSCredentialsSystemAuthCredentialsOptionsInternal.CreateBackgroundSnapshotView) : IntPtr.Zero);
				this.CreateBackgroundSnapshotViewContext = other.Value.CreateBackgroundSnapshotViewContext;
			}
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x0003F3D6 File Offset: 0x0003D5D6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_PresentationContextProviding);
			Helper.Dispose(ref this.m_CreateBackgroundSnapshotView);
			Helper.Dispose(ref this.m_CreateBackgroundSnapshotViewContext);
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x0003F3FD File Offset: 0x0003D5FD
		public void Get(out IOSCredentialsSystemAuthCredentialsOptions output)
		{
			output = default(IOSCredentialsSystemAuthCredentialsOptions);
			output.Set(ref this);
		}

		// Token: 0x04001267 RID: 4711
		private int m_ApiVersion;

		// Token: 0x04001268 RID: 4712
		private IntPtr m_PresentationContextProviding;

		// Token: 0x04001269 RID: 4713
		private IntPtr m_CreateBackgroundSnapshotView;

		// Token: 0x0400126A RID: 4714
		private IntPtr m_CreateBackgroundSnapshotViewContext;

		// Token: 0x0400126B RID: 4715
		private static IOSCreateBackgroundSnapshotViewInternal s_CreateBackgroundSnapshotView;
	}
}
