using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using OJTTraining.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OJTTrainingTest
{
    public class PaginationTest : TestContext
    {
        // title, skip, top, viewcount, totalcount, expectedpageitemcount, disablestatus, activestatus, displaytext
        private readonly object[][] pagePattern = new object[][] {
            new object[]{ "No Data",    0, 0, 0, 0, 3,  new int[]{ 1, 0, 1 },                           new int[]{ 0, 1, 0 },                           new string[]{ "Previous", "1", "Next" } },
            new object[]{ "1 Page",     0, 1, 1, 1, 3,  new int[]{ 1, 0, 1 },                           new int[]{ 0, 1, 0 },                           new string[]{ "Previous", "1", "Next" } },
            new object[]{ "2 Page 1/2", 0, 1, 1, 2, 4,  new int[]{ 1, 0, 0, 0 },                        new int[]{ 0, 1, 0, 0 },                        new string[]{ "Previous", "1", "2", "Next" } },
            new object[]{ "2 Page 2/2", 1, 1, 1, 2, 4,  new int[]{ 0, 0, 0, 1 },                        new int[]{ 0, 0, 1, 0 },                        new string[]{ "Previous", "1", "2", "Next" } },
            new object[]{ "3 Page 1/3", 0, 1, 1, 3, 5,  new int[]{ 1, 0, 0, 0, 0 },                     new int[]{ 0, 1, 0, 0, 0 },                     new string[]{ "Previous", "1", "2", "3", "Next" } },
            new object[]{ "3 Page 2/3", 1, 1, 1, 3, 5,  new int[]{ 0, 0, 0, 0, 0 },                     new int[]{ 0, 0, 1, 0, 0 },                     new string[]{ "Previous", "1", "2", "3", "Next" } },
            new object[]{ "3 Page 3/3", 2, 1, 1, 3, 5,  new int[]{ 0, 0, 0, 0, 1 },                     new int[]{ 0, 0, 0, 1, 0 },                     new string[]{ "Previous", "1", "2", "3", "Next" } },
            new object[]{ "4 Page 1/4", 0, 1, 1, 4, 6,  new int[]{ 1, 0, 0, 0, 0, 0 },                  new int[]{ 0, 1, 0, 0, 0, 0 },                  new string[]{ "Previous", "1", "2", "3", "4", "Next" } },
            new object[]{ "4 Page 2/4", 1, 1, 1, 4, 6,  new int[]{ 0, 0, 0, 0, 0, 0 },                  new int[]{ 0, 0, 1, 0, 0, 0 },                  new string[]{ "Previous", "1", "2", "3", "4", "Next" } },
            new object[]{ "4 Page 3/4", 2, 1, 1, 4, 6,  new int[]{ 0, 0, 0, 0, 0, 0 },                  new int[]{ 0, 0, 0, 1, 0, 0 },                  new string[]{ "Previous", "1", "2", "3", "4", "Next" } },
            new object[]{ "4 Page 4/4", 3, 1, 1, 4, 6,  new int[]{ 0, 0, 0, 0, 0, 1 },                  new int[]{ 0, 0, 0, 0, 1, 0 },                  new string[]{ "Previous", "1", "2", "3", "4", "Next" } },
            new object[]{ "5 Page 1/5", 0, 1, 1, 5, 7,  new int[]{ 1, 0, 0, 0, 0, 0, 0 },               new int[]{ 0, 1, 0, 0, 0, 0, 0 },               new string[]{ "Previous", "1", "2", "3", "...", "5", "Next" } },
            new object[]{ "5 Page 2/5", 1, 1, 1, 5, 7,  new int[]{ 0, 0, 0, 0, 0, 0, 0 },               new int[]{ 0, 0, 1, 0, 0, 0, 0 },               new string[]{ "Previous", "1", "2", "3", "4", "5", "Next" } },
            new object[]{ "5 Page 3/5", 2, 1, 1, 5, 7,  new int[]{ 0, 0, 0, 0, 0, 0, 0 },               new int[]{ 0, 0, 0, 1, 0, 0, 0 },               new string[]{ "Previous", "1", "2", "3", "4", "5", "Next" } },
            new object[]{ "5 Page 4/5", 3, 1, 1, 5, 7,  new int[]{ 0, 0, 0, 0, 0, 0, 0 },               new int[]{ 0, 0, 0, 0, 1, 0, 0 },               new string[]{ "Previous", "1", "2", "3", "4", "5", "Next" } },
            new object[]{ "5 Page 5/5", 4, 1, 1, 5, 7,  new int[]{ 0, 0, 0, 0, 0, 0, 1 },               new int[]{ 0, 0, 0, 0, 0, 1, 0 },               new string[]{ "Previous", "1", "...", "3", "4", "5", "Next" } },
            new object[]{ "6 Page 1/6", 0, 1, 1, 6, 7,  new int[]{ 1, 0, 0, 0, 0, 0, 0},                new int[]{ 0, 1, 0, 0, 0, 0, 0 },               new string[]{ "Previous", "1", "2", "3", "...", "6", "Next" } },
            new object[]{ "6 Page 2/6", 1, 1, 1, 6, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 1, 0, 0, 0, 0, 0 },            new string[]{ "Previous", "1", "2", "3", "4", "...", "6", "Next" } },
            new object[]{ "6 Page 3/6", 2, 1, 1, 6, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 0, 1, 0, 0, 0, 0 },            new string[]{ "Previous", "1", "2", "3", "4", "5", "6", "Next" } },
            new object[]{ "6 Page 4/6", 3, 1, 1, 6, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 0, 0, 1, 0, 0, 0 },            new string[]{ "Previous", "1", "2", "3", "4", "5", "6", "Next" } },
            new object[]{ "6 Page 5/6", 4, 1, 1, 6, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 0, 0, 0, 1, 0, 0 },            new string[]{ "Previous", "1", "...", "3", "4", "5", "6", "Next" } },
            new object[]{ "6 Page 6/6", 5, 1, 1, 6, 7,  new int[]{ 0, 0, 0, 0, 0, 0, 1 },               new int[]{ 0, 0, 0, 0, 0, 1, 0 },               new string[]{ "Previous", "1", "...", "4", "5", "6", "Next" } },
            new object[]{ "7 Page 1/7", 0, 1, 1, 7, 7,  new int[]{ 1, 0, 0, 0, 0, 0, 0},                new int[]{ 0, 1, 0, 0, 0, 0, 0 },               new string[]{ "Previous", "1", "2", "3", "...", "7", "Next" } },
            new object[]{ "7 Page 2/7", 1, 1, 1, 7, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 1, 0, 0, 0, 0, 0 },            new string[]{ "Previous", "1", "2", "3", "4", "...", "7", "Next" } },
            new object[]{ "7 Page 3/7", 2, 1, 1, 7, 9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },         new int[]{ 0, 0, 0, 1, 0, 0, 0, 0, 0 },         new string[]{ "Previous", "1", "2", "3", "4", "5", "...", "7", "Next" } },
            new object[]{ "7 Page 4/7", 3, 1, 1, 7, 9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },         new int[]{ 0, 0, 0, 0, 1, 0, 0, 0, 0 },         new string[]{ "Previous", "1", "2", "3", "4", "5", "6", "7", "Next" } },
            new object[]{ "7 Page 5/7", 4, 1, 1, 7, 9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },         new int[]{ 0, 0, 0, 0, 0, 1, 0, 0, 0 },         new string[]{ "Previous", "1", "...", "3", "4", "5", "6", "7", "Next" } },
            new object[]{ "7 Page 6/7", 5, 1, 1, 7, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 0, 0, 0, 1, 0, 0 },            new string[]{ "Previous", "1", "...", "4", "5", "6", "7", "Next" } },
            new object[]{ "7 Page 7/7", 6, 1, 1, 7, 7,  new int[]{ 0, 0, 0, 0, 0, 0, 1 },               new int[]{ 0, 0, 0, 0, 0, 1, 0 },               new string[]{ "Previous", "1", "...", "5", "6", "7", "Next" } },
            new object[]{ "8 Page 1/8", 0, 1, 1, 8, 7,  new int[]{ 1, 0, 0, 0, 0, 0, 0},                new int[]{ 0, 1, 0, 0, 0, 0, 0 },               new string[]{ "Previous", "1", "2", "3", "...", "8", "Next" } },
            new object[]{ "8 Page 2/8", 1, 1, 1, 8, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 1, 0, 0, 0, 0, 0 },            new string[]{ "Previous", "1", "2", "3", "4", "...", "8", "Next" } },
            new object[]{ "8 Page 3/8", 2, 1, 1, 8, 9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },         new int[]{ 0, 0, 0, 1, 0, 0, 0, 0, 0 },         new string[]{ "Previous", "1", "2", "3", "4", "5", "...", "8", "Next" } },
            new object[]{ "8 Page 4/8", 3, 1, 1, 8, 10, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },      new int[]{ 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },      new string[]{ "Previous", "1", "2", "3", "4", "5", "6", "...", "8", "Next" } },
            new object[]{ "8 Page 5/8", 4, 1, 1, 8, 10, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },      new int[]{ 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },      new string[]{ "Previous", "1", "...", "3", "4", "5", "6", "7", "8", "Next" } },
            new object[]{ "8 Page 6/8", 5, 1, 1, 8, 9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },         new int[]{ 0, 0, 0, 0, 0, 1, 0, 0, 0 },         new string[]{ "Previous", "1", "...", "4", "5", "6", "7", "8", "Next" } },
            new object[]{ "8 Page 7/8", 6, 1, 1, 8, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 0, 0, 0, 1, 0, 0 },            new string[]{ "Previous", "1", "...", "5", "6", "7", "8", "Next" } },
            new object[]{ "8 Page 8/8", 7, 1, 1, 8, 7,  new int[]{ 0, 0, 0, 0, 0, 0, 1 },               new int[]{ 0, 0, 0, 0, 0, 1, 0 },               new string[]{ "Previous", "1", "...", "6", "7", "8", "Next" } },
            new object[]{ "9 Page 1/9", 0, 1, 1, 9, 7,  new int[]{ 1, 0, 0, 0, 0, 0, 0},                new int[]{ 0, 1, 0, 0, 0, 0, 0 },               new string[]{ "Previous", "1", "2", "3", "...", "9", "Next" } },
            new object[]{ "9 Page 2/9", 1, 1, 1, 9, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 1, 0, 0, 0, 0, 0 },            new string[]{ "Previous", "1", "2", "3", "4", "...", "9", "Next" } },
            new object[]{ "9 Page 3/9", 2, 1, 1, 9, 9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },         new int[]{ 0, 0, 0, 1, 0, 0, 0, 0, 0 },         new string[]{ "Previous", "1", "2", "3", "4", "5", "...", "9", "Next" } },
            new object[]{ "9 Page 4/9", 3, 1, 1, 9, 10, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },      new int[]{ 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },      new string[]{ "Previous", "1", "2", "3", "4", "5", "6", "...", "9", "Next" } },
            new object[]{ "9 Page 5/9", 4, 1, 1, 9, 11, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   new int[]{ 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },   new string[]{ "Previous", "1", "...", "3", "4", "5", "6", "7", "...", "9", "Next" } },
            new object[]{ "9 Page 6/9", 5, 1, 1, 9, 10, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },      new int[]{ 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },      new string[]{ "Previous", "1", "...",  "4", "5", "6", "7", "8", "9", "Next" } },
            new object[]{ "9 Page 7/9", 6, 1, 1, 9, 9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },         new int[]{ 0, 0, 0, 0, 0, 1, 0, 0, 0 },         new string[]{ "Previous", "1", "...", "5", "6", "7", "8", "9", "Next" } },
            new object[]{ "9 Page 8/9", 7, 1, 1, 9, 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 0, 0, 0, 1, 0, 0 },            new string[]{ "Previous", "1", "...", "6", "7", "8", "9", "Next" } },
            new object[]{ "9 Page 9/9", 8, 1, 1, 9, 7,  new int[]{ 0, 0, 0, 0, 0, 0, 1 },               new int[]{ 0, 0, 0, 0, 0, 1, 0 },               new string[]{ "Previous", "1", "...", "7", "8", "9", "Next" } },
        };

        // title, pageitemcount, disable, active, text
        private readonly object[][] navigationPattern = new object[][] {
            new object[]{ "9 Page 1/9", 7,  new int[]{ 1, 0, 0, 0, 0, 0, 0},                new int[]{ 0, 1, 0, 0, 0, 0, 0 },               new string[]{ "Previous", "1", "2", "3", "...", "9", "Next" } },
            new object[]{ "9 Page 2/9", 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 1, 0, 0, 0, 0, 0 },            new string[]{ "Previous", "1", "2", "3", "4", "...", "9", "Next" } },
            new object[]{ "9 Page 3/9", 9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },         new int[]{ 0, 0, 0, 1, 0, 0, 0, 0, 0 },         new string[]{ "Previous", "1", "2", "3", "4", "5", "...", "9", "Next" } },
            new object[]{ "9 Page 4/9", 10, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },      new int[]{ 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },      new string[]{ "Previous", "1", "2", "3", "4", "5", "6", "...", "9", "Next" } },
            new object[]{ "9 Page 5/9", 11, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },   new int[]{ 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },   new string[]{ "Previous", "1", "...", "3", "4", "5", "6", "7", "...", "9", "Next" } },
            new object[]{ "9 Page 6/9", 10, new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },      new int[]{ 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },      new string[]{ "Previous", "1", "...",  "4", "5", "6", "7", "8", "9", "Next" } },
            new object[]{ "9 Page 7/9", 9,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },         new int[]{ 0, 0, 0, 0, 0, 1, 0, 0, 0 },         new string[]{ "Previous", "1", "...", "5", "6", "7", "8", "9", "Next" } },
            new object[]{ "9 Page 8/9", 8,  new int[]{ 0, 0, 0, 0, 0, 0, 0, 0 },            new int[]{ 0, 0, 0, 0, 0, 1, 0, 0 },            new string[]{ "Previous", "1", "...", "6", "7", "8", "9", "Next" } },
            new object[]{ "9 Page 9/9", 7,  new int[]{ 0, 0, 0, 0, 0, 0, 1 },               new int[]{ 0, 0, 0, 0, 0, 1, 0 },               new string[]{ "Previous", "1", "...", "7", "8", "9", "Next" } },
        };

        [Fact]
        public void MarkupTest()
        {
            Services.AddLocalization(options => options.ResourcesPath = "Resources");

            IRenderedComponent<Pagination> pagination = RenderComponent<Pagination>();

            // Assert
            Assert.Contains("custom-select", pagination.Find("select").ClassList);
            Assert.Contains("pagination", pagination.Find("ul").ClassList);

            var paginationList = pagination.FindAll("li");

            Assert.Equal(3, paginationList.Count);

            foreach (var item in paginationList)
            {
                Assert.Contains("page-item", item.ClassList);
            }
        }

        [Fact]
        public void WithParamMarkupTest()
        {
            Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Function Mock
            Func<Task> DoNothing = () => { return Task.CompletedTask; };

            IRenderedComponent<Pagination> pagination = RenderComponent<Pagination>(parameters =>
            {
                parameters
                .Add(parameter => parameter.Skip, 0)
                .Add(parameter => parameter.Top, 0)
                .Add(parameter => parameter.ViewCount, 0)
                .Add(parameter => parameter.TotalCount, 0)
                .Add(parameter => parameter.GetPage, DoNothing)
                .Add(parameter => parameter.Change, DoNothing);
            });

            // Assert
            Assert.Contains("custom-select", pagination.Find("select").ClassList);
            Assert.Contains("pagination", pagination.Find("ul").ClassList);

            var paginationList = pagination.FindAll("li");

            Assert.Equal(3, paginationList.Count);

            foreach (var item in paginationList)
            {
                Assert.Contains("page-item", item.ClassList);
            }
        }

        [Fact]
        public void PatternMarkupTest()
        {
            Services.AddLocalization(options => options.ResourcesPath = "Resources");

            foreach (var item in pagePattern)
            {
                IRenderedComponent<Pagination> pagination = RenderComponent<Pagination>(parameters =>
                {
                    parameters
                    .Add(parameter => parameter.Skip, (int)item[1])
                    .Add(parameter => parameter.Top, (int)item[2])
                    .Add(parameter => parameter.ViewCount, (int)item[3])
                    .Add(parameter => parameter.TotalCount, (int)item[4]);
                });

                CheckMarkup((int)item[5], (int[])item[6], (int[])item[7], (string[])item[8], pagination);
            }
        }

        [Fact]
        public void SelectTest()
        {
            Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Function Mock
            string changeVal = "";
            void Change(ChangeEventArgs var) { changeVal = var.Value.ToString(); }

            IRenderedComponent<Pagination> pagination = RenderComponent<Pagination>(parameters =>
            {
                parameters
                .Add(parameter => parameter.Skip, 0)
                .Add(parameter => parameter.Top, 1)
                .Add(parameter => parameter.ViewCount, 1)
                .Add(parameter => parameter.TotalCount, 9)
                .Add(parameter => parameter.Change, Change);
            });

            // Select
            pagination.Find("select").Change("10");
            Assert.Equal("10", changeVal);

            pagination.Find("select").Change("50");
            Assert.Equal("50", changeVal);

            pagination.Find("select").Change("100");
            Assert.Equal("100", changeVal);
        }

        [Fact]
        public void NextButtonMarkupTest()
        {
            Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Function Mock
            int getPageVal = 0;
            void GetPage(int var) { getPageVal = var; }

            IRenderedComponent<Pagination> pagination = RenderComponent<Pagination>(parameters =>
            {
                parameters
                .Add(parameter => parameter.Skip, 0)
                .Add(parameter => parameter.Top, 1)
                .Add(parameter => parameter.ViewCount, 1)
                .Add(parameter => parameter.TotalCount, 9)
                .Add(parameter => parameter.GetPage, GetPage);
            });

            // Next Button
            for (var index = 0; index < navigationPattern.Length; index++)
            {
                CheckMarkup((int)navigationPattern[index][1], (int[])navigationPattern[index][2], (int[])navigationPattern[index][3], (string[])navigationPattern[index][4], pagination);

                int nextButtonIndex = (int)navigationPattern[index][1] - 1;
                if (!pagination.FindAll("li")[nextButtonIndex].ClassList.Contains("disabled"))
                {
                    pagination.FindAll("button")[nextButtonIndex].Click();
                    Assert.Equal(index + 2, getPageVal);
                }
            }
        }

        [Fact]
        public void PreviousButtonMarkupTest()
        {
            Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Function Mock
            int getPageVal = 0;
            void GetPage(int var) { getPageVal = var; }

            IRenderedComponent<Pagination> pagination = RenderComponent<Pagination>(parameters =>
            {
                parameters
                .Add(parameter => parameter.Skip, 0)
                .Add(parameter => parameter.Top, 1)
                .Add(parameter => parameter.ViewCount, 1)
                .Add(parameter => parameter.TotalCount, 9)
                .Add(parameter => parameter.GetPage, GetPage);
            });

            // Previous Button
            for (var index = navigationPattern.Length - 1; index < 0; index--)
            {
                CheckMarkup((int)navigationPattern[index][1], (int[])navigationPattern[index][2], (int[])navigationPattern[index][3], (string[])navigationPattern[index][4], pagination);

                int prevButtonIndex = 0;
                if (!pagination.FindAll("li")[prevButtonIndex].ClassList.Contains("disabled"))
                {
                    pagination.FindAll("button")[prevButtonIndex].Click();
                    Assert.Equal(index, getPageVal);
                }
            }
        }

        [Fact]
        public void NavigationButtonMarkupTest()
        {
            Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Function Mock
            int getPageVal = 0;
            void GetPage(int var) { getPageVal = var; }

            // Page 5/9
            IRenderedComponent<Pagination> pagination = RenderComponent<Pagination>(parameters =>
            {
                parameters
                .Add(parameter => parameter.Skip, 4)
                .Add(parameter => parameter.Top, 1)
                .Add(parameter => parameter.ViewCount, 1)
                .Add(parameter => parameter.TotalCount, 9)
                .Add(parameter => parameter.GetPage, GetPage);
            });

            // Click button from left to right (Not Prev and Next)
            for (var i = 0; i < 9; i ++)
            {
                pagination.FindAll("button")[i + 1].Click();
                Assert.Equal(i + 1, getPageVal);
            }
        }

        private static void CheckMarkup(int length, int[] disableArray, int[] activeArray, string[] textArray, IRenderedComponent<Pagination> pagination)
        {
            Assert.Equal(length, pagination.FindAll("li").Count);

            for (var i = 0; i < length; i++)
            {
                Assert.Contains("page-item", pagination.FindAll("li")[i].ClassList);

                if (disableArray[i] == 1)
                {
                    Assert.Contains("disabled", pagination.FindAll("li")[i].ClassList);
                }
                else
                {
                    Assert.DoesNotContain("disabled", pagination.FindAll("li")[i].ClassList);
                }

                if (activeArray[i] == 1)
                {
                    Assert.Contains("active", pagination.FindAll("li")[i].ClassList);
                }
                else
                {
                    Assert.DoesNotContain("active", pagination.FindAll("li")[i].ClassList);
                }

                Assert.Equal(textArray[i].ToString(), pagination.FindAll("button")[i].InnerHtml);
            }
        }
    }
}
