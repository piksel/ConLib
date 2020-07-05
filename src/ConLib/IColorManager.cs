namespace ConLib
{
    public interface IColorManager
    {
        public void PushColor(ConCol color);
        public ConCol PopColor();
        public ConCol PeekColor();
    }
}