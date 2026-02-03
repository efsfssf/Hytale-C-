using System;
using System.IO;
using HytaleClient.Application;
using NLog;

namespace HytaleClient.Interface.CoherentUI.Internals
{
	// Token: 0x020008DF RID: 2271
	internal class CoUIGameFileHandler : CoUIFileHandler
	{
		// Token: 0x06004241 RID: 16961 RVA: 0x000C8752 File Offset: 0x000C6952
		public CoUIGameFileHandler(App app)
		{
			this._app = app;
		}

		// Token: 0x06004242 RID: 16962 RVA: 0x000C8764 File Offset: 0x000C6964
		protected override byte[] GetFile(string filePath)
		{
			bool flag = filePath.StartsWith("coui://assets/");
			if (flag)
			{
				filePath = filePath.Substring("coui://assets/".Length);
				bool flag2 = this._app.Stage == App.AppStage.InGame;
				if (flag2)
				{
					try
					{
						return this._app.InGame.GetAsset(filePath);
					}
					catch (FileNotFoundException ex)
					{
						CoUIGameFileHandler.Logger.Error(ex, "UI requested asset file which doesn't exist \"{0}\", {1}", new object[]
						{
							ex.FileName,
							filePath
						});
					}
				}
			}
			return base.GetFile(filePath);
		}

		// Token: 0x0400206A RID: 8298
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400206B RID: 8299
		private const string AssetsPrefix = "coui://assets/";

		// Token: 0x0400206C RID: 8300
		private readonly App _app;
	}
}
