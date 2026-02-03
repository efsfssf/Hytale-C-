using System;
using System.Collections.Generic;
using Coherent.UI.Binding;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.Graphics.Particles;
using HytaleClient.Graphics.Programs;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Machinima.Actors;
using HytaleClient.InGame.Modules.Machinima.Events;
using HytaleClient.InGame.Modules.Machinima.Settings;
using HytaleClient.InGame.Modules.Machinima.TrackPath;
using HytaleClient.Math;
using Newtonsoft.Json;

namespace HytaleClient.InGame.Modules.Machinima.Track
{
	// Token: 0x02000911 RID: 2321
	[CoherentType]
	internal class SceneTrack : Disposable
	{
		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x060046AA RID: 18090 RVA: 0x001092FD File Offset: 0x001074FD
		// (set) Token: 0x060046AB RID: 18091 RVA: 0x00109305 File Offset: 0x00107505
		[JsonProperty(PropertyName = "Keyframes")]
		[CoherentProperty("keyframes")]
		public List<TrackKeyframe> Keyframes { get; private set; } = new List<TrackKeyframe>();

		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x060046AC RID: 18092 RVA: 0x0010930E File Offset: 0x0010750E
		// (set) Token: 0x060046AD RID: 18093 RVA: 0x00109316 File Offset: 0x00107516
		[JsonIgnore]
		public Vector3[] DrawPoints { get; private set; }

		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x060046AE RID: 18094 RVA: 0x0010931F File Offset: 0x0010751F
		// (set) Token: 0x060046AF RID: 18095 RVA: 0x00109327 File Offset: 0x00107527
		[JsonIgnore]
		public float[] DrawFrames { get; private set; }

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x060046B0 RID: 18096 RVA: 0x00109330 File Offset: 0x00107530
		// (set) Token: 0x060046B1 RID: 18097 RVA: 0x00109338 File Offset: 0x00107538
		[JsonIgnore]
		public float[] SegmentLengths { get; private set; }

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x00109341 File Offset: 0x00107541
		// (set) Token: 0x060046B3 RID: 18099 RVA: 0x00109349 File Offset: 0x00107549
		[JsonIgnore]
		public LinePath Path { get; private set; } = new SplinePath();

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x060046B4 RID: 18100 RVA: 0x00109352 File Offset: 0x00107552
		// (set) Token: 0x060046B5 RID: 18101 RVA: 0x0010935A File Offset: 0x0010755A
		[JsonProperty(PropertyName = "PathType")]
		public SceneTrack.TrackPathType PathType { get; private set; } = SceneTrack.TrackPathType.Spline;

