using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal sealed class TestsCatalog : Catalog
    {
        private readonly List<Test> _tests = new List<Test>();

        public IReadOnlyList<Test> Tests
        {
            get => _tests;
        }


        private TestsCatalog(string name, long ownerId) : base(name, ownerId)
        {
        }


        public static TestsCatalog Create(string name, long ownerId)
        {
            return new TestsCatalog(name, ownerId);
        }

        public override void Delete()
        {
            base.Delete();
            foreach (Test test in Tests)
            {
                test.Delete();
            }
        }

        public void AddTest(Test test)
        {
            _tests.Add(test);
        }
    }
}
