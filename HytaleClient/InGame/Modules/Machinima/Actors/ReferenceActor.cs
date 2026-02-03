using System;
using Coherent.UI.Binding;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Machinima.Actors
{
	// Token: 0x0200092A RID: 2346
	[CoherentType]
	internal class ReferenceActor : SceneActor
	{
		// Token: 0x0600478D RID: 18317 RVA: 0x0010E734 File Offset: 0x0010C934
		protected override ActorType GetActorType()
		{
			return ActorType.Reference;
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x0010E738 File Offset: 0x0010C938
		public ReferenceActor(GameInstance gameInstance, string name) : base(gameInstance, name)
		{
			this._gameInstance = gameInstance;
			this._boxRenderer = new BoxRenderer(this._gameInstance.Engine.Graphics, this._gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram);
			this._boxColor = this._gameInstance.Engine.Graphics.YellowColor;
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x0010E7A6 File Offset: 0x0010C9A6
		protected override void DoDispose()
		{
			this._boxRenderer.Dispose();
			base.DoDispose();
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0010E7BC File Offset: 0x0010C9BC
		public override void Draw(ref Matrix viewProjectionMatrix)
		{
			base.Draw(ref viewProjectionMatrix);
			Vector3 position = this.Position;
			Vector3 vector = Vector3.One / -2f;
			this._modelMatrix = Matrix.Identity;
			Matrix.CreateTranslation(ref vector, out this._tempMatrix);
			Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
			Matrix.CreateScale(0.375f, out this._tempMatrix);
			Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
			Matrix.CreateFromYawPitchRoll(this.Look.Y + 1.5707964f, 0f, this.Look.X, out this._tempMatrix);
			Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(this.Look.Yaw, this.Look.Pitch, this.Look.Roll);
			Matrix.CreateFromQuaternion(ref quaternion, out this._tempMatrix);
			Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
			Matrix.CreateTranslation(ref position, out this._tempMatrix);
			Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
			Matrix.Multiply(ref this._modelMatrix, ref viewProjectionMatrix, out this._modelMatrix);
			this._boxRenderer.Draw(ref this._modelMatrix, this._boxColor, 0.7f, this._boxColor, 0.2f);
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0010E920 File Offset: 0x0010CB20
		public override SceneActor Clone(GameInstance gameInstance)
		{
			SceneActor result = new ReferenceActor(gameInstance, base.Name + "-copy");
			base.Track.CopyToActor(ref result);
			return result;
		}

		// Token: 0x040023EE RID: 9198
		private const float ActorBoxScale = 0.375f;

		// Token: 0x040023EF RID: 9199
		private GameInstance _gameInstance;

		// Token: 0x040023F0 RID: 9200
		private readonly BoxRenderer _boxRenderer;

		// Token: 0x040023F1 RID: 9201
		private Vector3 _boxColor;
	}
}
