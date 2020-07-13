using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Helpers;
using Moq;
using ObjectsComparer;
using Services.CourseLibraryService;
using Services.OperationCodes;
using Services.ResultObject;
using Xunit;

namespace XLearningApiUnitTest.TestServices
{
    public class CourseLibraryServiceTest
    {
        Mock<ICourseLibraryService> mockCourseLibraryService = new Mock<ICourseLibraryService>();
        Mock<Author> mockAuthor;
        public CourseLibraryServiceTest()
        {
            mockAuthor = new Mock<Author>();
            mockCourseLibraryService.Setup(m => m.AddAuthor(It.IsAny<Author>())).ReturnsAsync(new SuccessOperationResult<Author> { Result = mockAuthor.Object, Code = ConstOperationCodes.AUTHOR_CREATED });

            mockCourseLibraryService.Setup(m => m.GetAuthor(It.IsNotIn(Guid.Empty), false)).ReturnsAsync(new SuccessOperationResult<Author>
            {
                Code = ConstOperationCodes.AUTHOR_FOUND,
                Result = mockAuthor.Object
            });
            mockCourseLibraryService.Setup(m => m.GetAuthor(It.Is<Guid>(g => g.Equals(Guid.Empty)), false)).ReturnsAsync(
                new FailedOperationResult<Author>
                {
                    Code = ConstOperationCodes.AUTHOR_NOT_FOUND,
                }
                );
            mockCourseLibraryService.Setup(m => m.AddAuthor(It.Is<Author>(a => a.FirstName == "exsits"))).ReturnsAsync(
                new FailedOperationResult<Author>
                {
                    Code = ConstOperationCodes.AUTHOR_NAME_ALREADY_EXISTS,
                }
            );
            
        }
        [Fact]
        public async Task ShouldAddAuthor()
        {
           
            var expected = new SuccessOperationResult<Author>
            {
                Result = mockAuthor.Object,
                Code = ConstOperationCodes.AUTHOR_CREATED
            };
            var result = await mockCourseLibraryService.Object.AddAuthor(mockAuthor.Object);
            Assert.Equal(expected, result);
        }
        [Fact]
        public async Task ShouldNotAddAuthor()
        {

            var authorToAdd = new Author
            {
                FirstName = "exsits"
            };
            var expected = new FailedOperationResult<Author>
            {
                Code = ConstOperationCodes.AUTHOR_NAME_ALREADY_EXISTS
            };
            var result = await mockCourseLibraryService.Object.AddAuthor(authorToAdd);
            var comparer = new Comparer();
            Assert.True(comparer.Compare(result,expected));
        }
        [Fact]
        public async Task ShouldGetAuthor()
        {
            //arrange
            var expected = new SuccessOperationResult<Author>
            {
                Code = ConstOperationCodes.AUTHOR_FOUND,
                Result = mockAuthor.Object,
            };
            //act
            var result = await mockCourseLibraryService.Object.GetAuthor(Guid.NewGuid());
            var comparer = new Comparer();
            //assert

            Assert.True(comparer.Compare(result, expected));
        }
        [Fact]
        public async Task ShouldNotGetAuthor()
        {
            var expected = new FailedOperationResult<Author>
            {
                Code = ConstOperationCodes.AUTHOR_NOT_FOUND
            };

            var result = await mockCourseLibraryService.Object.GetAuthor(Guid.Empty);
            var comparer = new Comparer();
            Assert.True(comparer.Compare(result, expected));
        }
    }
}
