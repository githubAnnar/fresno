using System;
using Xunit;

namespace LanterneRouge.Fresno.Database.SQLite.Types
{
    public class TypesTest
    {
        [Fact]
        public void LiteralValueTest()
        {
            var testObject = new LiteralValue
            {
                StringLiteral = "string literal"
            };
            var actual = testObject.GetLiteralValue();
            Assert.Equal("string literal", actual);

            testObject = new LiteralValue
            {
                NumericLiteral = 9999
            };
            actual = testObject.GetLiteralValue();
            Assert.Equal("9999", actual);

            testObject = new LiteralValue
            {
                Value = LiteralValueEnum.NULL
            };
            actual = testObject.GetLiteralValue();
            Assert.Equal("NULL", actual);

            testObject = new LiteralValue
            {
                Value = LiteralValueEnum.NULL,
                StringLiteral = "this will fail"
            };
            Assert.Throws<ArgumentException>(testObject.GetLiteralValue);
        }
    }
}
