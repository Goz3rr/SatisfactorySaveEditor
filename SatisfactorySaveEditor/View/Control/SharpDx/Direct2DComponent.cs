using System;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;
using WriteFactory = SharpDX.DirectWrite.Factory;

namespace WpfSharpDxControl
{
	/// <summary>
	/// Creates Direct2D RenderTarget on top of DirectX element.
	/// </summary>
	public abstract class Direct2DComponent : DirectXComponent
	{
		private Factory _factory2D;
		private SharpDX.DirectWrite.Factory _factoryDWrite;
		private RenderTarget _renderTarget2D;

		protected Factory Factory2D => _factory2D;
		protected WriteFactory FactoryDWrite => _factoryDWrite;
		protected RenderTarget RenderTarget2D => _renderTarget2D;

		protected override void InternalInitialize()
		{
			base.InternalInitialize();

			_factory2D = new Factory();
			_factoryDWrite = new SharpDX.DirectWrite.Factory();

			using (var surface = BackBuffer.QueryInterface<Surface>())
			{
				_renderTarget2D = new RenderTarget(_factory2D, surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
			}
			_renderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
		}

		protected override void InternalUninitialize()
		{
			Utilities.Dispose(ref _renderTarget2D);
			Utilities.Dispose(ref _factory2D);
			Utilities.Dispose(ref _factoryDWrite);

			base.InternalUninitialize();
		}

		protected override void BeginRender()
		{
			base.BeginRender();

			_renderTarget2D.BeginDraw();
			_renderTarget2D.Clear(Color.Orange);
		}

		protected override void EndRender()
		{
			_renderTarget2D.EndDraw();

			base.EndRender();
		}
	}
}
