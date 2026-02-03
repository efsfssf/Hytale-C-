using System;
using HytaleClient.Core;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x02000982 RID: 2434
	internal abstract class ClientTool : Disposable
	{
		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06004D40 RID: 19776
		public abstract string ToolId { get; }

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06004D41 RID: 19777 RVA: 0x0014B528 File Offset: 0x00149728
		protected Vector3 BrushTarget
		{
			get
			{
				return this._builderTools.BrushTargetPosition;
			}
		}

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06004D42 RID: 19778 RVA: 0x0014B538 File Offset: 0x00149738
		// (set) Token: 0x06004D43 RID: 19779 RVA: 0x0014B550 File Offset: 0x00149750
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			private set
			{
				bool flag = value == this._isActive;
				if (!flag)
				{
					this._isActive = value;
					this.OnActiveStateChange(this._isActive);
				}
			}
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x0014B581 File Offset: 0x00149781
		public ClientTool(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			this._builderTools = gameInstance.BuilderToolsModule;
			this._graphics = gameInstance.Engine.Graphics;
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x0014B5AF File Offset: 0x001497AF
		public void SetActive(ClientItemStack itemStack)
		{
			this.IsActive = true;
			this.OnToolItemChange(itemStack);
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x0014B5C2 File Offset: 0x001497C2
		public void SetInactive()
		{
			this.IsActive = false;
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x0014B5CD File Offset: 0x001497CD
		protected override void DoDispose()
		{
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x0014B5D0 File Offset: 0x001497D0
		public virtual void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x0014B5D3 File Offset: 0x001497D3
		public virtual void Update(float deltaTime)
		{
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0014B5D6 File Offset: 0x001497D6
		protected virtual void OnActiveStateChange(bool newState)
		{
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x0014B5D9 File Offset: 0x001497D9
		public virtual void OnToolItemChange(ClientItemStack toolItem)
		{
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x0014B5DC File Offset: 0x001497DC
		public virtual bool NeedsDrawing()
		{
			return false;
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x0014B5DF File Offset: 0x001497DF
		public virtual bool NeedsTextDrawing()
		{
			return false;
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x0014B5E4 File Offset: 0x001497E4
		public virtual void Draw(ref Matrix viewProjectionMatrix)
		{
			bool flag = !this.NeedsDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x0014B60C File Offset: 0x0014980C
		public virtual void DrawText(ref Matrix viewProjectionMatrix)
		{
			bool flag = !this.NeedsTextDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with NeedsTextDrawing() first before calling this.");
			}
		}

		// Token: 0x0400287F RID: 10367
		protected readonly GameInstance _gameInstance;

		// Token: 0x04002880 RID: 10368
		protected readonly BuilderToolsModule _builderTools;

		// Token: 0x04002881 RID: 10369
		protected readonly GraphicsDevice _graphics;

		// Token: 0x04002882 RID: 10370
		private bool _isActive;
	}
}
