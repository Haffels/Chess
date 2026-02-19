namespace Game.Logic
{
    public class Timer
    {
        private int whiteTimeSeconds;
        private int blackTimeSeconds;
        private bool isRunning;
        private Thread timerThread;
        private char currentSide;
        private DateTime lastUpdate;

        public event Action<char> onTimeExpired;

        public Timer(int timePerSideInSeconds = 600)
        {
            whiteTimeSeconds = timePerSideInSeconds;
            blackTimeSeconds = timePerSideInSeconds;
            isRunning = false;
        }

        public void Start(char side)
        {
            if (isRunning) Stop();
            currentSide = side;
            isRunning   = true;
            lastUpdate  = DateTime.Now;
            timerThread = new Thread(TimerCountdown);
            timerThread.IsBackground = true;
            timerThread.Start();
        }

        public void Stop()
        {
            isRunning = false;
            if (timerThread != null && timerThread.IsAlive)
                timerThread.Join(100);
        }

        private void TimerCountdown()
        {
            while (isRunning)
            {
                Thread.Sleep(100);
                double elapsed = (DateTime.Now - lastUpdate).TotalSeconds;
                if (elapsed >= 1.0)
                {
                    lastUpdate = DateTime.Now;
                    if (currentSide == 'w')
                    {
                        whiteTimeSeconds--;
                        if (whiteTimeSeconds <= 0)
                        {
                            whiteTimeSeconds = 0;
                            isRunning = false;
                            if (onTimeExpired != null) onTimeExpired('w');
                        }
                    }
                    else
                    {
                        blackTimeSeconds--;
                        if (blackTimeSeconds <= 0)
                        {
                            blackTimeSeconds = 0;
                            isRunning = false;
                            if (onTimeExpired != null) onTimeExpired('b');
                        }
                    }
                }
            }
        }

        public int GetRemainingTime(char side)
        {
            return side == 'w' ? whiteTimeSeconds : blackTimeSeconds;
        }

        public string GetFormattedTime(char side)
        {
            int totalSeconds = GetRemainingTime(side);
            return $"{totalSeconds / 60:D2}:{totalSeconds % 60:D2}";
        }

        public void DisplayTimers()
        {
            Console.WriteLine($"White: {GetFormattedTime('w')} | Black: {GetFormattedTime('b')}");
        }
    }
}
