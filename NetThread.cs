
using System;
using System.Threading;


namespace Game.Network
{
    public sealed class NetThread
    {
        private Thread _thread;

        private readonly Action _callback;

        public int SleepTime;
        private bool _running;
        private readonly string _name;

        public bool IsRunning
        {
            get { return _running; }
        }

        public NetThread(string name, int sleepTime, Action callback)
        {
            _callback = callback;
            SleepTime = sleepTime;
            _name = name;
        }

        public void Start()
        {
            if (_running)
                return;
            _running = true;
            _thread = new Thread(ThreadLogic)
            {
                Name = _name,
                IsBackground = true
            };
            _thread.Start();
        }

        public void Stop()
        {
            if (!_running)
                return;
            _thread.Interrupt();
            _running = false;

            _thread.Join();
        }
        private void ThreadLogic()
        {
            while (_running)
            {
                _callback();
                Thread.Sleep(SleepTime);
            }
            try
            {
               Thread.Sleep(SleepTime);
            }
            catch (ThreadInterruptedException e)
           {
               // NetworkDebug.Debug.Log(LogerType.INFO, "cannot go to sleep  interrupted by main thread" + e.Message);
            }
        }
    }
}
