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

        public Timer(int timePerSideInSeconds = 600)
        {
            whiteTimeSeconds = timePerSideInSeconds;
            blackTimeSeconds = timePerSideInSeconds;
            isRunning = false;
        }
        
        public void Start(char side)
        {
            if (isRunning)
            {
                stop();
            }

            currentSide = side;
            isRunning = true;
            lastUpdate = DateTime.Now;

            timerThread = new Thread(timerCountdown);
            timerThread.IsBackground = true;
            timerThread.Start();
        }
        
        public void stop()
        {
            isRunning = false;
            if (timerThread != null && timerThread.IsAlive)
            {
                timerThread.Join(100); // Wait briefly for thread to finish
            }
        }
        
        private void timerCountdown()
        {
            while (isRunning)
            {
                Thread.Sleep(100); // Update every 100ms for smoother display
                
                DateTime now = DateTime.Now;
                double elapsedSeconds = (now - lastUpdate).TotalSeconds;
                
                if (elapsedSeconds >= 1.0)
                {
                    lastUpdate = now;
                    
                    if (currentSide == 'w')
                    {
                        whiteTimeSeconds--;
                        if (whiteTimeSeconds <= 0)
                        {
                            whiteTimeSeconds = 0;
                            isRunning = false;
                            onTimeExpired?.Invoke('w');
                        }
                    }
                    else
                    {
                        blackTimeSeconds--;
                        if (blackTimeSeconds <= 0)
                        {
                            blackTimeSeconds = 0;
                            isRunning = false;
                            onTimeExpired?.Invoke('b');
                        }
                    }
                }
            }
        }
        
        public event Action<char> onTimeExpired;
        
        public int getRemainingTime(char side)
        {
            return side == 'w' ? whiteTimeSeconds : blackTimeSeconds;
        }
        
        public string getFormattedTime(char side)
        {
            int TotalSeconds = getRemainingTime(side);
            int minutes = TotalSeconds / 60;
            int seconds = TotalSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }
        
        public void displayTimers()
        {
            Console.WriteLine($"White: {getFormattedTime('w')} | Black: {getFormattedTime('b')}");
        }
    }
}