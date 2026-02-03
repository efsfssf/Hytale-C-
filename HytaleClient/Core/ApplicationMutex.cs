using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace HytaleClient.Core
{
	// Token: 0x02000B78 RID: 2936
	internal class ApplicationMutex : IDisposable
	{
		// Token: 0x06005A19 RID: 23065 RVA: 0x001BFD40 File Offset: 0x001BDF40
		public ApplicationMutex()
		{
			string arg = ((GuidAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
			string name = string.Format("Global\\{{{0}}}", arg);
			this._mutex = new Mutex(false, name);
			this.Acquired = false;
			try
			{
				this.Acquired = this._mutex.WaitOne(10, false);
			}
			catch (AbandonedMutexException)
			{
				this.Acquired = true;
			}
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x001BFDD4 File Offset: 0x001BDFD4
		public void Dispose()
		{
			bool acquired = this.Acquired;
			if (acquired)
			{
				this._mutex.ReleaseMutex();
			}
		}

		// Token: 0x04003853 RID: 14419
		public readonly bool Acquired;

		// Token: 0x04003854 RID: 14420
		private Mutex _mutex;
	}
}
