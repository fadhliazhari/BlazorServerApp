using Bunit;
using OJTTraining.Shared;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OJTTrainingTest
{
    public class DropdownDownloadTest : TestContext
    {
        [Fact]
        public void MarkupTest()
        {
            // Act
            var dropdown = RenderComponent<DownloadDropdown>();

            // Assert
            Assert.True(dropdown.FindAll("div")[0].ClassList.Contains("dropdown"));
            Assert.True(dropdown.FindAll("div")[1].ClassList.Contains("dropdown-menu"));
            Assert.True(dropdown.FindAll("a")[0].ClassList.Contains("dropdown-item"));
            Assert.True(dropdown.FindAll("a")[1].ClassList.Contains("dropdown-item"));
        }

        [Fact]
        public void WithParamMarkupTest()
        {
            // Function Mock
            static async Task<byte[]> ReturnEmpty()
            {
                await Task.Delay(100);
                return Array.Empty<byte>();
            }

            // Act
            var dropdown = RenderComponent<DownloadDropdown>(parameters =>
            {
                parameters
                .Add(parameter => parameter.FileName, "test")
                .Add(parameter => parameter.CsvData, ReturnEmpty)
                .Add(parameter => parameter.ExcelData, ReturnEmpty);
            });

            // Assert
            Assert.True(dropdown.FindAll("div")[0].ClassList.Contains("dropdown"));
            Assert.True(dropdown.FindAll("div")[1].ClassList.Contains("dropdown-menu"));
            Assert.True(dropdown.FindAll("a")[0].ClassList.Contains("dropdown-item"));
            Assert.True(dropdown.FindAll("a")[1].ClassList.Contains("dropdown-item"));
        }

        [Fact]
        public void DropdownButtonTest()
        {
            // Act
            var dropdown = RenderComponent<DownloadDropdown>();

            // Assert before click
            Assert.False(bool.Parse(dropdown.Find("button").GetAttribute("aria-expanded")),
                "aria-expanded should be false before dropdown click");
            Assert.False(dropdown.FindAll("div")[1].ClassList.Contains("show"),
                "show should not be listed before dropdown click");

            // Click dropdown button
            dropdown.Find("button").Click();

            // Assert after click
            Assert.True(bool.Parse(dropdown.Find("button").GetAttribute("aria-expanded")),
                "aria-expanded should be true after dropdown click");
            Assert.True(dropdown.FindAll("div")[1].ClassList.Contains("show"),
                "show should be listed after dropdown click");

            // Lose focus dropdown button
            dropdown.Find("button").Blur();
            Thread.Sleep(300);

            // Assert after lose focus
            Assert.False(bool.Parse(dropdown.Find("button").GetAttribute("aria-expanded")),
                "aria-expanded should be false after lose focus");
            Assert.False(dropdown.FindAll("div")[1].ClassList.Contains("show"),
                "show should not be listed after lose focus");
        }

        [Fact]
        public void NoParamButtonTest()
        {
            var dropdown = RenderComponent<DownloadDropdown>();

            // Check all button function properly
            dropdown.FindAll("a")[0].Click();
            dropdown.FindAll("a")[1].Click();
        }

        [Fact]
        public void ExcelButtonTest()
        {
            // Parameters
            var fileName = "test";
            var testBytes = Encoding.ASCII.GetBytes("1234567890ABCDEFG");

            static Task<byte[]> ReturnDummy()
            {
                return Task.FromResult(Encoding.ASCII.GetBytes("1234567890ABCDEFG"));
            }

            // Act
            var dropdown = RenderComponent<DownloadDropdown>(parameters =>
            {
                parameters
                .Add(parameter => parameter.FileName, fileName)
                .Add(parameter => parameter.ExcelData, ReturnDummy);
            });

            var fullFileName = $"{fileName}_{ DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
            var plannedInvocation = JSInterop.SetupVoid("saveAsFile", fullFileName, Convert.ToBase64String(testBytes));

            dropdown.FindAll("a")[0].Click();

            // Assert
            Assert.Single(plannedInvocation.Invocations);
        }

        [Fact]
        public void CSVButtonTest()
        {
            // Parameters
            var fileName = "test";
            var testBytes = Encoding.ASCII.GetBytes("1234567890ABCDEFG");

            static Task<byte[]> ReturnDummy()
            {
                return Task.FromResult(Encoding.ASCII.GetBytes("1234567890ABCDEFG"));
            }

            // Act
            var dropdown = RenderComponent<DownloadDropdown>(parameters =>
            {
                parameters
                .Add(parameter => parameter.FileName, fileName)
                .Add(parameter => parameter.CsvData, ReturnDummy);
            });

            var fullFileName = $"{fileName}_{ DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
            var plannedInvocation = JSInterop.SetupVoid("saveAsFile", fullFileName, Convert.ToBase64String(testBytes));

            dropdown.FindAll("a")[1].Click();

            // Assert
            Assert.Single(plannedInvocation.Invocations);
        }
    }
}
