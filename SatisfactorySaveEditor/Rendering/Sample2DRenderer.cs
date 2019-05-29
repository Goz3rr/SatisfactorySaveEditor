using SharpDX;
using SharpDX.Direct2D1;
using WpfSharpDxControl;

namespace SatisfactorySaveEditor.Rendering
{
    class Sample2DRenderer : Direct2DComponent
    {
        private Vector2 _position;
        private Vector2 _speed;
        private SolidColorBrush _circleColor;

        public Sample2DRenderer()
        {
            _position = new Vector2(30, 30);
            _speed = new Vector2(5, 2);
            
        }

        protected override void InternalInitialize()
        {
            base.InternalInitialize();

            _circleColor = new SolidColorBrush(RenderTarget2D, new Color(1, 0.2f, 0.2f));
        }

        protected override void InternalUninitialize()
        {
            Utilities.Dispose(ref _circleColor);

            base.InternalUninitialize();
        }

        protected override void Render()
        {
            UpdatePosition();

            RenderTarget2D.Clear(new Color(1.0f, 0, 1.0f));
            RenderTarget2D.FillEllipse(new Ellipse(_position, 20, 20), _circleColor);
        }

        private void UpdatePosition()
        {
            _position += _speed;

            if (_position.X > SurfaceWidth)
            {
                _position.X = SurfaceWidth;
                _speed.X = -_speed.X;
            }
            else if (_position.X < 0)
            {
                _position.X = 0;
                _speed.X = -_speed.X;
            }

            if (_position.Y > SurfaceHeight)
            {
                _position.Y = SurfaceHeight;
                _speed.Y = -_speed.Y;
            }
            else if (_position.Y < 0)
            {
                _position.Y = 0;
                _speed.Y = -_speed.Y;
            }
        }
    }
}
