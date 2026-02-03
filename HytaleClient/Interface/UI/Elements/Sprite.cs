using System;
using System.Diagnostics;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000875 RID: 2165
	[UIMarkupElement]
	public class Sprite : Element
	{
		// Token: 0x1700107F RID: 4223
		// (set) Token: 0x06003D26 RID: 15654 RVA: 0x0009B7C8 File Offset: 0x000999C8
		[UIMarkupProperty]
		public Sprite.SpriteFrame Frame
		{
			set
			{
				this._frame = value;
				this._frameProgress = 0f;
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this.EnsureAutoPlay();
					this.UpdateAnimationCallback();
				}
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x06003D27 RID: 15655 RVA: 0x0009B802 File Offset: 0x00099A02
		// (set) Token: 0x06003D28 RID: 15656 RVA: 0x0009B80C File Offset: 0x00099A0C
		[UIMarkupProperty]
		public bool AutoPlay
		{
			get
			{
				return this._autoPlay;
			}
			set
			{
				this._autoPlay = value;
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this.EnsureAutoPlay();
					this.UpdateAnimationCallback();
				}
			}
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x0009B83B File Offset: 0x00099A3B
		public Sprite(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x0009B856 File Offset: 0x00099A56
		protected override void OnMounted()
		{
			this.EnsureAutoPlay();
			this.UpdateAnimationCallback();
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x0009B868 File Offset: 0x00099A68
		protected override void OnUnmounted()
		{
			bool isAnimationCallbackRegistered = this._isAnimationCallbackRegistered;
			if (isAnimationCallbackRegistered)
			{
				this._isAnimationCallbackRegistered = false;
				this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			}
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x0009B8A4 File Offset: 0x00099AA4
		public void Play()
		{
			bool isPlaying = this._isPlaying;
			if (!isPlaying)
			{
				this._isPlaying = true;
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this.UpdateAnimationCallback();
				}
			}
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x0009B8D8 File Offset: 0x00099AD8
		public void Pause()
		{
			bool flag = !this._isPlaying;
			if (!flag)
			{
				this._isPlaying = false;
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this.UpdateAnimationCallback();
				}
			}
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x0009B910 File Offset: 0x00099B10
		public void Stop()
		{
			bool flag = !this._isPlaying;
			if (!flag)
			{
				this._isPlaying = false;
				this._frameProgress = 0f;
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this.UpdateAnimationCallback();
				}
			}
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x0009B950 File Offset: 0x00099B50
		public void Reset()
		{
			this._frameProgress = 0f;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x0009B960 File Offset: 0x00099B60
		private void EnsureAutoPlay()
		{
			bool flag = this._frame.Count > 1 && this.AutoPlay && !this._isPlaying;
			if (flag)
			{
				this._isPlaying = true;
				this._frameProgress = 0f;
			}
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x0009B9A8 File Offset: 0x00099BA8
		private void UpdateAnimationCallback()
		{
			Debug.Assert(base.IsMounted);
			bool flag = this._frame.Count > 1;
			bool flag2 = flag && this._isPlaying;
			bool flag3 = this._isAnimationCallbackRegistered && !flag2;
			if (flag3)
			{
				this._isAnimationCallbackRegistered = false;
				this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			}
			else
			{
				bool flag4 = !this._isAnimationCallbackRegistered && flag2;
				if (flag4)
				{
					this._isAnimationCallbackRegistered = true;
					this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
				}
			}
			bool flag5 = this._texture != null;
			if (flag5)
			{
				this.SetupSourceRectangle();
			}
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x0009BA5A File Offset: 0x00099C5A
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._texture = this.Desktop.Provider.MakeTextureArea(this.TexturePath.Value);
			this.SetupSourceRectangle();
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x0009BA8C File Offset: 0x00099C8C
		private void Animate(float deltaTime)
		{
			bool flag = !this._isPlaying;
			if (!flag)
			{
				this._frameProgress += deltaTime * (float)this.FramesPerSecond;
				this.SetupSourceRectangle();
				bool flag2 = this.RepeatCount > 0 && this._frameProgress >= (float)(this.RepeatCount * this._frame.Count);
				if (flag2)
				{
					this._isPlaying = false;
					this.UpdateAnimationCallback();
				}
			}
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x0009BB04 File Offset: 0x00099D04
		private void SetupSourceRectangle()
		{
			int num = (int)this._frameProgress % this._frame.Count;
			int num2 = num / this._frame.PerRow;
			int num3 = num - num2 * this._frame.PerRow;
			this._sourceRectangle = new Rectangle(num3 * this._frame.Width * this._texture.Scale + this._texture.Rectangle.X, num2 * this._frame.Height * this._texture.Scale + this._texture.Rectangle.Y, this._frame.Width * this._texture.Scale, this._frame.Height * this._texture.Scale);
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x0009BBD4 File Offset: 0x00099DD4
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			this.Desktop.Batcher2D.SetTransformationMatrix(new Vector3((float)this._rectangleAfterPadding.Center.X, (float)this._rectangleAfterPadding.Center.Y, 0f), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(this.Angle)), 1f);
			Rectangle destRect = new Rectangle(-this._rectangleAfterPadding.Width / 2, -this._rectangleAfterPadding.Height / 2, this._rectangleAfterPadding.Width, this._rectangleAfterPadding.Height);
			this.Desktop.Batcher2D.RequestDrawPatch(this._texture.Texture, this._sourceRectangle, 0, 0, this._texture.Scale, destRect, this.Desktop.Scale, UInt32Color.White);
			this.Desktop.Batcher2D.SetTransformationMatrix(Matrix.Identity);
		}

		// Token: 0x04001C73 RID: 7283
		[UIMarkupProperty]
		public UIPath TexturePath;

		// Token: 0x04001C74 RID: 7284
		private TextureArea _texture;

		// Token: 0x04001C75 RID: 7285
		private Sprite.SpriteFrame _frame;

		// Token: 0x04001C76 RID: 7286
		[UIMarkupProperty]
		public int FramesPerSecond = 20;

		// Token: 0x04001C77 RID: 7287
		[UIMarkupProperty]
		public float Angle;

		// Token: 0x04001C78 RID: 7288
		[UIMarkupProperty]
		public int RepeatCount;

		// Token: 0x04001C79 RID: 7289
		private bool _isPlaying;

		// Token: 0x04001C7A RID: 7290
		private bool _autoPlay = true;

		// Token: 0x04001C7B RID: 7291
		private float _frameProgress;

		// Token: 0x04001C7C RID: 7292
		private Rectangle _sourceRectangle;

		// Token: 0x04001C7D RID: 7293
		private bool _isAnimationCallbackRegistered;

		// Token: 0x02000D40 RID: 3392
		[UIMarkupData]
		public class SpriteFrame
		{
			// Token: 0x04004149 RID: 16713
			public int Width;

			// Token: 0x0400414A RID: 16714
			public int Height;

			// Token: 0x0400414B RID: 16715
			public int PerRow;

			// Token: 0x0400414C RID: 16716
			public int Count;
		}
	}
}
