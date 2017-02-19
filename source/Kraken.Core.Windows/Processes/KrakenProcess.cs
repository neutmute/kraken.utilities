using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common.Logging;

namespace Kraken.Core
{
    public enum ProcessVerb
    {
        None,
        Edit,
        Print,
        OpenAs,
        /// <summary>
        /// Triggers elevated 
        /// </summary>
        RunAs
    }

    public class KrakenProcess : IDisposable
    {
        #region Fields/Events

        private Process _process;
        private static readonly List<Process> _processes;

        public event EventHandler Exited;

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #endregion

        #region Properties

        protected string FriendlyName { get; set; }

        protected string Filename { get; set; }

        protected string Arguments { get; set; }

        protected bool UseShellExecute { get; set; }

        protected bool CreateNoWindow { get; set; }

        protected string WorkingDirectory { get; set; }
        
        public bool SwallowExceptions { get; set; }

        protected ProcessVerb Verb { get; set; }

        #endregion

        #region Ctor

        protected KrakenProcess()
        {
            UseShellExecute = true;
            SwallowExceptions = true;
            CreateNoWindow = false;
            Verb = ProcessVerb.None;
        }

        #endregion

        #region Methods

        public virtual void Start()
        {
            StartWorker(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Exit code of started process</returns>
        public int StartWithWait()
        {
            return StartWorker(true);
        }

        /// <returns>Exit code of started process</returns>
        private int StartWorker(bool shouldWait)
        {
            try
            {
                Log.Info(m=>m(
                    "Executing {0}{3}: {1} {2}"
                    , FriendlyName
                    , Filename
                    , Arguments
                    , Verb == ProcessVerb.None ? string.Empty : " with verb " + Verb));

                _process = new Process();

                _process.StartInfo.FileName = Filename;
                _process.StartInfo.Verb = Verb == ProcessVerb.None ? null : Verb.ToString().ToLower();
                _process.StartInfo.Arguments = Arguments;
                _process.StartInfo.CreateNoWindow = CreateNoWindow;
                _process.StartInfo.UseShellExecute = UseShellExecute;
                _process.StartInfo.ErrorDialog = !SwallowExceptions;

                _process.StartInfo.WorkingDirectory = WorkingDirectory;
    
                _process.Exited += BubbleExited;
                _processes.Add(_process);

                _process.Start();

                if (shouldWait)
                {
                    _process.WaitForExit();

                    if (_process.ExitCode != 0)
                    {
                        Log.Warn(m => m("{0} exited with ErrorLevel={1}", FriendlyName, _process.ExitCode));
                    }

                    return _process.ExitCode;
                }
            }
            catch (Exception ex)
            {
                Log.Error(m => m("Execution of '{0}' failed: ({1})", FriendlyName, Filename), ex);
            }
            return 0;
        }

        void BubbleExited(object sender, EventArgs e)
        {
            if (Exited != null)
            {
                Exited(this, e);
            }
        }

        static KrakenProcess()
        {
            _processes = new List<Process>();
        }

        public static void CloseAll()
        {
            _processes.Where(p => !p.HasExited).ToList().ForEach(p1 => p1.CloseMainWindow());
        }

        public void Dispose()
        {
            if (_process != null)
            {
                _process.Dispose();
                _process = null;
            }
        }

        #endregion

    }
}
