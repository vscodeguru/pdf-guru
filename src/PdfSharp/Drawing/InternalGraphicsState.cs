

namespace PdfSharp.Drawing
{
    internal class InternalGraphicsState
    {
        public InternalGraphicsState(XGraphics gfx)
        {
            _gfx = gfx;
        }

        public InternalGraphicsState(XGraphics gfx, XGraphicsState state)
        {
            _gfx = gfx;
            State = state;
            State.InternalState = this;
        }

        public InternalGraphicsState(XGraphics gfx, XGraphicsContainer container)
        {
            _gfx = gfx;
            container.InternalState = this;
        }

        public XMatrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        XMatrix _transform;

        public void Pushed()
        {


        }

        public void Popped()
        {
            Invalid = true;



        }

        public bool Invalid;

        readonly XGraphics _gfx;

        internal XGraphicsState State;
    }
}
