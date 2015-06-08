using System;

namespace ContactConvert
{
    public class TextProgressBar : IDisposable
    {
        private readonly int _total;
        private readonly int _width;
        private int _cursorRow;
        private int _lastFilledSlots;
        private int _progress;

        public TextProgressBar(int total, int width = -1, bool drawImmediately = true)
        {
            _progress = 0;
            _total = total;

            if (width < 0)
                _width = Console.WindowWidth - string.Format("[]   {0} of {0}", _total).Length;
            else
                _width = width;

            _lastFilledSlots = -1;
            _cursorRow = -1;

            if (drawImmediately)
                Update(0);
        }

        public void Dispose()
        {
            Update(_total);

            if (Console.CursorTop == _cursorRow)
                Console.WriteLine("");
        }

        public void Update(int progress)
        {
            _progress = Math.Max(Math.Min(progress, _total), 0);

            if (_cursorRow < 0)
            {
                _cursorRow = Console.CursorTop;
                Console.CursorTop++;
                Console.CursorLeft = 0;
            }

            var filledSlots = (int) Math.Floor(_width*((double) progress)/_total);
            if (filledSlots != _lastFilledSlots)
            {
                _lastFilledSlots = filledSlots;
                DrawBar();
            }
            DrawText();
            if (Console.CursorTop == _cursorRow)
                Console.CursorLeft = Console.WindowWidth - 1;
        }

        public void ForceDraw()
        {
            DrawBar();
            DrawText();
            if (Console.CursorTop == _cursorRow)
                Console.CursorLeft = Console.WindowWidth - 1;
        }

        public static TextProgressBar operator ++(TextProgressBar bar)
        {
            bar.Increment();
            return bar;
        }

        public void Increment()
        {
            Update(_progress + 1);
        }

        private void DrawBar()
        {
            using (new ConsoleStateSaver())
            {
                Console.CursorVisible = false;
                Console.CursorTop = _cursorRow;

                // Draw the outline of the progress bar
                Console.CursorLeft = _width + 1;
                Console.Write("]");
                Console.CursorLeft = 0;
                Console.Write("[");

                // Draw progressed part
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(new string(' ', _lastFilledSlots));

                // Draw remaining part
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(new string(' ', _width - _lastFilledSlots));
            }
        }

        private void DrawText()
        {
            using (new ConsoleStateSaver())
            {
                // Write progress text
                Console.CursorVisible = false;
                Console.CursorTop = _cursorRow;
                Console.CursorLeft = _width + 4;
                Console.Write("{0} of {1}", _progress.ToString().PadLeft(_total.ToString().Length), _total);
                Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
            }
        }

        private class ConsoleStateSaver : IDisposable
        {
            private readonly ConsoleColor _bgColor;
            private readonly int _cursorLeft;
            private readonly int _cursorTop;
            private readonly bool _cursorVisible;

            public ConsoleStateSaver()
            {
                _bgColor = Console.BackgroundColor;
                _cursorTop = Console.CursorTop;
                _cursorLeft = Console.CursorLeft;
                _cursorVisible = Console.CursorVisible;
            }

            public void Dispose()
            {
                RestoreState();
            }

            private void RestoreState()
            {
                Console.BackgroundColor = _bgColor;
                Console.CursorTop = _cursorTop;
                Console.CursorLeft = _cursorLeft;
                Console.CursorVisible = _cursorVisible;
            }
        }
    }
}