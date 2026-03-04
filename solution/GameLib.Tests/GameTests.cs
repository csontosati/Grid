using GameLib.DAL.Entities;
using GameLib.DAL.Factories;

namespace GameLib.DAL.Tests
{
    public class GameTests : DbContextTestsBase
    {
        [Fact]
        public async Task AddNew_Game_Persisted()
        {
            
            //Arrange
            var entity = new GameEntity
            {
                Name = $"{nameof(GameEntity.Name)}",
                Description = $"{nameof(GameEntity.Description)}",
                Category = null,
                ImageUrl = null
            };

            //Act
            GameLibDbContextSut.Games.Add(entity);
            await GameLibDbContextSut.SaveChangesAsync();

            //Assert
            using var dbx =  base.DbContextFactory.CreateDbContext();
            var entityfromDb = dbx.Games.First(gameEntity => gameEntity.Id == entity.Id);
            Assert.Equal(entity, entityfromDb);

        }
    }
}
