using System.Text;

namespace Core.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetFullExceptionMessage(this Exception obj)
        {
            if (obj is AggregateException ae)
                return GetFullExceptionMessage(ae);

            if (obj == null)
                return "empty info";
            var n = (obj.InnerException != null) ? "\r\n" : "";
            string result = $"{obj.Message}{n}";
            while (obj.InnerException != null)
            {
                string n1 = ((obj.InnerException).InnerException != null) ? "\r\n" : "";
                result += $"{obj.InnerException.Message}{n1}";
                obj = obj.InnerException;
            }
            return $"{result} {obj.StackTrace}";
        }

        public static string GetFullExceptionMessage(this AggregateException obj)
        {
            if (obj == null)
                return "empty info";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(obj.Message);
            foreach (var e in obj.InnerExceptions)
                sb.AppendLine(GetFullExceptionMessage(e));
            return sb.ToString();
        }
    }
}
