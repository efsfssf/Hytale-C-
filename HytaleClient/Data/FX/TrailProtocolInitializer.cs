using System;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Trails;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.FX
{
	// Token: 0x02000B00 RID: 2816
	internal class TrailProtocolInitializer
	{
		// Token: 0x06005866 RID: 22630 RVA: 0x001AF774 File Offset: 0x001AD974
		public static void Initialize(Trail networkTrail, ref TrailSettings trailSettings)
		{
			trailSettings.Id = networkTrail.Id;
			trailSettings.Texture = networkTrail.Texture;
			switch (networkTrail.RenderMode)
			{
			case 0:
				trailSettings.RenderMode = FXSystem.RenderMode.BlendLinear;
				break;
			case 1:
				trailSettings.RenderMode = FXSystem.RenderMode.BlendAdd;
				break;
			case 2:
				trailSettings.RenderMode = FXSystem.RenderMode.Erosion;
				break;
			case 3:
				trailSettings.RenderMode = FXSystem.RenderMode.Distortion;
				break;
			}
			bool flag = networkTrail.IntersectionHighlight_ != null && networkTrail.IntersectionHighlight_.HighlightColor != null;
			if (flag)
			{
				trailSettings.IntersectionHighlightColor = new Vector3((float)((byte)networkTrail.IntersectionHighlight_.HighlightColor.Red) / 255f, (float)((byte)networkTrail.IntersectionHighlight_.HighlightColor.Green) / 255f, (float)((byte)networkTrail.IntersectionHighlight_.HighlightColor.Blue) / 255f);
				trailSettings.IntersectionHighlightThreshold = networkTrail.IntersectionHighlight_.HighlightThreshold;
			}
			trailSettings.LifeSpan = networkTrail.LifeSpan;
			trailSettings.Roll = networkTrail.Roll;
			bool flag2 = networkTrail.Start != null;
			if (flag2)
			{
				bool flag3 = networkTrail.Start.Color != null;
				if (flag3)
				{
					trailSettings.Start.Color = new Vector4((float)((byte)networkTrail.Start.Color.Red), (float)((byte)networkTrail.Start.Color.Green), (float)((byte)networkTrail.Start.Color.Blue), (float)((byte)networkTrail.Start.Color.Alpha)) / 255f;
				}
				else
				{
					trailSettings.Start.Color = new Vector4(1f);
				}
				trailSettings.Start.Width = networkTrail.Start.Width;
			}
			else
			{
				trailSettings.Start.Width = 1f;
			}
			bool flag4 = networkTrail.End != null;
			if (flag4)
			{
				bool flag5 = networkTrail.End.Color != null;
				if (flag5)
				{
					trailSettings.End.Color = new Vector4((float)((byte)networkTrail.End.Color.Red), (float)((byte)networkTrail.End.Color.Green), (float)((byte)networkTrail.End.Color.Blue), (float)((byte)networkTrail.End.Color.Alpha)) / 255f;
				}
				else
				{
					trailSettings.End.Color = new Vector4(1f);
				}
				trailSettings.End.Width = networkTrail.End.Width;
			}
			else
			{
				trailSettings.End.Width = 1f;
			}
			trailSettings.LightInfluence = networkTrail.LightInfluence;
			trailSettings.Smooth = networkTrail.Smooth;
			bool flag6 = networkTrail.FrameSize != null;
			if (flag6)
			{
				trailSettings.FrameSize = new Point(networkTrail.FrameSize.X, networkTrail.FrameSize.Y);
			}
			bool flag7 = networkTrail.FrameRange != null;
			if (flag7)
			{
				trailSettings.FrameRange = new Point(networkTrail.FrameRange.Min, networkTrail.FrameRange.Max);
			}
			trailSettings.FrameLifeSpan = networkTrail.FrameLifeSpan;
		}
	}
}
