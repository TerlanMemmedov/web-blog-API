//using web_blog.Data.Models;
//using web_blog.Data.ViewModels;

//namespace web_blog.Data.Services
//{
//    public class ArticlesSortingService
//    {
//        public ArticlesSortingService() { }


//        //Selection sort for number of read
//        public void SortByNumberOfReadMostOnTop(ref List<ArticleWithAuthorComEmoVM> articles)
//        {
//            for (int i = 0; i < articles.Count - 1; i++)
//            {
//                int index = i;
//                for (int y = i + 1; y < articles.Count; y++)
//                {
//                    if (articles[index].NumberOfRead < articles[y].NumberOfRead)
//                    {
//                        index = y;
//                    }
//                }

//                ArticleWithAuthorComEmoVM tempArticle = articles[index];
//                articles[index] = articles[i];
//                articles[i] = tempArticle;
//            }
//            //no return for now :). I did it with ref
//        }

//        //Selection sort for number of read
//        //there is a problem with selection sort. When we want to get most interactions, if there is two or more
//        //elements with same comment count, they will be randomly ordered according to the program and article count
//        //we can change a new sorting type
//        public void SortByMostInteractedForCommentsOnTop(ref List<ArticleWithAuthorComEmoVM> articles)
//        {
//            for (int i = 0; i < articles.Count - 1; i++)
//            {
//                int index = i;
//                for (int y = i + 1; y < articles.Count; y++)
//                {
//                    if (articles[index].Comments.Count < articles[y].Comments.Count)
//                    {
//                        index = y;
//                    }
//                }

//                ArticleWithAuthorComEmoVM tempArticle = articles[index];
//                articles[index] = articles[i];
//                articles[i] = tempArticle;
//            }
//            //no return for now :). I did it with ref
//        }
//    }
//}
