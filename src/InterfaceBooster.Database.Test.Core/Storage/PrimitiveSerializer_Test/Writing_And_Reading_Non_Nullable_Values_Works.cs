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
    public class Writing_And_Reading_Non_Nullable_Values_Works
    {
        [Test]
        public void Test_Writing_And_Reading_String_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            string testValue = @"This test contains some special chars and umlauts: öäüéèàôòõç ;.!_|+-*()[]<>/\'&%#@$°" + "\"";

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            string valueFromReader = PrimitiveSerializer.Read<string>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Boolean_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            bool testValue1 = true;
            bool testValue2 = false;

            PrimitiveSerializer.Write(ms, testValue1);
            PrimitiveSerializer.Write(ms, testValue2);

            // reset the stream position
            ms.Position = 0;

            bool value1FromReader = PrimitiveSerializer.Read<bool>(ms);
            bool value2FromReader = PrimitiveSerializer.Read<bool>(ms);

            Assert.AreEqual(testValue1, value1FromReader);
            Assert.AreEqual(testValue2, value2FromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Integer_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            int testValue = 2147483647;

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            int valueFromReader = PrimitiveSerializer.Read<int>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Float_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            float testValue = 3.4e38f;

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            float valueFromReader = PrimitiveSerializer.Read<float>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Double_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            double testValue = 1.79769313486231e308;

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            double valueFromReader = PrimitiveSerializer.Read<double>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Decimal_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            decimal testValue = 7.9e28M;

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            decimal valueFromReader = PrimitiveSerializer.Read<decimal>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_DateTime_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            DateTime testValue = new DateTime(1987, 12, 15, 23, 15, 0);

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            DateTime valueFromReader = PrimitiveSerializer.Read<DateTime>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Byte_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            byte testValue = 255;

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            byte valueFromReader = PrimitiveSerializer.Read<byte>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Sbyte_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            sbyte testValue = -128;

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            sbyte valueFromReader = PrimitiveSerializer.Read<sbyte>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Char_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            char testValue1 = 'R';
            char testValue2 = 'ä';

            PrimitiveSerializer.Write(ms, testValue1);
            PrimitiveSerializer.Write(ms, testValue2);

            // reset the stream position
            ms.Position = 0;

            char value1FromReader = PrimitiveSerializer.Read<char>(ms);
            char value2FromReader = PrimitiveSerializer.Read<char>(ms);

            Assert.AreEqual(testValue1, value1FromReader);
            Assert.AreEqual(testValue2, value2FromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Ushort_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            ushort testValue = 65535;

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            ushort valueFromReader = PrimitiveSerializer.Read<ushort>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Short_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            short testValue = -32768;

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            short valueFromReader = PrimitiveSerializer.Read<short>(ms);

            Assert.AreEqual(testValue, valueFromReader);
        }

        [Test]
        public void Test_Writing_And_Reading_Uint_By_Using_A_Stream_Works()
        {
            MemoryStream ms = new MemoryStream();

            uint testValue = 4294967295;

            PrimitiveSerializer.Write(ms, testValue);

            // reset the stream position
            ms.Position = 0;

            uint valueFromReader = PrimitiveSerializer.Read<uint>(ms);

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

            PrimitiveSerializer.Write(ms, stringValue);
            PrimitiveSerializer.Write(ms, boolValue);
            PrimitiveSerializer.Write(ms, intValue);
            PrimitiveSerializer.Write(ms, floatValue);
            PrimitiveSerializer.Write(ms, doubleValue);
            PrimitiveSerializer.Write(ms, datetimeValue);
            PrimitiveSerializer.Write(ms, byteValue);
            PrimitiveSerializer.Write(ms, sbyteValue);
            PrimitiveSerializer.Write(ms, charValue);
            PrimitiveSerializer.Write(ms, ushortValue);
            PrimitiveSerializer.Write(ms, shortValue);
            PrimitiveSerializer.Write(ms, uintValue);

            // reset the stream position
            ms.Position = 0;

            string stringValueFromReader = PrimitiveSerializer.Read<string>(ms);
            bool boolValueFromReader = PrimitiveSerializer.Read<bool>(ms);
            int intValueFromReader = PrimitiveSerializer.Read<int>(ms);
            float floatValueFromReader = PrimitiveSerializer.Read<float>(ms);
            double doubleValueFromReader = PrimitiveSerializer.Read<double>(ms);
            DateTime datetimeValueFromReader = PrimitiveSerializer.Read<DateTime>(ms);
            byte byteValueFromReader = PrimitiveSerializer.Read<byte>(ms);
            sbyte sbyteValueFromReader = PrimitiveSerializer.Read<sbyte>(ms);
            char charValueFromReader = PrimitiveSerializer.Read<char>(ms);
            ushort ushortValueFromReader = PrimitiveSerializer.Read<ushort>(ms);
            short shortValueFromReader = PrimitiveSerializer.Read<short>(ms);
            uint uintValueFromReader = PrimitiveSerializer.Read<uint>(ms);

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
