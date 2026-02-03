using System;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x02000981 RID: 2433
	internal class AnchorTool : ClientTool
	{
		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06004D3A RID: 19770 RVA: 0x0014B416 File Offset: 0x00149616
		public override string ToolId
		{
			get
			{
				return "SetAnchor";
			}
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x0014B41D File Offset: 0x0014961D
		public AnchorTool(GameInstance gameInstance) : base(gameInstance)
		{
			this._boxRenderer = new BoxRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x0014B449 File Offset: 0x00149649
		public void ShowAnchor(Vector3 position)
		{
			this._target = position;
			this._enabled = true;
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x0014B45A File Offset: 0x0014965A
		public void HideAnchors()
		{
			this._enabled = false;
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x0014B464 File Offset: 0x00149664
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			base.Draw(ref viewProjectionMatrix);
			GLFunctions gl = this._graphics.GL;
			Vector3 magentaColor = this._graphics.MagentaColor;
			Vector3 cameraPosition = this._gameInstance.SceneRenderer.Data.CameraPosition;
			gl.DepthFunc(GL.ALWAYS);
			this._boxRenderer.Draw(this._target - cameraPosition, new BoundingBox(Vector3.Zero, Vector3.One), viewProjectionMatrix, magentaColor, 0.4f, magentaColor, 0.03f);
			gl.DepthFunc((!this._graphics.UseReverseZ) ? GL.LEQUAL : GL.GEQUAL);
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x0014B510 File Offset: 0x00149710
		public override bool NeedsDrawing()
		{
			return this._enabled;
		}

		// Token: 0x0400287C RID: 10364
		private BoxRenderer _boxRenderer;

		// Token: 0x0400287D RID: 10365
		private Vector3 _target;

		// Token: 0x0400287E RID: 10366
		private bool _enabled;
	}
}
