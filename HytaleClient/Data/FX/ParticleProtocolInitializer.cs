using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.Data.FX
{
	// Token: 0x02000AFF RID: 2815
	internal class ParticleProtocolInitializer
	{
		// Token: 0x0600585A RID: 22618 RVA: 0x001AD35C File Offset: 0x001AB55C
		public static void Initialize(BlockParticleSet networkBlockParticleSet, ref ClientBlockParticleSet clientBlockParticleSet)
		{
			bool flag = networkBlockParticleSet.Color_ != null;
			if (flag)
			{
				clientBlockParticleSet.Color = UInt32Color.FromRGBA((byte)networkBlockParticleSet.Color_.Red, (byte)networkBlockParticleSet.Color_.Green, (byte)networkBlockParticleSet.Color_.Blue, byte.MaxValue);
			}
			else
			{
				clientBlockParticleSet.Color = ParticleSettings.DefaultColor;
			}
			clientBlockParticleSet.Scale = networkBlockParticleSet.Scale;
			bool flag2 = networkBlockParticleSet.PositionOffset != null;
			if (flag2)
			{
				clientBlockParticleSet.PositionOffset.X = networkBlockParticleSet.PositionOffset.X;
				clientBlockParticleSet.PositionOffset.Y = networkBlockParticleSet.PositionOffset.Y;
				clientBlockParticleSet.PositionOffset.Z = networkBlockParticleSet.PositionOffset.Z;
			}
			bool flag3 = networkBlockParticleSet.RotationOffset != null;
			if (flag3)
			{
				clientBlockParticleSet.RotationOffset = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(networkBlockParticleSet.RotationOffset.Yaw), MathHelper.ToRadians(networkBlockParticleSet.RotationOffset.Pitch), MathHelper.ToRadians(networkBlockParticleSet.RotationOffset.Roll));
			}
			else
			{
				clientBlockParticleSet.RotationOffset = Quaternion.Identity;
			}
			bool flag4 = networkBlockParticleSet.ParticleSystemIds != null;
			if (flag4)
			{
				clientBlockParticleSet.ParticleSystemIds = new Dictionary<ClientBlockParticleEvent, string>();
				foreach (KeyValuePair<BlockParticleEvent, string> keyValuePair in networkBlockParticleSet.ParticleSystemIds)
				{
					clientBlockParticleSet.ParticleSystemIds[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x0600585B RID: 22619 RVA: 0x001AD4F8 File Offset: 0x001AB6F8
		public static void Initialize(ModelParticle networkParticle, ref ModelParticleSettings clientModelParticle, NodeNameManager nodeNameManager)
		{
			clientModelParticle.SystemId = networkParticle.SystemId;
			clientModelParticle.DetachedFromModel = networkParticle.DetachedFromModel;
			bool flag = networkParticle.Color_ != null;
			if (flag)
			{
				clientModelParticle.Color = UInt32Color.FromRGBA((byte)networkParticle.Color_.Red, (byte)networkParticle.Color_.Green, (byte)networkParticle.Color_.Blue, byte.MaxValue);
			}
			clientModelParticle.Scale = networkParticle.Scale;
			clientModelParticle.TargetEntityPart = networkParticle.TargetEntityPart;
			bool flag2 = networkParticle.TargetNodeName != null;
			if (flag2)
			{
				clientModelParticle.TargetNodeNameId = nodeNameManager.GetOrAddNameId(networkParticle.TargetNodeName);
			}
			bool flag3 = networkParticle.PositionOffset != null;
			if (flag3)
			{
				clientModelParticle.PositionOffset.X = networkParticle.PositionOffset.X;
				clientModelParticle.PositionOffset.Y = networkParticle.PositionOffset.Y;
				clientModelParticle.PositionOffset.Z = networkParticle.PositionOffset.Z;
			}
			bool flag4 = networkParticle.RotationOffset != null;
			if (flag4)
			{
				clientModelParticle.RotationOffset = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(networkParticle.RotationOffset.Yaw), MathHelper.ToRadians(networkParticle.RotationOffset.Pitch), MathHelper.ToRadians(networkParticle.RotationOffset.Roll));
			}
		}

		// Token: 0x0600585C RID: 22620 RVA: 0x001AD640 File Offset: 0x001AB840
		public static void Initialize(ModelParticle[] networkParticles, out ModelParticleSettings[] clientModelParticles, NodeNameManager nodeNameManager)
		{
			bool flag = networkParticles == null || networkParticles.Length == 0;
			if (flag)
			{
				clientModelParticles = null;
			}
			else
			{
				clientModelParticles = new ModelParticleSettings[networkParticles.Length];
				for (int i = 0; i < networkParticles.Length; i++)
				{
					ModelParticleSettings modelParticleSettings = new ModelParticleSettings("");
					ParticleProtocolInitializer.Initialize(networkParticles[i], ref modelParticleSettings, nodeNameManager);
					clientModelParticles[i] = modelParticleSettings;
				}
			}
		}

		// Token: 0x0600585D RID: 22621 RVA: 0x001AD6A0 File Offset: 0x001AB8A0
		public static void Initialize(Particle particle, ParticleRotationInfluence rotationInfluence, ParticleRotationInfluence collisionRotationInfluence, ref ParticleSettings clientParticle)
		{
			clientParticle.TexturePath = particle.TexturePath;
			bool flag = particle.FrameSize != null;
			if (flag)
			{
				clientParticle.FrameSize.X = (ushort)particle.FrameSize.Width;
				clientParticle.FrameSize.Y = (ushort)particle.FrameSize.Height;
			}
			clientParticle.UVOption = particle.UvOption;
			clientParticle.ScaleRatio = particle.ScaleRatioConstraint;
			clientParticle.SoftParticlesOption = particle.SoftParticles;
			clientParticle.SoftParticlesFadeFactor = particle.SoftParticlesFadeFactor;
			clientParticle.UseSpriteBlending = particle.UseSpriteBlending;
			bool flag2 = rotationInfluence == 1 || rotationInfluence == 2 || rotationInfluence == 3;
			bool isBillboard = collisionRotationInfluence == 1 || collisionRotationInfluence == 2 || collisionRotationInfluence == 3;
			Vector2 vector = Vector2.Zero;
			Vector2 vector2 = Vector2.Zero;
			Vector2 vector3 = Vector2.Zero;
			bool flag3 = false;
			Vector2 vector4 = ParticleSettings.DefaultScale;
			Vector2 vector5 = ParticleSettings.DefaultScale;
			bool flag4 = false;
			float num = 1f;
			bool flag5 = false;
			Color color = null;
			bool flag6 = false;
			ByteVector2 byteVector = new ByteVector2(0, 0);
			bool flag7 = false;
			bool flag8 = particle.InitialAnimationFrame != null;
			if (flag8)
			{
				bool flag9 = particle.InitialAnimationFrame.Rotation != null;
				if (flag9)
				{
					bool flag10 = particle.InitialAnimationFrame.Rotation.X != null && !flag2;
					if (flag10)
					{
						vector = ParticleProtocolInitializer.CreateRotationFrame(particle.InitialAnimationFrame.Rotation.X);
					}
					bool flag11 = particle.InitialAnimationFrame.Rotation.Y != null && !flag2;
					if (flag11)
					{
						vector2 = ParticleProtocolInitializer.CreateRotationFrame(particle.InitialAnimationFrame.Rotation.Y);
					}
					bool flag12 = particle.InitialAnimationFrame.Rotation.Z != null;
					if (flag12)
					{
						vector3 = ParticleProtocolInitializer.CreateRotationFrame(particle.InitialAnimationFrame.Rotation.Z);
					}
				}
				RangeVector2f scale = particle.InitialAnimationFrame.Scale;
				bool flag13 = ((scale != null) ? scale.X : null) != null;
				if (flag13)
				{
					vector4 = ConversionHelper.RangeToVector2(particle.InitialAnimationFrame.Scale.X);
				}
				RangeVector2f scale2 = particle.InitialAnimationFrame.Scale;
				bool flag14 = ((scale2 != null) ? scale2.Y : null) != null;
				if (flag14)
				{
					vector5 = ConversionHelper.RangeToVector2(particle.InitialAnimationFrame.Scale.Y);
				}
				color = particle.InitialAnimationFrame.Color_;
				bool flag15 = particle.InitialAnimationFrame.Opacity != -1f;
				if (flag15)
				{
					num = particle.InitialAnimationFrame.Opacity;
				}
				bool flag16 = particle.InitialAnimationFrame.FrameIndex != null;
				if (flag16)
				{
					byteVector = ConversionHelper.RangeToByteVector2(particle.InitialAnimationFrame.FrameIndex);
				}
			}
			bool flag17 = particle.AnimationFrames == null;
			if (!flag17)
			{
				List<ParticleSettings.ScaleKeyframe> list = new List<ParticleSettings.ScaleKeyframe>(10);
				List<ParticleSettings.RotationKeyframe> list2 = new List<ParticleSettings.RotationKeyframe>(10);
				List<ParticleSettings.RangeKeyframe> list3 = new List<ParticleSettings.RangeKeyframe>(10);
				List<ParticleSettings.ColorKeyframe> list4 = new List<ParticleSettings.ColorKeyframe>(10);
				List<ParticleSettings.OpacityKeyframe> list5 = new List<ParticleSettings.OpacityKeyframe>(10);
				int[] array = Enumerable.ToArray<int>(particle.AnimationFrames.Keys);
				ParticleAnimationFrame[] array2 = Enumerable.ToArray<ParticleAnimationFrame>(particle.AnimationFrames.Values);
				Array.Sort<int, ParticleAnimationFrame>(array, array2, 0, array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					int num2 = array[i];
					ParticleAnimationFrame particleAnimationFrame = array2[i];
					bool flag18 = num2 < 0 || num2 > 100;
					if (!flag18)
					{
						bool flag19 = particleAnimationFrame.FrameIndex != null;
						if (flag19)
						{
							flag7 = (flag7 || num2 == 0);
							ByteVector2 byteVector2 = ConversionHelper.RangeToByteVector2(particleAnimationFrame.FrameIndex);
							list3.Add(new ParticleSettings.RangeKeyframe
							{
								Time = (byte)num2,
								Min = byteVector2.X,
								Max = byteVector2.Y
							});
						}
						bool flag20 = particleAnimationFrame.Scale != null;
						if (flag20)
						{
							flag4 = (flag4 || num2 == 0);
							ParticleSettings.ScaleKeyframe item = ParticleProtocolInitializer.CreateScaleKeyframe((byte)num2, particleAnimationFrame.Scale);
							item.Min.X = item.Min.X * vector4.X;
							item.Min.Y = item.Min.Y * vector5.X;
							item.Max.X = item.Max.X * vector4.Y;
							item.Max.Y = item.Max.Y * vector5.Y;
							ParticleProtocolInitializer.Sort(ref item.Min.X, ref item.Max.X);
							ParticleProtocolInitializer.Sort(ref item.Min.Y, ref item.Max.Y);
							list.Add(item);
						}
						bool flag21 = particleAnimationFrame.Rotation != null;
						if (flag21)
						{
							flag3 = (flag3 || num2 == 0);
							ParticleSettings.RotationKeyframe item2 = ParticleProtocolInitializer.CreateRotationKeyframe((byte)num2, particleAnimationFrame.Rotation, flag2);
							item2.Min.X = item2.Min.X + vector.X;
							item2.Min.Y = item2.Min.Y + vector2.X;
							item2.Min.Z = item2.Min.Z + vector3.X;
							item2.Max.X = item2.Max.X + vector.Y;
							item2.Max.Y = item2.Max.Y + vector2.Y;
							item2.Max.Z = item2.Max.Z + vector3.Y;
							ParticleProtocolInitializer.Sort(ref item2.Min.X, ref item2.Max.X);
							ParticleProtocolInitializer.Sort(ref item2.Min.Y, ref item2.Max.Y);
							ParticleProtocolInitializer.Sort(ref item2.Min.Z, ref item2.Max.Z);
							list2.Add(item2);
						}
						bool flag22 = particleAnimationFrame.Color_ != null;
						if (flag22)
						{
							flag6 = (flag6 || num2 == 0);
							ParticleSettings.ColorKeyframe item3 = ParticleProtocolInitializer.CreateColorKeyframe((byte)num2, particleAnimationFrame.Color_);
							list4.Add(item3);
						}
						bool flag23 = particleAnimationFrame.Opacity != -1f;
						if (flag23)
						{
							flag5 = (flag5 || num2 == 0);
							list5.Add(new ParticleSettings.OpacityKeyframe
							{
								Time = (byte)num2,
								Opacity = MathHelper.Clamp(particleAnimationFrame.Opacity * num, 0f, 1f)
							});
						}
					}
				}
				bool flag24 = particle.CollisionAnimationFrame != null;
				if (flag24)
				{
					bool flag25 = particle.CollisionAnimationFrame.Scale != null;
					if (flag25)
					{
						ParticleSettings.ScaleKeyframe item4 = ParticleProtocolInitializer.CreateScaleKeyframe(101, particle.CollisionAnimationFrame.Scale);
						ParticleProtocolInitializer.Sort(ref item4.Min.X, ref item4.Max.X);
						ParticleProtocolInitializer.Sort(ref item4.Min.Y, ref item4.Max.Y);
						list.Add(item4);
					}
					bool flag26 = particle.CollisionAnimationFrame.Rotation != null;
					if (flag26)
					{
						ParticleSettings.RotationKeyframe item5 = ParticleProtocolInitializer.CreateRotationKeyframe(101, particle.CollisionAnimationFrame.Rotation, isBillboard);
						ParticleProtocolInitializer.Sort(ref item5.Min.X, ref item5.Max.X);
						ParticleProtocolInitializer.Sort(ref item5.Min.Y, ref item5.Max.Y);
						ParticleProtocolInitializer.Sort(ref item5.Min.Z, ref item5.Max.Z);
						list2.Add(item5);
					}
					bool flag27 = particle.CollisionAnimationFrame.Opacity != -1f;
					if (flag27)
					{
						list5.Add(new ParticleSettings.OpacityKeyframe
						{
							Time = 101,
							Opacity = MathHelper.Clamp(particle.CollisionAnimationFrame.Opacity, 0f, 1f)
						});
					}
					bool flag28 = particle.CollisionAnimationFrame.Color_ != null;
					if (flag28)
					{
						ParticleSettings.ColorKeyframe item6 = ParticleProtocolInitializer.CreateColorKeyframe(101, particle.CollisionAnimationFrame.Color_);
						list4.Add(item6);
					}
					bool flag29 = particle.CollisionAnimationFrame.FrameIndex != null;
					if (flag29)
					{
						ByteVector2 byteVector3 = ConversionHelper.RangeToByteVector2(particle.CollisionAnimationFrame.FrameIndex);
						list3.Add(new ParticleSettings.RangeKeyframe
						{
							Time = 101,
							Min = byteVector3.X,
							Max = byteVector3.Y
						});
					}
				}
				bool flag30 = list.Count == 0 || !flag4;
				if (flag30)
				{
					ParticleAnimationFrame initialAnimationFrame = particle.InitialAnimationFrame;
					bool flag31 = ((initialAnimationFrame != null) ? initialAnimationFrame.Scale : null) != null;
					ParticleSettings.ScaleKeyframe item7;
					if (flag31)
					{
						item7 = ParticleProtocolInitializer.CreateScaleKeyframe(0, particle.InitialAnimationFrame.Scale);
						ParticleProtocolInitializer.Sort(ref item7.Min.X, ref item7.Max.X);
						ParticleProtocolInitializer.Sort(ref item7.Min.Y, ref item7.Max.Y);
					}
					else
					{
						item7 = new ParticleSettings.ScaleKeyframe
						{
							Time = 0,
							Min = ParticleSettings.DefaultScale * 0.03125f,
							Max = ParticleSettings.DefaultScale * 0.03125f
						};
					}
					list.Insert(0, item7);
				}
				clientParticle.ScaleKeyframes = list.ToArray();
				for (int j = clientParticle.ScaleKeyframes.Length - 1; j >= 0; j--)
				{
					ref ParticleSettings.ScaleKeyframe ptr = ref clientParticle.ScaleKeyframes[j];
					bool flag32 = ptr.Time <= 100;
					if (flag32)
					{
						ParticleSettings particleSettings = clientParticle;
						particleSettings.ScaleKeyFrameCount += 1;
						bool flag33 = ptr.Time > 0 && j != 0;
						if (flag33)
						{
							ref ParticleSettings.ScaleKeyframe ptr2 = ref clientParticle.ScaleKeyframes[j - 1];
							ptr.Min.X = ptr.Min.X - ptr2.Min.X;
							ptr.Min.Y = ptr.Min.Y - ptr2.Min.Y;
							ptr.Max.X = ptr.Max.X - ptr2.Max.X;
							ptr.Max.Y = ptr.Max.Y - ptr2.Max.Y;
						}
					}
				}
				ParticleAnimationFrame initialAnimationFrame2 = particle.InitialAnimationFrame;
				bool flag34 = ((initialAnimationFrame2 != null) ? initialAnimationFrame2.Rotation : null) != null && !flag3;
				if (flag34)
				{
					ParticleSettings.RotationKeyframe item8 = ParticleProtocolInitializer.CreateRotationKeyframe(0, particle.InitialAnimationFrame.Rotation, flag2);
					list2.Insert(0, item8);
				}
				clientParticle.RotationKeyframes = list2.ToArray();
				for (int k = clientParticle.RotationKeyframes.Length - 1; k >= 0; k--)
				{
					ref ParticleSettings.RotationKeyframe ptr3 = ref clientParticle.RotationKeyframes[k];
					bool flag35 = ptr3.Time <= 100;
					if (flag35)
					{
						ParticleSettings particleSettings2 = clientParticle;
						particleSettings2.RotationKeyFrameCount += 1;
						bool flag36 = ptr3.Time > 0 && k != 0;
						if (flag36)
						{
							ref ParticleSettings.RotationKeyframe ptr4 = ref clientParticle.RotationKeyframes[k - 1];
							ptr3.Min.X = ptr3.Min.X - ptr4.Min.X;
							ptr3.Min.Y = ptr3.Min.Y - ptr4.Min.Y;
							ptr3.Min.Z = ptr3.Min.Z - ptr4.Min.Z;
							ptr3.Max.X = ptr3.Max.X - ptr4.Max.X;
							ptr3.Max.Y = ptr3.Max.Y - ptr4.Max.Y;
							ptr3.Max.Z = ptr3.Max.Z - ptr4.Max.Z;
						}
					}
				}
				bool flag37 = !flag7;
				if (flag37)
				{
					list3.Insert(0, new ParticleSettings.RangeKeyframe
					{
						Time = 0,
						Min = byteVector.X,
						Max = byteVector.Y
					});
				}
				clientParticle.TextureIndexKeyFrames = list3.ToArray();
				for (int l = 0; l < clientParticle.TextureIndexKeyFrames.Length; l++)
				{
					bool flag38 = clientParticle.TextureIndexKeyFrames[l].Time <= 100;
					if (flag38)
					{
						ParticleSettings particleSettings3 = clientParticle;
						particleSettings3.TextureKeyFrameCount += 1;
					}
				}
				bool flag39 = color != null && !flag6;
				if (flag39)
				{
					ParticleSettings.ColorKeyframe item9 = ParticleProtocolInitializer.CreateColorKeyframe(0, color);
					list4.Insert(0, item9);
				}
				clientParticle.ColorKeyframes = list4.ToArray();
				for (int m = 0; m < clientParticle.ColorKeyframes.Length; m++)
				{
					bool flag40 = clientParticle.ColorKeyframes[m].Time <= 100;
					if (flag40)
					{
						ParticleSettings particleSettings4 = clientParticle;
						particleSettings4.ColorKeyFrameCount += 1;
					}
				}
				bool flag41 = num != 1f && !flag5;
				if (flag41)
				{
					list5.Insert(0, new ParticleSettings.OpacityKeyframe
					{
						Time = 0,
						Opacity = num
					});
				}
				clientParticle.OpacityKeyframes = list5.ToArray();
				for (int n = 0; n < clientParticle.OpacityKeyframes.Length; n++)
				{
					bool flag42 = clientParticle.OpacityKeyframes[n].Time <= 100;
					if (flag42)
					{
						ParticleSettings particleSettings5 = clientParticle;
						particleSettings5.OpacityKeyFrameCount += 1;
					}
				}
			}
		}

		// Token: 0x0600585E RID: 22622 RVA: 0x001AE47C File Offset: 0x001AC67C
		public static void Initialize(ParticleSpawner particleSpawner, ref ParticleSpawnerSettings clientParticleSpawner)
		{
			switch (particleSpawner.RenderMode)
			{
			case 0:
				clientParticleSpawner.RenderMode = FXSystem.RenderMode.BlendLinear;
				break;
			case 1:
				clientParticleSpawner.RenderMode = FXSystem.RenderMode.BlendAdd;
				break;
			case 2:
				clientParticleSpawner.RenderMode = FXSystem.RenderMode.Erosion;
				break;
			case 3:
				clientParticleSpawner.RenderMode = FXSystem.RenderMode.Distortion;
				break;
			}
			clientParticleSpawner.RotationInfluence = particleSpawner.ParticleRotationInfluence_;
			clientParticleSpawner.ParticleRotateWithSpawner = particleSpawner.ParticleRotateWithSpawner;
			clientParticleSpawner.TrailSpawnerPositionMultiplier = particleSpawner.TrailSpawnerPositionMultiplier;
			clientParticleSpawner.TrailSpawnerRotationMultiplier = particleSpawner.TrailSpawnerRotationMultiplier;
			bool flag = particleSpawner.ParticleCollision_ != null;
			if (flag)
			{
				clientParticleSpawner.ParticleCollisionBlockType = particleSpawner.ParticleCollision_.BlockType;
				clientParticleSpawner.ParticleCollisionAction = particleSpawner.ParticleCollision_.Action;
				clientParticleSpawner.ParticleCollisionRotationInfluence = particleSpawner.ParticleCollision_.ParticleRotationInfluence_;
			}
			clientParticleSpawner.LinearFiltering = particleSpawner.LinearFiltering;
			clientParticleSpawner.IsLowRes = particleSpawner.IsLowRes;
			UVMotion uvMotion_ = particleSpawner.UvMotion_;
			bool flag2 = uvMotion_ != null;
			if (flag2)
			{
				clientParticleSpawner.UVMotion.TexturePath = uvMotion_.Texture;
				clientParticleSpawner.UVMotion.AddRandomUVOffset = uvMotion_.AddRandomUVOffset;
				clientParticleSpawner.UVMotion.Speed = new Vector2(uvMotion_.SpeedX, uvMotion_.SpeedY);
				clientParticleSpawner.UVMotion.Strength = uvMotion_.Strength;
				clientParticleSpawner.UVMotion.StrengthCurveType = uvMotion_.StrengthCurveType;
				clientParticleSpawner.UVMotion.Scale = uvMotion_.Scale;
			}
			bool flag3 = particleSpawner.IntersectionHighlight_ != null && particleSpawner.IntersectionHighlight_.HighlightColor != null;
			if (flag3)
			{
				clientParticleSpawner.IntersectionHighlight.Color = new Vector3((float)((byte)particleSpawner.IntersectionHighlight_.HighlightColor.Red) / 255f, (float)((byte)particleSpawner.IntersectionHighlight_.HighlightColor.Green) / 255f, (float)((byte)particleSpawner.IntersectionHighlight_.HighlightColor.Blue) / 255f);
				clientParticleSpawner.IntersectionHighlight.Threshold = particleSpawner.IntersectionHighlight_.HighlightThreshold;
			}
			clientParticleSpawner.CameraOffset = particleSpawner.CameraOffset;
			clientParticleSpawner.VelocityStretchMultiplier = particleSpawner.VelocityStretchMultiplier;
			clientParticleSpawner.LightInfluence = particleSpawner.LightInfluence;
			clientParticleSpawner.LifeSpan = particleSpawner.LifeSpan;
			bool flag4 = particleSpawner.TotalParticles != null;
			if (flag4)
			{
				clientParticleSpawner.TotalParticles = ConversionHelper.RangeToPoint(particleSpawner.TotalParticles);
			}
			bool flag5 = particleSpawner.MaxConcurrentParticles > 0 && particleSpawner.MaxConcurrentParticles < 512;
			if (flag5)
			{
				clientParticleSpawner.MaxConcurrentParticles = particleSpawner.MaxConcurrentParticles;
			}
			bool flag6 = particleSpawner.ParticleLifeSpan != null;
			if (flag6)
			{
				clientParticleSpawner.ParticleLifeSpan = ConversionHelper.RangeToVector2(particleSpawner.ParticleLifeSpan);
			}
			bool flag7 = particleSpawner.SpawnRate != null;
			if (flag7)
			{
				clientParticleSpawner.SpawnRate = ConversionHelper.RangeToVector2(particleSpawner.SpawnRate);
			}
			clientParticleSpawner.SpawnBurst = particleSpawner.SpawnBurst;
			bool flag8 = !clientParticleSpawner.SpawnBurst;
			if (flag8)
			{
				clientParticleSpawner.MaxConcurrentParticles = (int)Math.Min(Math.Ceiling((double)(clientParticleSpawner.SpawnRate.Y * clientParticleSpawner.ParticleLifeSpan.Y)), (double)clientParticleSpawner.MaxConcurrentParticles);
			}
			bool flag9 = particleSpawner.WaveDelay != null;
			if (flag9)
			{
				clientParticleSpawner.WaveDelay = ConversionHelper.RangeToVector2(particleSpawner.WaveDelay);
			}
			InitialVelocity initialVelocity_ = particleSpawner.InitialVelocity_;
			bool flag10 = initialVelocity_ != null;
			if (flag10)
			{
				Vector2 vector = (initialVelocity_.Yaw == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(initialVelocity_.Yaw);
				Vector2 vector2 = (initialVelocity_.Pitch == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(initialVelocity_.Pitch);
				Vector2 vector3 = (initialVelocity_.Speed == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(initialVelocity_.Speed);
				clientParticleSpawner.InitialVelocityMin = new ParticleSpawnerSettings.InitialVelocity(MathHelper.ToRadians(vector.X), MathHelper.ToRadians(vector2.X), vector3.X);
				clientParticleSpawner.InitialVelocityMax = new ParticleSpawnerSettings.InitialVelocity(MathHelper.ToRadians(vector.Y), MathHelper.ToRadians(vector2.Y), vector3.Y);
				ParticleSpawnerSettings particleSpawnerSettings = clientParticleSpawner;
				particleSpawnerSettings.InitialVelocityMin.Speed = particleSpawnerSettings.InitialVelocityMin.Speed * 0.016666668f;
				ParticleSpawnerSettings particleSpawnerSettings2 = clientParticleSpawner;
				particleSpawnerSettings2.InitialVelocityMax.Speed = particleSpawnerSettings2.InitialVelocityMax.Speed * 0.016666668f;
			}
			clientParticleSpawner.EmitShape = particleSpawner.Shape;
			RangeVector3f emitOffset = particleSpawner.EmitOffset;
			bool flag11 = emitOffset != null;
			if (flag11)
			{
				Vector2 vector4 = (emitOffset.X == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(emitOffset.X);
				Vector2 vector5 = (emitOffset.Y == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(emitOffset.Y);
				Vector2 vector6 = (emitOffset.Z == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(emitOffset.Z);
				clientParticleSpawner.EmitOffsetMin = new Vector3(vector4.X, vector5.X, vector6.X);
				clientParticleSpawner.EmitOffsetMax = new Vector3(vector4.Y, vector5.Y, vector6.Y);
			}
			clientParticleSpawner.UseEmitDirection = particleSpawner.UseEmitDirection;
			bool flag12 = particleSpawner.Attractors != null;
			if (flag12)
			{
				clientParticleSpawner.Attractors = new ParticleAttractor[particleSpawner.Attractors.Length];
				for (int i = 0; i < particleSpawner.Attractors.Length; i++)
				{
					ParticleAttractor particleAttractor = particleSpawner.Attractors[i];
					ParticleAttractor particleAttractor2 = new ParticleAttractor
					{
						Position = ((particleAttractor.Position != null) ? new Vector3(particleAttractor.Position.X, particleAttractor.Position.Y, particleAttractor.Position.Z) : Vector3.Zero),
						RadialAxis = ((particleAttractor.RadialAxis != null) ? new Vector3(particleAttractor.RadialAxis.X, particleAttractor.RadialAxis.Y, particleAttractor.RadialAxis.Z) : Vector3.Zero),
						TrailPositionMultiplier = particleAttractor.TrailPositionMultiplier,
						RadialAcceleration = particleAttractor.RadialAcceleration,
						RadialTangentAcceleration = particleAttractor.RadialTangentAcceleration,
						LinearAcceleration = ((particleAttractor.LinearAcceleration != null) ? new Vector3(particleAttractor.LinearAcceleration.X, particleAttractor.LinearAcceleration.Y, particleAttractor.LinearAcceleration.Z) : Vector3.Zero),
						RadialImpulse = particleAttractor.RadialImpulse,
						RadialTangentImpulse = particleAttractor.RadialTangentImpulse,
						LinearImpulse = ((particleAttractor.LinearImpulse != null) ? new Vector3(particleAttractor.LinearImpulse.X, particleAttractor.LinearImpulse.Y, particleAttractor.LinearImpulse.Z) : Vector3.Zero),
						Radius = particleAttractor.Radius,
						DampingMultiplier = ((particleAttractor.DampingMultiplier != null) ? new Vector3(particleAttractor.DampingMultiplier.X, particleAttractor.DampingMultiplier.Y, particleAttractor.DampingMultiplier.Z) : Vector3.One)
					};
					bool flag13 = particleAttractor2.RadialAxis != Vector3.Zero;
					if (flag13)
					{
						particleAttractor2.RadialAxis = Vector3.Normalize(particleAttractor2.RadialAxis);
					}
					particleAttractor2.LinearImpulse *= 0.016666668f;
					particleAttractor2.RadialImpulse *= 0.016666668f;
					particleAttractor2.RadialTangentImpulse *= 0.016666668f;
					particleAttractor2.DampingMultiplier.X = (float)Math.Pow((double)particleAttractor2.DampingMultiplier.X, 0.1666666716337204);
					particleAttractor2.DampingMultiplier.Y = (float)Math.Pow((double)particleAttractor2.DampingMultiplier.Y, 0.1666666716337204);
					particleAttractor2.DampingMultiplier.Z = (float)Math.Pow((double)particleAttractor2.DampingMultiplier.Z, 0.1666666716337204);
					particleAttractor2.LinearAcceleration *= 0.0002777778f;
					particleAttractor2.RadialAcceleration *= 0.0002777778f;
					particleAttractor2.RadialTangentAcceleration *= 0.0002777778f;
					clientParticleSpawner.Attractors[i] = particleAttractor2;
				}
			}
			bool flag14 = clientParticleSpawner.EmitShape == ParticleSpawnerSettings.Shape.Sphere;
			if (flag14)
			{
				bool flag15 = (clientParticleSpawner.EmitOffsetMin.X == 0f && clientParticleSpawner.EmitOffsetMax.X == 0f) || (clientParticleSpawner.EmitOffsetMin.Y == 0f && clientParticleSpawner.EmitOffsetMax.Y == 0f) || (clientParticleSpawner.EmitOffsetMin.Z == 0f && clientParticleSpawner.EmitOffsetMax.Z == 0f);
				if (flag15)
				{
					clientParticleSpawner.EmitShape = ParticleSpawnerSettings.Shape.Circle;
				}
			}
			bool flag16 = clientParticleSpawner.EmitShape == ParticleSpawnerSettings.Shape.Cube;
			if (flag16)
			{
				bool flag17 = clientParticleSpawner.EmitOffsetMin.X == 0f && clientParticleSpawner.EmitOffsetMin.Y == 0f && clientParticleSpawner.EmitOffsetMin.Z == 0f;
				if (flag17)
				{
					clientParticleSpawner.EmitShape = ParticleSpawnerSettings.Shape.FullCube;
				}
			}
		}

		// Token: 0x0600585F RID: 22623 RVA: 0x001AED90 File Offset: 0x001ACF90
		public static void Initialize(ParticleSystem particleSystem, ref ParticleSystemSettings clientParticleSystem)
		{
			bool flag = particleSystem.LifeSpan > 0f;
			if (flag)
			{
				clientParticleSystem.LifeSpan = particleSystem.LifeSpan;
			}
			clientParticleSystem.CullDistanceSquared = ((particleSystem.CullDistance >= 1f) ? (particleSystem.CullDistance * particleSystem.CullDistance) : 1600f);
			clientParticleSystem.BoundingRadius = ((particleSystem.BoundingRadius >= 1f) ? particleSystem.BoundingRadius : 10f);
			clientParticleSystem.IsImportant = particleSystem.IsImportant;
			ParticleSpawnerGroup[] spawners = particleSystem.Spawners;
			int num = (spawners != null) ? spawners.Length : 0;
			clientParticleSystem.CreateSpawnerSettingsStorage((byte)num);
			for (int i = 0; i < num; i++)
			{
				ParticleSpawnerGroup particleSpawnerGroup = particleSystem.Spawners[i];
				ParticleSystemSettings.SystemSpawnerSettings systemSpawnerSettings = new ParticleSystemSettings.SystemSpawnerSettings();
				systemSpawnerSettings.ParticleSpawnerId = particleSpawnerGroup.SpawnerId;
				bool flag2 = particleSpawnerGroup.PositionOffset != null;
				if (flag2)
				{
					systemSpawnerSettings.PositionOffset = new Vector3(particleSpawnerGroup.PositionOffset.X, particleSpawnerGroup.PositionOffset.Y, particleSpawnerGroup.PositionOffset.Z);
				}
				bool flag3 = particleSpawnerGroup.RotationOffset != null;
				if (flag3)
				{
					systemSpawnerSettings.RotationOffset = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(particleSpawnerGroup.RotationOffset.Yaw), MathHelper.ToRadians(particleSpawnerGroup.RotationOffset.Pitch), MathHelper.ToRadians(particleSpawnerGroup.RotationOffset.Roll));
				}
				systemSpawnerSettings.FixedRotation = particleSpawnerGroup.FixedRotation;
				systemSpawnerSettings.StartDelay = particleSpawnerGroup.StartDelay;
				bool flag4 = particleSpawnerGroup.SpawnRate != null;
				if (flag4)
				{
					systemSpawnerSettings.SpawnRate = ConversionHelper.RangeToVector2(particleSpawnerGroup.SpawnRate);
				}
				Vector2 vector = (systemSpawnerSettings.SpawnRate == ParticleSystemSettings.DefaultSpawnRate) ? ParticleSystemSettings.DefaultSingleSpawnerLifeSpan : ParticleSystemSettings.DefaultSpawnerLifeSpan;
				systemSpawnerSettings.LifeSpan = ((particleSpawnerGroup.LifeSpan != null) ? ConversionHelper.RangeToVector2(particleSpawnerGroup.LifeSpan) : vector);
				bool flag5 = particleSpawnerGroup.TotalSpawners != 0;
				if (flag5)
				{
					systemSpawnerSettings.TotalSpawners = particleSpawnerGroup.TotalSpawners;
				}
				bool flag6 = particleSpawnerGroup.MaxConcurrent > 0 && systemSpawnerSettings.SpawnRate != ParticleSystemSettings.DefaultSpawnRate;
				if (flag6)
				{
					systemSpawnerSettings.MaxConcurrent = (int)MathHelper.Min((float)particleSpawnerGroup.MaxConcurrent, 10f);
				}
				bool flag7 = particleSpawnerGroup.WaveDelay != null;
				if (flag7)
				{
					systemSpawnerSettings.WaveDelay = ConversionHelper.RangeToVector2(particleSpawnerGroup.WaveDelay);
				}
				InitialVelocity initialVelocity_ = particleSpawnerGroup.InitialVelocity_;
				bool flag8 = initialVelocity_ != null;
				if (flag8)
				{
					Vector2 vector2 = (initialVelocity_.Yaw == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(initialVelocity_.Yaw);
					Vector2 vector3 = (initialVelocity_.Pitch == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(initialVelocity_.Pitch);
					Vector2 vector4 = (initialVelocity_.Speed == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(initialVelocity_.Speed);
					systemSpawnerSettings.InitialVelocityMin = new ParticleSpawnerSettings.InitialVelocity(MathHelper.ToRadians(vector2.X), MathHelper.ToRadians(vector3.X), vector4.X);
					systemSpawnerSettings.InitialVelocityMax = new ParticleSpawnerSettings.InitialVelocity(MathHelper.ToRadians(vector2.Y), MathHelper.ToRadians(vector3.Y), vector4.Y);
					ParticleSystemSettings.SystemSpawnerSettings systemSpawnerSettings2 = systemSpawnerSettings;
					systemSpawnerSettings2.InitialVelocityMin.Speed = systemSpawnerSettings2.InitialVelocityMin.Speed * 0.016666668f;
					ParticleSystemSettings.SystemSpawnerSettings systemSpawnerSettings3 = systemSpawnerSettings;
					systemSpawnerSettings3.InitialVelocityMax.Speed = systemSpawnerSettings3.InitialVelocityMax.Speed * 0.016666668f;
				}
				RangeVector3f emitOffset = particleSpawnerGroup.EmitOffset;
				bool flag9 = emitOffset != null;
				if (flag9)
				{
					Vector2 vector5 = (emitOffset.X == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(emitOffset.X);
					Vector2 vector6 = (emitOffset.Y == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(emitOffset.Y);
					Vector2 vector7 = (emitOffset.Z == null) ? Vector2.Zero : ConversionHelper.RangeToVector2(emitOffset.Z);
					systemSpawnerSettings.EmitOffsetMin = new Vector3(vector5.X, vector6.X, vector7.X);
					systemSpawnerSettings.EmitOffsetMax = new Vector3(vector5.Y, vector6.Y, vector7.Y);
				}
				bool flag10 = particleSpawnerGroup.Attractors != null;
				if (flag10)
				{
					systemSpawnerSettings.Attractors = new ParticleAttractor[particleSpawnerGroup.Attractors.Length];
					for (int j = 0; j < particleSpawnerGroup.Attractors.Length; j++)
					{
						ParticleAttractor particleAttractor = particleSpawnerGroup.Attractors[j];
						ParticleAttractor particleAttractor2 = new ParticleAttractor
						{
							Position = ((particleAttractor.Position != null) ? new Vector3(particleAttractor.Position.X, particleAttractor.Position.Y, particleAttractor.Position.Z) : Vector3.Zero),
							RadialAxis = ((particleAttractor.RadialAxis != null) ? new Vector3(particleAttractor.RadialAxis.X, particleAttractor.RadialAxis.Y, particleAttractor.RadialAxis.Z) : Vector3.Zero),
							TrailPositionMultiplier = particleAttractor.TrailPositionMultiplier,
							RadialAcceleration = particleAttractor.RadialAcceleration,
							RadialTangentAcceleration = particleAttractor.RadialTangentAcceleration,
							LinearAcceleration = ((particleAttractor.LinearAcceleration != null) ? new Vector3(particleAttractor.LinearAcceleration.X, particleAttractor.LinearAcceleration.Y, particleAttractor.LinearAcceleration.Z) : Vector3.Zero),
							RadialImpulse = particleAttractor.RadialImpulse,
							RadialTangentImpulse = particleAttractor.RadialTangentImpulse,
							LinearImpulse = ((particleAttractor.LinearImpulse != null) ? new Vector3(particleAttractor.LinearImpulse.X, particleAttractor.LinearImpulse.Y, particleAttractor.LinearImpulse.Z) : Vector3.Zero),
							Radius = particleAttractor.Radius,
							DampingMultiplier = ((particleAttractor.DampingMultiplier != null) ? new Vector3(particleAttractor.DampingMultiplier.X, particleAttractor.DampingMultiplier.Y, particleAttractor.DampingMultiplier.Z) : Vector3.One)
						};
						bool flag11 = particleAttractor2.RadialAxis != Vector3.Zero;
						if (flag11)
						{
							particleAttractor2.RadialAxis = Vector3.Normalize(particleAttractor2.RadialAxis);
						}
						particleAttractor2.LinearImpulse *= 0.016666668f;
						particleAttractor2.RadialImpulse *= 0.016666668f;
						particleAttractor2.RadialTangentImpulse *= 0.016666668f;
						particleAttractor2.DampingMultiplier.X = (float)Math.Pow((double)particleAttractor2.DampingMultiplier.X, 0.1666666716337204);
						particleAttractor2.DampingMultiplier.Y = (float)Math.Pow((double)particleAttractor2.DampingMultiplier.Y, 0.1666666716337204);
						particleAttractor2.DampingMultiplier.Z = (float)Math.Pow((double)particleAttractor2.DampingMultiplier.Z, 0.1666666716337204);
						particleAttractor2.LinearAcceleration *= 0.0002777778f;
						particleAttractor2.RadialAcceleration *= 0.0002777778f;
						particleAttractor2.RadialTangentAcceleration *= 0.0002777778f;
						systemSpawnerSettings.Attractors[j] = particleAttractor2;
					}
				}
				clientParticleSystem.SystemSpawnerSettingsList[i] = systemSpawnerSettings;
			}
		}

		// Token: 0x06005860 RID: 22624 RVA: 0x001AF4B4 File Offset: 0x001AD6B4
		private static ParticleSettings.ScaleKeyframe CreateScaleKeyframe(byte time, RangeVector2f scale)
		{
			Vector2 vector = (scale.X == null) ? ParticleSettings.DefaultScale : new Vector2(scale.X.Min, scale.X.Max);
			Vector2 vector2 = (scale.Y == null) ? ParticleSettings.DefaultScale : new Vector2(scale.Y.Min, scale.Y.Max);
			return new ParticleSettings.ScaleKeyframe
			{
				Time = time,
				Min = new Vector2(vector.X, vector2.X) * 0.03125f,
				Max = new Vector2(vector.Y, vector2.Y) * 0.03125f
			};
		}

		// Token: 0x06005861 RID: 22625 RVA: 0x001AF574 File Offset: 0x001AD774
		private static Vector2 CreateRotationFrame(Rangef rotationRange)
		{
			Vector2 vector = ConversionHelper.RangeToVector2(rotationRange);
			vector.X = MathHelper.ToRadians(vector.X);
			vector.Y = MathHelper.ToRadians(vector.Y);
			return vector;
		}

		// Token: 0x06005862 RID: 22626 RVA: 0x001AF5B4 File Offset: 0x001AD7B4
		private static ParticleSettings.RotationKeyframe CreateRotationKeyframe(byte time, RangeVector3f rotation, bool isBillboard)
		{
			Vector2 vector = (rotation.X == null || isBillboard) ? Vector2.Zero : new Vector2(MathHelper.ToRadians(rotation.X.Min), MathHelper.ToRadians(rotation.X.Max));
			Vector2 vector2 = (rotation.Y == null || isBillboard) ? Vector2.Zero : new Vector2(MathHelper.ToRadians(rotation.Y.Min), MathHelper.ToRadians(rotation.Y.Max));
			Vector2 vector3 = (rotation.Z == null) ? Vector2.Zero : new Vector2(MathHelper.ToRadians(rotation.Z.Min), MathHelper.ToRadians(rotation.Z.Max));
			return new ParticleSettings.RotationKeyframe
			{
				Time = time,
				Min = new Vector3(vector.X, vector2.X, vector3.X),
				Max = new Vector3(vector.Y, vector2.Y, vector3.Y)
			};
		}

		// Token: 0x06005863 RID: 22627 RVA: 0x001AF6C0 File Offset: 0x001AD8C0
		private static ParticleSettings.ColorKeyframe CreateColorKeyframe(byte time, Color networkColor)
		{
			UInt32Color defaultColor = ParticleSettings.DefaultColor;
			defaultColor.SetR((byte)MathHelper.Clamp((int)((byte)networkColor.Red), 0, 255));
			defaultColor.SetG((byte)MathHelper.Clamp((int)((byte)networkColor.Green), 0, 255));
			defaultColor.SetB((byte)MathHelper.Clamp((int)((byte)networkColor.Blue), 0, 255));
			return new ParticleSettings.ColorKeyframe
			{
				Time = time,
				Color = defaultColor
			};
		}

		// Token: 0x06005864 RID: 22628 RVA: 0x001AF744 File Offset: 0x001AD944
		private static void Sort(ref float min, ref float max)
		{
			bool flag = min > max;
			if (flag)
			{
				float num = min;
				min = max;
				max = num;
			}
		}
	}
}
