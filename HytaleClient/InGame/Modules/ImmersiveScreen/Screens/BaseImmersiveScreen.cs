using System;
using HytaleClient.Core;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.ImmersiveScreen.Screens
{
	// Token: 0x0200093D RID: 2365
	internal abstract class BaseImmersiveScreen : Disposable
	{
		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x0011A534 File Offset: 0x00118734
		// (set) Token: 0x060048D2 RID: 18642 RVA: 0x0011A53C File Offset: 0x0011873C
		public Vector3 BlockPosition { get; protected set; }

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x060048D3 RID: 18643 RVA: 0x0011A545 File Offset: 0x00118745
		// (set) Token: 0x060048D4 RID: 18644 RVA: 0x0011A54D File Offset: 0x0011874D
		public Vector2 ScreenSizeInPixels { get; private set; }

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x060048D5 RID: 18645 RVA: 0x0011A556 File Offset: 0x00118756
		// (set) Token: 0x060048D6 RID: 18646 RVA: 0x0011A55E File Offset: 0x0011875E
		public float MaxVisibilityDistance { get; protected set; }

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x060048D7 RID: 18647 RVA: 0x0011A567 File Offset: 0x00118767
		// (set) Token: 0x060048D8 RID: 18648 RVA: 0x0011A56F File Offset: 0x0011876F
		public float MaxSoundDistance { get; protected set; }

		// Token: 0x060048D9 RID: 18649 RVA: 0x0011A578 File Offset: 0x00118778
		public BaseImmersiveScreen(GameInstance gameInstance, Vector3 blockPosition, ImmersiveView.ViewScreen screen)
		{
			this._gameInstance = gameInstance;
			this.BlockPosition = blockPosition;
			this._screenOffset = new Vector3(screen.OffsetX, screen.OffsetY, screen.OffsetZ);
			this._isBillboard = screen.UseBillboardRotation;
			this._screenDirection = new Vector2(MathHelper.ToRadians(screen.Yaw), MathHelper.ToRadians(screen.Pitch));
			this._screenAutoRotationSpeed = new Vector2(screen.YawRotateSpeed, screen.PitchRotateSpeed);
			this._screenSizeInBlocks = new Vector2(screen.SizeX, screen.SizeY);
			int num = Math.Max(screen.Resolution, 1);
			float num2 = this._screenSizeInBlocks.Y / this._screenSizeInBlocks.X;
			this.ScreenSizeInPixels = ((num2 < 1f) ? new Vector2((float)num, (float)num * num2) : new Vector2((float)num / num2, (float)num));
			this._screenPixelsToBlockRatio = new Vector2(this._screenSizeInBlocks.X / this.ScreenSizeInPixels.X, this._screenSizeInBlocks.Y / this.ScreenSizeInPixels.Y);
			this.MaxSoundDistance = (float)screen.MaxSoundDistance;
			this.MaxVisibilityDistance = (float)screen.MaxVisibilityDistance;
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x0011A6B8 File Offset: 0x001188B8
		public void Update(float deltaTime)
		{
			bool isBillboard = this._isBillboard;
			if (isBillboard)
			{
				Vector3 vector = this.GetOffsetPosition() - this._gameInstance.LocalPlayer.Position;
				vector.Normalize();
				this._screenDirection = new Vector2((float)Math.Atan2((double)vector.X, (double)vector.Z) + 3.1415927f, 0f);
			}
			else
			{
				this._screenDirection.X = MathHelper.WrapAngle(this._screenDirection.X + MathHelper.ToRadians(this._screenAutoRotationSpeed.X) * 60f * deltaTime);
				this._screenDirection.Y = MathHelper.WrapAngle(this._screenDirection.Y + MathHelper.ToRadians(this._screenAutoRotationSpeed.Y) * 60f * deltaTime);
			}
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x0011A78C File Offset: 0x0011898C
		public bool NeedsDrawing()
		{
			bool flag = Vector3.Distance(this._gameInstance.LocalPlayer.Position, this.GetOffsetPosition()) < this.MaxVisibilityDistance;
			bool result;
			if (flag)
			{
				bool flag2 = this is ImmersiveWebScreen && this != this._gameInstance.ImmersiveScreenModule.ActiveWebScreen;
				result = !flag2;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x0011A7F4 File Offset: 0x001189F4
		public void PrepareForDraw(ref Matrix viewProjectionMatrix)
		{
			bool flag = !this.NeedsDrawing();
			if (flag)
			{
				throw new Exception("PrepareForDraw called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
			Vector3 vector = new Vector3(this._screenSizeInBlocks.X / -2f, 0f, 0f);
			Vector3 offsetPosition = this.GetOffsetPosition();
			Matrix matrix;
			Matrix.CreateFromYawPitchRoll(this._screenDirection.X, this._screenDirection.Y, 0f, out matrix);
			Matrix matrix2;
			Matrix.CreateTranslation(ref vector, out matrix2);
			Matrix.Multiply(ref matrix2, ref matrix, out matrix);
			Matrix.CreateTranslation(ref offsetPosition, out matrix2);
			Matrix.Multiply(ref matrix, ref matrix2, out matrix);
			Matrix.Multiply(ref matrix, ref viewProjectionMatrix, out matrix);
			Matrix.CreateScale(this._screenPixelsToBlockRatio.X * this.ScreenSizeInPixels.X, this._screenPixelsToBlockRatio.Y * this.ScreenSizeInPixels.Y, 1f, out this._mvpMatrix);
			Matrix.Multiply(ref this._mvpMatrix, ref matrix, out this._mvpMatrix);
		}

		// Token: 0x060048DD RID: 18653
		public abstract void Draw();

		// Token: 0x060048DE RID: 18654 RVA: 0x0011A8F4 File Offset: 0x00118AF4
		public Vector3 GetOffsetPosition()
		{
			Vector3 value = new Vector3(this.BlockPosition.X + 0.5f, this.BlockPosition.Y, this.BlockPosition.Z + 0.5f);
			bool isBillboard = this._isBillboard;
			Vector3 result;
			if (isBillboard)
			{
				result = value + this._screenOffset;
			}
			else
			{
				Vector3 screenOffset = this._screenOffset;
				Matrix matrix;
				Matrix.CreateFromYawPitchRoll(this._screenDirection.X, this._screenDirection.Y, 0f, out matrix);
				Matrix matrix2;
				Matrix.CreateTranslation(ref screenOffset, out matrix2);
				Matrix.Multiply(ref matrix2, ref matrix, out matrix);
				result = value + matrix.Translation;
			}
			return result;
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x0011A9A8 File Offset: 0x00118BA8
		public Vector2 ViewOffsetToPixelPosition(Vector2 viewOffset)
		{
			return new Vector2((float)Math.Round((double)(this.ScreenSizeInPixels.X - viewOffset.X * this.ScreenSizeInPixels.X)), (float)Math.Round((double)(viewOffset.Y * this.ScreenSizeInPixels.Y)));
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x0011AA00 File Offset: 0x00118C00
		public bool CheckRayIntersection(Ray viewRay, out ViewPlaneIntersection intersection)
		{
			Vector3 offsetPosition = this.GetOffsetPosition();
			Vector3 vector = viewRay.Position - offsetPosition;
			Vector3 vector2 = new Vector3(this._screenSizeInBlocks.X / 2f, 0f, 0f);
			Matrix matrix;
			Matrix.CreateFromYawPitchRoll(-this._screenDirection.X, -this._screenDirection.Y, 0f, out matrix);
			Matrix matrix2;
			Matrix.CreateTranslation(ref vector, out matrix2);
			Matrix.Multiply(ref matrix2, ref matrix, out matrix2);
			Vector3 direction = Vector3.Transform(viewRay.Direction, matrix);
			Matrix.CreateTranslation(ref vector2, out matrix);
			Matrix.Multiply(ref matrix2, ref matrix, out matrix2);
			Ray ray = new Ray(matrix2.Translation + offsetPosition, direction);
			intersection = default(ViewPlaneIntersection);
			bool flag = ray.Direction.Z <= 0f;
			if (flag)
			{
				float num = (ray.Position.Z - offsetPosition.Z) / -ray.Direction.Z;
				Vector3 vector3 = new Vector3(ray.Position.X + num * ray.Direction.X, ray.Position.Y + num * ray.Direction.Y, ray.Position.Z + num * ray.Direction.Z);
				bool flag2 = vector3.Y >= offsetPosition.Y && vector3.Y <= offsetPosition.Y + this._screenSizeInBlocks.Y && vector3.X >= offsetPosition.X && vector3.X <= offsetPosition.X + this._screenSizeInBlocks.X;
				if (flag2)
				{
					Vector2 pixelPos = new Vector2(1f - (vector3.X - offsetPosition.X) / this._screenSizeInBlocks.X, (vector3.Y - offsetPosition.Y) / this._screenSizeInBlocks.Y);
					Vector3 worldPos = viewRay.Position + viewRay.Direction * num;
					intersection = new ViewPlaneIntersection(worldPos, pixelPos);
					return true;
				}
			}
			return false;
		}

		// Token: 0x040024D6 RID: 9430
		protected GameInstance _gameInstance;

		// Token: 0x040024D8 RID: 9432
		private Vector3 _screenOffset;

		// Token: 0x040024D9 RID: 9433
		private bool _isBillboard;

		// Token: 0x040024DA RID: 9434
		private Vector2 _screenDirection;

		// Token: 0x040024DB RID: 9435
		private Vector2 _screenAutoRotationSpeed;

		// Token: 0x040024DC RID: 9436
		private Vector2 _screenSizeInBlocks;

		// Token: 0x040024DD RID: 9437
		private Vector2 _screenPixelsToBlockRatio;

		// Token: 0x040024E1 RID: 9441
		protected Matrix _mvpMatrix;
	}
}
