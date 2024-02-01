using System.Text;

namespace Application.Helpers
{
    public static class StringHelper
    {
        public static string ErrorListToString(IEnumerable<string> errors)
        {
            var message = new StringBuilder();
            foreach (var error in errors)
            {
                message.AppendLine($"- {error}");
            }

            return message.ToString().Trim();
        }
    }
}
