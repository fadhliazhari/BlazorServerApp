using System;
using Xunit;
using OJTTraining.Shared;
using Bunit;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace OJTTrainingTest
{
    public class DropdownDownloadTest : TestContext
    {
        [Fact]
        public void MarkupTest()
        {
            // Act
            var dropdown = RenderComponent<DownloadDropdown>();

            // Expected output
            string expectedHtml = @"
                <div class='dropdown'>
                    <button class='btn btn-secondary dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
		                <span class='oi oi-data-transfer-download' aria-hidden='true'>
			                Download
                        </span>
	                </button>
	                <div class='dropdown-menu'>
		                <a class='dropdown-item' type='button'>
                            <span class='oi oi-spreadsheet' aria-hidden='true'>
                                Excel
                            </span>
                        </a>
		                <a class='dropdown-item' type='button'>
			                <span class='oi oi-document' aria-hidden='true'>
                                CSV
                            </span>
                        </a>
	                </div>
                </div>";

            // Assert
            dropdown.MarkupMatches(expectedHtml);
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

            // Expected output
            string expectedHtml = @"
                <div class='dropdown'>
                    <button class='btn btn-secondary dropdown-toggle' data-toggle='dropdown' aria-expanded='false'>
		                <span class='oi oi-data-transfer-download' aria-hidden='true'>
			                Download
                        </span>
	                </button>
	                <div class='dropdown-menu'>
		                <a class='dropdown-item' type='button'>
			                <span class='oi oi-spreadsheet' aria-hidden='true'>
                                Excel
                            </span>
                        </a>
		                <a class='dropdown-item' type='button'>
			                <span class='oi oi-document' aria-hidden='true'>
                                CSV
                            </span>
                        </a>
	                </div>
                </div>";

            // Assert
            dropdown.MarkupMatches(expectedHtml);
        }

        [Fact]
        public void DropdownButtonTest()
        {
            // Act
            var dropdown = RenderComponent<DownloadDropdown>();

            Assert.False(bool.Parse(dropdown.Find("button").GetAttribute("aria-expanded")),
                "aria-expanded should be false before dropdown click");
            Assert.False(dropdown.FindAll("div")[1].ClassList.Contains("show"),
                "show should not be listed before dropdown click");

            // Click dropdown button
            dropdown.Find("button").Click();

            Assert.True(bool.Parse(dropdown.Find("button").GetAttribute("aria-expanded")),
                "aria-expanded should be true after dropdown click");
            Assert.True(dropdown.FindAll("div")[1].ClassList.Contains("show"),
                "show should be listed after dropdown click");

            // Lose focus dropdown button
            dropdown.Find("button").Blur();
            Thread.Sleep(300);

            Assert.False(bool.Parse(dropdown.Find("button").GetAttribute("aria-expanded")),
                "aria-expanded should be false after lose focus");
            Assert.False(dropdown.FindAll("div")[1].ClassList.Contains("show"),
                "show should not be listed after lose focus");
        }

        [Fact]
        public void NoParamButtonTest()
        {
            var dropdown = RenderComponent<DownloadDropdown>();

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

            var csv = dropdown.FindAll("a")[1];

            csv.Click();

            Assert.Single(plannedInvocation.Invocations);
        }
    }
}
