using FluentSiren.AspNetCore.Mvc.Formatters;
using FluentSiren.Builders;
using Microsoft.AspNetCore.Mvc.Formatters;
using Xunit;

namespace FluentSiren.AspNetCore.Mvc.Tests.Unit.Formatters
{
    public class SirenOutputFormatterTests
    {
        public SirenOutputFormatterTests()
        {
            _sirenOutputFormatter = new SirenOutputFormatter();
        }

        private SirenOutputFormatter _sirenOutputFormatter;

        [Fact]
        public void it_can_write_to_stream()
        {
            var entity = new EntityBuilder().Build();

        }

        [Fact]
        public void it_cant_write_object_that_is_not_entity()
        {
        }
    }
}