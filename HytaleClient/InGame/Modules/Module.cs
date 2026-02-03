using System;
using HytaleClient.Core;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008F7 RID: 2295
	internal abstract class Module : Disposable
	{
		// Token: 0x06004453 RID: 17491 RVA: 0x000E6D1A File Offset: 0x000E4F1A
		public Module(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x000E6D2B File Offset: 0x000E4F2B
		public virtual void Initialize()
		{
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x000E6D2E File Offset: 0x000E4F2E
		[Obsolete]
		public virtual void Tick()
		{
			throw new Exception("Module.Tick should never be called directly!");
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x000E6D3B File Offset: 0x000E4F3B
		[Obsolete]
		public virtual void OnNewFrame(float deltaTime)
		{
			throw new Exception("Module.OnNewFrame should never be called directly!");
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x000E6D48 File Offset: 0x000E4F48
		protected override void DoDispose()
		{
		}

		// Token: 0x040021B6 RID: 8630
		protected GameInstance _gameInstance;
	}
}
