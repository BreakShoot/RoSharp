using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RoSharp
{
    class ProcessWatcher : IDisposable
    {
        public event OnProcessCreatedDelegate Created;
        private readonly Timer _timer;
        private readonly string _processname;
        private bool _disposed = false;
        private Process _process;

        public ProcessWatcher(string processName)
        {
            this._processname = processName;

            this._timer = new Timer();
            this._timer.Elapsed += TimerOnElapsed;
            this._timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName(this._processname);

            if (processes.Length > 0)
            {
                this.OnProcessCreated(processes[0]);
            }
        }

        protected virtual void OnProcessCreated(Process process)
        {
            this._timer.Stop();
            this._process = process;
            this._process.EnableRaisingEvents = true;
            this._process.Exited += (self, e) => this._timer.Start();

            OnProcessCreatedDelegate handler = Created;

            if (handler != null)
            {
                handler(this, process);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _timer.Dispose();
                }

                this._disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public delegate void OnProcessCreatedDelegate(Object sender, Process process);
    }
}
