using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HytaleClient.Utils;
using Newtonsoft.Json;
using NLog;

namespace HytaleClient.Core
{
	// Token: 0x02000B7A RID: 2938
	public abstract class Disposable : IDisposable
	{
		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x06005A34 RID: 23092 RVA: 0x001C10C7 File Offset: 0x001BF2C7
		// (set) Token: 0x06005A35 RID: 23093 RVA: 0x001C10CF File Offset: 0x001BF2CF
		[JsonIgnore]
		public bool Disposed { get; protected set; }

		// Token: 0x06005A36 RID: 23094 RVA: 0x001C10D8 File Offset: 0x001BF2D8
		public void Dispose()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool disposed = this.Disposed;
			if (!disposed)
			{
				byte b;
				Disposable.UndisposedDisposables.TryRemove(this._reference, ref b);
				this.DoDispose();
				this.Disposed = true;
			}
		}

		// Token: 0x06005A37 RID: 23095
		protected abstract void DoDispose();

		// Token: 0x06005A38 RID: 23096 RVA: 0x001C1120 File Offset: 0x001BF320
		public static void LogSummary(bool unfinalized)
		{
			Dictionary<string, List<Disposable>> dictionary = new Dictionary<string, List<Disposable>>();
			ICollection<WeakReference<Disposable>> collection = unfinalized ? Disposable.UnfinalizedDisposables.Keys : Disposable.UndisposedDisposables.Keys;
			foreach (WeakReference<Disposable> weakReference in collection)
			{
				Disposable disposable;
				weakReference.TryGetTarget(out disposable);
				string key = (disposable != null) ? disposable.GetType().FullName : "(Expired)";
				List<Disposable> list;
				dictionary.TryGetValue(key, out list);
				bool flag = list == null;
				if (flag)
				{
					list = (dictionary[key] = new List<Disposable>());
				}
				list.Add(disposable);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("---- Summary of " + (unfinalized ? "unfinalized" : "undisposed") + " disposable references ----");
			foreach (KeyValuePair<string, List<Disposable>> keyValuePair in Enumerable.ThenBy<KeyValuePair<string, List<Disposable>>, string>(Enumerable.OrderBy<KeyValuePair<string, List<Disposable>>, int>(dictionary, (KeyValuePair<string, List<Disposable>> kvp) => -kvp.Value.Count), (KeyValuePair<string, List<Disposable>> kvp) => kvp.Key))
			{
				stringBuilder.AppendLine(string.Format("{0} : {1}", keyValuePair.Key, keyValuePair.Value.Count));
			}
			Disposable.Logger.Info<StringBuilder>(stringBuilder);
		}

		// Token: 0x06005A39 RID: 23097 RVA: 0x001C12C4 File Offset: 0x001BF4C4
		protected Disposable()
		{
			this._reference = new WeakReference<Disposable>(this);
			Disposable.UndisposedDisposables.TryAdd(this._reference, 0);
			Disposable.UnfinalizedDisposables.TryAdd(this._reference, 0);
		}

		// Token: 0x06005A3A RID: 23098 RVA: 0x001C1314 File Offset: 0x001BF514
		protected override void Finalize()
		{
			try
			{
				bool flag = !this.Disposed && !CrashHandler.IsCrashing;
				byte b;
				if (flag)
				{
					Disposable.Logger.Warn<string, string>(this.StackTrace, "Object was not disposed! {0}", base.GetType().FullName);
					Disposable.UndisposedDisposables.TryRemove(this._reference, ref b);
				}
				Disposable.UnfinalizedDisposables.TryRemove(this._reference, ref b);
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x0400385E RID: 14430
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400385F RID: 14431
		public static readonly ConcurrentDictionary<WeakReference<Disposable>, byte> UndisposedDisposables = new ConcurrentDictionary<WeakReference<Disposable>, byte>();

		// Token: 0x04003860 RID: 14432
		public static readonly ConcurrentDictionary<WeakReference<Disposable>, byte> UnfinalizedDisposables = new ConcurrentDictionary<WeakReference<Disposable>, byte>();

		// Token: 0x04003861 RID: 14433
		protected readonly string StackTrace = Environment.StackTrace;

		// Token: 0x04003862 RID: 14434
		protected readonly WeakReference<Disposable> _reference;
	}
}
