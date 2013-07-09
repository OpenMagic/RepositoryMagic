using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepositoryMagic.Tests.TestHelpers;

namespace RepositoryMagic.Tests
{
    [TestClass]
    public abstract class IRepositoryTests : TestBase
    {
        protected abstract IRepository<TestModel, int> NewRepository();

        protected virtual IRepository<IRepositoryTests.TestModel, int> NewRepository(IEnumerable<TestModel> initialItems)
        {
            var repo = this.NewRepository();

            foreach (var item in initialItems)
            {
                repo.Insert(item);
            }

            return repo;
        }

        protected virtual IRepository<TestModel, int> CleanRepository()
        {
            var repo = this.NewRepository();

            foreach (var item in repo.Get().ToArray())
            {
                repo.Delete(item.Id);
            }

            return repo;
        }

        protected virtual IRepository<IRepositoryTests.TestModel, int> CleanRepository(IEnumerable<TestModel> initialItems)
        {
            this.CleanRepository();

            return this.NewRepository(initialItems);
        }

        private IEnumerable<TestModel> TestModels(int count)
        {
            var items = new List<TestModel>();

            for (int i = 0; i < count; i++)
            {
                items.Add(new TestModel() { Id = i + 1 });
            }

            return items;
        }

        [TestMethod]
        public void Delete_ShouldThrow_ItemNotFoundException_When_id_DoesNotExist()
        {
            // Given
            var repo = this.CleanRepository();

            // When
            Action action = () => repo.Delete(0);

            // Then
            action.ShouldThrow<ItemNotFoundException>().WithMessage("TestModels/0 cannot be deleted because it does not exist.");
        }

        [TestMethod]
        public void Delete_ShouldDeleteItemEqualTo_id()
        {
            // Given
            var testModels = this.TestModels(3);
            var repo = this.CleanRepository(testModels);

            // When
            repo.Delete(2);

            // Then
            repo.Get().Should().Equal(
                from testModel in testModels
                where testModel.Id != 2
                select testModel
            );
        }

        [TestMethod]
        public void Exist_ShouldReturnTrue_When_id_DoesExist()
        {
            // Given
            var repo = this.CleanRepository(this.TestModels(3));

            // When
            var exists = repo.Exists(2);

            // Then            
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void Exists_ShouldReturnFalse_When_id_DoesNotExist()
        {
            // Given
            var repo = this.CleanRepository(this.TestModels(3));

            // When
            var exists = repo.Exists(29);

            // Then            
            exists.Should().BeFalse();
        }

        [TestMethod]
        public void Find_ShouldReturnModel_When_id_DoesExist()
        {
            // Given
            var items = this.TestModels(3);
            var repo = this.CleanRepository(items);

            // When
            var model = repo.Find(2);

            // Then            
            model.Should().BeSameAs(items.ElementAt(1));
        }

        [TestMethod]
        public void Find_ShouldReturnNull_When_id_DoesNotExist()
        {
            // Given
            var repo = this.CleanRepository(this.TestModels(3));

            // When
            var model = repo.Find(29);

            // Then            
            model.Should().BeNull();
        }

        [TestMethod]
        public void Get_ShouldReturnModel_When_id_DoesExist()
        {
            // Given
            var items = this.TestModels(3);
            var repo = this.CleanRepository(items);

            // When
            var model = repo.Get(2);

            // Then            
            model.Should().BeSameAs(items.ElementAt(1));
        }

        [TestMethod]
        public void Get_ShouldThrow_ItemNotFoundException_When_id_DoesNotExist()
        {
            // Given
            var repo = this.CleanRepository(this.TestModels(3));

            // When
            Action action = () => repo.Get(29);

            // Then            
            action.ShouldThrow<ItemNotFoundException>().WithMessage("TestModels/29 does not exist.");
        }

        [TestMethod]
        public void Get_ShouldReturnAllModels()
        {
            // Given
            var items = this.TestModels(3);
            var repo = this.CleanRepository(items);

            // When
            var models = repo.Get();

            // Then            
            models.Should().BeEquivalentTo(items);
        }

        [TestMethod]
        public void Insert_ShouldThrow_ArgumentNullException_When_model_IsNull()
        {
            // Given
            var repo = this.CleanRepository();

            // When
            Action action = () => repo.Insert(null);

            // Then    
            this.ShouldThrowArgumentNullException(action, "model");
        }

        [TestMethod]
        public void Insert_ShouldThrow_DuplicateItemException_When_model_AlreadyExists()
        {
            // Given
            var items = this.TestModels(3);
            var repo = this.CleanRepository(items);

            // When
            Action action = () => repo.Insert(items.ElementAt(1));

            // Then    
            action.ShouldThrow<DuplicateItemException>().WithMessage("TestModels/2 cannot be inserted because it already exists.");
        }

        [TestMethod]
        public void Insert_ShouldAdd_model()
        {
            // Given
            var model = this.TestModels(1).Single();
            var repo = this.CleanRepository();

            // When
            repo.Insert(model);

            // Then    
            repo.Get().Single().Should().BeSameAs(model);
        }

        [TestMethod]
        public void Update_ShouldThrow_ArgumentNullException_When_model_IsNull()
        {
            // Given
            var repo = this.CleanRepository();

            // When
            Action action = () => repo.Update(null);

            // Then    
            this.ShouldThrowArgumentNullException(action, "model");
        }

        [TestMethod]
        public void Update_ShouldUpdateRepositoryWith_model()
        {
            // Given
            var models = this.TestModels(1);
            var model = models.Single();
            var repo = this.CleanRepository(models);

            model.Name = "fred";

            // When
            repo.Update(model);

            // Then    
            this.NewRepository().Get().Single().Should().Be(model);
        }

        public class TestModel : IModel<int>
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
