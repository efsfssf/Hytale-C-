using System;
using HytaleClient.Core;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Fonts
{
	// Token: 0x02000AC0 RID: 2752
	internal class TextRenderer : Disposable
	{
		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x060056C7 RID: 22215 RVA: 0x001A168C File Offset: 0x0019F88C
		// (set) Token: 0x060056C8 RID: 22216 RVA: 0x001A16A4 File Offset: 0x0019F8A4
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				bool flag = value != this._text;
				if (flag)
				{
					this._text = value;
					this.BuildGeometry();
				}
			}
		}

		// Token: 0x17001344 RID: 4932
		// (get) Token: 0x060056C9 RID: 22217 RVA: 0x001A16D2 File Offset: 0x0019F8D2
		public ushort IndicesCount
		{
			get
			{
				return (ushort)this._indices.Length;
			}
		}

		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x060056CA RID: 22218 RVA: 0x001A16DD File Offset: 0x0019F8DD
		// (set) Token: 0x060056CB RID: 22219 RVA: 0x001A16E5 File Offset: 0x0019F8E5
		public GLVertexArray VertexArray { get; private set; }

		// Token: 0x060056CC RID: 22220 RVA: 0x001A16F0 File Offset: 0x0019F8F0
		public TextRenderer(GraphicsDevice graphics, Font font, string text, uint fillColor = 4294967295U, uint outlineColor = 4278190080U)
		{
			this._graphics = graphics;
			this._font = font;
			this._text = text;
			this.FillColor = fillColor;
			this.OutlineColor = outlineColor;
			GLFunctions gl = this._graphics.GL;
			this.VertexArray = gl.GenVertexArray();
			this._verticesBuffer = gl.GenBuffer();
			this._indicesBuffer = gl.GenBuffer();
			gl.BindVertexArray(this.VertexArray);
			gl.BindBuffer(this.VertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			TextProgram textProgram = this._graphics.GPUProgramStore.TextProgram;
			IntPtr pointer = IntPtr.Zero;
			gl.EnableVertexAttribArray(textProgram.AttribPosition.Index);
			gl.VertexAttribPointer(textProgram.AttribPosition.Index, 3, GL.FLOAT, false, TextVertex.Size, pointer);
			pointer += 12;
			gl.EnableVertexAttribArray(textProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(textProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, TextVertex.Size, pointer);
			pointer += 8;
			gl.EnableVertexAttribArray(textProgram.AttribFillColor.Index);
			gl.VertexAttribIPointer(textProgram.AttribFillColor.Index, 1, GL.UNSIGNED_INT, TextVertex.Size, pointer);
			pointer += 4;
			gl.EnableVertexAttribArray(textProgram.AttribOutlineColor.Index);
			gl.VertexAttribIPointer(textProgram.AttribOutlineColor.Index, 1, GL.UNSIGNED_INT, TextVertex.Size, pointer);
			pointer += 4;
			this.BuildGeometry();
		}

		// Token: 0x060056CD RID: 22221 RVA: 0x001A1898 File Offset: 0x0019FA98
		private unsafe void BuildGeometry()
		{
			this._vertices = new TextVertex[this._text.Length * 4];
			this._indices = new ushort[this._text.Length * 6];
			for (int i = 0; i < this._text.Length; i++)
			{
				this._indices[i * 6] = (ushort)(i * 4);
				this._indices[i * 6 + 1] = (ushort)(i * 4 + 1);
				this._indices[i * 6 + 2] = (ushort)(i * 4 + 2);
				this._indices[i * 6 + 3] = (ushort)(i * 4);
				this._indices[i * 6 + 4] = (ushort)(i * 4 + 2);
				this._indices[i * 6 + 5] = (ushort)(i * 4 + 3);
			}
			int num = 0;
			this._textWidth = 0f;
			this._textHeight = (float)this._font.LineSkip;
			Vector3 vector = new Vector3((float)(-(float)this._font.Spread / 2), 0f, 0f);
			foreach (ushort key in this._text)
			{
				Rectangle rectangle;
				bool flag = this._font.GlyphAtlasRectangles.TryGetValue(key, out rectangle);
				if (flag)
				{
					float x = (float)rectangle.X / (float)this._font.TextureAtlas.Width;
					float x2 = (float)(rectangle.X + rectangle.Width) / (float)this._font.TextureAtlas.Width;
					float y = (float)rectangle.Y / (float)this._font.TextureAtlas.Height;
					float y2 = (float)(rectangle.Y + rectangle.Height) / (float)this._font.TextureAtlas.Height;
					this._vertices[num * 4].TextureCoordinates = new Vector2(x2, y);
					this._vertices[num * 4 + 1].TextureCoordinates = new Vector2(x, y);
					this._vertices[num * 4 + 3].TextureCoordinates = new Vector2(x2, y2);
					this._vertices[num * 4 + 2].TextureCoordinates = new Vector2(x, y2);
					this._vertices[num * 4].Position = vector + new Vector3((float)rectangle.Width, (float)rectangle.Height, 0f);
					this._vertices[num * 4 + 1].Position = vector + new Vector3(0f, (float)rectangle.Height, 0f);
					this._vertices[num * 4 + 2].Position = vector;
					this._vertices[num * 4 + 3].Position = vector + new Vector3((float)rectangle.Width, 0f, 0f);
					this._vertices[num * 4].FillColor = this.FillColor;
					this._vertices[num * 4 + 1].FillColor = this.FillColor;
					this._vertices[num * 4 + 2].FillColor = this.FillColor;
					this._vertices[num * 4 + 3].FillColor = this.FillColor;
					this._vertices[num * 4].OutlineColor = this.OutlineColor;
					this._vertices[num * 4 + 1].OutlineColor = this.OutlineColor;
					this._vertices[num * 4 + 2].OutlineColor = this.OutlineColor;
					this._vertices[num * 4 + 3].OutlineColor = this.OutlineColor;
					float num2 = this._font.GlyphAdvances[key];
					vector += new Vector3(num2, 0f, 0f);
					this._textWidth += num2;
				}
				num++;
			}
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this.VertexArray);
			gl.BindBuffer(this.VertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			gl.BindBuffer(this.VertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			TextVertex[] array;
			TextVertex* value;
			if ((array = this._vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._vertices.Length * TextVertex.Size), (IntPtr)((void*)value), GL.STATIC_DRAW);
			array = null;
			ushort[] array2;
			ushort* value2;
			if ((array2 = this._indices) == null || array2.Length == 0)
			{
				value2 = null;
			}
			else
			{
				value2 = &array2[0];
			}
			gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(this._indices.Length * 2), (IntPtr)((void*)value2), GL.STATIC_DRAW);
			array2 = null;
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x001A1D94 File Offset: 0x0019FF94
		public float GetHorizontalOffset(TextRenderer.TextAlignment alignment)
		{
			float result;
			switch (alignment)
			{
			case TextRenderer.TextAlignment.Left:
				result = 0f;
				break;
			case TextRenderer.TextAlignment.Center:
				result = this._textWidth / 2f;
				break;
			case TextRenderer.TextAlignment.Right:
				result = this._textWidth;
				break;
			default:
				throw new Exception("Unreachable");
			}
			return result;
		}

		// Token: 0x060056CF RID: 22223 RVA: 0x001A1DE8 File Offset: 0x0019FFE8
		public float GetVerticalOffset(TextRenderer.TextVerticalAlignment verticalAlignment)
		{
			float result;
			switch (verticalAlignment)
			{
			case TextRenderer.TextVerticalAlignment.Top:
				result = this._textHeight;
				break;
			case TextRenderer.TextVerticalAlignment.Middle:
				result = this._textHeight / 2f;
				break;
			case TextRenderer.TextVerticalAlignment.Bottom:
				result = 0f;
				break;
			default:
				throw new Exception("Unreachable");
			}
			return result;
		}

		// Token: 0x060056D0 RID: 22224 RVA: 0x001A1E3C File Offset: 0x001A003C
		protected override void DoDispose()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteVertexArray(this.VertexArray);
			gl.DeleteBuffer(this._verticesBuffer);
			gl.DeleteBuffer(this._indicesBuffer);
		}

		// Token: 0x060056D1 RID: 22225 RVA: 0x001A1E80 File Offset: 0x001A0080
		public void Draw()
		{
			bool flag = this._text == string.Empty;
			if (!flag)
			{
				GLFunctions gl = this._graphics.GL;
				this._graphics.GPUProgramStore.TextProgram.AssertInUse();
				gl.AssertTextureBound(GL.TEXTURE0, this._font.TextureAtlas.GLTexture);
				gl.BindVertexArray(this.VertexArray);
				gl.DrawElements(GL.TRIANGLES, this._indices.Length, GL.UNSIGNED_SHORT, (IntPtr)0);
			}
		}

		// Token: 0x04003442 RID: 13378
		private readonly Font _font;

		// Token: 0x04003443 RID: 13379
		public const uint WhiteColor = 4294967295U;

		// Token: 0x04003444 RID: 13380
		public const uint LightGrayColor = 4290822336U;

		// Token: 0x04003445 RID: 13381
		public const uint MediumGrayColor = 4288716960U;

		// Token: 0x04003446 RID: 13382
		public const uint GrayColor = 4286611584U;

		// Token: 0x04003447 RID: 13383
		public const uint BlackColor = 4278190080U;

		// Token: 0x04003448 RID: 13384
		public uint FillColor;

		// Token: 0x04003449 RID: 13385
		public uint OutlineColor;

		// Token: 0x0400344A RID: 13386
		private readonly GraphicsDevice _graphics;

		// Token: 0x0400344B RID: 13387
		private string _text;

		// Token: 0x0400344C RID: 13388
		private float _textWidth;

		// Token: 0x0400344D RID: 13389
		private float _textHeight;

		// Token: 0x0400344E RID: 13390
		private TextVertex[] _vertices;

		// Token: 0x0400344F RID: 13391
		private ushort[] _indices;

		// Token: 0x04003451 RID: 13393
		private GLBuffer _verticesBuffer;

		// Token: 0x04003452 RID: 13394
		private GLBuffer _indicesBuffer;

		// Token: 0x02000F0C RID: 3852
		public enum TextAlignment
		{
			// Token: 0x040049E3 RID: 18915
			Left,
			// Token: 0x040049E4 RID: 18916
			Center,
			// Token: 0x040049E5 RID: 18917
			Right
		}

		// Token: 0x02000F0D RID: 3853
		public enum TextVerticalAlignment
		{
			// Token: 0x040049E7 RID: 18919
			Top,
			// Token: 0x040049E8 RID: 18920
			Middle,
			// Token: 0x040049E9 RID: 18921
			Bottom
		}
	}
}
