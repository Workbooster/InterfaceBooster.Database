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
    public class Writing_And_Reading_Nullable_Values_Works
    {
        [Test]
        public void Test_Writing_And_Reading_String_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            string testValue = @"This test contains some special chars and umlauts: öäüéèàôòõç ;.!_|+-*()[]<>/\'&%#@$°" + "\"";

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            string valueFromReader = PrimitiveSerializer.Read<string>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Boolean_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            bool testValue1 = true;
            bool testValue2 = false;

            PrimitiveSerializer.WriteNullable(ms, testValue1);
            PrimitiveSerializer.WriteNullable(ms, testValue2);

            // reset the stream position
            ms.Position = 0;

            bool value1FromReader = PrimitiveSerializer.Read<bool>(ms, true);
            bool value2FromReader = PrimitiveSerializer.Read<bool>(ms, true);

            Assert.AreEqual(testValue1, value1FromReader);
            Assert.AreEqual(testValue2, value2FromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Integer_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            int testValue = 2147483647;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            int valueFromReader = PrimitiveSerializer.Read<int>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Float_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            float testValue = 3.4e38f;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            float valueFromReader = PrimitiveSerializer.Read<float>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Double_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            double testValue = 1.79769313486231e308;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            double valueFromReader = PrimitiveSerializer.Read<double>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Decimal_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            decimal testValue = 7.9e28M;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            decimal valueFromReader = PrimitiveSerializer.Read<decimal>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_DateTime_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            DateTime testValue = new DateTime(1987, 12, 15, 23, 15, 0);

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            DateTime valueFromReader = PrimitiveSerializer.Read<DateTime>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Byte_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            byte testValue = 255;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            byte valueFromReader = PrimitiveSerializer.Read<byte>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Sbyte_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            sbyte testValue = -128;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            sbyte valueFromReader = PrimitiveSerializer.Read<sbyte>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Char_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            char testValue1 = 'R';
            char testValue2 = 'ä';

            PrimitiveSerializer.WriteNullable(ms, testValue1);
            PrimitiveSerializer.WriteNullable(ms, testValue2);

            // reset the stream position
            ms.Position = 0;

            char value1FromReader = PrimitiveSerializer.Read<char>(ms, true);
            char value2FromReader = PrimitiveSerializer.Read<char>(ms, true);

            Assert.AreEqual(testValue1, value1FromReader);
            Assert.AreEqual(testValue2, value2FromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Ushort_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            ushort testValue = 65535;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            ushort valueFromReader = PrimitiveSerializer.Read<ushort>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Short_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            short testValue = -32768;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            short valueFromReader = PrimitiveSerializer.Read<short>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Uint_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            uint testValue = 4294967295;

            PrimitiveSerializer.WriteNullable(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            uint valueFromReader = PrimitiveSerializer.Read<uint>(ms, true);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Multiple_Values_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            string stringValue = "Test Value!";
            bool boolValue = true;
            int intValue = 612;
            float floatValue = 17.346f;
            double doubleValue = 255.8946;
            DateTime datetimeValue = new DateTime(2014, 02, 04);
            byte byteValue = 123;
            sbyte sbyteValue = 12;
            char charValue = 'C';
            ushort ushortValue = 7598;
            short shortValue = 9412;
            uint uintValue = 147523;

            PrimitiveSerializer.WriteNullable(ms, stringValue);
            PrimitiveSerializer.WriteNullable(ms, boolValue);
            PrimitiveSerializer.WriteNullable(ms, intValue);
            PrimitiveSerializer.WriteNullable(ms, floatValue);
            PrimitiveSerializer.WriteNullable(ms, doubleValue);
            PrimitiveSerializer.WriteNullable(ms, datetimeValue);
            PrimitiveSerializer.WriteNullable(ms, byteValue);
            PrimitiveSerializer.WriteNullable(ms, sbyteValue);
            PrimitiveSerializer.WriteNullable(ms, charValue);
            PrimitiveSerializer.WriteNullable(ms, ushortValue);
            PrimitiveSerializer.WriteNullable(ms, shortValue);
            PrimitiveSerializer.WriteNullable(ms, uintValue);

            // reset the stream position
            ms.Position = 0;

            string stringValueFromReader = PrimitiveSerializer.Read<string>(ms, true);
            bool boolValueFromReader = PrimitiveSerializer.Read<bool>(ms, true);
            int intValueFromReader = PrimitiveSerializer.Read<int>(ms, true);
            float floatValueFromReader = PrimitiveSerializer.Read<float>(ms, true);
            double doubleValueFromReader = PrimitiveSerializer.Read<double>(ms, true);
            DateTime datetimeValueFromReader = PrimitiveSerializer.Read<DateTime>(ms, true);
            byte byteValueFromReader = PrimitiveSerializer.Read<byte>(ms, true);
            sbyte sbyteValueFromReader = PrimitiveSerializer.Read<sbyte>(ms, true);
            char charValueFromReader = PrimitiveSerializer.Read<char>(ms, true);
            ushort ushortValueFromReader = PrimitiveSerializer.Read<ushort>(ms, true);
            short shortValueFromReader = PrimitiveSerializer.Read<short>(ms, true);
            uint uintValueFromReader = PrimitiveSerializer.Read<uint>(ms, true);

            Assert.AreEqual(stringValue, stringValueFromReader);
            Assert.AreEqual(boolValue, boolValueFromReader);
            Assert.AreEqual(intValue, intValueFromReader);
            Assert.AreEqual(floatValue, floatValueFromReader);
            Assert.AreEqual(doubleValue, doubleValueFromReader);
            Assert.AreEqual(datetimeValue, datetimeValueFromReader);
            Assert.AreEqual(byteValue, byteValueFromReader);
            Assert.AreEqual(sbyteValue, sbyteValueFromReader);
            Assert.AreEqual(charValue, charValueFromReader);
            Assert.AreEqual(ushortValue, ushortValueFromReader);
            Assert.AreEqual(shortValue, shortValueFromReader);
            Assert.AreEqual(uintValue, uintValueFromReader);
        }
    }
}
