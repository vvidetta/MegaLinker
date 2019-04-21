using System.Linq;
using NUnit.Framework;

namespace MegaLinker.Core.Test
{
    [TestFixture]
    public class LinkerOptionsTests
    {
        [Test]
        public void GivenArgumentListWithoutOOptionCheckThatOutputFileIsAOut()
        {
            var args = new string[] { "AnObject.mob" };

            var linkerOptions = LinkerOptions.Parse(args);

            Assert.AreEqual("a.out", linkerOptions.OutputFile);
        }

        [Test]
        public void GivenArgumentListWithIncompleteOOptionCheckThatIncompleteArgumentExceptionIsThrown()
        {
            var args = new string[] { "AnObject.mob", "-o" };
            Assert.Throws<IncompleteArgumentException>(() => LinkerOptions.Parse(args));
        }

        [Test]
        public void GivenArgumentListWithOOptionCheckThatOutputFileIsAsSet()
        {
            var args = new string[] { "-o", "MyProgram.bin", "AnObject.mob" };
            var linkerOptions = LinkerOptions.Parse(args);
            Assert.AreEqual("MyProgram.bin", linkerOptions.OutputFile);
        }

        [Test]
        public void GivenEmptyArgumentListCheckThatParseThrowsANoInputObjectsException()
        {
            var args = new string[] { };
            Assert.Throws<NoInputObjectsException>(() => LinkerOptions.Parse(args));
        }

        [Test]
        public void GivenAnInputObjectWithIncorrectExtensionCheckThatParseThrowsAnInvalidInputObjectException()
        {
            var args = new string[] { "MyObject.obj" };
            Assert.Throws<InvalidObjectExtensionException>(() => LinkerOptions.Parse(args));
        }

        [Test]
        public void GivenInputObjectsWithValidExtensionsCheckThatParseCopiesThemIntoTheLinkerOptions()
        {
            var args = new string[] { "MyObject1.mob", "MyObject2.mob" };
            var linkerOptions = LinkerOptions.Parse(args);
            Assert.That(linkerOptions.InputObjects.Contains("MyObject1.mob"));
            Assert.That(linkerOptions.InputObjects.Contains("MyObject2.mob"));
        }

        [Test]
        public void GivenDuplicateInputObjectsCheckThatTheLinkerOptionsContainUniqueInputObjects()
        {
            var args = new string[] { "MyObject1.mob", "MyObject1.mob" };
            var linkerOptions = LinkerOptions.Parse(args);
            Assert.That(linkerOptions.InputObjects.Count(x => x == "MyObject1.mob") == 1);
        }
    }
}
