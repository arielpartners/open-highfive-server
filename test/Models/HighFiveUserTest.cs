using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace highfive_server.Models
{
    public class Tests
    {
        private DbContextOptions<HighFiveContext> _contextOptions;

        [Fact]
        public void Test1() 
        {
            Assert.True(true);
        }
    }
}
