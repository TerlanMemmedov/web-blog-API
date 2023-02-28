
using System.ComponentModel;
using System.Net;
using web_blog.Data.Enums;

namespace web_blog.Data.Errors
{
    public class ErrorFormat
    {
        private Dictionary<ErrorNamesForMessages, string> errorMess = new Dictionary<ErrorNamesForMessages, string>()
        { 
            [ErrorNamesForMessages.ArticleNotFound] = "Article that you are looking for not found",
            [ErrorNamesForMessages.ProblemWithUser] = "There is a problem with your profile. If problem continues," +
            " try to log out and sign in again",
            [ErrorNamesForMessages.CommentNotFound] = "Comment that you are looking for not found",
            [ErrorNamesForMessages.ArticleCantPosted] = "Problem with the server. Article can't be posted. Try the solutions: 1. Try to refresh the page 2. Try log out and sign in again, and post your article again"
        };

        [DisplayName("Error")]
        public string NameOfError { get; set; }

        [DisplayName("Error Message")]
        public string MessageofError { get; set; }

        public ErrorFormat(int numStatus, ErrorNamesForMessages message)
        {
            string name = Enum.GetName(typeof(HttpStatusCode), numStatus).ToString();

            this.NameOfError = "Error: " + numStatus.ToString() + " - " + name;
            this.MessageofError = errorMess[message];
        }

        public static ErrorFormat GetErrorJsonType(int numStatus, ErrorNamesForMessages message)
        {
            var ErrorInJson = new ErrorFormat(numStatus, message);

            return ErrorInJson;
        }


    }
}
