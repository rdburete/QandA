using QandA.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QandA.Data.Cache
{
    public interface IQuestionCache
    {
        QuestionGetSingleResponse Get(int questionId);

        void Remove(int questionId);

        void Set(QuestionGetSingleResponse question);
    }
}
