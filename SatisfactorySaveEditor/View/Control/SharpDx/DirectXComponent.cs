using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;

namespace WpfSharpDxControl
{
	/// <summary>
	/// Create SharpDx swapchain hosted in the controls parent Hwnd
	/// Resources created on Loaded, destroyed on Unloaded. 
	/// </summary>
	public abstract class DirectXComponent : Win32HwndControl
	{
		private Device _device;
		private SwapChain _swapChain;
		private Texture2D _backBuffer;
		private RenderTargetView _renderTargetView;

		protected Device Device => _device;
		protected SwapChain SwapChain => _swapChain;
		protected Texture2D BackBuffer => _backBuffer;
		protected RenderTargetView RenderTargetView => _renderTargetView;

		protected int SurfaceWidth { get; private set; }
		protected int SurfaceHeight { get; private set; }

		public bool Rendering { get; private set; }

		protected DirectXComponent()
		{
		}

		protected override sealed void Initialize()
		{
			InternalInitialize();

			Rendering = true;
			CompositionTarget.Rendering += OnCompositionTargetRendering;
		}

		protected override sealed void Uninitialize()
		{
			Rendering = false;
			CompositionTarget.Rendering -= OnCompositionTargetRendering;

			InternalUninitialize();
		}

        protected sealed override void Resized()
        {
 	        InternalUninitialize();
            InternalInitialize();
        }

		private void OnCompositionTargetRendering(object sender, EventArgs eventArgs)
		{
			if (!Rendering)
				return;

			try
			{
				BeginRender();
				Render();
				EndRender();
			}
			catch (SharpDXException e)
			{
				if (e.HResult == HResults.D2DERR_RECREATE_TARGET || e.HResult == HResults.DXGI_ERROR_DEVICE_REMOVED)
				{
					Uninitialize();
					Initialize();
				}
				else throw;
			}
		}

		private double GetDpiScale()
		{
			PresentationSource source = PresentationSource.FromVisual(this);

			return source.CompositionTarget.TransformToDevice.M11;
		}

		/// <summary>
		/// Create required DirectX resources.
		/// Derived calls should begin with base.InternalInitialize()
		/// </summary>
		protected virtual void InternalInitialize()
		{
			var dpiScale = GetDpiScale();
			SurfaceWidth = (int)(ActualWidth < 0 ? 0 : Math.Ceiling(ActualWidth * dpiScale));
			SurfaceHeight = (int)(ActualHeight < 0 ? 0 : Math.Ceiling(ActualHeight * dpiScale));

			var swapChainDescription = new SwapChainDescription
			{
				OutputHandle = Hwnd,
				BufferCount = 1,
				Flags = SwapChainFlags.AllowModeSwitch,
				IsWindowed = true,
				ModeDescription = new ModeDescription(SurfaceWidth, SurfaceHeight, new Rational(60, 1), Format.B8G8R8A8_UNorm),
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SwapEffect.Discard,
				Usage = Usage.RenderTargetOutput | Usage.Shared
			};

			SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, swapChainDescription, out _device, out _swapChain);

			// Ignore all windows events
			using (var factory = _swapChain.GetParent<Factory>())
			{
				factory.MakeWindowAssociation(Hwnd, WindowAssociationFlags.IgnoreAll);
			}

			// New RenderTargetView from the backbuffer
			_backBuffer = Resource.FromSwapChain<Texture2D>(_swapChain, 0);
			_renderTargetView = new RenderTargetView(_device, _backBuffer);
		}

		/// <summary>
		/// Destory all DirectX resources.
		/// Derived methods should end with base.InternalUninitialize();
		/// </summary>
		protected virtual void InternalUninitialize()
		{
			Utilities.Dispose(ref _renderTargetView);
			Utilities.Dispose(ref _backBuffer);
			Utilities.Dispose(ref _swapChain);

			// This is a workaround for an issue in SharpDx3.0.2 (https://github.com/sharpdx/SharpDX/issues/731)
			// Will need to be removed when fixed in next SharpDx release
			((IUnknown)_device).Release();
			Utilities.Dispose(ref _device);

			GC.Collect(2, GCCollectionMode.Forced);
		}

		/// <summary>
		/// Begin render.
		/// Derived methods should begin with base.BeginRender()
		/// </summary>
		protected virtual void BeginRender()
		{
			_device.ImmediateContext.Rasterizer.SetViewport(0, 0, (float)ActualWidth, (float)ActualHeight);
			_device.ImmediateContext.OutputMerger.SetRenderTargets(_renderTargetView);
		}

		/// <summary>
		/// Finish render.
		/// Derived methods must call base.EndRender() 
		/// </summary>
		protected virtual void EndRender()
		{
			_swapChain.Present(1, PresentFlags.None);
		}

		/// <summary>
		/// Perform render.
		/// </summary>
		protected abstract void Render();
	}

	internal class HResults
	{
		// ReSharper disable InconsistentNaming
		public const int D2DERR_RECREATE_TARGET		= unchecked((int)0x8899000C);
		public const int DXGI_ERROR_DEVICE_REMOVED	= unchecked((int)0x887A0005);
		// ReSharper restore InconsistentNaming
	}
}
