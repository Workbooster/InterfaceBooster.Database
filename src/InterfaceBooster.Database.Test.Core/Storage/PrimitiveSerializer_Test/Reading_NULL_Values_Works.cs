using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Core.Storage;

namespace InterfaceBooster.Database.Test.Core.Storage.PrimitiveSerializer_Test
{
    [TestFixture]
    public class Reading_NULL_Values_Works
    {
        [Test]
        public void Test_Reading_NULL_String_Works()
        {
            MemoryStream ms = new MemoryStream();

            string testValue = null;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            object valueFromReader = PrimitiveSerializer.Read(ms, typeof(string), true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Reading_NULL_Bool_Works()
        {
            MemoryStream ms = new MemoryStream();

            object testValue = null;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            object valueFromReader = PrimitiveSerializer.Read(ms, typeof(bool), true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Reading_NULL_Int_Works()
        {
            MemoryStream ms = new MemoryStream();

            object testValue = null;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            object valueFromReader = PrimitiveSerializer.Read(ms, typeof(int), true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Reading_NULL_Decimal_Works()
        {
            MemoryStream ms = new MemoryStream();

            object testValue = null;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            object valueFromReader = PrimitiveSerializer.Read(ms, typeof(decimal), true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Reading_NULL_Double_Works()
        {
            MemoryStream ms = new MemoryStream();

            object testValue = null;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            object valueFromReader = PrimitiveSerializer.Read(ms, typeof(double), true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Reading_NULL_Char_Works()
        {
            MemoryStream ms = new MemoryStream();

            object testValue = null;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            object valueFromReader = PrimitiveSerializer.Read(ms, typeof(char), true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Reading_NULL_DateTime_Works()
        {
            MemoryStream ms = new MemoryStream();

            object testValue = null;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            object valueFromReader = PrimitiveSerializer.Read(ms, typeof(DateTime), true);

            Assert.AreEqual(testValue, valueFromReader);
        }
    }
}
