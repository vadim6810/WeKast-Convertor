using System;
using System.ComponentModel;
using System.Threading;

namespace WeCastConvertor.Utils
{
    public class AbortableBackgroundWorker : BackgroundWorker
    {
        public AbortableBackgroundWorker()
        {
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
        }
        /// <summary>
        /// Start work in current thread
        /// </summary>
        /// <param name="objectState"></param>
        public void RunWork(object objectState)
        {
            DoWorkEventArgs args = new DoWorkEventArgs(objectState);
            Exception eee = null;
            try { OnDoWork(args); }
            catch (Exception ex) { eee = ex; }
            OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(args.Result, eee, args.Cancel));
        }
        public object Tag { get; set; }
        private Thread workerThread;

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            workerThread = Thread.CurrentThread;
            try
            {
                base.OnDoWork(e);
                workerThread = null;
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true; //We must set Cancel property to true!
                Thread.ResetAbort(); //Prevents ThreadAbortException propagation
            }
        }
        /// <summary>
        /// Kill the background thread
        /// </summary>
        public void Abort()
        {
            if (!IsBusy) return;
            CancelAsync();
            try
            {
                if (workerThread != null)
                {
                    workerThread.Abort();
                    workerThread = null;
                }
            }
            catch { }
        }
    }

}
