using System.Runtime.CompilerServices;
using TestMe.TestCreation.Domain;
using TestMe.TestCreation.Persistence;

[assembly: InternalsVisibleTo("TestMe.TestCreation.Tests")]
[assembly: InternalsVisibleTo("TestMe.Presentation.API.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]      // needed by Moq
namespace TestMe.TestCreation
{
    internal static class TestUtils
    {
        public const long OwnerId = 1;
        public const long OtherOwnerId = 2;

        public const long ValidTestsCatalogId = 5;
        public const long ValidQuestionsCatalogId = 1;
        public const long DeletedTestsCatalogId = 4;
        public const long NotExisitngTestsCatalogId = -1;
        public const long OtherOwnerTestsCatalogId = 9;
        public const long DeletedQuestionsCatalogId = 4;
        public const long NotExisitngQuestionsCatalogId = -1;
        public const long OtherUserQuestionsCatalogId = 8;

        public const long ValidQuestionId = 1;
        public const long DeletedQuestionId = 5;
        public const long NotExisitngQuestionId = -1;
        public const long OtherUserQuestionId = 8;

        public const long ValidTestId = 1;
        public const long DeletedTestId = 4;
        public const long NotExisitngTestId = -1;
        public const long OtherUserTestId = 3;

        public const long ValidTestItemId = 1;
        public const long NotExisitngTestItemId = -1;
        public const long OtherTestItemId = 4;

        public static ITestCreationUoW CreateTestCreationUoW(TestCreationDbContext context)
        {
            return new TestCreationUoW(context);
        }

        public static void Seed(TestCreationDbContext context)
        {          
            var owner1 = Owner.Create(1);
            var owner2 = Owner.Create(2);
            var owner7 = Owner.Create(7);
            context.Owners.Add(owner1);
            context.Owners.Add(owner2);
            context.Owners.Add(owner7);

            context.SaveChanges();

            var policy1 = AddQuestionsCatalogPolicyFactory.Create(MembershipLevel.Regular);

            QuestionsCatalog catalogA = owner1.AddQuestionsCatalog("Owner 1, QuestionsCatalog A", policy1);
            QuestionsCatalog catalogB = owner1.AddQuestionsCatalog("Owner 1, QuestionsCatalog B", policy1);
            QuestionsCatalog catalogC = owner1.AddQuestionsCatalog("Owner 1, QuestionsCatalog C", policy1);
            QuestionsCatalog catalogD = owner1.AddQuestionsCatalog("Owner 1, QuestionsCatalog D, deleted", policy1);
            catalogD.Delete();
            TestsCatalog catalogE = owner1.AddTestsCatalog("Owner 1, TestsCatalog E");
            TestsCatalog catalogF = owner1.AddTestsCatalog("Owner 1, TestsCatalog F");
            TestsCatalog catalogG = owner1.AddTestsCatalog("Owner 1, TestsCatalog G, deleted");
            catalogG.Delete();
            QuestionsCatalog catalogH = owner2.AddQuestionsCatalog("Owner 2, QuestionsCatalog H", policy1);
            TestsCatalog catalogI = owner2.AddTestsCatalog("Owner 2, TestsCatalog I");

            context.SaveChanges();

            var policy = AddQuestionPolicyFactory.Create(MembershipLevel.Regular);

            var q1 = Question.Create("Owner 1, Catalog A, Question 1", owner1.OwnerId);
            q1.AddAnswer("Q1 A1", false);           
            catalogA.AddQuestion(q1, policy);

            context.SaveChanges();

            var q2 = Question.Create("Owner 1, Catalog A, Question 2", owner1.OwnerId);
            q2.AddAnswer("Q2 A1", false);
            q2.AddAnswer("Q2 A2", true);        
            catalogA.AddQuestion(q2, policy);

            context.SaveChanges();

            var q3 = Question.Create("Owner 1, Catalog A, Question 3", owner1.OwnerId);
            q3.AddAnswer("Q3 A1", false);
            q3.AddAnswer("Q3 A2", true);
            q3.AddAnswer("Q3 A3", false);
            catalogA.AddQuestion(q3, policy);

            context.SaveChanges();

            var q4 = Question.Create("Owner 1, Catalog B, Question 4", owner1.OwnerId);
            q4.AddAnswer("Q4 A1", false);
            q4.AddAnswer("Q4 A2", true);
            q4.AddAnswer("Q4 A3", false);
            q4.AddAnswer("Q4 A4", true);
            catalogB.AddQuestion(q4, policy);

            var q5 = Question.Create("Owner 1, Catalog B, Question 5, deleted", owner1.OwnerId);
            q5.AddAnswer("Q5 A1", false);
            q5.AddAnswer("Q5 A2", true);
            q5.AddAnswer("Q5 A3", false);
            q5.AddAnswer("Q5 A4", true);
            q5.AddAnswer("Q5 A5", false);
            q5.Delete();
            catalogB.AddQuestion(q5, policy);

            var q6 = Question.Create("Owner 1, Catalog C, Question 6", owner1.OwnerId);
            q6.AddAnswer("Q6 A1", false);
            q6.AddAnswer("Q6 A2", true);
            q6.AddAnswer("Q6 A3", false);
            q6.AddAnswer("Q6 A4", true);
            q6.AddAnswer("Q6 A5", false);
            q6.AddAnswer("Q6 A6", true);
            catalogC.AddQuestion(q6, policy);

            var q7 = Question.Create("Owner 1, Catalog D deleted, Question 7", owner1.OwnerId);
            q7.AddAnswer("Q7 A1", false);
            q7.AddAnswer("Q7 A2", true);
            catalogD.AddQuestion(q7, policy);

            var q8 = Question.Create("Owner 2, Catalog H, Question 8", owner2.OwnerId);
            q8.AddAnswer("Q8 A1", false);           
            catalogH.AddQuestion(q8, policy);

            context.SaveChanges();

            var t1 = Test.Create(owner1.OwnerId, "Owner 1, catalog E, Test 1");
            catalogE.AddTest(t1);

            t1.AddQuestion(q1);
            t1.AddQuestion(q3);
            t1.AddQuestion(q5);

            var t2 = Test.Create(owner1.OwnerId, "Owner 1, catalog F, Test 2");
            catalogF.AddTest(t2);
            t2.AddQuestion(q2);
            t2.AddQuestion(q4);
            t2.AddQuestion(q6);
           

            var t3 = Test.Create(owner2.OwnerId, "Owner 2, catalog I, Test 3");
            catalogI.AddTest(t3);
            t3.AddQuestion(q7);
            t3.AddQuestion(q8);


            var t4 = Test.Create(owner1.OwnerId, "Owner 1, catalog F, Test 4");
            catalogF.AddTest(t2);
            t4.Delete();

            context.SaveChanges();
        }
    }
}
