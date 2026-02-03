using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HytaleClient.Core;
using HytaleClient.Graphics.Batcher2D;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Gizmos
{
	// Token: 0x02000A9A RID: 2714
	internal class Graph : Disposable
	{
		// Token: 0x06005574 RID: 21876 RVA: 0x00191FE4 File Offset: 0x001901E4
		public unsafe Graph(GraphicsDevice graphics, Batcher2D batcher2d, Font font, int historyDuration, string title = "")
		{
			this._graphics = graphics;
			this._batcher2D = batcher2d;
			this._font = font;
			this._historyDuration = historyDuration;
			this._indices = new ushort[historyDuration * 6];
			this._vertices = new float[historyDuration * 32];
			GLFunctions gl = this._graphics.GL;
			this._vertexArray = gl.GenVertexArray();
			gl.BindVertexArray(this._vertexArray);
			this._verticesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			this._indicesBuffer = gl.GenBuffer();
			gl.BindBuffer(this._vertexArray, GL.ELEMENT_ARRAY_BUFFER, this._indicesBuffer);
			for (int i = 0; i < historyDuration; i++)
			{
				this._vertices[i * 32] = 1f + (float)i;
				this._vertices[i * 32 + 1] = 0f;
				this._vertices[i * 32 + 2] = 0f;
				this._vertices[i * 32 + 3] = 0f;
				this._vertices[i * 32 + 4] = 0f;
				this._vertices[i * 32 + 5] = 0f;
				this._vertices[i * 32 + 6] = 0f;
				this._vertices[i * 32 + 7] = 0f;
				this._vertices[i * 32 + 8] = 0f + (float)i;
				this._vertices[i * 32 + 9] = 0f;
				this._vertices[i * 32 + 10] = 0f;
				this._vertices[i * 32 + 11] = 0f;
				this._vertices[i * 32 + 12] = 0f;
				this._vertices[i * 32 + 13] = 0f;
				this._vertices[i * 32 + 14] = 0f;
				this._vertices[i * 32 + 15] = 0f;
				this._vertices[i * 32 + 16] = 0f + (float)i;
				this._vertices[i * 32 + 17] = 1f;
				this._vertices[i * 32 + 18] = 0f;
				this._vertices[i * 32 + 19] = 0f;
				this._vertices[i * 32 + 20] = 0f;
				this._vertices[i * 32 + 21] = 0f;
				this._vertices[i * 32 + 22] = 0f;
				this._vertices[i * 32 + 23] = 0f;
				this._vertices[i * 32 + 24] = 1f + (float)i;
				this._vertices[i * 32 + 25] = 1f;
				this._vertices[i * 32 + 26] = 0f;
				this._vertices[i * 32 + 27] = 0f;
				this._vertices[i * 32 + 28] = 0f;
				this._vertices[i * 32 + 29] = 0f;
				this._vertices[i * 32 + 30] = 0f;
				this._vertices[i * 32 + 31] = 0f;
			}
			for (int j = 0; j < historyDuration; j++)
			{
				this._indices[j * 6] = (ushort)(j * 4);
				this._indices[j * 6 + 1] = (ushort)(j * 4 + 1);
				this._indices[j * 6 + 2] = (ushort)(j * 4 + 2);
				this._indices[j * 6 + 3] = (ushort)(j * 4);
				this._indices[j * 6 + 4] = (ushort)(j * 4 + 2);
				this._indices[j * 6 + 5] = (ushort)(j * 4 + 3);
			}
			float[] array;
			float* value;
			if ((array = this._vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._vertices.Length * 4), (IntPtr)((void*)value), GL.DYNAMIC_DRAW);
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
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			gl.EnableVertexAttribArray(basicProgram.AttribPosition.Index);
			gl.VertexAttribPointer(basicProgram.AttribPosition.Index, 3, GL.FLOAT, false, 32, IntPtr.Zero);
			gl.EnableVertexAttribArray(basicProgram.AttribTexCoords.Index);
			gl.VertexAttribPointer(basicProgram.AttribTexCoords.Index, 2, GL.FLOAT, false, 32, (IntPtr)12);
			bool flag = !string.IsNullOrEmpty(title);
			if (flag)
			{
				this._title = title;
			}
		}

		// Token: 0x06005575 RID: 21877 RVA: 0x00192534 File Offset: 0x00190734
		protected override void DoDispose()
		{
			GLFunctions gl = this._graphics.GL;
			gl.DeleteBuffer(this._verticesBuffer);
			gl.DeleteBuffer(this._indicesBuffer);
			gl.DeleteVertexArray(this._vertexArray);
		}

		// Token: 0x06005576 RID: 21878 RVA: 0x00192578 File Offset: 0x00190778
		public void AddAxis(string name, float value)
		{
			Graph.Axis item = new Graph.Axis
			{
				Value = value,
				Label = name,
				Margin = this._font.CalculateTextWidth(name) * this._textHeight / (float)this._font.BaseSize
			};
			this._axes.Add(item);
		}

		// Token: 0x06005577 RID: 21879 RVA: 0x001925D4 File Offset: 0x001907D4
		public void UpdateTextHeight(float textHeight)
		{
			this._textHeight = textHeight;
			for (int i = 0; i < this._axes.Count; i++)
			{
				this._axes[i] = new Graph.Axis
				{
					Value = this._axes[i].Value,
					Label = this._axes[i].Label,
					Margin = this._font.CalculateTextWidth(this._axes[i].Label) * this._textHeight / (float)this._font.BaseSize
				};
			}
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x00192688 File Offset: 0x00190888
		public void UpdateHistory(Graph.DataSet data, float scale = 1f)
		{
			bool flag = scale != 1f;
			if (flag)
			{
				this._axisScale = scale;
				for (int i = 0; i < data.GetValuesCount(); i++)
				{
					int num = (i + data.FirstValueIndex) % data.MaxDataCount;
					this._vertices[i * 32 + 17] = data.History[num] / this._axisScale;
					this._vertices[i * 32 + 25] = data.History[num] / this._axisScale;
				}
			}
			else
			{
				this._axisScale = 1f;
				for (int j = 0; j < data.GetValuesCount(); j++)
				{
					int num2 = (j + data.FirstValueIndex) % data.MaxDataCount;
					this._vertices[j * 32 + 17] = data.History[num2];
					this._vertices[j * 32 + 25] = data.History[num2];
				}
			}
		}

		// Token: 0x06005579 RID: 21881 RVA: 0x00192788 File Offset: 0x00190988
		public void UpdateAxisData(ref Matrix projectionMatrix)
		{
			Matrix.CreateScale(ref this.Scale, out this._dataDrawMatrix);
			Matrix matrix;
			Matrix.CreateTranslation(ref this.Position, out matrix);
			Matrix.Multiply(ref this._dataDrawMatrix, ref matrix, out this._dataDrawMatrix);
			Matrix.Multiply(ref this._dataDrawMatrix, ref projectionMatrix, out this._dataDrawMatrix);
		}

		// Token: 0x0600557A RID: 21882 RVA: 0x001927E0 File Offset: 0x001909E0
		public void PrepareForDrawAxisAndLabels()
		{
			bool flag = this._title != null;
			if (flag)
			{
				this._batcher2D.RequestDrawText(this._font, this._textHeight, this._title, new Vector3(this.LabelPosition.X, this.LabelPosition.Y, 0f), UInt32Color.White, false, false, 0f);
			}
			for (int i = 0; i < this._axes.Count; i++)
			{
				float num = this.LabelPosition.Y - this._axes[i].Value / this._axisScale * this.Scale.Y;
				this._batcher2D.RequestDrawText(this._font, this._textHeight, this._axes[i].Label, new Vector3(this.LabelPosition.X - 10f - this._axes[i].Margin, num, 0f), UInt32Color.White, false, false, 0f);
				this._batcher2D.RequestDrawTexture(this._graphics.WhitePixelTexture, new Rectangle(0, 0, 1, 1), new Vector3((float)((int)this.LabelPosition.X), (float)((int)num), 0f), (float)((int)((float)this._historyDuration * this.Scale.X)), 1f, UInt32Color.White);
			}
		}

		// Token: 0x0600557B RID: 21883 RVA: 0x00192954 File Offset: 0x00190B54
		public unsafe void DrawData()
		{
			BasicProgram basicProgram = this._graphics.GPUProgramStore.BasicProgram;
			basicProgram.AssertInUse();
			basicProgram.Opacity.AssertValue(0.8f);
			this._graphics.GL.AssertTextureBound(GL.TEXTURE0, this._graphics.WhitePixelTexture.GLTexture);
			basicProgram.Color.SetValue(this.Color);
			basicProgram.MVPMatrix.SetValue(ref this._dataDrawMatrix);
			GLFunctions gl = this._graphics.GL;
			gl.BindVertexArray(this._vertexArray);
			gl.BindBuffer(this._vertexArray, GL.ARRAY_BUFFER, this._verticesBuffer);
			float[] array;
			float* value;
			if ((array = this._vertices) == null || array.Length == 0)
			{
				value = null;
			}
			else
			{
				value = &array[0];
			}
			gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(this._vertices.Length * 4), (IntPtr)((void*)value), GL.DYNAMIC_DRAW);
			array = null;
			gl.DrawElements(GL.TRIANGLES, this._indices.Length, GL.UNSIGNED_SHORT, (IntPtr)0);
		}

		// Token: 0x0400325A RID: 12890
		private const float LabelMargin = 10f;

		// Token: 0x0400325B RID: 12891
		private const float TitleMargin = 6f;

		// Token: 0x0400325C RID: 12892
		private const int VerticesByPoint = 32;

		// Token: 0x0400325D RID: 12893
		private readonly GraphicsDevice _graphics;

		// Token: 0x0400325E RID: 12894
		private readonly Font _font;

		// Token: 0x0400325F RID: 12895
		public Vector3 Position = Vector3.Zero;

		// Token: 0x04003260 RID: 12896
		public Vector3 LabelPosition = Vector3.Zero;

		// Token: 0x04003261 RID: 12897
		public Vector3 Scale = Vector3.One;

		// Token: 0x04003262 RID: 12898
		public Vector3 Color = Vector3.Zero;

		// Token: 0x04003263 RID: 12899
		private float _textHeight = 1f;

		// Token: 0x04003264 RID: 12900
		private int _historyDuration;

		// Token: 0x04003265 RID: 12901
		private readonly ushort[] _indices;

		// Token: 0x04003266 RID: 12902
		private readonly float[] _vertices;

		// Token: 0x04003267 RID: 12903
		private readonly List<Graph.Axis> _axes = new List<Graph.Axis>();

		// Token: 0x04003268 RID: 12904
		public string AxisUnit;

		// Token: 0x04003269 RID: 12905
		private float _axisScale = 1f;

		// Token: 0x0400326A RID: 12906
		private Matrix _dataDrawMatrix;

		// Token: 0x0400326B RID: 12907
		private readonly GLBuffer _indicesBuffer;

		// Token: 0x0400326C RID: 12908
		private readonly GLVertexArray _vertexArray;

		// Token: 0x0400326D RID: 12909
		private readonly GLBuffer _verticesBuffer;

		// Token: 0x0400326E RID: 12910
		private readonly string _title;

		// Token: 0x0400326F RID: 12911
		private Batcher2D _batcher2D;

		// Token: 0x02000EE8 RID: 3816
		public struct DataSet
		{
			// Token: 0x17001475 RID: 5237
			// (get) Token: 0x0600680A RID: 26634 RVA: 0x0021A17C File Offset: 0x0021837C
			// (set) Token: 0x0600680B RID: 26635 RVA: 0x0021A184 File Offset: 0x00218384
			public int FirstValueIndex { get; private set; }

			// Token: 0x17001476 RID: 5238
			// (get) Token: 0x0600680C RID: 26636 RVA: 0x0021A18D File Offset: 0x0021838D
			// (set) Token: 0x0600680D RID: 26637 RVA: 0x0021A195 File Offset: 0x00218395
			public int NextValueIndex { get; private set; }

			// Token: 0x17001477 RID: 5239
			// (get) Token: 0x0600680E RID: 26638 RVA: 0x0021A19E File Offset: 0x0021839E
			// (set) Token: 0x0600680F RID: 26639 RVA: 0x0021A1A6 File Offset: 0x002183A6
			public int MaxDataCount { get; private set; }

			// Token: 0x17001478 RID: 5240
			// (get) Token: 0x06006810 RID: 26640 RVA: 0x0021A1AF File Offset: 0x002183AF
			// (set) Token: 0x06006811 RID: 26641 RVA: 0x0021A1B7 File Offset: 0x002183B7
			public float[] History { get; private set; }

			// Token: 0x17001479 RID: 5241
			// (get) Token: 0x06006812 RID: 26642 RVA: 0x0021A1C0 File Offset: 0x002183C0
			// (set) Token: 0x06006813 RID: 26643 RVA: 0x0021A1C8 File Offset: 0x002183C8
			public float AverageValue { get; private set; }

			// Token: 0x1700147A RID: 5242
			// (get) Token: 0x06006814 RID: 26644 RVA: 0x0021A1D1 File Offset: 0x002183D1
			// (set) Token: 0x06006815 RID: 26645 RVA: 0x0021A1D9 File Offset: 0x002183D9
			public float MaxValue { get; private set; }

			// Token: 0x06006816 RID: 26646 RVA: 0x0021A1E2 File Offset: 0x002183E2
			public DataSet(int maxData)
			{
				this.FirstValueIndex = 0;
				this.NextValueIndex = 0;
				this.MaxDataCount = maxData;
				this.History = new float[maxData];
				this.AverageValue = 0f;
				this.MaxValue = 0f;
			}

			// Token: 0x06006817 RID: 26647 RVA: 0x0021A224 File Offset: 0x00218424
			public int GetValuesCount()
			{
				return (this.FirstValueIndex < this.NextValueIndex) ? this.NextValueIndex : this.MaxDataCount;
			}

			// Token: 0x06006818 RID: 26648 RVA: 0x0021A254 File Offset: 0x00218454
			public void RecordValue(float value)
			{
				float num = this.History[this.NextValueIndex];
				this.History[this.NextValueIndex] = value;
				this.AverageValue = this.AverageValue - this.AverageValue / (float)this.MaxDataCount + value / (float)this.MaxDataCount;
				bool flag = num != 0f;
				if (flag)
				{
					this.AverageValue = (this.AverageValue - num / (float)this.MaxDataCount) / (1f - 1f / (float)this.MaxDataCount);
				}
				this.MaxValue = ((this.MaxValue < value) ? value : this.MaxValue);
				this.NextValueIndex = (this.NextValueIndex + 1) % this.MaxDataCount;
				bool flag2 = this.NextValueIndex == this.FirstValueIndex;
				if (flag2)
				{
					this.FirstValueIndex = (this.FirstValueIndex + 1) % this.MaxDataCount;
				}
			}
		}

		// Token: 0x02000EE9 RID: 3817
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct Axis
		{
			// Token: 0x04004928 RID: 18728
			public float Value;

			// Token: 0x04004929 RID: 18729
			public string Label;

			// Token: 0x0400492A RID: 18730
			public float Margin;
		}
	}
}
