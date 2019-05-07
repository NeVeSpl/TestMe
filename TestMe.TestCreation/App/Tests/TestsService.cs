using System.Collections.Generic;
using System.Linq;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Tests.Input;
using TestMe.TestCreation.App.Tests.Output;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Tests
{
    internal sealed class TestsService : ITestsService
    {
        private readonly TestReader testReader;
        private readonly ITestCreationUoW uow;


        public TestsService(TestReader testReader, ITestCreationUoW uow)
        {
            this.testReader = testReader;
            this.uow = uow;
        }


        public Result<List<TestHeaderDTO>> ReadTestHeaders(long ownerId, long catalogId)
        {
            return testReader.GetTestHeaders(ownerId, catalogId);
        }

        public Result<TestDTO> ReadTestWithQuestionItemsAndQuestionHeaders(long ownerId, long testId)
        {
            return testReader.GetTest(ownerId, testId, true);
        }

        public Result<long> CreateTest(long ownerId, CreateTest createTest)
        {
            var catalog = uow.TestsCatalogs.GetById(createTest.CatalogId.Value);

            if (catalog == null)
            {
                return Result.Error("Catalog not found");
            }
            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            var test = Test.Create(ownerId, createTest.Title);
            catalog.AddTest(test);            
            uow.Save();

            return Result.Ok(test.TestId);
        }

        public Result UpdateTest(long ownerId, long testId, UpdateTest updateTest)
        {
            Test test = uow.Tests.GetByIdWithTestItems(testId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            test.Title = updateTest.Title;

            if (test.CatalogId != updateTest.CatalogId)
            {
                Catalog destination = uow.TestsCatalogs.GetById(updateTest.CatalogId.Value);
                // todo : implement moving between catalogs
            }

            uow.Save();

            return Result.Ok();
        }

        public Result DeleteTest(long ownerId, long testId)
        {
            var test = uow.Tests.GetByIdWithTestItems(testId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            test.Delete();
            uow.Save();

            return Result.Ok();
        }


        public Result<long> CreateQuestionItem(long ownerId, long testId, AddQuestionItem addQuestionItem)
        {           
            Test test = uow.Tests.GetByIdWithTestItems(testId);
            Question question = uow.Questions.GetByIdWithAnswers(addQuestionItem.QuestionId.Value);

            if (test == null) 
            {
                return Result.NotFound();
            }
            if (question == null)
            {
                return Result.Error("Question not found");
            }
            if ((test.OwnerId != ownerId) || (question.OwnerId != ownerId))
            {
                return Result.Unauthorized();
            }  

            QuestionItem item = test.AddQuestion(question);
            uow.Save();

            return Result.Ok(item.QuestionItemId);
        }

        public Result UpdateQuestionItem(long ownerId, long testId, long questionItemId, UpdateQuestionItem updateQuestionItem)
        {
            Test test = uow.Tests.GetByIdWithTestItems(testId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            QuestionItem item = test.Questions.FirstOrDefault(x => x.QuestionItemId == questionItemId);

            if (item == null)
            {
                return Result.NotFound();
            }
            // todo : update question item

            uow.Save();

            return Result.Ok();
        }

        public Result DeleteQuestionItem(long ownerId, long testId, long questionItemId)
        {
            Test test = uow.Tests.GetByIdWithTestItems(testId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            QuestionItem item = test.Questions.FirstOrDefault(x => x.QuestionItemId == questionItemId);

            if (item == null)
            {
                return Result.NotFound();
            }

            test.RemoveQuestion(item);
            uow.Save();

            return Result.Ok();
        }
    }
}
