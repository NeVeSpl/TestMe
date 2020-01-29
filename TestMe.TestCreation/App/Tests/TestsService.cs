using System.Collections.Generic;
using System.Linq;
using TestMe.BuildingBlocks.App;
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

        public Result<long> CreateTest(CreateTest createTest)
        {
            var catalog = uow.TestsCatalogs.GetById(createTest.CatalogId);

            if (catalog == null)
            {
                return Result.Error("Catalog not found");
            }
            if (catalog.OwnerId != createTest.UserId)
            {
                return Result.Unauthorized();
            }

            var test = Test.Create(createTest.UserId, createTest.Title);
            catalog.AddTest(test);            
            uow.Save();

            return Result.Ok(test.TestId);
        }

        public Result UpdateTest(UpdateTest updateTest)
        {
            Test test = uow.Tests.GetByIdWithTestItems(updateTest.TestId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != updateTest.UserId)
            {
                return Result.Unauthorized();
            }

            test.Title = updateTest.Title;

            if (test.CatalogId != updateTest.CatalogId)
            {
                Catalog destination = uow.TestsCatalogs.GetById(updateTest.CatalogId);
                // todo : implement moving test between catalogs
            }

            uow.Save();

            return Result.Ok();
        }

        public Result DeleteTest(DeleteTest deleteTest)
        {
            var test = uow.Tests.GetByIdWithTestItems(deleteTest.TestId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != deleteTest.UserId)
            {
                return Result.Unauthorized();
            }

            test.Delete();
            uow.Save();

            return Result.Ok();
        }


        public Result<long> CreateQuestionItem(CreateQuestionItem addQuestionItem)
        {           
            Test test = uow.Tests.GetByIdWithTestItems(addQuestionItem.TestId);
            Question question = uow.Questions.GetByIdWithAnswers(addQuestionItem.QuestionId);

            if (test == null) 
            {
                return Result.NotFound();
            }
            if (question == null)
            {
                return Result.Error("Question not found");
            }
            if ((test.OwnerId != addQuestionItem.UserId) || (question.OwnerId != addQuestionItem.UserId))
            {
                return Result.Unauthorized();
            }  

            QuestionItem item = test.AddQuestion(question);
            uow.Save();

            return Result.Ok(item.QuestionItemId);
        }

        public Result UpdateQuestionItem(UpdateQuestionItem updateQuestionItem)
        {
            Test test = uow.Tests.GetByIdWithTestItems(updateQuestionItem.TestId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != updateQuestionItem.UserId)
            {
                return Result.Unauthorized();
            }

            QuestionItem item = test.Questions.FirstOrDefault(x => x.QuestionItemId == updateQuestionItem.QuestionItemId);

            if (item == null)
            {
                return Result.NotFound();
            }
            // todo : update question item

            uow.Save();

            return Result.Ok();
        }

        public Result DeleteQuestionItem(DeleteQuestionItem deleteQuestionItem)
        {
            Test test = uow.Tests.GetByIdWithTestItems(deleteQuestionItem.TestId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != deleteQuestionItem.UserId)
            {
                return Result.Unauthorized();
            }

            QuestionItem item = test.Questions.FirstOrDefault(x => x.QuestionItemId == deleteQuestionItem.QuestionItemId);

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
