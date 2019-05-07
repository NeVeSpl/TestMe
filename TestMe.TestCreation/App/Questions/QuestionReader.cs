﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Questions.Output;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.Questions
{
    internal sealed class QuestionReader
    { 
        private readonly TestCreationDbContext context;


        public QuestionReader(TestCreationDbContext context)
        {
            this.context = context;            
        }


        public Result<QuestionDTO> GetQuestion(long ownerId, long questionId, bool includeAnswers)
        {
            QuestionDTO question = context.Questions.AsNoTracking().Where(x => x.QuestionId == questionId).Select(QuestionDTO.Mapping).FirstOrDefault();

            if (question == null)
            {
                return Result.NotFound();
            }

            var catalog = context.Questions.Where(x => x.QuestionId == questionId).Join(context.QuestionsCatalogs, 
                                                                                         x => x.CatalogId,
                                                                                         x => x.CatalogId,
                                                                                         (x,y) =>  new { x.OwnerId }).FirstOrDefault();

            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            if (includeAnswers)
            {
                question.Answers = context.Answers.AsNoTracking().Where(x => x.QuestionId == questionId).OrderBy(x => x.AnswerId).Select(AnswerDTO.Mapping).ToList();
            }

            return Result.Ok(question);
        }

        public Result<QuestionHeaderDTO> GetQuestionHeader(long ownerId, long questionId)
        {
            var question = context.Questions.AsNoTracking().Where(x => x.QuestionId == questionId).Select(QuestionHeaderDTO.Mapping).FirstOrDefault();

            if (question == null)
            {
                return Result.NotFound();
            }

            var catalog = context.Questions.Where(x => x.QuestionId == questionId).Join(context.QuestionsCatalogs,
                                                                                        x => x.CatalogId,
                                                                                        x => x.CatalogId,
                                                                                        (x, y) => new { x.OwnerId }).FirstOrDefault();

            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            return Result.Ok(question);
        }

        public Result<List<QuestionHeaderDTO>> GetQuestionsHeaders(long ownerId, long catalogId)
        {
            var catalog = context.QuestionsCatalogs.Where(x => x.CatalogId == catalogId).Select(x => new { x.OwnerId } ).FirstOrDefault();

            if (catalog == null)
            {
                return Result.NotFound();
            }
            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            var questions = context.Questions.AsNoTracking().Where(x => x.CatalogId == catalogId).Select(QuestionHeaderDTO.Mapping).ToList();

            return Result.Ok(questions);
        }
    }
}
