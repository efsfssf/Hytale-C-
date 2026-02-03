using System;
using HytaleClient.Data.Items;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools.Client
{
	// Token: 0x02000989 RID: 2441
	internal class LineTool : ClientTool
	{
		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06004D79 RID: 19833 RVA: 0x0014DE31 File Offset: 0x0014C031
		public override string ToolId
		{
			get
			{
				return "Line";
			}
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x0014DE38 File Offset: 0x0014C038
		public LineTool(GameInstance gameInstance) : base(gameInstance)
		{
			this._boxRenderer = new BoxRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			this._lineRenderer = new LineRenderer(this._graphics, this._graphics.GPUProgramStore.BasicProgram);
			Vector3 value = new Vector3(0.05f, 0.05f, 0.05f);
			this._blockBox = new BoundingBox(Vector3.Zero - value, Vector3.One + value);
			this._lineOffset = Vector3.One * 0.5f;
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x0014DEDC File Offset: 0x0014C0DC
		protected override void DoDispose()
		{
			this._boxRenderer.Dispose();
			this._lineRenderer.Dispose();
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x0014DEF8 File Offset: 0x0014C0F8
		public override void Update(float deltaTime)
		{
			bool flag = base.BrushTarget.IsNaN();
			if (flag)
			{
				this._hasTarget = false;
			}
			else
			{
				bool flag2 = base.BrushTarget == this._target;
				if (!flag2)
				{
					this._target = base.BrushTarget;
					this._hasTarget = true;
					bool flag3 = this._gameInstance.Input.IsShiftHeld();
					bool flag4 = this._lineStarted && flag3;
					if (flag4)
					{
						Vector3 vector = Vector3.Normalize(this._origin - this._target);
						float scaleFactor = Vector3.Distance(this._origin, this._target);
						float value = (float)Math.Asin((double)(-(double)vector.Y));
						float value2 = (float)Math.Atan2((double)vector.X, (double)vector.Z);
						float interval = MathHelper.ToRadians(45f);
						float yaw = MathHelper.SnapValue(value2, interval);
						float pitch = MathHelper.SnapValue(value, interval);
						Matrix matrix;
						Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f, out matrix);
						Vector3 value3 = Vector3.Transform(Vector3.Forward, matrix);
						this._target = this._origin + value3 * scaleFactor;
						this._target = new Vector3((float)Math.Floor((double)this._target.X), (float)Math.Floor((double)this._target.Y), (float)Math.Floor((double)this._target.Z));
					}
					bool lineStarted = this._lineStarted;
					if (lineStarted)
					{
						this._lineRenderer.UpdateLineData(new Vector3[]
						{
							this._origin + this._lineOffset,
							this._target + this._lineOffset
						});
					}
				}
			}
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x0014E0B8 File Offset: 0x0014C2B8
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			base.Draw(ref viewProjectionMatrix);
			Vector3 cameraPosition = this._gameInstance.SceneRenderer.Data.CameraPosition;
			bool hasTarget = this._hasTarget;
			if (hasTarget)
			{
				this._boxRenderer.Draw(this._target - cameraPosition, this._blockBox, viewProjectionMatrix, this._graphics.WhiteColor, 0.3f, this._graphics.WhiteColor, 0.2f);
			}
			bool lineStarted = this._lineStarted;
			if (lineStarted)
			{
				this._boxRenderer.Draw(this._origin - cameraPosition, this._blockBox, viewProjectionMatrix, this._graphics.WhiteColor, 0.3f, this._graphics.WhiteColor, 0.2f);
			}
			bool flag = this._hasTarget && this._lineStarted;
			if (flag)
			{
				Vector3 vector = this._origin - this._target;
				Vector3 color = this._graphics.WhiteColor;
				bool flag2 = vector.X == 0f && vector.Z == 0f && vector.Y != 0f;
				if (flag2)
				{
					color = this._graphics.GreenColor;
				}
				else
				{
					bool flag3 = vector.Y == 0f && vector.Z == 0f && vector.X != 0f;
					if (flag3)
					{
						color = this._graphics.RedColor;
					}
					else
					{
						bool flag4 = vector.X == 0f && vector.Y == 0f && vector.Z != 0f;
						if (flag4)
						{
							color = this._graphics.BlueColor;
						}
					}
				}
				this._lineRenderer.Draw(ref this._gameInstance.SceneRenderer.Data.ViewProjectionMatrix, color, 0.75f);
			}
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x0014E2B8 File Offset: 0x0014C4B8
		public override bool NeedsDrawing()
		{
			return this._hasTarget || this._lineStarted;
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x0014E2DC File Offset: 0x0014C4DC
		public override void OnInteraction(InteractionType interactionType, InteractionModule.ClickType clickType, InteractionContext context, bool firstRun)
		{
			bool flag = clickType == InteractionModule.ClickType.None;
			if (!flag)
			{
				bool flag2 = interactionType == 0;
				if (flag2)
				{
					this._lineStarted = false;
				}
				else
				{
					bool lineStarted = this._lineStarted;
					if (lineStarted)
					{
						this.OnLineAction(this._origin, this._target);
					}
					switch (this.LineType)
					{
					case LineTool.LineConnectionType.Continous:
					{
						bool flag3 = !this._lineStarted;
						if (flag3)
						{
							this._lineStarted = true;
						}
						this._origin = this._target;
						break;
					}
					case LineTool.LineConnectionType.Split:
					{
						bool flag4 = !this._lineStarted;
						if (flag4)
						{
							this._lineStarted = true;
							this._origin = this._target;
						}
						else
						{
							this._lineStarted = false;
						}
						break;
					}
					case LineTool.LineConnectionType.Origin:
					{
						bool flag5 = !this._lineStarted;
						if (flag5)
						{
							this._lineStarted = true;
							this._origin = this._target;
						}
						break;
					}
					}
				}
			}
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x0014E3C4 File Offset: 0x0014C5C4
		public override void OnToolItemChange(ClientItemStack itemStack)
		{
			BuilderTool toolFromItemStack = BuilderTool.GetToolFromItemStack(this._gameInstance, itemStack);
			bool flag;
			if (toolFromItemStack != null)
			{
				object obj;
				if (itemStack == null)
				{
					obj = null;
				}
				else
				{
					JObject metadata = itemStack.Metadata;
					obj = ((metadata != null) ? metadata["ToolData"] : null);
				}
				flag = (obj == null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				this.LineRadius = int.Parse(toolFromItemStack.GetItemArgValueOrDefault(ref itemStack, "LineRadius"));
				this.LineType = (LineTool.LineConnectionType)Enum.Parse(typeof(LineTool.LineConnectionType), toolFromItemStack.GetItemArgValueOrDefault(ref itemStack, "LineType"));
			}
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x0014E44C File Offset: 0x0014C64C
		private void OnLineAction(Vector3 start, Vector3 end)
		{
			this._gameInstance.Connection.SendPacket(new BuilderToolLineAction((int)start.X, (int)start.Y, (int)start.Z, (int)end.X, (int)end.Y, (int)end.Z));
		}

		// Token: 0x040028BA RID: 10426
		private const string TypeArgKey = "LineType";

		// Token: 0x040028BB RID: 10427
		private const string RadiusArgKey = "LineRadius";

		// Token: 0x040028BC RID: 10428
		private const int SnapAngleDegrees = 45;

		// Token: 0x040028BD RID: 10429
		private readonly BoxRenderer _boxRenderer;

		// Token: 0x040028BE RID: 10430
		private readonly LineRenderer _lineRenderer;

		// Token: 0x040028BF RID: 10431
		private readonly BoundingBox _blockBox;

		// Token: 0x040028C0 RID: 10432
		private readonly Vector3 _lineOffset;

		// Token: 0x040028C1 RID: 10433
		private int LineRadius;

		// Token: 0x040028C2 RID: 10434
		private LineTool.LineConnectionType LineType;

		// Token: 0x040028C3 RID: 10435
		private bool _lineStarted;

		// Token: 0x040028C4 RID: 10436
		private bool _hasTarget;

		// Token: 0x040028C5 RID: 10437
		private Vector3 _origin;

		// Token: 0x040028C6 RID: 10438
		private Vector3 _target;

		// Token: 0x02000E74 RID: 3700
		private enum LineConnectionType
		{
			// Token: 0x04004673 RID: 18035
			Continous,
			// Token: 0x04004674 RID: 18036
			Split,
			// Token: 0x04004675 RID: 18037
			Origin
		}
	}
}
