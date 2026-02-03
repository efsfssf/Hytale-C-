using System;
using HytaleClient.Core;
using SDL2;

namespace HytaleClient.Graphics
{
	// Token: 0x0200099F RID: 2463
	public class Cursors : Disposable
	{
		// Token: 0x06004F19 RID: 20249 RVA: 0x00163B3C File Offset: 0x00161D3C
		public Cursors()
		{
			this.Arrow = SDL.SDL_CreateSystemCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_ARROW);
			this.IBeam = SDL.SDL_CreateSystemCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_IBEAM);
			this.Hand = SDL.SDL_CreateSystemCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_HAND);
			this.Move = SDL.SDL_CreateSystemCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZEALL);
			this.SizeWE = SDL.SDL_CreateSystemCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZEWE);
			this.SizeNS = SDL.SDL_CreateSystemCursor(SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZENS);
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x00163B9C File Offset: 0x00161D9C
		protected override void DoDispose()
		{
			SDL.SDL_FreeCursor(this.Hand);
			SDL.SDL_FreeCursor(this.IBeam);
			SDL.SDL_FreeCursor(this.Hand);
			SDL.SDL_FreeCursor(this.Move);
			SDL.SDL_FreeCursor(this.SizeWE);
			SDL.SDL_FreeCursor(this.SizeNS);
		}

		// Token: 0x04002A58 RID: 10840
		public readonly IntPtr Arrow;

		// Token: 0x04002A59 RID: 10841
		public readonly IntPtr IBeam;

		// Token: 0x04002A5A RID: 10842
		public readonly IntPtr Hand;

		// Token: 0x04002A5B RID: 10843
		public readonly IntPtr Move;

		// Token: 0x04002A5C RID: 10844
		public readonly IntPtr SizeWE;

		// Token: 0x04002A5D RID: 10845
		public readonly IntPtr SizeNS;
	}
}