		// Token: 0x060046B6 RID: 18102 RVA: 0x00109363 File Offset: 0x00107563
		public SceneTrack()
		{
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x0010939C File Offset: 0x0010759C
		public SceneTrack(GameInstance gameInstance, SceneActor parent)
		{
			this.Initialize(gameInstance, parent);
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x001093EC File Offset: 0x001075EC
		public void Initialize(GameInstance gameInstance, SceneActor parent)
		{
			bool initialized = this._initialized;
			if (!initialized)
			{
				this.Parent = parent;
				GraphicsDevice graphics = gameInstance.Engine.Graphics;
				BasicProgram basicProgram = graphics.GPUProgramStore.BasicProgram;
				this._pathLineRenderer = new LineRenderer(graphics, basicProgram);
				this._keyframeAngleRenderer = new LineRenderer(graphics, basicProgram);
				this._keyframeBoxRenderer = new BoxRenderer(graphics, basicProgram);
				this._gameInstance = gameInstance;
				bool flag = this.PathType == SceneTrack.TrackPathType.Bezier;
				if (flag)
				{
					this.Path = new BezierPath();
				}
				this._keyframeAngleRenderer.UpdateLineData(new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0.5f, 0f, 0f)
				});
				this.UpdatePositions();
				this._initialized = true;
			}
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x001094C8 File Offset: 0x001076C8
		public void Draw(ref Matrix viewProjectionMatrix, bool drawPath = true, bool drawNodes = true, bool drawRotationAngle = true)
		{
			bool flag = !this._initialized || (this.Parent is CameraActor && (this.Parent as CameraActor).Active) || (!drawPath && !drawNodes && !drawRotationAngle);
			if (!flag)
			{
				MachinimaModule machinimaModule = this._gameInstance.MachinimaModule;
				GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
				bool flag2 = this.Parent == this._gameInstance.MachinimaModule.ActiveActor;
				float num = 0.3f;
				bool flag3 = flag2;
				if (flag3)
				{
					num = 0.7f;
				}
				bool flag4 = drawPath && this._positions.Count > 1;
				if (flag4)
				{
					this._pathLineRenderer.Draw(ref viewProjectionMatrix, graphics.WhiteColor, num);
				}
				bool flag5 = (drawNodes || drawRotationAngle) && machinimaModule.ShowPathNodes;
				if (flag5)
				{
					for (int i = 0; i < this.Keyframes.Count; i++)
					{
						Vector3 item = this._positions[i].Item1;
						TrackKeyframe trackKeyframe = this.Keyframes[i];
						bool flag6 = machinimaModule.SelectedKeyframe == trackKeyframe;
						Vector3 vector;
						if (flag6)
						{
							vector = graphics.CyanColor;
							this.UpdatePositions();
							float currentFrame = machinimaModule.CurrentFrame;
							bool flag7 = !(machinimaModule.SelectedActor is PlayerActor);
							if (flag7)
							{
								this.Update(currentFrame, currentFrame);
							}
						}
						else
						{
							bool flag8 = machinimaModule.HoveredKeyframe == trackKeyframe;
							if (flag8)
							{
								vector = graphics.YellowColor;
							}
							else
							{
								bool flag9 = machinimaModule.ActiveKeyframe == trackKeyframe;
								if (flag9)
								{
									vector = graphics.MagentaColor;
								}
								else
								{
									bool flag10 = i == 0;
									if (flag10)
									{
										vector = graphics.GreenColor;
									}
									else
									{
										bool flag11 = i == this.Keyframes.Count - 1;
										if (flag11)
										{
											vector = graphics.RedColor;
										}
										else
										{
											vector = graphics.BlueColor;
										}
									}
								}
							}
						}
						bool flag12 = (machinimaModule.SelectionMode == MachinimaModule.EditorSelectionMode.Actor && machinimaModule.HoveredKeyframe != null && machinimaModule.HoveredActor.Track == this) || (machinimaModule.SelectionMode == MachinimaModule.EditorSelectionMode.Scene && machinimaModule.HoveredKeyframe != null);
						if (flag12)
						{
							vector = graphics.YellowColor;
						}
						else
						{
							bool flag13 = (machinimaModule.SelectionMode == MachinimaModule.EditorSelectionMode.Actor && machinimaModule.ActiveActor == this.Parent) || (machinimaModule.SelectionMode == MachinimaModule.EditorSelectionMode.Scene && machinimaModule.ActiveKeyframe != null);
							if (flag13)
							{
								vector = graphics.MagentaColor;
							}
						}
						if (drawNodes)
						{
							bool flag14 = this._gameInstance.Input.IsAltHeld() && machinimaModule.HoveredKeyframe == trackKeyframe && machinimaModule.HoveredActor is EntityActor && !(machinimaModule.HoveredActor is ItemActor) && machinimaModule.SelectionMode == MachinimaModule.EditorSelectionMode.Keyframe;
							if (flag14)
							{
								Vector3 vector2 = graphics.BlueColor;
								bool bodyRotateHover = machinimaModule.BodyRotateHover;
								if (bodyRotateHover)
								{
									vector = graphics.BlueColor;
									vector2 = graphics.YellowColor;
								}
								this._keyframeBoxRenderer.Draw(item, TrackKeyframe.KeyframeBox, viewProjectionMatrix, vector, num, vector, num / 3f);
								item.Y -= 0.25f;
								this._keyframeBoxRenderer.Draw(item, TrackKeyframe.KeyframeBox, viewProjectionMatrix, vector2, num, vector2, num / 3f);
							}
							else
							{
								this._keyframeBoxRenderer.Draw(item, TrackKeyframe.KeyframeBox, viewProjectionMatrix, vector, num, vector, num / 3f);
							}
						}
						if (drawRotationAngle)
						{
							Vector3 item2 = this._positions[i].Item3;
							Vector3 item3 = this._positions[i].Item2;
							Matrix.CreateFromYawPitchRoll(item2.Y + item3.Y + 1.5707964f, 0f, item2.X, out this._modelMatrix);
							Matrix.CreateTranslation(ref item, out this._tempMatrix);
							Matrix.Multiply(ref this._modelMatrix, ref this._tempMatrix, out this._modelMatrix);
							Matrix.Multiply(ref this._modelMatrix, ref viewProjectionMatrix, out this._modelMatrix);
							this._keyframeAngleRenderer.Draw(ref this._modelMatrix, vector, num);
						}
					}
				}
			}
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x001098EC File Offset: 0x00107AEC
		protected override void DoDispose()
		{
			bool flag = !this._initialized;
			if (!flag)
			{
				EntityActor entityActor = this.Parent as EntityActor;
				bool flag2 = entityActor != null;
				if (flag2)
				{
					entityActor.Despawn(this._gameInstance);
				}
				this._pathLineRenderer.Dispose();
				this._keyframeAngleRenderer.Dispose();
				this._keyframeBoxRenderer.Dispose();
				this.ClearParticles();
			}
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x00109958 File Offset: 0x00107B58
		public void Update(float currentFrame, float lastFrame)
		{
			TrackKeyframe currentFrame2 = this.GetCurrentFrame(currentFrame);
			bool flag = currentFrame2 == null;
			if (!flag)
			{
				this.Parent.LoadKeyframe(currentFrame2);
				for (int i = 0; i < this._particleSystemProxies.Count; i++)
				{
					ParticleSystemProxy particleSystemProxy = this._particleSystemProxies[i];
					bool flag2 = particleSystemProxy == null || particleSystemProxy.IsExpired;
					if (flag2)
					{
						this._particleSystemProxies.RemoveAt(i);
					}
					else
					{
						particleSystemProxy.Position = this.Parent.Position;
						particleSystemProxy.Rotation = Quaternion.CreateFromYawPitchRoll(this.Parent.Rotation.Yaw, this.Parent.Rotation.Pitch, this.Parent.Rotation.Roll);
					}
				}
				bool running = this._gameInstance.MachinimaModule.Running;
				if (running)
				{
					this.TriggerEvents(currentFrame, lastFrame);
				}
			}
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x00109A48 File Offset: 0x00107C48
		public void UpdateKeyframeData()
		{
			this.Keyframes.Sort((TrackKeyframe x, TrackKeyframe y) => x.Frame.CompareTo(y.Frame));
			bool flag = this.Parent != null;
			if (flag)
			{
				this.UpdatePositions();
			}
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x00109A98 File Offset: 0x00107C98
		public void UpdatePositions()
		{
			this._positions = new List<Tuple<Vector3, Vector3, Vector3>>();
			List<Vector3> list = new List<Vector3>();
			float frame = -1f;
			for (int i = 0; i < this.Keyframes.Count; i++)
			{
				int nextKeyframe = this.GetNextKeyframe(frame);
				bool flag = nextKeyframe == -1;
				if (flag)
				{
					break;
				}
				frame = this.Keyframes[nextKeyframe].Frame;
				KeyframeSetting<Vector3> setting = this.Keyframes[nextKeyframe].GetSetting<Vector3>("Position");
				KeyframeSetting<Vector3> setting2 = this.Keyframes[nextKeyframe].GetSetting<Vector3>("Rotation");
				KeyframeSetting<Vector3> setting3 = this.Keyframes[nextKeyframe].GetSetting<Vector3>("Look");
				bool flag2 = setting == null || setting3 == null;
				if (!flag2)
				{
					Vector3 value = setting.Value;
					Vector3 value2 = setting2.Value;
					Vector3 value3 = setting3.Value;
					this._positions.Add(new Tuple<Vector3, Vector3, Vector3>(value, value2, value3));
					list.Add(value);
					bool flag3 = this.PathType == SceneTrack.TrackPathType.Bezier && i < this.Keyframes.Count - 1;
					if (flag3)
					{
						KeyframeSetting<Vector3[]> setting4 = this.Keyframes[nextKeyframe].GetSetting<Vector3[]>("Curve");
						bool flag4 = setting4 != null;
						if (flag4)
						{
							Vector3[] value4 = setting4.Value;
							for (int j = 0; j < value4.Length; j++)
							{
								list.Add(value4[j] + value);
							}
						}
					}
				}
			}
			bool flag5 = this._positions.Count < 2;
			if (!flag5)
			{
				this._pathControlPositions = list.ToArray();
				this.Path.UpdatePoints(this._pathControlPositions);
				this.SegmentLengths = this.Path.GetSegmentLengths();
				this.DrawFrames = this.Path.GetDrawFrames();
				this.DrawPoints = this.Path.GetDrawPoints();
				this._pathLineRenderer.UpdateLineData(this.DrawPoints);
			}
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x00109CA4 File Offset: 0x00107EA4
		public float GetPositionPathFrame(Vector3 position)
		{
			bool flag = this._positions.Count < 2;
			float result;
			if (flag)
			{
				result = -1f;
			}
			else
			{
				for (int i = 0; i < this._positions.Count - 1; i++)
				{
					int num = i;
					int num2 = i + 1;
					int index = (i - 1 >= 0) ? (i - 1) : num;
					int index2 = (i + 2 < this._positions.Count) ? (i + 2) : num2;
					Vector3 item = this._positions[index].Item1;
					Vector3 item2 = this._positions[num].Item1;
					Vector3 item3 = this._positions[num2].Item1;
					Vector3 item4 = this._positions[index2].Item1;
					float num3 = Vector3.Distance(this._positions[num].Item1, this._positions[num2].Item1);
					double num4 = Math.Round((double)(num3 / 0.1f));
					int num5 = 0;
					while ((double)num5 <= num4)
					{
						float num6 = (float)((double)num5 / num4);
						Vector3 value;
						Vector3.Spline(ref num6, ref item, ref item2, ref item3, ref item4, out value);
						bool flag2 = value == position;
						if (flag2)
						{
							float num7 = this.Keyframes[i + 1].Frame - this.Keyframes[i].Frame;
							return this.Keyframes[i].Frame + num7 * num6;
						}
						num5++;
					}
				}
				result = -1f;
			}
			return result;
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x00109E44 File Offset: 0x00108044
		public TrackKeyframe GetKeyframeByFrame(float keyframePosition)
		{
			for (int i = 0; i < this.Keyframes.Count; i++)
			{
				bool flag = this.Keyframes[i].Frame == keyframePosition;
				if (flag)
				{
					return this.Keyframes[i];
				}
			}
			return null;
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x00109E9C File Offset: 0x0010809C
		public TrackKeyframe GetKeyframe(int keyframeId)
		{
			foreach (TrackKeyframe trackKeyframe in this.Keyframes)
			{
				bool flag = trackKeyframe.Id == keyframeId;
				if (flag)
				{
					return trackKeyframe;
				}
			}
			return null;
		}

		// Token: 0x060046C1 RID: 18113 RVA: 0x00109F04 File Offset: 0x00108104
		public void AddKeyframe(TrackKeyframe keyframe, bool update = true)
		{
			bool flag = this.GetKeyframeByFrame(keyframe.Frame) != null;
			if (flag)
			{
				throw new Exception("Unable to add new keyframe, one already exists at that point in time.");
			}
			bool flag2 = this.Path is BezierPath;
			if (flag2)
			{
				KeyframeSetting<Vector3[]> keyframeSetting = keyframe.GetSetting<Vector3[]>("Curve");
				bool flag3 = keyframeSetting == null;
				if (flag3)
				{
					Vector3[] positions = new Vector3[]
					{
						Vector3.One,
						Vector3.One * -1f
					};
					keyframeSetting = new CurveSetting(positions);
					keyframe.AddSetting(keyframeSetting);
				}
			}
			this.Keyframes.Add(keyframe);
			if (update)
			{
				this.UpdateKeyframeData();
			}
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x00109FB0 File Offset: 0x001081B0
		public void RemoveKeyframe(float keyframePosition)
		{
			bool flag = this.Keyframes.Count <= 1;
			if (flag)
			{
				throw new Exception("Unable to remove keyframe, a minimum of one is required.");
			}
			int num = -1;
			for (int i = 0; i < this.Keyframes.Count; i++)
			{
				bool flag2 = this.Keyframes[i].Frame == keyframePosition;
				if (flag2)
				{
					num = i;
					break;
				}
			}
			bool flag3 = num == -1;
			if (!flag3)
			{
				this.Keyframes.RemoveAt(num);
				this.UpdateKeyframeData();
			}
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x0010A03B File Offset: 0x0010823B
		public void ClearKeyframes()
		{
			this.Keyframes.RemoveRange(1, this.Keyframes.Count - 1);
			this.UpdateKeyframeData();
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x0010A060 File Offset: 0x00108260
		public void OffsetPositions(Vector3 offset)
		{
			for (int i = 0; i < this.Keyframes.Count; i++)
			{
				TrackKeyframe trackKeyframe = this.Keyframes[i];
				KeyframeSetting<Vector3> setting = trackKeyframe.GetSetting<Vector3>("Position");
				bool flag = setting == null;
				if (!flag)
				{
					setting.Value += offset;
				}
			}
			this.UpdatePositions();
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x0010A0CC File Offset: 0x001082CC
		private void TriggerEvents(float currentFrame, float lastFrame)
		{
			bool flag = currentFrame == lastFrame;
			if (!flag)
			{
				for (int i = 0; i < this.Keyframes.Count; i++)
				{
					TrackKeyframe trackKeyframe = this.Keyframes[i];
					bool flag2 = trackKeyframe.Frame <= currentFrame && trackKeyframe.Frame >= lastFrame;
					if (flag2)
					{
						trackKeyframe.TriggerEvents(this._gameInstance, this);
					}
				}
			}
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x0010A13C File Offset: 0x0010833C
		public SceneTrack CopyToEntity(Entity entity)
		{
			EntityActor entityActor = new EntityActor(this._gameInstance, entity.Name, entity);
			entityActor.SetBaseModel(entity.ModelPacket);
			SceneTrack sceneTrack = this.Clone();
			sceneTrack.Initialize(this._gameInstance, entityActor);
			entityActor.Track = sceneTrack;
			Vector3 offset = entity.Position - this.GetStartingPosition();
			sceneTrack.OffsetPositions(offset);
			return sceneTrack;
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x0010A1A8 File Offset: 0x001083A8
		public void CopyToActor(ref SceneActor actor)
		{
			SceneTrack sceneTrack = this.Clone();
			actor.Track.Dispose();
			actor.Track = sceneTrack;
			sceneTrack.Initialize(this._gameInstance, actor);
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x0010A1E4 File Offset: 0x001083E4
		private SceneTrack Clone()
		{
			SceneTrack sceneTrack = new SceneTrack();
			sceneTrack.PathType = this.PathType;
			foreach (TrackKeyframe trackKeyframe in this.Keyframes)
			{
				sceneTrack.AddKeyframe(trackKeyframe.Clone(), true);
			}
			return sceneTrack;
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x0010A25C File Offset: 0x0010845C
		public Vector3 GetStartingPosition()
		{
			float frame = -1f;
			for (int i = 0; i < this.Keyframes.Count; i++)
			{
				int nextKeyframe = this.GetNextKeyframe(frame);
				bool flag = nextKeyframe == -1;
				if (flag)
				{
					break;
				}
				frame = this.Keyframes[nextKeyframe].Frame;
				KeyframeSetting<Vector3> setting = this.Keyframes[nextKeyframe].GetSetting<Vector3>("Position");
				bool flag2 = setting == null;
				if (!flag2)
				{
					return setting.Value;
				}
			}
			return Vector3.Zero;
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x0010A2EC File Offset: 0x001084EC
		public TrackKeyframe GetCurrentFrame(float frame)
		{
			bool flag = this.Keyframes.Count == 0;
			TrackKeyframe result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int previousKeyframe = this.GetPreviousKeyframe(frame);
				int nextKeyframe = this.GetNextKeyframe(frame);
				bool flag2 = previousKeyframe == -1 || nextKeyframe == -1 || this.Keyframes.Count == previousKeyframe;
				if (flag2)
				{
					result = null;
				}
				else
				{
					result = this.InterpolateKeyframe(frame, previousKeyframe, nextKeyframe);
				}
			}
			return result;
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x0010A350 File Offset: 0x00108550
		public void InsertKeyframeOffset(float insertFrame, float timeOffset)
		{
			for (int i = 0; i < this.Keyframes.Count; i++)
			{
				bool flag = this.Keyframes[i].Frame >= insertFrame;
				if (flag)
				{
					this.Keyframes[i].Frame += timeOffset;
				}
			}
			this.Keyframes.Sort((TrackKeyframe x, TrackKeyframe y) => x.Frame.CompareTo(y.Frame));
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x0010A3DC File Offset: 0x001085DC
		public int GetPreviousKeyframe(float frame)
		{
			bool flag = this.Keyframes.Count == 0;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = frame <= this.Keyframes[0].Frame;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					int num = -1;
					float num2 = -1f;
					for (int i = 0; i < this.Keyframes.Count; i++)
					{
						bool flag3 = this.Keyframes[i].Frame > frame;
						if (!flag3)
						{
							float num3 = frame - this.Keyframes[i].Frame;
							bool flag4 = num2 == -1f || num3 < num2;
							if (flag4)
							{
								num = i;
								num2 = num3;
							}
						}
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x0010A4A4 File Offset: 0x001086A4
		public int GetNextKeyframe(float frame)
		{
			bool flag = this.Keyframes.Count == 0;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = frame >= this.Keyframes[this.Keyframes.Count - 1].Frame;
				if (flag2)
				{
					result = this.Keyframes.Count - 1;
				}
				else
				{
					int num = -1;
					float num2 = -1f;
					for (int i = 0; i < this.Keyframes.Count; i++)
					{
						bool flag3 = this.Keyframes[i].Frame <= frame;
						if (!flag3)
						{
							float num3 = this.Keyframes[i].Frame - frame;
							bool flag4 = num2 == -1f || num3 < num2;
							if (flag4)
							{
								num = i;
								num2 = num3;
							}
						}
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x0010A58C File Offset: 0x0010878C
		public int GetNextKeyframe(float frame, KeyframeSettingType settingType)
		{
			bool flag = this.Keyframes.Count == 0;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = frame >= this.Keyframes[this.Keyframes.Count - 1].Frame;
				if (flag2)
				{
					result = this.Keyframes.Count - 1;
				}
				else
				{
					int num = -1;
					float num2 = -1f;
					for (int i = 0; i < this.Keyframes.Count; i++)
					{
						bool flag3 = this.Keyframes[i].Frame <= frame;
						if (!flag3)
						{
							bool flag4 = false;
							foreach (KeyValuePair<string, IKeyframeSetting> keyValuePair in this.Keyframes[i].Settings)
							{
								bool flag5 = keyValuePair.Value.Name == settingType.ToString();
								if (flag5)
								{
									flag4 = true;
									break;
								}
							}
							bool flag6 = !flag4;
							if (!flag6)
							{
								float num3 = this.Keyframes[i].Frame - frame;
								bool flag7 = num2 == -1f || num3 < num2;
								if (flag7)
								{
									num = i;
									num2 = num3;
								}
							}
						}
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x0010A708 File Offset: 0x00108908
		public TrackKeyframe GetKeyframeFromEventId(int eventId)
		{
			for (int i = 0; i < this.Keyframes.Count; i++)
			{
				bool flag = this.Keyframes[i].HasEvent(eventId);
				if (flag)
				{
					return this.Keyframes[i];
				}
			}
			return null;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x0010A75C File Offset: 0x0010895C
		private TrackKeyframe InterpolateKeyframe(float frame, int prevFrameIndex, int nextFrameIndex)
		{
			bool flag = prevFrameIndex == nextFrameIndex;
			TrackKeyframe result;
			if (flag)
			{
				TrackKeyframe trackKeyframe = this.Keyframes[prevFrameIndex].Clone();
				trackKeyframe.Frame = frame;
				result = trackKeyframe;
			}
			else
			{
				TrackKeyframe trackKeyframe2 = this.Keyframes[prevFrameIndex];
				TrackKeyframe trackKeyframe3 = this.Keyframes[nextFrameIndex];
				float frame2 = trackKeyframe2.Frame;
				float frame3 = trackKeyframe3.Frame;
				float num = frame3 - frame2;
				float num2 = frame - frame2;
				float num3 = (num <= 0f) ? 0f : (num2 / num);
				int num4 = prevFrameIndex + 1;
				int index = (prevFrameIndex - 1 >= 0) ? (prevFrameIndex - 1) : prevFrameIndex;
				int index2 = (prevFrameIndex + 2 < this._positions.Count) ? (prevFrameIndex + 2) : num4;
				KeyframeSetting<Easing.EasingType> setting = trackKeyframe2.GetSetting<Easing.EasingType>("Easing");
				Easing.EasingType easingType = (setting == null) ? Easing.EasingType.Linear : setting.Value;
				TrackKeyframe trackKeyframe = new TrackKeyframe(frame);
				foreach (KeyValuePair<string, IKeyframeSetting> keyValuePair in trackKeyframe3.Settings)
				{
					string name = keyValuePair.Value.Name;
					Type valueType = keyValuePair.Value.ValueType;
					bool flag2 = name == "Position";
					if (flag2)
					{
						trackKeyframe.AddSetting(new PositionSetting(this.Path.GetPathPosition(prevFrameIndex, num3, true, easingType)));
					}
					else
					{
						bool flag3 = name == "Rotation";
						if (flag3)
						{
							Vector3 item = this._positions[index].Item2;
							Vector3 item2 = this._positions[prevFrameIndex].Item2;
							Vector3 item3 = this._positions[num4].Item2;
							Vector3 item4 = this._positions[index2].Item2;
							float value = MathHelper.Spline(num3, item.X, item2.X, item3.X, item4.X);
							float angle = MathHelper.SplineAngle(num3, item.Y, item2.Y, item3.Y, item4.Y);
							float angle2 = MathHelper.SplineAngle(num3, item.Z, item2.Z, item3.Z, item4.Z);
							Vector3 position = new Vector3(MathHelper.Clamp(value, -1.5707964f, 1.5707964f), MathHelper.WrapAngle(angle), MathHelper.WrapAngle(angle2));
							trackKeyframe.AddSetting(new RotationSetting(position));
						}
						else
						{
							bool flag4 = name == "Look";
							if (flag4)
							{
								Vector3 item5 = this._positions[index].Item3;
								Vector3 item6 = this._positions[prevFrameIndex].Item3;
								Vector3 item7 = this._positions[num4].Item3;
								Vector3 item8 = this._positions[index2].Item3;
								float value2 = MathHelper.Spline(num3, item5.X, item6.X, item7.X, item8.X);
								float angle3 = MathHelper.SplineAngle(num3, item5.Y, item6.Y, item7.Y, item8.Y);
								float angle4 = MathHelper.SplineAngle(num3, item5.Z, item6.Z, item7.Z, item8.Z);
								Vector3 position2 = new Vector3(MathHelper.Clamp(value2, -1.5707964f, 1.5707964f), MathHelper.WrapAngle(angle3), MathHelper.WrapAngle(angle4));
								trackKeyframe.AddSetting(new LookSetting(position2));
							}
							else
							{
								bool flag5 = name == "FieldOfView";
								if (flag5)
								{
									KeyframeSetting<float> setting2 = trackKeyframe2.GetSetting<float>("FieldOfView");
									bool flag6 = setting2 != null;
									if (flag6)
									{
										float value3 = setting2.Value;
										float value4 = trackKeyframe3.GetSetting<float>("FieldOfView").Value;
										float fov = MathHelper.Lerp(value3, value4, num3);
										trackKeyframe.AddSetting(new FieldOfViewSetting(fov));
									}
								}
							}
						}
					}
				}
				result = trackKeyframe;
			}
			return result;
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x0010AB70 File Offset: 0x00108D70
		public float GetTrackLength()
		{
			float num = 0f;
			for (int i = 0; i < this.Keyframes.Count; i++)
			{
				bool flag = this.Keyframes[i].Frame > num;
				if (flag)
				{
					num = this.Keyframes[i].Frame;
				}
			}
			return num;
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x0010ABD4 File Offset: 0x00108DD4
		public void ListKeyframes()
		{
			bool flag = this.Keyframes.Count == 0;
			if (flag)
			{
				this._gameInstance.Chat.Log("No keyframes found for actor '" + this.Parent.Name + "'");
			}
			else
			{
				this._gameInstance.Chat.Log(string.Format("{0} keyframes found for actor '{1}'", this.Keyframes.Count, this.Parent.Name));
				for (int i = 0; i < this.Keyframes.Count; i++)
				{
					TrackKeyframe trackKeyframe = this.Keyframes[i];
					string arg = (trackKeyframe.Events.Count == 0) ? "" : string.Format(" - Events: {0}", trackKeyframe.Events.Count);
					this._gameInstance.Chat.Log(string.Format("#{0} - Frame: {1}{2}", i, trackKeyframe.Frame, arg));
				}
			}
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x0010ACE8 File Offset: 0x00108EE8
		public void ListEvents()
		{
			List<Tuple<float, List<string>>> list = new List<Tuple<float, List<string>>>();
			foreach (TrackKeyframe trackKeyframe in this.Keyframes)
			{
				bool flag = trackKeyframe.Events.Count == 0;
				if (!flag)
				{
					list.Add(new Tuple<float, List<string>>(trackKeyframe.Frame, new List<string>()));
					int index = list.Count - 1;
					foreach (KeyframeEvent keyframeEvent in trackKeyframe.Events)
					{
						list[index].Item2.Add(keyframeEvent.ToString());
					}
				}
			}
			bool flag2 = list.Count == 0;
			if (flag2)
			{
				this._gameInstance.Chat.Log("No keyframe events found for actor '" + this.Parent.Name + "'");
			}
			else
			{
				foreach (Tuple<float, List<string>> tuple in list)
				{
					this._gameInstance.Chat.Log(string.Format("{0} events for keyframe position {1}", tuple.Item2.Count, tuple.Item1));
					foreach (string message in tuple.Item2)
					{
						this._gameInstance.Chat.Log(message);
					}
				}
			}
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x0010AEE4 File Offset: 0x001090E4
		public void SetPathType(SceneTrack.TrackPathType pathType, bool reset = false)
		{
			bool flag = this.PathType == pathType && !reset;
			if (!flag)
			{
				bool flag2 = false;
				bool flag3 = pathType == SceneTrack.TrackPathType.Spline;
				if (flag3)
				{
					this.Path = new SplinePath();
				}
				else
				{
					bool flag4 = pathType == SceneTrack.TrackPathType.Bezier;
					if (flag4)
					{
						this.Path = new BezierPath();
						int i = 0;
						while (i < this.Keyframes.Count)
						{
							KeyframeSetting<Vector3[]> keyframeSetting = this.Keyframes[i].GetSetting<Vector3[]>("Curve");
							bool flag5 = keyframeSetting != null;
							if (!flag5)
							{
								goto IL_99;
							}
							bool flag6 = !flag2;
							if (flag6)
							{
								flag2 = true;
							}
							bool flag7 = !reset;
							if (!flag7)
							{
								goto IL_99;
							}
							IL_16C:
							i++;
							continue;
							IL_99:
							bool flag8 = i == this.Keyframes.Count - 1;
							Vector3[] positions;
							if (flag8)
							{
								positions = new Vector3[]
								{
									Vector3.One,
									Vector3.One * -1f
								};
							}
							else
							{
								Vector3 value = this.Keyframes[i + 1].GetSetting<Vector3>("Position").Value - this.Keyframes[i].GetSetting<Vector3>("Position").Value;
								positions = new Vector3[]
								{
									value * 0.33f,
									value * 0.67f
								};
							}
							keyframeSetting = new CurveSetting(positions);
							this.Keyframes[i].AddSetting(keyframeSetting);
							goto IL_16C;
						}
					}
				}
				this.PathType = pathType;
				bool flag9 = !flag2 && !reset;
				if (flag9)
				{
					this.SmoothBezierPath();
				}
				this.UpdateKeyframeData();
			}
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x0010B0A4 File Offset: 0x001092A4
		public float GetPathSegmentLength(int index, int count = 1)
		{
			float num = 0f;
			int num2 = index;
			while (num2 < index + count && num2 < this.SegmentLengths.Length)
			{
				num += this.SegmentLengths[num2];
				num2++;
			}
			return num;
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x0010B0E8 File Offset: 0x001092E8
		public float GetPathSegmentSpeed(int index, int count = 1)
		{
			float pathSegmentLength = this.GetPathSegmentLength(index, count);
			int index2 = (int)MathHelper.Min((float)(index + count), (float)(this.Keyframes.Count - 1));
			float num = this.Keyframes[index2].Frame - this.Keyframes[index].Frame;
			return pathSegmentLength / num;
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x0010B14C File Offset: 0x0010934C
		public void SetPathSegmentSpeed(float speed, int index, int count = 1)
		{
			int num = index;
			while (num < index + count && num < this.SegmentLengths.Length)
			{
				float num2 = (num == 0) ? 0f : this.Keyframes[num].Frame;
				float pathSegmentLength = this.GetPathSegmentLength(num, 1);
				float num3 = MathHelper.Max(pathSegmentLength / speed, 1f);
				this.Keyframes[num + 1].Frame = (float)Math.Round((double)(num2 + num3));
				num++;
			}
			this.UpdateKeyframeData();
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x0010B1D8 File Offset: 0x001093D8
		public void ScalePathSpeed(float scale)
		{
			scale = 1f / scale;
			List<float> list = new List<float>();
			for (int i = 1; i < this.Keyframes.Count; i++)
			{
				float frame = this.Keyframes[i - 1].Frame;
				float frame2 = this.Keyframes[i].Frame;
				float num = frame2 - frame;
				list.Add(num * scale);
			}
			for (int j = 1; j < this.Keyframes.Count; j++)
			{
				this.Keyframes[j].Frame = this.Keyframes[j - 1].Frame + list[j - 1];
			}
			this.UpdateKeyframeData();
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x0010B2A4 File Offset: 0x001094A4
		public void RotatePath(Vector3 rotation, Vector3 origin)
		{
			bool flag = origin.IsNaN();
			if (flag)
			{
				origin = this.GetStartingPosition();
			}
			Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(rotation.Yaw, rotation.Pitch, rotation.Roll);
			Matrix.CreateFromQuaternion(ref quaternion, out this._tempMatrix);
			for (int i = 0; i < this.Keyframes.Count; i++)
			{
				KeyframeSetting<Vector3[]> setting = this.Keyframes[i].GetSetting<Vector3[]>("Curve");
				bool flag2 = setting != null && setting.Value != null;
				if (flag2)
				{
					Vector3 value = this.Keyframes[i].GetSetting<Vector3>("Position").Value;
					Vector3 value2 = Vector3.Transform(value - origin, this._tempMatrix) + origin;
					for (int j = 0; j < setting.Value.Length; j++)
					{
						Vector3 value3 = Vector3.Transform(setting.Value[j] + value - origin, this._tempMatrix);
						setting.Value[j] = value3 + origin - value2;
					}
				}
				KeyframeSetting<Vector3> setting2 = this.Keyframes[i].GetSetting<Vector3>("Position");
				bool flag3;
				if (setting2 != null)
				{
					Vector3 value4 = setting2.Value;
					flag3 = true;
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					Vector3 value5 = Vector3.Transform(setting2.Value - origin, this._tempMatrix);
					setting2.Value = value5 + origin;
				}
				KeyframeSetting<Vector3> setting3 = this.Keyframes[i].GetSetting<Vector3>("Rotation");
				bool flag5;
				if (setting3 != null)
				{
					Vector3 value6 = setting3.Value;
					flag5 = true;
				}
				else
				{
					flag5 = false;
				}
				bool flag6 = flag5;
				if (flag6)
				{
					Vector3 value7 = setting3.Value;
					value7.Y = MathHelper.WrapAngle(value7.Y + rotation.Y);
					setting3.Value = value7;
				}
			}
			this.UpdateKeyframeData();
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x0010B49C File Offset: 0x0010969C
		public void SmoothBezierPath()
		{
			bool flag = !(this.Path is BezierPath);
			if (!flag)
			{
				bool flag2 = this.Keyframes.Count < 3;
				if (flag2)
				{
					bool flag3 = this.Keyframes.Count == 1;
					if (flag3)
					{
						this.Keyframes[0].AddSetting(new CurveSetting(new Vector3[]
						{
							Vector3.Zero,
							Vector3.Zero
						}));
					}
					else
					{
						bool flag4 = this.Keyframes.Count == 2;
						if (flag4)
						{
							Vector3 value = this.Keyframes[1].GetSetting<Vector3>("Position").Value - this.Keyframes[0].GetSetting<Vector3>("Position").Value;
							this.Keyframes[0].AddSetting(new CurveSetting(new Vector3[]
							{
								value * 0.33f,
								value * 0.67f
							}));
							this.Keyframes[1].AddSetting(new CurveSetting(new Vector3[]
							{
								Vector3.Zero,
								Vector3.Zero
							}));
						}
					}
				}
				else
				{
					Vector3[] array = new Vector3[this.Keyframes.Count];
					for (int i = 0; i < this.Keyframes.Count; i++)
					{
						array[i] = this.Keyframes[i].GetSetting<Vector3>("Position").Value;
					}
					Vector3[] array2;
					Vector3[] array3;
					BezierPath.GetCurveControlPoints(array, out array2, out array3);
					for (int j = 0; j < this.Keyframes.Count - 1; j++)
					{
						KeyframeSetting<Vector3[]> setting = this.Keyframes[j].GetSetting<Vector3[]>("Curve");
						Vector3 value2 = this.Keyframes[j].GetSetting<Vector3>("Position").Value;
						setting.Value[0] = array2[j] - value2;
						setting.Value[1] = array3[j] - value2;
					}
					this.UpdateKeyframeData();
				}
			}
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x0010B6FE File Offset: 0x001098FE
		public void AddParticleSystem(ParticleSystemProxy particleSystem)
		{
			this._particleSystemProxies.Add(particleSystem);
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x0010B710 File Offset: 0x00109910
		public void ClearParticles()
		{
			foreach (ParticleSystemProxy particleSystemProxy in this._particleSystemProxies)
			{
				bool flag = particleSystemProxy != null && !particleSystemProxy.IsExpired;
				if (flag)
				{
					particleSystemProxy.Expire(false);
				}
			}
			this._particleSystemProxies.Clear();
		}

		// Token: 0x04002395 RID: 9109
		private GameInstance _gameInstance;

		// Token: 0x04002396 RID: 9110
		private const float DrawnPathSectionLength = 0.1f;

		// Token: 0x04002397 RID: 9111
		private bool _initialized = false;

		// Token: 0x04002398 RID: 9112
		[JsonIgnore]
		public SceneActor Parent;

		// Token: 0x0400239A RID: 9114
		private LineRenderer _pathLineRenderer;

		// Token: 0x0400239B RID: 9115
		private LineRenderer _keyframeAngleRenderer;

		// Token: 0x0400239C RID: 9116
		private BoxRenderer _keyframeBoxRenderer;

		// Token: 0x0400239D RID: 9117
		private List<ParticleSystemProxy> _particleSystemProxies = new List<ParticleSystemProxy>();

		// Token: 0x040023A1 RID: 9121
		private Vector3[] _pathControlPositions;

		// Token: 0x040023A4 RID: 9124
		private Matrix _modelMatrix;

		// Token: 0x040023A5 RID: 9125
		private Matrix _tempMatrix;

		// Token: 0x040023A6 RID: 9126
		private List<Tuple<Vector3, Vector3, Vector3>> _positions;

		// Token: 0x040023A7 RID: 9127
		private Dictionary<KeyframeSettingType, List<TrackKeyframe>> _keyframeData;

		// Token: 0x02000E00 RID: 3584
		public enum TrackPathType
		{
			// Token: 0x040044CF RID: 17615
			Line,
			// Token: 0x040044D0 RID: 17616
			Spline,
			// Token: 0x040044D1 RID: 17617
			Bezier
		}
	}
}
