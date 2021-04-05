using QandA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QandA.Data
{
    public interface IDataRepository
    {
        Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsAsync();

        Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsWithAnswersAsync();

        Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsBySearchAsync(string search);

        Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsBySearchWithPagingAsync(string search, int pageNumber, int pageSize);

        Task<IEnumerable<QuestionGetManyResponse>> GetUnansweredQuestionsAsync();

        Task<QuestionGetSingleResponse> GetQuestionAsync(int questionId);

        Task<bool> QuestionExistsAsync(int questionId);

        Task<AnswerGetResponse> GetAnswerAsync(int answerId);

        Task<QuestionGetSingleResponse> PostQuestionAsync(QuestionPostFullRequest question);
        
        Task<QuestionGetSingleResponse> PutQuestionAsync(int questionId, QuestionPutRequest question);
        
        Task DeleteQuestionAsync(int questionId);

        Task<AnswerGetResponse> PostAnswerAsync(AnswerPostFullRequest answer);
    }
}
