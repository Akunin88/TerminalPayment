using Core.EventModels;
using System;

namespace Client.UI.ErrWin
{
    public class ErrWindowManager : PropertyObject
    {
        private string stackTrace, message, source;

        public string StackTrace { get => stackTrace; set { if (stackTrace != value) { stackTrace = value; raisePropertyChanged(nameof(StackTrace)); } } }
        public string Message { get => message; set { if (message != value) { message = value; raisePropertyChanged(nameof(Message)); } } }
        public string Source { get => source; set { if (source != value) { source = value; raisePropertyChanged(nameof(Source)); } } }

        public ErrWindowManager(Exception e)
        {
            Tuple<string, string> ErrorInfo = GetExceptionInfo(e);
            Message = ErrorInfo.Item1;
            StackTrace = ErrorInfo.Item2;
            Source = e.Source;
        }
        public Tuple<string, string> GetExceptionInfo(Exception ex)
        {
            string Message = ex.Message;
            string StackTrace = ex.StackTrace;
            if (ex.InnerException != null)
            {
                Tuple<string, string> tupleRec = GetExceptionInfo(ex.InnerException);
                Message = $"{ex.Message}\n{tupleRec.Item1}";
                StackTrace = $"{ex.StackTrace}\n{tupleRec.Item2}";
            }
            return new Tuple<string, string>(Message, StackTrace);
        }
    }
}
