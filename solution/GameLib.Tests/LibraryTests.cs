using Xunit.Abstractions;

namespace GameLib.DAL.Tests;

public class LibraryTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_Game_Persisted()
    {

    }
}