using System;

namespace Epic.OnlineServices.Auth
{
	// Token: 0x02000671 RID: 1649
	public struct IOSCredentialsSystemAuthCredentialsOptions
	{
		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x0003F243 File Offset: 0x0003D443
		// (set) Token: 0x06002AF2 RID: 10994 RVA: 0x0003F24B File Offset: 0x0003D44B
		public IntPtr PresentationContextProviding { get; set; }

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x0003F254 File Offset: 0x0003D454
		// (set) Token: 0x06002AF4 RID: 10996 RVA: 0x0003F25C File Offset: 0x0003D45C
		public IOSCreateBackgroundSnapshotView CreateBackgroundSnapshotView { get; set; }

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x0003F265 File Offset: 0x0003D465
		// (set) Token: 0x06002AF6 RID: 10998 RVA: 0x0003F26D File Offset: 0x0003D46D
		public IntPtr CreateBackgroundSnapshotViewContext { get; set; }

		// Token: 0x06002AF7 RID: 10999 RVA: 0x0003F276 File Offset: 0x0003D476
		internal void Set(ref IOSCredentialsSystemAuthCredentialsOptionsInternal other)
		{
			this.PresentationContextProviding = other.PresentationContextProviding;
			this.CreateBackgroundSnapshotViewContext = other.CreateBackgroundSnapshotViewContext;
		}
	}
}
