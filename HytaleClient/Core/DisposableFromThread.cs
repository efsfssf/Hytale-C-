using System;
using System.Diagnostics;
using System.Threading;

namespace HytaleClient.Core
{
	// Token: 0x02000B7B RID: 2939
	internal abstract class DisposableFromThread : Disposable
	{
		// Token: 0x06005A3C RID: 23100 RVA: 0x001C13C0 File Offset: 0x001BF5C0
		protected DisposableFromThread(Thread thread)
		{
			this._thread = thread;
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x001C13D4 File Offset: 0x001BF5D4
		public new void Dispose()
		{
			Debug.Assert(Thread.CurrentThread.ManagedThreadId == this._thread.ManagedThreadId);
			bool disposed = base.Disposed;
			if (!disposed)
			{
				byte b;
				Disposable.UndisposedDisposables.TryRemove(this._reference, ref b);
				this.DoDispose();
				base.Disposed = true;
			}
		}

		// Token: 0x04003863 RID: 14435
		private readonly Thread _thread;
	}
}
