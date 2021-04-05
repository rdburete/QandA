using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using QandA.Controllers;
using QandA.Data;
using QandA.Data.Cache;
using QandA.Data.Models;
using Xunit;

namespace BackendTests
{
    public class QuestionsControllerTets
    {
        [Fact]
        public async void GetQuestions_WhenNoParameters_ReturnsAllQuestions()
        {
            var mockQuestions = new List<QuestionGetManyResponse>();

            for (int i = 1; i <= 10; i++)
            {
                mockQuestions.Add(new QuestionGetManyResponse
                {
                    QuestionId = 1,
                    Title = $"Test title {i}",
                    Content = $"Test content {i}",
                    UserName = "User1",
                    Answers = new List<AnswerGetResponse>()
                });
            }

            // Use Setup() for mocking methods
            var mockDataRepository = new Mock<IDataRepository>();
            mockDataRepository
                .Setup(repo => repo.GetQuestionsAsync())
                .Returns(() => Task.FromResult(mockQuestions.AsEnumerable()));

            // Use SetupGet() for mocking property getters
            var mockConfiguration = new Mock<IConfigurationRoot>();
            mockConfiguration
                .SetupGet(config => config[It.IsAny<string>()])
                .Returns("some settings");

            var questionsController = new QuestionsController(
                mockDataRepository.Object,
                null,
                null,
                mockConfiguration.Object);

            var result = await questionsController.GetQuestions(null, false);

            Assert.Equal(10, result.Count());
            mockDataRepository.Verify(mock => mock.GetQuestionsAsync(), Times.Once());
        }

        [Fact]
        public async void GetQuestions_WhenHaveSearchParameter_ReturnsCorrectQuestions()
        {
            var mockQuestions = new List<QuestionGetManyResponse>();

            mockQuestions.Add(new QuestionGetManyResponse
            {
                QuestionId = 1,
                Title = $"Test title",
                Content = $"Test content",
                UserName = "User1",
                Answers = new List<AnswerGetResponse>()
            });

            var mockDataRepository = new Mock<IDataRepository>();
            mockDataRepository
                .Setup(repo => repo.GetQuestionsBySearchWithPagingAsync("Test", 1, 20))
                .Returns(() => Task.FromResult(mockQuestions.AsEnumerable()));

            var mockConfiguration = new Mock<IConfigurationRoot>();
            mockConfiguration
                .SetupGet(config => config[It.IsAny<string>()])
                .Returns("some settings");

            var questionsController = new QuestionsController(
                mockDataRepository.Object,
                null,
                null,
                mockConfiguration.Object);

            var result = await questionsController.GetQuestions("Test", false);

            Assert.Single(result);
            mockDataRepository.Verify(mock => mock.GetQuestionsBySearchWithPagingAsync("Test", 1, 20), Times.Once());
        }

        [Fact]
        public async void GetQuestion_WhenQuestionNotFound_Returns404()
        {
            var mockDataRepository = new Mock<IDataRepository>();
            mockDataRepository
                .Setup(repo => repo.GetQuestionAsync(1))
                .Returns(() => Task.FromResult(default(QuestionGetSingleResponse)));

            var mockQuestionCache = new Mock<IQuestionCache>();
            mockQuestionCache
                .Setup(cache => cache.Get(1))
                .Returns(() => null);

            var mockConfiguration = new Mock<IConfigurationRoot>();
            mockConfiguration
                .SetupGet(config => config[It.IsAny<string>()])
                .Returns("some settings");

            var questionsController = new QuestionsController(
                mockDataRepository.Object,
                mockQuestionCache.Object,
                null,
                mockConfiguration.Object);

            var result = await questionsController.GetQuestion(1);

            var actionResult = Assert.IsType<ActionResult<QuestionGetSingleResponse>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async void GetQuestion_WhenQuestionIsFound_ReturnsQuestion()
        {
            var mockQuestion = new QuestionGetSingleResponse
            {
                QuestionId = 1,
                Title = "Test"
            };

            var mockDataRepository = new Mock<IDataRepository>();
            mockDataRepository
                .Setup(repo => repo.GetQuestionAsync(1))
                .Returns(() => Task.FromResult(mockQuestion));

            var mockQuestionCache = new Mock<IQuestionCache>();
            mockQuestionCache
                .Setup(cache => cache.Get(1))
                .Returns(() => mockQuestion);

            var mockConfiguration = new Mock<IConfigurationRoot>();
            mockConfiguration
                .SetupGet(config => config[It.IsAny<string>()])
                .Returns("some settings");

            var questionsController = new QuestionsController(
                mockDataRepository.Object,
                mockQuestionCache.Object,
                null,
                mockConfiguration.Object);

            var result = await questionsController.GetQuestion(1);

            var actionResult = Assert.IsType<ActionResult<QuestionGetSingleResponse>>(result);
            var questionResult = Assert.IsType<QuestionGetSingleResponse>(actionResult.Value);
            Assert.Equal(1, questionResult.QuestionId);
        }
    }
}
