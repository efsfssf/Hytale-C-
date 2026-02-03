using System;
using Coherent.UI;
using NLog;

namespace HytaleClient.Interface.CoherentUI.Internals
{
	// Token: 0x020008DD RID: 2269
	internal class CoUIContextListener : ContextListener
	{
		// Token: 0x06004236 RID: 16950 RVA: 0x000C8368 File Offset: 0x000C6568
		public CoUIContextListener(Action onContextReady)
		{
			this._onContextReady = onContextReady;
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x000C8379 File Offset: 0x000C6579
		public override void ContextReady()
		{
			CoUIContextListener.Logger.Info("CoUIContextListener.ContextReady", "CoherentUI");
			this._onContextReady();
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x000C839D File Offset: 0x000C659D
		public override void OnError(ContextError contextError)
		{
			CoUIContextListener.Logger.Info<string, ContextErrorType>("CoUIContextListener.OnError: {0} (#{1})", contextError.Error, contextError.ErrorCode);
		}

		// Token: 0x04002061 RID: 8289
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002062 RID: 8290
		private readonly Action _onContextReady;
	}
}
